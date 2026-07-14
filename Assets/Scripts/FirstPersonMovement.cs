using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class FirstPersonMovement : MonoBehaviour
{
    // Constants
    const float GRAVITY = -9.81f;
    public readonly float[] TIMELINE_HEIGHTS = { -198.8f, 1.5f, 201.15f };

    private Transform _cameraTransform;
    private CharacterController _characterController;
    private InputAction _moveAction, _lookAction, _jumpAction;
    private InputAction _timeline1Action, _timeline2Action, _timeline3Action;
    private InputAction pauseAction;
    private GameObject[] _timelines = new GameObject[3];
    private int _currentTimelineIndex = 1;
    private Inventory inventory;
    private float _verticalRotation;
    private Vector3 _playerVelocity;
    private Vector3 _externalMovement;
    public bool[] inWarningZone;

    [SerializeField] float movementSpeed = 8f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float jumpHeight = 1f;
    public float moveSpeedMultiplier = 1f; // Used by other scripts



    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraTransform = transform.GetChild(0).GetComponent<Transform>();
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _timeline1Action = InputSystem.actions.FindAction("Timeline1");
        _timeline2Action = InputSystem.actions.FindAction("Timeline2");
        _timeline3Action = InputSystem.actions.FindAction("Timeline3");
        pauseAction = InputSystem.actions.FindAction("Pause");

        _timelines[0] = GameObject.Find("Timeline1");
        _timelines[1] = GameObject.Find("Timeline2");
        _timelines[2] = GameObject.Find("Timeline3");

        inventory = GetComponent<Inventory>();

        // Start in present timeline
        ActivateTimeline(1);

        // Events registration
        _timeline1Action.performed += OnTimeline1;
        _timeline2Action.performed += OnTimeline2;
        _timeline3Action.performed += OnTimeline3;
        pauseAction.performed += OnPause; // Read when "ESC" is pressed

        EnablePlayerInput();
        PauseManager.instance.SetPaused(PauseState.None);

        inWarningZone = new bool[] { false, false, false };
    }



    // Callback for jumping in timeline1
    private void OnTimeline1(InputAction.CallbackContext ctx)
    {
        // If trying to jump in the past is risky, don't do anything the first time (if player retry he will jump)
        if (inWarningZone[0])
        {
            EventsManager.Instance.InvokeWarningEvent();
            SetWarning(0, false);
            //warningAccepted[0] = true;
            return;
        }

        ActivateTimeline(0);
    }

    // Callback for jumping in timeline2
    private void OnTimeline2(InputAction.CallbackContext ctx)
    {
        // If trying to jump in the past is risky, don't do anything the first time (if player retry he will jump)
        if (inWarningZone[1])
        {
            EventsManager.Instance.InvokeWarningEvent();
            SetWarning(1, false);
            //warningAccepted[1] = true;
            return;
        }

        ActivateTimeline(1);
    }

    // Callback for jumping in timeline3
    private void OnTimeline3(InputAction.CallbackContext ctx)
    {
        // If trying to jump in the future is risky, don't do anything, unless player retry
        if (inWarningZone[2])
        {
            EventsManager.Instance.InvokeWarningEvent();
            SetWarning(2, false);
            return;
        }

        ActivateTimeline(2);
    }



    public void SetWarning(int warningTimelineIndex, bool isWarning)
    {
        // Change warning state
        inWarningZone[warningTimelineIndex] = isWarning;
    }


    public void ResetWarning(int warningTimelineIndex)
    {
        // Reset timeline warning state
        inWarningZone[warningTimelineIndex] = false;
    }



    private void OnPause(InputAction.CallbackContext ctx)
    {
        switch (PauseManager.instance.paused)
        {
            // Game running -> pause it normally
            case PauseState.None:
                PauseManager.instance.SetPaused(PauseState.Menu);
                break;

            // Game on pause menu
            case PauseState.Menu:
                // If player press ESC on options menu, just go back to pause menu
                if (UIManager.instance.IsESCPressedOnOptionsMenu()) return;
                
                // If menu was opened over a tutorial panel
                if (PauseManager.instance.TutorialWasInterrupted)
                {
                    // Reset tutorial as before
                    PauseManager.instance.TutorialWasInterrupted = false;
                    PauseManager.instance.SetPaused(PauseState.Tutorial);
                }
                else
                {
                    // Close pause menu normally
                    PauseManager.instance.SetPaused(PauseState.None);
                }

                break;

            case PauseState.Tutorial:
                // Open pause menu, interrupting current tutorial
                PauseManager.instance.TutorialWasInterrupted = true;
                PauseManager.instance.SetPaused(PauseState.Menu);
                break;

            case PauseState.Defeat:
                DisablePlayerInput();
                break;
        }
    }



    private void OnDestroy()
    {
        if (_timeline1Action != null)
            _timeline1Action.performed -= OnTimeline1;

        if (_timeline2Action != null)
            _timeline2Action.performed -= OnTimeline2;

        if (_timeline3Action != null)
            _timeline3Action.performed -= OnTimeline3;

        if (pauseAction != null)
            pauseAction.performed -= OnPause;
    }



    // Update is called once per frame
    private void LateUpdate()
    {
        if (PauseManager.instance.paused != PauseState.None)
            return;

        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        Vector2 lookValue = _lookAction.ReadValue<Vector2>();

        CalculateVerticalMovement(); // Checks if the player has jumped and adjust verical movement accordingly

        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveValue.x, 0, moveValue.y));

        Vector3 horizontalMovement = moveDirection * movementSpeed * moveSpeedMultiplier;
        Vector3 verticalMovement = Vector3.up * _playerVelocity.y;

        _characterController.Move(
            (horizontalMovement + verticalMovement) * Time.deltaTime
            + _externalMovement);

        // Rotation
        transform.Rotate(new Vector3(0, lookValue.x, 0) * rotationSpeed * Time.deltaTime);

        _verticalRotation -= lookValue.y * rotationSpeed * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f); // Limits the vertical rotation angle between -90 and 90 degrees
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);

        _externalMovement = Vector3.zero;
    }



    private void ActivateTimeline(int activeIndex)
    {
        // Player can only grab objects from the future
        if((_currentTimelineIndex == 0 || _currentTimelineIndex == 1) && !inventory.IsEmpty())
        {
            inventory.ClearInventory();
        }

        // Destination timeline height
        float targetBaseHeight = TIMELINE_HEIGHTS[activeIndex] + (transform.position.y - TIMELINE_HEIGHTS[_currentTimelineIndex]);

        // Attiva solo la timeline scelta
        for (int i = 0; i < _timelines.Length; i++)
        {
            _timelines[i].SetActive(i == activeIndex);
        }

        Physics.SyncTransforms();


        _characterController.enabled = false;

        // Same position but with new y coordinate, potentially unsafe
        Vector3 targetPos = new Vector3(transform.position.x, targetBaseHeight, transform.position.z);
        
        // Search nearest valid position
        Vector3 safePos = FindNearestValidPosition(targetPos);

        transform.position = safePos;

        _characterController.enabled = true;

        // Reset vertical velocity
        _playerVelocity.y = 0f;

        // Update current timeline
        _currentTimelineIndex = activeIndex;
    }



    private void CalculateVerticalMovement()
    {
        bool isGrounded = _characterController.isGrounded;

        if (isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -5f;
        }

        if (isGrounded && _jumpAction.WasPressedThisFrame())
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
        }

        _playerVelocity.y += GRAVITY * Time.deltaTime; // Apply gravity
    }



    public void ForceMove(Vector3 delta)
    {
        _externalMovement += delta;
    }



    public void SetVerticalVelocity(float velocity)
    {
        _playerVelocity.y = velocity;
    }



    public int GetCurrentTimelineIndex()
    {
        return _currentTimelineIndex;
    }



    bool IsPositionValid(Vector3 position)
    {
        CharacterController cc = GetComponent<CharacterController>();

        float radius = cc.radius;
        float height = cc.height;

        Vector3 bottom = position + Vector3.up * radius;
        Vector3 top = position + Vector3.up * (height - radius);

        return !Physics.CheckCapsule(
            bottom,
            top,
            radius,
            LayerMask.GetMask("Environment")
        );
    }



    Vector3 FindNearestValidPosition(Vector3 targetPosition)
    {
        float castRadius = _characterController.radius * 0.9f;
        Vector3 castOrigin = targetPosition + Vector3.up * 10f;

        if (Physics.SphereCast(
                castOrigin,
                castRadius,
                Vector3.down,
                out RaycastHit hit,
                30f,
                LayerMask.GetMask("Ground")))
        {
            // Maintain player attached to the ground
            Vector3 groundedPosition = hit.point;

            // CharacterController center
            groundedPosition.y += _characterController.skinWidth + 1f;

            if (IsPositionValid(groundedPosition))
                return groundedPosition;

            // Radial search around the found point
            return FindNearbyValidPosition(groundedPosition);
        }

        return targetPosition;
    }



    Vector3 FindNearbyValidPosition(Vector3 center)
    {
        float searchRadius = 0.5f;
        float maxRadius = 5f;
        float radiusStep = 0.5f;

        while (searchRadius <= maxRadius)
        {
            const int samples = 24;

            for (int i = 0; i < samples; i++)
            {
                float angle = i * Mathf.PI * 2f / samples;

                Vector3 candidate =
                    center +
                    new Vector3(
                        Mathf.Cos(angle) * searchRadius,
                        1f,
                        Mathf.Sin(angle) * searchRadius);

                if (IsPositionValid(candidate))
                    return candidate;
            }

            searchRadius += radiusStep;
        }

        return center;
    }



    public void DisablePlayerInput()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _jumpAction.Disable();

        _timeline1Action.Disable();
        _timeline2Action.Disable();
        _timeline3Action.Disable();
    }



    public void EnablePlayerInput()
    {
        _moveAction.Enable();
        _lookAction.Enable();
        _jumpAction.Enable();

        _timeline1Action.Enable();
        _timeline2Action.Enable();
        _timeline3Action.Enable();
    }
}