using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSpam
{
    public partial class Form1 : Form
    {
        public static bool spam = false;

        public static int valid;
        public static int unverified;
        public static int invalid;

        public Form1()
        {
            InitializeComponent();
        }

        private void monoFlat_Toggle1_ToggledChanged()
        {

        }

        private void monoFlat_Button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists("tokens.txt"))
            {
                MessageBox.Show("Please drop a tokens.txt file with all your tokens line by line next to this executable.", "DSpam");
                return;
            }
            spam = true;

            new Thread(() =>
            {
                if (spam == true)
                {
                    while (spam == true)
                    {
                        using (var client = new WebClient())
                        {

                            using (var streamReader = File.OpenText("tokens.txt"))
                            {
                                var lines = streamReader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                foreach (var line in lines)
                                {
                                    try
                                    {
                                        var values = new NameValueCollection();
                                        values["content"] = message.Text; //message
                                        client.Headers["authorization"] = line; //token
                                        var response = client.UploadValues("https://discordapp.com/api/v6/channels/" + chanelid.Text + "/messages", values);
                                    }
                                    catch (WebException ex)
                                    {
                                        var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                                        if (resp.Contains("401: Unauthorized"))
                                        {
                                            MessageBox.Show("A Token is invalid, please recheck.", "DSpam");
                                        }
                                        else if (resp.Contains("You need to verify your account in order to perform this action."))
                                        {
                                            MessageBox.Show("A Token is unverified, please recheck.", "DSpam");
                                        }
                                        else if (resp.Contains("TOO MANY"))
                                        {
                                            Thread.Sleep(3000);
                                        }
                                        else
                                        {
                                            valid++;
                                            File.AppendAllText("verified.txt", line + Environment.NewLine);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }).Start();
        }

        private void monoFlat_Button2_Click(object sender, EventArgs e)
        {
            spam = false;
        }

        private void monoFlat_Button3_Click(object sender, EventArgs e)
        {
            string fileName = "tokens.txt";

            if (!File.Exists("tokens.txt"))
            {
                MessageBox.Show("Please drop a tokens.txt file with all your tokens line by line next to this executable.", "DSpam");
                return;
            }

            if (!File.Exists("verified.txt"))
            {
                File.Create("verified.txt");
            }

            if (!File.Exists("unverified.txt"))
            {
                File.Create("unverified.txt");
            }

            if (!File.Exists("invalid.txt"))
            {
                File.Create("invalid.txt");
            }

            File.WriteAllText("verified.txt", "");
            File.WriteAllText("unverified.txt", "");
            File.WriteAllText("invalid.txt", "");

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    using (var client = new WebClient())
                    {
                        var values = new System.Collections.Specialized.NameValueCollection();
                        values[""] = "";
                        client.Headers.Add("Authorization", line);

                        try
                        {
                            var response = client.UploadValues("https://discordapp.com/api/v6/invite/dsaadsdasdasddasd", values);
                        }
                        catch (WebException ex)
                        {
                            if (!File.Exists("verified.txt"))
                            {
                                File.Create("verified.txt");
                            }

                            if (!File.Exists("unverified.txt"))
                            {
                                File.Create("unverified.txt");
                            }

                            if (!File.Exists("invalid.txt"))
                            {
                                File.Create("invalid.txt");
                            }

                            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                            if (resp.Contains("401: Unauthorized"))
                            {
                                invalid++;
                                File.AppendAllText("invalid.txt", line + Environment.NewLine);
                            }
                            else if (resp.Contains("You need to verify your account in order to perform this action."))
                            {
                                unverified++;
                                File.AppendAllText("unverified.txt", line + Environment.NewLine);
                            }
                            else
                            {
                                valid++;
                                File.AppendAllText("verified.txt", line + Environment.NewLine);
                            }
                        }
                    }
                }

                int total = valid + invalid + unverified;

                /*
                Console.WriteLine("VERIFIED: " + valid);
                Console.WriteLine("UNVERIFIED: " + unverified);
                Console.WriteLine("INVALID: " + invalid);
                Console.WriteLine("TOTAL: " + total);
                Console.ReadLine();
                */

                MessageBox.Show("VERIFIED: " + valid + "\nUNVERIFIED: " + unverified + "\nINVALID: " + invalid + "\nTOTAL: " + total, "DSpam Checker");
                valid = 0;
                unverified = 0;
                invalid = 0;
            }
        }

        private void JOIN_Click(object sender, EventArgs e)
        {
            if (!File.Exists("tokens.txt"))
            {
                MessageBox.Show("Please drop a tokens.txt file with all your tokens line by line next to this executable.", "DSpam");
                return;
            }
            spam = true;

            new Thread(() =>
            {
                using (var client = new WebClient())
                {

                    using (var streamReader = File.OpenText("verified.txt"))
                    {
                        var lines = streamReader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in lines)
                        {
                            try
                            {
                                var values = new NameValueCollection();
                                client.Headers["authorization"] = line; //token
                                var response = client.UploadValues("https://discordapp.com/api/v6/invite/" + invite.Text, values);
                                MessageBox.Show("LOL");
                            }
                            catch (WebException ex)
                            {
                                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                                if (resp.Contains("401: Unauthorized"))
                                {
                                    MessageBox.Show("A Token is invalid, please recheck.", "DSpam");
                                }
                                else if (resp.Contains("You need to verify your account in order to perform this action."))
                                {
                                    MessageBox.Show("A Token is unverified, please recheck.", "DSpam");
                                }
                                else if (resp.Contains("Unknown"))
                                {
                                    MessageBox.Show("Unknown.", "DSpam");
                                    return;
                                }
                                else
                                {
                                }
                            }
                        }
                    }
                }
            }).Start();
        }

        private void monoFlat_HeaderLabel1_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void monoFlat_Panel1_Click(object sender, EventArgs e)
        {

        }
    }
}
