using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float speedIncrement = 0.001f;
    [SerializeField] float gravity = -13.0f;
    float gravityTmp;
    [SerializeField] float jumpForce = 5;
    [SerializeField][Range(0f,0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0f,0.5f)] float mouseSmoothTime = 0.03f;
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
    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameObject.FindObjectOfType<PlayerStartingPoint>().position;
        gravityTmp = gravity;
        cc = GetComponent<CharacterController>();

        slide_event_fmod = FMODUnity.RuntimeManager.CreateInstance("event:/Slide");

        beatController = GameObject.FindObjectOfType<BeatController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping)
        {
            groundedCheck = isJumping;
        }

        if (isJumping != groundedCheck)
        {
            groundedCheck = isJumping;
            Debug.Log(grounded);
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
        if (cc.velocity != Vector3.zero)
        {
            musicTimer += Time.deltaTime;

            if (musicSpeed <= 25)
                musicSpeed = speed;
            else if (musicTimer >= 0.3f)
            {
                musicTimer = 0;
                musicSpeed += speedIncrement * Time.deltaTime;
            }
            
            elastic = Mathf.SmoothDamp(elastic, musicSpeed, ref velocity, 3f);
            /*if (elastic >= speed)
            {
                /*musicTimer += Time.deltaTime;
                if (musicTimer >= 1 && elastic <= 35)
                {
                    elastic += .5f;
                    musicTimer = 0;
                }
                elastic += Time.deltaTime / 2;
            }
            else
            {
                elastic = Mathf.SmoothDamp(elastic, speed, ref velocity, 1.5f);
            }*/
        }
        /*else
            if (elastic > speed)
                elastic = Mathf.SmoothDamp(elastic, speed, ref velocity, 0.8f);*/
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
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.LeftShift)) && grounded)
        {
            
            slide_event_fmod.start();
            transform.localScale = Vector3.up/2 + Vector3.right + Vector3.forward;
            canMove = false;
            
            direction = transform.forward;
            speed += dashSpeed/* - speed * (Time.deltaTime * 0.5f)*/;
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
            glideTimer += Time.deltaTime;
            if (cc.velocity.y < 0 && speed < walkSpeed && glideTimer > 0.15f)
            {
                glideTimer = 0;
                speed += speedIncrement * 4 * Time.deltaTime;
            }

            cc.Move(direction * speed * Time.deltaTime + Vector3.up * gravity * 5 * Time.deltaTime + Input.GetAxisRaw("Horizontal") * transform.right * Time.deltaTime * 7);
            /*if (glideTimer >= dashTime)
            {
                getUp = true;
            }*/
        }
    }

    void UpdateMovement()
    {
        //Debug.Log(cc.isGrounded);
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        if (targetDir.magnitude == 0 && speed > 10)
        {
            //Debug.Log("Decrement");
            decrementTimer += Time.deltaTime;
            if (decrementTimer > 0.15f)
            {
                speed -= speedIncrement * Time.deltaTime;
                decrementTimer = 0;
            }
        }
        currentDirection = Vector2.SmoothDamp(currentDirection, targetDir, ref currentDirectionVelocity, moveSmoothTime);

        //if (cc.isGrounded)
        if (grounded || isWallrunning == true)
        {
            //Debug.Log("CanJump");
            isJumping = false;
            velocityY = 0;
            jumpTimer = 0;
            //isWallrunning = false;
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
            velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * speed;

        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            isJumping = true;
            Debug.Log(isWallrunning);
            //isWallrunning = false;
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

        //if (cc.isGrounded == false && isWallrunning == false)
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
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Ground")
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
