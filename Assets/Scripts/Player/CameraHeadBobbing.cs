namespace PierreARNAUDET.Camera
{
    using UnityEngine;

    using PierreARNAUDET.Core.Attributes;

    public class CameraHeadBobbing : MonoBehaviour
    {
        [Settings]
        [SerializeField]
        [Range(1f, 100f)] private float transitionSpeed = 20.0f;
        [SerializeField]
        [Range(0.01f, 1f)] private float bobAmount = 0.05f;
        [SerializeField]
        [Range(1f, 10f)] private float bobSpeed = 5.0f;

        private Vector3 restPosition = Vector3.zero;
        float timer = Mathf.PI / 2;

        private void Awake()
        {
            restPosition = transform.localPosition;
        }

        public void HeadBobbingCamera()
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                timer += bobSpeed * Time.deltaTime;
                Vector3 newPosition = new Vector3(Mathf.Cos(timer) * bobAmount, restPosition.y + Mathf.Abs((Mathf.Sin(timer) * bobAmount)), restPosition.z); //abs val of y for a parabolic path
                transform.localPosition = newPosition;
            }
            else
            {
                timer = Mathf.PI / 2;
                Vector3 newPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, restPosition.x, transitionSpeed), Mathf.Lerp(transform.localPosition.y, restPosition.y, transitionSpeed * Time.deltaTime), Mathf.Lerp(transform.localPosition.z, restPosition.z, transitionSpeed * Time.deltaTime)); //transition smoothly from walking to stopping.
                transform.localPosition = newPosition;
            }

            if (timer > Mathf.PI * 2) timer = 0;
        }
    }
}