using DefaultNamespace.UI;
using Helper.Patterns.FSM;

namespace DefaultNamespace
{
    public class DefeatState: AbstractState<GameState>
    {
        private MainGameUI _mainGameUi; 
        
        public DefeatState()
        {
            _mainGameUi = RealizeBox.instance.mainGameUi;
        }
        
        public override void Enter(GameState last)
        {
            base.Enter(last);
            //TODO Start Lose Animation, Show defeat window;
            _mainGameUi.ShowDefeatWindow();
        }

        public override void Exit(GameState last)
        {
        }
        
    }
}