namespace AllegianceForms.Controls
{
    partial class QuickChatItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickChatItem));
            this.CommandText = new System.Windows.Forms.Label();
            this.Key = new System.Windows.Forms.Label();
            this.OpenMenu = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.OpenMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // CommandText
            // 
            this.CommandText.Location = new System.Drawing.Point(3, 3);
            this.CommandText.Name = "CommandText";
            this.CommandText.Size = new System.Drawing.Size(254, 15);
            this.CommandText.TabIndex = 1;
            this.CommandText.Text = "[Quick Chat Text]";
            this.CommandText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Key
            // 
            this.Key.Location = new System.Drawing.Point(255, 3);
            this.Key.Name = "Key";
            this.Key.Size = new System.Drawing.Size(33, 15);
            this.Key.TabIndex = 1;
            this.Key.Text = "[Key]";
            this.Key.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OpenMenu
            // 
            this.OpenMenu.Image = ((System.Drawing.Image)(resources.GetObject("OpenMenu.Image")));
            this.OpenMenu.Location = new System.Drawing.Point(290, 2);
            this.OpenMenu.Name = "OpenMenu";
            this.OpenMenu.Size = new System.Drawing.Size(16, 16);
            this.OpenMenu.TabIndex = 2;
            this.OpenMenu.TabStop = false;
            // 
            // QuickChatItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.OpenMenu);
            this.Controls.Add(this.Key);
            this.Controls.Add(this.CommandText);
            this.ForeColor = System.Drawing.Color.Silver;
            this.Name = "QuickChatItem";
            this.Size = new System.Drawing.Size(308, 20);
            ((System.ComponentModel.ISupportInitialize)(this.OpenMenu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label CommandText;
        private System.Windows.Forms.Label Key;
        private System.Windows.Forms.PictureBox OpenMenu;
    }
}
