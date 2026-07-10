using MMI2026.LabEscape.Core;
using UnityEngine;

namespace MMI2026.LabEscape.Managers
{
    public class FusionManager : MonoBehaviour
    {
        [SerializeField] private Transform pointerTarget;
        [SerializeField] private float pointerContextWindowSeconds = 2.0f;

        private CommandData lastPointerCommand;

        public void TrackPointerContext(CommandData command)
        {
            if (command == null || command.SourceModality != ModalityType.KeyboardMouse) return;
            if (string.IsNullOrWhiteSpace(command.TargetId)) return;
            lastPointerCommand = command;
        }

        public CommandData ApplyFusion(CommandData command)
        {
            if (command == null) return null;
            if (!command.IsDeicticTarget) return command;

            var fusedTarget = ResolvePointerTarget(command);
            command.TargetId = fusedTarget;
            return command;
        }

        private string ResolvePointerTarget(CommandData command)
        {
            if (lastPointerCommand != null && command != null)
            {
                var delta = (command.TimestampUtc - lastPointerCommand.TimestampUtc).TotalSeconds;
                if (delta >= 0 && delta <= pointerContextWindowSeconds)
                {
                    return lastPointerCommand.TargetId;
                }
            }

            return pointerTarget == null ? string.Empty : pointerTarget.name;
        }
    }
}
