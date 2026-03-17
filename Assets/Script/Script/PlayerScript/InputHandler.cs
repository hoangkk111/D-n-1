using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    #region Hold Input
    public bool HoldUp { get; private set; }
    public bool HoldDown { get; private set; }
    public bool HoldLeft { get; private set; }
    public bool HoldRight { get; private set; }
    #endregion

    #region Press Input
    public bool PressUp { get; private set; }
    public bool PressDown { get; private set; }
    public bool PressLeft {get; private set; }
    public bool PressRight {get; private set; }

    public bool PressJump { get; private set; }

    public bool PressAttack { get; private set; }
    #endregion

    #region Release Input
    public bool releaseJump { get; private set; }
    #endregion
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject); else Instance = this;
    }

    void Update()
    {
        HoldUp = Input.GetKey(KeyCode.W);
        HoldDown = Input.GetKey(KeyCode.S);
        HoldLeft = Input.GetKey(KeyCode.A);
        HoldRight = Input.GetKey(KeyCode.D);

        PressUp = Input.GetKeyDown(KeyCode.W);
        PressDown = Input.GetKeyDown(KeyCode.S);
        PressLeft = Input.GetKeyDown(KeyCode.A);
        PressRight = Input.GetKeyDown(KeyCode.D);

        PressJump = Input.GetKeyDown(KeyCode.Space);
        releaseJump = Input.GetKeyUp(KeyCode.Space);

        PressAttack = Input.GetKeyDown(KeyCode.Mouse0);

        #region Debug
        //if (HoldLeft) Debug.Log("Input get: A");
        //if (HoldRight) Debug.Log("Input get: D");

        //if (PressLeft) Debug.Log("Input get: A (Press)");
        //if (PressRight) Debug.Log("Input get: D (Press)");

        //if (PressJump) Debug.Log("Input get: Space");

        //if (PressAttack) Debug.Log("Input get: Mouse0 (Press)");
        #endregion
    }
}
