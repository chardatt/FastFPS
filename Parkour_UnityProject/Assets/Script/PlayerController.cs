using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    float gravityTmp;
    [SerializeField] float jumpForce = 5;
    [SerializeField][Range(0f,0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0f,0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] float fallGravityMultiplier = 1;
    [SerializeField] float smallJumpGravityMultiplier = 2;
    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController cc;
    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    public bool isJumping = false;
    public bool isWallrunning = false;
    public bool canMove = true;
    public bool moving = false;

    // Temporaire pour le son de pas
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        gravityTmp = gravity;
        cc = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        
        UpdateMovement();
    }

    public void GravityOff()
    {
        gravity = 0;
    }

    public void GravityOn()
    {
        gravity = gravityTmp;
    }

    void UpdateMovement()
    {
        Debug.Log(cc.isGrounded);
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDir, ref currentDirectionVelocity, moveSmoothTime);

        if (cc.isGrounded)
        {
            isJumping = false;
            velocityY = 0;
            //isWallrunning = false;
        }
        else
        {
            isJumping = true;
        }

        if (isWallrunning == false)
            velocityY += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && (isJumping == false || isWallrunning))
        {
            transform.parent = null;
            isJumping = true;
            velocityY = Mathf.Sqrt(-2f * gravity * jumpForce);
            //velocityY = jumpForce;
            Debug.Log("Jumping");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Jump");
        }

        if (cc.isGrounded == false && isWallrunning == false)
        {
            if (velocityY < 0)
            {
                velocityY += gravity * Time.deltaTime * fallGravityMultiplier;
            }
            else if (!Input.GetButton("Jump"))
            {
                velocityY += gravity * Time.deltaTime * smallJumpGravityMultiplier;
            }
        }

        Vector3 velocity = Vector3.zero;
        if (canMove)
            velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed;
        if (isWallrunning == false)
            velocity += Vector3.up * velocityY;

        if (velocity != Vector3.zero)
        {
            
            timer += Time.deltaTime;
            //if ((velocity.x > 0.25f || velocity.x < -0.25f) && (velocity.z > 0.25f || velocity.z < -0.25f) && timer >= .3f && cc.isGrounded)
            if (Vector3.Distance(cc.velocity, Vector3.zero) > .25f && timer >= .3f && cc.isGrounded)
            {
                timer = 0;
                moving = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/Walk");
                Debug.Log("Moving");
            }
            
            //if ((velocity.x < 0.25f || velocity.x > -0.25f) && (velocity.z < 0.25f || velocity.z > -0.25f))
            if (Vector3.Distance(cc.velocity, Vector3.zero) < .25f || cc.isGrounded == false)
            {
                moving = false;
                Debug.Log("Not Moving");
            }

            cc.Move(velocity * Time.deltaTime);
        }

    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90, 90);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        
        if (other.gameObject.tag == "Ground")
        {
            transform.parent = other.gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            transform.parent = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Ground")
        {
            if (transform.parent == null)
                transform.parent = hit.gameObject.transform;
        }
        else
            transform.parent = null;
    }
}
