using UnityEngine.VFX;
using UnityEngine;

namespace TheWatch.Core
{
    public class ClickEffectsController : MonoBehaviour
    {
        [SerializeField] private GameObject _clickEffect;

        private const string ClickEvent = "Click";

        private VisualEffect[] _visualEffect;

        protected void Awake()
        {
            _visualEffect = _clickEffect.GetComponentsInChildren<VisualEffect>();
        }

        protected void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            if (!RaycastUtils.CheckRaycastFromMouse(out RaycastHit hitInfo))
            {
                return;
            }

            _clickEffect.transform.position = hitInfo.point;

            foreach (VisualEffect effect in _visualEffect)
            {
                effect.SendEvent(ClickEvent);
            }
        }
    }
}
