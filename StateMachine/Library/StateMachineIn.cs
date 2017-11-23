using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine.Library
{
    public class StateMachine<TState, TCommand> : IStateMachineOn<TState, TCommand>, IStateMachineMoveTo<TState, TCommand>, IStateMachineInThenExecute<TState, TCommand> where TState : struct, IConvertible where TCommand : struct, IConvertible
    {
        private readonly IList<StateMachineTransition> _transitions;

        private TState _currentState;
        private TCommand _actualCommand;

        private StateMachine()
        {
            _transitions = new List<StateMachineTransition>();
        }

        public static IStateMachineIn<TState, TCommand> Create()
        {
            return new StateMachine<TState, TCommand>();
        }

        public Machine BuildMachine(TState state)
        {
            return new Machine(state, _transitions);
        }

        public IStateMachineOn<TState, TCommand> In(TState currentState)
        {
            _currentState = currentState;
            return this;
        }

        public IStateMachineMoveTo<TState, TCommand> On(TCommand actualCommand)
        {
            _actualCommand = actualCommand;
            return this;
        }

        public IStateMachineInThenExecute<TState, TCommand> MoveTo(TState nextState)
        {
            AddTransition(_actualCommand, _currentState, nextState);
            return this;
        }

        IStateMachineIn<TState, TCommand> IStateMachineInThenExecute<TState, TCommand>.ThenExecute(Action toExecute)
        {
            if (!_transitions.Any()) return this;

            _transitions.Last().ToExecute = toExecute;

            return this;
        }

        private void AddTransition(TCommand commnad, TState currentState, TState nextState)
        {
            _transitions.Add(new StateMachineTransition(currentState, nextState, commnad));
        }


        public class Machine
        {
            private readonly IList<StateMachineTransition> _transitions;

            public TState CurrentState { get; private set; }

            public Machine(TState state, IList<StateMachineTransition> transitions)
            {
                _transitions = transitions;
                CurrentState = state;
            }

            public bool TransitionIsValid(TCommand command)
            {
                return _transitions.Any(f => f.Match(CurrentState, command));
            }

            public TState MoveNext(TCommand command)
            {
                return CurrentState = GetNextState(command);
            }

            private TState GetNextState(TCommand command)
            {
                var transition = _transitions.FirstOrDefault(f => f.Match(CurrentState, command));

                if (transition == null)
                    throw new Exception($"Invalid state transition. Current State: {CurrentState}. Command: {command}");

                transition.ToExecute?.Invoke();

                return transition.NextState;
            }
        }

        public class StateMachineTransition
        {
            private readonly TState _currentState;
            private readonly TCommand _command;

            public Action ToExecute { get; set; }

            public TState NextState { get; }

            public bool Match(TState currentState, TCommand command)
            {
                return _currentState.Equals(currentState) && _command.Equals(command);
            }

            public StateMachineTransition(TState currentState, TState nextState, TCommand command)
            {
                _currentState = currentState;
                _command = command;
                NextState = nextState;
            }
        }
    }
}