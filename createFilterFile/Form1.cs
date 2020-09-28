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
                badwords.Add("motherfucker");
                badwords.Add("fuck");
                badwords.Add("fucking");
                badwords.Add(" ass");
                badwords.Add("asshole");
                badwords.Add(" ass.");
                badwords.Add(" ass!");
                badwords.Add(" ass?");
                badwords.Add(" ass ");
                badwords.Add("pussy");
                badwords.Add("dick");
                badwords.Add("bastard");
                badwords.Add("dammit");
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

                // load the entire text file into an array
                do
                {
                    data.Add(myFilterFile.ReadLine());
                } while (!myFilterFile.EndOfStream);
                myFilterFile.Close();

                // check each line of the array for a bad word, then rewind to grab the timestamp. Then restore the array counter to continue to next sentence.
                for (int i =0; i < data.Count; i++) {
                    if (FindWord(badwords, data[i]) == true)
                    {
                        string finalstring;
                        j = i; // store the current array position
                        while (data[i].Contains("-->") == false) i--;

                        // adjust data value to go up to next value if > 100
                        string[] parts = data[i].Split(',');
                        if (Int16.Parse(parts[2]) > 100)
                        {
                            string[] parts2 = parts[1].Split(':');
                            int value = Int16.Parse(parts2[2]) + 1;
                            string finalvalue = value.ToString("D2");
                            finalstring = parts[0] + "," + parts2[0] + ":" + parts2[1] + ":" + finalvalue + ",000";
                        }
                        else
                            finalstring = data[i];

//                        badwordlist.Add("mute;" + data[i]);
                        badwordlist.Add("mute;" + finalstring);
                        i = j; // restore the current array position 
                    }
                }

                // write out the filter file
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

        // Search the submitted string for all of the badwords in the badword array
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
