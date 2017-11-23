using StateMachine.Library;
using System;

namespace StateMachine
{
    public enum State
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum Command
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }

    public class Program
    {
        public static void Main()
        {
            var stateMachine = new StateMachine<State, Command>();

            stateMachine
                .In(State.Inactive).On(Command.Begin).MoveTo(State.Active)
                .In(State.Active).On(Command.Pause).MoveTo(State.Paused)
                .In(State.Paused).On(Command.Resume).MoveTo(State.Active).ThenExecute(() => Console.WriteLine("==> Resume called... <=="))
                .In(State.Active).On(Command.End).MoveTo(State.Inactive)
                .In(State.Paused).On(Command.End).MoveTo(State.Inactive)
                .In(State.Inactive).On(Command.Exit).MoveTo(State.Terminated).ThenExecute(() => Console.WriteLine("==> Exit called... <=="));

            var p = stateMachine.Create(State.Inactive);
            var r = stateMachine.Create(State.Paused);
            var s = stateMachine.Create(State.Terminated);

            Console.WriteLine($"Initial State = {p.CurrentState}");
            Console.WriteLine($"Begin = {p.CurrentState} -> {p.MoveNext(Command.Begin)}");
            Console.WriteLine($"Pause = {p.CurrentState} -> {p.MoveNext(Command.Pause)}");
            Console.WriteLine($"End = {p.CurrentState} -> {p.MoveNext(Command.End)}");
            Console.WriteLine($"Exit = {p.CurrentState} -> {p.MoveNext(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {r.CurrentState}");
            Console.WriteLine($"Resume = {r.MoveNext(Command.Resume)}");
            Console.WriteLine($"End = {r.MoveNext(Command.End)}");
            Console.WriteLine($"Exit = {r.MoveNext(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {s.CurrentState}");
            Console.WriteLine($"Begin can be fired = {s.TransitionIsValid(Command.Begin)}");

            try
            {
                s.MoveNext(Command.Begin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadLine();
        }
    }
}
