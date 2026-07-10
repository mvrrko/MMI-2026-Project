using System;
using MMI2026.LabEscape.Core;
using UnityEngine;

namespace MMI2026.LabEscape.Input
{
    public class KeyboardMouseInputSource : MonoBehaviour, ICommandSource
    {
        [Header("Pointer targeting")]
        [SerializeField] private Transform currentPointerTarget;
        [SerializeField] private Camera pointerCamera;
        [SerializeField] private float pointerDistance = 4.0f;
        [SerializeField] private LayerMask pointerMask = ~0;

        public event Action<CommandData> OnCommand;

        public void StartListening() { }
        public void StopListening() { }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                Emit(GameActionType.OpenInventory, string.Empty, "keyboard:I");
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                Emit(GameActionType.Interact, ResolvePointerTargetId(), "keyboard:E");
            }

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Emit(GameActionType.Interact, ResolvePointerTargetId(), "mouse:left");
            }
        }

        private void Emit(GameActionType action, string targetId, string raw)
        {
            OnCommand?.Invoke(new CommandData
            {
                Action = action,
                TargetId = targetId,
                RawCommand = raw,
                SourceModality = ModalityType.KeyboardMouse,
                TimestampUtc = DateTime.UtcNow
            });
        }

        private string ResolvePointerTargetId()
        {
            if (TryResolveRaycastTarget(out var raycastTarget))
            {
                return raycastTarget;
            }

            return currentPointerTarget == null ? string.Empty : currentPointerTarget.name;
        }

        private bool TryResolveRaycastTarget(out string targetId)
        {
            targetId = string.Empty;
            var activeCamera = pointerCamera != null ? pointerCamera : Camera.main;
            if (activeCamera == null) return false;

            var ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (!Physics.Raycast(ray, out var hitInfo, pointerDistance, pointerMask)) return false;

            targetId = hitInfo.transform.name;
            return !string.IsNullOrWhiteSpace(targetId);
        }
    }
}
