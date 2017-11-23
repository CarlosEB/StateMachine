using System;

namespace StateMachine.Library
{
    public interface IStateMachine<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        StateMachine<TState, TCommand>.Machine Create(TState state);

        IStateMachineIn<TState, TCommand> In(TState currentState);
    }

    public interface IStateMachineIn<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        IStateMachineOn<TState, TCommand> On(TCommand actualCommand);
    }

    public interface IStateMachineOn<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        IStateMachineMoveTo<TState, TCommand> MoveTo(TState nextState);
    }

    public interface IStateMachineMoveTo<TState, TCommand> : IStateMachine<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        IStateMachine<TState, TCommand> ThenExecute(Action toExecute);
    }
}