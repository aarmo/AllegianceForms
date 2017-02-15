namespace AllegianceForms
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
            this.Skirmish = new System.Windows.Forms.Button();
            this.Dogfight = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AppVersion = new System.Windows.Forms.Label();
            this.CustomGame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Skirmish
            // 
            this.Skirmish.BackColor = System.Drawing.Color.Black;
            this.Skirmish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Skirmish.Location = new System.Drawing.Point(12, 105);
            this.Skirmish.Name = "Skirmish";
            this.Skirmish.Size = new System.Drawing.Size(163, 45);
            this.Skirmish.TabIndex = 1;
            this.Skirmish.Text = "Conquest - Quick Battle";
            this.Skirmish.UseVisualStyleBackColor = false;
            this.Skirmish.Click += new System.EventHandler(this.Skirmish_Click);
            this.Skirmish.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Skirmish.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Dogfight
            // 
            this.Dogfight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Dogfight.Location = new System.Drawing.Point(12, 156);
            this.Dogfight.Name = "Dogfight";
            this.Dogfight.Size = new System.Drawing.Size(260, 45);
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
            this.Exit.Location = new System.Drawing.Point(12, 237);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(260, 45);
            this.Exit.TabIndex = 1;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            this.Exit.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.Exit.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 102);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Lucida Console", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(191, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Forms";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AppVersion
            // 
            this.AppVersion.BackColor = System.Drawing.Color.Transparent;
            this.AppVersion.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppVersion.ForeColor = System.Drawing.Color.Lime;
            this.AppVersion.Location = new System.Drawing.Point(10, 285);
            this.AppVersion.Name = "AppVersion";
            this.AppVersion.Size = new System.Drawing.Size(272, 23);
            this.AppVersion.TabIndex = 3;
            this.AppVersion.Text = "(ALPHA) v0.1a";
            this.AppVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // CustomGame
            // 
            this.CustomGame.BackColor = System.Drawing.Color.Black;
            this.CustomGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomGame.Location = new System.Drawing.Point(181, 105);
            this.CustomGame.Name = "CustomGame";
            this.CustomGame.Size = new System.Drawing.Size(91, 45);
            this.CustomGame.TabIndex = 1;
            this.CustomGame.Text = "Custom Battle";
            this.CustomGame.UseVisualStyleBackColor = false;
            this.CustomGame.Click += new System.EventHandler(this.CustomGame_Click);
            this.CustomGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.CustomGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(284, 312);
            this.Controls.Add(this.AppVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Dogfight);
            this.Controls.Add(this.CustomGame);
            this.Controls.Add(this.Skirmish);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allegiance Forms";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Skirmish;
        private System.Windows.Forms.Button Dogfight;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label AppVersion;
        private System.Windows.Forms.Button CustomGame;
    }
}