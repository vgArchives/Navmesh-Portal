using UnityEngine.VFX;
using UnityEngine;

namespace TheWatch.Core
{
    public class ClickEffectsController : MonoBehaviour
    {
        [SerializeField] private GameObject _clickEffect;
        [SerializeField] private LayerMask _raycastGround;

        private const float RaycastMaxDistance = 1000f;

        private const string ClickEvent = "Click";

        private Camera _mainCamera;
        private VisualEffect[] _visualEffect;

        protected void Awake()
        {
            _mainCamera = Camera.main;
            _visualEffect = _clickEffect.GetComponentsInChildren<VisualEffect>();
        }

        protected void Update()
        {
            if (!Input.GetMouseButtonDown(1))
            {
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, RaycastMaxDistance, _raycastGround))
            {
                return;
            }

            _clickEffect.transform.position = hit.point;

            foreach (VisualEffect effect in _visualEffect)
            {
                effect.SendEvent(ClickEvent);
            }
        }
    }
}
