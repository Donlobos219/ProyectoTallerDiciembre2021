using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechController : MonoBehaviour
{
    public int MaxHealth = 5000;
    public int currentHealth;
    public HealthBar healthBar;
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector3 PlayerMouseInput;
    private float xRot;

    [SerializeField] private Transform MeshCamera;
    [SerializeField] private CharacterController Controller;
    [Space]
    [SerializeField] private float Speed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float Gravity = -9.81f;

    public GameObject currentHealthText;
    public GameObject MechHealthUI;

    public GameObject explotion;

    public Transform firePoint;
    public Transform firePoint2;
    public Rigidbody missile;
    public Rigidbody missile2;

    public float bulletImpulse;


    [SerializeField] private string mechType;
    public static Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        if(mechType == "Mech1")
        {
            Speed = 12f;
            bulletImpulse = 45f;
        }
        currentHealthText.SetActive(true);
        MechHealthUI.SetActive(true);
        currentHealth = MaxHealth;
        MaxHealth = 5000;
        StartCoroutine(TrackPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        currentHealthText.GetComponent<Text>().text = "" + currentHealth;
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentHealth --;

        MovePlayer();
        MovePlayerCamera();

        if(Input.GetMouseButtonDown(0))
        {
            Rigidbody bullet = (Rigidbody)Instantiate(missile, firePoint.position + transform.forward, firePoint.rotation);
            bullet.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);
            Rigidbody bullet2 = (Rigidbody)Instantiate(missile2, firePoint2.position + transform.forward, firePoint2.rotation);
            bullet2.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);
            Destroy(bullet.gameObject, 5);
            Destroy(bullet2.gameObject, 5);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)&& mechType == "Mech1")
        {
            Speed = 30f;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)&& mechType == "Mech1")
        {
            Speed = 12f;
        }
    }
    void DecreaseHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        //y= gravity * -2f * Time.deltaTime;

        if (Controller.isGrounded)
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

    public void TakeDamage()
    {
        currentHealth -= 500;
    }

    private void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        MeshCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
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
        if(collision.gameObject.tag == "Missile")
        {
            currentHealth -= 300;
            Instantiate(explotion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
