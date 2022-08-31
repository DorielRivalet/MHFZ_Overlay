using System;
using System.Text;
using System.Threading;

namespace DiscordRPC.Example

{
    class Program
    {

        /// <summary>
        /// The level of logging to use.
        /// </summary>
        private static Logging.LogLevel logLevel = Logging.LogLevel.None;

        /// <summary>
        /// The pipe to connect too.
        /// </summary>
        private static int discordPipe = -1;

        /// <summary>
        /// The current presence to send to discord.
        /// </summary>
        private static RichPresence presence = new RichPresence()
        {
            Details = "In Mezeporta",
            State = "Waiting for quest",
            //check img folder
            Assets = new Assets()
            {
                LargeImageKey = "cattleya",
                LargeImageText = "Cattleya",
                SmallImageKey = "lance",
                SmallImageText = "Lance"
            },
            Buttons = new Button[]
                {
                    new Button() { Label = "Join Server", Url = "" }
                }
        };

        /// <summary>
        /// The discord client
        /// </summary>
        private static DiscordRpcClient client;

        /// <summary>
        /// Is the main loop currently running?
        /// </summary>
        private static bool isRunning = true;

        /// <summary>
        /// The string builder for the command
        /// </summary>
        private static StringBuilder word = new StringBuilder();


        //Main Loop
        static void Main(string[] args)
        {
            //Reads the arguments for the pipe
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-pipe":
                        discordPipe = int.Parse(args[++i]);
                        break;

                    default: break;
                }
            }

            //Seting a random details to test the update rate of the presence
            FullClientExample();
            //Issue104();
            //IssueMultipleSets();
            //IssueJoinLogic();

            Console.WriteLine("Press any key to terminate");
            Console.ReadKey();
        }

        static void FullClientExample()
        {
            //Create a new DiscordRpcClient. We are filling some of the defaults as examples.
            using (client = new DiscordRpcClient("",          //The client ID of your Discord Application
                    pipe: discordPipe,                                          //The pipe number we can locate discord on. If -1, then we will scan.
                    logger: new Logging.ConsoleLogger(logLevel, true),          //The loger to get information back from the client.
                    autoEvents: true,                                           //Should the events be automatically called?
                    client: new IO.ManagedNamedPipeClient()                     //The pipe client to use. Required in mono to be changed.
                ))
            {
                //If you are going to make use of the Join / Spectate buttons, you are required to register the URI Scheme with the client.
                client.RegisterUriScheme();

                //Give the game some time so we have a nice countdown
                presence.Timestamps = Timestamps.Now;

                //Set some new presence to tell Discord we are in a game.
                // If the connection is not yet available, this will be queued until a Ready event is called, 
                // then it will be sent. All messages are queued until Discord is ready to receive them.
                client.SetPresence(presence);

                //Initialize the connection. This must be called ONLY once.
                //It must be called before any updates are sent or received from the discord client.
                client.Initialize();

                //Start our main loop. In a normal game you probably don't have to do this step.
                // Just make sure you call .Invoke() or some other dequeing event to receive your events.
                MainLoop();
            }
        }
        static void MainLoop()
        {
            /*
			 * Enter a infinite loop, polling the Discord Client for events.
			 * In game termonology, this will be equivalent to our main game loop. 
			 * If you were making a GUI application without a infinite loop, you could implement
			 * this with timers.
			*/
            isRunning = true;
            while (client != null && isRunning)
            {
                //We will invoke the client events. 
                // In a game situation, you would do this in the Update.
                // Not required if AutoEvents is enabled.
                //if (client != null && !client.AutoEvents)
                //	client.Invoke();

                //This can be what ever value you want, as long as it is faster than 30 seconds.
                //Console.Write("+");
                Thread.Sleep(25);

                client.SetPresence(presence);
            }

            Console.WriteLine("Press any key to terminate");
            Console.ReadKey();
        }
    }
}