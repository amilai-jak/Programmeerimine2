using Timer = System.Windows.Forms.Timer;

namespace MathQuiz
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private Label timeLabel;
        private Label plusLeftLabel;
        private Label plusRightLabel;
        private Label minusLeftLabel;
        private Label minusRightLabel;
        private Label timesLeftLabel;
        private Label timesRightLabel;
        private Label dividedLeftLabel;
        private Label dividedRightLabel;
        private Label plusLabel;
        private Label minusLabel;
        private Label timesLabel;
        private Label dividedLabel;
        private Label equalsLabel1;
        private Label equalsLabel2;
        private Label equalsLabel3;
        private Label equalsLabel4;
        private NumericUpDown sum;
        private NumericUpDown difference;
        private NumericUpDown product;
        private NumericUpDown quotient;
        private Button startButton;
        private Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private Label MakeLabel(string text, int x, int y, int w = 50)
        {
            var label = new Label();
            label.AutoSize = false;
            label.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
            label.Location = new Point(x, y);
            label.Name = "label_" + text + "_" + x;
            label.Size = new Size(w, 40);
            label.TabIndex = 0;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            return label;
        }

        private NumericUpDown MakeAnswerBox(int x, int y)
        {
            var box = new NumericUpDown();
            box.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
            box.Location = new Point(x, y);
            box.Maximum = 200M;
            box.Name = "answerBox_" + x;
            box.Size = new Size(100, 40);
            box.TabIndex = 1;
            box.Enter += new EventHandler(this.answer_Enter);
            return box;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.timeLabel = new Label();
            this.startButton = new Button();
            this.timer1 = new Timer(this.components);

            this.SuspendLayout();

            // timeLabel
            this.timeLabel.BorderStyle = BorderStyle.FixedSingle;
            this.timeLabel.Font = new Font("Segoe UI", 18F, FontStyle.Regular);
            this.timeLabel.Location = new Point(240, 20);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new Size(200, 40);
            this.timeLabel.TabIndex = 0;

            // Rea 1: liitmine
            this.plusLeftLabel = MakeLabel("?", 20, 80);
            this.plusLabel = MakeLabel("+", 70, 80, 30);
            this.plusRightLabel = MakeLabel("?", 100, 80);
            this.equalsLabel1 = MakeLabel("=", 150, 80, 30);
            this.sum = MakeAnswerBox(190, 80);

            // Rea 2: lahutamine
            this.minusLeftLabel = MakeLabel("?", 20, 140);
            this.minusLabel = MakeLabel("-", 70, 140, 30);
            this.minusRightLabel = MakeLabel("?", 100, 140);
            this.equalsLabel2 = MakeLabel("=", 150, 140, 30);
            this.difference = MakeAnswerBox(190, 140);

            // Rea 3: korrutamine
            this.timesLeftLabel = MakeLabel("?", 20, 200);
            this.timesLabel = MakeLabel("x", 70, 200, 30);
            this.timesRightLabel = MakeLabel("?", 100, 200);
            this.equalsLabel3 = MakeLabel("=", 150, 200, 30);
            this.product = MakeAnswerBox(190, 200);

            // Rea 4: jagamine
            this.dividedLeftLabel = MakeLabel("?", 20, 260);
            this.dividedLabel = MakeLabel("/", 70, 260, 30);
            this.dividedRightLabel = MakeLabel("?", 100, 260);
            this.equalsLabel4 = MakeLabel("=", 150, 260, 30);
            this.quotient = MakeAnswerBox(190, 260);

            // startButton
            this.startButton.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
            this.startButton.Location = new Point(150, 320);
            this.startButton.Name = "startButton";
            this.startButton.Size = new Size(160, 40);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start the quiz";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new EventHandler(this.startButton_Click);

            // timer1
            this.timer1.Interval = 1000;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);

            // Form1
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(460, 390);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.plusLeftLabel);
            this.Controls.Add(this.plusLabel);
            this.Controls.Add(this.plusRightLabel);
            this.Controls.Add(this.equalsLabel1);
            this.Controls.Add(this.sum);
            this.Controls.Add(this.minusLeftLabel);
            this.Controls.Add(this.minusLabel);
            this.Controls.Add(this.minusRightLabel);
            this.Controls.Add(this.equalsLabel2);
            this.Controls.Add(this.difference);
            this.Controls.Add(this.timesLeftLabel);
            this.Controls.Add(this.timesLabel);
            this.Controls.Add(this.timesRightLabel);
            this.Controls.Add(this.equalsLabel3);
            this.Controls.Add(this.product);
            this.Controls.Add(this.dividedLeftLabel);
            this.Controls.Add(this.dividedLabel);
            this.Controls.Add(this.dividedRightLabel);
            this.Controls.Add(this.equalsLabel4);
            this.Controls.Add(this.quotient);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Math Quiz";
            this.ResumeLayout(false);
        }
    }
}
