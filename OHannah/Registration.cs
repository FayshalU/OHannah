using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace OHannah
{
    public partial class Registration : Form
    {

        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        SpeechRecognitionEngine engine2;
        //SqlConnection con;
        DataAccess data = new DataAccess();

        public string name;
        public string userId;
        public string password;
        public string phone;
        public string email;

        public Registration()
        {
            InitializeComponent();
            

            try
            {
                engine.SpeechRecognized += engine_SpeechRecognized;

                this.LoadGrammer();
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
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\registrationCommands.txt");
            texts.Add(commands);
            Grammar words = new Grammar(new GrammarBuilder(texts));
            engine.LoadGrammar(words);
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox6.Text += speech + "\r\n";
            switch (speech)
            {
                case "check user":
                    button3.PerformClick();
                    break;
                case "add id":
                    AddId();
                    break;
                case "add name":
                    AddName();
                    break;
                case "add password":
                    AddPass();
                    break;
                case "add email":
                    AddEmail();
                    break;
                case "add number":
                    AddNumber();
                    break;
                case "confirm registration":
                //case "confirm":
                    Button1.PerformClick();
                    break;
                //case "back":
                case "go back":
                    Button2.PerformClick();
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;

         

            if (TextBox1.Text != "" && TextBox2.Text != "" && TextBox3.Text != "" && TextBox4.Text != "" && TextBox5.Text != "")
            {
                if (IsValidEmail(TextBox4.Text))
                {
                    if (CheckNumber())
                    {
                        if (data.CheckID(TextBox1.Text))
                        {
                            userId = TextBox1.Text;
                            name = TextBox2.Text;
                            password = TextBox3.Text;
                            email = TextBox4.Text;
                            phone = TextBox5.Text;
                            data.InsertUser(userId,password,email,phone,name);
                            MessageBox.Show("User added");
                            LogIn.name = name;
                            LogIn.email = this.email;
                            LogIn.Number = phone;
                            this.Dispose();
                            Form1 f = new Form1();
                            f.Show();
                        }
                        else
                        {
                            MessageBox.Show("User id is not available");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter a valid number");
                    }
                }
                else
                {
                    MessageBox.Show("Enter a valid mail id");
                }
            }
            else
            {
                if (TextBox1.Text == "")
                {
                    label6.Visible = true;

                }
                if (TextBox2.Text == "")
                {
                    label7.Visible = true;

                }
                if (TextBox3.Text == "")
                {
                    label8.Visible = true;

                }
                if (TextBox4.Text == "")
                {
                    label9.Visible = true;

                }
                if (TextBox5.Text == "" )
                {
                    label10.Visible = true;
                }
            }
             
        }

        bool CheckNumber()
        {
            if ((TextBox5.Text.Length == 11) && (TextBox5.Text.ElementAt(0) == '0') && (TextBox5.Text.ElementAt(1) == '1'))
                return true;
            else
                return false;
        }
        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncCancel();
            this.Dispose();
            LogIn l = new LogIn();
            l.Show();
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "")
            {
                label6.Visible = true;
            }
            else
            {
                label6.Visible = false;

                if (data.CheckID(TextBox1.Text))
                {
                    MessageBox.Show("User id is available");
                }
                else
                {
                    MessageBox.Show("User id is not available");
                }
            }
        }
        

        private void Registration_Load(object sender, EventArgs e)
        {
            ohannah.Speak("Enter your information");
        }

        void AddId()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell a user id");
            TextBox1.Focus();
            //TextBox1.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine2_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsRegistration.txt");
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
            textBox6.Text += speech + "\r\n";

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
                case "check user":
                    button3.PerformClick();
                    break;
                case "add name":
                    engine2.Dispose();
                    AddName();
                    break;
                case "add password":
                    engine2.Dispose();
                    AddPass();
                    break;
                case "add email":
                    engine2.Dispose();
                    AddEmail();
                    break;
                case "add number":
                    engine2.Dispose();
                    AddNumber();
                    break;
                case "confirm registration":
                //case "confirm":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                //case "back":
                case "go back":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;

            }
        }

        void AddName()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your name");
            TextBox2.Focus();
            //TextBox2.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine3_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsRegistration.txt");
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
            textBox6.Text += speech + "\r\n";

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
                case "space":
                    TextBox2.Text += " ";
                    break;
                case "add id":
                    engine2.Dispose();
                    AddId();
                    break;
                case "add password":
                    engine2.Dispose();
                    AddPass();
                    break;
                case "add email":
                    engine2.Dispose();
                    AddEmail();
                    break;
                case "add number":
                    engine2.Dispose();
                    AddNumber();
                    break;
                case "confirm registration":
                //case "confirm":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                //case "back":
                case "go back":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;

            }
        }

        void AddPass()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell a password");
            TextBox3.Focus();
            //TextBox3.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine4_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsRegistration.txt");
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

        void engine4_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox6.Text += speech + "\r\n";

            switch (speech)
            {
                case "clear":
                    TextBox3.Text = "";
                    break;
                case "erase":
                    if (TextBox3.Text.Length > 0)
                    {
                        TextBox3.Text = TextBox3.Text.Substring(0, (TextBox3.Text.Length - 1));
                    }
                    break;
                case "a":
                    TextBox3.Text += "a";
                    break;
                case "b":
                    TextBox3.Text += "b";
                    break;
                case "c":
                    TextBox3.Text += "c";
                    break;
                case "d":
                    TextBox3.Text += "d";
                    break;
                case "e":
                    TextBox3.Text += "e";
                    break;
                case "f":
                    TextBox3.Text += "f";
                    break;
                case "g":
                    TextBox3.Text += "g";
                    break;
                case "h":
                    TextBox3.Text += "h";
                    break;
                case "i":
                    TextBox3.Text += "i";
                    break;
                case "j":
                    TextBox3.Text += "j";
                    break;
                case "k":
                    TextBox3.Text += "k";
                    break;
                case "l":
                    TextBox3.Text += "l";
                    break;
                case "m":
                    TextBox3.Text += "m";
                    break;
                case "n":
                    TextBox3.Text += "n";
                    break;
                case "o":
                    TextBox3.Text += "o";
                    break;
                case "p":
                    TextBox3.Text += "p";
                    break;
                case "q":
                    TextBox3.Text += "q";
                    break;
                case "r":
                    TextBox3.Text += "r";
                    break;
                case "s":
                    TextBox3.Text += "s";
                    break;
                case "t":
                    TextBox3.Text += "t";
                    break;
                case "u":
                    TextBox3.Text += "u";
                    break;
                case "v":
                    TextBox3.Text += "v";
                    break;
                case "w":
                    TextBox3.Text += "w";
                    break;
                case "x":
                    TextBox3.Text += "x";
                    break;
                case "y":
                    TextBox3.Text += "y";
                    break;
                case "z":
                    TextBox3.Text += "z";
                    break;
                case "one":
                    TextBox3.Text += "1";
                    break;
                case "two":
                    TextBox3.Text += "2";
                    break;
                case "three":
                    TextBox3.Text += "3";
                    break;
                case "four":
                    TextBox3.Text += "4";
                    break;
                case "five":
                    TextBox3.Text += "5";
                    break;
                case "six":
                    TextBox3.Text += "6";
                    break;
                case "seven":
                    TextBox3.Text += "7";
                    break;
                case "eight":
                    TextBox3.Text += "8";
                    break;
                case "nine":
                    TextBox3.Text += "9";
                    break;
                case "zero":
                    TextBox3.Text += "0";
                    break;
                case "dot":
                    TextBox3.Text += ".";
                    break;
                case "at":
                    TextBox3.Text += "@";
                    break;
                case "star":
                    TextBox3.Text += "*";
                    break;
                case "hash":
                    TextBox3.Text += "#";
                    break;
                case "dollar":
                    TextBox3.Text += "$";
                    break;
                case "parcent":
                    TextBox3.Text += "%";
                    break;
                case "show":
                case "hide":
                case "show password":
                case "hide password":
                    button4.PerformClick();
                    break;
                case "add id":
                    engine2.Dispose();
                    AddId();
                    break;
                case "add name":
                    engine2.Dispose();
                    AddName();
                    break;
                case "add email":
                    engine2.Dispose();
                    AddEmail();
                    break;
                case "add number":
                    engine2.Dispose();
                    AddNumber();
                    break;
                case "confirm registration":
               // case "confirm":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                //case "back":
                case "go back":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;

            }
        }

        void AddEmail()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your email");
            TextBox4.Focus();
            //TextBox4.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine5_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsRegistration.txt");
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

        void engine5_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox6.Text += speech + "\r\n";

            switch (speech)
            {
                case "clear":
                    TextBox4.Text = "";
                    break;
                case "erase":
                    if (TextBox4.Text.Length > 0)
                    {
                        TextBox4.Text = TextBox4.Text.Substring(0, (TextBox4.Text.Length - 1));
                    }
                    break;
                case "a":
                    TextBox4.Text += "a";
                    break;
                case "b":
                    TextBox4.Text += "b";
                    break;
                case "c":
                    TextBox4.Text += "c";
                    break;
                case "d":
                    TextBox4.Text += "d";
                    break;
                case "e":
                    TextBox4.Text += "e";
                    break;
                case "f":
                    TextBox4.Text += "f";
                    break;
                case "g":
                    TextBox4.Text += "g";
                    break;
                case "h":
                    TextBox4.Text += "h";
                    break;
                case "i":
                    TextBox4.Text += "i";
                    break;
                case "j":
                    TextBox4.Text += "j";
                    break;
                case "k":
                    TextBox4.Text += "k";
                    break;
                case "l":
                    TextBox4.Text += "l";
                    break;
                case "m":
                    TextBox4.Text += "m";
                    break;
                case "n":
                    TextBox4.Text += "n";
                    break;
                case "o":
                    TextBox4.Text += "o";
                    break;
                case "p":
                    TextBox4.Text += "p";
                    break;
                case "q":
                    TextBox4.Text += "q";
                    break;
                case "r":
                    TextBox4.Text += "r";
                    break;
                case "s":
                    TextBox4.Text += "s";
                    break;
                case "t":
                    TextBox4.Text += "t";
                    break;
                case "u":
                    TextBox4.Text += "u";
                    break;
                case "v":
                    TextBox4.Text += "v";
                    break;
                case "w":
                    TextBox4.Text += "w";
                    break;
                case "x":
                    TextBox4.Text += "x";
                    break;
                case "y":
                    TextBox4.Text += "y";
                    break;
                case "z":
                    TextBox4.Text += "z";
                    break;
                case "one":
                    TextBox4.Text += "1";
                    break;
                case "two":
                    TextBox4.Text += "2";
                    break;
                case "three":
                    TextBox4.Text += "3";
                    break;
                case "four":
                    TextBox4.Text += "4";
                    break;
                case "five":
                    TextBox4.Text += "5";
                    break;
                case "six":
                    TextBox4.Text += "6";
                    break;
                case "seven":
                    TextBox4.Text += "7";
                    break;
                case "eight":
                    TextBox4.Text += "8";
                    break;
                case "nine":
                    TextBox4.Text += "9";
                    break;
                case "zero":
                    TextBox4.Text += "0";
                    break;
                case "dot":
                    TextBox4.Text += ".";
                    break;
                case "at":
                    TextBox4.Text += "@";
                    break;
                case "star":
                    TextBox4.Text += "*";
                    break;
                case "hash":
                    TextBox4.Text += "#";
                    break;
                case "dollar":
                    TextBox4.Text += "$";
                    break;
                case "parcent":
                    TextBox4.Text += "%";
                    break;
                case "add id":
                    engine2.Dispose();
                    AddId();
                    break;
                case "add name":
                    engine2.Dispose();
                    AddName();
                    break;
                case "add password":
                    engine2.Dispose();
                    AddPass();
                    break;
                case "add number":
                    engine2.Dispose();
                    AddNumber();
                    break;
                case "confirm registration":
                //case "confirm":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                //case "back":
                case "go back":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;

            }
        }

        void AddNumber()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Please spell your number");
            TextBox5.Focus();
            //TextBox5.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine6_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alphabetsRegistration.txt");
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

        void engine6_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox6.Text += speech + "\r\n";

            switch (speech)
            {
                case "clear":
                    TextBox5.Text = "";
                    break;
                case "erase":
                    if (TextBox5.Text.Length > 0)
                    {
                        TextBox5.Text = TextBox5.Text.Substring(0, (TextBox5.Text.Length - 1));
                    }
                    break;
                
                case "one":
                    TextBox5.Text += "1";
                    break;
                case "two":
                    TextBox5.Text += "2";
                    break;
                case "three":
                    TextBox5.Text += "3";
                    break;
                case "four":
                    TextBox5.Text += "4";
                    break;
                case "five":
                    TextBox5.Text += "5";
                    break;
                case "six":
                    TextBox5.Text += "6";
                    break;
                case "seven":
                    TextBox5.Text += "7";
                    break;
                case "eight":
                    TextBox5.Text += "8";
                    break;
                case "nine":
                    TextBox5.Text += "9";
                    break;
                case "zero":
                    TextBox5.Text += "0";
                    break;
                
                case "add id":
                    engine2.Dispose();
                    AddId();
                    break;
                case "add name":
                    engine2.Dispose();
                    AddName();
                    break;
                case "add password":
                    engine2.Dispose();
                    AddPass();
                    break;
                case "add email":
                    engine2.Dispose();
                    AddEmail();
                    break;
                case "confirm registration":
                //case "confirm":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button1.PerformClick();
                    break;
                //case "back":
                case "go back":
                    engine2.Dispose();
                    engine.RecognizeAsync(RecognizeMode.Multiple);
                    Button2.PerformClick();
                    break;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (TextBox3.PasswordChar.Equals('*'))
            {
                TextBox3.PasswordChar = '\0';
                button4.Text = "Hide";
            }
            else
            {
                TextBox3.PasswordChar = '*';
                button4.Text = "Show";
            }
        }
    }
}
