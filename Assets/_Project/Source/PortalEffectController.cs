using DG.Tweening;
using UnityEngine;

namespace TheWatch.Core
{
    public class PortalEffectController : MonoBehaviour
    {
        [SerializeField] private Ease _tweenEase;
        [SerializeField] private float _tweenDuration;

        public void InitializeEffect(Vector3 finalPosition)
        {
            transform.DOKill();

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => gameObject.SetActive(false));
            sequence.Append(transform.DOMove(new Vector3(finalPosition.x, finalPosition.y + 20f, finalPosition.z), 0.1f));
            sequence.AppendCallback(() => gameObject.SetActive(true));
            sequence.Append(transform.DOMove(finalPosition, _tweenDuration)).SetEase(_tweenEase);
        }
    }
}
