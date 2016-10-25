using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;

namespace Educational_Youtube_Saver
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> URLs = new Dictionary<string, string>();
        int videoCount = 0;





        public void allDone()
        {
            WaitingLoading loadingScreen = new WaitingLoading("Downloading Video Please wait...");
            if (URLs.Count != 0)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    loadingScreen.Owner = this;
                    loadingScreen.Show();
                });
            }
            if (URLs.Count == 0)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                });
            }
            while (true)
            {
                if (videoCount != 0)
                {
                    if (videoCount >= URLs.Count)
                    {
                        URLs.Clear();
                        this.Invoke((MethodInvoker)delegate() { loadingScreen.Close(); });
                        MessageBox.Show("All downloads complete, right click on any song to open the downloads folder. You may also download more videos");
                        videoCount = 0;
                        this.Invoke((MethodInvoker)delegate()
                        {
                            button1.Enabled = true;
                            button2.Enabled = true;
                            button3.Enabled = true;
                            button4.Enabled = true;
                        });
                        break;
                    }
                }
            }
        }
        public void executeMP3(KeyValuePair<string, string> entry)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                //START PROC
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = Environment.CurrentDirectory,
                        FileName = @"MP3.exe",
                        Arguments = entry.Key,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                proc.WaitForExit();
                videoCount++;
            };
            bw.RunWorkerCompleted += delegate
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    //MessageBox.Show(entry.Value);
                    listBox1.Items.Remove(entry.Value);
                    listBox1.Items.Add("DOWNLOADED COMPLETE FOR " + entry.Value);
                });
            };
            bw.RunWorkerAsync();
        }
        public void executeVideo(KeyValuePair<string, string> entry)
        {
          var bw = new BackgroundWorker();
          bw.DoWork += delegate {
            //START PROC
              var proc = new Process
              {
                  StartInfo = new ProcessStartInfo
                  {
                      WorkingDirectory = Environment.CurrentDirectory,
                      FileName = @"Video.exe",
                      Arguments = entry.Key,
                      UseShellExecute = false,
                      RedirectStandardOutput = false,
                      CreateNoWindow = true
                  }
              };
              proc.Start();
              proc.WaitForExit();
              videoCount++;
          };
          bw.RunWorkerCompleted += delegate {
              this.Invoke((MethodInvoker)delegate()
              {
                  //MessageBox.Show(entry.Value);
                  listBox1.Items.Remove(entry.Value);
                  listBox1.Items.Add("DOWNLOADED COMPLETE FOR " + entry.Value);
              });
          };
          bw.RunWorkerAsync();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            foreach (KeyValuePair<string, string> entry in URLs)//KEY IS URL...
            {
                executeVideo(entry);
            }
            BackgroundWorker bw1 = new BackgroundWorker();
            bw1.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
            {
                allDone();//Listens for program end...
            });
            bw1.RunWorkerAsync();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button1.Enabled = false;
            button4.Enabled = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
            {
                foreach (KeyValuePair<string, string> entry in URLs)//KEY IS URL...
                {
                    executeMP3(entry);
                }
                Thread.Sleep(1000);
            });
            BackgroundWorker bw1 = new BackgroundWorker();
            bw1.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
            {
                allDone();
            });
            bw.RunWorkerAsync();
            bw1.RunWorkerAsync();
        }











        public Form1()
        {
            InitializeComponent();
        }
        #region Private Members
        private ContextMenuStrip listboxContextMenu;
        #endregion
        private void button3_Click(object sender, EventArgs e)
        {
            WaitingLoading loadingScreen = new WaitingLoading("Loading Video Information. This may take a few seconds...");
            string URL = textBox1.Text;
            if (!URL.Contains("youtube.com"))
            {
                MessageBox.Show("Your link must be a valid youtube.com URL, all other links will fail.");
            }
            else
            {
                loadingScreen.Show(this);

                textBox1.Text = string.Empty;
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = Environment.CurrentDirectory,// working directory
                        FileName = @"Title.exe ",
                        Arguments = URL,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = true
                    }
                };
                proc.Start();
                proc.WaitForExit();
                loadingScreen.Hide();
                String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EYS\";
                string line = File.ReadAllText(path + @"\temp");
                File.Delete(path + @"\temp");
                if (listBox1.Items.Contains(line) || URLs.ContainsKey(line))
                {
                    MessageBox.Show("List already contains a video with the same title.");
                }
                else
                {
                    listBox1.Items.Add(line);
                    URLs.Add(URL, line);//URL then title...
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            //assign a contextmenustrip
            listboxContextMenu = new ContextMenuStrip();
            listboxContextMenu.Opening += new CancelEventHandler(listboxContextMenu_Opening);
            listBox1.ContextMenuStrip = listboxContextMenu;
            listboxContextMenu.ItemClicked += new ToolStripItemClickedEventHandler(apples);
            this.ActiveControl = listBox1;
        }
        private void listboxContextMenu_Opening(object sender, CancelEventArgs e)
        {
            listboxContextMenu.Items.Clear();
            for (int i = 0; i < listboxContextMenu.Items.Count; i++)
            {
                listboxContextMenu.Items.RemoveAt(i);
            }
            listboxContextMenu.Items.Add(string.Format("Open Downloads Folder"));
        }
        private void apples(object sender, EventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\EYS\";
            Process.Start("explorer.exe", path);

        }
        private void button4_Click(object sender, EventArgs e)
            {
                if (listBox1.SelectedIndex != -1)
                {
                    try
                    {
                        var item = URLs.First(kvp => kvp.Value == listBox1.SelectedItem.ToString());
                        URLs.Remove(item.Key);
                    }
                    catch (Exception) { }
                    listBox1.Items.Remove(listBox1.SelectedItem.ToString());
                }
            }
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                //MessageBox.Show("ASDF");
                var openURL = URLs.FirstOrDefault(x => x.Value == listBox1.SelectedItem.ToString()).Key;
                Process.Start(openURL);
            }



        }
        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listBox1.IndexFromPoint(e.Location);
                if (item >= 0)
                {
                    listBox1.SelectedIndex = item;
                    listboxContextMenu.Show(listBox1, e.Location);
                }
            }
        }
    }
}
