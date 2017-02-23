namespace AllegianceForms
{
    partial class Sector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sector));
            this.tick = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.CreditsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.PilotsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.CommandsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.miniMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.researchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pilotListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyAIDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.pauseGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.GameOverPanel = new System.Windows.Forms.Panel();
            this.TotalBases2 = new System.Windows.Forms.Label();
            this.TotalBases1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TotalConstructorsDestroyed2 = new System.Windows.Forms.Label();
            this.TotalConstructorsDestroyed1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TotalConstructors2 = new System.Windows.Forms.Label();
            this.TotalConstructors1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TotalMiners2 = new System.Windows.Forms.Label();
            this.TotalMiners1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TotalMinersDestroyed2 = new System.Windows.Forms.Label();
            this.TotalMinersDestroyed1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TotalBasesDestroyed2 = new System.Windows.Forms.Label();
            this.TotalBasesDestroyed1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Team2 = new System.Windows.Forms.Label();
            this.TotalMined2 = new System.Windows.Forms.Label();
            this.Team1 = new System.Windows.Forms.Label();
            this.TotalMined1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WinLose = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AlertMessage = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.GameOverPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tick
            // 
            this.tick.Enabled = true;
            this.tick.Interval = 50;
            this.tick.Tick += new System.EventHandler(this.tick_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreditsLabel,
            this.PilotsLabel,
            this.CommandsLabel,
            this.toolStripDropDownButton1});
            this.statusStrip.Location = new System.Drawing.Point(0, 711);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1184, 30);
            this.statusStrip.TabIndex = 10;
            this.statusStrip.Text = "statusStrip";
            // 
            // CreditsLabel
            // 
            this.CreditsLabel.BackColor = System.Drawing.Color.White;
            this.CreditsLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.CreditsLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.CreditsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditsLabel.ForeColor = System.Drawing.Color.Olive;
            this.CreditsLabel.Name = "CreditsLabel";
            this.CreditsLabel.Size = new System.Drawing.Size(93, 25);
            this.CreditsLabel.Text = "Credits: $0";
            // 
            // PilotsLabel
            // 
            this.PilotsLabel.BackColor = System.Drawing.Color.White;
            this.PilotsLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.PilotsLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.PilotsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PilotsLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.PilotsLabel.Name = "PilotsLabel";
            this.PilotsLabel.Size = new System.Drawing.Size(145, 25);
            this.PilotsLabel.Text = "Docked Pilots: 16";
            // 
            // CommandsLabel
            // 
            this.CommandsLabel.BackColor = System.Drawing.Color.Silver;
            this.CommandsLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.CommandsLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.CommandsLabel.ForeColor = System.Drawing.Color.Black;
            this.CommandsLabel.Name = "CommandsLabel";
            this.CommandsLabel.Size = new System.Drawing.Size(843, 25);
            this.CommandsLabel.Spring = true;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.BackColor = System.Drawing.Color.White;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miniMapToolStripMenuItem,
            this.researchToolStripMenuItem,
            this.pilotListToolStripMenuItem,
            this.enemyAIDebugToolStripMenuItem,
            this.toolStripMenuItem1,
            this.pauseGameToolStripMenuItem});
            this.toolStripDropDownButton1.ForeColor = System.Drawing.Color.Black;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(88, 28);
            this.toolStripDropDownButton1.Text = "&Windows";
            // 
            // miniMapToolStripMenuItem
            // 
            this.miniMapToolStripMenuItem.Name = "miniMapToolStripMenuItem";
            this.miniMapToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.miniMapToolStripMenuItem.Text = "[F3] Mini M&ap";
            this.miniMapToolStripMenuItem.Click += new System.EventHandler(this.miniMapToolStripMenuItem_Click);
            // 
            // researchToolStripMenuItem
            // 
            this.researchToolStripMenuItem.Name = "researchToolStripMenuItem";
            this.researchToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.researchToolStripMenuItem.Text = "[F5] &Research";
            this.researchToolStripMenuItem.Click += new System.EventHandler(this.researchToolStripMenuItem_Click);
            // 
            // pilotListToolStripMenuItem
            // 
            this.pilotListToolStripMenuItem.Name = "pilotListToolStripMenuItem";
            this.pilotListToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.pilotListToolStripMenuItem.Text = "[F6] &Pilot List";
            this.pilotListToolStripMenuItem.Click += new System.EventHandler(this.pilotListToolStripMenuItem_Click);
            // 
            // enemyAIDebugToolStripMenuItem
            // 
            this.enemyAIDebugToolStripMenuItem.Name = "enemyAIDebugToolStripMenuItem";
            this.enemyAIDebugToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.enemyAIDebugToolStripMenuItem.Text = "[F12] Enemy AI &Testing";
            this.enemyAIDebugToolStripMenuItem.Click += new System.EventHandler(this.enemyAIDebugToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(234, 6);
            // 
            // pauseGameToolStripMenuItem
            // 
            this.pauseGameToolStripMenuItem.Name = "pauseGameToolStripMenuItem";
            this.pauseGameToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.pauseGameToolStripMenuItem.Text = "[&Esc] Main Menu";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // GameOverPanel
            // 
            this.GameOverPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GameOverPanel.Controls.Add(this.TotalBases2);
            this.GameOverPanel.Controls.Add(this.TotalBases1);
            this.GameOverPanel.Controls.Add(this.label6);
            this.GameOverPanel.Controls.Add(this.TotalConstructorsDestroyed2);
            this.GameOverPanel.Controls.Add(this.TotalConstructorsDestroyed1);
            this.GameOverPanel.Controls.Add(this.label9);
            this.GameOverPanel.Controls.Add(this.TotalConstructors2);
            this.GameOverPanel.Controls.Add(this.TotalConstructors1);
            this.GameOverPanel.Controls.Add(this.label8);
            this.GameOverPanel.Controls.Add(this.TotalMiners2);
            this.GameOverPanel.Controls.Add(this.TotalMiners1);
            this.GameOverPanel.Controls.Add(this.label7);
            this.GameOverPanel.Controls.Add(this.TotalMinersDestroyed2);
            this.GameOverPanel.Controls.Add(this.TotalMinersDestroyed1);
            this.GameOverPanel.Controls.Add(this.label5);
            this.GameOverPanel.Controls.Add(this.TotalBasesDestroyed2);
            this.GameOverPanel.Controls.Add(this.TotalBasesDestroyed1);
            this.GameOverPanel.Controls.Add(this.label4);
            this.GameOverPanel.Controls.Add(this.Team2);
            this.GameOverPanel.Controls.Add(this.TotalMined2);
            this.GameOverPanel.Controls.Add(this.Team1);
            this.GameOverPanel.Controls.Add(this.TotalMined1);
            this.GameOverPanel.Controls.Add(this.label3);
            this.GameOverPanel.Controls.Add(this.WinLose);
            this.GameOverPanel.Controls.Add(this.label1);
            this.GameOverPanel.Location = new System.Drawing.Point(12, 12);
            this.GameOverPanel.Name = "GameOverPanel";
            this.GameOverPanel.Size = new System.Drawing.Size(403, 332);
            this.GameOverPanel.TabIndex = 11;
            this.GameOverPanel.Visible = false;
            // 
            // TotalBases2
            // 
            this.TotalBases2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalBases2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalBases2.Location = new System.Drawing.Point(253, 155);
            this.TotalBases2.Name = "TotalBases2";
            this.TotalBases2.Size = new System.Drawing.Size(138, 28);
            this.TotalBases2.TabIndex = 0;
            this.TotalBases2.Text = "0";
            this.TotalBases2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalBases1
            // 
            this.TotalBases1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalBases1.ForeColor = System.Drawing.Color.Lime;
            this.TotalBases1.Location = new System.Drawing.Point(109, 155);
            this.TotalBases1.Name = "TotalBases1";
            this.TotalBases1.Size = new System.Drawing.Size(138, 28);
            this.TotalBases1.TabIndex = 0;
            this.TotalBases1.Text = "0";
            this.TotalBases1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(6, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 28);
            this.label6.TabIndex = 0;
            this.label6.Text = "Bases Built";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalConstructorsDestroyed2
            // 
            this.TotalConstructorsDestroyed2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalConstructorsDestroyed2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalConstructorsDestroyed2.Location = new System.Drawing.Point(253, 295);
            this.TotalConstructorsDestroyed2.Name = "TotalConstructorsDestroyed2";
            this.TotalConstructorsDestroyed2.Size = new System.Drawing.Size(138, 28);
            this.TotalConstructorsDestroyed2.TabIndex = 0;
            this.TotalConstructorsDestroyed2.Text = "0";
            this.TotalConstructorsDestroyed2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalConstructorsDestroyed1
            // 
            this.TotalConstructorsDestroyed1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalConstructorsDestroyed1.ForeColor = System.Drawing.Color.Lime;
            this.TotalConstructorsDestroyed1.Location = new System.Drawing.Point(109, 295);
            this.TotalConstructorsDestroyed1.Name = "TotalConstructorsDestroyed1";
            this.TotalConstructorsDestroyed1.Size = new System.Drawing.Size(138, 28);
            this.TotalConstructorsDestroyed1.TabIndex = 0;
            this.TotalConstructorsDestroyed1.Text = "0";
            this.TotalConstructorsDestroyed1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(5, 295);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 28);
            this.label9.TabIndex = 0;
            this.label9.Text = "Constructors Lost:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalConstructors2
            // 
            this.TotalConstructors2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalConstructors2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalConstructors2.Location = new System.Drawing.Point(253, 267);
            this.TotalConstructors2.Name = "TotalConstructors2";
            this.TotalConstructors2.Size = new System.Drawing.Size(138, 28);
            this.TotalConstructors2.TabIndex = 0;
            this.TotalConstructors2.Text = "0";
            this.TotalConstructors2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalConstructors1
            // 
            this.TotalConstructors1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalConstructors1.ForeColor = System.Drawing.Color.Lime;
            this.TotalConstructors1.Location = new System.Drawing.Point(109, 267);
            this.TotalConstructors1.Name = "TotalConstructors1";
            this.TotalConstructors1.Size = new System.Drawing.Size(138, 28);
            this.TotalConstructors1.TabIndex = 0;
            this.TotalConstructors1.Text = "0";
            this.TotalConstructors1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(6, 267);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 28);
            this.label8.TabIndex = 0;
            this.label8.Text = "Constructors Built:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalMiners2
            // 
            this.TotalMiners2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalMiners2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalMiners2.Location = new System.Drawing.Point(253, 211);
            this.TotalMiners2.Name = "TotalMiners2";
            this.TotalMiners2.Size = new System.Drawing.Size(138, 28);
            this.TotalMiners2.TabIndex = 0;
            this.TotalMiners2.Text = "0";
            this.TotalMiners2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalMiners1
            // 
            this.TotalMiners1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalMiners1.ForeColor = System.Drawing.Color.Lime;
            this.TotalMiners1.Location = new System.Drawing.Point(109, 211);
            this.TotalMiners1.Name = "TotalMiners1";
            this.TotalMiners1.Size = new System.Drawing.Size(138, 28);
            this.TotalMiners1.TabIndex = 0;
            this.TotalMiners1.Text = "0";
            this.TotalMiners1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(6, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 28);
            this.label7.TabIndex = 0;
            this.label7.Text = "Miners Built";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalMinersDestroyed2
            // 
            this.TotalMinersDestroyed2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalMinersDestroyed2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalMinersDestroyed2.Location = new System.Drawing.Point(253, 239);
            this.TotalMinersDestroyed2.Name = "TotalMinersDestroyed2";
            this.TotalMinersDestroyed2.Size = new System.Drawing.Size(138, 28);
            this.TotalMinersDestroyed2.TabIndex = 0;
            this.TotalMinersDestroyed2.Text = "0";
            this.TotalMinersDestroyed2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalMinersDestroyed1
            // 
            this.TotalMinersDestroyed1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalMinersDestroyed1.ForeColor = System.Drawing.Color.Lime;
            this.TotalMinersDestroyed1.Location = new System.Drawing.Point(109, 239);
            this.TotalMinersDestroyed1.Name = "TotalMinersDestroyed1";
            this.TotalMinersDestroyed1.Size = new System.Drawing.Size(138, 28);
            this.TotalMinersDestroyed1.TabIndex = 0;
            this.TotalMinersDestroyed1.Text = "0";
            this.TotalMinersDestroyed1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(5, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 28);
            this.label5.TabIndex = 0;
            this.label5.Text = "Miners Lost:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalBasesDestroyed2
            // 
            this.TotalBasesDestroyed2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalBasesDestroyed2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalBasesDestroyed2.Location = new System.Drawing.Point(253, 183);
            this.TotalBasesDestroyed2.Name = "TotalBasesDestroyed2";
            this.TotalBasesDestroyed2.Size = new System.Drawing.Size(138, 28);
            this.TotalBasesDestroyed2.TabIndex = 0;
            this.TotalBasesDestroyed2.Text = "0";
            this.TotalBasesDestroyed2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalBasesDestroyed1
            // 
            this.TotalBasesDestroyed1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalBasesDestroyed1.ForeColor = System.Drawing.Color.Lime;
            this.TotalBasesDestroyed1.Location = new System.Drawing.Point(109, 183);
            this.TotalBasesDestroyed1.Name = "TotalBasesDestroyed1";
            this.TotalBasesDestroyed1.Size = new System.Drawing.Size(138, 28);
            this.TotalBasesDestroyed1.TabIndex = 0;
            this.TotalBasesDestroyed1.Text = "0";
            this.TotalBasesDestroyed1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(5, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "Bases Lost:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Team2
            // 
            this.Team2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Team2.ForeColor = System.Drawing.Color.LightPink;
            this.Team2.Location = new System.Drawing.Point(253, 99);
            this.Team2.Name = "Team2";
            this.Team2.Size = new System.Drawing.Size(138, 28);
            this.Team2.TabIndex = 0;
            this.Team2.Text = "Team 2";
            this.Team2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalMined2
            // 
            this.TotalMined2.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalMined2.ForeColor = System.Drawing.Color.LightPink;
            this.TotalMined2.Location = new System.Drawing.Point(253, 127);
            this.TotalMined2.Name = "TotalMined2";
            this.TotalMined2.Size = new System.Drawing.Size(138, 28);
            this.TotalMined2.TabIndex = 0;
            this.TotalMined2.Text = "0";
            this.TotalMined2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Team1
            // 
            this.Team1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Team1.ForeColor = System.Drawing.Color.Lime;
            this.Team1.Location = new System.Drawing.Point(109, 99);
            this.Team1.Name = "Team1";
            this.Team1.Size = new System.Drawing.Size(138, 28);
            this.Team1.TabIndex = 0;
            this.Team1.Text = "Team 1";
            this.Team1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalMined1
            // 
            this.TotalMined1.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalMined1.ForeColor = System.Drawing.Color.Lime;
            this.TotalMined1.Location = new System.Drawing.Point(109, 127);
            this.TotalMined1.Name = "TotalMined1";
            this.TotalMined1.Size = new System.Drawing.Size(138, 28);
            this.TotalMined1.TabIndex = 0;
            this.TotalMined1.Text = "0";
            this.TotalMined1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(6, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "Resources Mined";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WinLose
            // 
            this.WinLose.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinLose.Location = new System.Drawing.Point(8, 41);
            this.WinLose.Name = "WinLose";
            this.WinLose.Size = new System.Drawing.Size(383, 58);
            this.WinLose.TabIndex = 0;
            this.WinLose.Text = "You Win!";
            this.WinLose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game Over";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlertMessage
            // 
            this.AlertMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AlertMessage.ForeColor = System.Drawing.Color.DimGray;
            this.AlertMessage.Location = new System.Drawing.Point(12, 13);
            this.AlertMessage.Name = "AlertMessage";
            this.AlertMessage.Size = new System.Drawing.Size(1160, 41);
            this.AlertMessage.TabIndex = 12;
            this.AlertMessage.Text = "[Alert Message]";
            this.AlertMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AlertMessage.Visible = false;
            // 
            // Sector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1184, 741);
            this.Controls.Add(this.AlertMessage);
            this.Controls.Add(this.GameOverPanel);
            this.Controls.Add(this.statusStrip);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Lime;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Sector";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allegiance Forms - Conquest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Sector_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Sector_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Sector_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Sector_KeyUp);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Sector_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Sector_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Sector_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Sector_MouseUp);
            this.Move += new System.EventHandler(this.Sector_Move);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.GameOverPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tick;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel CreditsLabel;
        private System.Windows.Forms.ToolStripStatusLabel CommandsLabel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem researchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miniMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pauseGameToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripStatusLabel PilotsLabel;
        private System.Windows.Forms.ToolStripMenuItem enemyAIDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pilotListToolStripMenuItem;
        private System.Windows.Forms.Panel GameOverPanel;
        private System.Windows.Forms.Label TotalBases2;
        private System.Windows.Forms.Label TotalBases1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label TotalConstructorsDestroyed2;
        private System.Windows.Forms.Label TotalConstructorsDestroyed1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label TotalConstructors2;
        private System.Windows.Forms.Label TotalConstructors1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label TotalMiners2;
        private System.Windows.Forms.Label TotalMiners1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label TotalMinersDestroyed2;
        private System.Windows.Forms.Label TotalMinersDestroyed1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label TotalBasesDestroyed2;
        private System.Windows.Forms.Label TotalBasesDestroyed1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label TotalMined2;
        private System.Windows.Forms.Label Team1;
        private System.Windows.Forms.Label TotalMined1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label WinLose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Team2;
        private System.Windows.Forms.Label AlertMessage;
    }
}

