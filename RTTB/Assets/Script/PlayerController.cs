using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    [SerializeField] private float smoothCamFovMod = 0.5f;
    
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float speedIncrement = 0.001f;
    [SerializeField] float gravity = -13.0f;
    float gravityTmp;
    [SerializeField] float jumpForce = 5;
    [SerializeField][Range(0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] float fallGravityMultiplier = 1;
    [SerializeField] float smallJumpGravityMultiplier = 2;

    [SerializeField] float speed = 10;

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

    /// Wallrunning Var
    public Transform wallrunTransform;
    bool wallrunningJump = false;
    [SerializeField] float wallrunJumpSpeedXZ;
    [SerializeField] float wallrunJumpY;
    public Vector3 hitPointWall;
    public float wallrunTimer = 0.3f;
    public float wallrunCD = 0.2f;

    //bool isGliding = false;
    //float magicNumber = 0.0001f;
    Vector3 moveVector;
    public bool grounded = false;
    //List<GameObject> groundObjects = new List<GameObject>();
    // Temporaire pour le son de pas
    float timer;
    float decrementTimer;
    float jumpTimer;
    float glideTimer;
    float musicSpeed = 0;
    float musicTimer = 0;

    //// Glide Var
    bool getUp = false;
    [SerializeField] float dashTime = 1;
    [SerializeField] int dashSpeed = 5;
    private FMOD.Studio.EventInstance slide_event_fmod;
    Vector3 direction;

    float elastic;
    float velocity;
    BeatController beatController;

    bool groundedCheck;

    Vector2 targetDir;


    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    public float playerHeight = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        //transform.position = GameObject.FindObjectOfType<PlayerStartingPoint>().position;
        gravityTmp = gravity;
        cc = GetComponent<CharacterController>();

        slide_event_fmod = FMODUnity.RuntimeManager.CreateInstance("event:/Slide");

        beatController = GameObject.FindObjectOfType<BeatController>();
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection(Vector3 directionS)
    {
        return Vector3.ProjectOnPlane(directionS, slopeHit.normal).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        var fovMod = speed / 30;
        vcam.m_Lens.FieldOfView = Mathf.Lerp(60, 90,fovMod);
//        Debug.Log(vcam.m_Lens.FieldOfView + " " + fovMod);

        if (isJumping)
        {
            groundedCheck = isJumping;
        }

        if (isJumping != groundedCheck)
        {
            groundedCheck = isJumping;
            Debug.Log(grounded);
            if (grounded == true)
                CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Reception");
        }

        /*if (grounded != groundedCheck)
        {
            groundedCheck = grounded;
            Debug.Log(grounded);
            //FMODUnity.RuntimeManager.PlayOneShot("");
        }*/


        float g;
        beatController.event_fmod.getParameterByName("Speed", out g);
        elastic = Mathf.SmoothDamp(elastic, speed, ref velocity, 3f);
        //        Debug.Log(elastic + " " + speed + " " + musicSpeed);

        beatController.event_fmod.setParameterByName("Speed", elastic);
        //        Debug.Log(speed + " " + elastic + " " + musicSpeed);



        if (cc.collisionFlags == CollisionFlags.Below || cc.isGrounded)
        {
            //Debug.Log("Grounded " + CollisionFlags.Below.ToString());
            grounded = true;
        }
        else
        {
            grounded = false;
            //Debug.Log("Not Grounded");
        }
        UpdateMouseLook();

        UpdateGlissade();

        UpdateMovement();

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("is Wallrunning? " + isWallrunning);
        }

        if (wallrunTimer <= wallrunCD)
            wallrunTimer += Time.deltaTime;

        if (wallrunTransform != null && isJumping == true)
        {
            //                Debug.Log(Vector3.up * Mathf.Sqrt(-2f * gravity * wallrunJumpY));
            //cc.Move((((transform.position - wallrunTransform.position).normalized * wallrunJumpSpeedXZ) + Vector3.up * Mathf.Sqrt(-2f * gravity * jumpForce)) * Time.deltaTime);
            Vector3 dirXZ = ((transform.position - hitPointWall).normalized + transform.forward / 2);

            dirXZ.y = 0;
            cc.Move(((dirXZ * wallrunJumpSpeedXZ) + Vector3.up * Mathf.Sqrt(-2f * gravity * wallrunJumpY)) * Time.deltaTime);
        }

        //Debug.Log("Gravity state: " + gravity + " isGrounded? " + cc.isGrounded);
    }

    public void GravityOff()
    {
        //Debug.Log("No gravity");
        velocityY = 0;
        gravity = 0;
    }

    public void GravityOn()
    {
        //Debug.Log("Gravity");
        gravity = gravityTmp;
    }

    void UpdateGlissade()
    {
        Vector3 rightDir = transform.right * (Input.GetAxisRaw("Horizontal") * 2 * Time.deltaTime);
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.LeftShift)) && grounded)
        {

            slide_event_fmod.start();
            transform.localScale = Vector3.up / 2 + Vector3.right + Vector3.forward;
            canMove = false;

            direction = transform.forward;
            speed += dashSpeed * Time.deltaTime/* - speed * (Time.deltaTime * 0.5f)*/;
            if (speed > walkSpeed)
                speed = walkSpeed + 0.01f;
        }
        if (Input.GetButtonUp("Fire1") || Input.GetButtonDown("Jump") || getUp || Input.GetKeyUp(KeyCode.LeftShift))
        {
            transform.localScale = Vector3.up + Vector3.right + Vector3.forward;
            canMove = true;
            glideTimer = 0;
            getUp = false;
            slide_event_fmod.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            direction = Vector3.zero;
        }
        if (direction != Vector3.zero /*&& grounded*/)
        {
            /*glideTimer += Time.deltaTime;
            if (cc.velocity.y < 0 && speed < walkSpeed && glideTimer > 0.15f)
            {
                glideTimer = 0;
                speed += speedIncrement * 4 * Time.deltaTime;
            }*/

            cc.Move(direction * (speed * Time.deltaTime) + Vector3.up * (gravity * Time.deltaTime) + rightDir);
            /*if (glideTimer >= dashTime)
            {
                getUp = true;
            }*/
        }
    }

    private void FixedUpdate()
    {
        if (direction != Vector3.zero /*&& grounded*/)
        {
            glideTimer += Time.deltaTime;
            if (cc.velocity.y < -0.2f && speed < walkSpeed && glideTimer > 0.15f)
            {
                Debug.Log("SpeedIncrement Glide");
                glideTimer = 0;
                speed += speedIncrement * 2 * Time.deltaTime;
            }
        }


        if (targetDir.magnitude == 0)
        {
            //Debug.Log("Decrement");
            decrementTimer += Time.deltaTime;
            if (decrementTimer > 0.15f)
            {
                if (speed > 10)
                    speed -= speedIncrement * Time.deltaTime;
                decrementTimer = 0;
            }
        }

        timer += Time.deltaTime;

        if (Vector3.Distance(cc.velocity, Vector3.zero) > .25f && timer >= .3f && grounded && canMove == true)
        {
            timer = 0;
            moving = true;
            /*FMODUnity.RuntimeManager.PlayOneShot("event:/Walk");*/
            if (speed < walkSpeed)
                speed += speedIncrement * Time.deltaTime;
            //Debug.Log("Moving");
        }
    }

    void UpdateMovement()
    {
        //Debug.Log(cc.isGrounded);


        targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        //Debug.Log(targetDir.magnitude + "Magnitude");
        

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDir, ref currentDirectionVelocity, moveSmoothTime);

        if (grounded || isWallrunning == true)
        {
            isJumping = false;
            velocityY = 0;
            jumpTimer = 0;
        }
        else
        {
            if (jumpTimer > 0.3f)
            {
                isJumping = true;
            }
            else
            {
                jumpTimer += Time.deltaTime;
            }
        }

        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = Vector3.zero;
        if (canMove)
        {
            if (OnSlope())
            {
                Vector3 direction = (transform.forward * currentDirection.y + transform.right * currentDirection.x);
                velocity = GetSlopeMoveDirection(direction) * speed;
            }
            else
            {
                velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * speed;
            }
        }
            

        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            isJumping = true;
            Debug.Log(isWallrunning);

            if (isWallrunning)
            {
                wallrunTimer = 0;
                isWallrunning = false;
                wallrunTransform = transform.parent;
            }
            else
                velocityY = Mathf.Sqrt(-2f * gravity * jumpForce);

            transform.parent = null;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Jump");
        }

        if (grounded == false && isWallrunning == false)
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


        if (isWallrunning == false)
            velocity += Vector3.up * velocityY;

        if (velocity != Vector3.zero && canMove || isWallrunning)
        {
            if (Vector3.Distance(cc.velocity, Vector3.zero) < .25f || grounded == false)
            {
                moving = false;
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
        transform.Rotate(currentMouseDelta.x * mouseSensitivity * Vector3.up);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            transform.parent = hit.gameObject.transform;
            wallrunTransform = null;
        }
        else if (isWallrunning == false)
            transform.parent = null;

        if (hit.gameObject.transform != wallrunTransform)
        {
            //            Debug.Log("Stop Wallrunning Jump " + hit.gameObject.name);
            wallrunTransform = null;
        }
    }
}
