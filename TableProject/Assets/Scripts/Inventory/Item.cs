using UnityEngine;


public class Item : ScriptableObject
{

	public string name;
	public Sprite icon;            

	public virtual void Use()
	{
		
	}

	public void RemoveFromInventory()
	{
		Inventory.instance.Remove(this);
	}

}