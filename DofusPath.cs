using System;
using System.Windows.Forms;
using System.IO;
namespace MultiWindows
{
    public partial class DofusPath : Form
    {
        public DofusPath()
        {
            InitializeComponent();
        }

        private void flatButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog() {  Multiselect = false, Title = "Open File | Dofus Retro.exe", Filter = "Dofus Retro.exe|Dofus Retro.exe" };
            if (OFD.ShowDialog() == DialogResult.OK)
                flatTextBox1.Text = OFD.FileName;
        }

        private void flatButton2_Click(object sender, EventArgs e)
        {
            if (flatTextBox1.Text != string.Empty && File.Exists(flatTextBox1.Text)) { 
                File.WriteAllText("DofusPath.txt", flatTextBox1.Text);
                Close();
            }
        }

        private void flatClose1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
