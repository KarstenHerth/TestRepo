using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    public string itemName = "New Item";
    public string description = "Something about this item...";

    // for representation in the inventory
    public Sprite icon = null;


}
