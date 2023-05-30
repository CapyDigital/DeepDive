using UnityEngine;

public class PlayerMovementDesktop : MonoBehaviour
{
    private const float FORWARD_ACCELERATION        = 10.0f;
    private const float BACKWARD_ACCELERATION       = 10.0f;
    private const float STRAFE_ACCELERATION         = 10.0f;
    private const float JUMP_ACCELERATION           = 200.0f;
    private const float GRAVITY_ACCELERATION        = 10.0f;
    private const float MAX_FORWARD_VELOCITY        = 4.0f;
    private const float MAX_BACKWARD_VELOCITY       = 2.0f;
    private const float MAX_STRAFE_VELOCITY         = 3.0f;
    private const float MAX_JUMP_VELOCITY           = 50.0f;
    private const float MAX_FALL_VELOCITY           = 100.0f;
    private const float ROTATION_VELOCITY_FACTOR    = 2.0f;
    private const float MIN_TILT_ROTATION           = 70.0f;
    private const float MAX_TILT_ROTATION           = 290.0f;

    private CharacterController _controller;
    private Transform           _cameraTransform;
    private Vector3             _acceleration;
    private Vector3             _velocity;
    private bool                _jump;
    private bool                _cameraLocked;
    private bool                _isCrouched;
    private bool                _canStandup;

    void Start()
    {
        _controller         = GetComponent<CharacterController>();
        _cameraTransform    = GetComponentInChildren<Camera>().transform;
        _acceleration       = Vector3.zero;
        _velocity           = Vector3.zero;
        _jump               = false;
        _cameraLocked       = false;
        _isCrouched         = false;
        _canStandup         = true;

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.visible      = false;
        Cursor.lockState    = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateJump();

        _cameraLocked = Input.GetMouseButton(1);
        if (!_cameraLocked)
        {
            UpdateRotation();
            UpdateTilt();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!_isCrouched)
                Crouch();
            else if (_isCrouched && _canStandup)
                Standup();
        }
    }

    private void UpdateJump()
    {
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
            _jump = true;
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * ROTATION_VELOCITY_FACTOR;

        transform.Rotate(0f, rotation, 0f);
    }

    private void UpdateTilt()
    {
        Vector3 cameraRotation = _cameraTransform.localEulerAngles;

        cameraRotation.x -= Input.GetAxis("Mouse Y") * ROTATION_VELOCITY_FACTOR;

        if (cameraRotation.x < 180f)
            cameraRotation.x = Mathf.Min(cameraRotation.x, MIN_TILT_ROTATION);
        else
            cameraRotation.x = Mathf.Max(cameraRotation.x, MAX_TILT_ROTATION);

        _cameraTransform.localEulerAngles = cameraRotation;
    }

    void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateAcceleration()
    {
        _acceleration.z = Input.GetAxis("Vertical");
        _acceleration.z *= (_acceleration.z > 0f) ? FORWARD_ACCELERATION : BACKWARD_ACCELERATION;

        _acceleration.x = Input.GetAxis("Horizontal") * STRAFE_ACCELERATION;

        if (_jump)
        {
            _acceleration.y = JUMP_ACCELERATION;
            _jump = false;
        }
        else if (_controller.isGrounded)
            _acceleration.y = 0f;
        else
            _acceleration.y = -GRAVITY_ACCELERATION;
    }

    private void UpdateVelocity()
    {
        _velocity += _acceleration * Time.fixedDeltaTime;

        _velocity.z = (_acceleration.z == 0f || _acceleration.z * _velocity.z < 0f) ?
            0f : Mathf.Clamp(_velocity.z, -MAX_BACKWARD_VELOCITY, MAX_FORWARD_VELOCITY);
        
        _velocity.x = (_acceleration.x == 0f || _acceleration.x * _velocity.x < 0f) ?
            0f : Mathf.Clamp(_velocity.x, -MAX_STRAFE_VELOCITY, MAX_STRAFE_VELOCITY);

        _velocity.y = (_acceleration.y == 0f) ?
            -0.1f : Mathf.Clamp(_velocity.y, -MAX_FALL_VELOCITY, MAX_JUMP_VELOCITY);
    }

    private void UpdatePosition()
    {
        Vector3 motion = _velocity * Time.fixedDeltaTime;

        _controller.Move(transform.TransformVector(motion));
    }

    private void Crouch()
    {
        Vector3 newYScale = new Vector3(1, 0.5f, 1);
        Vector3 newCamPos = new Vector3(0, 0.2f, 0);
        transform.GetChild(0).transform.localScale = newYScale;
        _cameraTransform.localPosition = newCamPos;
        _controller.height = 1;
        _isCrouched = true;
    }

    private void Standup()
    {
        Vector3 newYScale = new Vector3(1, 1, 1);
        transform.GetChild(0).transform.localScale = newYScale;
        Vector3 newCamPos = new Vector3(0, 0.65f, 0);
        _cameraTransform.localPosition = newCamPos;
        _controller.height = 2;
        _isCrouched = false;
    }

    public void ChangeStandupBlockedStatus(bool status) => _canStandup = status;
}
