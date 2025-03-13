using Emgu.CV;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PantheonLootParser
{
	public partial class MainForm : Form
	{
		private String settingsPath = Environment.ExpandEnvironmentVariables(@"%AppData%\..\LocalLow\Visionary Realms\Pantheon\Settings\CharacterSettings");
		private ChatWindowSettings chatWindowSettings;
		private Utils utils = new Utils();
		private ShalazamParser parser = new ShalazamParser();

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
				MessageBox.Show("Ќе удалось зарегистрировать гор€чую клавишу");

			for(Int32 index = 0; index < clbFilters.Items.Count; index++)
				clbFilters.SetItemChecked(index, true);

			FillCharacters();
		}

		private void FillCharacters()
		{
			foreach(var characterName in Directory.EnumerateDirectories(settingsPath))
			{
				cbCharacterList.Items.Add(Path.GetFileName(characterName));
			}
			cbCharacterList.SelectedIndex = cbCharacterList.Items.Count - 1;
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg == 0x0312) // WM_HOTKEY
			{
				if(m.WParam.ToInt32() == hotkeyId)
				{
					if (cbCharacterList.SelectedIndex == 0)
					{
						MessageBox.Show("”кажите персонажа. Ёто нужно дл€ получени€ настроек месторасположени€ окна чата.", "ѕредупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}

					Mat mat = utils.GetPantheonScreen();
					if (mat != null)
						RecognizeLoot(mat);
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
				if (item != null && !shalazamItems.Any(p => p.ID == item.ID))
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
					itemsDescription += "\\n|---===---|\\n";

				itemsDescription += lootItemDescription;
			}
			if(!String.IsNullOrWhiteSpace(itemsDescription))
			{
				if(rbSendToChat.Checked)
					SendTextToChat(itemsDescription);
				else if(rbClipboardCopy.Checked)
					Clipboard.SetText(txtChatPrefix.Text + itemsDescription);
			}
			SystemSounds.Hand.Play();
		}

		private void SendTextToChat(String text)
		{
			SendKeys.SendWait("~");
			Clipboard.SetText(txtChatPrefix.Text + text);
			SendKeys.SendWait("^{v}");
			SendKeys.SendWait("~");
		}

		private async void cbCharacterList_SelectedValueChanged(object sender, EventArgs e)
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

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{

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

		private async void button2_Click(object sender, EventArgs e)
		{
			String lootItemDescription = await parser.GetItem(new DataItem { ID = 630, Name = "A fresh-baked cookie" });
		}
	}
}
