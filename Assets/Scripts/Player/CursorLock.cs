namespace PierreARNAUDET.Camera
{
    using UnityEngine;

    public class CursorLock : MonoBehaviour
    {
        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}