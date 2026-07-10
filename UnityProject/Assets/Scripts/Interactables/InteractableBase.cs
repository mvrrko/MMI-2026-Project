using UnityEngine;

namespace MMI2026.LabEscape.Interactables
{
    public abstract class InteractableBase : MonoBehaviour
    {
        [SerializeField] private string objectId;
        public string ObjectId => string.IsNullOrWhiteSpace(objectId) ? gameObject.name : objectId;

        public abstract void Execute();
    }
}
