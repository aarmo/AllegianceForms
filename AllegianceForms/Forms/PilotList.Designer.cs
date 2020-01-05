namespace AllegianceForms.Forms
{
    partial class PilotList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PilotList));
            this.BorderPanel = new System.Windows.Forms.Panel();
            this.FilterCons = new System.Windows.Forms.PictureBox();
            this.FilterMiners = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PilotItems = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FilterSmall = new System.Windows.Forms.PictureBox();
            this.FilterCap = new System.Windows.Forms.PictureBox();
            this.BorderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterCons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterMiners)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterSmall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterCap)).BeginInit();
            this.SuspendLayout();
            // 
            // BorderPanel
            // 
            this.BorderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BorderPanel.Controls.Add(this.FilterCap);
            this.BorderPanel.Controls.Add(this.FilterCons);
            this.BorderPanel.Controls.Add(this.FilterSmall);
            this.BorderPanel.Controls.Add(this.FilterMiners);
            this.BorderPanel.Controls.Add(this.panel1);
            this.BorderPanel.Controls.Add(this.label6);
            this.BorderPanel.Controls.Add(this.label5);
            this.BorderPanel.Controls.Add(this.label4);
            this.BorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BorderPanel.Location = new System.Drawing.Point(0, 0);
            this.BorderPanel.Name = "BorderPanel";
            this.BorderPanel.Size = new System.Drawing.Size(444, 219);
            this.BorderPanel.TabIndex = 0;
            // 
            // FilterCons
            // 
            this.FilterCons.BackColor = System.Drawing.Color.DarkGreen;
            this.FilterCons.Image = ((System.Drawing.Image)(resources.GetObject("FilterCons.Image")));
            this.FilterCons.Location = new System.Drawing.Point(40, 4);
            this.FilterCons.Name = "FilterCons";
            this.FilterCons.Size = new System.Drawing.Size(16, 16);
            this.FilterCons.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FilterCons.TabIndex = 6;
            this.FilterCons.TabStop = false;
            this.FilterCons.Click += new System.EventHandler(this.FilterCons_Click);
            this.FilterCons.DoubleClick += new System.EventHandler(this.FilterCons_DoubleClick);
            // 
            // FilterMiners
            // 
            this.FilterMiners.BackColor = System.Drawing.Color.DarkGreen;
            this.FilterMiners.Image = ((System.Drawing.Image)(resources.GetObject("FilterMiners.Image")));
            this.FilterMiners.Location = new System.Drawing.Point(22, 4);
            this.FilterMiners.Name = "FilterMiners";
            this.FilterMiners.Size = new System.Drawing.Size(16, 16);
            this.FilterMiners.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FilterMiners.TabIndex = 6;
            this.FilterMiners.TabStop = false;
            this.FilterMiners.Click += new System.EventHandler(this.FilterMiners_Click);
            this.FilterMiners.DoubleClick += new System.EventHandler(this.FilterMiners_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.PilotItems);
            this.panel1.Location = new System.Drawing.Point(3, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(436, 190);
            this.panel1.TabIndex = 5;
            // 
            // PilotItems
            // 
            this.PilotItems.AutoSize = true;
            this.PilotItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.PilotItems.Location = new System.Drawing.Point(5, 0);
            this.PilotItems.Name = "PilotItems";
            this.PilotItems.Size = new System.Drawing.Size(410, 187);
            this.PilotItems.TabIndex = 3;
            this.PilotItems.WrapContents = false;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Lime;
            this.label6.Location = new System.Drawing.Point(284, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Info:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Lime;
            this.label5.Location = new System.Drawing.Point(156, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Sector:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Lime;
            this.label4.Location = new System.Drawing.Point(50, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Type:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FilterSmall
            // 
            this.FilterSmall.BackColor = System.Drawing.Color.DarkGreen;
            this.FilterSmall.Image = ((System.Drawing.Image)(resources.GetObject("FilterSmall.Image")));
            this.FilterSmall.Location = new System.Drawing.Point(4, 4);
            this.FilterSmall.Name = "FilterSmall";
            this.FilterSmall.Size = new System.Drawing.Size(16, 16);
            this.FilterSmall.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FilterSmall.TabIndex = 6;
            this.FilterSmall.TabStop = false;
            this.FilterSmall.Click += new System.EventHandler(this.FilterSmall_Click);
            this.FilterSmall.DoubleClick += new System.EventHandler(this.FilterSmall_DoubleClick);
            // 
            // FilterCap
            // 
            this.FilterCap.BackColor = System.Drawing.Color.DarkGreen;
            this.FilterCap.Image = ((System.Drawing.Image)(resources.GetObject("FilterCap.Image")));
            this.FilterCap.Location = new System.Drawing.Point(58, 4);
            this.FilterCap.Name = "FilterCap";
            this.FilterCap.Size = new System.Drawing.Size(16, 16);
            this.FilterCap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FilterCap.TabIndex = 6;
            this.FilterCap.TabStop = false;
            this.FilterCap.Click += new System.EventHandler(this.FilterCap_Click);
            this.FilterCap.DoubleClick += new System.EventHandler(this.FilterCap_DoubleClick);
            // 
            // PilotList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(444, 219);
            this.Controls.Add(this.BorderPanel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "PilotList";
            this.Text = "PilotList";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PilotList_KeyDown);
            this.BorderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilterCons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterMiners)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterSmall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterCap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BorderPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel PilotItems;
        private System.Windows.Forms.PictureBox FilterMiners;
        private System.Windows.Forms.PictureBox FilterCons;
        private System.Windows.Forms.PictureBox FilterSmall;
        private System.Windows.Forms.PictureBox FilterCap;
    }
}