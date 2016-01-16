﻿using System;
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

        public form1()
        {
            InitializeComponent();
        }

        
        private void form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "bunny", "hello", "hai", "hey", "what time is it", "what day is it", "goodbye", "bye", "see you" });
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
            int ranNum = new Random(1,10);
            switch (input)
            {
                case "hai":
                case "hello":
                case "hey":
                case "bunny":
                    if (ranNum < 5)
                    {
                        bunny.Speak("Hello");
                    }
                    else
                    {
                        bunny.Speak("Hai");
                    }
                    break;

                case "what time is it":
                    DateTime now = new DateTime();
                    string time = now.GetDateTimeFormats('t')[0];
                    bunny.Speak(time);
                    break;

                case "what date is it":
                    bunny.Speak(DateTime.Today.ToString("dd-MM-yyyy"));
                    break;

                case "goodbye":
                case "bye":
                    if (ranNum < 5)
                    {
                        bunny.Speak("Farewell");
                    }
                    else
                    {
                        bunny.Speak("GoodBye");
                    }
                    break;
                   
            }
        }
    }
}
