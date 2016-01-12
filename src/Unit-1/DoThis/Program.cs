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

	        var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
	        var tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

			var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCoordinatorActor));
	        var fileValidatorActor = MyActorSystem.ActorOf(fileValidatorActorProps, "fileValidatorActor");

			var consoleReaderProps = Props.Create(() => new ConsoleReaderActor(fileValidatorActor));
	        var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");
   
			// tell console reader to begin
			consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.AwaitTermination();
        }
    }
    #endregion
}
