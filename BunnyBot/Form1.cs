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
using System.Xml;
using BunnyBot.Properties;    

namespace BunnyBot
{
    public partial class form1 : Form
    {
        public static SpeechRecognitionEngine user = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
        public static SpeechSynthesizer bunny = new SpeechSynthesizer();
        public static String userName = Environment.UserName;
        private int ranNum;
        bool wake = false;
        int count = 1;
        int i = 0;
        public static String Temperature, Condition, Humidity, WinSpeed, TFCond, TFHigh, TFLow, Town;
        Grammar shellcommandgrammar; //Grammar variables allow us to load and unload words into vocabulary and update them during runtime without the need to restart the program
        Grammar webcommandgrammar;
        Grammar socialcommandgrammar;
        String[] ArrayShellCommands; //These arrays will be loaded with custom commands, responses, and File Locations/URLs
        String[] ArrayShellResponse;
        String[] ArrayShellLocation;
        String[] ArrayWebCommands;
        String[] ArrayWebResponse;
        String[] ArrayWebURL;
        String[] ArraySocialCommands;
        String[] ArraySocialResponse;
        public static string scpath; //These strings will be used to refer to the Shell Command text document
        public static string srpath; //These strings will be used to refer to the Shell Response text document
        public static string slpath; //These strings will be used to refer to the Shell Location text document
        public static string webcpath; //These strings will be used to refer to the Web Command text document
        public static string webrpath; //These strings will be used to refer to the Web Response text document
        public static string weblpath; //These strings will be used to refer to the Web URL text document
        public static string socialcpath; //These strings will be used to refer to the Social Command text document
        public static string socialrpath; //These strings will be used to refer to the Social respond text document
        public static String QEvent;
        StreamWriter sw;


        public form1()
        {

            InitializeComponent();

            //Create Directory
            Directory.CreateDirectory(@"C:\Users\" + userName + "\\Documents\\DB"); //We create 'DB(dataBase)' folder in the My Documents folder so we have a place to store our text documents

            Settings.Default.ShellC = @"C:\Users\" + userName + "\\Documents\\DB\\Shell Commands.dat"; //We save the text document file locations into our settings even before they've been created so we can refer to them easily and globally
            Settings.Default.ShellR = @"C:\Users\" + userName + "\\Documents\\DB\\Shell Response.dat";
            Settings.Default.ShellL = @"C:\Users\" + userName + "\\Documents\\DB\\Shell Location.dat";
            Settings.Default.WebC = @"C:\Users\" + userName + "\\Documents\\DB\\Web Commands.dat";
            Settings.Default.WebR = @"C:\Users\" + userName + "\\Documents\\DB\\Web Response.dat";
            Settings.Default.WebL = @"C:\Users\" + userName + "\\Documents\\DB\\Web URL.dat";
            Settings.Default.SocC = @"C:\Users\" + userName + "\\Documents\\DB\\Social Commands.dat";
            Settings.Default.SocR = @"C:\Users\" + userName + "\\Documents\\DB\\Social Response.dat";
            Settings.Default.Save();

            scpath = Settings.Default.ShellC; //The text document file locations are passed on to these variables because they are easier to refer to but admittedly is an unnecessary step
            srpath = Settings.Default.ShellR;
            slpath = Settings.Default.ShellL;
            webcpath = Settings.Default.WebC;
            webrpath = Settings.Default.WebR;
            weblpath = Settings.Default.WebL;
            socialcpath = Settings.Default.SocC;
            socialrpath = Settings.Default.SocR;

            if (!File.Exists(scpath)) //This is used to create the Custom Command text documents if they don't already exist and write in default commands so we don't encounter any errors. These text documents should always have at least one valid line in them
            {
                sw = File.CreateText(scpath);
                sw.Write("My Documents");
                sw.Close();
            }

            if (!File.Exists(srpath))
            {
                sw = File.CreateText(srpath);
                sw.Write("Right away");
                sw.Close();
            }

            if (!File.Exists(slpath))
            {
                sw = File.CreateText(slpath);
                sw.Write(@"C:\Users\" + userName + "\\Documents");
                sw.Close();
            }

            if (!File.Exists(webcpath))
            {
                sw = File.CreateText(webcpath);
                sw.Write("Open Google");
                sw.Close();
            }

            if (!File.Exists(webrpath))
            {
                sw = File.CreateText(webrpath);
                sw.Write("Very well");
                sw.Close();
            }

            if (!File.Exists(weblpath))
            {
                sw = File.CreateText(weblpath);
                sw.Write("http://www.google.com");
                sw.Close();
            }

            if (!File.Exists(socialcpath))
            {
                sw = File.CreateText(socialcpath);
                sw.Write("How are you");
                sw.Close();
            }

            if (!File.Exists(socialrpath))
            {
                sw = File.CreateText(socialrpath);
                sw.Write("Quiet well. Thanks for asking");
                sw.Close();
            }
                        
            ArrayShellCommands = File.ReadAllLines(scpath); //This loads all written commands in our Custom Commands text documents into arrays so they can be loaded into our grammars
            ArrayShellResponse = File.ReadAllLines(srpath);
            ArrayShellLocation = File.ReadAllLines(slpath);
            ArrayWebCommands = File.ReadAllLines(webcpath); //This loads all written commands in our Custom Commands text documents into arrays so they can be loaded into our grammars
            ArrayWebResponse = File.ReadAllLines(webrpath);
            ArrayWebURL = File.ReadAllLines(weblpath);
            ArraySocialCommands = File.ReadAllLines(socialcpath); //This loads all written commands in our Custom Commands text documents into arrays so they can be loaded into our grammars
            ArraySocialResponse = File.ReadAllLines(socialrpath);

            try
            {
                shellcommandgrammar = new Grammar(new GrammarBuilder(new Choices(ArrayShellCommands)));
                user.LoadGrammarAsync(shellcommandgrammar);
            }
            catch
            {
                bunny.SpeakAsync("I've detected an in valid entry in your shell commands, possibly a blank line. Shell commands will cease to work until it is fixed.");
            }

            try
            {
                webcommandgrammar = new Grammar(new GrammarBuilder(new Choices(ArrayWebCommands)));
                user.LoadGrammarAsync(webcommandgrammar);
            }
            catch
            {
                bunny.SpeakAsync("I've detected an in valid entry in your web commands, possibly a blank line. Web commands will cease to work until it is fixed.");
            }

            try
            {
                socialcommandgrammar = new Grammar(new GrammarBuilder(new Choices(ArraySocialCommands)));
                user.LoadGrammarAsync(socialcommandgrammar);
            }
            catch
            {
                bunny.SpeakAsync("I've detected an in valid entry in your social commands, possibly a blank line. Social commands will cease to work until it is fixed.");
            }
            
            //user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(PlayFile_SpeechRecognized);
            // user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(AlarmClock_SpeechRecognized);
            // user.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(_recognizer_AudioLevelUpdated);
            //user.SpeechRecognized +=new EventHandler<SpeechRecognizedEventArgs>(User_SpeechRecognized);
            //user.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(User_SpeechDetected);
            //user.SetInputToDefaultAudioDevice();
            //user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(user_SpeechRecognized); //The event handler that allows us to say "JARVIS Come Back Online". Only one Speech Recognition Engine is active at a time.
            //Process.Start(@"C:\\Users\\RAMYA\\Downloads\\Speech-master\\BunnyBot\\Resources\\Intro.mp4");
            //axWindowsMediaPlayer1.uiMode = "none";
        }
             
        public static void GetWeather()
        {
            try
            {
                string query = String.Format("http://weather.yahooapis.com/forecastrss?w=" + Settings.Default.WOEID.ToString() + "&u=" + Settings.Default.Temperature);
                XmlDocument wData = new XmlDocument();
                wData.Load(query);

                XmlNamespaceManager man = new XmlNamespaceManager(wData.NameTable);
                man.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

                XmlNode channel = wData.SelectSingleNode("rss").SelectSingleNode("channel");
                XmlNodeList nodes = wData.SelectNodes("/rss/channel/item/yweather:forecast", man);

                Temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", man).Attributes["temp"].Value;

                Condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", man).Attributes["text"].Value;

                Humidity = channel.SelectSingleNode("yweather:atmosphere", man).Attributes["humidity"].Value;

                WinSpeed = channel.SelectSingleNode("yweather:wind", man).Attributes["speed"].Value;

                Town = channel.SelectSingleNode("yweather:location", man).Attributes["city"].Value;

                TFCond = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", man).Attributes["text"].Value;

                TFHigh = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", man).Attributes["high"].Value;

                TFLow = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", man).Attributes["low"].Value;

                QEvent = "connected";
            }
            catch
            {
                QEvent = "failed";
            }
        }
      
        void playvideo()
        {
            //axWindowsMediaPlayer1.Visible = true;
            //axWindowsMediaPlayer1.uiMode = "none";
            user.RecognizeAsyncCancel();
            axWindowsMediaPlayer1.URL = "Resources\\Intro.mp4";
            //axWindowsMediaPlayer1.URL = "C:\\Users\\" +userName+ "\\Downloads\\Speech-master\\BunnyBot\\Resources\\Intro.mp4";
            //bunny.SelectVoiceByHints(VoiceGender.Male);
            //bunny.SpeakAsync(userName.ToString());
            bunny.SpeakAsync("Hello " + userName.ToString() + " I am Bunny. The voice of virtual intelligence. Let me make a room to reside in your memory. I will be your digital personal Assistant ");
            axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(axWindowsMediaPlayer1_PlayStateChange);   

        }

        void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                axWindowsMediaPlayer1.Visible = false;
                user.RecognizeAsync(RecognizeMode.Multiple);
                //this.WindowState = FormWindowState.Normal;
                //this.WindowStyle = WindowStyle.SingleBorderWindow;
                //this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        public void speak(string recognizedVoice)
        {
            
            outputText.AppendText(recognizedVoice + "\n");
            bunny.SpeakAsync(recognizedVoice);
        }

        private void form1_Load(object sender, EventArgs e)
        {
            playvideo();

            //user.SetInputToDefaultAudioDevice();

            //user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(user_SpeechRecognized);
            // user.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(User_SpeechRecognitionRejected);
            //user.RecognizeAsync(RecognizeMode.Multiple);
            //axWindowsMediaPlayer1.Visible = true;
            //axWindowsMediaPlayer1.Visible = false;
            //axWindowsMediaPlayer1.fullScreen = false;
            //Choices commands = new Choices();
            //commands.Add(new string[] { "whats the weather like", "whats the temperatures like", "open windows defender", "open paint", "open notepad", "open powerpoint", "open excel", "open wordpad", "open windows media player", "switch window", "close window", "close salutation", "open salutation", "open mydownloads", "open mydocuments", "open mypictures", "open mymusic", "open myvideos", "open mycomputer", "hide debug", "debug", "hide commands", "show me basic commands", "shutup", "wake up bunny", "sleep", "hai", "hello", "bunny", "hey", "how are you", "my name", "my name bunny", "tell me a story", "what is love", "do you believe in love", "can i change your name", "how old are you", "what is your age bunny", "guess what", "time is it", "what time is it", "day is it", "what date is it", "out of my way", "offscreen", "off screen", "come back", "onscreen", "on screen", "fullscreen", "full screen", "exit fullscreen", "exit full screen", "goodbye", "bye", "see you", "bye", "go offline", "shutdown", "restart", "logoff", "open google", "open facebook" });
            //GrammarBuilder gbuiler = new GrammarBuilder();
            //gbuiler.Append(commands);
            //Grammar grammer = new Grammar(gbuiler);
            //user.LoadGrammarAsync(grammer);
            //user.SetInputToDefaultAudioDevice();
            pictureBox1.ImageLocation = "Resources\\BunnyBot.gif";
            string[] commands=File.ReadAllLines(Environment.CurrentDirectory +"\\Commands.txt");
            user.LoadGrammar(new Grammar(new GrammarBuilder(new Choices(commands))));
            user.SetInputToDefaultAudioDevice();
            user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Shell_SpeechRecognized); //These are event handlers that are responsible for carrying out all necessary tasks if a speech event is recognized
            user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Social_SpeechRecognized);
            user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Web_SpeechRecognized);
            user.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(User_SpeechRecognized);
            user.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(User_AudioLevelUpdated);
            user.RecognizeAsync(RecognizeMode.Multiple);
            user.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(User_SpeechRecognitionRejected);
            //user.SpeechRecognized +=new EventHandler<SpeechRecognizedEventArgs>(User_SpeechRecognized);
            //bunny.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
        }

        private void User_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            progressBar1.Value = e.AudioLevel;
        }

        private void Shell_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text; //Sets the SpeechRecognized event variable to a string variable called speech
            i = 0; //Ensures "i" is = to 0 so we can start our loop from the beginning of our arrays
            try
            {
                foreach (string line in ArrayShellCommands)
                {
                    if (line == speech) //If line == speech it will open the corresponding program/file
                    {
                        System.Diagnostics.Process.Start(ArrayShellLocation[i]); //Opens the program/file of the same elemental position as the ArrayShellCommands command that was equal to speech
                        speak(ArrayShellResponse[i]); //Gives the response of the same elemental position as the ArrayShellCommands command that was equal to speech
                    }
                    i += 1; //if the line in ArrayShellCommands does not equal speech it will add 1 to "i" and go through the loop until it finds a match between the line and spoken event
                }
                inputText.AppendText(speech + "\n");
            }
            catch
            {
                i += 1;
                speak("Im sorry it appears the shell command " + speech + " on line " + i + " is accompanied by either a blank line or an incorrect file location");
            }
        }
        
        private void Social_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            i = 0;
            try
            {
                foreach (string line in ArraySocialCommands)
                {
                    if (line == speech)
                    {
                        speak(ArraySocialResponse[i]);
                    }
                    i += 1;
                }
                inputText.AppendText(speech + "\n");
            }
            catch
            {
                i += 1;
                speak("Please check the " + speech + " social command on line " + i + ". It appears to be missing a proper response");
            }
        }
        
        private void Web_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            i = 0;
            try
            {
                foreach (string line in ArrayWebCommands)
                {
                    if (line == speech)
                    {
                        System.Diagnostics.Process.Start(ArrayWebURL[i]);
                        speak(ArrayWebResponse[i]);
                    }
                    i += 1;
                }
                inputText.AppendText(speech + "\n");
            }
            catch
            {
                i += 1;
                speak("Please check the " + speech + "web command on line " + i + ". It appears to be missing a proper response or web U R L");
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            bunny.SpeakAsyncCancelAll();
            bunny.SpeakAsync("HELP");

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            bunny.SpeakAsyncCancelAll();
            bunny.SpeakAsync("SETTINGS");
        }

        private void User_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Random r = new Random();
            ranNum = r.Next(1, 3);
            if (ranNum < 2)
            {
                speak("I thought you would never ask that! So i never learnt that!");
            }
            else if (ranNum > 2)
            {
                speak("Sorry, I am out of bound! I will look into it and i will surely learn it soon.");
            }
        }

        private void User_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string input = e.Result.Text;
            Random rnd = new Random();
            ranNum = rnd.Next(1, 6);

            //Advanced Commands 
            if (input == "wake up bunny")
            {
                wake = true;
                label3.Text = " State : Awake";
                speak("I am online and ready to execute your commands");
            }

            if (input == "sleep")
            {
                wake = false;
                label3.Text = "State : Sleep";
                speak("wake me up again");
            }

            if (wake)
            {
                if (input == "i want to add custom commands")
                {
                    customize cs = new customize();
                    cs.ShowDialog();
                }

                else if (input == "update commands")
                {
                    speak("This may take a few seconds");
                    user.UnloadGrammar(shellcommandgrammar);
                    user.UnloadGrammar(webcommandgrammar);
                    user.UnloadGrammar(socialcommandgrammar);
                    ArrayShellCommands = File.ReadAllLines(scpath);
                    ArrayShellResponse = File.ReadAllLines(srpath);
                    ArrayShellLocation = File.ReadAllLines(slpath);
                    ArrayWebCommands = File.ReadAllLines(webcpath);
                    ArrayWebResponse = File.ReadAllLines(webrpath);
                    ArrayWebURL = File.ReadAllLines(weblpath);
                    ArraySocialCommands = File.ReadAllLines(socialcpath);
                    ArraySocialResponse = File.ReadAllLines(socialrpath);
                    try
                    {
                        shellcommandgrammar = new Grammar(new GrammarBuilder(new Choices(ArrayShellCommands)));
                        user.LoadGrammar(shellcommandgrammar);
                    }
                    catch
                    {
                        bunny.SpeakAsync("I've detected an in valid entry in your shell commands, possibly a blank line. Shell commands will cease to work until it is fixed.");
                    }
                    try
                    {
                        webcommandgrammar = new Grammar(new GrammarBuilder(new Choices(ArrayWebCommands)));
                        user.LoadGrammar(webcommandgrammar);
                    }
                    catch
                    {
                        bunny.SpeakAsync("I've detected an in valid entry in your web commands, possibly a blank line. Web commands will cease to work until it is fixed.");
                    }
                    try
                    {
                        socialcommandgrammar = new Grammar(new GrammarBuilder(new Choices(ArraySocialCommands)));
                        user.LoadGrammar(socialcommandgrammar);
                    }
                    catch
                    {
                        bunny.SpeakAsync("I've detected an in valid entry in your social commands, possibly a blank line. Social commands will cease to work until it is fixed.");
                    }
                    bunny.SpeakAsync("All commands updated");
                }

                else if (input == "hows the weather" || input == "whats the weather like")
                {
                    GetWeather();
                    if (QEvent == "connected")
                    {
                        speak("The weather in " + Town + " is " + Condition + " at " + Temperature + " degrees. There is a humidity of " + Humidity + " and a windspeed of " + WinSpeed + " miles per hour");
                    }
                    else if (QEvent == "failed")
                    {
                        speak("I seem to be having a bit of trouble connecting to the server. Just look out the window");
                    }
                }

                else if (input == "whats tomorrows forecast")
                {
                    GetWeather();
                    if (QEvent == "connected")
                    {
                        speak("Tomorrows forecast is " + TFCond + " with a high of " + TFHigh + " and a low of " + TFLow);
                    }
                    else if (QEvent == "failed")
                    {
                        speak("I could not access the server, are you sure you have the right W O E I D?");
                    }
                }

                else if (input == "whats the temperatures outside")
                {
                    GetWeather();
                    if (QEvent == "connected")
                    {
                        speak(Temperature + " degrees");
                    }
                    else if (QEvent == "failed")
                    {
                        speak("I could not connect to the weather service");
                    }
                }

                else if (input == "show basic commands")
                {
                    speak("Displaying the basic commands! ");
                    Commands.Visible = true;
                    string[] BasicCommands = (File.ReadAllLines(@"Commands.txt"));
                    Commands.Items.Clear();
                    Commands.SelectionMode = SelectionMode.None;
                    foreach (string command in BasicCommands)
                    {
                        Commands.Items.Add(command);
                    }
                }

                else if (input == "show shell commands")
                {
                    speak("Displaying the shell commands");
                    Commands.Items.Clear();
                    Commands.SelectionMode = SelectionMode.None;
                    foreach (string command in ArrayShellCommands)
                    {
                        Commands.Items.Add(command);
                    }
                }

                else if (input == "show social commands")
                {
                    speak("Displaying the social commands");
                    Commands.Items.Clear();
                    Commands.SelectionMode = SelectionMode.None;
                    foreach (string command in ArraySocialCommands)
                    {
                        Commands.Items.Add(command);
                    }
                }

                else if (input == "show web commands")
                {
                    speak("Displaying the web commands");
                    Commands.Items.Clear();
                    Commands.SelectionMode = SelectionMode.None;
                    foreach (string command in ArrayWebCommands)
                    {
                        Commands.Items.Add(command);
                    }
                }

                else if (input == "hide basic commands" || input == "hide shell commands" || input == "hide social commands" || input == "hide web commands")
                {
                    speak("Alright!");
                    Commands.Visible = false;
                }

                else if (input == "debug")
                {
                    speak("Debugging mode activated ");
                    inputText.Visible = true;
                    outputText.Visible = true;
                }

                else if (input == "hide debug")
                {
                    speak("Debugging is off ");
                    inputText.Visible = false;
                    outputText.Visible = false;
                }

                /*else if (input == "open mycomputer")
                {
                    speak("Opening My computer ");
                    Process.Start("explorer", myComputer);
                }

                else if (input == "open mymusic")
                {
                    speak("Opening My Music ");
                    Process.Start("explorer", myMusic);
                }

                else if (input == "open mypictures")
                {
                    speak("Opening My Pictures ");
                    Process.Start("explorer", myPictures);
                }

                else if (input == "open myvideos")
                {
                    speak("Opening My Videos ");
                    Process.Start("explorer", myVideos);
                }

                else if (input == "open mydocuments")
                {
                    speak("Opening My Documents ");
                    Process.Start("explorer", myDocuments);
                }
                else if (input == "open mydownloads")
                {
                    speak("Opening My Downloads ");
                    Process.Start("explorer", myDownloads);
                }*/

                //Social commands
                else if (input == "hai" || input == "hello")
                {
                    if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 12)
                    {
                        speak("Good Morning" + userName);
                    }
                    else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                    {
                        speak("Good Afternoon " + userName);
                    }
                    else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 24)
                    {
                        speak("Good Evening " + userName);
                    }
                    else if (DateTime.Now.Hour < 5)
                    {
                        speak("Hello " + userName + ", it's getting late");
                    }
                }

                else if (input == "bunny" || input == "hey")
                {
                    ranNum = rnd.Next(1, 5);
                    if (ranNum == 1)
                    {
                        speak("Yes ");
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

                else if (input == "whats my name")
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

                else if (input == "what time is it")
                {
                    //string time = now.GetDateTimeFormats('t')[0];
                    speak(DateTime.Now.ToString("h:mm tt"));
                }

                else if (input == "what day is it")
                {
                    speak(DateTime.Now.ToString("dddd"));
                }

                else if (input == "what date is it")
                {
                    speak(DateTime.Now.ToString("dd-MM-yyyy"));
                }

                else if (input == "shutup")
                {
                    bunny.SpeakAsyncCancelAll();
                    speak("Sorry!");
                }

                //windows form controllers
                else if (input == "out of my way" || input == "off screen")
                {
                    if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Minimized;
                        speak("My Apologies");
                    }
                }

                else if (input == "come back" || input == "on screen")
                {
                    if (WindowState == FormWindowState.Minimized)
                    {
                        speak("Onscreen ");
                        WindowState = FormWindowState.Normal;
                    }
                }

                else if (input == "go fullscreen")
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    TopMost = true;
                    speak("Expanding");
                }

                else if (input == "exit fullscreen")
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                    TopMost = false;
                    speak("Exiting ");

                }
                else if (input == "invisible")
                {
                    if (ActiveForm.Visible == true)
                    {
                        ActiveForm.ShowInTaskbar = false;
                        ActiveForm.Hide();
                        speak("I am now invisible. You can access me by clicking on the icon down here in the tray.");
                    }
                }

                /*
                //browser commands
                else if (input == "open google")
                {
                    speak("Diving to google");
                    Process.Start("http://www.google.com");
                }

                else if (input == "open facebook")
                {
                    speak("What a dumb choice? But still i will open it ");
                    Process.Start("http://www.facebook.com");
                }*/

                //closing application
                else if (input.Contains("goodbye") || input.Contains("see you") || input.Contains("bye") || input.Contains("go offline"))
                {
                    if (ranNum > 3)
                    {
                        speak("Farewell");
                        //Close();
                    }
                    else
                    {
                        speak("Goodbye");
                        //Close();
                    }
                }

                else if (input == "switch window")
                {
                    SendKeys.SendWait("%{TAB " + count + "}");
                    count += 1;
                }

                else if (input == "close window")
                {
                    SendKeys.SendWait("%{F4}");
                }

                else if (input == "shutdown")
                {
                    //System.Diagnostics.Process.Start("shutdown", "-s");
                }

                else if (input == "restart")
                {
                    //System.Diagnostics.Process.Start("shutdown", "-r");

                }

                else if (input == "logoff")
                {
                    //System.Diagnostics.Process.Start("shutdown", "-l");
                }

                inputText.AppendText(input + "\n");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            help obj = new help();
            obj.ShowDialog();
        }
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        private void basicCommands_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            customize form2 = new customize();
            form2.ShowDialog();
        }
    }
}
