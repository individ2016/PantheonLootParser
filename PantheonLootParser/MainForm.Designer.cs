namespace PantheonLootParser
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			lblCharacter = new Label();
			cbCharacterList = new ComboBox();
			lblChatPrefix = new Label();
			txtChatPrefix = new TextBox();
			lblFilters = new Label();
			clbFilters = new CheckedListBox();
			rbSendToChat = new RadioButton();
			rbClipboardCopy = new RadioButton();
			lbResultAction = new Label();
			txtAddSkipText = new TextBox();
			btnAddSkipItem = new Button();
			btnRemoveSkipItem = new Button();
			label1 = new Label();
			txtSearch = new TextBox();
			txtReplace = new TextBox();
			btnRemoveReplacement = new Button();
			btnAddReplacement = new Button();
			label2 = new Label();
			txtAttributeSplitter = new TextBox();
			lvReplacements = new ListView();
			Key = new ColumnHeader();
			Value = new ColumnHeader();
			label3 = new Label();
			txtItemSplitter = new TextBox();
			SuspendLayout();
			// 
			// lblCharacter
			// 
			lblCharacter.AutoSize = true;
			lblCharacter.Location = new Point(10, 13);
			lblCharacter.Name = "lblCharacter";
			lblCharacter.Size = new Size(58, 15);
			lblCharacter.TabIndex = 0;
			lblCharacter.Text = "Character";
			// 
			// cbCharacterList
			// 
			cbCharacterList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			cbCharacterList.FormattingEnabled = true;
			cbCharacterList.Items.AddRange(new object[] { "Select character" });
			cbCharacterList.Location = new Point(96, 10);
			cbCharacterList.Name = "cbCharacterList";
			cbCharacterList.Size = new Size(442, 23);
			cbCharacterList.TabIndex = 1;
			cbCharacterList.SelectedValueChanged += cbCharacterList_SelectedValueChanged;
			// 
			// lblChatPrefix
			// 
			lblChatPrefix.AutoSize = true;
			lblChatPrefix.Location = new Point(10, 42);
			lblChatPrefix.Name = "lblChatPrefix";
			lblChatPrefix.Size = new Size(65, 15);
			lblChatPrefix.TabIndex = 2;
			lblChatPrefix.Text = "Chat Prefix";
			// 
			// txtChatPrefix
			// 
			txtChatPrefix.Location = new Point(96, 39);
			txtChatPrefix.Name = "txtChatPrefix";
			txtChatPrefix.Size = new Size(75, 23);
			txtChatPrefix.TabIndex = 3;
			txtChatPrefix.Text = "/g \\n";
			txtChatPrefix.TextChanged += txtChatPrefix_TextChanged;
			// 
			// lblFilters
			// 
			lblFilters.AutoSize = true;
			lblFilters.Location = new Point(10, 101);
			lblFilters.Name = "lblFilters";
			lblFilters.Size = new Size(61, 15);
			lblFilters.TabIndex = 4;
			lblFilters.Text = "Skip items";
			// 
			// clbFilters
			// 
			clbFilters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			clbFilters.CheckOnClick = true;
			clbFilters.FormattingEnabled = true;
			clbFilters.Location = new Point(10, 119);
			clbFilters.MultiColumn = true;
			clbFilters.Name = "clbFilters";
			clbFilters.Size = new Size(528, 184);
			clbFilters.TabIndex = 5;
			// 
			// rbSendToChat
			// 
			rbSendToChat.AutoSize = true;
			rbSendToChat.Location = new Point(96, 71);
			rbSendToChat.Name = "rbSendToChat";
			rbSendToChat.Size = new Size(91, 19);
			rbSendToChat.TabIndex = 7;
			rbSendToChat.Text = "Send to chat";
			rbSendToChat.UseVisualStyleBackColor = true;
			// 
			// rbClipboardCopy
			// 
			rbClipboardCopy.AutoSize = true;
			rbClipboardCopy.Checked = true;
			rbClipboardCopy.Location = new Point(193, 71);
			rbClipboardCopy.Name = "rbClipboardCopy";
			rbClipboardCopy.Size = new Size(120, 19);
			rbClipboardCopy.TabIndex = 8;
			rbClipboardCopy.TabStop = true;
			rbClipboardCopy.Text = "Copy to clipboard";
			rbClipboardCopy.UseVisualStyleBackColor = true;
			// 
			// lbResultAction
			// 
			lbResultAction.AutoSize = true;
			lbResultAction.Location = new Point(12, 75);
			lbResultAction.Name = "lbResultAction";
			lbResultAction.Size = new Size(75, 15);
			lbResultAction.TabIndex = 9;
			lbResultAction.Text = "Result action";
			// 
			// txtAddSkipText
			// 
			txtAddSkipText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtAddSkipText.Location = new Point(96, 93);
			txtAddSkipText.Name = "txtAddSkipText";
			txtAddSkipText.Size = new Size(385, 23);
			txtAddSkipText.TabIndex = 10;
			// 
			// btnAddSkipItem
			// 
			btnAddSkipItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnAddSkipItem.Image = Properties.Resources.Add_thin_10x_16x;
			btnAddSkipItem.Location = new Point(486, 93);
			btnAddSkipItem.Name = "btnAddSkipItem";
			btnAddSkipItem.Size = new Size(23, 23);
			btnAddSkipItem.TabIndex = 11;
			btnAddSkipItem.UseVisualStyleBackColor = true;
			btnAddSkipItem.Click += btnAddSkipItem_Click;
			// 
			// btnRemoveSkipItem
			// 
			btnRemoveSkipItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnRemoveSkipItem.Image = Properties.Resources.Remove_16xSM;
			btnRemoveSkipItem.Location = new Point(515, 93);
			btnRemoveSkipItem.Name = "btnRemoveSkipItem";
			btnRemoveSkipItem.Size = new Size(23, 23);
			btnRemoveSkipItem.TabIndex = 12;
			btnRemoveSkipItem.UseVisualStyleBackColor = true;
			btnRemoveSkipItem.Click += btnRemoveSkipItem_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(10, 316);
			label1.Name = "label1";
			label1.Size = new Size(71, 15);
			label1.TabIndex = 13;
			label1.Text = "Replace text";
			// 
			// txtSearch
			// 
			txtSearch.Location = new Point(96, 308);
			txtSearch.Name = "txtSearch";
			txtSearch.Size = new Size(190, 23);
			txtSearch.TabIndex = 14;
			// 
			// txtReplace
			// 
			txtReplace.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtReplace.Location = new Point(293, 308);
			txtReplace.Name = "txtReplace";
			txtReplace.Size = new Size(188, 23);
			txtReplace.TabIndex = 15;
			// 
			// btnRemoveReplacement
			// 
			btnRemoveReplacement.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnRemoveReplacement.Image = Properties.Resources.Remove_16xSM;
			btnRemoveReplacement.Location = new Point(515, 308);
			btnRemoveReplacement.Name = "btnRemoveReplacement";
			btnRemoveReplacement.Size = new Size(23, 23);
			btnRemoveReplacement.TabIndex = 17;
			btnRemoveReplacement.UseVisualStyleBackColor = true;
			btnRemoveReplacement.Click += btnRemoveReplacement_Click;
			// 
			// btnAddReplacement
			// 
			btnAddReplacement.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnAddReplacement.Image = Properties.Resources.Add_thin_10x_16x;
			btnAddReplacement.Location = new Point(486, 308);
			btnAddReplacement.Name = "btnAddReplacement";
			btnAddReplacement.Size = new Size(23, 23);
			btnAddReplacement.TabIndex = 16;
			btnAddReplacement.UseVisualStyleBackColor = true;
			btnAddReplacement.Click += btnAddReplacement_Click;
			// 
			// label2
			// 
			label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			label2.AutoSize = true;
			label2.Location = new Point(365, 42);
			label2.Name = "label2";
			label2.Size = new Size(93, 15);
			label2.TabIndex = 19;
			label2.Text = "Attribute splitter";
			// 
			// txtAttributeSplitter
			// 
			txtAttributeSplitter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			txtAttributeSplitter.Location = new Point(464, 39);
			txtAttributeSplitter.Name = "txtAttributeSplitter";
			txtAttributeSplitter.Size = new Size(75, 23);
			txtAttributeSplitter.TabIndex = 20;
			txtAttributeSplitter.Text = "\\n";
			txtAttributeSplitter.TextChanged += txtAttributeSplitter_TextChanged;
			// 
			// lvReplacements
			// 
			lvReplacements.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			lvReplacements.Columns.AddRange(new ColumnHeader[] { Key, Value });
			lvReplacements.FullRowSelect = true;
			lvReplacements.Location = new Point(10, 334);
			lvReplacements.MultiSelect = false;
			lvReplacements.Name = "lvReplacements";
			lvReplacements.Size = new Size(527, 215);
			lvReplacements.TabIndex = 21;
			lvReplacements.UseCompatibleStateImageBehavior = false;
			lvReplacements.View = View.Details;
			// 
			// Key
			// 
			Key.Text = "SearchText";
			Key.Width = 120;
			// 
			// Value
			// 
			Value.Text = "ReplaceText";
			Value.Width = 120;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(177, 42);
			label3.Name = "label3";
			label3.Size = new Size(70, 15);
			label3.TabIndex = 22;
			label3.Text = "Item splitter";
			// 
			// txtItemSplitter
			// 
			txtItemSplitter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtItemSplitter.Location = new Point(253, 39);
			txtItemSplitter.Name = "txtItemSplitter";
			txtItemSplitter.Size = new Size(106, 23);
			txtItemSplitter.TabIndex = 23;
			txtItemSplitter.Text = "\\n";
			txtItemSplitter.TextChanged += txtItemSplitter_TextChanged;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(549, 561);
			Controls.Add(txtItemSplitter);
			Controls.Add(label3);
			Controls.Add(lvReplacements);
			Controls.Add(txtAttributeSplitter);
			Controls.Add(label2);
			Controls.Add(btnRemoveReplacement);
			Controls.Add(btnAddReplacement);
			Controls.Add(txtReplace);
			Controls.Add(txtSearch);
			Controls.Add(label1);
			Controls.Add(btnRemoveSkipItem);
			Controls.Add(btnAddSkipItem);
			Controls.Add(txtAddSkipText);
			Controls.Add(lbResultAction);
			Controls.Add(rbClipboardCopy);
			Controls.Add(rbSendToChat);
			Controls.Add(clbFilters);
			Controls.Add(lblFilters);
			Controls.Add(txtChatPrefix);
			Controls.Add(lblChatPrefix);
			Controls.Add(cbCharacterList);
			Controls.Add(lblCharacter);
			MinimumSize = new Size(565, 600);
			Name = "MainForm";
			Text = "Pantheon loot recognizer";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label lblCharacter;
		private ComboBox cbCharacterList;
		private Label lblChatPrefix;
		private TextBox txtChatPrefix;
		private Label lblFilters;
		private CheckedListBox clbFilters;
		private RadioButton rbSendToChat;
		private RadioButton rbClipboardCopy;
		private Label lbResultAction;
		private TextBox txtAddSkipText;
		private Button btnAddSkipItem;
		private Button btnRemoveSkipItem;
		private Label label1;
		private TextBox txtSearch;
		private TextBox txtReplace;
		private Button btnRemoveReplacement;
		private Button btnAddReplacement;
		private ListBox lbReplacements;
		private Label label2;
		private TextBox txtAttributeSplitter;
		private ListView lvReplacements;
		private ColumnHeader Key;
		private ColumnHeader Value;
		private Label label3;
		private TextBox txtItemSplitter;
	}
}
