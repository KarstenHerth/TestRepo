using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    // quick reference to the inventory
    Inventory inventory;

    // for getting all the slots
    public Transform itemsParent;

    // for getting all equipment slots
    public Transform equipmentParent;

    // slots of the inventory
    InventorySlot[] slots;

    // Equipment slots
    EquipmentSlot[] equipmentSlots;

    // textbox for the gold
    public Text textGold;

    // reference to the UI for toggling visibility
    public GameObject inventoryUI;


	// Use this for initialization
	void Start () {
        inventory = Inventory.instance;
        inventory.hasChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();

        textGold.text = inventory.gold.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        // toggle visibility of inventory
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }

    }

    void UpdateUI()
    {
        // loop throug all slots and fill in the items or clear the slot if there is no item 
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        // update equipment slots
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            EquipLocation equipSlot = equipmentSlots[i].equipmentSlotType;
            if (inventory.equipment.ContainsKey(equipSlot))
            {
                equipmentSlots[i].AddItem(inventory.equipment[equipSlot]);
            }
            else
            {
                equipmentSlots[i].ClearSlot();
            }
        }

        // update gold
        textGold.text = inventory.gold.ToString();
    }
}
