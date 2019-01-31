using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace OHannah
{
    public partial class MailBox : Form
    {
        public SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
         SpeechSynthesizer ohannah = new SpeechSynthesizer();

        public MailBox()
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
            textBox1.Text += speech + "\r\n";
            switch (speech)
            {
                case "go to inbox":
                case "open inbox":
                case "inbox":
                    button1.PerformClick();
                    break;
                case "go to compose":
                case "compose an email":
                case "send an email":
                    button2.PerformClick();
                    break;
                case "back":
                    button3.PerformClick();
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            engine.RecognizeAsyncCancel();
            Compose c = new Compose(this);
            c.Show();
        }

        private void MailBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncCancel();
            this.Dispose();
            Email em = new Email();
            em.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            this.Hide();
            Inbox c = new Inbox(this);
            c.Show();
        }
    }
}
