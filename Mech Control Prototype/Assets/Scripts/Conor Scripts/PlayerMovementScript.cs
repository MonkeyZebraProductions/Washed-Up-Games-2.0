using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{

    //Moving
    public float PlayerSpeed = 2.0f;
    public float CollisionFixForce=1f;
    public float moveSmoothing = 0.1f;
    public float rotationSmoothing = 0.1f;
    public Transform TopPart, BottomPart, Gun;
    Vector3 move;
    public bool CanMove;

    //Jumping
    public float JumpHeight = 1.0f;
    public float JumpMultiplier=0.9f;
    public float CancelJumpMultiplier = 0.5f;
    public float GravityValue = -9.81f;
    public float GroundCheck = 0.2f;
    private float ySpeed;
    public int MaxJumps=1;
    private int jumps;
    public float jumpButtonGracePeriod;
    private float currentJH, currentJM,fallVelocity;
    private bool _isJumping;

    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private Ray GroundRay;

    //Dashing
    public float DashSpeed=5f;
    public float DashTime,DashCooldown = 1f;
    public int MaxDashes = 1;
    private int dashes;
    private bool _isDashing;

    //references
    Vector2 smoothInputVelocity;
    float smoothVelocity;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Vector2 currentInputVector;
    private bool _isGrounded;
    private Transform cameraTransform;
    private Vector3 AdjustedCameraTransformZ;
    

    private InputAction Move;
    private InputAction Jump;
    private InputAction Dash;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        Move = playerInput.actions["Move"]; 
        Jump = playerInput.actions["Jump"];
        Dash = playerInput.actions["Dash"];
        cameraTransform = Camera.main.transform;
        dashes = MaxDashes;
        CanMove = true;
    }

    void Update()
    {
        GroundRay = new Ray(transform.position, Vector3.down);
        
        _isGrounded = controller.isGrounded;

        AdjustedCameraTransformZ=new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
        
        //if (_isGrounded && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

        if(_isGrounded)
        {
            lastGroundedTime = Time.time;
        }
      
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            jumps = MaxJumps;
            fallVelocity = 1;
            ySpeed = -0.5f;
        }
        else if (!_isJumping)
        {
            fallVelocity += 0.01f;
            jumps=0;
        }

        if (CanMove)
        {


            //Move Player
            Vector2 moveInput = Move.ReadValue<Vector2>();
            if (_isDashing)
            {
                controller.Move(BottomPart.forward * DashSpeed * Time.deltaTime);
            }
            else
            {
                currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInput, ref smoothInputVelocity, moveSmoothing);
                move = new Vector3(currentInputVector.x, 0, currentInputVector.y);
                move.y = 0f;
                move = move.x * cameraTransform.right.normalized + move.z * AdjustedCameraTransformZ.normalized;
                controller.Move(move * Time.deltaTime * PlayerSpeed);
            }

            //rotate player parts
            Quaternion TopRotate = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f);
            TopPart.rotation = TopRotate;
            Quaternion GunRotate = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x, 0f, 0f);
            Gun.localRotation = GunRotate;
            float targetAngel = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(BottomPart.eulerAngles.y, targetAngel, ref smoothVelocity, rotationSmoothing);
            BottomPart.rotation = Quaternion.Euler(0f, angle, 0f);



            // Changes the height position of the player..

            //if()
            //{
            //    jumpButtonPressedTime = Time.time;
            //}

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && jumps >= 1)
            {
                _isJumping = true;
                currentJH = JumpHeight;
                currentJM = JumpMultiplier;
                fallVelocity = 1;
                jumps -= 1;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }

            if (_isJumping)
            {
                ySpeed = currentJH;
            }
            playerVelocity = AdjustVelocityToSlope(playerVelocity);
            ySpeed += GravityValue * Time.deltaTime;
            playerVelocity.y = ySpeed;
            controller.Move(playerVelocity * Time.deltaTime*fallVelocity);

            //Dashing
            if (Dash.triggered && dashes >= 1)
            {
                StartCoroutine(DashTimer());
            }
        }

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
    }

    void JumpCancel()
    {
        currentJM = CancelJumpMultiplier;
    }
    private IEnumerator DashTimer()
    {
        _isDashing = true;
        dashes -= 1;
        yield return new WaitForSeconds(DashTime);
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
