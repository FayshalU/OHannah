using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace OHannah
{
    public partial class LogIn : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        //SqlConnection con;
        SpeechRecognitionEngine engine2;

        public static string userId;
        public static string name;
        public static string email;
        public static string Number;

        DataAccess data = new DataAccess();

        public LogIn()
        {
            InitializeComponent();
            

            try
            {


                engine.SpeechRecognized += engine_SpeechRecognized;

                this.LoadGrammer();
                //engine.LoadGrammar(new DictationGrammar());
                engine.SetInputToDefaultAudioDevice();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                //ohannah.SpeakCompleted += ohannah_SpeakCompleted;
                ohannah.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                if (ohannah.State == SynthesizerState.Speaking)
                {
                    ohannah.SpeakAsyncCancelAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not find the command.\n" + ex.Message);
            }
        }

        void LoadGrammer()
        {
            Choices texts = new Choices();
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\loginCommands.txt");
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
                case "login":
                case "log in":
                    Button1.PerformClick();
                    break;
                case "show":
                case "show password":
                case "hide":
                case "hide password":
                    button3.PerformClick();
                    break;
                case "add id":
                    AddId();
                    break;
                case "add password":
                    AddPass();
                    break;
                case "registration":
                case "go to registration":
                case "registar me":
                    Button2.PerformClick();
                    break;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label5.Visible = false;
            //engine.RecognizeAsync(RecognizeMode.Multiple);
            Button1.Focus();

            if (TextBox1.Text != "" && TextBox2.Text != "")
            {
                if (data.CheckUser(TextBox1.Text, TextBox2.Text))
                {
                    this.Hide();
                    userId = TextBox1.Text;
                    engine2.Dispose();
                    engine.RecognizeAsyncCancel();
                    MessageBox.Show("Logged in!");
                    Form1 f = new Form1();
                    f.Show();
                }
                else
                {
                    MessageBox.Show("User id and Password does not match","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else 
            {
                if(TextBox1.Text == "")
                {
                    label4.Visible = true;
                }
                if (TextBox2.Text == "")
                {
                    label5.Visible = true;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            engine.RecognizeAsyncStop();
            Registration f1 = new Registration();
            f1.Show();
        }
        

        private void LogIn_Load(object sender, EventArgs e)
        {
            //ohannah.SpeakAsync("Please enter your user id and password");
            AddId();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (TextBox2.PasswordChar.Equals('*'))
            {
                TextBox2.PasswordChar = '\0';
                button3.Text = "Hide";
            }
            else
            {
                TextBox2.PasswordChar = '*';
                button3.Text = "Show";
            }
        }

        void AddId()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your user id");
            TextBox1.Focus();
            //TextBox1.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine2_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabets.txt");
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
                    TextBox1.Text = "";
                    break;
                case "erase":
                    if (TextBox1.Text.Length > 0)
                    {
                        TextBox1.Text = TextBox1.Text.Substring(0, (TextBox1.Text.Length - 1));
                    }
                    break;
                case "a":
                    TextBox1.Text += "a";
                    break;
                case "b":
                    TextBox1.Text += "b";
                    break;
                case "c":
                    TextBox1.Text += "c";
                    break;
                case "d":
                    TextBox1.Text += "d";
                    break;
                case "e":
                    TextBox1.Text += "e";
                    break;
                case "f":
                    TextBox1.Text += "f";
                    break;
                case "g":
                    TextBox1.Text += "g";
                    break;
                case "h":
                    TextBox1.Text += "h";
                    break;
                case "i":
                    TextBox1.Text += "i";
                    break;
                case "j":
                    TextBox1.Text += "j";
                    break;
                case "k":
                    TextBox1.Text += "k";
                    break;
                case "l":
                    TextBox1.Text += "l";
                    break;
                case "m":
                    TextBox1.Text += "m";
                    break;
                case "n":
                    TextBox1.Text += "n";
                    break;
                case "o":
                    TextBox1.Text += "o";
                    break;
                case "p":
                    TextBox1.Text += "p";
                    break;
                case "q":
                    TextBox1.Text += "q";
                    break;
                case "r":
                    TextBox1.Text += "r";
                    break;
                case "s":
                    TextBox1.Text += "s";
                    break;
                case "t":
                    TextBox1.Text += "t";
                    break;
                case "u":
                    TextBox1.Text += "u";
                    break;
                case "v":
                    TextBox1.Text += "v";
                    break;
                case "w":
                    TextBox1.Text += "w";
                    break;
                case "x":
                    TextBox1.Text += "x";
                    break;
                case "y":
                    TextBox1.Text += "y";
                    break;
                case "z":
                    TextBox1.Text += "z";
                    break;
                case "one":
                    TextBox1.Text += "1";
                    break;
                case "two":
                    TextBox1.Text += "2";
                    break;
                case "three":
                    TextBox1.Text += "3";
                    break;
                case "four":
                    TextBox1.Text += "4";
                    break;
                case "five":
                    TextBox1.Text += "5";
                    break;
                case "six":
                    TextBox1.Text += "6";
                    break;
                case "seven":
                    TextBox1.Text += "7";
                    break;
                case "eight":
                    TextBox1.Text += "8";
                    break;
                case "nine":
                    TextBox1.Text += "9";
                    break;
                case "zero":
                    TextBox1.Text += "0";
                    break;
                case "dot":
                    TextBox1.Text += ".";
                    break;
                case "at":
                    TextBox1.Text += "@";
                    break;
                case "star":
                    TextBox1.Text += "*";
                    break;
                case "hash":
                    TextBox1.Text += "#";
                    break;
                case "dollar":
                    TextBox1.Text += "$";
                    break;
                case "parcent":
                    TextBox1.Text += "%";
                    break;
                case "add password":
                    engine2.Dispose();
                    AddPass();
                    break;
                case "login":
                case "log in":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                case "registration":
                case "go to registration":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;
                    
            }
        }

        void AddPass()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your password");
            TextBox2.Focus();
            //TextBox2.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine3_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabets.txt");
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
                    TextBox2.Text = "";
                    break;
                case "erase":
                    if (TextBox2.Text.Length > 0)
                    {
                        TextBox2.Text = TextBox2.Text.Substring(0, (TextBox2.Text.Length - 1));
                    }
                    break;
                case "a":
                    TextBox2.Text += "a";
                    break;
                case "b":
                    TextBox2.Text += "b";
                    break;
                case "c":
                    TextBox2.Text += "c";
                    break;
                case "d":
                    TextBox2.Text += "d";
                    break;
                case "e":
                    TextBox2.Text += "e";
                    break;
                case "f":
                    TextBox2.Text += "f";
                    break;
                case "g":
                    TextBox2.Text += "g";
                    break;
                case "h":
                    TextBox2.Text += "h";
                    break;
                case "i":
                    TextBox2.Text += "i";
                    break;
                case "j":
                    TextBox2.Text += "j";
                    break;
                case "k":
                    TextBox2.Text += "k";
                    break;
                case "l":
                    TextBox2.Text += "l";
                    break;
                case "m":
                    TextBox2.Text += "m";
                    break;
                case "n":
                    TextBox2.Text += "n";
                    break;
                case "o":
                    TextBox2.Text += "o";
                    break;
                case "p":
                    TextBox2.Text += "p";
                    break;
                case "q":
                    TextBox2.Text += "q";
                    break;
                case "r":
                    TextBox2.Text += "r";
                    break;
                case "s":
                    TextBox2.Text += "s";
                    break;
                case "t":
                    TextBox2.Text += "t";
                    break;
                case "u":
                    TextBox2.Text += "u";
                    break;
                case "v":
                    TextBox2.Text += "v";
                    break;
                case "w":
                    TextBox2.Text += "w";
                    break;
                case "x":
                    TextBox2.Text += "x";
                    break;
                case "y":
                    TextBox2.Text += "y";
                    break;
                case "z":
                    TextBox2.Text += "z";
                    break;
                case "one":
                    TextBox2.Text += "1";
                    break;
                case "two":
                    TextBox2.Text += "2";
                    break;
                case "three":
                    TextBox2.Text += "3";
                    break;
                case "four":
                    TextBox2.Text += "4";
                    break;
                case "five":
                    TextBox2.Text += "5";
                    break;
                case "six":
                    TextBox2.Text += "6";
                    break;
                case "seven":
                    TextBox2.Text += "7";
                    break;
                case "eight":
                    TextBox2.Text += "8";
                    break;
                case "nine":
                    TextBox2.Text += "9";
                    break;
                case "zero":
                    TextBox2.Text += "0";
                    break;
                case "dot":
                    TextBox2.Text += ".";
                    break;
                case "at":
                    TextBox2.Text += "@";
                    break;
                case "star":
                    TextBox2.Text += "*";
                    break;
                case "hash":
                    TextBox2.Text += "#";
                    break;
                case "dollar":
                    TextBox2.Text += "$";
                    break;
                case "parcent":
                    TextBox2.Text += "%";
                    break;
                case "login":
                case "log in":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                case "show":
                case "show password":
                case "hide":
                case "hide password":
                    button3.PerformClick();
                    break;
                case "add id":
                    engine2.Dispose();
                    AddId();
                    break;
                case "registration":
                case "go to registration":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;
            }
        }

    }
}
