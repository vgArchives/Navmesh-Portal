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
        [SerializeField] private GameObject _abilityIndicator;
        [SerializeField] private List<GameObject> _createdPortalsList = new List<GameObject>();

        private const float PortalPreparationTime = 1.5f;

        private PlayerManager _playerManager;
        private GameplayState _gameplayState;


        protected void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }

        protected void Start()
        {
            _playerManager.OnCastAbility += HandleAbilityCast;
            _playerManager.OnMove += HandleMovement;
            _playerManager.OnEnteredPortal += HandlePortalEnter;
        }

        protected void Update()
        {
            if (_gameplayState != GameplayState.AbilityState)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlacePortal(Input.mousePosition);
            }
            else
            {
                UpdateAbilityIndicatorPosition();
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
            _abilityIndicator.SetActive(true);
        }

        private void HandleMovement(Vector3 positionToMove)
        {
            _gameplayState = GameplayState.MovingState;
            _abilityIndicator.SetActive(false);
        }

        private void HandlePortalEnter(GameObject receivedObject)
        {
            if (_createdPortalsList.Any(portalObject => !portalObject.activeSelf))
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

        private void PlacePortal(Vector3 inputPosition)
        {
            if (!RaycastUtils.CheckRaycastFromMouse(out RaycastHit raycastHit, inputPosition))
            {
                return;
            }

            Vector3 portalPosition = new (raycastHit.point.x, raycastHit.point.y + _portalPrefab.transform.position.y, raycastHit.point.z);

            GameObject portalObject = _createdPortalsList[0];
            portalObject.GetComponent<PortalEffectController>().InitializeEffect(portalPosition);

            _createdPortalsList.RemoveAt(0);
            _createdPortalsList.Add(portalObject);
            _gameplayState = GameplayState.None;
            _abilityIndicator.SetActive(false);
        }

        private void SetPortalsColliderState(bool state)
        {
            foreach (GameObject portalObject in _createdPortalsList)
            {
                portalObject.GetComponent<Collider>().enabled = state;
            }
        }

        private void UpdateAbilityIndicatorPosition()
        {
            if (!RaycastUtils.CheckRaycastFromMouse(out RaycastHit hitInfo))
            {
                return;
            }

            Vector3 pos = new Vector3(hitInfo.point.x, 0.1f, hitInfo.point.z);
            _abilityIndicator.transform.position = pos;
        }
    }
}
