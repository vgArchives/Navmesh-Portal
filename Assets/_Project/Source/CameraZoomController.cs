using Cinemachine;
using UnityEngine;

namespace TheWatch.Core
{
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField] private float _zoomSpeed = 5.0f;
        [SerializeField] private float _minZoomDistance = 17.0f;
        [SerializeField] private float _maxZoomDistance = 21.0f;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        protected void Update()
        {
            float scrollWheelValue = Input.mouseScrollDelta.y;
            UpdateCameraDistance(scrollWheelValue);
        }

        private void UpdateCameraDistance(float scrollValue)
        {
            CinemachineFramingTransposer cinemachineFramingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            float currentDistance = cinemachineFramingTransposer.m_CameraDistance;
            float newDistance = Mathf.Clamp(currentDistance - scrollValue * _zoomSpeed, _minZoomDistance, _maxZoomDistance);
            cinemachineFramingTransposer.m_CameraDistance = newDistance;
        }
    }
}
