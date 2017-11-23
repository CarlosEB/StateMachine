using System;

namespace StateMachine.Library
{
    public interface IStateMachineIn<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        StateMachine<TState, TCommand>.Machine BuildMachine(TState state);
        IStateMachineOn<TState, TCommand> In(TState currentState);
    }

    public interface IStateMachineOn<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        IStateMachineMoveTo<TState, TCommand> On(TCommand actualCommand);
    }

    public interface IStateMachineMoveTo<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        IStateMachineInThenExecute<TState, TCommand> MoveTo(TState nextState);
    }

    public interface IStateMachineInThenExecute<TState, TCommand> : IStateMachineIn<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        IStateMachineIn<TState, TCommand> ThenExecute(Action toExecute);
    }
}