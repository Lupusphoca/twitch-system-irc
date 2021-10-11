namespace PierreARNAUDET.Player
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;

    public class PlayerMovement : MonoBehaviour
    {
        [Data]
        [SerializeField] PlayerData playerData;

        RaycastHit hit;

        public void MovePlayer()
        {
            var horizInput = Input.GetAxisRaw(playerData.HorizontalAxisName);
            var vertInput = Input.GetAxisRaw(playerData.VerticalAxisName);

            var forwardMovement = playerData.CharacterController.transform.forward * vertInput;
            var rightMovement = playerData.CharacterController.transform.right * horizInput;

            playerData.CharacterController.Move(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * playerData.MovementSpeed * Time.deltaTime);

            if ((vertInput != 0 || horizInput != 0) && OnSlope())
            {
                playerData.CharacterController.Move(Vector3.down * playerData.CharacterController.height / 2 * playerData.SlopeForce * Time.deltaTime);

            }
            else
            {
                playerData.CharacterController.SimpleMove(Vector3.zero);
            }
        }

        private bool OnSlope()
        {
            if (playerData.IsJumping)
            {
                return false;
            }

            if (Physics.Raycast(transform.position, Vector3.down, out hit, playerData.CharacterController.height / 2 * playerData.SlopeForceRayLength))
            {
                if (hit.normal != Vector3.up)
                {
                    return true;
                }
            }

            return false;
        }
    }
}