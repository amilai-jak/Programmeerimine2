namespace PictureViewer
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox checkBox1;
        private Button showButton;
        private Button clearButton;
        private Button backgroundButton;
        private Button closeButton;
        private OpenFileDialog openFileDialog1;
        private ColorDialog colorDialog1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.pictureBox1 = new PictureBox();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.checkBox1 = new CheckBox();
            this.showButton = new Button();
            this.clearButton = new Button();
            this.backgroundButton = new Button();
            this.closeButton = new Button();
            this.openFileDialog1 = new OpenFileDialog();
            this.colorDialog1 = new ColorDialog();

            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();

            // tableLayoutPanel1
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new Size(800, 520);
            this.tableLayoutPanel1.TabIndex = 0;

            // pictureBox1
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.Location = new Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(794, 436);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;

            // flowLayoutPanel1
            this.flowLayoutPanel1.Controls.Add(this.checkBox1);
            this.flowLayoutPanel1.Controls.Add(this.showButton);
            this.flowLayoutPanel1.Controls.Add(this.clearButton);
            this.flowLayoutPanel1.Controls.Add(this.backgroundButton);
            this.flowLayoutPanel1.Controls.Add(this.closeButton);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            this.flowLayoutPanel1.Location = new Point(3, 445);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(794, 72);
            this.flowLayoutPanel1.TabIndex = 1;

            // checkBox1
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new Padding(6, 10, 6, 0);
            this.checkBox1.Size = new Size(70, 30);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Stretch";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);

            // showButton
            this.showButton.AutoSize = true;
            this.showButton.Location = new Point(79, 3);
            this.showButton.Name = "showButton";
            this.showButton.Padding = new Padding(6, 4, 6, 4);
            this.showButton.Size = new Size(110, 33);
            this.showButton.TabIndex = 1;
            this.showButton.Text = "Show a picture";
            this.showButton.UseVisualStyleBackColor = true;
            this.showButton.Click += new EventHandler(this.showButton_Click);

            // clearButton
            this.clearButton.AutoSize = true;
            this.clearButton.Location = new Point(195, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Padding = new Padding(6, 4, 6, 4);
            this.clearButton.Size = new Size(115, 33);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Clear the picture";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new EventHandler(this.clearButton_Click);

            // backgroundButton
            this.backgroundButton.AutoSize = true;
            this.backgroundButton.Location = new Point(316, 3);
            this.backgroundButton.Name = "backgroundButton";
            this.backgroundButton.Padding = new Padding(6, 4, 6, 4);
            this.backgroundButton.Size = new Size(150, 33);
            this.backgroundButton.TabIndex = 3;
            this.backgroundButton.Text = "Set the background color";
            this.backgroundButton.UseVisualStyleBackColor = true;
            this.backgroundButton.Click += new EventHandler(this.backgroundButton_Click);

            // closeButton
            this.closeButton.AutoSize = true;
            this.closeButton.Location = new Point(472, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Padding = new Padding(6, 4, 6, 4);
            this.closeButton.Size = new Size(70, 33);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new EventHandler(this.closeButton_Click);

            // openFileDialog1
            this.openFileDialog1.FileName = "openFileDialog1";

            // Form1
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 520);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Picture Viewer";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
