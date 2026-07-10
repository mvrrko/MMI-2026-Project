using MMI2026.LabEscape.Managers;
using UnityEngine;

namespace MMI2026.LabEscape.UI
{
    public class WinStateUI : MonoBehaviour
    {
        [SerializeField] private ObjectiveManager objectiveManager;
        [SerializeField] private GameObject winPanel;

        private void Start()
        {
            if (winPanel != null) winPanel.SetActive(false);

            if (objectiveManager != null && objectiveManager.IsCompleted)
            {
                ShowWinPanel();
            }
        }

        private void OnEnable()
        {
            if (objectiveManager != null)
            {
                objectiveManager.OnEscapeCompleted += ShowWinPanel;
            }
        }

        private void OnDisable()
        {
            if (objectiveManager != null)
            {
                objectiveManager.OnEscapeCompleted -= ShowWinPanel;
            }
        }

        private void ShowWinPanel()
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
    }
}
