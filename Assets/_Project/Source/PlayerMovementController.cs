using UnityEngine;
using UnityEngine.AI;

namespace TheWatch.Core
{
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.01f;

        private float _rotationVelocity;
        private PlayerManager _playerManager;
        private NavMeshAgent _navMeshAgent;

        protected void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected void Start()
        {
            _playerManager.OnMove += HandleMovement;
            _playerManager.OnEnteredPortal += HandlePortalEnter;
            _playerManager.OnCastAbility += HandleAbilityCast;
        }

        protected void OnDestroy()
        {
            _playerManager.OnMove -= HandleMovement;
            _playerManager.OnEnteredPortal -= HandlePortalEnter;
            _playerManager.OnCastAbility -= HandleAbilityCast;
        }

        private void HandleMovement(Vector3 positionToMove)
        {
            if (!RaycastUtils.CheckRaycastFromMouse(out RaycastHit raycastHit))
            {
                return;
            }

            _navMeshAgent.destination = raycastHit.point;

            Quaternion lookRotation = Quaternion.LookRotation(raycastHit.point - transform.position);

            float rotationYAxis = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookRotation.eulerAngles.y,
                ref _rotationVelocity, _rotationSpeed * (Time.deltaTime * _navMeshAgent.speed));

            transform.eulerAngles = new Vector3(0f, rotationYAxis, 0f);
        }

        private void HandlePortalEnter(GameObject receivedObject)
        {
            _navMeshAgent.ResetPath();
        }

        private void HandleAbilityCast()
        {
            _navMeshAgent.ResetPath();
        }
    }
}
