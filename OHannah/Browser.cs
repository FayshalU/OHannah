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
using mshtml;
using System.Text.RegularExpressions;
using System.Net;

namespace OHannah
{

    public partial class Browser : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        string[] commands , keywords;
        string convert = null;
        List<string> listItem = new List<string>();

        public Browser()
        {
            InitializeComponent();
            //listItem = null;    
            try
            {
                commands = File.ReadAllLines(Environment.CurrentDirectory + "\\browserCommands.txt");
                keywords = File.ReadAllLines(Environment.CurrentDirectory + "\\browserKeywords.txt");
                engine = CreateSpeech("en-US");
                engine.SpeechRecognized += engine_SpeechRecognized;
                engine.SpeechRecognized += engine_WebSpeechRecognized;
                
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
            catch (Exception e)
            {
                MessageBox.Show("Can not find the command.\n" + e.Message);
            }
        }

        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox2.Text += (speech + "\r\n");
            switch (speech)
            {

                case "search":
                    ohannah.SpeakAsyncCancelAll();
                    button3.PerformClick();
                    break;
                case "pause":
                    button5.PerformClick();
                        break;
                case "resume":
                    button5.PerformClick();
                        break;
                case "stop":
                    button6.PerformClick();
                        break;
                case "start reading":
                        ohannah.SpeakAsyncCancelAll();
                        this.CopyScreen();
                        button4.PerformClick();
                        break;
                case "read":
                case "get the result":
                case "what is the result":
                case "whats the result":
                case "read the result":
                        ohannah.SpeakAsyncCancelAll();
                        this.GetResult();
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
                case "go back":
                case "close":
                case "close website":
                case "close browser":
                        ohannah.SpeakAsyncCancelAll();
                        engine.RecognizeAsyncStop();
                        this.Dispose();
                        Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
                        break;
                        
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
                MessageBox.Show(speech +" not found. Using default.");
                engine = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            }
            return engine;
        }

        void LoadGrammer()
        {
            try
            {
                Choices texts = new Choices();
                string[] command = File.ReadAllLines(Environment.CurrentDirectory + "\\browserDefault.txt");
                texts.Add(command);
                Grammar words = new Grammar(new GrammarBuilder(texts));
                engine.LoadGrammar(words);
                try
                {
                    commands = File.ReadAllLines(Environment.CurrentDirectory + "\\browserCommands.txt");
                    keywords = File.ReadAllLines(Environment.CurrentDirectory + "\\browserKeywords.txt");
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

        void engine_WebSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            int i = 0;
            try
            {
                foreach (string line in commands)
                {
                    if (speech == line)
                    {
                        ohannah.Speak("Searching for " + keywords[i]);
                        textBox1.Text = keywords[i];
                        listItem.Clear();
                        button3.PerformClick();
                        textBox2.Text += (speech + "\r\n");
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                ohannah.Speak("Please check the commands." + speech + "seems to be missing");
            }
        }

        private void Browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = "https://www.bing.com/search?q=" + textBox1.Text;
            //string url = "https://www.google.com/search?q=" + textBox1.Text;
            webBrowser1.Navigate(url);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.CopyScreen();
            button4.Enabled = false;
            button5.Enabled = true;
            try
            {
                ohannah.SpeakAsync(convert);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (ohannah.State == SynthesizerState.Speaking)
            {
                ohannah.Pause();
                button5.Text = "Resume";
            }
            else if (ohannah.State == SynthesizerState.Paused)
            {
                ohannah.Resume();
                button5.Text = "Pause";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (ohannah.State == SynthesizerState.Paused)
            {
                ohannah.Resume();
            }
            ohannah.SpeakAsyncCancelAll();
            button4.Enabled = true;
        }

        void GetResult()
        {

            string url = textBox1.Text;
            WebClient client = new WebClient();
            string page = client.DownloadString("https://www.bing.com/search?q=" + url);
            //string page = client.DownloadString("https://www.google.com/search?q=" + url);
            string news = "<div class=\"b_snippet\">(.*?)</div>";
            news = "<div class=\"b_attribution\">(.*?)</div>";
            news = "<p>(.*?)</p>";
            //MessageBox.Show("outside");
            foreach (Match match in Regex.Matches(page, news))
            {
                try
                {
                    //MessageBox.Show("inside1");
                    listItem.Add(match.Groups[1].Value.Replace("<strong>", "").Replace("</strong>", "").Replace("&", "").Replace("#", ""));
                    //MessageBox.Show("inside2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            if(listItem.Any())
            {
                foreach (string s in listItem)
                {
                    try
                    {
                        
                        //MessageBox.Show(s);
                        ohannah.SpeakAsync(s);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            


        }

        void CopyScreen()
        {
            IHTMLDocument2 htmldoc = webBrowser1.Document.DomDocument as IHTMLDocument2;
            IHTMLSelectionObject selection = htmldoc.selection;
            IHTMLTxtRange range = selection.createRange() as IHTMLTxtRange;

            if (selection != null)
            {
                if (range != null)
                {
                    convert = range.text;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ohannah.SpeakAsyncCancelAll();
            engine.RecognizeAsyncStop();
            this.Dispose();
            Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
        }

        

    }
}
