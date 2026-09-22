using Timer = System.Windows.Forms.Timer;

namespace MatchingGame
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private Label MakeIconLabel(int number)
        {
            var label = new Label();
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Webdings", 48F, FontStyle.Bold);
            label.Name = "label" + number;
            label.Size = new Size(125, 125);
            label.TabIndex = number - 1;
            label.Text = "c";
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Click += new EventHandler(this.label_Click);
            return label;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.timer1 = new Timer(this.components);

            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();

            // tableLayoutPanel1
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            this.label1 = MakeIconLabel(1);
            this.label2 = MakeIconLabel(2);
            this.label3 = MakeIconLabel(3);
            this.label4 = MakeIconLabel(4);
            this.label5 = MakeIconLabel(5);
            this.label6 = MakeIconLabel(6);
            this.label7 = MakeIconLabel(7);
            this.label8 = MakeIconLabel(8);
            this.label9 = MakeIconLabel(9);
            this.label10 = MakeIconLabel(10);
            this.label11 = MakeIconLabel(11);
            this.label12 = MakeIconLabel(12);
            this.label13 = MakeIconLabel(13);
            this.label14 = MakeIconLabel(14);
            this.label15 = MakeIconLabel(15);
            this.label16 = MakeIconLabel(16);

            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label11, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label12, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label14, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label15, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label16, 3, 3);

            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new Size(534, 511);
            this.tableLayoutPanel1.TabIndex = 0;

            // timer1
            this.timer1.Interval = 750;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);

            // Form1
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(534, 511);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Matching Game";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
