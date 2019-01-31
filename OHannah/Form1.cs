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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Diagnostics;
using System.Globalization;

namespace OHannah
{
    public partial class Form1 : Form
    {
        public static SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        public SpeechSynthesizer ohannah = new SpeechSynthesizer();
        Alarm a;
        public Form1()
        {
            InitializeComponent();
            try
            {
                
                engine.AudioLevelUpdated += engine_AudioLevelUpdated;
                engine.SpeechRecognized += engine_SpeechRecognized;

                this.LoadGrammer();
                engine.SetInputToDefaultAudioDevice();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                ohannah.SpeakCompleted += ohannah_SpeakCompleted;
                ohannah.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                if (ohannah.State == SynthesizerState.Speaking)
                {
                    ohannah.SpeakAsyncCancelAll();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Can not find the command.\n"+e.Message);
            }
        }

        void ohannah_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            
        }

        void engine_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            progress.Value = e.AudioLevel;
        }

        void LoadGrammer()
        {
            Choices texts = new Choices();
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\commands.txt");
            texts.Add(commands);
            Grammar words = new Grammar(new GrammarBuilder(texts));
            engine.LoadGrammar(words);
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            //textBox1.Text = e.Result.Text;
            string speech = e.Result.Text;

            textBox1.TextAlign = HorizontalAlignment.Right;
            switch (speech)
            {
                case "hello":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("Hello " + LogIn.name);
                    textBox1.Text += "Hello " + LogIn.name + "\r\n";
                    break;
                case "how are you ohannah":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("I am fine sir. How about you?");
                    textBox1.Text += "I am fine sir. How about you?\r\n";
                    break;
                case "tell me a joke":
                case "tell joke":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    TellJoke();
                    break;
                case "stop":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    a.StopPlayer();
                    break;
                case "open notepad":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening notepad");

                    textBox1.Text += "opening notepad\r\n";
                    using(Process pro = Process.Start("notepad.exe"))
                    {
                        pro.WaitForExit();
                    }
                    break;
                case "open calculator":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening calculator");

                    textBox1.Text += "opening calculator\r\n";
                    using (Process pro = Process.Start("calc.exe"))
                    {
                        pro.WaitForExit();
                    }
                    break;
                case "open google chrome":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening google chrome");

                    textBox1.Text += "opening google chrome\r\n";
                    using (Process pro = Process.Start("chrome.exe"))
                    {
                        pro.WaitForExit();
                    }
                    break;
                case "open firefox":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening firefox");

                    textBox1.Text += "opening firefox\r\n";
                    using (Process pro = Process.Start("firefox.exe"))
                    {
                        pro.WaitForExit();
                    }
                    break;
                case "open KMPlayer":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening KMPlayer");

                    textBox1.Text += "opening KMPlayer\r\n";
                    using (Process pro = Process.Start("KMPlayer.exe"))
                    {
                        pro.WaitForExit();
                    }
                    break;
                case "open visual studio":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening visual studio");

                    textBox1.Text += "opening visual studio\r\n";
                    using (Process pro = Process.Start("devenv.exe"))
                    {
                        pro.WaitForExit();
                    }
                    break;
                case "open media player":
                case "open player":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening media player");
                    textBox1.Text += "opening media player\r\n";
                    button1.PerformClick();
                    break;
                case "open email":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening email");
                    textBox1.Text += "opening email\r\n";
                    button2.PerformClick();
                    break;
                case "open browser":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening browser");
                    textBox1.Text += "opening browser\r\n";
                    button3.PerformClick();
                    break;
                case "open wiki":
                case "open wikipedia":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening wikipedia");
                    textBox1.Text += "opening wikipedia\r\n";
                    button4.PerformClick();
                    break;
                case "check weather":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    ohannah.Speak("opening weather window");
                    textBox1.Text += "opening weather window\r\n";
                    button5.PerformClick();
                    break;
                case "set a reminder":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    button6.PerformClick();
                    break;
                case "set an alarm":
                    textBox1.Text += speech + "\r\n";
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    button7.PerformClick();
                    break;

            
            }


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            MediaPlayer m = new MediaPlayer();
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            Email m = new Email();
            m.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            Browser m = new Browser();
            m.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            Wikipedia m = new Wikipedia();
            m.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            Weather m = new Weather();
            m.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            Reminder m = new Reminder();
            m.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            //this.Hide();
            Alarm m = new Alarm();
            m.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reminder r = new Reminder();
            a = new Alarm();
            r.timer1.Start();
            a.timer2.Start();

            DateTime dateTime = DateTime.Now;
            string tt = dateTime.ToString("tt", CultureInfo.InvariantCulture);
            //MessageBox.Show(tt);

            textBox1.TextAlign = HorizontalAlignment.Left;
            if (DateTime.Now.Hour < 12 && tt == "AM")
            {
                ohannah.Speak("Good morning "+LogIn.name + ". How can i assist you?");
                textBox1.Text += "Good morning " + LogIn.name + ". How can i assist you?\r\n";
            }
            else if (DateTime.Now.Hour == 12 && tt == "PM")
            {
                ohannah.Speak("Good noon " + LogIn.name + ". How can i assist you?");
                textBox1.Text += "Good noon " + LogIn.name + ". How can i assist you?\r\n";
            }
            else if ((DateTime.Now.Hour <4) && tt == "PM")
            {
                ohannah.Speak("Good afternoon " + LogIn.name + ". How can i assist you?");
                textBox1.Text += "Good afternoon " + LogIn.name + ". How can i assist you?\r\n";
            }
            else if (DateTime.Now.Hour > 4 && tt == "PM")
            {
                ohannah.Speak("Good evening " + LogIn.name + ". How can i assist you?");
                textBox1.Text += "Good evening " + LogIn.name + ". How can i assist you?\r\n";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        Random r = new Random();
        void TellJoke()
        {
            textBox1.TextAlign = HorizontalAlignment.Left;
            int x = r.Next(8);
            switch (x)
            {
                case 0:
                    ohannah.Speak("Can a kangaroo jump higher than a house? Of course, a house doesn’t jump at all.");
                    textBox1.Text += "Can a kangaroo jump higher than a house? Of course, a house doesn’t jump at all.\r\n";
                    break;
                case 1:
                    ohannah.Speak("Anton, do you think I’m a bad mother? My name is Paul.");
                    textBox1.Text += "Anton, do you think I’m a bad mother? My name is Paul.\r\n";
                    break;
                case 2:
                    ohannah.Speak("What is the difference between a snowman and a snowwoman? Snowballs.");
                    textBox1.Text += "What is the difference between a snowman and a snowwoman? Snowballs.\r\n";
                    break;
                case 3:
                    ohannah.Speak("My wife suffers from a drinking problem.-Oh is she an alcoholic?-No, I am, but she’s the one who suffers.");
                    textBox1.Text += "My wife suffers from a drinking problem.-Oh is she an alcoholic?-No, I am, but she’s the one who suffers.\r\n";
                    break;
                case 4:
                    ohannah.Speak("Police: Open the door! -Man: I don’t want any balls! -Police: What? We don’t have any balls!-Man: I know.");
                    textBox1.Text += "Police: Open the door! -Man: I don’t want any balls! -Police: What? We don’t have any balls!-Man: I know.\r\n";
                    break;
                case 5:
                    ohannah.Speak("A wife complains to her husband: “Just look at that couple down the road, how lovely they are. He keeps holding her hand, kissing her, holding the door for her, why can’t you do the same?”The husband: “Are you mad? I barely know that woman!”");
                    textBox1.Text += "A wife complains to her husband: “Just look at that couple down the road, how lovely they are. He keeps holding her hand, kissing her, holding the door for her, why can’t you do the same?”The husband: “Are you mad? I barely know that woman!”\r\n";
                    break;
                case 6:
                    ohannah.Speak("Me and my wife decided that we don't want to have children anymore. So anybody who wants one can leave us their phone number and address and we will bring you one.");
                    textBox1.Text += "Me and my wife decided that we don't want to have children anymore. So anybody who wants one can leave us their phone number and address and we will bring you one.\r\n";
                    break;
                case 7:
                    ohannah.Speak("Patient: Oh doctor, I’m just so nervous. This is my first operation.-Doctor: Don't worry. Mine too.");
                    textBox1.Text += "Patient: Oh doctor, I’m just so nervous. This is my first operation.-Doctor: Don't worry. Mine too.\r\n";
                    break;
                
            }
        }
    }
}
