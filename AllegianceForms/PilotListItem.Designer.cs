namespace AllegianceForms
{
    partial class PilotListItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PilotListItem));
            this.Image = new System.Windows.Forms.PictureBox();
            this.ShipType = new System.Windows.Forms.Label();
            this.Sector = new System.Windows.Forms.Label();
            this.Info = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Image)).BeginInit();
            this.SuspendLayout();
            // 
            // Image
            // 
            this.Image.Image = ((System.Drawing.Image)(resources.GetObject("Image.Image")));
            this.Image.Location = new System.Drawing.Point(0, 3);
            this.Image.Name = "Image";
            this.Image.Size = new System.Drawing.Size(32, 32);
            this.Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Image.TabIndex = 0;
            this.Image.TabStop = false;
            this.Image.DoubleClick += new System.EventHandler(this.Info_DoubleClick);
            this.Image.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
            this.Image.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
            // 
            // ShipType
            // 
            this.ShipType.Location = new System.Drawing.Point(38, 3);
            this.ShipType.Name = "ShipType";
            this.ShipType.Size = new System.Drawing.Size(100, 32);
            this.ShipType.TabIndex = 1;
            this.ShipType.Text = "[ShipName]";
            this.ShipType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ShipType.DoubleClick += new System.EventHandler(this.Info_DoubleClick);
            this.ShipType.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
            this.ShipType.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
            // 
            // Sector
            // 
            this.Sector.Location = new System.Drawing.Point(144, 3);
            this.Sector.Name = "Sector";
            this.Sector.Size = new System.Drawing.Size(119, 32);
            this.Sector.TabIndex = 1;
            this.Sector.Text = "[SectorID - Name]";
            this.Sector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Sector.DoubleClick += new System.EventHandler(this.Info_DoubleClick);
            this.Sector.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
            this.Sector.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
            // 
            // Info
            // 
            this.Info.Location = new System.Drawing.Point(269, 3);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(126, 32);
            this.Info.TabIndex = 1;
            this.Info.Text = "[Health State]";
            this.Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Info.DoubleClick += new System.EventHandler(this.Info_DoubleClick);
            this.Info.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
            this.Info.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
            // 
            // PilotListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.Info);
            this.Controls.Add(this.Sector);
            this.Controls.Add(this.ShipType);
            this.Controls.Add(this.Image);
            this.ForeColor = System.Drawing.Color.Silver;
            this.Name = "PilotListItem";
            this.Size = new System.Drawing.Size(398, 38);
            this.DoubleClick += new System.EventHandler(this.Info_DoubleClick);
            this.MouseEnter += new System.EventHandler(this.Info_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Info_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Image;
        private System.Windows.Forms.Label ShipType;
        private System.Windows.Forms.Label Sector;
        private System.Windows.Forms.Label Info;
    }
}
