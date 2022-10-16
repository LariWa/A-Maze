using UnityEngine;
using UnityEngine.UI;



public class InventorySlot : MonoBehaviour
{

    public Image icon;
    public Image btn;
    public Color inUse;
    public Color neutral;
    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        UseItem();
    }


    public void ResetColor()
    {
        btn.color = neutral;
    }
    public void UseItem()
    {
        if (item != null)
        {
            InventoryUI.instance.ResetColorInSlots();
            btn.color = inUse;
            BaseClient.instance.SendToServer(new Net_ObjInteraction_MSg(item.objCode, true));
        }
    }
}
