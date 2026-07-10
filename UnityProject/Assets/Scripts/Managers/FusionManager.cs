using MMI2026.LabEscape.Core;
using UnityEngine;

namespace MMI2026.LabEscape.Managers
{
    public class FusionManager : MonoBehaviour
    {
        [SerializeField] private Transform pointerTarget;

        public CommandData ApplyFusion(CommandData command)
        {
            if (command == null) return null;
            if (!command.IsDeicticTarget) return command;

            var fusedTarget = pointerTarget == null ? string.Empty : pointerTarget.name;
            command.TargetId = fusedTarget;
            return command;
        }
    }
}
