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
            var machine = new StateMachine<State, Command>();

            machine
                .In(State.Inactive).On(Command.Begin).MoveTo(State.Active)
                .In(State.Active).On(Command.Pause).MoveTo(State.Paused)
                .In(State.Paused).On(Command.Resume).MoveTo(State.Active).ThenExecute(() => Console.WriteLine("==> Resume called... <=="))
                .In(State.Active).On(Command.End).MoveTo(State.Inactive)
                .In(State.Paused).On(Command.End).MoveTo(State.Inactive)
                .In(State.Inactive).On(Command.Exit).MoveTo(State.Terminated).ThenExecute(() => Console.WriteLine("==> Exit called... <=="));

            var p = machine.GetMachine(State.Inactive);
            var r = machine.GetMachine(State.Paused);
            var s = machine.GetMachine(State.Terminated);

            Console.WriteLine($"Initial State = {p.CurrentState}");
            Console.WriteLine($"Begin = {p.CurrentState} -> {p.MoveNextState(Command.Begin)}");
            Console.WriteLine($"Pause = {p.CurrentState} -> {p.MoveNextState(Command.Pause)}");
            Console.WriteLine($"End = {p.CurrentState} -> {p.MoveNextState(Command.End)}");
            Console.WriteLine($"Exit = {p.CurrentState} -> {p.MoveNextState(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {r.CurrentState}");
            Console.WriteLine($"Resume = {r.MoveNextState(Command.Resume)}");
            Console.WriteLine($"End = {r.MoveNextState(Command.End)}");
            Console.WriteLine($"Exit = {r.MoveNextState(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {s.CurrentState}");
            Console.WriteLine($"Begin can be fired = {s.CommandIsValid(Command.Begin)}");

            try
            {
                s.MoveNextState(Command.Begin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadLine();
        }
    }
}
