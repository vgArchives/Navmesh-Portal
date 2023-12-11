using UnityEngine;
using UnityEngine.AI;

namespace TheWatch.Core
{
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private LayerMask _raycastGround;

        private const float RaycastMaxDistance = 1000f;

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
        }

        protected void OnDestroy()
        {
            _playerManager.OnMove -= HandleMovement;
        }

        private void HandleMovement(Vector3 positionToMove)
        {
            Ray ray = _mainCamera.ScreenPointToRay(positionToMove);

            if (Physics.Raycast(ray, out RaycastHit hit, RaycastMaxDistance, _raycastGround))
            {
                _navMeshAgent.destination = hit.point;
            }
        }
    }
}
