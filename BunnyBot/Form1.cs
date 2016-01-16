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

        public form1()
        {
            InitializeComponent();
        }

        
        private void form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "bunny", "hello", "hai", "hey", "what time is it", "what day is it" });
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
            switch (input)
            {
                case "hai":
                case "hello":
                case "hey":
                case "bunny":
                    bunny.Speak("Hello");
                    break;
            }
        }
    }
}
