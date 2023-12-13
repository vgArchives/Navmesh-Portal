using System;
using UnityEngine;

namespace TheWatch.Core
{
    public class PlayerManager : MonoBehaviour
    {
        public event Action<Vector3> OnMove;
        public event Action OnCastAbility;
        public event Action<GameObject> OnEnteredPortal;

        protected void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                HandleMovingState();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                HandleAbilityState();
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PortalEffectController _))
            {
                OnEnteredPortal?.Invoke(other.gameObject);
            }
        }

        private void HandleMovingState()
        {
            OnMove?.Invoke(Input.mousePosition);
        }

        private void HandleAbilityState()
        {
            OnCastAbility?.Invoke();
        }
    }
}
