using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Dictionary<pickUpObjCode, GameObject> itemsInScene;

    void Awake()
    {
        instance = this;
        itemsInScene = new Dictionary<pickUpObjCode, GameObject>();

    }
    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);
        InventoryUI.instance.addItemToUI(item);
    }
    public void Init()
    {
        var items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            itemsInScene.Add(item.GetComponent<ItemPickup>().item.objCode, item);
        }
    }
    public void OnMsg(pickUpObjCode objCode, bool use)
    {
        var item = itemsInScene[objCode];
        Add(item.GetComponent<ItemPickup>().item);
        item.SetActive(false);
    }

}