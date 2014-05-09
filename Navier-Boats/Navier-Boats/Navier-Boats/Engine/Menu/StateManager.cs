using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Navier_Boats.Game.Menu;
using Microsoft.Xna.Framework.Content;

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

        private Stack<GameStates> currentState;

        public IGameState CurrentState
        {
            get { return states[currentState.Peek()]; }
        }

        public IGameState this[GameStates index]
        {
            get { return states[index]; }
            set { states[index] = value; }
        }

        private StateManager()
        {
            states = new Dictionary<GameStates, IGameState>();
            currentState = new Stack<GameStates>();
        }

        public void InitializeStateManager(GameStates defaultState)
        {
            currentState.Push(defaultState);
            foreach (KeyValuePair<GameStates, IGameState> state in states)
            {
                state.Value.Initialize();
            }
        }

        public void LoadStateContentFiles(GameStates defaultState, ContentManager content)
        {
            currentState.Push(defaultState);
            foreach (KeyValuePair<GameStates, IGameState> state in states)
            {
                state.Value.LoadContent(content);
            }
        }

        public void PushState(GameStates newState)
        {
            currentState.Push(newState);
        }
        
        public void PopState()
        {
            currentState.Pop();
            if (currentState.Count == 0)
                global::System.Environment.Exit(0);
        }

        public void PopState(GameStates targetState)
        {
            GameStates curState = currentState.Peek();
            while(currentState.Count > 0)
            {
                curState = currentState.Peek();
                if (curState == targetState)
                    break;
                currentState.Pop();
            }
        }
    }
}
