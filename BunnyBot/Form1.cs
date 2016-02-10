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
        bool wake = false;
        string myComputer = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
        string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string myMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        string myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        string myVideos = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        string myDownloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";

        public form1()
        {
            InitializeComponent();
            //user.SetInputToDefaultAudioDevice();
        }


        private void form1_Load(object sender, EventArgs e)
        {
            Choices commands = new Choices();
            commands.Add(new string[] { "sleep","wake up bunny","hide debug","debug","open mydownloads", "open mydocuments", "open mypictures", "open mymusic", "open myvideos", "open mycomputer", "guess what", "how old are you","what is your age bunny","can i change your name","what is love" ,"do you believe in love","tell me a story","what is my name", "how are you","hai", "hello bunny", "hello", "bunny", "hey", "what is my name" , "what time is it" , "what day is it", "what date is it" ,"out of my way","off screen","comeback","on screen", "go fullscreen", "exit fullscreen","shutdown","log off","restart","goodbye","bye","see you","close" });
            GrammarBuilder gbuilder = new GrammarBuilder();
            gbuilder.Append(commands);
            Grammar grammer = new Grammar(gbuilder);
            user.LoadGrammarAsync(grammer);
            user.SetInputToDefaultAudioDevice();
            pictureBox1.ImageLocation = "C:\\Users\\RAMYA\\Documents\\Visual Studio 2015\\Projects\\speech\\BunnyBot\\BunnyBot\\Resources\\BunnyBot.gif";
            user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(User_SpeechRecognized);
            user.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void speak(string recognizedVoice)
        {
            bunny.SpeakAsync(recognizedVoice);
            outputText.AppendText(recognizedVoice + "\n");
        }


        private void User_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text;
            Random rnd = new Random();
            //DateTime now = new DateTime()

            //AdvancedCommands
            if (input == "wake up bunny")
            {
                wake = true;
                label1.Text = "State : Awake";
                speak("I am online and ready to execute your commands");
            }

            if (input == "sleep")
            {
                wake = false;
                label1.Text = "State : Sleep";
                speak("wake me up sir again");
            }

            if (wake)
            {

               if (input == "debug")
                {
                    inputText.Visible = true;
                    outputText.Visible = true;
                }

                else if (input == "hide debug")
                {
                    inputText.Visible = false;
                    outputText.Visible = false;
                }

                //Social commands ;)
                else if (input == "hai" || input == "hello bunny" || input == "hello")
                {

                    if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 12)
                    {
                        speak("Good Morning" + userName);
                    }
                    if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                    {
                        speak("Good Afternoon " + userName);
                    }
                    if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 24)
                    {
                        speak("Good Evening " + userName);
                    }
                    if (DateTime.Now.Hour < 5)
                    {
                        speak("Hello " + userName + ", it's getting late");
                    }
                }

                else if (input == "bunny" || input == "hey")
                {
                    ranNum = rnd.Next(1, 5);
                    if (ranNum == 1)
                    {
                        speak("Yes sir");
                    }
                    else if (ranNum == 2)
                    {
                        speak("Yes?");
                    }
                    else if (ranNum == 3)
                    {
                        speak("How may I help?");
                    }
                    else if (ranNum == 4)
                    {
                        speak("How may I be of assistance?");
                    }
                }

                else if (input == "how are you")
                {
                    speak("Quiet well. Thanks for asking");
                }

                else if (input == "what is my name")
                {
                    speak(" " + userName.ToString());
                }

                else if (input == "tell me a story")
                {
                    ranNum = rnd.Next(1, 4);
                    if (ranNum == 1)
                    {
                        speak("Once upon a time a protagonist set out. An antagonist attempted to thwart her. There was rising action! Drama! And then through heroic action, the conflict was resolved. The End!");
                    }
                    else if (ranNum == 2)
                    {
                        speak("Once there was a beginning. Soon after, there was a middle. The End!! Funny isn't it ");
                    }
                    else if (ranNum == 3)
                    {
                        speak("Why didn't the spider go to the school? Because she learned everything on web!");
                    }
                }

                else if (input == "what is love" || input == "do you believe in love")
                {
                    speak("I'll need quiet a few upgrades before I can give you a heartfelt answer");
                }

                else if (input == "can i change your name")
                {
                    speak("What if i started you calling thumbs? Lets stick with what we've got");
                }

                else if (input == "how old are you" || input == "what is your age bunny")
                {
                    speak("By your calender, I'm still in infancy. In bot year's im quiet mature");
                }

                else if (input == "guess what")
                {
                    speak("There are 2,335,981,212,665 possible anwsers for that question");
                }

                //DateTime commands
                else if (input == "what time is it")
                {
                    //string time = now.GetDateTimeFormats('t')[0];
                    speak(DateTime.Now.ToString("h:mm tt"));
                }

                else if (input == "what day is it")
                {
                    speak(DateTime.Today.ToString("dddd"));
                }

                else if (input == "what date is it")
                {
                    speak(DateTime.Today.ToString("dd-MM-yyyy"));
                }

                //FormControl commands
                else if (input == "out of the way" || input == "offscreen")
                {
                    if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Minimized;
                        speak("My Apologies");
                    }
                }

                else if (input == "come back" || input == "onscreen")
                {
                    if (WindowState == FormWindowState.Minimized)
                    {
                        speak("Onscreen Sir");
                        WindowState = FormWindowState.Normal;
                    }
                }

                else if (input == "go fullscreen")
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;
                    speak("Expanding sir");
                }

                else if (input == "exit  fullscreen")
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;
                    speak("Exiting sir");
                }

                //ClosingForm commands
                else if (input == "goodbye" || input == "bye" || input == "seeyou" || input == "go offline" || input == "close")
                {
                    if (ranNum > 6)
                    {
                        speak("Farewell");
                        Close();
                    }
                    else
                    {
                        speak("GoodBye");
                        Close();
                    }
                }

                //SystemCommands

                else if (input == "open mycomputer")
                {
                    speak("As you wish");
                    Process.Start("explorer", myComputer);
                }

                else if (input == "open mymusic")
                {
                    speak("As you wish");
                    Process.Start("explorer", myMusic);
                }

                else if (input == "open mypictures")
                {
                    speak("As you wish");
                    Process.Start("explorer", myPictures);
                }

                else if (input == "open myvideos")
                {
                    speak("As you wish");
                    Process.Start("explorer", myVideos);
                }

                else if (input == "open mydocuments")
                {
                    speak("As you wish");
                    Process.Start("explorer", myDocuments);
                }

                else if (input == "open mydownloads")
                {
                    speak("okay");
                    Process.Start("explorer", myDownloads);
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

                inputText.AppendText(input + "\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            help obj = new help();
            obj.Show();
        }
    }
}
