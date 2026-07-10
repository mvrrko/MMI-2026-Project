using UnityEngine;

namespace MMI2026.LabEscape.Interactables
{
    public class GeneratorInteractable : InteractableBase
    {
        [SerializeField] private Light indicatorLight;

        public override void Execute()
        {
            if (indicatorLight != null) indicatorLight.enabled = true;
        }
    }
}
