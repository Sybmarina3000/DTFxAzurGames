using Helper.Patterns.FSM;

namespace DefaultNamespace
{
    public class FirstAnimationState : AbstractState<GameState>
    {
        private FirstCameraGameAnimation _firstCameraGameAnimation;
        
        public FirstAnimationState()
        {
            _firstCameraGameAnimation = RealizeBox.instance.firstCameraGameAnimation;
        }

        public override void Enter(GameState last)
        {
            base.Enter(last);

            _firstCameraGameAnimation.StartAnimation();
        }

        
        public override void Exit(GameState last)
        {
        }
    }
}