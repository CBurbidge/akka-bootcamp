using System;
using Akka.Actor;

namespace WinTail
{
    class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
	        var msg = message as string;

	        if (string.IsNullOrEmpty(msg))
	        {
		        Self.Tell(new Messages.NullInputError("No input received."));
	        }
	        else
	        {
		        var valid = IsValid(msg);
		        if (valid)
		        {
			        _consoleWriterActor.Tell(new Messages.InputSuccess("Message valid :)."));
		        }
		        else
		        {
			        _consoleWriterActor.Tell(new Messages.ValidationError("Had odd number of chars, bad for some reason."));
		        }
	        }

			Sender.Tell(new Messages.ContinueProcessing());
		}

	    private bool IsValid(string message)
	    {
			var valid = message.Length % 2 == 0;
			return valid;
		}
    }
}