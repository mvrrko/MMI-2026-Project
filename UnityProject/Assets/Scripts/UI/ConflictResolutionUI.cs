using System;
using UnityEngine;
using UnityEngine.UI;

namespace MMI2026.LabEscape.UI
{
    public class ConflictResolutionUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text messageText;

        private Action<bool> pendingCallback;

        public void Show(string message, Action<bool> onChoice)
        {
            pendingCallback = onChoice;
            if (messageText != null) messageText.text = message;
            if (panel != null) panel.SetActive(true);
        }

        public void ChoosePointerTarget()
        {
            CloseWithChoice(true);
        }

        public void ChooseVoiceTarget()
        {
            CloseWithChoice(false);
        }

        public void CancelChoice()
        {
            pendingCallback?.Invoke(false);
            pendingCallback = null;
            if (panel != null) panel.SetActive(false);
        }

        private void CloseWithChoice(bool usePointer)
        {
            pendingCallback?.Invoke(usePointer);
            pendingCallback = null;
            if (panel != null) panel.SetActive(false);
        }
    }
}
