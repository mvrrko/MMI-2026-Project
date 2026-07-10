using UnityEngine;

namespace MMI2026.LabEscape.Interactables
{
    public class KeycardInteractable : InteractableBase
    {
        [SerializeField] private bool hideOnPickup = true;

        public override void Execute()
        {
            if (hideOnPickup) gameObject.SetActive(false);
        }
    }
}
