using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool isGrounded;
    private bool canDoubleJump;

    public Transform orientation = null;
    private Vector3 moveDirection = new Vector3(0, 0, 0);

    private Rigidbody rigidBody = null;
    public float moveSpeed = 5f;
    public float airMultiplier = 0.4f;
    private bool jumpRequested;
    public float jumpForce = 8f;

    public LayerMask groundMask;        
    public Collider groundCheckTrigger;

    public bool canMove = true; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // GroundCheck();
        PlayerInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        Walk();               // jump WHILE walking
        if (jumpRequested)
        {
            Jump();             
        }
    }
    private void PlayerInput()
    {
        // get input for movement
        // raw to prevent any kind of smoothing and floaty-ness
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump Input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                jumpRequested = true;
                canDoubleJump = true;
                Debug.Log("Jump");
            }
            else if (canDoubleJump)  //if i turn this off Double jump is disabled
            {
                jumpRequested = true;
                canDoubleJump = false;
                Debug.Log("Double Jump");
            }
        }

        //if (Input.GetButtonDown("Jump") && isGrounded == true)
        //{
        //    isGrounded = false;
        //    canDoubleJump = true;

        //    Debug.Log("Jump");
        //}
        //else if (Input.GetButtonDown("Jump") && canDoubleJump == true)
        //{
        //    canDoubleJump = false;
        //    Debug.Log("Double Jump");
        //}
    }

    private void Walk()
    {
        if (!canMove) // stop walking if player cant move
        {
            PlayerAudio.instance.StopWalk();
            return;
        }
        // calculate movement direction:

        // to walk in the direction we are looking at
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // to add a force to the rigidbody in the move direction
        if (isGrounded) // on ground
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else // in air
        {
            rigidBody.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);   //when falling
        }

        //walksfx
        if (isGrounded && moveDirection.magnitude > 0.1f)
            PlayerAudio.instance.PlayWalk();
        else
            PlayerAudio.instance.StopWalk();

    }

    private void Jump()
    {
        // Reset vertical velocity for *CONSISTENT* jumps
        rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x, 0f, rigidBody.linearVelocity.z);

        // Apply jump force
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //jumpsfx
        PlayerAudio.instance.PlayJump();

        jumpRequested = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundMask) != 0)
        {
            isGrounded = true;
            canDoubleJump = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundMask) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & groundMask) != 0)
        {
            isGrounded = false;
        }
    }

    //private void GroundCheck()
    //{
    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

    //    // When we land, double jump resets
    //    if (isGrounded)
    //    {
    //        canDoubleJump = false;
    //    }
    //}


    private void SpeedControl()
    {

        // get the flat velocity of your rigid body, which is just the x and z axis
        // if this velocity is greater than the movement speed you want to create a new vector
        // set it equal to your normalized flat velocity times your movement speed
        // and then apply this limited velocity again to your rigid body.

        Vector3 flatVel = new Vector3(rigidBody.linearVelocity.x, 0f, rigidBody.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigidBody.linearVelocity = new Vector3(limitedVel.x, rigidBody.linearVelocity.y, limitedVel.z);
        }
    }


}
