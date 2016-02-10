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
using System.Diagnostics;
using System.IO;

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
            pictureBox1.ImageLocation = "C:\\Users\\RAMYA\\Documents\\Visual Studio 2015\\Projects\\speech\\BunnyBot\\BunnyBot\\Resources\\BunnyBot.gif";
            //user.SetInputToDefaultAudioDevice();
        }


        private void form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "hai", "hello bunny", "hello", "bunny", "hey", "what is my name" , "what time is it" , "what day is it", "what date is it" ,"out of my way","off screen","comeback","on screen", "go fullscreen", "exit fullscreen","shutdown","log off","restart","goodbye","bye","see you","close" });
            GrammarBuilder gbuilder = new GrammarBuilder();
            gbuilder.Append(commands);
            Grammar grammer = new Grammar(gbuilder);
            user.LoadGrammarAsync(grammer);
            user.SetInputToDefaultAudioDevice();
            user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(User_SpeechRecognized);
            user.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void User_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text;
            Random rnd = new Random();
            DateTime now = new DateTime();
            if (input == "hai" || input == "hello bunny" || input == "hello")
            {

                if (now.Hour >= 5 && now.Hour < 12)
                {
                    bunny.SpeakAsync("Good Morning" + userName);
                }
                if (now.Hour >= 12 && now.Hour < 18)
                {
                    bunny.SpeakAsync("Good Afternoon " + userName);
                }
                if (now.Hour >= 18 && now.Hour < 24)
                {
                    bunny.SpeakAsync("Good Evening " + userName);
                }
                if (now.Hour < 5)
                {
                    bunny.SpeakAsync("Hello " + userName + ", it's getting late");
                }
            }
            else if (input == "bunny" || input == "hey")
            {
                ranNum = rnd.Next(1, 5);
                if (ranNum == 1)
                {
                    bunny.SpeakAsync("Yes sir");
                }
                else if (ranNum == 2)
                {
                    bunny.SpeakAsync("Yes?");
                }
                else if (ranNum == 3)
                {
                    bunny.SpeakAsync("How may I help?");
                }
                else if (ranNum == 4)
                {
                    bunny.SpeakAsync("How may I be of assistance?");
                }
            }

            else if (input == "what is my name")
            {
                bunny.SpeakAsync(" " + userName.ToString());
            }
            else if (input == "what time is it")
            {
                string time = now.GetDateTimeFormats('t')[0];
                bunny.SpeakAsync("time");
            }
            else if (input == "what day is it")
            {
                bunny.SpeakAsync(DateTime.Today.ToString("dddd"));
            }
            else if (input == "what date is it")
            {
                bunny.SpeakAsync(DateTime.Today.ToString("dd-MM-yyyy"));
            }
            else if (input == "out of the way" || input == "offscreen")
            {
                if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Minimized;
                    bunny.SpeakAsync("My Apologies");
                }
            }

            else if (input == "come back" || input == "onscreen")
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    bunny.SpeakAsync("Onscreen Sir");
                    WindowState = FormWindowState.Normal;
                }
            }
            else if (input == "go fullscreen")
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                TopMost = true;
                bunny.SpeakAsync("Expanding sir");
            }
            else if (input == "exit  fullscreen")
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                TopMost = false;
                bunny.SpeakAsync("Exiting sir");
            }
            else if (input == "goodbye" || input == "bye" || input == "seeyou" || input == "go offline" || input == "close")
            {
                if (ranNum > 6)
                {
                    bunny.SpeakAsync("Farewell");
                    Close();
                }
                else
                {
                    bunny.SpeakAsync("GoodBye");
                    Close();
                }
            }
            else if (input == "shutdown")
            {
                System.Diagnostics.Process.Start("shutdown", "-s");
            }
            else if (input == "logoff")
            {
                System.Diagnostics.Process.Start("logoff", "-l");
            }
            else if (input == "restart")
            {
                System.Diagnostics.Process.Start("restart", "-r");
            }
        }
    }
}
