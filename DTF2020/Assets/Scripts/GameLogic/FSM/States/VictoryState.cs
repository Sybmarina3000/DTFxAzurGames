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

            _mainGameUi.HideGameUi();
            
            int countStar = 0;
            var currScore = RealizeBox.instance.score.currentScore;
            if (currScore >= 30)
                countStar = 3;
            else if (currScore >= 15)
                countStar = 2;
            else
                countStar = 1;

            PlayerLoadPrefs.currentStars = countStar;
            PlayerLoadPrefs.SaveCurrentLevelProgress();
            
            RealizeBox.instance.movie.gameObject.SetActive(true);
            RealizeBox.instance.movie.StartShow();
        }

        public override void Exit(GameState last)
        {
        }
    }
}