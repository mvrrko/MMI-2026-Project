using UnityEngine;

namespace MMI2026.LabEscape.Input
{
    public class CenterScreenTargetResolver : MonoBehaviour
    {
        [SerializeField] private Camera pointerCamera;
        [SerializeField] private float pointerDistance = 4.0f;
        [SerializeField] private LayerMask pointerMask = ~0;
        [SerializeField] private Transform fallbackTarget;

        public bool TryGetCurrentTarget(out string targetId)
        {
            targetId = string.Empty;
            var activeCamera = pointerCamera != null ? pointerCamera : Camera.main;
            if (activeCamera == null) return false;

            var ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out var hitInfo, pointerDistance, pointerMask))
            {
                targetId = hitInfo.transform.name;
                return !string.IsNullOrWhiteSpace(targetId);
            }

            if (fallbackTarget == null) return false;
            targetId = fallbackTarget.name;
            return !string.IsNullOrWhiteSpace(targetId);
        }
    }
}
