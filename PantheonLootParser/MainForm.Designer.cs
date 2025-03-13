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
			button1 = new Button();
			rbSendToChat = new RadioButton();
			rbClipboardCopy = new RadioButton();
			lbResultAction = new Label();
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
			cbCharacterList.Location = new Point(97, 10);
			cbCharacterList.Name = "cbCharacterList";
			cbCharacterList.Size = new Size(216, 23);
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
			txtChatPrefix.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			txtChatPrefix.Location = new Point(97, 39);
			txtChatPrefix.Name = "txtChatPrefix";
			txtChatPrefix.Size = new Size(216, 23);
			txtChatPrefix.TabIndex = 3;
			txtChatPrefix.Text = "/g \\n";
			// 
			// lblFilters
			// 
			lblFilters.AutoSize = true;
			lblFilters.Location = new Point(10, 105);
			lblFilters.Name = "lblFilters";
			lblFilters.Size = new Size(99, 15);
			lblFilters.TabIndex = 4;
			lblFilters.Text = "Don't show items";
			// 
			// clbFilters
			// 
			clbFilters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			clbFilters.CheckOnClick = true;
			clbFilters.FormattingEnabled = true;
			clbFilters.Items.AddRange(new object[] { "General", "Resource", "Reagent", "Component", "Food", "Ingredient", "Material", "Clickie" });
			clbFilters.Location = new Point(10, 123);
			clbFilters.MultiColumn = true;
			clbFilters.Name = "clbFilters";
			clbFilters.Size = new Size(303, 130);
			clbFilters.TabIndex = 5;
			// 
			// button1
			// 
			button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			button1.Location = new Point(203, 94);
			button1.Name = "button1";
			button1.Size = new Size(109, 23);
			button1.TabIndex = 6;
			button1.Text = "Test chat image";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// rbSendToChat
			// 
			rbSendToChat.AutoSize = true;
			rbSendToChat.Location = new Point(97, 71);
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
			rbClipboardCopy.Location = new Point(194, 71);
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
			lbResultAction.Location = new Point(12, 73);
			lbResultAction.Name = "lbResultAction";
			lbResultAction.Size = new Size(75, 15);
			lbResultAction.TabIndex = 9;
			lbResultAction.Text = "Result action";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(324, 261);
			Controls.Add(lbResultAction);
			Controls.Add(rbClipboardCopy);
			Controls.Add(rbSendToChat);
			Controls.Add(button1);
			Controls.Add(clbFilters);
			Controls.Add(lblFilters);
			Controls.Add(txtChatPrefix);
			Controls.Add(lblChatPrefix);
			Controls.Add(cbCharacterList);
			Controls.Add(lblCharacter);
			MinimumSize = new Size(340, 300);
			Name = "MainForm";
			Text = "Pantheon loot recognizer";
			FormClosing += MainForm_FormClosing;
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
		private Button button1;
		private RadioButton rbSendToChat;
		private RadioButton rbClipboardCopy;
		private Label lbResultAction;
	}
}
