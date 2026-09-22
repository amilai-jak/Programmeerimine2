using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // Esimene ja teine klikitud silt
        private Label firstClicked = null;
        private Label secondClicked = null;

        // Kaks korda kasutatavad ikoonid
        private List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }

        // Jagab igale sildile juhusliku ikooni (iga ikoon tapselt kaks korda)
        private void AssignIconsToSquares()
        {
            var remaining = new List<string>(icons);

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    int randomNumber = random.Next(remaining.Count);

                    iconLabel.Text = remaining[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;

                    remaining.RemoveAt(randomNumber);
                }
            }
        }

        // Sildi klikk - pöörab ikooni ümber ja otsib paari
        private void label_Click(object sender, EventArgs e)
        {
            // Kui taimer töötab, ei tohi uusi sildid klikkida
            if (timer1.Enabled == true)
            {
                return;
            }

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // Juba ümberpööratud silt
                if (clickedLabel.ForeColor == Color.Black)
                {
                    return;
                }

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                timer1.Start();
            }
        }

        // Kui kaks ikooni ei klapi, pöörab need tagasi
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        // Kontrollib, kas koik paarid on leitud
        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                    {
                        return;
                    }
                }
            }

            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }
    }
}
