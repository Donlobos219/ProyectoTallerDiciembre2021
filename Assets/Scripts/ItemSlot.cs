using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    private Item item;
    // Start is called before the first frame update
    //private CharacterControllerMovement characterController;

    void Start()
    {
        //characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerMovement>();
    }
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
            //characterController.SpawnMeca1();
        }


    }

    public void DestroySlot()
    {
        Destroy(gameObject);
    }
}
