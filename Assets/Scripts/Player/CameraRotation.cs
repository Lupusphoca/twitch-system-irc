namespace PierreARNAUDET.Camera
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;

    public class CameraRotation : MonoBehaviour
    {
        [Data]
        [SerializeField] string mouseXInputName = "Mouse X";
        public string MouseXInputName { get => mouseXInputName; set => mouseXInputName = value; }
        [SerializeField] string mouseYInputName = "Mouse Y";
        public string MouseYInputName { get => mouseYInputName; set => mouseYInputName = value; }

        [SerializeField] Transform transformPlayer;
        public Transform TransformPlayer { get => transformPlayer; set => transformPlayer = value; }
        [SerializeField] Transform transformCamera;

        [Settings]
        [SerializeField] float mouseSensitivity = 150f;

        float xAxisClamp = 0f;

        public void RotateCamera()
        {
            var mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

            xAxisClamp += mouseY;

            if (xAxisClamp > 90.0f)
            {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(270.0f);
            }
            else if (xAxisClamp < -90.0f)
            {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampXAxisRotationToValue(90.0f);
            }

            transformCamera.Rotate(Vector3.left * mouseY);
            transformPlayer.Rotate(Vector3.up * mouseX);
        }

        private void ClampXAxisRotationToValue(float value)
        {
            var eulerRotation = transformCamera.eulerAngles;
            eulerRotation.x = value;
            transformCamera.eulerAngles = eulerRotation;
        }
    }
}