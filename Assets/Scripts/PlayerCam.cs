using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float senX = 200f;
    [SerializeField] private float senY = 200f;
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;
    private Vector2 lastTouchPosition;
    private bool isRotating = false;

    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // For testing in editor with mouse
        float mouseX = Input.GetAxis("Mouse X") * senX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * senY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
#else
        HandleTouchInput();
#endif

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            isRotating = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        // Check if the touch is on the right half of the screen
        if (touch.position.x > Screen.width / 2)
        {
            if (touch.phase == TouchPhase.Began)
            {
                isRotating = true;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 delta = touch.deltaPosition;

                float touchX = delta.x * senX * 0.01f;
                float touchY = delta.y * senY * 0.01f;

                yRotation += touchX;
                xRotation -= touchY;
            }
        }
    }
}
