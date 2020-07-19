using Helper.Patterns.FSM;

namespace DefaultNamespace
{
    public enum GameState
    {
        FirstAnimation,
        PlayGame,
        Victory,
        Lose,
    }
    
    public class GameFSM : AbstractFSM<GameState>
    {
        public void Start()
        {
            AbstractState<GameState>.SetMainFSM(this);
            
            //Add state class 
            _statesDictionary.Add(GameState.FirstAnimation, new FirstAnimationState());
            _statesDictionary.Add(GameState.PlayGame, new PlayGameState());
            
            _statesDictionary.Add(GameState.Lose, new DefeatState());
        }

        public override void StartFSM()
        {
            _current = GameState.FirstAnimation;
            _statesDictionary[_current].Enter(GameState.FirstAnimation);   
        }
    }
}