using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Foces")]
    public float speed = 10;
    public float jumpForce = 10;
    public int jumpCount = 2;
    public float groundPoundForce = 20;
    [Space]
    [Header("Jump Camera Shack")]
    public float cameraShackJumpDurashion = 1;
    public float cameraShackJumpDecreaseFactor = 1;
    public float cameraShackJumpAmount = 1;
    [Space]
    [Header("Ground Pound Camera Shack")]
    public float cameraShackGroundPoundDurashion = 1;
    public float cameraShackGroundPoundDecreaseFactor = 1;
    public float cameraShackGroundPoundAmount = 1;
    [Space]
    [Header("Stun Times")]
    public float groundPoundGroundStunTime = 2;
    public float groundPoundAirStunTime = 4;
    [Space]
    [Header("Jump Cloud")]
    public GameObject jumpCloud;
    public Transform jumpCloudEmitter;
    [Space]
    [Header("Telemetry")]
    [SerializeField]
    private int jumpCounter;
    [SerializeField]
    private bool groundPound = false;
    [SerializeField]
    private bool airStunTimeUpGroundPound = true;


    private Rigidbody2D rb;
    private Animator anim;
    private GameObject mainCam;
    private CameraShack cameraShack;
    private GameObject PlayerGroundTrigger;
    private GetStateOfTrigger PlayerGroundCollider;
    private Timer timer;



    void Start()
    {
        timer = GetComponent<Timer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        PlayerGroundTrigger = GameObject.Find("Player-Ground-Trigger");

        PlayerGroundCollider = PlayerGroundTrigger.GetComponent<GetStateOfTrigger>();

        mainCam = Camera.main.gameObject;
        cameraShack = mainCam.GetComponent<CameraShack>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GroundPond();
        Jump();
        Animator();

    }

    public void Animator()
    {

        if(!groundPound && airStunTimeUpGroundPound)
        {
            if (rb.velocity.x != 0 && rb.velocity.y == 0 && !groundPound)
            {
                anim.SetBool("Is-Running", true);
                anim.SetBool("Is-Fly-Running", false);
                anim.SetBool("Is-Idle", false);

            }
            else if (rb.velocity.x != 0 && rb.velocity.y != 0 && !groundPound)
            {
                anim.SetBool("Is-Fly-Running", true);
                anim.SetBool("Is-Running", false);
                anim.SetBool("Is-Idle", false);
                anim.SetBool("Ground-Pound-Fall", false);
                anim.SetBool("Ground-Pound-Smash", false);
            }
            else if (PlayerGroundCollider.stateOftrigger)
            {
                anim.SetBool("Is-Running", false);
                anim.SetBool("Is-Fly-Running", false);
                anim.SetBool("Is-Idle", true);
                anim.SetBool("Ground-Pound-Fall", false);
                anim.SetBool("Ground-Pound-Smash", false);

            }
            else if (!PlayerGroundCollider.stateOftrigger)
            {
                anim.SetBool("Is-Running", false);
                anim.SetBool("Is-Fly-Running", true);
                anim.SetBool("Is-Idle", false);
                anim.SetBool("Ground-Pound-Fall", false);
                anim.SetBool("Ground-Pound-Smash", false);
            }
        }else if(groundPound && !airStunTimeUpGroundPound)
        {
            anim.SetBool("Is-Running", false);
            anim.SetBool("Is-Fly-Running", false);
            anim.SetBool("Is-Idle", false);
            anim.SetBool("Ground-Pound-Fall", true);
            anim.SetBool("Ground-Pound-Smash", false);
        }else if(groundPound && airStunTimeUpGroundPound && PlayerGroundCollider.stateOftrigger)
        {
            anim.SetBool("Is-Running", false);
            anim.SetBool("Is-Fly-Running", false);
            anim.SetBool("Is-Idle", false);
            anim.SetBool("Ground-Pound-Fall", true);
            anim.SetBool("Ground-Pound-Smash", true);
        }
        



        if(rb.velocity.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }else if(rb.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    public void Jump()
    {
        


        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter > 0 && !groundPound)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Instantiate(jumpCloud, jumpCloudEmitter.position, jumpCloud.transform.rotation);


            cameraShack.shackAmount = cameraShackJumpAmount;
            cameraShack.shackDurashion = cameraShackJumpDurashion;
            cameraShack.decreaseFactor = cameraShackGroundPoundDecreaseFactor;
            cameraShack.Shack = true;
            jumpCounter--;
        }

        if (PlayerGroundCollider.stateOftrigger && jumpCounter < jumpCount)
        {
            jumpCounter = jumpCount;
        }
    }

    public void GroundPond()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            
            
            if (!PlayerGroundCollider.stateOftrigger)
            {
                timer.StartTimer();
                airStunTimeUpGroundPound = false;
                groundPound = true;

            }
            
        }
        if(PlayerGroundCollider.stateOftrigger && groundPound)
        {
            
            
            cameraShack.shackAmount = cameraShackGroundPoundAmount;
            cameraShack.shackDurashion = cameraShackGroundPoundDurashion;
            cameraShack.decreaseFactor = cameraShackJumpDecreaseFactor;
            cameraShack.Shack = true;

        }

        //ground pound air stun
        if(groundPound && timer.time > groundPoundAirStunTime && !PlayerGroundCollider.stateOftrigger )
        {
            timer.ResetTimer();
            airStunTimeUpGroundPound = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(0, -1 * groundPoundForce);

        }
        else if(!airStunTimeUpGroundPound)
        {
            
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            
            
            
        }

        //ground pound ground stun time
        if (groundPound && timer.time > groundPoundGroundStunTime && PlayerGroundCollider.stateOftrigger)
        {
            groundPound = false;
            timer.StopTimer();
        }
    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        if (!groundPound)
        {
            rb.velocity = new Vector2(speed * x, rb.velocity.y);
        }
        
    }
}
