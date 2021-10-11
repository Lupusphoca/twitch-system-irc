namespace PierreARNAUDET.Player
{
    using UnityEngine;

    using PierreARNAUDET.Camera;
    using PierreARNAUDET.Core.Attributes;

    public class PlayerData : MonoBehaviour
    {
        [Data]
        [SerializeField] CharacterController characterController;
        [SerializeField] CameraHeadBobbing cameraHeadBobbing;
        [SerializeField] Transform transformCameraPlayer;
        [SerializeField] Transform transformCameraPivot;

        public Transform TransformCameraPlayer { get => transformCameraPlayer; }
        public CharacterController CharacterController { get => characterController; }
        public CameraHeadBobbing CameraHeadBobbing { get => cameraHeadBobbing; }
        public Transform TransformCameraPivot { get => transformCameraPivot; }

        [Header("Character movement variables")]
        [SerializeField] [Range(1f, 20f)] float movementSpeed = 0.1f;
        [SerializeField] string horizontalAxisName = "Horizontal";
        string verticalAxisName = "Vertical";
        float yPlayerVelocity = 0.0f;

        public float MovementSpeed { get => movementSpeed; }
        public string HorizontalAxisName { get => horizontalAxisName; }
        public string VerticalAxisName { get => verticalAxisName; }
        public float YPlayerVelocity { get => yPlayerVelocity; set => yPlayerVelocity = value; }

        [Header("Character jump variables")]
        [SerializeField] AnimationCurve jumpFallOf;
        [SerializeField] [Range(1f, 10f)] private float jumpMultiplier = 4.0f;
        [SerializeField] KeyCode jumpKey;
        [SerializeField] AnimationCurve crouchCurve;
        [SerializeField] float minimalVelocityToStartLerpCrouch = 8f;
        bool isJumping = false;

        public AnimationCurve JumpFallOf { get => jumpFallOf; }
        public float JumpMultiplier { get => jumpMultiplier; }
        public KeyCode JumpKey { get => jumpKey; }
        public AnimationCurve CrouchCurve { get => crouchCurve; }
        public float MinimalVelocityToStartLerpCrouch { get => minimalVelocityToStartLerpCrouch; }
        public bool IsJumping { get => isJumping; set => isJumping = value; }

        [Header("Jittering slope control")]
        [SerializeField] [Range(0f, 10f)] float slopeForceRayLength = 1.5f;
        [SerializeField] [Range(0f, 10f)] float slopeForce = 5.0f;

        public float SlopeForceRayLength { get => slopeForceRayLength; }
        public float SlopeForce { get => slopeForce; }
    }
}