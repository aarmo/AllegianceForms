namespace AllegianceForms.Forms
{
    partial class CommandBar
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
            this.flowLayoutPanelOrders = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PilotsLabel = new System.Windows.Forms.Label();
            this.ResourcesLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelOrders
            // 
            this.flowLayoutPanelOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelOrders.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelOrders.Location = new System.Drawing.Point(0, 301);
            this.flowLayoutPanelOrders.Name = "flowLayoutPanelOrders";
            this.flowLayoutPanelOrders.Size = new System.Drawing.Size(120, 531);
            this.flowLayoutPanelOrders.TabIndex = 0;
            this.flowLayoutPanelOrders.WrapContents = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PilotsLabel);
            this.panel1.Controls.Add(this.ResourcesLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 100);
            this.panel1.TabIndex = 0;
            // 
            // PilotsLabel
            // 
            this.PilotsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PilotsLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PilotsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PilotsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PilotsLabel.ForeColor = System.Drawing.Color.GreenYellow;
            this.PilotsLabel.Location = new System.Drawing.Point(0, 74);
            this.PilotsLabel.Name = "PilotsLabel";
            this.PilotsLabel.Size = new System.Drawing.Size(102, 23);
            this.PilotsLabel.TabIndex = 2;
            this.PilotsLabel.Text = "999";
            this.PilotsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResourcesLabel
            // 
            this.ResourcesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourcesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ResourcesLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResourcesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResourcesLabel.ForeColor = System.Drawing.Color.Gold;
            this.ResourcesLabel.Location = new System.Drawing.Point(0, 24);
            this.ResourcesLabel.Name = "ResourcesLabel";
            this.ResourcesLabel.Size = new System.Drawing.Size(102, 23);
            this.ResourcesLabel.TabIndex = 3;
            this.ResourcesLabel.Text = "100000";
            this.ResourcesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "Docked Pilots:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label1
            // 
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(3, 1);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(96, 23);
            this.Label1.TabIndex = 5;
            this.Label1.Text = "Resources:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(0, 100);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(120, 201);
            this.flowLayoutPanelButtons.TabIndex = 1;
            this.flowLayoutPanelButtons.WrapContents = false;
            // 
            // CommandBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(120, 832);
            this.Controls.Add(this.flowLayoutPanelOrders);
            this.Controls.Add(this.flowLayoutPanelButtons);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CommandBar";
            this.Text = "CommandBar";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOrders;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label PilotsLabel;
        private System.Windows.Forms.Label ResourcesLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
    }
}