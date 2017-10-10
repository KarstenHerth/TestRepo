using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : Interactable {

    public Item item;

    public override void Interact()
    {
        Debug.Log("Picking up " + item.itemName);
        Debug.Log("Description: " + item.description);

        // Add the item to the inventory if possible
        bool wasPicked = Inventory.instance.Add(item);
        if (wasPicked)
        {
            currentAgent.GetComponent<PlayerController>().ResetTarget();
            DestroyObject(this.gameObject);
        }

     
    }

}
