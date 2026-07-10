using UnityEngine;

namespace MMI2026.LabEscape.Player
{
    public class FirstPersonLookController : MonoBehaviour
    {
        [SerializeField] private Transform playerBody;
        [SerializeField] private float lookSensitivity = 2.0f;
        [SerializeField] private float maxPitch = 80f;

        private float pitch;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            var mouseX = UnityEngine.Input.GetAxis("Mouse X") * lookSensitivity;
            var mouseY = UnityEngine.Input.GetAxis("Mouse Y") * lookSensitivity;

            pitch = Mathf.Clamp(pitch - mouseY, -maxPitch, maxPitch);
            transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}
