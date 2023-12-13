using UnityEngine;
using UnityEngine.UI;

namespace TheWatch.Core
{
    public class QuitGameView : MonoBehaviour
    {
        [SerializeField] private Button _quitButton;

        protected void Start()
        {
            _quitButton.onClick.AddListener(HandleQuitButtonClick);
        }

        protected void OnDestroy()
        {
            _quitButton.onClick.RemoveListener(HandleQuitButtonClick);
        }

        private void HandleQuitButtonClick()
        {
            Application.Quit();
        }
    }
}
