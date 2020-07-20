using UnityEngine;

namespace DefaultNamespace
{
    public class Movie : MonoBehaviour
    {

        public void StartShow()
        {
            RealizeBox.instance.camera.SetActive(false);
            RealizeBox.instance.victoryCamera.SetActive(true);
            RealizeBox.instance.staslDiamond.AnimationState.SetAnimation(0,"activation", false);
            
            Invoke(nameof(HideAndOpenVin), 4.2f);
        }

        public void HideAndOpenVin()
        {
            
            RealizeBox.instance.victoryPanel.gameObject.SetActive(true);
            RealizeBox.instance.victoryPanel.Show();
            gameObject.SetActive(false);
        }
    }
}