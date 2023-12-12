using UnityEngine;
using UnityEngine.AI;

namespace TheWatch.Core
{
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private LayerMask _raycastGround;
        [SerializeField] private float _rotationSpeed = 0.01f;

        private const float RaycastMaxDistance = 1000f;

        private float _rotationVelocity;
        private PlayerManager _playerManager;
        private NavMeshAgent _navMeshAgent;
        private Camera _mainCamera;

        protected void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _mainCamera = Camera.main;
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
            Ray ray = _mainCamera.ScreenPointToRay(positionToMove);

            if (!Physics.Raycast(ray, out RaycastHit hit, RaycastMaxDistance, _raycastGround))
            {
                return;
            }

            _navMeshAgent.destination = hit.point;

            Quaternion lookRotation = Quaternion.LookRotation(hit.point - transform.position);

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
