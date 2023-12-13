using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TheWatch.Core
{
    public class PortalEffectController : MonoBehaviour
    {
        [SerializeField] private Ease _tweenEase;
        [SerializeField] private float _tweenDuration;
        [SerializeField] private float _timeUntilDetroy = 10f;
        [SerializeField] private Image _fillCircle;

        private Color startColor = Color.green;
        private Color endColor = Color.red;
        private float _destroyTimer;

        public void InitializeEffect(Vector3 finalPosition)
        {
            _destroyTimer = 0;
            transform.DOKill();

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => gameObject.SetActive(false));
            sequence.Append(transform.DOMove(new Vector3(finalPosition.x, finalPosition.y + 20f, finalPosition.z), 0.1f));
            sequence.AppendCallback(() => gameObject.SetActive(true));
            sequence.Append(transform.DOMove(finalPosition, _tweenDuration)).SetEase(_tweenEase);
        }

        protected void Update()
        {
            if (_destroyTimer >= _timeUntilDetroy)
            {
                _destroyTimer = 0;
                gameObject.SetActive(false);
            }
            else
            {
                _destroyTimer += Time.deltaTime;
                UpdateFillAmount();
            }
        }

        private void UpdateFillAmount()
        {
            float fillAmount = 1 - _destroyTimer / _timeUntilDetroy;
            _fillCircle.fillAmount = fillAmount;
            _fillCircle.color = Color.Lerp(startColor, endColor, 1 - fillAmount);
        }
    }
}
