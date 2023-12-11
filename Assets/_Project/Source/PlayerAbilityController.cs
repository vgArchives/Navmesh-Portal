using UnityEngine;

namespace TheWatch.Core
{
    [RequireComponent(typeof(PlayerManager))]
    public class PlayerAbilityController : MonoBehaviour
    {
        [SerializeField] private GameObject _portalPrefab;
        [SerializeField] private LayerMask _raycastGround;

        private const float RaycastMaxDistance = 1000f;

        private PlayerManager _playerManager;
        private Camera _mainCamera;
        private GameplayState _gameplayState;

        protected void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _mainCamera = Camera.main;
        }

        protected void Start()
        {
            _playerManager.OnCastAbility += HandleAbilityCast;
        }

        protected void OnDestroy()
        {
            _playerManager.OnCastAbility -= HandleAbilityCast;
        }

        protected void Update()
        {
            if (Input.GetMouseButtonDown(0) && _gameplayState == GameplayState.AbilityState)
            {
                PlacePortal(Input.mousePosition);
            }
        }

        private void HandleAbilityCast()
        {
            _gameplayState = GameplayState.AbilityState;
        }

        private void PlacePortal(Vector3 positionToPlace)
        {
            Ray ray = _mainCamera.ScreenPointToRay(positionToPlace);

            if (Physics.Raycast(ray, out RaycastHit hit, RaycastMaxDistance, _raycastGround))
            {
                Vector3 portalPosition = new Vector3(hit.point.x, hit.point.y + _portalPrefab.transform.position.y,
                    hit.point.z);

                Instantiate(_portalPrefab, portalPosition, Quaternion.identity);
                _gameplayState = GameplayState.None;
            }
        }
    }
}
