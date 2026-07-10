using MMI2026.LabEscape.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MMI2026.LabEscape.Managers
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] private Text statusText;

        public bool HasKeycard { get; private set; }
        public bool GeneratorActive { get; private set; }
        public bool ExitDoorUnlocked { get; private set; }
        public bool ExitDoorOpen { get; private set; }

        public void HandleCommand(CommandData command)
        {
            if (command == null) return;

            switch (command.Action)
            {
                case GameActionType.PickUpKeycard:
                    HasKeycard = true;
                    break;
                case GameActionType.ActivateGenerator:
                    if (HasKeycard) GeneratorActive = true;
                    break;
                case GameActionType.UnlockDoor:
                    if (HasKeycard && GeneratorActive) ExitDoorUnlocked = true;
                    break;
                case GameActionType.OpenDoor:
                    if (ExitDoorUnlocked) ExitDoorOpen = true;
                    break;
            }

            RenderStatus();
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
