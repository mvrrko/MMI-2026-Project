using MMI2026.LabEscape.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MMI2026.LabEscape.Managers
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] private Text statusText;
        [SerializeField] private Text completionText;

        public bool HasKeycard { get; private set; }
        public bool GeneratorActive { get; private set; }
        public bool ExitDoorUnlocked { get; private set; }
        public bool ExitDoorOpen { get; private set; }
        public bool IsCompleted => ExitDoorOpen;

        public event Action OnEscapeCompleted;

        private void Start()
        {
            RenderStatus();
            if (completionText != null)
            {
                completionText.text = string.Empty;
            }
        }

        public void HandleCommand(CommandData command)
        {
            if (command == null) return;
            var wasCompleted = IsCompleted;

            switch (command.Action)
            {
                case GameActionType.Interact:
                    HandleInteract(command.TargetId);
                    break;
                case GameActionType.PickUpKeycard:
                    HasKeycard = true;
                    break;
                case GameActionType.ActivateGenerator:
                    if (HasKeycard && IsTarget(command.TargetId, "Generator")) GeneratorActive = true;
                    break;
                case GameActionType.UnlockDoor:
                    if (HasKeycard && GeneratorActive && IsTarget(command.TargetId, "Door_Exit", "Door")) ExitDoorUnlocked = true;
                    break;
                case GameActionType.OpenDoor:
                    if (ExitDoorUnlocked && IsTarget(command.TargetId, "Door_Exit", "Door")) ExitDoorOpen = true;
                    break;
            }

            RenderStatus();
            if (!wasCompleted && IsCompleted)
            {
                if (completionText != null)
                {
                    completionText.text = "Escape complete! Final product ready for recording.";
                }
                OnEscapeCompleted?.Invoke();
            }
        }

        private void HandleInteract(string targetId)
        {
            if (IsTarget(targetId, "Keycard"))
            {
                HasKeycard = true;
                return;
            }

            if (IsTarget(targetId, "Generator"))
            {
                if (HasKeycard) GeneratorActive = true;
                return;
            }

            if (IsTarget(targetId, "Door_Exit", "Door"))
            {
                if (!ExitDoorUnlocked)
                {
                    if (HasKeycard && GeneratorActive) ExitDoorUnlocked = true;
                    return;
                }

                ExitDoorOpen = true;
            }
        }

        private static bool IsTarget(string targetId, params string[] aliases)
        {
            if (string.IsNullOrWhiteSpace(targetId)) return false;
            foreach (var alias in aliases)
            {
                if (string.Equals(targetId, alias, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void RenderStatus()
        {
            if (statusText == null) return;
            statusText.text =
                $"Keycard: {(HasKeycard ? "Done" : "Pending")}\n" +
                $"Generator: {(GeneratorActive ? "Done" : "Pending")}\n" +
                $"Door Unlocked: {(ExitDoorUnlocked ? "Done" : "Pending")}\n" +
                $"Escape: {(ExitDoorOpen ? "Done" : "Pending")}";
        }
    }
}
