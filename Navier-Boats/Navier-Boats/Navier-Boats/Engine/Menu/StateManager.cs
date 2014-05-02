using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Game.Menu;

namespace Navier_Boats.Engine.Menu
{
    class StateManager
    {
        #region SINGLETON_MEMBERS

        private static StateManager _instance;

        public static StateManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StateManager();
            }
            return _instance;
        }

        #endregion
        
        private Dictionary<GameStates, IGameState> states;

        private Stack<IGameState> currentState;

        public IGameState CurrentState
        {
            get { return currentState.Peek(); }
        }

        public IGameState this[GameStates index]
        {
            get { return states[index]; }
            set { states[index] = value; }
        }

        private StateManager()
        {
            states = new Dictionary<GameStates, IGameState>();
            currentState = new Stack<IGameState>();
        }

        public void InitializeStateManager(GameStates defaultState)
        {
            currentState.Push(states[defaultState]);
        }

        public void PushState(GameStates newState)
        {
            currentState.Push(states[newState]);
        }
        
        public void PopState()
        {
            currentState.Pop();
            if (currentState.Count == 0)
                global::System.Environment.Exit(0);
        }
    }
}
