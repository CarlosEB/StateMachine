using System;
using System.Collections.Generic;

namespace StateMachine.Library
{
    public class StateMachine<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        private readonly Dictionary<StateMachineTransition, TState> _transitions;

        public StateMachine()
        {
            _transitions = new Dictionary<StateMachineTransition, TState>();
        }

        public Machine GetMachine(TState state)
        {
            return new Machine(state, _transitions);
        }


        public void AddTransition(TCommand commnad, TState currentState, TState nextState)
        {
            _transitions.Add(new StateMachineTransition(currentState, commnad), nextState);
        }

        public class StateMachineTransition
        {
            private readonly TState _currentState;
            private readonly TCommand _command;

            public StateMachineTransition(TState currentState, TCommand command)
            {
                _currentState = currentState;
                _command = command;
            }

            public override int GetHashCode()
            {
                return _currentState.GetHashCode() * 17 + _command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as StateMachineTransition;

                return other != null && _currentState.Equals(other._currentState) && _command.Equals(other._command);
            }
        }

        public class Machine
        {
            private readonly Dictionary<StateMachineTransition, TState> _transitions;

            public Machine(TState state, Dictionary<StateMachineTransition, TState> transitions)
            {
                _transitions = transitions;
                CurrentProcessState = state;
            }

            public TState CurrentProcessState { get; private set; }

            public bool NextStateIsValid(TCommand command)
            {
                return _transitions.TryGetValue(new StateMachineTransition(CurrentProcessState, command), out TState _);
            }

            public TState MoveNextState(TCommand command)
            {
                CurrentProcessState = GetNextState(command);

                return CurrentProcessState;
            }

            private TState GetNextState(TCommand command)
            {
                if (!_transitions.TryGetValue(new StateMachineTransition(CurrentProcessState, command), out TState nextState))
                    throw new Exception($"Invalid currentState transition: {CurrentProcessState} ==> {command}");

                return nextState;
            }
        }
    }
}
