using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GrappleSystem : MonoBehaviour
{
    private PlayerInput playerInput;

    public WeaponSystemController weaponSystemController;

    public WeaponSystemController pistol;
    public WeaponSystemController shotgun;

    //input actions


    private InputAction Grapple;

    [Header("Grapple Hook")]
    //GameObject SetUp
    public GameObject GrappleObject;
    public GrapplingHook _gH, VisibleAnchor;
    public bool IsGrappling;
    private PlayerMovementScript _pMS;
    public Transform SpawnPoint;
    private CharacterController controller;

    //bool set up
    public bool _isAiming;
    private bool _moveToGrapple;
    Vector3 moveVector;

    [Header("When Object is Grappled")]
    //WhenObjectIsGrabbed
    public GameObject GrabbedObject;
    private Rigidbody _grappleRigidbody;
    public float LaunchForce;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        Grapple = playerInput.actions["Grapple"];
        IsGrappling = false;

        controller = GetComponentInParent<CharacterController>();
        _pMS = GetComponentInParent<PlayerMovementScript>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(SpawnPoint.position, SpawnPoint.forward * 10, Color.green);

        if (Grapple.triggered && weaponSystemController._isAiming && !weaponSystemController.isFiring)
        {
            IsGrappling = true;

            if (VisibleAnchor == null && GrabbedObject == null)
            {
                //spawn Grapple Hook if not visible or Grabbing
                _gH.target = SpawnPoint.position + SpawnPoint.forward * _gH.Length;
                Instantiate(GrappleObject, SpawnPoint.position + SpawnPoint.forward * _gH.SpawnDistance * 1.1f, Quaternion.identity);
                VisibleAnchor = FindObjectOfType<GrapplingHook>();
                VisibleAnchor.TargetReached = false;
            }
            else
            {
                if (GrabbedObject != null)
                {
                    GrabbedObject.transform.parent = null;
                    _grappleRigidbody.isKinematic = false;
                    _grappleRigidbody.AddForce(transform.forward * LaunchForce, ForceMode.Impulse);
                    GrabbedObject = null;
                    IsGrappling = false;
                }
                else if (VisibleAnchor.IsHooked)
                {
                    //moves the Player if attatched to a point
                    _moveToGrapple = true;
                    moveVector = VisibleAnchor.target - transform.position;
                    _pMS.CanMove = false;
                }
                else
                {
                    //brings hook back
                    VisibleAnchor.TargetReached = false;
                    VisibleAnchor.target = SpawnPoint.position;
                    _pMS.CanMove = false;
                }
            }
        }

        //destroys hook once it gets close to the player
        if (Vector3.Distance(VisibleAnchor.transform.position, transform.position) <= VisibleAnchor.SpawnDistance)
        {
            if (VisibleAnchor.ObjectGrabbed)
            {
                GrabbedObject = VisibleAnchor.transform.GetChild(0).gameObject;
                GrabbedObject.transform.SetParent(this.transform);
                _grappleRigidbody = GrabbedObject.GetComponent<Rigidbody>();
                _grappleRigidbody.isKinematic = true;
            }
            else
            {
                IsGrappling = false;
            }
            Debug.Log("Die");
            Destroy(VisibleAnchor.gameObject);
            _moveToGrapple = false;
            _pMS.CanMove = true;
        }
        if (_moveToGrapple)
        {
            controller.Move(moveVector * Time.deltaTime * _gH.ZipSpeed);
        }
    }
}



