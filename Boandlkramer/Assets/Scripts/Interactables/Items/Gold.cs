using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Pickup {

    // Amount of gold
    public int amount = 1;

    public override void Interact()
    {
        Debug.Log("Amount of Gold collected: " + amount);

        // add gold to the inventory
        Inventory.instance.AddGold(amount);

        currentAgent.GetComponent<PlayerController>().ResetTarget();
        DestroyObject(this.gameObject);
    }
    

}
