using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptPrueba : MonoBehaviour
{
    [SerializeField] private string enemyType;

    //public bool enemy1Defeated;
    //public bool enemy2Defeated;
    //public bool enemy3Defeated;

    private PlayerPrueba playerPrueba;

    int enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerPrueba = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPrueba>();

        if (enemyType == "Jefe1")
        {
            enemyHealth = 3;
        }

        if (enemyType == "Jefe2")
        {
            enemyHealth = 5;
        }

        if (enemyType == "Jefe3")
        {
            enemyHealth = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyType == "Jefe1")
        {
            if(enemyHealth <= 0)
            {
                playerPrueba.Enemy1Defeated();
            }
        }

        if (enemyType == "Jefe2")
        {
            if (enemyHealth <= 0)
            {
                playerPrueba.Enemy2Defeated();
            }
        }

        if (enemyType == "Jefe3")
        {
            if (enemyHealth <= 0)
            {
                playerPrueba.Enemy3Defeated();
            }
        }
    }

    

    
}
