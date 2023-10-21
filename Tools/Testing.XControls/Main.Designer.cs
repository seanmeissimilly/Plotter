namespace TestingXControls
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mname = new System.Windows.Forms.Label();
            this.module_name = new System.Windows.Forms.TextBox();
            this.inputboxtext = new System.Windows.Forms.TextBox();
            this.exitboxtext = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(771, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "Menu";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem,
            this.runToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.compileToolStripMenuItem.Text = "Compile";
            this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 24);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboToolStripMenuItem
            // 
            this.aboToolStripMenuItem.Name = "aboToolStripMenuItem";
            this.aboToolStripMenuItem.Size = new System.Drawing.Size(174, 24);
            this.aboToolStripMenuItem.Text = "About Plotter";
            this.aboToolStripMenuItem.Click += new System.EventHandler(this.aboToolStripMenuItem_Click);
            // 
            // mname
            // 
            this.mname.BackColor = System.Drawing.Color.Transparent;
            this.mname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mname.Location = new System.Drawing.Point(524, 28);
            this.mname.Name = "mname";
            this.mname.Size = new System.Drawing.Size(114, 19);
            this.mname.TabIndex = 15;
            this.mname.Text = "Module name:";
            // 
            // module_name
            // 
            this.module_name.Location = new System.Drawing.Point(644, 27);
            this.module_name.Multiline = true;
            this.module_name.Name = "module_name";
            this.module_name.Size = new System.Drawing.Size(116, 20);
            this.module_name.TabIndex = 16;
            // 
            // inputboxtext
            // 
            this.inputboxtext.BackColor = System.Drawing.Color.White;
            this.inputboxtext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputboxtext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputboxtext.ForeColor = System.Drawing.Color.Black;
            this.inputboxtext.Location = new System.Drawing.Point(12, 61);
            this.inputboxtext.Multiline = true;
            this.inputboxtext.Name = "inputboxtext";
            this.inputboxtext.Size = new System.Drawing.Size(365, 318);
            this.inputboxtext.TabIndex = 17;
            this.inputboxtext.TabStop = false;
            // 
            // exitboxtext
            // 
            this.exitboxtext.BackColor = System.Drawing.Color.White;
            this.exitboxtext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exitboxtext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitboxtext.ForeColor = System.Drawing.Color.Black;
            this.exitboxtext.Location = new System.Drawing.Point(404, 61);
            this.exitboxtext.Multiline = true;
            this.exitboxtext.Name = "exitboxtext";
            this.exitboxtext.Size = new System.Drawing.Size(356, 318);
            this.exitboxtext.TabIndex = 18;
            this.exitboxtext.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 401);
            this.Controls.Add(this.exitboxtext);
            this.Controls.Add(this.inputboxtext);
            this.Controls.Add(this.module_name);
            this.Controls.Add(this.mname);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Plotter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboToolStripMenuItem;
        private System.Windows.Forms.Label mname;
        private System.Windows.Forms.TextBox module_name;
        private System.Windows.Forms.TextBox inputboxtext;
        private System.Windows.Forms.TextBox exitboxtext;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}