using DefaultNamespace.UI;
using Helper.Patterns.FSM;

namespace DefaultNamespace
{
    public class PlayGameState : AbstractState<GameState>
    {
        private MainGameUI _mainGameUi;
        private TouchController _touchController;
        private Score _score;
        
        public PlayGameState()
        {
            _mainGameUi = RealizeBox.instance.mainGameUi;
            _touchController = RealizeBox.instance.touchController;
            _score = RealizeBox.instance.score;
        }
        
        public override void Enter(GameState last)
        {
            
            base.Enter(last);
            _mainGameUi.ShowGameUi();
            _score.StartDecreaseScore();
            //TODO Add another controller with slow 
            _touchController.enabled = true;
        }

        
        public override void Exit(GameState last)
        {
            _touchController.enabled = false;
        }
    }
}