using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Xml;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace OHannah
{
    public partial class Inbox : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();

        MailBox mail;
        string subject, author, tag, summary,speak = null;
        public Inbox(MailBox m)
        {
            InitializeComponent();
            mail = m;
            try
            {

                engine.SpeechRecognized += engine_SpeechRecognized;

                this.LoadGrammer();
                engine.SetInputToDefaultAudioDevice();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                ohannah.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                //ohannah.SpeakCompleted += ohannah_SpeakCompleted;
                if (ohannah.State == SynthesizerState.Speaking)
                {
                    ohannah.SpeakAsyncCancelAll();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not find the command.\n" + e.Message);
            }
            this.GetFeeds();
        }

        void LoadGrammer()
        {
            Choices texts = new Choices();
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\emailCommands.txt");
            texts.Add(commands);
            Grammar words = new Grammar(new GrammarBuilder(texts));
            engine.LoadGrammar(words);
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox1.Text += speech + "\r\n";
            switch (speech)
            {
                case "go back":
                case "back":
                    button1.PerformClick();
                    break;
            }
        }

        private void Inbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void GetFeeds()
        {
            WebClient client = new WebClient();
            //XmlNodeList nodeList = default(XmlNodeList);
            XmlNode node1 = default(XmlNode);
            XmlNode node2 = default(XmlNode);
            XmlDocument xdoc = new XmlDocument();
            string response;

            try
            {
                client.Credentials = new NetworkCredential(Email.address, Email.password);
                response = Encoding.UTF8.GetString(client.DownloadData("https://mail.google.com/mail/feed/atom"));
                response = response.Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");
                xdoc.LoadXml(response);

                node1 = xdoc.SelectSingleNode("/feed/fullcount");
                Variables.mailcounter = Convert.ToInt16(node1.InnerText);
                //MessageBox.Show(Variables.mailcounter + " Mail");

                tag = xdoc.SelectSingleNode("/feed/tagline").InnerText;
                //MessageBox.Show("You have " + tag);
                //ohannah.Speak("You have " + tag);
                speak += ("You have " + tag);

                if (Variables.mailcounter > 0)
                {
                    node2 = xdoc.SelectSingleNode("feed").SelectSingleNode("entry");
                    subject = node2.SelectSingleNode("title").InnerText;
                    author = node2.SelectSingleNode("author").SelectSingleNode("name").InnerText;
                    summary = node2.SelectSingleNode("summary").InnerText;
                    //MessageBox.Show("Mail from: " + author + " Subject: " + subject);
                    //ohannah.Speak("Mail from: " + author + ". Subject: " + subject);
                    speak += (". Mail from: " + author + ". Subject: " + subject);
                }
                this.ShowMessage();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ShowMessage()
        {
            WebClient client = new WebClient();
            XmlDocument xdoc = new XmlDocument();
            string response = null;
            try
            {
                client.Credentials = new NetworkCredential(Email.address, Email.password);
                response = Encoding.UTF8.GetString(client.DownloadData("https://mail.google.com/mail/feed/atom"));
                response = response.Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");
                xdoc.LoadXml(response);

                if (Variables.mailcounter > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Subject");
                    dt.Columns.Add("Sender");
                    dt.Columns.Add("Summary");

                    foreach (XmlNode node in xdoc.SelectNodes("feed/entry"))
                    {
                        dt.Rows.Add(new object[] { node.SelectSingleNode("title").InnerText, node.SelectSingleNode("author").InnerText, node.SelectSingleNode("summary").InnerText });
                    }

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ohannah.SpeakAsync(speak);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            this.Dispose();
            mail.engine.RecognizeAsync(RecognizeMode.Multiple);
            mail.Show();
        }
    }
}
