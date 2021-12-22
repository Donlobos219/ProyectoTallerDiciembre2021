using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    public GameObject objectToAdd;
    public List<Item> itemList = new List<Item>();
    public List<Item> itemList1 = new List<Item>();
    public List<Item> itemList2 = new List<Item>();
    public List<Item> craftingRecipes = new List<Item>();
    // Start is called before the first frame update
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Inventory.instance.AddItem(itemList[Random.Range(0, itemList.Count)]);
            
        }
    }
    public void OnStatItemUse(StatItemType itemType, int amount)
    {
        Debug.Log("Consume " + itemType + " Add Amount " + amount);
    }

    void AddCircuits()
    {
        Inventory.instance.AddItem(itemList1[Random.Range(0, itemList1.Count)]);
    }

    void AddGear()
    {
        Inventory.instance.AddItem(itemList2[Random.Range(0, itemList2.Count)]);
    }
}
