using UnityEngine;

namespace MMI2026.LabEscape.Interactables
{
    public class DoorInteractable : InteractableBase
    {
        [SerializeField] private Transform doorLeaf;
        [SerializeField] private Vector3 openLocalEuler = new Vector3(0f, 90f, 0f);

        public override void Execute()
        {
            if (doorLeaf != null) doorLeaf.localEulerAngles = openLocalEuler;
        }
    }
}
