using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using mshtml;
using System.Net;
using System.Xml;

namespace OHannah
{
    public partial class Wikipedia : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        string[] commands, keywords;
        string convert;
        List<string> listItem = new List<string>();
        public Wikipedia()
        {
            InitializeComponent();
            try
            {
                commands = File.ReadAllLines(Environment.CurrentDirectory + "\\wikipediaCommands.txt");
                keywords = File.ReadAllLines(Environment.CurrentDirectory + "\\wikipediaKeywords.txt");
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

        private void Wikipedia_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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

        void LoadGrammer()
        {
            try
            {
                Choices texts = new Choices();
                string[] command = File.ReadAllLines(Environment.CurrentDirectory + "\\wikipediaDefault.txt");
                texts.Add(command);
                Grammar words = new Grammar(new GrammarBuilder(texts));
                engine.LoadGrammar(words);
                try
                {
                    commands = File.ReadAllLines(Environment.CurrentDirectory + "\\wikipediaCommands.txt");
                    keywords = File.ReadAllLines(Environment.CurrentDirectory + "\\wikipediaKeywords.txt");
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
            textBox2.Text += speech + "\r\n";

            int i = 0;
            try
            {
                foreach (string line in commands)
                {
                    if (speech == line)
                    {
                        ohannah.SpeakAsync("Searching for " + keywords[i]);
                        textBox1.Text = keywords[i];
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
                case "pause":
                    button3.PerformClick();
                    break;
                case "resume":
                    button3.PerformClick();
                    break;
                case "stop":
                    button4.PerformClick();
                    break;
                case "start reading":
                    ohannah.SpeakAsyncCancelAll();
                    this.CopyScreen();
                    button2.PerformClick();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://en.wikipedia.org/wiki/" + textBox1.Text;
            webBrowser1.Navigate(url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.CopyScreen();
            button2.Enabled = false;
            button4.Enabled = true;
            try
            {
                ohannah.SpeakAsync(convert);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ohannah.State == SynthesizerState.Speaking)
            {
                ohannah.Pause();
                button3.Text = "Resume";
            }
            else if (ohannah.State == SynthesizerState.Paused)
            {
                ohannah.Resume();
                button3.Text = "Pause";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ohannah.State == SynthesizerState.Paused)
            {
                ohannah.Resume();
            }
            ohannah.SpeakAsyncCancelAll();
            button2.Enabled = true;
        }

        void GetResult()
        {

            string url = textBox1.Text;
            
            try
            {
                MessageBox.Show("Outside");
                var webClient = new WebClient();
                var pageSourceCode = webClient.DownloadString("http://en.wikipedia.org/w/api.php?format=xml&action=query&prop=extracts&titles=" + url + "&redirects=true");

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(pageSourceCode);

                var fnode = doc.GetElementsByTagName("extract")[0];

                string ss = fnode.InnerText;

                Regex regex = new Regex("\\<[^\\>]*\\>");

                String.Format("Before:{0}", ss); // HTML Text

                ss = regex.Replace(ss, String.Empty);

                string result = String.Format(ss);

                ohannah.SpeakAsync(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void button5_Click(object sender, EventArgs e)
        {
            ohannah.SpeakAsyncCancelAll();
            engine.RecognizeAsyncStop();
            this.Dispose();
            Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
        }

    }
}
