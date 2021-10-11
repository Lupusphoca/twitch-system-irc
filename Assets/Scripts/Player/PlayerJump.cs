namespace PierreARNAUDET.Player
{
    using System.Collections;

    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;

    public class PlayerJump : MonoBehaviour
    {
        [Data]
        [SerializeField] PlayerData playerData;

        public void JumpInput()
        {
            if (Input.GetKeyDown(playerData.JumpKey) && !playerData.IsJumping)
            {
                playerData.IsJumping = true;
                StartCoroutine(JumpEvent());
            }

            if (playerData.IsJumping)
            {
                playerData.YPlayerVelocity = playerData.CharacterController.velocity.y;
            }
        }

        private IEnumerator JumpEvent()
        {
            //playerData.CharacterController.slopeLimit = 90.0f;
            var timeInAir = 0.0f;

            do
            {
                var jumpForce = playerData.JumpFallOf.Evaluate(timeInAir);
                playerData.CharacterController.Move(Vector3.up * jumpForce * playerData.JumpMultiplier * Time.deltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while (!playerData.CharacterController.isGrounded && playerData.CharacterController.collisionFlags != CollisionFlags.Above);

            //playerData.CharacterController.slopeLimit = 45.0f;

            if (playerData.CharacterController.velocity.y < -playerData.MinimalVelocityToStartLerpCrouch)
            {
                StartCoroutine(Crouch());
            }
            else
            {
                playerData.IsJumping = false;
            }
        }

        private IEnumerator Crouch()
        {
            Vector3 _initialPosition = playerData.TransformCameraPivot.localPosition;
            float time = 0f;
            float crouchValue = 0f;

            do
            {
                crouchValue = playerData.CrouchCurve.Evaluate(time);
                playerData.TransformCameraPivot.localPosition = new Vector3(_initialPosition.x, _initialPosition.y + crouchValue, _initialPosition.z);
                time += Time.deltaTime;
                yield return null;
            } while (time < playerData.CrouchCurve.keys[playerData.CrouchCurve.length - 1].time);

            playerData.TransformCameraPivot.localPosition = _initialPosition;
            playerData.IsJumping = false;
        }
    }
}