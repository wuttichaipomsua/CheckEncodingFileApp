using CheckEncodingFileApp.SimpleHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CheckEncodingFileApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    bool isNotUTF8 = false;

                    // Clear path textbox
                    textBox1.Text = "";
                    textBox1.Text = fbd.SelectedPath;

                    // Clear result textbox
                    richTextBox1.Clear();

                    string[] files = Directory.GetFiles(fbd.SelectedPath,"*.sql",SearchOption.AllDirectories);

                    if (files.Length>0)
                    {
                        Encoding utf8 = Encoding.UTF8;
                        Encoding t;
                        foreach (var item in files)
                        {
                            string endcode = "";
                            try
                            {
                                t = FileEncoding.DetectFileEncoding(item, utf8);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error " + ex.StackTrace);
                                return;
                            }

                            if (!t.EncodingName.Contains("UTF-8"))
                            {
                                isNotUTF8 = true;
                            }

                            endcode = t.EncodingName;

                            richTextBox1.SelectionColor = (t.EncodingName.Contains("UTF-8")?Color.White:Color.Red);
                            richTextBox1.AppendText(item + "\t" + endcode + System.Environment.NewLine);
                        }

                        if (isNotUTF8)
                        {
                            MessageBox.Show("Some files are not UTF-8 Encoding");
                        }
                        else
                        {
                            MessageBox.Show("All files are UTF-8 Encoding");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not found sql file");
                    }

                }
            }

        }
    }
}
