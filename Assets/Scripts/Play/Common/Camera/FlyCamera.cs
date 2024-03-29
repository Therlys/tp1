using UnityEngine;

namespace Game
{
    public class FlyCamera : MonoBehaviour
    {
        private const string MOUSE_X = "Mouse X";
        private const string MOUSE_Y = "Mouse Y";

        [Header("Key Mapping")] [SerializeField] private KeyCode forwardKey = KeyCode.W;
        [SerializeField] private KeyCode backwardKey = KeyCode.S;
        [SerializeField] private KeyCode strafeLeftKey = KeyCode.A;
        [SerializeField] private KeyCode strafeRightKey = KeyCode.D;
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [Header("Movement")] [SerializeField] private float speed = 5f;
        [SerializeField] private float runSpeedMultiplier = 4f;
        [SerializeField] private float mouseSensitivity = 1;

        private Vector2 rotationEuler;

        private void Awake()
        {
            rotationEuler = Vector3.zero;
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            //Rotation
            var mousePositionDelta = new Vector2(Input.GetAxisRaw(MOUSE_X), Input.GetAxisRaw(MOUSE_Y));

            rotationEuler += mousePositionDelta * mouseSensitivity;
            rotationEuler.y = Mathf.Clamp(rotationEuler.y, -90, 90);

            transform.rotation = Quaternion.Euler(-rotationEuler.y, rotationEuler.x, 0);

            //Translation
            var direction = Vector3.zero;

            if (Input.GetKey(forwardKey)) direction += Vector3.forward;
            if (Input.GetKey(backwardKey)) direction += Vector3.back;
            if (Input.GetKey(strafeLeftKey)) direction += Vector3.left;
            if (Input.GetKey(strafeRightKey)) direction += Vector3.right;

            var speed = this.speed;
            if (Input.GetKey(runKey)) speed *= runSpeedMultiplier;

            transform.Translate(direction * (speed * Time.deltaTime));
        }
    }
}