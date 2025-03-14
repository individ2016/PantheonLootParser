using Emgu.CV;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Linq;
using System.Text.Json.Serialization;

namespace PantheonLootParser
{
	public partial class MainForm : Form
	{
		private DAL _DAL = new DAL();
		private String settingsPath = Environment.ExpandEnvironmentVariables(@"%AppData%\..\LocalLow\Visionary Realms\Pantheon\Settings\CharacterSettings");
		private ChatWindowSettings chatWindowSettings;
		private Utils utils = new Utils();
		private ShalazamParser parser;

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		const uint MOD_ALT = 0x0001;
		const uint MOD_CONTROL = 0x0002;
		const uint MOD_SHIFT = 0x0004;
		const uint MOD_WIN = 0x0008;
		private int hotkeyId = 9000; // уникальный идентификатор гор€чей клавиши

		public MainForm()
		{
			InitializeComponent();

			// –егистраци€ гор€чей клавиши Ctrl+Shift+S
			if(!RegisterHotKey(this.Handle, hotkeyId, MOD_CONTROL | MOD_SHIFT, (uint)Keys.V))
				MessageBox.Show("Cant register hot-key. Try restart.");

			_DAL.EnsureDBCreated();
			parser = new ShalazamParser(_DAL);

			FillCharacters();
			txtChatPrefix.Text = _DAL.GetSettings(UserSettingsEnum.ChatPrefix);
			txtAttributeSplitter.Text = _DAL.GetSettings(UserSettingsEnum.AttributeSplitter);
			txtItemSplitter.Text = _DAL.GetSettings(UserSettingsEnum.ItemSplitter);
			String? resultAction = _DAL.GetSettings(UserSettingsEnum.ResultAction);
			if(resultAction == "0")
				rbSendToChat.Checked = true;
			else
				rbClipboardCopy.Checked = true;
			FillSkipItems();
			FillReplacements();

			clbFilters.ItemCheck += clbFilters_ItemCheck;
			rbClipboardCopy.CheckedChanged += rbClipboardCopy_CheckedChanged;
			cbCharacterList.SelectedIndexChanged += cbCharacterList_SelectedIndexChanged;
		}

		private async void cbCharacterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(cbCharacterList.SelectedIndex > -1)
				await _DAL.UpdateSettings(UserSettingsEnum.LastCharacter, (String)cbCharacterList.Items[cbCharacterList.SelectedIndex]);
		}

		private void rbClipboardCopy_CheckedChanged(object sender, EventArgs e)
		{
			if(rbSendToChat.Checked)
				_DAL.UpdateSettings(UserSettingsEnum.ResultAction, "0");
			else
				_DAL.UpdateSettings(UserSettingsEnum.ResultAction, "1");
		}

		private async void FillSkipItems()
		{
			await foreach(var item in _DAL.GetSkipItems())
				clbFilters.Items.Add(item.Key, item.Value);
		}

		private async void FillReplacements()
		{
			await foreach(var item in _DAL.GetReplacements())
			{
				ListViewItem lvItem = new ListViewItem();
				lvItem.Text = item.Key;
				lvItem.SubItems.Add(item.Value);
				lvReplacements.Items.Add(lvItem);
			}
		}

		private void FillCharacters()
		{
			String? lastChar = _DAL.GetSettings(UserSettingsEnum.LastCharacter);
			Int32 lastIndex = 0;
			Int32 selIndex = -1;
			foreach(var characterDirName in Directory.EnumerateDirectories(settingsPath))
			{
				var characterName = Path.GetFileName(characterDirName);
				cbCharacterList.Items.Add(characterName);
				lastIndex++;
				if(lastChar == characterName)
					selIndex = lastIndex;
			}

			if(selIndex != -1)
				cbCharacterList.SelectedIndex = selIndex;
			else
				cbCharacterList.SelectedIndex = lastIndex;
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == 0x0312) // WM_HOTKEY
			{
				if(m.WParam.ToInt32() == hotkeyId)
				{
					if(cbCharacterList.SelectedIndex == 0)
					{
						MessageBox.Show("”кажите персонажа. Ёто нужно дл€ получени€ настроек месторасположени€ окна чата.", "ѕредупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}

					Mat mat = utils.GetPantheonScreen();
					if(mat != null)
						RecognizeLoot(mat);
					else
						SystemSounds.Exclamation.Play();
				}
			}

			base.WndProc(ref m);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			// ќтмена регистрации гор€чей клавиши при закрытии формы
			UnregisterHotKey(this.Handle, hotkeyId);
			base.OnFormClosing(e);
		}

		private async void RecognizeLoot(Mat mat)
		{
			String str = utils.RecognizeStrings(mat);
			String[] lootItems = TextUtils.FindByLootPatterns(str);

			List<DataItem> shalazamItems = new List<DataItem>();
			foreach(String lootItem in lootItems)
			{
				DataItem item = await parser.GetItem(lootItem);
				if(item != null && !shalazamItems.Any(p => p.ID == item.ID))
					shalazamItems.Add(item);
			}

			String itemsDescription = String.Empty;
			foreach(DataItem shalazamItem in shalazamItems)
			{
				String lootItemDescription = await parser.GetItem(shalazamItem);

				Boolean shouldBeFiltered = false;
				foreach(String filterItem in clbFilters.CheckedItems)
					if(lootItemDescription.Contains(filterItem))
					{
						shouldBeFiltered = true;
						break;
					}
				if(shouldBeFiltered)
					continue;

				if(!String.IsNullOrWhiteSpace(itemsDescription))
					itemsDescription += txtItemSplitter.Text;

				itemsDescription += lootItemDescription;
			}

			if(!String.IsNullOrWhiteSpace(itemsDescription))
			{
				Dictionary<String, String> replacements = new Dictionary<String, String>();
				foreach(ListViewItem item in lvReplacements.Items)
					replacements.Add(item.Name, item.SubItems[0].Text);
				itemsDescription = TextUtils.MakeReplacements(itemsDescription, replacements);

				if(rbSendToChat.Checked)
					SendTextToChat(itemsDescription);
				else if(rbClipboardCopy.Checked)
					Clipboard.SetText(txtChatPrefix.Text + itemsDescription);
				SystemSounds.Hand.Play();
			} else
			{
				Clipboard.SetText("");
				SystemSounds.Exclamation.Play();
			}
		}

		private void SendTextToChat(String text)
		{
			SendKeys.SendWait("~");
			Clipboard.SetText(txtChatPrefix.Text + text);
			SendKeys.SendWait("^{v}");
			SendKeys.SendWait("~");
		}

		private void cbCharacterList_SelectedValueChanged(object sender, EventArgs e)
		{
			if(cbCharacterList.SelectedIndex == 0)
				return;

			using(var stream = new FileStream(Path.Combine(settingsPath, (String)cbCharacterList.Items[cbCharacterList.SelectedIndex], "CharacterSettings.json"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using(var reader = new StreamReader(stream))
			{
				string json = reader.ReadToEnd();
				JsonDocument jsonDocument = JsonDocument.Parse(json);
				utils.ChatWindowSettings = jsonDocument.RootElement.GetProperty("WindowLocations").GetProperty("Panel_Chat").Deserialize<ChatWindowSettings>();
				utils.ChatWindowSettings.CustomScale = jsonDocument.RootElement.GetProperty("WindowSettingValues").GetProperty("Panel_Chat").GetProperty("CustomScale").GetDouble() / 100;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.Filter = "Image files|*.png;*.jpg;*.bmp";
			if(fileDialog.ShowDialog() == DialogResult.OK)
			{
				rbClipboardCopy.Checked = true;
				Mat mat = new Mat(fileDialog.FileName);
				RecognizeLoot(mat);
			}
		}

		private async void btnAddSkipItem_Click(object sender, EventArgs e)
		{
			if(String.IsNullOrWhiteSpace(txtAddSkipText.Text))
			{
				MessageBox.Show("Type text to skip");
				return;
			}

			await _DAL.AddSkipItem(txtAddSkipText.Text);
			clbFilters.Items.Add(txtAddSkipText.Text, true);
			txtAddSkipText.Text = String.Empty;
		}

		private async void clbFilters_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			await _DAL.UpdateSkipItem((String)clbFilters.Items[e.Index], e.NewValue == CheckState.Checked);
		}

		private async void btnRemoveSkipItem_Click(object sender, EventArgs e)
		{
			if(clbFilters.SelectedIndex == -1)
			{
				MessageBox.Show("Select item to delete");
				return;
			}

			await _DAL.RemoveSkipItem((String)clbFilters.Items[clbFilters.SelectedIndex]);
			clbFilters.Items.RemoveAt(clbFilters.SelectedIndex);
		}

		private async void btnAddReplacement_Click(object sender, EventArgs e)
		{
			if(String.IsNullOrWhiteSpace(txtSearch.Text))
			{
				MessageBox.Show("Type text to search in description");
				return;
			}

			if(String.IsNullOrWhiteSpace(txtReplace.Text))
			{
				MessageBox.Show("Type text to replace when search text found in description");
				return;
			}

			await _DAL.AddReplacement(txtSearch.Text, txtReplace.Text);
			ListViewItem lvItem = new ListViewItem();
			lvItem.Text = txtSearch.Text;
			lvItem.SubItems.Add(txtReplace.Text);
			lvReplacements.Items.Add(lvItem);
			txtSearch.Text = txtReplace.Text = String.Empty;
		}

		private async void btnRemoveReplacement_Click(object sender, EventArgs e)
		{
			if(lvReplacements.SelectedItems.Count == 0)
			{
				MessageBox.Show("Select item to delete");
				return;
			}
			await _DAL.RemoveReplacement(lvReplacements.SelectedItems[0].Text);
			lvReplacements.Items.RemoveAt(lvReplacements.SelectedIndices[0]);
		}

		private async void txtChatPrefix_TextChanged(object sender, EventArgs e)
		{
			await _DAL.UpdateSettings(UserSettingsEnum.ChatPrefix, txtChatPrefix.Text);
		}

		private async void txtItemSplitter_TextChanged(object sender, EventArgs e)
		{
			await _DAL.UpdateSettings(UserSettingsEnum.ItemSplitter, txtItemSplitter.Text);
		}

		private async void txtAttributeSplitter_TextChanged(object sender, EventArgs e)
		{
			await _DAL.UpdateSettings(UserSettingsEnum.AttributeSplitter, txtAttributeSplitter.Text);
		}
	}
}
