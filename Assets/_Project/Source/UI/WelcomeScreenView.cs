using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TheWatch.Core
{
    public class WelcomeScreenView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected void Start()
        {
            _startButton.onClick.AddListener(HandleStartButtonClick);
        }

        protected void Update()
        {
            if (Input.anyKeyDown)
            {
                HandleStartButtonClick();
            }
        }

        protected void OnDestroy()
        {
            _startButton.onClick.RemoveListener(HandleStartButtonClick);
        }

        private void HandleStartButtonClick()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, 0.5f));
            sequence.AppendCallback(() => gameObject.SetActive(false));
        }
    }
}
