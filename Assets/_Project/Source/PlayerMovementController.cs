using UnityEngine;
using UnityEngine.AI;

namespace TheWatch.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private LayerMask _raycastGround;

        private const float RaycastMaxDistance = 1000f;

        private NavMeshAgent _navMeshAgent;
        private Camera _mainCamera;

        protected void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _mainCamera = Camera.main;
        }
        
        protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Move();
            }
        }

        private void Move()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, RaycastMaxDistance, _raycastGround))
            {
                _navMeshAgent.destination = hit.point;
            }
        }
    }
}
