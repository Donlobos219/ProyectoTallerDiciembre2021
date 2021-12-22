using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject explosion;

    private MechController mechController;
    // Start is called before the first frame update
    void Start()
    {
        mechController = GameObject.FindGameObjectWithTag("Mech1").GetComponent<MechController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mech1")
        {
            //mechController.TakeDamage();
            //Instantiate(explosion, transform.position, Quaternion.identity);

            
            Destroy(gameObject);
        }
    }

    public void HurtPlayer()
    {
        //mechController.TakeDamage();
    }
}
