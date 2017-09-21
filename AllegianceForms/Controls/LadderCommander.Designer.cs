namespace AllegianceForms.Controls
{
    partial class LadderCommander
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
            this.RankNumber = new System.Windows.Forms.Label();
            this.CommanderName = new System.Windows.Forms.Label();
            this.Avatar = new System.Windows.Forms.PictureBox();
            this.GamesPlayed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Avatar)).BeginInit();
            this.SuspendLayout();
            // 
            // RankNumber
            // 
            this.RankNumber.AutoSize = true;
            this.RankNumber.Location = new System.Drawing.Point(3, 22);
            this.RankNumber.Name = "RankNumber";
            this.RankNumber.Size = new System.Drawing.Size(28, 13);
            this.RankNumber.TabIndex = 49;
            this.RankNumber.Text = "###";
            // 
            // CommanderName
            // 
            this.CommanderName.AutoSize = true;
            this.CommanderName.Location = new System.Drawing.Point(93, 22);
            this.CommanderName.Name = "CommanderName";
            this.CommanderName.Size = new System.Drawing.Size(100, 13);
            this.CommanderName.TabIndex = 49;
            this.CommanderName.Text = "[Commander Name]";
            // 
            // Avatar
            // 
            this.Avatar.Location = new System.Drawing.Point(37, 3);
            this.Avatar.Name = "Avatar";
            this.Avatar.Size = new System.Drawing.Size(50, 50);
            this.Avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Avatar.TabIndex = 50;
            this.Avatar.TabStop = false;
            // 
            // GamesPlayed
            // 
            this.GamesPlayed.AutoSize = true;
            this.GamesPlayed.Location = new System.Drawing.Point(276, 22);
            this.GamesPlayed.Name = "GamesPlayed";
            this.GamesPlayed.Size = new System.Drawing.Size(90, 13);
            this.GamesPlayed.TabIndex = 49;
            this.GamesPlayed.Text = "### (### / ###)";
            // 
            // LadderCommander
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.Avatar);
            this.Controls.Add(this.CommanderName);
            this.Controls.Add(this.GamesPlayed);
            this.Controls.Add(this.RankNumber);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "LadderCommander";
            this.Size = new System.Drawing.Size(382, 55);
            ((System.ComponentModel.ISupportInitialize)(this.Avatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RankNumber;
        private System.Windows.Forms.Label CommanderName;
        private System.Windows.Forms.PictureBox Avatar;
        private System.Windows.Forms.Label GamesPlayed;
    }
}
