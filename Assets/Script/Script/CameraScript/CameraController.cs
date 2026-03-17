using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    float currentCameraSpeed = 5f;

    public float cameraSpeed;

    public bool slowCameraMovement = false;
    [Range(0, 20)] public float slowCameraSpeed;

    public bool fastCameraMovement = false;
    [Range(0, 20)] public float fastCameraSpeed;

    private Camera mainCamera;

    private bool cmUp = false;
    private bool cmDown = false;
    private bool cmLeft = false;
    private bool cmRight = false;

    private bool PresscmUp = false;
    private bool PresscmDown = false;
    private bool PresscmLeft = false;
    private bool PresscmRight = false;

    private float timePressCMUp = -1f;
    private float timePressCMDown = -1f;
    private float timePressCMLeft = -1f;
    private float timePressCMRight = -1f;

    private MoveDirection moveDirection = MoveDirection.None;

    void Start()
    {
       mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        cmUp            = Input.GetKey(KeyCode.UpArrow);
        cmDown          = Input.GetKey(KeyCode.DownArrow);
        cmLeft          = Input.GetKey(KeyCode.LeftArrow);
        cmRight         = Input.GetKey(KeyCode.RightArrow); 

        PresscmUp       = Input.GetKeyDown(KeyCode.UpArrow);
        PresscmDown     = Input.GetKeyDown(KeyCode.DownArrow);
        PresscmLeft     = Input.GetKeyDown(KeyCode.LeftArrow);
        PresscmRight    = Input.GetKeyDown(KeyCode.RightArrow);

        if (PresscmUp)      { timePressCMUp = Time.time; }
        if (PresscmDown)    { timePressCMDown = Time.time; }
        if (PresscmLeft)    { timePressCMLeft = Time.time; }
        if (PresscmRight)   { timePressCMRight = Time.time; }

        if (cmLeft && cmRight)
        {
            moveDirection = (timePressCMLeft > timePressCMRight) ? MoveDirection.Left : MoveDirection.Right;
        }
        else if (cmUp && cmDown)
        {
            moveDirection = (timePressCMUp > timePressCMDown) ? MoveDirection.Up : MoveDirection.Down;
        }
        else if (cmUp)
        {
            moveDirection = MoveDirection.Up;
        }
        else if (cmDown)
        {
            moveDirection = MoveDirection.Down;
        }
        else if (cmLeft)
        {
            moveDirection = MoveDirection.Left;
        }
        else if (cmRight)
        {
            moveDirection = MoveDirection.Right;
        }
        else
        {
            moveDirection = MoveDirection.None;
        }
        MoveCamera();
        CameraZoom();
    }
    private void MoveCamera()
    {
        switch (moveDirection)
        {
            case MoveDirection.Left:
                transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
                break;
            case MoveDirection.Right:
                transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
                break;
            case MoveDirection.Up:
                transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
                break;
            case MoveDirection.Down:
                transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
                break;
            default:
                break;
        }
    }
    private void CameraZoom()
    {
        if (Input.GetKey(KeyCode.PageUp))
        {
            mainCamera.orthographicSize -= cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.PageDown))
        {
            mainCamera.orthographicSize += cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightControl))
        {
            slowCameraMovement = true;
            cameraSpeed = slowCameraSpeed;
            Debug.Log("Slow Camera Movement");
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            slowCameraMovement = false;
            cameraSpeed = currentCameraSpeed;
        }
        
        if (Input.GetKey(KeyCode.RightShift))
        {
            fastCameraMovement = true;
            cameraSpeed = fastCameraSpeed;
            Debug.Log("Fast Camera Movement");
        }
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            fastCameraMovement = false;
            cameraSpeed = currentCameraSpeed;
        }
    }
}

