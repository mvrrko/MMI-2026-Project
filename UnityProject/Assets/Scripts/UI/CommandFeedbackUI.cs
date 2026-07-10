using MMI2026.LabEscape.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MMI2026.LabEscape.UI
{
    public class CommandFeedbackUI : MonoBehaviour
    {
        [SerializeField] private Text feedbackText;

        public void Show(CommandData command)
        {
            if (feedbackText == null || command == null) return;
            feedbackText.text =
                $"[{command.SourceModality}] {command.Action} target={command.TargetId} raw=\"{command.RawCommand}\"";
        }
    }
}
