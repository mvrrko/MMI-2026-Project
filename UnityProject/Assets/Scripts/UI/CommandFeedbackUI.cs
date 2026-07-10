using MMI2026.LabEscape.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MMI2026.LabEscape.UI
{
    public class CommandFeedbackUI : MonoBehaviour
    {
        [SerializeField] private Text feedbackText;
        [SerializeField] private bool showRawCommandDetails;

        public void Show(CommandData command)
        {
            if (feedbackText == null || command == null) return;
            var source = command.SourceModality == ModalityType.Voice ? "Voice" : "Keyboard/Mouse";
            var target = string.IsNullOrWhiteSpace(command.TargetId) ? "none" : command.TargetId;

            feedbackText.text = $"{source} -> {command.Action} ({target})";
            if (showRawCommandDetails)
            {
                feedbackText.text += $"\nraw: {command.RawCommand}";
            }
        }
    }
}
