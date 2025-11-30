using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    
    [Header("References")]
    public Transform player = null;
    public Transform playerObj = null;
    public Transform orientation = null;
    public Rigidbody rigidBody = null;  

    public float rotationSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // lock cursor in center of screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate orientation
        //Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //orientation.forward = viewDir.normalized;

        Vector3 flatCamForward = transform.forward;
        flatCamForward.y = 0f;
        orientation.forward = flatCamForward.normalized;


        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // if the input direction is not zero we want to
        // smoothly change the forward direction of the player object
        // to the input direction using our rotation speed 
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
