using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace OHannah
{
    public partial class MediaPlayer : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        string[] files, paths;
        public MediaPlayer()
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
                MessageBox.Show("Can not find the command.\n"+e.Message);
            }
        }

        private void MediaPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        void LoadGrammer()
        {
            Choices texts = new Choices();
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\playerCommands.txt");
            texts.Add(commands);
            Grammar words = new Grammar(new GrammarBuilder(texts));
            engine.LoadGrammar(words);
        }
        void engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            switch (speech)
            {
                case "open play list":
                case "add play list":
                case "add playlist":
                case "add music":
                    textBox1.Text += (speech + " \r\n");
                    ohannah.SpeakAsync("Choose music from your device.");
                    button1.PerformClick();
                    break;
                case "maximize":
                case "go full screen":
                case "go fullscreen":
                    textBox1.Text += (speech + "\r\n");
                    button10.PerformClick();
                    break;
                case "minimize":
                case "hide media player":
                    textBox1.Text += (speech + "\r\n");
                    WindowState = FormWindowState.Minimized;
                    break;
                case "show media player":
                    textBox1.Text += (speech + "\r\n");
                    WindowState = FormWindowState.Normal;
                    break;
                case "exit full screen":
                case "exit fullscreen":
                    textBox1.Text += (speech + "\r\n");
                    player.Focus();
                    player.fullScreen = false;
                    break;
                case "play":
                case "play music":
                    textBox1.Text += (speech + "\r\n");
                    button4.PerformClick();
                    break;
                case "pause":
                    textBox1.Text += (speech + "\r\n");
                    button5.PerformClick();
                    break;
                case "stop":
                    textBox1.Text += (speech + "\r\n");
                    button6.PerformClick();
                    break;
                case "resume":
                    textBox1.Text += (speech + "\r\n");
                    button4.PerformClick();
                    break;
                case "previous":
                    textBox1.Text += (speech + "\r\n");
                    button3.PerformClick();
                    break;
                case "next":
                    textBox1.Text += (speech + "\r\n");
                    button7.PerformClick();
                    break;
                case "fast forward":
                case "forward":
                    textBox1.Text += (speech + "\r\n");
                    button8.PerformClick();
                    break;
                case "fast reverse":
                case "reverse":
                    textBox1.Text += (speech + "\r\n");
                    button2.PerformClick();
                    break;
                case "go back":
                case "close media player":
                case "close":
                    textBox1.Text += (speech + "\r\n");
                    engine.RecognizeAsyncStop();
                    this.Dispose();
                    Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
                        break;
                case "mute volume":
                case "mute":
                        textBox1.Text += (speech + "\r\n");
                        button9.PerformClick();
                    break;
                case "volume up":
                    textBox1.Text += (speech + "\r\n");
                    if (player.settings.volume <= 90)
                    {
                        player.settings.volume += 10;
                        //trackBar1.Value += 10;
                    }
                    break;
                case "volume down":
                    textBox1.Text += (speech + "\r\n");
                    if (player.settings.volume >= 10)
                    {
                        player.settings.volume -= 10;
                        //trackBar1.Value += 10;
                    }
                    break;



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "(mp3,wab,mp4,mov,wmv,mpg,avi,3gp,flv|*.mp3;*.wab;*.mp4;*.mov;*.wmv;*.mpg;*.avi;*.3gp;*.flv|all files|*.*";
            of.Multiselect = true;
            if (of.ShowDialog() == DialogResult.OK)
            {
                files = of.SafeFileNames;
                paths = of.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    PlayList.Items.Add(files[i]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.fastReverse();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (PlayList.SelectedIndex == 0)
                {

                }
                else
                {
                    player.Ctlcontrols.previous();
                    PlayList.SelectedIndex -= 1;
                    PlayList.Update();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (PlayList.Items.Count != 0)
            {
                PlayList.SelectedIndex = 0;
                player.Ctlcontrols.play();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.pause();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (PlayList.SelectedIndex < (PlayList.Items.Count - 1))
                {
                    player.Ctlcontrols.next();
                    PlayList.SelectedIndex += 1;
                    PlayList.Update();
                }
                else
                {
                    PlayList.SelectedIndex = 0;
                    PlayList.Update();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.fastForward();
        }

        private void PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            player.URL = paths[PlayList.SelectedIndex];
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int rate = 10 * (trackBar1.Value);
            player.settings.volume = rate;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (player.settings.volume == 100)
            {
                player.settings.volume = 0;
            }
            else
            {
                player.settings.volume = 100;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                player.fullScreen = true;
            }
            else
            {
                player.Focus();
                player.fullScreen = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            engine.RecognizeAsyncStop();
            this.Dispose();
            Form1.engine.RecognizeAsync(RecognizeMode.Multiple);
        }
    }
}
