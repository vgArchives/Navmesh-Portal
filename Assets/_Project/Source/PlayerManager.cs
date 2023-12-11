using System;
using UnityEngine;

namespace TheWatch.Core
{
    public class PlayerManager : MonoBehaviour
    {
        public event Action<Vector3> OnMove;
        public event Action OnCastAbility;

        protected void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                HandleMovingState();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                HandleAbilityState();
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
