namespace AllegianceForms.Forms
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.Dogfight = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.AppVersion = new System.Windows.Forms.Label();
            this.CustomGame = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MapDesigner = new System.Windows.Forms.Button();
            this.QuickPlay = new System.Windows.Forms.Button();
            this.PlayCampaign = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Dogfight
            // 
            this.Dogfight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Dogfight.Location = new System.Drawing.Point(53, 238);
            this.Dogfight.Name = "Dogfight";
            this.Dogfight.Size = new System.Drawing.Size(91, 45);
            this.Dogfight.TabIndex = 1;
            this.Dogfight.Text = "Lucky Dogfight";
            this.Dogfight.UseVisualStyleBackColor = true;
            this.Dogfight.Click += new System.EventHandler(this.Dogfight_Click);
            this.Dogfight.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Dogfight.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Exit
            // 
            this.Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Exit.Location = new System.Drawing.Point(53, 301);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(260, 45);
            this.Exit.TabIndex = 1;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            this.Exit.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Exit.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // AppVersion
            // 
            this.AppVersion.BackColor = System.Drawing.Color.Transparent;
            this.AppVersion.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppVersion.ForeColor = System.Drawing.Color.Lime;
            this.AppVersion.Location = new System.Drawing.Point(2, 339);
            this.AppVersion.Name = "AppVersion";
            this.AppVersion.Size = new System.Drawing.Size(366, 31);
            this.AppVersion.TabIndex = 3;
            this.AppVersion.Text = "(ALPHA) v0.1a";
            this.AppVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // CustomGame
            // 
            this.CustomGame.BackColor = System.Drawing.Color.Black;
            this.CustomGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomGame.Location = new System.Drawing.Point(222, 187);
            this.CustomGame.Name = "CustomGame";
            this.CustomGame.Size = new System.Drawing.Size(91, 45);
            this.CustomGame.TabIndex = 1;
            this.CustomGame.Text = "Custom Battle";
            this.CustomGame.UseVisualStyleBackColor = false;
            this.CustomGame.Click += new System.EventHandler(this.CustomGame_Click);
            this.CustomGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.CustomGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(2, -13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(369, 156);
            this.label1.TabIndex = 4;
            // 
            // MapDesigner
            // 
            this.MapDesigner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapDesigner.Location = new System.Drawing.Point(150, 238);
            this.MapDesigner.Name = "MapDesigner";
            this.MapDesigner.Size = new System.Drawing.Size(163, 45);
            this.MapDesigner.TabIndex = 1;
            this.MapDesigner.Text = "Map Designer";
            this.MapDesigner.UseVisualStyleBackColor = true;
            this.MapDesigner.Click += new System.EventHandler(this.MapDesigner_Click);
            this.MapDesigner.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MapDesigner.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // QuickPlay
            // 
            this.QuickPlay.BackColor = System.Drawing.Color.Black;
            this.QuickPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.QuickPlay.Location = new System.Drawing.Point(53, 187);
            this.QuickPlay.Name = "QuickPlay";
            this.QuickPlay.Size = new System.Drawing.Size(163, 45);
            this.QuickPlay.TabIndex = 1;
            this.QuickPlay.Text = "Quick Skirmish";
            this.QuickPlay.UseVisualStyleBackColor = false;
            this.QuickPlay.Click += new System.EventHandler(this.QuickPlay_Click);
            this.QuickPlay.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.QuickPlay.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // PlayCampaign
            // 
            this.PlayCampaign.BackColor = System.Drawing.Color.Black;
            this.PlayCampaign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayCampaign.Location = new System.Drawing.Point(53, 136);
            this.PlayCampaign.Name = "PlayCampaign";
            this.PlayCampaign.Size = new System.Drawing.Size(260, 45);
            this.PlayCampaign.TabIndex = 1;
            this.PlayCampaign.Text = "Start Campaign";
            this.PlayCampaign.UseVisualStyleBackColor = false;
            this.PlayCampaign.Click += new System.EventHandler(this.PlayCampaign_Click);
            this.PlayCampaign.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.PlayCampaign.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(369, 374);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.MapDesigner);
            this.Controls.Add(this.Dogfight);
            this.Controls.Add(this.PlayCampaign);
            this.Controls.Add(this.CustomGame);
            this.Controls.Add(this.QuickPlay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AppVersion);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allegiance Forms";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Dogfight;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Label AppVersion;
        private System.Windows.Forms.Button CustomGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MapDesigner;
        private System.Windows.Forms.Button QuickPlay;
        private System.Windows.Forms.Button PlayCampaign;
    }
}