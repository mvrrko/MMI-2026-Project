using MMI2026.LabEscape.Core;
using MMI2026.LabEscape.UI;
using System;
using System.Collections;
using UnityEngine;

namespace MMI2026.LabEscape.Managers
{
    public class ConflictResolutionManager : MonoBehaviour
    {
        [SerializeField] private ConflictResolutionUI ui;
        [SerializeField] private float conflictWindowSeconds = 1.0f;
        [SerializeField] private float autoCancelSeconds = 3.0f;

        private CommandData lastPointerDrivenCommand;
        private CommandData pendingVoiceCommand;
        private string pendingPointerTargetId;
        private Coroutine pendingTimeoutRoutine;

        public event Action<CommandData> OnCommandResolved;

        private void OnDisable()
        {
            CancelPendingConflict();
        }

        public void TrackPointerContext(CommandData command)
        {
            if (command == null || command.SourceModality != ModalityType.KeyboardMouse) return;
            if (string.IsNullOrWhiteSpace(command.TargetId)) return;
            lastPointerDrivenCommand = command;
        }

        public bool TryResolveConflict(CommandData incomingVoiceCommand, out CommandData resolved)
        {
            resolved = incomingVoiceCommand;
            if (incomingVoiceCommand == null || incomingVoiceCommand.SourceModality != ModalityType.Voice) return true;
            if (lastPointerDrivenCommand == null) return true;
            if (string.IsNullOrWhiteSpace(lastPointerDrivenCommand.TargetId)) return true;
            if (string.IsNullOrWhiteSpace(incomingVoiceCommand.TargetId)) return true;

            var delta = (incomingVoiceCommand.TimestampUtc - lastPointerDrivenCommand.TimestampUtc).TotalSeconds;
            if (delta < 0 || delta > conflictWindowSeconds) return true;

            if (lastPointerDrivenCommand.TargetId == incomingVoiceCommand.TargetId) return true;

            if (pendingVoiceCommand != null)
            {
                CancelPendingConflict();
            }

            pendingVoiceCommand = incomingVoiceCommand;
            pendingPointerTargetId = lastPointerDrivenCommand.TargetId;

            if (ui == null)
            {
                resolved = null;
                ClearPendingState();
                return false;
            }

            ui.Show(
                $"Targets differ: pointer={pendingPointerTargetId}, voice={incomingVoiceCommand.TargetId}",
                HandleChoice);
            pendingTimeoutRoutine = StartCoroutine(AutoCancelAfterDelay());

            resolved = null;
            return false;
        }

        private void HandleChoice(ConflictChoice choice)
        {
            if (pendingVoiceCommand == null) return;

            if (pendingTimeoutRoutine != null)
            {
                StopCoroutine(pendingTimeoutRoutine);
                pendingTimeoutRoutine = null;
            }

            switch (choice)
            {
                case ConflictChoice.Pointer:
                    pendingVoiceCommand.TargetId = pendingPointerTargetId;
                    OnCommandResolved?.Invoke(pendingVoiceCommand);
                    break;
                case ConflictChoice.Voice:
                    OnCommandResolved?.Invoke(pendingVoiceCommand);
                    break;
                case ConflictChoice.Cancel:
                    break;
            }

            ClearPendingState();
        }

        private IEnumerator AutoCancelAfterDelay()
        {
            yield return new WaitForSeconds(autoCancelSeconds);
            if (pendingVoiceCommand == null) yield break;
            ui?.Hide();
            ClearPendingState();
        }

        private void CancelPendingConflict()
        {
            if (pendingTimeoutRoutine != null)
            {
                StopCoroutine(pendingTimeoutRoutine);
                pendingTimeoutRoutine = null;
            }
            ui?.Hide();
            ClearPendingState();
        }

        private void ClearPendingState()
        {
            pendingVoiceCommand = null;
            pendingPointerTargetId = string.Empty;
        }
    }
}
