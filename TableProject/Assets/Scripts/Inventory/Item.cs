using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory")]
public class Item : ScriptableObject
{

    public pickUpObjCode objCode;
    public Sprite icon;
}