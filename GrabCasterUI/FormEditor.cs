using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace GrabCasterUI
{
    public partial class FormEditor : Form
    {
        public string filename { get; set; }

        public FormEditor()
        {
            InitializeComponent();
        }

        private void FormEditor_Load(object sender, EventArgs e)
        {

            textBox1.Text = File.ReadAllText(filename);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (Global.MessageBoxForm("Save the configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No) return;
            File.WriteAllText(filename, textBox1.Text);
            this.Close();
        }
    }
}
