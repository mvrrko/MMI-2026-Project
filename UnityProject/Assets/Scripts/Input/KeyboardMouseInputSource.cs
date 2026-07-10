using System;
using MMI2026.LabEscape.Core;
using UnityEngine;

namespace MMI2026.LabEscape.Input
{
    public class KeyboardMouseInputSource : MonoBehaviour, ICommandSource
    {
        [Header("Pointer targeting")]
        [SerializeField] private Transform currentPointerTarget;
        [SerializeField] private CenterScreenTargetResolver targetResolver;

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
            if (targetResolver != null && targetResolver.TryGetCurrentTarget(out var resolvedTarget))
            {
                return resolvedTarget;
            }

            return currentPointerTarget == null ? string.Empty : currentPointerTarget.name;
        }
    }
}
