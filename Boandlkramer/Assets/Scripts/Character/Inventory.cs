using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton
    public static Inventory instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one inventory created!");
            return;
        }
        instance = this;
    }

    #endregion

    // for updating the graphical representation of the inventory if the inventory has changed
    public delegate void HasChanged();
    public HasChanged hasChangedCallback;

    // size of the inventory
    public int size = 20;

    // list for the items the player carries
    public List<Item> items = new List<Item>();

    // amount of gold the player owns
    public int gold = 0;

    // for currently equipped items
    public Dictionary<EquipLocation, Equipment> equipment = new Dictionary<EquipLocation, Equipment>();

    // adds an item to the inventory
    public bool Add(Item item)
    {
        // check if there is some room left for the new item
        if (items.Count >= size)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        items.Add(item);

        if (hasChangedCallback != null)
            hasChangedCallback.Invoke();

        return true;
    }

    // removes an item from the inventory
    public void Remove(Item item)
    {
        items.Remove(item);
        if (hasChangedCallback != null)
            hasChangedCallback.Invoke();
    }

    // adds (substracts for negativ sign) gold to the inventory
    public void AddGold(int amount)
    {
        gold += amount;
        if (hasChangedCallback != null)
            hasChangedCallback.Invoke();
    }

    // puts equipment from the inventory to current equipment
    public void Equip(Equipment equip)
    {
        // and remove it from the inventory
        items.Remove((Item)equip);

        // put the new equipment to the slot
        equipment.Add(equip.equipTo, equip);

        // Update UI
        if (hasChangedCallback != null)
            hasChangedCallback.Invoke();
    }


    public void Unequip(Equipment equip)
    {
            if (!Add(equip))
            {
                Debug.Log("Inventory is full!");
                return;
            }
            else
            {
                Debug.Log("Putting " + equip.name + " to the inventory again!");
                equipment.Remove(equip.equipTo);
                // Update UI
                if (hasChangedCallback != null)
                    hasChangedCallback.Invoke();
            }
    }

}
