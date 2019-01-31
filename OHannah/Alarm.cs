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
using System.Media;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace OHannah
{
    public partial class Alarm : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        SpeechRecognitionEngine engine2;
        SoundPlayer player = new SoundPlayer();
        DateTime time;
        DataAccess data = new DataAccess();

        int hour;
        int min;

        //SqlConnection con;
        string userId = LogIn.userId;
        DateTime aDate;
        public Alarm()
        {
            InitializeComponent();
            
            
        }

        void LoadGrammer()
        {
            Choices texts = new Choices();
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\alarmCommands.txt");
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
                case "set alarm":
                    button1.PerformClick();
                    break;
                case "stop":
                    this.StopPlayer();
                    break;
                case "back":
                case "go back":
                    button2.PerformClick();
                    break;
            }
        }

        private void Alarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            engine2.Dispose();
            this.Dispose();
            Form1.engine.RecognizeAsync(RecognizeMode.Multiple);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            time = dateTimePicker1.Value;
            this.SetAlarm();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show(aDate.ToShortDateString());

            DateTime currTime = DateTime.Now;

            if (aDate.ToShortDateString().Equals("1/1/0001") || aDate.ToShortDateString().Equals("1/1/1900"))
            {
                aDate = data.GetAlarm(userId);
                //MessageBox.Show(aDate.ToString());
                if (aDate.Date == currTime.Date && aDate.Hour == currTime.Hour && aDate.Minute == currTime.Minute)
                {
                    //timer2.Stop();
                    //MessageBox.Show(aDate);

                    data.DeleteAlarm(userId);

                    try
                    {
                        
                        player.SoundLocation = Environment.CurrentDirectory + "\\smoke-detector-1.wav";
                        player.PlayLooping();
                        DialogResult result = MessageBox.Show("You have an alarm");
                        if (result == DialogResult.OK)
                        {

                            player.Stop();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("You have an alarm");
                        MessageBox.Show(ex.Message);

                    }
                }
            }
            else
            {
                aDate = data.GetAlarm(userId);
                if (aDate.Date == currTime.Date && aDate.Hour == currTime.Hour && aDate.Minute == currTime.Minute )
                {
                    //timer2.Stop();

                    data.DeleteAlarm(userId);

                    try
                    {
                        
                        player.SoundLocation = Environment.CurrentDirectory + "\\smoke-detector-1.wav";
                        player.PlayLooping();
                        DialogResult result = MessageBox.Show("You have an alarm");
                        if(result == DialogResult.OK)
                        {
                            
                            player.Stop();
                        }
                        
                        
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("You have an alarm");
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        public void SetAlarm()
        {
            label1.Visible = false;

            
            //MessageBox.Show(time.ToString());
            //MessageBox.Show(DateTime.Now.ToString());
            if (time <= DateTime.Now)
            {
                label1.Visible = true;
                ohannah.Speak("Invalid time");
                SelectHour();
            }
            else
            {
                label1.Visible = false;
                //engine2.Dispose();
                data.InsertAlarm(time,userId);
                //MessageBox.Show("Alarm set at :" + time.ToLongDateString() +"\t  "+ time.ToLongTimeString());
                ohannah.Speak("Alarm set at :" + time.ToLongDateString() + "\t  " + time.ToLongTimeString());

                button2.PerformClick();
                //engine.RecognizeAsync(RecognizeMode.Multiple);
                //timer2.Start();
            }
        }


        public void StopPlayer()
        {
            player.Stop();
        }

        private void Alarm_Load(object sender, EventArgs e)
        {
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

            SelectHour();
        }

        void SelectHour()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Select hour");
            dateTimePicker1.Focus();
            //TextBox1.Text = "";
            
            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine2_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\numbers.txt");
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
            textBox1.Text += speech + "\r\n";
            switch (speech)
            {
                case "one":
                    hour = 1;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "two":
                    hour = 2;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "three":
                    hour = 3;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "four":
                    hour = 4;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "five":
                    hour = 5;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "six":
                    hour = 6;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "seven":
                    hour = 7;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "eight":
                    hour = 8;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "nine":
                    hour = 9;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "ten":
                    hour = 10;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "eleven":
                    hour = 11;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "twelve":
                    hour = 12;
                    engine2.Dispose();
                    SelectMin();
                    break;
                case "go back":
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
            }


        }

        void SelectMin()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Hour selected. Select minute");
            dateTimePicker1.Focus();
            //TextBox1.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine3_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\numbers.txt");
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
            textBox1.Text += speech + "\r\n";
            switch (speech)
            {
                case "zero":
                    min = 0;
                    engine2.Dispose();
                    Choice();
                    break;
                case "one":
                    min = 1;
                    engine2.Dispose();
                    Choice();
                    break;
                case "two":
                    min = 2;
                    engine2.Dispose();
                    Choice();
                    break;
                case "three":
                    min = 3;
                    engine2.Dispose();
                    Choice();
                    break;
                case "four":
                    min = 4;
                    engine2.Dispose();
                    Choice();
                    break;
                case "five":
                    min = 5;
                    engine2.Dispose();
                    Choice();
                    break;
                case "six":
                    min = 6;
                    engine2.Dispose();
                    Choice();
                    break;
                case "seven":
                    min = 7;
                    engine2.Dispose();
                    Choice();
                    break;
                case "eight":
                    min = 8;
                    engine2.Dispose();
                    Choice();
                    break;
                case "nine":
                    min = 9;
                    engine2.Dispose();
                    Choice();
                    break;
                case "ten":
                    min = 10;
                    engine2.Dispose();
                    Choice();
                    break;
                case "eleven":
                    min = 11;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twelve":
                    min = 12;
                    engine2.Dispose();
                    Choice();
                    break;
                case "go back":
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
                case "thirteen":
                    min = 13;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fourteen":
                    min = 14;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifteen":
                    min = 15;
                    engine2.Dispose();
                    Choice();
                    break;
                case "sixteen":
                    min = 16;
                    engine2.Dispose();
                    Choice();
                    break;
                case "seventeen":
                    min = 17;
                    engine2.Dispose();
                    Choice();
                    break;
                case "eighteen":
                    min = 18;
                    engine2.Dispose();
                    Choice();
                    break;
                case "nineteen":
                    min = 19;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty":
                    min = 20;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-one":
                    min = 21;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-two":
                    min = 22;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-three":
                    min = 23;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-four":
                    min = 24;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-five":
                    min = 25;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-six":
                    min = 26;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-seven":
                    min = 27;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-eight":
                    min = 28;
                    engine2.Dispose();
                    Choice();
                    break;
                case "twenty-nine":
                    min = 29;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty":
                    min = 30;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-one":
                    min = 31;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-two":
                    min = 32;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-three":
                    min = 33;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-four":
                    min = 34;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-five":
                    min = 35;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-six":
                    min = 36;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-seven":
                    min = 37;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-eight":
                    min = 38;
                    engine2.Dispose();
                    Choice();
                    break;
                case "thirty-nine":
                    min = 39;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty":
                    min = 40;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-one":
                    min = 21;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-two":
                    min = 42;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-three":
                    min = 43;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-four":
                    min = 44;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-five":
                    min = 45;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-six":
                    min = 46;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-seven":
                    min = 47;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-eight":
                    min = 48;
                    engine2.Dispose();
                    Choice();
                    break;
                case "forty-nine":
                    min = 49;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty":
                    min = 50;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-one":
                    min = 51;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-two":
                    min = 52;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-three":
                    min = 53;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-four":
                    min = 54;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-five":
                    min = 55;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-six":
                    min = 56;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-seven":
                    min = 57;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-eight":
                    min = 58;
                    engine2.Dispose();
                    Choice();
                    break;
                case "fifty-nine":
                    min = 59;
                    engine2.Dispose();
                    Choice();
                    break;
                //case "sixty":
                //    min = 60;
                //    engine2.Dispose();
                //    Choice();
                //    break;
            }


        }

        void Choice()
        {
            ohannah.Speak("Alarm time selected. Do you want to change?");
            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine4_SpeechRecognized;
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(new Choices("yes","no","go back"));
                engine2.LoadGrammar(new Grammar(grammarBuilder));

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
            textBox1.Text += speech + "\r\n";
            if (e.Result.Text == "yes")
            {
                engine2.RecognizeAsyncStop();
                SelectHour();
            }
            else if (e.Result.Text == "no")
            {
                engine2.RecognizeAsyncStop();
                DateTime d2 = DateTime.Now;
                string tt = d2.ToString("tt", CultureInfo.InvariantCulture);


                if (tt == "PM")
                {
                    hour = hour + 12;
                }

                DateTime d = new DateTime(d2.Year, d2.Month, d2.Day, hour, min, 0);
                //MessageBox.Show(d.ToLongTimeString());
                time = d;
                engine.RecognizeAsync(RecognizeMode.Multiple);
                SetAlarm();

            }
            else if ((e.Result.Text == "go back"))
            {
                button2.PerformClick();
            }
            
        }

    }
}
