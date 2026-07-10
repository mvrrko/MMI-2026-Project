using UnityEngine;

namespace MMI2026.LabEscape.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonMovementController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float walkSpeed = 4.0f;
        [SerializeField] private float sprintSpeed = 6.5f;
        [SerializeField] private float gravity = -20.0f;

        private float verticalVelocity;

        private void Awake()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
        }

        private void Update()
        {
            var moveX = UnityEngine.Input.GetAxis("Horizontal");
            var moveZ = UnityEngine.Input.GetAxis("Vertical");
            var desiredMove = (transform.right * moveX) + (transform.forward * moveZ);
            var speed = UnityEngine.Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
            characterController.Move(desiredMove * (speed * Time.deltaTime));

            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            verticalVelocity += gravity * Time.deltaTime;
            characterController.Move(Vector3.up * (verticalVelocity * Time.deltaTime));
        }
    }
}
