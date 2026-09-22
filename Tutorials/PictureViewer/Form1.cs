using System;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // "Show a picture" nupp - avab failidialoogi ja naitab valitud pilti
        private void showButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }

        // "Stretch" märkeruut - venitab pildi akna suuruseks
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        // "Clear the picture" nupp - tyhjendab pildi
        private void clearButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        // "Set the background color" nupp - muudab pildi taustavärvi
        private void backgroundButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
            }
        }

        // "Close" nupp - sulgeb vormi
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
