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
using System.Globalization;

namespace BunnyBot
{
    public partial class form1 : Form
    {
        SpeechRecognitionEngine user = new SpeechRecognitionEngine();
        SpeechSynthesizer bunny = new SpeechSynthesizer();
        string userName = Environment.UserName;
        private int ranNum;

        public form1()
        {
            InitializeComponent();
        }

        
        private void form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "bunny", "hello", "hai", "hey", "hello bunny", "what is my name", "what time is it", "what day is it", "goodbye", "goodbye bunny", "close bunny", "go offline", "bye", "see you" });
            GrammarBuilder gBuiler = new GrammarBuilder();
            gBuiler.Append(commands);
            Grammar grammer = new Grammar(gBuiler);
            user.LoadGrammarAsync(grammer);
            user.SetInputToDefaultAudioDevice();
            user.SpeechRecognized += User_SpeechRecognized;
                                   
        }

        private void User_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text;
            Random rnd = new Random();
            DateTime now = new DateTime();
            switch (input)
            {
                case "hai":
                case "hello":
                case "hello bunny":
                    if ( now.Hour >= 5 && now.Hour < 12)
                    {
                        bunny.Speak("Good Morning" +userName );
                    }
                    if (now.Hour >= 12 && now.Hour < 18)
                    {
                        bunny.Speak("Good Afternoon " +userName);
                    }
                    if (now.Hour >= 18 && now.Hour < 24)
                    {
                        bunny.Speak("Good Evening " +userName);
                    }
                    if (now.Hour < 5)
                    {
                        bunny.Speak("Hello " +userName + ", it's getting late");
                    }
                    break;
                case "bunny":
                case "hey":
                    ranNum = rnd.Next(1, 5);
                    if (ranNum == 1)
                    {
                        bunny.Speak("Yes sir");
                    }
                    else if (ranNum == 2)
                    {
                        bunny.Speak("Yes?");
                    }
                    else if (ranNum == 3)
                    {
                        bunny.Speak("How may I help?");
                    }
                    else if (ranNum == 4)
                    {
                        bunny.Speak("How may I be of assistance?");
                    }
                    break;

                case "what is my name":
                    bunny.Speak( " " +userName );
                    break;

                case "what time is it":
                    string time = now.GetDateTimeFormats('t')[0];
                    bunny.Speak("time");
                    break;

                case "what date is it":
                    bunny.Speak(DateTime.Today.ToString("dd-MM-yyyy"));
                    break;

                case "goodbye":
                case "bye":
                case "goodbye bunny":
                case "close bunny":
                case "go offline":
                case "see you":
                    if (ranNum > 6)
                    {
                        bunny.Speak("Farewell");
                        Close();
                    }
                    else
                    {
                        bunny.Speak("GoodBye");
                        Close();
                    }
                    break;
                   
            }
        }
    }
}
