using UnityEngine;

namespace TheWatch.Core
{
    public static class RaycastUtils
    {
        private const float RaycastMaxDistance = 1000f;
        private const int RaycastLayer = 1 << 6;

        public static bool CheckRaycastFromMouse(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, RaycastMaxDistance, RaycastLayer);
        }

        public static bool CheckRaycastFromMouse(out RaycastHit hit, Vector3 inputPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            return Physics.Raycast(ray, out hit, RaycastMaxDistance, RaycastLayer);
        }
    }
}
