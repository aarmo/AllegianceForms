namespace AllegianceForms.Forms
{
    partial class Research
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
            this.ConstructionButton = new System.Windows.Forms.Button();
            this.StarbaseButton = new System.Windows.Forms.Button();
            this.SupremacyButton = new System.Windows.Forms.Button();
            this.TacticalButton = new System.Windows.Forms.Button();
            this.ExpansionButton = new System.Windows.Forms.Button();
            this.BorderPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ResearchItems = new System.Windows.Forms.FlowLayoutPanel();
            this.ShipyardButton = new System.Windows.Forms.Button();
            this.BorderPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConstructionButton
            // 
            this.ConstructionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConstructionButton.ForeColor = System.Drawing.Color.White;
            this.ConstructionButton.Location = new System.Drawing.Point(3, 3);
            this.ConstructionButton.Name = "ConstructionButton";
            this.ConstructionButton.Size = new System.Drawing.Size(175, 60);
            this.ConstructionButton.TabIndex = 0;
            this.ConstructionButton.Text = "Construction";
            this.ConstructionButton.UseVisualStyleBackColor = true;
            this.ConstructionButton.Click += new System.EventHandler(this.ConstructionButton_Click);
            this.ConstructionButton.MouseEnter += new System.EventHandler(this.ConstructionButton_MouseEnter);
            // 
            // StarbaseButton
            // 
            this.StarbaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StarbaseButton.Location = new System.Drawing.Point(3, 69);
            this.StarbaseButton.Name = "StarbaseButton";
            this.StarbaseButton.Size = new System.Drawing.Size(175, 60);
            this.StarbaseButton.TabIndex = 0;
            this.StarbaseButton.Text = "Starbase";
            this.StarbaseButton.UseVisualStyleBackColor = true;
            this.StarbaseButton.Click += new System.EventHandler(this.StarbaseButton_Click);
            this.StarbaseButton.MouseEnter += new System.EventHandler(this.ConstructionButton_MouseEnter);
            // 
            // SupremacyButton
            // 
            this.SupremacyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SupremacyButton.ForeColor = System.Drawing.Color.White;
            this.SupremacyButton.Location = new System.Drawing.Point(3, 135);
            this.SupremacyButton.Name = "SupremacyButton";
            this.SupremacyButton.Size = new System.Drawing.Size(175, 60);
            this.SupremacyButton.TabIndex = 0;
            this.SupremacyButton.Text = "Supremacy";
            this.SupremacyButton.UseVisualStyleBackColor = true;
            this.SupremacyButton.Click += new System.EventHandler(this.SupremacyButton_Click);
            this.SupremacyButton.MouseEnter += new System.EventHandler(this.ConstructionButton_MouseEnter);
            // 
            // TacticalButton
            // 
            this.TacticalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TacticalButton.ForeColor = System.Drawing.Color.White;
            this.TacticalButton.Location = new System.Drawing.Point(3, 201);
            this.TacticalButton.Name = "TacticalButton";
            this.TacticalButton.Size = new System.Drawing.Size(175, 60);
            this.TacticalButton.TabIndex = 0;
            this.TacticalButton.Text = "Tactical";
            this.TacticalButton.UseVisualStyleBackColor = true;
            this.TacticalButton.Click += new System.EventHandler(this.TacticalButton_Click);
            this.TacticalButton.MouseEnter += new System.EventHandler(this.ConstructionButton_MouseEnter);
            // 
            // ExpansionButton
            // 
            this.ExpansionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExpansionButton.ForeColor = System.Drawing.Color.White;
            this.ExpansionButton.Location = new System.Drawing.Point(3, 267);
            this.ExpansionButton.Name = "ExpansionButton";
            this.ExpansionButton.Size = new System.Drawing.Size(175, 60);
            this.ExpansionButton.TabIndex = 0;
            this.ExpansionButton.Text = "Expansion";
            this.ExpansionButton.UseVisualStyleBackColor = true;
            this.ExpansionButton.Click += new System.EventHandler(this.ExpansionButton_Click);
            this.ExpansionButton.MouseEnter += new System.EventHandler(this.ConstructionButton_MouseEnter);
            // 
            // BorderPanel
            // 
            this.BorderPanel.BackColor = System.Drawing.Color.Black;
            this.BorderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BorderPanel.Controls.Add(this.panel1);
            this.BorderPanel.Controls.Add(this.ConstructionButton);
            this.BorderPanel.Controls.Add(this.StarbaseButton);
            this.BorderPanel.Controls.Add(this.SupremacyButton);
            this.BorderPanel.Controls.Add(this.ShipyardButton);
            this.BorderPanel.Controls.Add(this.ExpansionButton);
            this.BorderPanel.Controls.Add(this.TacticalButton);
            this.BorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BorderPanel.ForeColor = System.Drawing.Color.White;
            this.BorderPanel.Location = new System.Drawing.Point(0, 0);
            this.BorderPanel.Name = "BorderPanel";
            this.BorderPanel.Size = new System.Drawing.Size(458, 621);
            this.BorderPanel.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.ResearchItems);
            this.panel1.Location = new System.Drawing.Point(184, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 613);
            this.panel1.TabIndex = 4;
            // 
            // ResearchItems
            // 
            this.ResearchItems.AutoSize = true;
            this.ResearchItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ResearchItems.Location = new System.Drawing.Point(5, 0);
            this.ResearchItems.Name = "ResearchItems";
            this.ResearchItems.Size = new System.Drawing.Size(234, 613);
            this.ResearchItems.TabIndex = 3;
            this.ResearchItems.WrapContents = false;
            // 
            // ShipyardButton
            // 
            this.ShipyardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShipyardButton.ForeColor = System.Drawing.Color.White;
            this.ShipyardButton.Location = new System.Drawing.Point(3, 333);
            this.ShipyardButton.Name = "ShipyardButton";
            this.ShipyardButton.Size = new System.Drawing.Size(175, 60);
            this.ShipyardButton.TabIndex = 0;
            this.ShipyardButton.Text = "Shipyard";
            this.ShipyardButton.UseVisualStyleBackColor = true;
            this.ShipyardButton.Click += new System.EventHandler(this.ShipyardButton_Click);
            this.ShipyardButton.MouseEnter += new System.EventHandler(this.ConstructionButton_MouseEnter);
            // 
            // Research
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(458, 621);
            this.Controls.Add(this.BorderPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Research";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Research";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Research_KeyDown);
            this.BorderPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ConstructionButton;
        private System.Windows.Forms.Button StarbaseButton;
        private System.Windows.Forms.Button SupremacyButton;
        private System.Windows.Forms.Button TacticalButton;
        private System.Windows.Forms.Button ExpansionButton;
        private System.Windows.Forms.Panel BorderPanel;
        private System.Windows.Forms.FlowLayoutPanel ResearchItems;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ShipyardButton;
    }
}