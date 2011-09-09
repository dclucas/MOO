namespace Moo.Demo
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pgrTo = new System.Windows.Forms.PropertyGrid();
            this.cbxTo = new System.Windows.Forms.ComboBox();
            this.btnMap = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pgrFrom = new System.Windows.Forms.PropertyGrid();
            this.cbxFrom = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMap, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(651, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pgrTo);
            this.panel2.Controls.Add(this.cbxTo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(358, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(290, 256);
            this.panel2.TabIndex = 4;
            // 
            // pgrTo
            // 
            this.pgrTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgrTo.Location = new System.Drawing.Point(0, 21);
            this.pgrTo.Name = "pgrTo";
            this.pgrTo.Size = new System.Drawing.Size(290, 235);
            this.pgrTo.TabIndex = 1;
            // 
            // cbxTo
            // 
            this.cbxTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxTo.FormattingEnabled = true;
            this.cbxTo.Location = new System.Drawing.Point(0, 0);
            this.cbxTo.Name = "cbxTo";
            this.cbxTo.Size = new System.Drawing.Size(290, 21);
            this.cbxTo.TabIndex = 0;
            this.cbxTo.SelectedIndexChanged += new System.EventHandler(this.cbxTo_SelectedIndexChanged);
            // 
            // btnMap
            // 
            this.btnMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMap.Location = new System.Drawing.Point(298, 3);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(54, 256);
            this.btnMap.TabIndex = 1;
            this.btnMap.Text = "Map >>";
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pgrFrom);
            this.panel1.Controls.Add(this.cbxFrom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 256);
            this.panel1.TabIndex = 3;
            // 
            // pgrFrom
            // 
            this.pgrFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgrFrom.Location = new System.Drawing.Point(0, 21);
            this.pgrFrom.Name = "pgrFrom";
            this.pgrFrom.Size = new System.Drawing.Size(289, 235);
            this.pgrFrom.TabIndex = 1;
            // 
            // cbxFrom
            // 
            this.cbxFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxFrom.FormattingEnabled = true;
            this.cbxFrom.Location = new System.Drawing.Point(0, 0);
            this.cbxFrom.Name = "cbxFrom";
            this.cbxFrom.Size = new System.Drawing.Size(289, 21);
            this.cbxFrom.TabIndex = 0;
            this.cbxFrom.SelectedIndexChanged += new System.EventHandler(this.cbxFrom_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnMap;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 262);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Moo Demo";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PropertyGrid pgrTo;
        private System.Windows.Forms.ComboBox cbxTo;
        private System.Windows.Forms.Button btnMap;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PropertyGrid pgrFrom;
        private System.Windows.Forms.ComboBox cbxFrom;
    }
}

