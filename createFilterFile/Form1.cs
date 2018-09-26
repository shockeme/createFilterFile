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

            string path = Directory.GetCurrentDirectory();
            string curFile = path + "\\badwords.txt";

            // either use a badwords.txt file or use this internal list. I prefer the file since you can add badwords without having to recompile
            if (File.Exists(curFile))
            {
                string text = System.IO.File.ReadAllText(curFile);
                String[] words = text.Split(',');
                foreach (string word in words)
                    badwords.Add(word);
            } else {
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
                 badwords.Add("dick");
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> data = new List<string>();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader myFilterFile = new StreamReader(openFileDialog1.FileName);
                int j = 0;
                do
                {
                    data.Add(myFilterFile.ReadLine());
                } while (!myFilterFile.EndOfStream);
                myFilterFile.Close();


                for (int i =0; i < data.Count; i++) {
                    if (FindWord(badwords, data[i]) == true)
                    {
                        j = i; // store the current array position
                        while (data[i].Contains("-->") == false) i--;
                        badwordlist.Add("mute;" + data[i]);
                        i = j; // restore the current array position 
                    }
                } 
            
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(textBox1.Text))
                {
                    file.WriteLine("1");
                    for (int i = 0; i < badwordlist.Count; i++)
                    {
                        file.WriteLine(badwordlist[i]); 
                    }
                }
                badwordlist.Clear();

                MessageBox.Show("All Done!!\r\nFilter File is: " + textBox1.Text);
            }
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
