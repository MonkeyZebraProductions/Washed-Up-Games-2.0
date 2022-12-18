using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Moving")]
    //Moving
    public float PlayerSpeed = 2.0f;
    public float moveSmoothing = 0.1f;
    public float rotationSmoothing = 0.1f;
    public Transform TopPart, BottomPart, Gun;
    Vector3 move;
    public bool CanMove;
    private bool _isSliding;
    private Vector3 slopeSlideVelocity;

    [Header("Jumping")]
    //Jumping
    public float JumpHeight = 1.0f;
    public float JumpMultiplier = 0.9f;
    public float CancelJumpMultiplier = 0.5f;
    public float GravityValue = -9.81f;
    public float GroundCheck = 0.2f;
    private float ySpeed;
    public int MaxJumps = 1;
    private int jumps;
    public float jumpButtonGracePeriod;
    private float currentJH, currentJM, fallVelocity;
    private bool _isJumping;

    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private Ray GroundRay;

    [Header("Dashing")]
    //Dashing
    public float DashSpeed = 5f;
    public float DashTime, DashCooldown = 1f;
    public float DashDecrease = 1f;
    public int MaxDashes = 1;
    private int dashes;
    private bool _isDashing, _stopWindow;

    [Header("Jetpack")]
    //Jetpack
    public float JetpackSpeed;
    public float MaxJetpackFeul=100;
    private float _jetpackFeul;
    public float JetpackDecreaseRate = 1;
    private bool _isJetpack;

    //references
    Vector2 smoothInputVelocity;
    float smoothVelocity;

    private CharacterController controller;
    private PlayerInput playerInput;
    public Vector3 playerVelocity;
    private Vector2 currentInputVector;
    private bool _isGrounded;
    private float _currentSpeed;

    //camera 
    private Transform cameraTransform;
    private Vector3 AdjustedCameraTransformZ;
    private CameraRotationOffset _cMO;

    private InputAction Move;
    private InputAction Jump;
    private InputAction Dash;

    [Header("Weapon Switching")]
    // Weapon Switching
    public float weaponIndicator;
    public GameObject[] weapons = new GameObject[4];

    [Header("UpgradeBools")]
    public bool DashEnabled, JetpackEnabled;

    [Header("Sounds")]
    public AudioSource Walk;

    public Animator TopAnimator, BottomAnimator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        dashes = MaxDashes;
        CanMove = true;
        _currentSpeed = PlayerSpeed;
        _jetpackFeul = MaxJetpackFeul;
        Jump = playerInput.actions["Jump"];
        Dash = playerInput.actions["Dash"];
    }

    void Update()
    {
        Move = playerInput.actions["Move"];

        _cMO = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CameraRotationOffset>();

        GroundRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * GroundCheck, Color.blue);
        _isGrounded = controller.isGrounded;

        AdjustedCameraTransformZ = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);

        //if (_isGrounded && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

        if (_isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            jumps = MaxJumps;
            fallVelocity = 1;
            if (slopeSlideVelocity != Vector3.zero)
            {
                _isSliding = true;
            }

            if (_isSliding == false)
            {
                ySpeed = -0.5f;
            }
            if (_jetpackFeul < MaxJetpackFeul && Jump.IsPressed() == false)
            {
                _jetpackFeul += JetpackDecreaseRate * Time.deltaTime;
            }
        }
        else if (!_isJumping && !_isJetpack)
        {
            fallVelocity += 0.01f;
            jumps = 0;
        }

        Vector3 AjdDash = AdjustVelocityToSlope(BottomPart.forward);

        if (CanMove)
        {
            //Move Player
            Vector2 moveInput = Move.ReadValue<Vector2>();
            if (_isDashing)
            {
                controller.Move(AjdDash * DashSpeed * Time.deltaTime);
            }
            else
            {

                currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInput, ref smoothInputVelocity, moveSmoothing);
                move = new Vector3(currentInputVector.x, 0, currentInputVector.y);
                move.y = 0f;
                move = move.x * cameraTransform.right.normalized + move.z * AdjustedCameraTransformZ.normalized;
                move = AdjustVelocityToSlope(move);
                if (!_isDashing && _currentSpeed > PlayerSpeed)
                {
                    _currentSpeed -= DashDecrease * Time.deltaTime;
                }
                

                controller.Move(move * Time.deltaTime * _currentSpeed);


            }

            if (moveInput.magnitude == 0 && _stopWindow)
            {
                _currentSpeed -= DashDecrease * Time.deltaTime;
                controller.Move(AjdDash * _currentSpeed * Time.deltaTime);
                
            }

            if(moveInput.magnitude>0)
            {
                _stopWindow = false;
                _currentSpeed = PlayerSpeed;
                if(!Walk.isPlaying && _isGrounded)
                {
                    Walk.Play();
                    TopAnimator.SetBool("IsWalking", true);
                    BottomAnimator.SetBool("IsWalking", true);
                }
                
            }
            else
            {
                TopAnimator.SetBool("IsWalking", false);
                BottomAnimator.SetBool("IsWalking", false);
            }

            if (_currentSpeed <= 0.5f)
            {
                _stopWindow = false;
                _currentSpeed = PlayerSpeed;
            }
            //rotate player parts
            Quaternion TopRotate = Quaternion.Euler(-_cMO.m_Offset.x, cameraTransform.rotation.eulerAngles.y - _cMO.m_Offset.y, 0f);
            TopPart.rotation = TopRotate;
            Quaternion GunRotate = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, 0f, 0f);
            Gun.localRotation = GunRotate;
            float targetAngel = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float Botangle = Mathf.SmoothDampAngle(BottomPart.eulerAngles.y, targetAngel, ref smoothVelocity, rotationSmoothing);
            BottomPart.rotation = Quaternion.Euler(0f, Botangle, 0f);


            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && jumps >= 1 && _isSliding == false)
            {
                _isJumping = true;
                currentJH = JumpHeight;
                currentJM = JumpMultiplier;
                fallVelocity = 1;
                jumps -= 1;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }


            //Dashing
            if (Dash.triggered && dashes >= 1 && DashEnabled)
            {
                StartCoroutine(DashTimer());
            }

            if (_isJumping)
            {
                ySpeed = currentJH;
            }

            if (_isJetpack && _jetpackFeul > 0)
            {
                
                fallVelocity = 1;
                if(ySpeed<JetpackSpeed)
                {
                    ySpeed += 3*Time.deltaTime;
                }
                
                _jetpackFeul -= JetpackDecreaseRate * Time.deltaTime;
            }
        }
        ySpeed += GravityValue * Time.deltaTime;
        playerVelocity.y = ySpeed;
        SetSlopeSlideVelocity();

        if (slopeSlideVelocity == Vector3.zero)
        {
            _isSliding = false;
        }
        if (_isSliding)
        {
            Vector3 velocity = slopeSlideVelocity;
            velocity.y = ySpeed;

            controller.Move(velocity * Time.deltaTime);
        }
        controller.Move(playerVelocity * Time.deltaTime*fallVelocity);
        //Debug.Log(fallVelocity);

       
    }

    void FixedUpdate()
    {
        if (_isJumping)
        {
            currentJH *= currentJM;
            if (currentJH <= 1)
            {
                _isJumping = false;
            }
        }
    }

    private void JumpVoid()
    {
        jumpButtonPressedTime = Time.time;
        if (jumps == 0)
        { 
            _isJumping = false;
            if (_jetpackFeul > 0 && JetpackEnabled)
            {
                _isJetpack = true;
            }

        }
    }

    void JumpCancel()
    {
        currentJM = CancelJumpMultiplier;
       _isJetpack = false;
        //fallVelocity = 1;
    }
    private IEnumerator DashTimer()
    {
        _stopWindow = false;
        //_currentSpeed = DashSpeed;
        _isDashing = true;
        dashes -= 1;
        yield return new WaitForSeconds(DashTime);
        _stopWindow = true;
        _isDashing = false;

        yield return new WaitForSeconds(DashCooldown);
        dashes = MaxDashes;
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {


        if (Physics.Raycast(GroundRay, out RaycastHit hitInfo, GroundCheck))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(GroundRay, out RaycastHit hitInfo, GroundCheck))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle >= controller.slopeLimit)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, ySpeed, 0), hitInfo.normal);
                return;
            }
        }

        if (_isSliding)
        {
            slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * 3;

            if (slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }

        slopeSlideVelocity = Vector3.zero;
    }

    private void OnEnable()
    {
        Jump.started += context => JumpVoid();
        Jump.canceled += context => JumpCancel();
    }

    private void OnDisable()
    {
        Jump.started -= context => JumpVoid();
        Jump.canceled -= context => JumpCancel();
    }
}
