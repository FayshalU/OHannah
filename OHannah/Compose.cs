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
using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace OHannah
{
    public partial class Compose : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        SpeechRecognitionEngine engine2;
        SpeechRecognitionEngine engine3;

        MailBox mail;
        string[] files = null, paths=null;
        public Compose(MailBox m)
        {
            InitializeComponent();
            mail = m;
            try
            {

                engine.SpeechRecognized += engine_SpeechRecognized;

                this.LoadGrammer();
                //engine.LoadGrammar(new DictationGrammar());
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
            textBox5.Text += speech + "\r\n";
            switch (speech)
            {
                    
                case "go back":
                case "back":
                    button2.PerformClick();
                    break;
                case "add id":
                    AddId();
                    break;
                case "add subject":
                    this.AddSubject();
                    break;
                case "add body":
                case "add message":
                    this.AddBody();
                    break;
                case "add an attachment":
                case "add a file": 
                    button3.PerformClick();
                    break;
                case "send":
                case "send email":
                    button1.PerformClick();
                    break;
            }
        }

        private void Compose_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                if (IsValidEmail(textBox1.Text))
                {
                    label4.Text = null;
                    try
                    {
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(Email.address, Email.password);
                        MailMessage msg = new MailMessage(Email.address, textBox1.Text, textBox2.Text, textBox3.Text);
                        if (paths != null)
                        {
                            foreach (string path in paths)
                            {
                                msg.Attachments.Add(new Attachment(path));
                            }
                        }
                        client.Send(msg);
                        //MessageBox.Show("Mail sent!");
                        ohannah.Speak("Mail sent!");
                        this.button2.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("UserId and Password does not match.\n" + ex.Message);
                        ohannah.Speak("UserId and Password does not match.");
                    }
                }
                else
                {
                    //MessageBox.Show("Email format is not valid");
                    ohannah.Speak("Email format is not valid.");
                }

            }
            else
            {
                label4.Text = "This is required";
                //ohannah.Speak("Mail id is required");
            }
        }
        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            this.Dispose();
            mail.engine.RecognizeAsync(RecognizeMode.Multiple);
            mail.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ohannah.SpeakAsync("Select a file to send");
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            if (of.ShowDialog() == DialogResult.OK)
            {
                files = of.SafeFileNames;
                paths = of.FileNames;
                foreach (string file in files)
                {
                    textBox4.Text += file + "\t";
                }
                ohannah.Speak("Files added.");
            }
        }

        private void Compose_Load(object sender, EventArgs e)
        {
            //ohannah.SpeakAsync("please spell receivers mail id");
            AddId();
            
        }

        void AddId()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell receivers mail id");
            textBox1.Focus();
            //TextBox1.Text = "";

            engine3 = new SpeechRecognitionEngine();
            try
            {

                engine3.SpeechRecognized += engine4_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsCompose.txt");
                texts.Add(commands);
                Grammar words = new Grammar(new GrammarBuilder(texts));
                engine3.LoadGrammar(words);
                engine3.SetInputToDefaultAudioDevice();
                engine3.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception e)
            {
                MessageBox.Show("Can not find the command.\n" + e.Message);
            }
        }


        void engine4_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox5.Text += speech + "\r\n";
            switch (speech)
            {
                case "clear":
                    textBox1.Text = "";
                    break;
                case "erase":
                    if (textBox1.Text.Length > 0)
                    {
                        textBox1.Text = textBox1.Text.Substring(0, (textBox1.Text.Length - 1));
                    }
                    break;
                case "a":
                    textBox1.Text += "a";
                    break;
                case "b":
                    textBox1.Text += "b";
                    break;
                case "c":
                    textBox1.Text += "c";
                    break;
                case "d":
                    textBox1.Text += "d";
                    break;
                case "e":
                    textBox1.Text += "e";
                    break;
                case "f":
                    textBox1.Text += "f";
                    break;
                case "g":
                    textBox1.Text += "g";
                    break;
                case "h":
                    textBox1.Text += "h";
                    break;
                case "i":
                    textBox1.Text += "i";
                    break;
                case "j":
                    textBox1.Text += "j";
                    break;
                case "k":
                    textBox1.Text += "k";
                    break;
                case "l":
                    textBox1.Text += "l";
                    break;
                case "m":
                    textBox1.Text += "m";
                    break;
                case "n":
                    textBox1.Text += "n";
                    break;
                case "o":
                    textBox1.Text += "o";
                    break;
                case "p":
                    textBox1.Text += "p";
                    break;
                case "q":
                    textBox1.Text += "q";
                    break;
                case "r":
                    textBox1.Text += "r";
                    break;
                case "s":
                    textBox1.Text += "s";
                    break;
                case "t":
                    textBox1.Text += "t";
                    break;
                case "u":
                    textBox1.Text += "u";
                    break;
                case "v":
                    textBox1.Text += "v";
                    break;
                case "w":
                    textBox1.Text += "w";
                    break;
                case "x":
                    textBox1.Text += "x";
                    break;
                case "y":
                    textBox1.Text += "y";
                    break;
                case "z":
                    textBox1.Text += "z";
                    break;
                case "one":
                    textBox1.Text += "1";
                    break;
                case "two":
                    textBox1.Text += "2";
                    break;
                case "three":
                    textBox1.Text += "3";
                    break;
                case "four":
                    textBox1.Text += "4";
                    break;
                case "five":
                    textBox1.Text += "5";
                    break;
                case "six":
                    textBox1.Text += "6";
                    break;
                case "seven":
                    textBox1.Text += "7";
                    break;
                case "eight":
                    textBox1.Text += "8";
                    break;
                case "nine":
                    textBox1.Text += "9";
                    break;
                case "zero":
                    textBox1.Text += "0";
                    break;
                case "dot":
                    textBox1.Text += ".";
                    break;
                case "at":
                    textBox1.Text += "@";
                    break;
                case "star":
                    textBox1.Text += "*";
                    break;
                case "hash":
                    textBox1.Text += "#";
                    break;
                case "dollar":
                    textBox1.Text += "$";
                    break;
                case "parcent":
                    textBox1.Text += "%";
                    break;
                case "add subject":
                    engine3.Dispose();
                    AddSubject();
                    break;
                case "add body":
                case "add message ":
                    engine3.Dispose();
                    AddBody();
                    break;
                case "add an attachment":
                case "add a file":
                    engine3.Dispose();
                    button3.PerformClick();
                    break;
                case "send":
                case "send email":
                    engine3.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    button1.PerformClick();
                    break;
                case "back":
                case "go back":
                    engine3.Dispose();
                    button2.PerformClick();
                    break;
            }

        }


        void AddSubject()
        {
            //ohannah.SpeakAsync("Adding subject");
            textBox2.Focus();
            ohannah.Speak("Adding subject");
            ohannah.SpeakAsyncCancelAll();
            engine.RecognizeAsyncCancel();
            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine2_SpeechRecognized;
                engine2.LoadGrammar(new DictationGrammar());
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(new Choices("add body","add message","add an attachment","add a file","add id","clear","erase"));
                engine2.LoadGrammar(new Grammar(grammarBuilder));
                
                engine2.SetInputToDefaultAudioDevice();
                engine2.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception e)
            {
                MessageBox.Show("Can not find the command.\n" + e.Message);
            }

        }

        void engine2_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox5.Text += speech + "\r\n";

            if ((e.Result.Text == "add body") || (e.Result.Text == "add message"))
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                ohannah.Resume();
                ohannah.Speak("Subject added");
                this.AddBody();
            }
            else if ((e.Result.Text == "add id"))
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                ohannah.Resume();
                ohannah.Speak("Subject added");
                AddId();
            }
            else if ((e.Result.Text == "clear"))
            {
                textBox2.Text = "";
            }
            else if ((e.Result.Text == "erase"))
            {
                if (textBox2.Text.Length > 0)
                {
                    textBox2.Text = textBox2.Text.Substring(0, (textBox2.Text.Length - 1));
                }
            }
            else if ((e.Result.Text == "add an attachment") || (e.Result.Text == "add a file"))
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                ohannah.Resume();
                ohannah.Speak("Subject added");
                button3.PerformClick();
            }
            else
            {
                textBox2.Text += (speech + " ");
            }
        }

        void AddBody()
        {
            //ohannah.SpeakAsync("Adding body");
            textBox3.Focus();
            ohannah.Speak("Adding body");
            ohannah.SpeakAsyncCancelAll();
            engine.RecognizeAsyncCancel();
            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine3_SpeechRecognized;
                engine2.LoadGrammar(new DictationGrammar());
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(new Choices("add subject", "add an attachment", "add a file", "add id", "clear", "erase"));
                engine2.LoadGrammar(new Grammar(grammarBuilder));
                
                engine2.SetInputToDefaultAudioDevice();
                engine2.RecognizeAsync(RecognizeMode.Multiple);

            }
            catch (Exception e)
            {
                MessageBox.Show("Can not find the command.\n" + e.Message);
            }
        }

        void engine3_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox5.Text += speech + "\r\n";

            if ((e.Result.Text == "add subject"))
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                ohannah.Resume();
                ohannah.Speak("Body added");
                this.AddSubject();
            }
            else if ((e.Result.Text == "add id"))
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                ohannah.Resume();
                ohannah.Speak("Body added");
                AddId();
            }
            else if ((e.Result.Text == "clear"))
            {
                textBox3.Text = "";
            }
            else if ((e.Result.Text == "erase"))
            {
                if (textBox3.Text.Length > 0)
                {
                    textBox3.Text = textBox3.Text.Substring(0, (textBox3.Text.Length - 1));
                }
            }
            else if ((e.Result.Text == "add an attachment") || (e.Result.Text == "add a file"))
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                ohannah.Resume();
                ohannah.Speak("Body added");
                button3.PerformClick();
            }
            else
            {
                textBox3.Text += (speech+" ");
            }
        }

    }
}
