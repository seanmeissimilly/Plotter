namespace TestingXControls
{
    partial class Graficador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graficador));
            this.functionsViewer1 = new XControls.FunctionsViewer();
            this.SuspendLayout();
            // 
            // functionsViewer1
            // 
            this.functionsViewer1.BackColor = System.Drawing.Color.SteelBlue;
            this.functionsViewer1.Center = ((System.Drawing.PointF)(resources.GetObject("functionsViewer1.Center")));
            this.functionsViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionsViewer1.Location = new System.Drawing.Point(0, 0);
            this.functionsViewer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.functionsViewer1.Name = "functionsViewer1";
            this.functionsViewer1.Scale = 3F;
            this.functionsViewer1.Size = new System.Drawing.Size(583, 640);
            this.functionsViewer1.TabIndex = 0;
            // 
            // Graficador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 640);
            this.Controls.Add(this.functionsViewer1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Graficador";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Graficador";
            this.ResumeLayout(false);

        }

        #endregion

        private global::XControls.FunctionsViewer functionsViewer1;




    }
}

