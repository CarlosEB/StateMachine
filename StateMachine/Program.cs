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
            var stateMachine = StateMachine<State, Command>.Create();

            var a = 11;

            stateMachine
                .In(State.Active).When(Command.Pause).MoveTo(State.Paused).If(() => a == 1)
                .In(State.Active).When(Command.End).MoveTo(State.Inactive)

                .In(State.Paused).When(Command.Resume).MoveTo(State.Active).ThenExecute(() => Console.WriteLine("==> Resume called... <=="))
                .In(State.Paused).When(Command.End).MoveTo(State.Inactive)

                .In(State.Inactive).When(Command.Begin).MoveTo(State.Active)
                .In(State.Inactive).When(Command.Exit).MoveTo(State.Terminated).ThenExecute(() => Console.WriteLine("==> Exit called... <=="));

            var machine1 = stateMachine.BuildMachine(State.Inactive);
            var machine2 = stateMachine.BuildMachine(State.Paused);
            var machine3 = stateMachine.BuildMachine(State.Terminated);

            Console.WriteLine($"Initial State = {machine1.CurrentState}");
            Console.WriteLine($"Begin = {machine1.CurrentState} -> {machine1.MoveNext(Command.Begin)}");
            Console.WriteLine($"Pause = {machine1.CurrentState} -> {machine1.MoveNext(Command.Pause)}");
            Console.WriteLine($"End = {machine1.CurrentState} -> {machine1.MoveNext(Command.End)}");
            Console.WriteLine($"Exit = {machine1.CurrentState} -> {machine1.MoveNext(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {machine2.CurrentState}");
            Console.WriteLine($"Resume = {machine2.CurrentState} -> {machine2.MoveNext(Command.Resume)}");
            Console.WriteLine($"End = {machine2.CurrentState} -> {machine2.MoveNext(Command.End)}");
            Console.WriteLine($"Exit = {machine2.CurrentState} -> {machine2.MoveNext(Command.Exit)}");

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine($"Initial State = {machine3.CurrentState}");
            Console.WriteLine($"Begin can be fired = {machine3.TransitionIsValid(Command.Begin)}");

            try
            {
                machine3.MoveNext(Command.Begin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadLine();
        }
    }
}