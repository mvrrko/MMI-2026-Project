using System;
using UnityEngine;
using UnityEngine.UI;

namespace MMI2026.LabEscape.UI
{
    public enum ConflictChoice
    {
        Pointer = 0,
        Voice = 1,
        Cancel = 2
    }

    public class ConflictResolutionUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text messageText;

        private Action<ConflictChoice> pendingCallback;

        public void Show(string message, Action<ConflictChoice> onChoice)
        {
            pendingCallback = onChoice;
            if (messageText != null) messageText.text = message;
            if (panel != null) panel.SetActive(true);
        }

        public void ChoosePointerTarget()
        {
            CloseWithChoice(ConflictChoice.Pointer);
        }

        public void ChooseVoiceTarget()
        {
            CloseWithChoice(ConflictChoice.Voice);
        }

        public void CancelChoice()
        {
            CloseWithChoice(ConflictChoice.Cancel);
        }

        public void Hide()
        {
            pendingCallback = null;
            if (panel != null) panel.SetActive(false);
        }

        private void CloseWithChoice(ConflictChoice choice)
        {
            pendingCallback?.Invoke(choice);
            pendingCallback = null;
            if (panel != null) panel.SetActive(false);
        }
    }
}
