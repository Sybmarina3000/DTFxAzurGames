using DefaultNamespace.UI;
using Helper.Patterns.FSM;
using UnityEngine;

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
            
            _mainGameUi.HideGameUi();
            Physics2D.autoSimulation = false;
            RealizeBox.instance.player.setSlowMode(false);
            RealizeBox.instance.player.endAnim();
            RealizeBox.instance.loseGameAnimation.StartLose();
           // _mainGameUi.ShowDefeatWindow();
        }

        public override void Exit(GameState last)
        {
        }
        
    }
}