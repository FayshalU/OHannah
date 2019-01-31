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

namespace OHannah
{
    public partial class Weather : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        string[] commands, keywords;
        public Weather()
        {
            InitializeComponent();

            try
            {
                commands = File.ReadAllLines(Environment.CurrentDirectory + "\\weatherCommands.txt");
                keywords = File.ReadAllLines(Environment.CurrentDirectory + "\\weatherKeywords.txt");
                engine = CreateSpeech("en-US");
                engine.SpeechRecognized += engine_SpeechRecognized;
                engine.SpeechRecognized += engine_WeatherSpeechRecognized;

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

        private void Weather_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        void LoadGrammer()
        {
            try
            {
                Choices texts = new Choices();
                string[] command = File.ReadAllLines(Environment.CurrentDirectory + "\\weatherDefault.txt");
                texts.Add(command);
                Grammar words = new Grammar(new GrammarBuilder(texts));
                engine.LoadGrammar(words);
                try
                {
                    commands = File.ReadAllLines(Environment.CurrentDirectory + "\\weatherCommands.txt");
                    keywords = File.ReadAllLines(Environment.CurrentDirectory + "\\weatherKeywords.txt");
                    Grammar webCommands = new Grammar(new GrammarBuilder(new Choices(commands)));
                    engine.LoadGrammar(webCommands);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        SpeechRecognitionEngine CreateSpeech(string speech)
        {
            foreach (RecognizerInfo info in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (info.Culture.ToString() == speech)
                {
                    engine = new SpeechRecognitionEngine(info);
                    break;
                }
            }
            if (engine == null)
            {
                MessageBox.Show(speech + " not found. Using default.");
                engine = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            }
            return engine;
        }

        void engine_WeatherSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox2.Text += speech + "\r\n";
            //MessageBox.Show(speech);
            int i = 0;
            try
            {
                foreach (string line in commands)
                {
                    if (speech == line)
                    {
                        //MessageBox.Show("inside");
                        
                        textBox1.Text = keywords[i];
                        ohannah.SpeakAsync("Getting weather of " + keywords[i]);
                        button1.PerformClick();
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                ohannah.Speak("Please check the commands." + speech + "seems to be missing");
                MessageBox.Show(ex.Message);
            }
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox2.Text += speech + "\r\n";

            switch (speech)
            {
                case "search":
                    ohannah.SpeakAsyncCancelAll();
                    button1.PerformClick();
                    break;
                
                case "minimize":
                case "hide website":
                case "hide browser":
                    WindowState = FormWindowState.Minimized;
                    break;
                case "maximize":
                case "show website":
                case "show browser":
                    WindowState = FormWindowState.Normal;
                    break;
                case "close":
                case "go back":
                case "close website":
                case "close browser":
                    ohannah.SpeakAsyncCancelAll();
                    button2.PerformClick();

                    break;
            }
        }

        void CheckWeather()
        {
            engine.RecognizeAsyncStop();
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "";
            GetWeather weather = new GetWeather(textBox1.Text);
            label7.Text += weather.Temp;
            ohannah.SpeakAsync(label1.Text + label7.Text);
            label8.Text += weather.TempMax;
            ohannah.SpeakAsync(label2.Text + label8.Text);
            label9.Text += weather.TempMin;
            ohannah.SpeakAsync(label3.Text + label9.Text);
            label10.Text += weather.Humidity;
            ohannah.SpeakAsync(label4.Text + label10.Text);
            label11.Text += weather.Wind;
            ohannah.SpeakAsync(label5.Text + label11.Text);
            label12.Text += weather.Clouds;
            ohannah.SpeakAsync(label6.Text + label12.Text);
            //ohannah.Speak(label1.Text + label2.Text + label3.Text + label4.Text + label5.Text + label6.Text );

            engine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            this.Dispose();
            Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.CheckWeather();
            
        }

        

    }
}
