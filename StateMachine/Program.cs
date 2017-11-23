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

            machine.AddTransition(Command.Begin, State.Inactive, State.Active);
            machine.AddTransition(Command.Pause, State.Active, State.Paused);
            machine.AddTransition(Command.Resume, State.Paused, State.Active);
            machine.AddTransition(Command.End, State.Active, State.Inactive);
            machine.AddTransition(Command.End, State.Paused, State.Inactive);
            machine.AddTransition(Command.Exit, State.Inactive, State.Terminated);

            var p = machine.GetMachine(State.Inactive);
            var r = machine.GetMachine(State.Paused);
            var s = machine.GetMachine(State.Terminated);

            Console.WriteLine($"Initial State = {p.CurrentProcessState}");
            Console.WriteLine($"Begin = {p.CurrentProcessState} -> {p.MoveNextState(Command.Begin)}");
            Console.WriteLine($"Pause = {p.CurrentProcessState} -> {p.MoveNextState(Command.Pause)}");
            Console.WriteLine($"End = {p.CurrentProcessState} -> {p.MoveNextState(Command.End)}");
            Console.WriteLine($"Exit = {p.CurrentProcessState} -> {p.MoveNextState(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {r.CurrentProcessState}");
            Console.WriteLine($"Resume = {r.MoveNextState(Command.Resume)}");
            Console.WriteLine($"End = {r.MoveNextState(Command.End)}");
            Console.WriteLine($"Exit = {r.MoveNextState(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {s.CurrentProcessState}");
            Console.WriteLine($"Begin is a valid next state = {s.NextStateIsValid(Command.Begin)}");

            Console.ReadLine();
        }
    }
}
