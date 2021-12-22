using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterControllerMovement : MonoBehaviour
{
    public static Vector3 playerPos;

    public GameObject objectToAdd;
    public List<Item> itemList1 = new List<Item>();
    public List<Item> itemList2 = new List<Item>();
    public List<Item> craftingRecipes = new List<Item>();

    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector3 PlayerMouseInput;
    private float xRot;

    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float Gravity = -9.81f;

    public GameObject meca1;
    public GameObject Linterna;
    public Rigidbody ThrowrableLight;

    public Transform ThowrableObject;
    public Transform objectSpawn;

    public GameObject BluePrintMenu;
    public GameObject GameMenu;

    public GameObject CircuitsIcon;
    public GameObject GearIcon;
    public GameObject RustyIcon;
    public GameObject LightIcon;
    public GameObject ResourcesText;
    public GameObject CircuitsText;
    public GameObject GearText;
    public GameObject JunkText;
    public GameObject BuildBasicLanternText;
    public GameObject BuildThrowrableLightText;
    public GameObject UsedDrinkText;
    public GameObject NoEnoughLightText;

    public float CantidadCircuitos;
    public float CantidadEngranajes;
    public float CantidadChatarra;
    public float ThrowrableLightCount;

    public bool Mecha1;
    public bool LinternaActiva;
    public bool ThrowrableLightActive;
    public bool BebidaEnergetica;
    public bool CanBuildMech1;
    public bool accessKey1;
    public bool accessKey2;
    public bool accessKey3;

    public float delay = 5f;
    public float theText = 2f;
    public float buildTime = 3f;
    public float impulse = 20f;

    public Transform mech;
    public GameObject mechCam;

    public bool InventoryOpen => inventoryOpen;
    private bool inventoryOpen = false;

    public bool GameMenuOpen => gameMenuOpen;
    private bool gameMenuOpen = false;

    public AudioSource audioSource;
    public AudioClip buildSFX;

    public GameObject HiddenDoor;

    public int currentHealth;
    public int maxHealth = 100;

    //public GameObject CircuitosCantidad;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrackPlayer());

        audioSource = GetComponent<AudioSource>();
        ResourcesText.SetActive(false);
        CircuitsText.SetActive(false);
        GearText.SetActive(false);
        JunkText.SetActive(false);
        BuildBasicLanternText.SetActive(false);
        BuildThrowrableLightText.SetActive(false);
        UsedDrinkText.SetActive(false);
        NoEnoughLightText.SetActive(false);

        GameMenu.SetActive(false);

        CantidadCircuitos = 0f;
        CantidadEngranajes = 0f;
        CantidadChatarra = 0f;

        Linterna.SetActive(false);

        accessKey1 = false;
        accessKey2 = false;
        accessKey3 = false;
        CanBuildMech1 = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameMenuOpen)
            {
                CloseGameMenu();
            }
            else
            {
                OpenGameMenu();
            }
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(InventoryOpen)
            {
                CloseBluePrintMenu();
            }
            else
            {
                OpenBluePrintMenu();
            }   
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Speed = 25f;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            Speed = 10f;
        }

        CircuitsIcon.GetComponent<Text>().text = "" + CantidadCircuitos;
        GearIcon.GetComponent<Text>().text = "" + CantidadEngranajes;
        RustyIcon.GetComponent<Text>().text = "" + CantidadChatarra;
        LightIcon.GetComponent<Text>().text = "" + ThrowrableLightCount;

        if(Input.GetMouseButtonDown(0) && ThrowrableLightCount >= 1)
        {
            Rigidbody shoot = (Rigidbody)Instantiate(ThrowrableLight, ThowrableObject.transform.position + transform.forward, transform.rotation);
            shoot.AddForce(transform.forward * impulse, ForceMode.Impulse);
            ThrowrableLightCount -= 1f;
        }

        if (Input.GetMouseButtonDown(0) && ThrowrableLightCount <= 0)
        {
            NoEnoughLightText.SetActive(true);
            BuildBasicLanternText.SetActive(false);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            ResourcesText.SetActive(true);
            StartCoroutine(TextAppear(theText));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)&& Mecha1 == true)
        {
            Speed = 0f;
            audioSource.clip = buildSFX;
            audioSource.Play();
            StartCoroutine(BuildMecha1(buildTime));
            CanBuildMech1 = false;
            CantidadCircuitos -= 4f;
            CantidadEngranajes -= 3f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Mecha1 == false)
        {
            ResourcesText.SetActive(true);
            StartCoroutine(TextAppear(theText));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && LinternaActiva == true)
        {
            BuildBasicLanternText.SetActive(true);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            StartCoroutine(TextAppear(theText));
            SpawnLinterna();
            CantidadCircuitos -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && LinternaActiva == false)
        {
            ResourcesText.SetActive(true);
            BuildBasicLanternText.SetActive(false);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            StartCoroutine(TextAppear(theText));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && BebidaEnergetica == true)
        {
            UsedDrinkText.SetActive(true);
            BuildBasicLanternText.SetActive(false);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            StartCoroutine(TextAppear(theText));
            IncreaseSpeedMovement();
            CantidadChatarra -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && BebidaEnergetica == false)
        {
            BuildBasicLanternText.SetActive(false);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            ResourcesText.SetActive(true);
            StartCoroutine(TextAppear(theText));
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && ThrowrableLightActive == true)
        {
            BuildBasicLanternText.SetActive(false);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            BuildThrowrableLightText.SetActive(true);
            StartCoroutine(TextAppear(theText));
            ThorwTheLight();        
            CantidadCircuitos -= 2f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && ThrowrableLightActive == false)
        {
            BuildBasicLanternText.SetActive(false);
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            ResourcesText.SetActive(true);
            StartCoroutine(TextAppear(theText));
        }

        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();

        if (CantidadCircuitos >= 4f && CantidadEngranajes >= 3f && CanBuildMech1 == true)
        {
            Mecha1 = true;
        }

        if(CantidadCircuitos <= 3f && CantidadEngranajes <= 2f)
        {
            Mecha1 = false;
        }

        if(CantidadChatarra >=1)
        {
            BebidaEnergetica = true;
        }

        if (CantidadChatarra <= 0)
        {
            BebidaEnergetica = false;
        }

        if(CantidadCircuitos >= 1)
        {
            LinternaActiva = true;
        }

        if(CantidadCircuitos <= 0)
        {
            LinternaActiva = false;
        }

        if(CantidadCircuitos >= 2)
        {
            ThrowrableLightActive = true;
        }

        if (CantidadCircuitos <= 1)
        {
            ThrowrableLightActive = false;
        }

        if(accessKey1 = true && accessKey2 == true && accessKey3 == true)
        {
            Destroy(HiddenDoor.gameObject);
        }

    }
    private void OpenBluePrintMenu()
    {
        inventoryOpen = true;
        BluePrintMenu.SetActive(true);  
    }
    private void CloseBluePrintMenu()
    {
        inventoryOpen = false;
        BluePrintMenu.SetActive(false);      
    }
    private void OpenGameMenu()
    {
        gameMenuOpen = true;
        GameMenu.SetActive(true);
    }
    private void CloseGameMenu()
    {
        gameMenuOpen = false;
        GameMenu.SetActive(false);
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        //y= gravity * -2f * Time.deltaTime;

        if(Controller.isGrounded)
        {
            Velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = JumpForce;
            }
        }
        else
        {
            Velocity.y -= Gravity * -2f * Time.deltaTime;
        }
        Controller.Move(MoveVector * Speed * Time.deltaTime);
        Controller.Move(Velocity * Time.deltaTime);

    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Circuito")
        {
            CircuitsText.SetActive(true);
            GearText.SetActive(false);
            JunkText.SetActive(false);
            StartCoroutine(TextAppear(theText));
            AddCircuits();
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Engranaje")
        {
            CircuitsText.SetActive(false);
            GearText.SetActive(true);
            JunkText.SetActive(false);
            StartCoroutine(TextAppear(theText));
            AddGear();
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Chatarra")
        {
            CircuitsText.SetActive(false);
            GearText.SetActive(false);
            JunkText.SetActive(true);
            StartCoroutine(TextAppear(theText));
            AddJunk();
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "LlaveAcceso1")
        {
            accessKey1 = true;
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        GameMenu.SetActive(false);
    }

    public void AddCircuits()
    {
        CantidadCircuitos += 1f;
    }

    public void AddGear()
    {
        CantidadEngranajes += 1f;
    }

    public void AddJunk()
    {
        CantidadChatarra += 1f;
    }

    public void SpawnMeca1()
    {
        Instantiate(meca1, objectSpawn.position, objectSpawn.rotation);
    }

    public void SpawnLinterna()
    {
        Linterna.SetActive(true);
        StartCoroutine(DesactivarLinterna(delay));
    }

    public void ThorwTheLight()
    {
        ThrowrableLightCount += 1;
    }

    public void IncreaseSpeedMovement()
    {
        Speed = 25f;
        StartCoroutine(SpeedIncreased(delay));
    }

    IEnumerator DesactivarLinterna(float delay)
    {
        yield return new WaitForSeconds(delay);
        Linterna.SetActive(false);
    }
    IEnumerator SpeedIncreased(float delay)
    {
        yield return new WaitForSeconds(delay);
        Speed = 10f;
    }

    IEnumerator BuildMecha1(float buildTime)
    {
        mechCam.SetActive(false);
        mech.gameObject.SetActive(true);
        SpawnMeca1();
        yield return new WaitForSeconds(buildTime);
        Speed = 10f;
    }

    IEnumerator TextAppear(float theText)
    {
        yield return new WaitForSeconds(theText);
        CircuitsText.SetActive(false);
        GearText.SetActive(false);
        JunkText.SetActive(false);
        ResourcesText.SetActive(false);
        BuildBasicLanternText.SetActive(false);
        BuildThrowrableLightText.SetActive(false);
        UsedDrinkText.SetActive(false);
        NoEnoughLightText.SetActive(false);
    }

    IEnumerator TrackPlayer()
    {
        while (true)
        {
            playerPos = gameObject.transform.position;
            yield return null;
        }
    }

    IEnumerator CanBuildTheMech1(float buildTime)
    {
        yield return WaitForSeconds(buildTime);
        CanBuildMech1 = true;
    }
}
