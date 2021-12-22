using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrueba : MonoBehaviour
{
    public bool enemy1Defeated;
    public bool enemy2Defeated;
    public bool enemy3Defeated;
    // Start is called before the first frame update
    void Start()
    {
        enemy1Defeated = false;
        enemy2Defeated = false;
        enemy3Defeated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy1Defeated == true && enemy2Defeated == true && enemy3Defeated == true)
        {
            SceneManager.LoadScene("leve3");
        }
    }

    public void Enemy1Defeated()
    {
        enemy1Defeated = true;
    }

    public void Enemy2Defeated()
    {
        enemy1Defeated = true;
    }

    public void Enemy3Defeated()
    {
        enemy1Defeated = true;
    }
}
