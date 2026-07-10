using MMI2026.LabEscape.Core;
using MMI2026.LabEscape.UI;
using UnityEngine;

namespace MMI2026.LabEscape.Managers
{
    public class ConflictResolutionManager : MonoBehaviour
    {
        [SerializeField] private ConflictResolutionUI ui;
        [SerializeField] private float conflictWindowSeconds = 1.0f;

        private CommandData lastPointerDrivenCommand;

        public void TrackPointerContext(CommandData command)
        {
            if (command == null || command.SourceModality != ModalityType.KeyboardMouse) return;
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

            if (ui != null)
            {
                ui.Show(
                    $"Targets differ: pointer={lastPointerDrivenCommand.TargetId}, voice={incomingVoiceCommand.TargetId}",
                    usePointer =>
                    {
                        if (usePointer)
                        {
                            incomingVoiceCommand.TargetId = lastPointerDrivenCommand.TargetId;
                        }
                    });
            }

            resolved = incomingVoiceCommand;
            return true;
        }
    }
}
