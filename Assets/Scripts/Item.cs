using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "item/baseItem")]


public class Item : ScriptableObject
{
    [SerializeField] private string itemType;
    private CharacterControllerMovement characterController;

    public bool CanSpawnMeca1;
    public GameObject meca1;
    new public string name = "Default Item";
    public Sprite icon = null;
    public GameObject prefab;
    //public Transform player;

    void Start()
    {
        CanSpawnMeca1 = false;
        //player = GameObject.FindWithTag("Player");
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerMovement>();
    }
    public virtual void Use()
    {
        Debug.Log("Using" + name);
        
        if(itemType == "Meca1")
        {
            //Instantiate(meca1, player.transform.position, player.transform.rotation);
            //CanSpawnMeca1 = true;
            //SpawnMeca1();
            characterController.SpawnMeca1();
            //CanSpawnMeca1 = true;
            //Instantiate(meca, spawn.position, spawn.rotation);
            //GameObject spawnedItem = Instantiate(meca1, player.transform.position, Quaternion.identity);
        }

        
    }
    
}



