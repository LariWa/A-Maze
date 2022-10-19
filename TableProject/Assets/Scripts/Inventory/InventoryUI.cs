using UnityEngine;



public class InventoryUI : MonoBehaviour
{

    public GameObject inventoryUI;
    public Transform itemsParent;
    Inventory inventory;
    public static InventoryUI instance;
    public int index = 0;

    void Start()
    {
        inventory = Inventory.instance;
        instance = this;
    }

    public void addItemToUI(Item item)
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        slots[index].AddItem(inventory.items[index]);
        index++;
    }
    public void ResetColorInSlots()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots)
        {
            slot.ResetColor();
        }
    }
    public void clear()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in slots)
        {
            slot.reset();
        }
    }

}