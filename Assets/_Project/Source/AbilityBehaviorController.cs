using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace TheWatch.Core
{
    [RequireComponent(typeof(PlayerManager))]
    public class AbilityBehaviorController : MonoBehaviour
    {
        [SerializeField] private GameObject _portalPrefab;
        [SerializeField] private LayerMask _raycastGround;
        [SerializeField] private List<GameObject> _createdPortalsList = new List<GameObject>();

        private const float RaycastMaxDistance = 1000f;
        private const int MaxPortalsCount = 2;
        private const int MinPortalsCount = 1;
        private const float PortalPreparationTime = 1f;

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
            _playerManager.OnMove += HandleMovement;
            _playerManager.OnEnteredPortal += HandlePortalEnter;
        }

        protected void Update()
        {
            if (Input.GetMouseButtonDown(0) && _gameplayState == GameplayState.AbilityState)
            {
                PlacePortal(Input.mousePosition);
            }
        }

        protected void OnDestroy()
        {
            _playerManager.OnCastAbility -= HandleAbilityCast;
            _playerManager.OnMove -= HandleMovement;
            _playerManager.OnEnteredPortal -= HandlePortalEnter;
        }

        private void HandleAbilityCast()
        {
            _gameplayState = GameplayState.AbilityState;
        }

        private void HandleMovement(Vector3 positionToMove)
        {
            _gameplayState = GameplayState.MovingState;
        }

        private void HandlePortalEnter(GameObject receivedObject)
        {
            if (_createdPortalsList.Count <= MinPortalsCount)
            {
                return;
            }

            Vector3 positionToTeleport = Vector3.zero;

            foreach (GameObject portalObject in _createdPortalsList)
            {
                SetPortalsColliderState(false);

                if (portalObject != receivedObject)
                {
                    positionToTeleport = portalObject.transform.position;
                }
            }

            _playerManager.transform.position = positionToTeleport;

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(PortalPreparationTime);
            sequence.AppendCallback(() => SetPortalsColliderState(true));
        }

        private void PlacePortal(Vector3 positionToPlace)
        {
            Ray ray = _mainCamera.ScreenPointToRay(positionToPlace);

            if (!Physics.Raycast(ray, out RaycastHit hit, RaycastMaxDistance, _raycastGround))
            {
                return;
            }

            Vector3 portalPosition = new Vector3(hit.point.x, hit.point.y + _portalPrefab.transform.position.y,
                hit.point.z);

            if (_createdPortalsList.Count >= MaxPortalsCount)
            {
                GameObject objectToMove = _createdPortalsList[0];
                objectToMove.transform.position = portalPosition;

                _createdPortalsList.RemoveAt(0);
                _createdPortalsList.Add(objectToMove);
            }
            else
            {
                GameObject portalObject = Instantiate(_portalPrefab, portalPosition, Quaternion.identity);
                _createdPortalsList.Add(portalObject);
            }

            _gameplayState = GameplayState.None;
        }

        private void SetPortalsColliderState(bool state)
        {
            foreach (GameObject portalObject in _createdPortalsList)
            {
                portalObject.GetComponent<Collider>().enabled = state;
            }
        }
    }
}
