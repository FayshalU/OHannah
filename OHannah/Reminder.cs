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
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace OHannah
{
    public partial class Reminder : Form
    {
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
        SpeechSynthesizer ohannah = new SpeechSynthesizer();
        SpeechRecognitionEngine engine2;
        DataAccess data = new DataAccess();

        public static string rDate, rMessage;
        //SqlConnection con;
        string userId = LogIn.userId;
        string userMail = LogIn.email;
        int month, min, hour;
        DateTime date;

        public String Message { get; set; }

        public Reminder()
        {
            InitializeComponent();
            
        }

        void LoadGrammer()
        {
            Choices texts = new Choices();
            string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\reminderCommands.txt");
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
                //case "set":
                case "set reminder":
                    button1.PerformClick();
                    break;
                case "add date":
                    AddMonth();
                    break;
                case "back":
                case "go back":
                    button2.PerformClick();
                    break;
            }
        }


        private void GetReminder()
        {

            label1.Visible = false;
            label3.Visible = false;

            Message = richTextBox1.Text;
            //MessageBox.Show(date.ToShortDateString());


            if (date.Date < DateTime.Now.Date && Message.Equals(""))
            {
                label1.Visible = true;
                label3.Visible = true;
            }
            else if (date.Date < DateTime.Now.Date)
            {
                label3.Visible = true;
            }
            else if (Message.Equals(""))
            {
                label1.Visible = true;
            }
            else
            {
                data.InsertDate(date,userId,Message);
                //MessageBox.Show("Reminder Added for: "+Message + " at: "+date.ToShortDateString());
                ohannah.Speak("Reminder Added for: " + Message + " at: " + date.ToShortDateString());

                button2.PerformClick();

                //timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("Out");
            //MessageBox.Show(rDate.ToShortDateString());
            rDate = null;

            ohannah.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);

            if (rDate == null)
            {
                data.GetDate(userId);
                //MessageBox.Show(rDate);
                if (rDate == DateTime.Now.Date.ToShortDateString())
                {
                    //timer1.Stop();
                    //MessageBox.Show(rDate);

                    rDate = null;
                    
                    ohannah.Speak("You have a reminder: " + rMessage);
                    MessageBox.Show("You have a reminder: " + rMessage);
                    data.DeleteDate(userId);

                    try
                    {
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("ohannah4777@gmail.com", "ohannah47");
                        MailMessage msg = new MailMessage("ohannah4777@gmail.com", userMail, "Reminder", "You have a reminder: " + rMessage + "\n---OHannah");
                        client.Send(msg);

                        client.Credentials = new NetworkCredential("ahasanhamza@gmail.com", "hamza420");
                        MailMessage msg1 = new MailMessage("ahasanhamza@gmail.com", "+880" + LogIn.Number + "@txtlocal.co.uk", "Reminder", "You have a reminder: " + rMessage + "\n---OHannah");
                        client.Send(msg1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //MessageBox.Show("Logged in!");

                    
                }
            }
            else
            {
                //MessageBox.Show(rDate);
                if (rDate == DateTime.Now.Date.ToShortDateString())
                {
                    //timer1.Stop();
                    //MessageBox.Show(rDate);

                    rDate = null;
                    
                    ohannah.Speak("You have a reminder: " + rMessage);
                    MessageBox.Show("You have a reminder: " + rMessage);
                    data.DeleteDate(userId);

                    try
                    {
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.Timeout = 10000;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("ohannah4777@gmail.com", "ohannah47");
                        MailMessage msg = new MailMessage("ohannah4777@gmail.com", userMail, "Reminder", "You have a reminder: " + rMessage + "\n---OHannah");
                        client.Send(msg);

                        client.Credentials = new NetworkCredential("ahasanhamza@gmail.com", "hamza420");
                        MailMessage msg1 = new MailMessage("ahasanhamza@gmail.com", "+880" + LogIn.Number + "@txtlocal.co.uk", "Reminder", "You have a reminder: " + rMessage + "\n---OHannah");
                        client.Send(msg1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //MessageBox.Show("Logged in!");

                    
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            date = dateTimePicker1.Value.Date;
            this.GetReminder();

            
        }

        

        private void Reminder_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Reminder_Load(object sender, EventArgs e)
        {
            try
            {
                

                engine.SpeechRecognized += engine_SpeechRecognized;

                this.LoadGrammer();
                engine.LoadGrammar(new DictationGrammar());
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

            
            this.AddMessage();
        }

        void AddMessage()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("What you want me to remind you for?");
            richTextBox1.Focus();

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine2_SpeechRecognized;
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(new Choices("set reminder", "back", "go back","add date"));
                engine2.LoadGrammar(new Grammar(grammarBuilder));
                engine2.LoadGrammar(new DictationGrammar());
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

            if (e.Result.Text == "set reminder")
            {
                engine2.RecognizeAsyncStop();
                //ohannah.Resume();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                //ohannah.SpeakAsync("Message added");
                button1.PerformClick();
            }
            else if (e.Result.Text == "add date")
            {
                engine2.RecognizeAsyncStop();
                
                ohannah.Speak("Message added");
                AddMonth();
            }
            else if (e.Result.Text == "back" || e.Result.Text == "go back")
            {
                engine2.RecognizeAsyncStop();
                //ohannah.Resume();
                engine.RecognizeAsync(RecognizeMode.Multiple);
                //ohannah.SpeakAsync("Message added");
                button2.PerformClick();
            }
            else
            {
                richTextBox1.Text += (speech + " ");
            }
        }

        void AddMonth()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Select month");
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
                case "one":
                    month = 1;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "two":
                    month = 2;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "three":
                    month = 3;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "four":
                    month = 4;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "five":
                    month = 5;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "six":
                    month = 6;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "seven":
                    month = 7;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "eight":
                    month = 8;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "nine":
                    month = 9;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "ten":
                    month = 10;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "eleven":
                    month = 11;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "twelve":
                    month = 12;
                    engine2.Dispose();
                    SelectDay();
                    break;
                case "go back":
                    engine2.RecognizeAsyncCancel();
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
            }


        }

        void SelectDay()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("Select day");
            dateTimePicker1.Focus();
            //TextBox1.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine4_SpeechRecognized;
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
        void engine4_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            textBox1.Text += speech + "\r\n";

            switch (speech)
            {
                case "one":
                    min = 1;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "two":
                    min = 2;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "three":
                    min = 3;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "four":
                    min = 4;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "five":
                    min = 5;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "six":
                    min = 6;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "seven":
                    min = 7;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "eight":
                    min = 8;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "nine":
                    min = 9;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "ten":
                    min = 10;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "eleven":
                    min = 11;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twelve":
                    min = 12;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "go back":
                    engine2.RecognizeAsyncCancel();
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
                case "thirteen":
                    min = 13;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "fourteen":
                    min = 14;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "fifteen":
                    min = 15;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "sixteen":
                    min = 16;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "seventeen":
                    min = 17;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "eighteen":
                    min = 18;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "nineteen":
                    min = 19;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty":
                    min = 20;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-one":
                    min = 21;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-two":
                    min = 22;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-three":
                    min = 23;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-four":
                    min = 24;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-five":
                    min = 25;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-six":
                    min = 26;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-seven":
                    min = 27;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-eight":
                    min = 28;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "twenty-nine":
                    min = 29;
                    engine2.Dispose();
                    SelectYear();
                    break;
                case "thirty":
                    min = 30;
                    engine2.Dispose();
                    SelectYear();
                    break;
                

            }
        }

        void SelectYear()
        {
            engine.RecognizeAsyncCancel();
            ohannah.Speak("After how many years? If this year select zero");
            dateTimePicker1.Focus();
            //TextBox1.Text = "";

            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine5_SpeechRecognized;
                Choices texts = new Choices();
                string[] commands = File.ReadAllLines(Environment.CurrentDirectory + "\\numbersYear.txt");
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
            textBox1.Text += speech + "\r\n";

            switch (speech)
            {
                case "zero":
                    hour = 0;
                    engine2.Dispose();
                    Check();
                    break;
                case "one":
                    hour = 1;
                    engine2.Dispose();
                    Check();
                    break;
                case "two":
                    hour = 2;
                    engine2.Dispose();
                    Check();
                    break;
                case "three":
                    hour = 3;
                    engine2.Dispose();
                    Check();
                    break;
                case "four":
                    hour = 4;
                    engine2.Dispose();
                    Check();
                    break;
                case "five":
                    hour = 5;
                    engine2.Dispose();
                    Check();
                    break;
                case "six":
                    hour = 6;
                    engine2.Dispose();
                    Check();
                    break;
                case "seven":
                    hour = 7;
                    engine2.Dispose();
                    Check();
                    break;
                case "eight":
                    hour = 8;
                    engine2.Dispose();
                    Check();
                    break;
                case "nine":
                    hour = 9;
                    engine2.Dispose();
                    Check();
                    break;
                case "ten":
                    hour = 10;
                    engine2.Dispose();
                    Check();
                    break;
                case "eleven":
                    hour = 11;
                    engine2.Dispose();
                    Check();
                    break;
                case "twelve":
                    hour = 12;
                    engine2.Dispose();
                    Check();
                    break;
                case "go back":
                    engine2.RecognizeAsyncCancel();
                    engine2.Dispose();
                    button2.PerformClick();
                    break;
            }



        }

        void Check()
        {
            ohannah.Speak("Reminder time selected. Do you want to change?");
            engine2 = new SpeechRecognitionEngine();
            try
            {

                engine2.SpeechRecognized += engine6_SpeechRecognized;
                //engine2.LoadGrammar(new DictationGrammar());
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(new Choices("yes", "no", "go back"));
                engine2.LoadGrammar(new Grammar(grammarBuilder));

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
            textBox1.Text += speech + "\r\n";

            if (e.Result.Text == "yes")
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                AddMonth();
            }
            else if (e.Result.Text == "no")
            {
                engine2.RecognizeAsyncStop();
                engine2.Dispose();
                DateTime d2 = DateTime.Now;

                DateTime d = new DateTime(d2.Year+hour, month,min);

                date = d;
                engine.RecognizeAsync(RecognizeMode.Multiple);
                GetReminder();

            }
            else if ((e.Result.Text == "go back"))
            {
                engine2.RecognizeAsyncCancel();
                engine2.Dispose();
                button2.PerformClick();
            }

        }

    }
    
}
