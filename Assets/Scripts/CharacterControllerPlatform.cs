using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControllerPlatform : MonoBehaviour
{
    public float gravityScale = 8.0f;
    public static float globalGravity = -9.81f;

    public static Vector3 playerPos;

    public float speed;
    public float jumpForce;

    [Space(15)]
    public float checkDistance;
    public Transform GroundCheck;
    public LayerMask GroundMask;

    [Space(15)]
    public Transform PlayerMesh;

    [Space(15)]
    public bool canJump;
    public bool canMove;

    public bool hasKey = false;
    public bool hasKey2 = false;
    public bool hasKey3 = false;
    public bool opennedDoor = false;
    public bool opennedDoor2 = false;
    public bool opennedDoor3 = false;

    public bool normalJump;
    public bool canUseChargeJump;
    public bool CanDash;
    public bool canDoDash;
    public bool canDoShoot;
    public bool canNormalShoot;
    public bool canUseSuperShoot;

    public float shootDelay = 2;
    public float delay = 5;
    public float miniDashSpeed = 400f;
    public float DashSpeed = 1000f;

    private float jumpPressure;
    private float minJump;
    private float maxJumpPressure;

    private Rigidbody rbody;

    public Transform spawnPoint;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public Transform dashLeft;

    public Rigidbody bullet;
    public Rigidbody SuperBullet;
    public float impulse = 20f;
    public float superImpulse = 70f;
    
    
    //public GameObject emptyObject;

    //public GameObject dashIcon;
    //public GameObject superJumpIcon;
    //public GameObject superShotIcon;
    //public GameObject fastIcon;
    //public GameObject KeyIcon;

    public bool relentizarPersonaje;
    public float relentizar = 2f;

    //static Animator anim;

    //public AudioSource source;
    //public AudioClip[] magicSFX;

    //public Animator doorAnim;

    //public Animator Door1Anim;
    //public Animator Door2Anim;

    private float x;

    // Start is called before the first frame update
    void Start()
    {
        //superShotIcon.SetActive(false);
        //superJumpIcon.SetActive(false);
        //dashIcon.SetActive(false);
        //fastIcon.SetActive(false);
        //KeyIcon.SetActive(false);
        //anim = GetComponent<Animator>();
        relentizarPersonaje = false;
        canNormalShoot = true;
        canDoShoot = true;
        jumpPressure = 0f;
        minJump = 40f;
        maxJumpPressure = 120f;
        rbody = GetComponent<Rigidbody>();
        normalJump = true;
        canUseChargeJump = false;
        canUseSuperShoot = false;
        CanDash = false;
        canDoDash = true;
        StartCoroutine(TrackPlayer());
    }
    public void CanUseChargedJump()
    {
        canUseChargeJump = true;
    }
    void FixedUpdate()
    {
        
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rbody.AddForce(gravity, ForceMode.Acceleration);

        //Cursor.lockState = CursorLockMode.Locked;

        Rigidbody rb = GetComponent<Rigidbody>();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;    
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 MoveDirection = forward * verticalInput + right * horizontalInput;

        rb.velocity = new Vector3(MoveDirection.x * speed, rb.velocity.y, MoveDirection.z * speed);

        if(MoveDirection != new Vector3(0,0,0))
        {
            PlayerMesh.rotation = Quaternion.LookRotation(MoveDirection);
        }

        if (Input.GetKey(KeyCode.W))
        {
            //anim.SetBool("isWalking", true);
        }
        else
        {
            //anim.SetBool("isWalking", false);
        }
            
    }
    public void RelentizarPersonaje()
    {
        speed = 10;
        StartCoroutine(NormalSpeed(5f));
        relentizarPersonaje = false;
    }

    void MoveHorizontal()
    {
        x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(x, 0, 0);
    }
    void Update()
    {
        
        MoveHorizontal();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.gameObject.transform.position = spawnPoint.position;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.gameObject.transform.position = spawnPoint2.position;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.gameObject.transform.position = spawnPoint3.position;
        }

        if (relentizarPersonaje == true)
        {
            RelentizarPersonaje();
        }
        if(canDoShoot == true && canNormalShoot == true)
        {
            if(Input.GetMouseButton(0))
            {
                //Rigidbody shoot = (Rigidbody)Instantiate(bullet, emptyObject.transform.position + transform.forward, transform.rotation);
                //shoot.AddForce(transform.forward * impulse, ForceMode.Impulse);
                StartCoroutine(CanShoot(shootDelay));
                //source.clip = magicSFX[Random.Range(0, magicSFX.Length)];
                //source.Play();
                //anim.SetTrigger("isAttacking");
                canDoShoot = false;                       
            }
        }

        if (canUseSuperShoot == true && canNormalShoot == false)
            if (Input.GetMouseButton(0))
            {
                //Rigidbody shoot = (Rigidbody)Instantiate(SuperBullet, emptyObject.transform.position + transform.forward, transform.rotation);
                //shoot.AddForce(transform.forward * superImpulse, ForceMode.Impulse);
                StartCoroutine(CanShoot(shootDelay));
                canDoShoot = false;
                
            }

                canJump = Physics.CheckSphere(GroundCheck.position, checkDistance, GroundMask);
        
        if(CanDash == true && canDoDash == true)
        {
            if(Input.GetKeyDown(KeyCode.R))
             {
                Rigidbody rb = GetComponent<Rigidbody>();
                StartCoroutine(CanDoDash(delay));
                
                //Rigidbody rb = GetComponent<Rigidbody>();
                //rb.velocity = Vector3.right * DashSpeed;
                this.GetComponent<Rigidbody>().AddForce(transform.forward * DashSpeed, ForceMode.Impulse);
                canDoDash = false;
            }               
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            //this.gameObject.transform.Translate(Vector3.left * miniDashSpeed * Time.deltaTime);
            this.GetComponent<Rigidbody>().AddForce(-transform.right * miniDashSpeed, ForceMode.Impulse);           
            //anim.SetTrigger("isDashLeft");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            this.GetComponent<Rigidbody>().AddForce(transform.right * miniDashSpeed, ForceMode.Impulse);
            //anim.SetTrigger("isDashRight");
            //this.gameObject.transform.Translate(Vector3.right * miniDashSpeed * Time.deltaTime);
        }

        if (canJump && normalJump == true && Input.GetButtonDown("Jump"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.up * jumpForce;
        }

        if(canJump && normalJump == false && canUseChargeJump == true)
        {
            if(Input.GetButton("Jump"))
            {
                if(jumpPressure < maxJumpPressure)
                {
                    jumpPressure += Time.deltaTime*10f;
                }
                else
                {
                    jumpPressure = maxJumpPressure;
                }
            }
            else
            {
                if(jumpPressure > 0f)
                {
                    jumpPressure = jumpPressure + minJump;
                    rbody.velocity = new Vector3(jumpPressure/10, jumpPressure,0f);
                    jumpPressure = 0f;
                    //canUseChargeJump = false;
                }
            }
            
        }
    }

    private void OnDrawGizmoSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.transform.position, checkDistance);
    }

    IEnumerator CanShoot(float shootDelay)
    {
        yield return new WaitForSeconds(shootDelay);
        canDoShoot = true;

    }
    IEnumerator CanDoDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDoDash = true;
    }

    IEnumerator NormalSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);
        speed = 30;
    }

    IEnumerator TrackPlayer()
    {
        while (true)
        {
            playerPos = gameObject.transform.position;
            yield return null;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "TrampaNivel1.1")
        {
            this.gameObject.transform.position = spawnPoint.position;
            PlayerMesh.transform.Rotate(0.0f, -180.0f, 0.0f, Space.World);
        }

        if(collision.gameObject.tag == "TrampaNivel1.2")
        {
            this.gameObject.transform.position = spawnPoint2.position;
            //Quaternion target = Quaternion.Euler(0, -180, 0);
            PlayerMesh.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }

        if (collision.gameObject.tag == "TrampaNivel1.3")
        {
            this.gameObject.transform.position = spawnPoint3.position;
            //Quaternion target = Quaternion.Euler(0, -180, 0);
            PlayerMesh.transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Nivel2")
        {
            SceneManager.LoadScene("Nivel1");
        }

        if (other.gameObject.tag == "Nivel3")
        {
            SceneManager.LoadScene("Nivel2");
        }

        if (other.gameObject.tag == "Final")
        {
            SceneManager.LoadScene("Final");
        }

        if (other.gameObject.tag == "Key")
        {
            hasKey = true;
            //KeyIcon.SetActive(true);
            Destroy(other.gameObject);
            opennedDoor = true;           
        }
        if(other.gameObject.tag == "Key2")
        {
            hasKey2 = true;
            //KeyIcon.SetActive(true);
            Destroy(other.gameObject);
            opennedDoor2 = true;
        }
        if (other.gameObject.tag == "Key3")
        {
            hasKey3 = true;
            //KeyIcon.SetActive(true);
            Destroy(other.gameObject);
            opennedDoor3 = true;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("OpenedDoor") && opennedDoor == true)
        {
            //other.GetComponent<DoorClosed>().SwitchDoor();
        }

        if (other.gameObject.CompareTag("OpenedDoor2") && opennedDoor2 == true)
        {
            //other.GetComponent<DoorClosed>().SwitchDoor2();
        }

        if (other.gameObject.CompareTag("OpenedDoor3") && opennedDoor3 == true)
        {
            //other.GetComponent<DoorClosed>().SwitchDoor3();
        }

        if (other.gameObject.CompareTag("PuertaAcero"))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                //anim.SetTrigger("isActionLever");
                //doorAnim.SetTrigger("isSteelDoorOpen");
            }  
        }

        if (other.gameObject.CompareTag("TheDoorOpen"))
        {
            Destroy(other.gameObject);
        }

    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Nivel2");
    }
}
