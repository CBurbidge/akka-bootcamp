using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
	        MyActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
	        var consoleWriterProps = Props.Create(() => new ConsoleWriterActor());
	        var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

			var validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
	        var validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

			var consoleReaderProps = Props.Create(() => new ConsoleReaderActor(validationActor));
	        var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

	        //var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps);
	        //var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)));
            
			// tell console reader to begin
			consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.AwaitTermination();
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }
    #endregion
}
