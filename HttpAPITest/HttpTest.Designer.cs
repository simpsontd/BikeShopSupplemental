namespace HttpAPITest
{
    partial class HttpTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.httpComboBox = new System.Windows.Forms.ComboBox();
            this.requestBodyTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.keysListBox = new System.Windows.Forms.ListBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.requestBodyLabel = new System.Windows.Forms.Label();
            this.dictionaryKeysLabel = new System.Windows.Forms.Label();
            this.urlLabel = new System.Windows.Forms.Label();
            this.requestTypeLabel = new System.Windows.Forms.Label();
            this.valueLabel = new System.Windows.Forms.Label();
            this.responseBodyTextBox = new System.Windows.Forms.TextBox();
            this.responseBodyLabel = new System.Windows.Forms.Label();
            this.SampleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(223, 75);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(367, 20);
            this.urlTextBox.TabIndex = 0;
            this.urlTextBox.Text = "https://segfault.asuscomm.com:9300/api/Metrics/Customer/1";
            // 
            // httpComboBox
            // 
            this.httpComboBox.FormattingEnabled = true;
            this.httpComboBox.Location = new System.Drawing.Point(66, 74);
            this.httpComboBox.Name = "httpComboBox";
            this.httpComboBox.Size = new System.Drawing.Size(121, 21);
            this.httpComboBox.TabIndex = 1;
            // 
            // requestBodyTextBox
            // 
            this.requestBodyTextBox.Location = new System.Drawing.Point(66, 152);
            this.requestBodyTextBox.Multiline = true;
            this.requestBodyTextBox.Name = "requestBodyTextBox";
            this.requestBodyTextBox.Size = new System.Drawing.Size(367, 149);
            this.requestBodyTextBox.TabIndex = 2;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(626, 74);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(113, 23);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // keysListBox
            // 
            this.keysListBox.FormattingEnabled = true;
            this.keysListBox.Location = new System.Drawing.Point(507, 152);
            this.keysListBox.Name = "keysListBox";
            this.keysListBox.Size = new System.Drawing.Size(120, 290);
            this.keysListBox.TabIndex = 4;
            this.keysListBox.SelectedIndexChanged += new System.EventHandler(this.keysListBox_SelectedIndexChanged);
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(660, 281);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(164, 20);
            this.valueTextBox.TabIndex = 5;
            // 
            // requestBodyLabel
            // 
            this.requestBodyLabel.AutoSize = true;
            this.requestBodyLabel.Location = new System.Drawing.Point(66, 133);
            this.requestBodyLabel.Name = "requestBodyLabel";
            this.requestBodyLabel.Size = new System.Drawing.Size(74, 13);
            this.requestBodyLabel.TabIndex = 6;
            this.requestBodyLabel.Text = "Request Body";
            // 
            // dictionaryKeysLabel
            // 
            this.dictionaryKeysLabel.AutoSize = true;
            this.dictionaryKeysLabel.Location = new System.Drawing.Point(507, 133);
            this.dictionaryKeysLabel.Name = "dictionaryKeysLabel";
            this.dictionaryKeysLabel.Size = new System.Drawing.Size(80, 13);
            this.dictionaryKeysLabel.TabIndex = 7;
            this.dictionaryKeysLabel.Text = "Dictionary Keys";
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(223, 56);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(29, 13);
            this.urlLabel.TabIndex = 8;
            this.urlLabel.Text = "URL";
            // 
            // requestTypeLabel
            // 
            this.requestTypeLabel.AutoSize = true;
            this.requestTypeLabel.Location = new System.Drawing.Point(66, 55);
            this.requestTypeLabel.Name = "requestTypeLabel";
            this.requestTypeLabel.Size = new System.Drawing.Size(31, 13);
            this.requestTypeLabel.TabIndex = 9;
            this.requestTypeLabel.Text = "Type";
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(722, 262);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(34, 13);
            this.valueLabel.TabIndex = 10;
            this.valueLabel.Text = "Value";
            // 
            // responseBodyTextBox
            // 
            this.responseBodyTextBox.Location = new System.Drawing.Point(66, 337);
            this.responseBodyTextBox.Multiline = true;
            this.responseBodyTextBox.Name = "responseBodyTextBox";
            this.responseBodyTextBox.Size = new System.Drawing.Size(367, 149);
            this.responseBodyTextBox.TabIndex = 11;
            // 
            // responseBodyLabel
            // 
            this.responseBodyLabel.AutoSize = true;
            this.responseBodyLabel.Location = new System.Drawing.Point(66, 321);
            this.responseBodyLabel.Name = "responseBodyLabel";
            this.responseBodyLabel.Size = new System.Drawing.Size(82, 13);
            this.responseBodyLabel.TabIndex = 12;
            this.responseBodyLabel.Text = "Response Body";
            // 
            // SampleButton
            // 
            this.SampleButton.Location = new System.Drawing.Point(679, 379);
            this.SampleButton.Name = "SampleButton";
            this.SampleButton.Size = new System.Drawing.Size(117, 46);
            this.SampleButton.TabIndex = 13;
            this.SampleButton.Text = "POST Sample Customer";
            this.SampleButton.UseVisualStyleBackColor = true;
            this.SampleButton.Click += new System.EventHandler(this.SampleButton_Click);
            // 
            // HttpTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 520);
            this.Controls.Add(this.SampleButton);
            this.Controls.Add(this.responseBodyLabel);
            this.Controls.Add(this.responseBodyTextBox);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.requestTypeLabel);
            this.Controls.Add(this.urlLabel);
            this.Controls.Add(this.dictionaryKeysLabel);
            this.Controls.Add(this.requestBodyLabel);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.keysListBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.requestBodyTextBox);
            this.Controls.Add(this.httpComboBox);
            this.Controls.Add(this.urlTextBox);
            this.Name = "HttpTest";
            this.Text = "HttpTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.ComboBox httpComboBox;
        private System.Windows.Forms.TextBox requestBodyTextBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.ListBox keysListBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.Label requestBodyLabel;
        private System.Windows.Forms.Label dictionaryKeysLabel;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label requestTypeLabel;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.TextBox responseBodyTextBox;
        private System.Windows.Forms.Label responseBodyLabel;
        private System.Windows.Forms.Button SampleButton;
    }
}

