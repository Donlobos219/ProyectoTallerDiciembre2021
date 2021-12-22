using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    private CharacterControllerMovement characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CanSpawnMeca1()
    {
        characterController.SpawnMeca1();
    }
}
