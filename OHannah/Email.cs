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
    public partial class Email : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        SpeechRecognitionEngine engine2;

        public static string address;
        public static string password;
        public Email()
        {
            InitializeComponent();
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
            textBox3.Text += speech + "\r\n";
            switch (speech)
            {
                case "use default mail id":
                    textBox1.Text = "ohannah477@gmail.com";
                    textBox2.Text = "ohannah47";
                    button1.PerformClick();
                    break;
                case "yes":
                    textBox1.Text = "";
                    AddEmail();
                    break;
                case "no":
                    AddPass();
                    break;
                case "add id":
                    AddEmail();
                    break;
                case "add password":
                    AddPass();
                    break;
                case "log in":
                    button1.PerformClick();
                    break;
                case "go back":
                case "back":
                    button2.PerformClick();
                    break;
            }
        }

        private void Email_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = null;
            label5.Text = null;
            button1.Focus();
            if (textBox1.Text.Equals("") && textBox2.Text.Equals(""))
            {
                label4.Text = "This is required";
                label5.Text = "This is required";
                //ohannah.Speak("Mail id is required");
                //ohannah.Speak("Password is required");
            }
            if (textBox1.Text.Equals(""))
            {
                label4.Text = "This is required";
                //ohannah.Speak("Mail id is required");
            }
            else
            { }
            if (textBox2.Text.Equals(""))
            {
                label5.Text = "This is required";
                //ohannah.Speak("Password is required");
            }
            else
            { }
            if (!textBox1.Text.Equals("") && !textBox2.Text.Equals(""))
            {
                if (IsValidEmail(textBox1.Text))
                {
                    try
                    {
                        address = textBox1.Text.Trim();
                        password = textBox2.Text.Trim();
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(Email.address,Email.password);
                        //MessageBox.Show(address + password);
                        //ohannah4777 ohannah47
                        MailMessage msg = new MailMessage(address, "faysal4777@gmail.com", "Test", "Test");
                        client.Send(msg);
                        //MessageBox.Show("Logged in!");
                        address = textBox1.Text;//.Trim();
                        password = textBox2.Text;//.Trim();
                        ohannah.Speak("Logging in");
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("UserId and Password does not match.\n" + ex.Message);
                        ohannah.Speak("UserId and Password does not match.");
                        return;
                    }
                    this.ohannah.SpeakAsyncCancelAll();
                    engine.Dispose();
                    this.Dispose();
                    MailBox m = new MailBox();
                    m.Show();
                }
                else
                {
                    //MessageBox.Show("Email format is not valid");
                    ohannah.Speak("Email format is not valid");
                }

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
            Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Email_Load(object sender, EventArgs e)
        {
            textBox1.Text = LogIn.email;
            ohannah.Speak("I have added your email. Would you like to change it?");
        }

        void AddEmail()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your mail id");
            textBox1.Focus();
            //TextBox1.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine2_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsEmail.txt");
                texts.Add(commands);
                Grammar words = new Grammar(new GrammarBuilder(texts));
                engine2.LoadGrammar(words);
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
            textBox3.Text += speech + "\r\n";
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
                case "add password":
                    engine2.Dispose();
                    AddPass();
                    break;
                case "login":
                case "log in":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    button1.PerformClick();
                    break;
                case "back":
                case "go back":
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
            }

        }


        void AddPass()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your password");
            textBox2.Focus();
            //TextBox2.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine3_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsEmail.txt");
                texts.Add(commands);
                Grammar words = new Grammar(new GrammarBuilder(texts));
                engine2.LoadGrammar(words);
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
            textBox3.Text += speech + "\r\n";
            switch (speech)
            {
                case "clear":
                    textBox2.Text = "";
                    break;
                case "erase":
                    if (textBox2.Text.Length > 0)
                    {
                        textBox2.Text = textBox2.Text.Substring(0, (textBox2.Text.Length - 1));
                    }
                    break;
                case "a":
                    textBox2.Text += "a";
                    break;
                case "b":
                    textBox2.Text += "b";
                    break;
                case "c":
                    textBox2.Text += "c";
                    break;
                case "d":
                    textBox2.Text += "d";
                    break;
                case "e":
                    textBox2.Text += "e";
                    break;
                case "f":
                    textBox2.Text += "f";
                    break;
                case "g":
                    textBox2.Text += "g";
                    break;
                case "h":
                    textBox2.Text += "h";
                    break;
                case "i":
                    textBox2.Text += "i";
                    break;
                case "j":
                    textBox2.Text += "j";
                    break;
                case "k":
                    textBox2.Text += "k";
                    break;
                case "l":
                    textBox2.Text += "l";
                    break;
                case "m":
                    textBox2.Text += "m";
                    break;
                case "n":
                    textBox2.Text += "n";
                    break;
                case "o":
                    textBox2.Text += "o";
                    break;
                case "p":
                    textBox2.Text += "p";
                    break;
                case "q":
                    textBox2.Text += "q";
                    break;
                case "r":
                    textBox2.Text += "r";
                    break;
                case "s":
                    textBox2.Text += "s";
                    break;
                case "t":
                    textBox2.Text += "t";
                    break;
                case "u":
                    textBox2.Text += "u";
                    break;
                case "v":
                    textBox2.Text += "v";
                    break;
                case "w":
                    textBox2.Text += "w";
                    break;
                case "x":
                    textBox2.Text += "x";
                    break;
                case "y":
                    textBox2.Text += "y";
                    break;
                case "z":
                    textBox2.Text += "z";
                    break;
                case "one":
                    textBox2.Text += "1";
                    break;
                case "two":
                    textBox2.Text += "2";
                    break;
                case "three":
                    textBox2.Text += "3";
                    break;
                case "four":
                    textBox2.Text += "4";
                    break;
                case "five":
                    textBox2.Text += "5";
                    break;
                case "six":
                    textBox2.Text += "6";
                    break;
                case "seven":
                    textBox2.Text += "7";
                    break;
                case "eight":
                    textBox2.Text += "8";
                    break;
                case "nine":
                    textBox2.Text += "9";
                    break;
                case "zero":
                    textBox2.Text += "0";
                    break;
                case "dot":
                    textBox2.Text += ".";
                    break;
                case "at":
                    textBox2.Text += "@";
                    break;
                case "star":
                    textBox2.Text += "*";
                    break;
                case "hash":
                    textBox2.Text += "#";
                    break;
                case "dollar":
                    textBox2.Text += "$";
                    break;
                case "parcent":
                    textBox2.Text += "%";
                    break;
                case "show":
                case "hide":
                case "show password":
                case "hide password":
                    button3.PerformClick();
                    break;
                case "login":
                case "log in":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    button1.PerformClick();
                    break;

                case "add id":
                    engine2.Dispose();
                    AddEmail();
                    break;
                case "back":
                case "go back":
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox2.PasswordChar.Equals('*'))
            {
                textBox2.PasswordChar = '\0';
                button3.Text = "Hide";
            }
            else
            {
                textBox2.PasswordChar = '*';
                button3.Text = "Show";
            }
        }



    }
}
