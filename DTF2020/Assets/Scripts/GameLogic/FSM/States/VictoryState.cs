using DefaultNamespace.UI;
using Helper.Patterns.FSM;

namespace DefaultNamespace
{
    public class VictoryState : AbstractState<GameState>
    {
        private MainGameUI _mainGameUi; 
        
        public VictoryState()
        {
            _mainGameUi = RealizeBox.instance.mainGameUi;
        }
        
        public override void Enter(GameState last)
        {
            base.Enter(last);
            //TODO Start win Animation, Show victory window;
            _mainGameUi.ShowVictoryWindow();
        }

        public override void Exit(GameState last)
        {
        }
        
    }
}