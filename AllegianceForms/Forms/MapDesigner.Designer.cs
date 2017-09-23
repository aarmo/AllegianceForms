namespace AllegianceForms.Forms
{
    partial class MapDesigner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapDesigner));
            this.MapPanel = new System.Windows.Forms.Panel();
            this.SectorLabel = new System.Windows.Forms.Label();
            this.Open = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.MapName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Clear = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Preview = new System.Windows.Forms.Button();
            this.MapPicture = new System.Windows.Forms.PictureBox();
            this.SavePreviewForAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MapPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // MapPanel
            // 
            this.MapPanel.BackColor = System.Drawing.Color.Transparent;
            this.MapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapPanel.Location = new System.Drawing.Point(80, 28);
            this.MapPanel.Name = "MapPanel";
            this.MapPanel.Size = new System.Drawing.Size(200, 200);
            this.MapPanel.TabIndex = 0;
            this.MapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MapPanel_Paint);
            this.MapPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseClick);
            // 
            // SectorLabel
            // 
            this.SectorLabel.BackColor = System.Drawing.Color.Lime;
            this.SectorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SectorLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SectorLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SectorLabel.ForeColor = System.Drawing.Color.Black;
            this.SectorLabel.Location = new System.Drawing.Point(2, 164);
            this.SectorLabel.Name = "SectorLabel";
            this.SectorLabel.Size = new System.Drawing.Size(22, 24);
            this.SectorLabel.TabIndex = 0;
            this.SectorLabel.Text = "O";
            this.SectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SectorLabel.Visible = false;
            this.SectorLabel.Click += new System.EventHandler(this.SectorLabel_Click);
            // 
            // Open
            // 
            this.Open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Open.Location = new System.Drawing.Point(2, 2);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(72, 39);
            this.Open.TabIndex = 1;
            this.Open.Text = "Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.Open_Click);
            this.Open.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyDown);
            this.Open.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Open.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Save
            // 
            this.Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Save.Location = new System.Drawing.Point(2, 47);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(72, 39);
            this.Save.TabIndex = 2;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            this.Save.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyDown);
            this.Save.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Save.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MapName
            // 
            this.MapName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapName.Location = new System.Drawing.Point(145, 2);
            this.MapName.Name = "MapName";
            this.MapName.Size = new System.Drawing.Size(135, 20);
            this.MapName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(77, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Map Name:";
            // 
            // Clear
            // 
            this.Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Clear.Location = new System.Drawing.Point(2, 92);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(72, 27);
            this.Clear.TabIndex = 2;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            this.Clear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyDown);
            this.Clear.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Clear.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Map files|*.map|All files|*.*";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Map files|*.map|All files|*.*";
            // 
            // Preview
            // 
            this.Preview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Preview.Location = new System.Drawing.Point(286, 2);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(72, 39);
            this.Preview.TabIndex = 2;
            this.Preview.Text = "Preview";
            this.Preview.UseVisualStyleBackColor = true;
            this.Preview.Click += new System.EventHandler(this.Preview_Click);
            this.Preview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyDown);
            this.Preview.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Preview.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MapPicture
            // 
            this.MapPicture.Location = new System.Drawing.Point(286, 4);
            this.MapPicture.Name = "MapPicture";
            this.MapPicture.Size = new System.Drawing.Size(230, 225);
            this.MapPicture.TabIndex = 4;
            this.MapPicture.TabStop = false;
            this.MapPicture.Visible = false;
            this.MapPicture.Click += new System.EventHandler(this.MapPicture_Click);
            // 
            // SavePreviewForAll
            // 
            this.SavePreviewForAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SavePreviewForAll.Location = new System.Drawing.Point(2, 202);
            this.SavePreviewForAll.Name = "SavePreviewForAll";
            this.SavePreviewForAll.Size = new System.Drawing.Size(72, 27);
            this.SavePreviewForAll.TabIndex = 2;
            this.SavePreviewForAll.Text = "Export Pics";
            this.SavePreviewForAll.UseVisualStyleBackColor = true;
            this.SavePreviewForAll.Click += new System.EventHandler(this.SavePreviewForAll_Click);
            this.SavePreviewForAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyDown);
            this.SavePreviewForAll.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.SavePreviewForAll.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // MapDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(520, 233);
            this.Controls.Add(this.Preview);
            this.Controls.Add(this.MapPicture);
            this.Controls.Add(this.SectorLabel);
            this.Controls.Add(this.MapPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MapName);
            this.Controls.Add(this.SavePreviewForAll);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Open);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MapDesigner";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Map Designer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MapDesigner_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.MapPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel MapPanel;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.TextBox MapName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SectorLabel;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button Preview;
        private System.Windows.Forms.PictureBox MapPicture;
        private System.Windows.Forms.Button SavePreviewForAll;
    }
}