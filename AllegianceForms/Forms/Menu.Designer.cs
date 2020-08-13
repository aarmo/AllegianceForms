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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.AppVersion = new System.Windows.Forms.Label();
            this.CustomGame = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.MapDesigner = new System.Windows.Forms.Button();
            this.animateStars = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // AppVersion
            // 
            this.AppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppVersion.BackColor = System.Drawing.Color.Transparent;
            this.AppVersion.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppVersion.ForeColor = System.Drawing.Color.Lime;
            this.AppVersion.Location = new System.Drawing.Point(3, 412);
            this.AppVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AppVersion.Name = "AppVersion";
            this.AppVersion.Size = new System.Drawing.Size(556, 49);
            this.AppVersion.TabIndex = 7;
            this.AppVersion.Text = "(ALPHA) v0.1a";
            this.AppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CustomGame
            // 
            this.CustomGame.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CustomGame.BackColor = System.Drawing.Color.Black;
            this.CustomGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustomGame.Location = new System.Drawing.Point(87, 232);
            this.CustomGame.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CustomGame.Name = "CustomGame";
            this.CustomGame.Size = new System.Drawing.Size(390, 69);
            this.CustomGame.TabIndex = 3;
            this.CustomGame.Text = "Play Game";
            this.CustomGame.UseVisualStyleBackColor = false;
            this.CustomGame.Click += new System.EventHandler(this.CustomGame_Click);
            this.CustomGame.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.CustomGame.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.Image = ((System.Drawing.Image)(resources.GetObject("Title.Image")));
            this.Title.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Title.Location = new System.Drawing.Point(2, -32);
            this.Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(561, 228);
            this.Title.TabIndex = 0;
            // 
            // MapDesigner
            // 
            this.MapDesigner.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MapDesigner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MapDesigner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MapDesigner.Location = new System.Drawing.Point(87, 310);
            this.MapDesigner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MapDesigner.Name = "MapDesigner";
            this.MapDesigner.Size = new System.Drawing.Size(390, 69);
            this.MapDesigner.TabIndex = 5;
            this.MapDesigner.Text = "Map Designer";
            this.MapDesigner.UseVisualStyleBackColor = true;
            this.MapDesigner.Click += new System.EventHandler(this.MapDesigner_Click);
            this.MapDesigner.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.MapDesigner.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // animateStars
            // 
            this.animateStars.Interval = 30;
            this.animateStars.Tick += new System.EventHandler(this.animateStars_Tick);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(561, 462);
            this.Controls.Add(this.MapDesigner);
            this.Controls.Add(this.CustomGame);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.AppVersion);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allegiance Forms";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Menu_Paint);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label AppVersion;
        private System.Windows.Forms.Button CustomGame;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button MapDesigner;
        private System.Windows.Forms.Timer animateStars;
    }
}