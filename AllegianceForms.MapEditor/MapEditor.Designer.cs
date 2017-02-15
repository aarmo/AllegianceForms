namespace AllegianceForms.MapEditor
{
    partial class MapEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            this.ClearMap = new System.Windows.Forms.Button();
            this.SaveMap = new System.Windows.Forms.Button();
            this.LoadMap = new System.Windows.Forms.Button();
            this.PropertiesPanel = new System.Windows.Forms.Panel();
            this.SectorName = new System.Windows.Forms.TextBox();
            this.StartPos = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Symmetrical = new System.Windows.Forms.CheckBox();
            this.Random2 = new System.Windows.Forms.Button();
            this.Random2Size = new System.Windows.Forms.ComboBox();
            this.PropertiesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClearMap
            // 
            this.ClearMap.BackColor = System.Drawing.Color.LightCoral;
            this.ClearMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearMap.Location = new System.Drawing.Point(103, 2);
            this.ClearMap.Name = "ClearMap";
            this.ClearMap.Size = new System.Drawing.Size(44, 23);
            this.ClearMap.TabIndex = 2;
            this.ClearMap.Text = "Clear";
            this.ClearMap.UseVisualStyleBackColor = false;
            this.ClearMap.Click += new System.EventHandler(this.ClearMap_Click);
            // 
            // SaveMap
            // 
            this.SaveMap.BackColor = System.Drawing.Color.YellowGreen;
            this.SaveMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveMap.Location = new System.Drawing.Point(2, 2);
            this.SaveMap.Name = "SaveMap";
            this.SaveMap.Size = new System.Drawing.Size(44, 23);
            this.SaveMap.TabIndex = 0;
            this.SaveMap.Text = "Save";
            this.SaveMap.UseVisualStyleBackColor = false;
            this.SaveMap.Click += new System.EventHandler(this.SaveMap_Click);
            // 
            // LoadMap
            // 
            this.LoadMap.BackColor = System.Drawing.Color.PaleTurquoise;
            this.LoadMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadMap.Location = new System.Drawing.Point(52, 2);
            this.LoadMap.Name = "LoadMap";
            this.LoadMap.Size = new System.Drawing.Size(44, 23);
            this.LoadMap.TabIndex = 1;
            this.LoadMap.Text = "Load";
            this.LoadMap.UseVisualStyleBackColor = false;
            this.LoadMap.Click += new System.EventHandler(this.LoadMap_Click);
            // 
            // PropertiesPanel
            // 
            this.PropertiesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertiesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PropertiesPanel.Controls.Add(this.SectorName);
            this.PropertiesPanel.Controls.Add(this.StartPos);
            this.PropertiesPanel.Controls.Add(this.label2);
            this.PropertiesPanel.Controls.Add(this.label1);
            this.PropertiesPanel.Enabled = false;
            this.PropertiesPanel.Location = new System.Drawing.Point(615, 2);
            this.PropertiesPanel.Name = "PropertiesPanel";
            this.PropertiesPanel.Size = new System.Drawing.Size(167, 76);
            this.PropertiesPanel.TabIndex = 3;
            // 
            // SectorName
            // 
            this.SectorName.Location = new System.Drawing.Point(41, 25);
            this.SectorName.Name = "SectorName";
            this.SectorName.Size = new System.Drawing.Size(121, 20);
            this.SectorName.TabIndex = 2;
            this.SectorName.TextChanged += new System.EventHandler(this.SectorName_TextChanged);
            // 
            // StartPos
            // 
            this.StartPos.AutoSize = true;
            this.StartPos.Location = new System.Drawing.Point(41, 51);
            this.StartPos.Name = "StartPos";
            this.StartPos.Size = new System.Drawing.Size(91, 17);
            this.StartPos.TabIndex = 1;
            this.StartPos.Text = "Start Position:";
            this.StartPos.UseVisualStyleBackColor = true;
            this.StartPos.CheckedChanged += new System.EventHandler(this.StartPos_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Properties:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Symmetrical
            // 
            this.Symmetrical.AutoSize = true;
            this.Symmetrical.Location = new System.Drawing.Point(328, 7);
            this.Symmetrical.Name = "Symmetrical";
            this.Symmetrical.Size = new System.Drawing.Size(74, 17);
            this.Symmetrical.TabIndex = 7;
            this.Symmetrical.Text = "Symmetric";
            this.Symmetrical.UseVisualStyleBackColor = true;
            // 
            // Random2
            // 
            this.Random2.BackColor = System.Drawing.Color.Violet;
            this.Random2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Random2.Location = new System.Drawing.Point(153, 2);
            this.Random2.Name = "Random2";
            this.Random2.Size = new System.Drawing.Size(82, 23);
            this.Random2.TabIndex = 6;
            this.Random2.Text = "Randomise";
            this.Random2.UseVisualStyleBackColor = false;
            this.Random2.Click += new System.EventHandler(this.Random2_Click);
            // 
            // Random2Size
            // 
            this.Random2Size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Random2Size.FormattingEnabled = true;
            this.Random2Size.Location = new System.Drawing.Point(241, 4);
            this.Random2Size.Name = "Random2Size";
            this.Random2Size.Size = new System.Drawing.Size(81, 21);
            this.Random2Size.TabIndex = 8;
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.Random2Size);
            this.Controls.Add(this.Symmetrical);
            this.Controls.Add(this.Random2);
            this.Controls.Add(this.PropertiesPanel);
            this.Controls.Add(this.LoadMap);
            this.Controls.Add(this.SaveMap);
            this.Controls.Add(this.ClearMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MapEditor";
            this.Text = "Allegiance Forms - Map Editor []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapEditor_FormClosing);
            this.SizeChanged += new System.EventHandler(this.MapEditor_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapEditor_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapEditor_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MapEditor_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapEditor_MouseMove);
            this.PropertiesPanel.ResumeLayout(false);
            this.PropertiesPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ClearMap;
        private System.Windows.Forms.Button SaveMap;
        private System.Windows.Forms.Button LoadMap;
        private System.Windows.Forms.Panel PropertiesPanel;
        private System.Windows.Forms.TextBox SectorName;
        private System.Windows.Forms.CheckBox StartPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox Symmetrical;
        private System.Windows.Forms.Button Random2;
        private System.Windows.Forms.ComboBox Random2Size;
    }
}

