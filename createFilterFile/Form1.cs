using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace createFilterFile
{
    public partial class Form1 : Form
    {
        List<string> badwords = new List<string>();
        List<string> badwordlist = new List<string>();

        public Form1()
        {
            InitializeComponent();
            badwords.Add("shit");
            badwords.Add("bullshit");
            badwords.Add("damn");
            badwords.Add("bitch");
            badwords.Add("fuck");
            badwords.Add(" ass");
            badwords.Add("asshole");
            badwords.Add(" ass.");
            badwords.Add(" ass!");
            badwords.Add(" ass?");
            badwords.Add(" ass ");
            badwords.Add("pussy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader myFilterFile = new StreamReader(openFileDialog1.FileName);
                do
                {
                   
                    string number = myFilterFile.ReadLine();
                    string timestamp = myFilterFile.ReadLine();
                    string sentence = myFilterFile.ReadLine();
                    string space = myFilterFile.ReadLine();
                    string nextspace = "";

                    if (space != "")
                        nextspace = myFilterFile.ReadLine();

                    if (FindWord(badwords, sentence) == true)
                        badwordlist.Add("mute;" + timestamp);

                    if(FindWord(badwords, space) == true)
                        badwordlist.Add("mute;" + timestamp);

                } while (!myFilterFile.EndOfStream);
                myFilterFile.Close();

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(textBox1.Text))
                {
                    file.WriteLine("1");
                    for (int i = 0; i < badwordlist.Count; i++)
                    {
                        file.WriteLine(badwordlist[i]); 
                    }
                }
                badwordlist.Clear();

                MessageBox.Show("All Done!!");          }
        }
        private Boolean FindWord(List<string> badwords, string sentence)
        {
            if (sentence == null)
                return false;

            for (int i=0;i<badwords.Count;i++)
            {
                if (sentence.ToUpper().Contains(badwords[i].ToUpper()) == true)
                    return true;
            }
            return false;
        }
    }
}
