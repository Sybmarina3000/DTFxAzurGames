using DefaultNamespace.UI;
using Helper.Patterns.FSM;

namespace DefaultNamespace
{
    public class PlayGameState : AbstractState<GameState>
    {
        private MainGameUI _mainGameUi;
        private TouchController _touchController;
        
        public PlayGameState()
        {
            _mainGameUi = RealizeBox.instance.mainGameUi;
            _touchController = RealizeBox.instance.touchController;
        }
        
        public override void Enter(GameState last)
        {
            base.Enter(last);
            _mainGameUi.ShowGameUi();
            //TODO Add another controller with slow 
            _touchController.enabled = true;
        }

        
        public override void Exit(GameState last)
        {
            _touchController.enabled = false;
        }
    }
}