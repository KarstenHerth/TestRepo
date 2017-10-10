using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipLocation { Hands, Chest, Head, Gloves, Boots };


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    // slot on which this item can be equipped
    public EquipLocation equipTo;

    // stats of the Equipment
    public int armor;
    public float damage;

    // bonuses for stats
    public int health = 0;
    public int mana = 0;
    public int strength = 0;
    public int dexterity = 0;
    public int intelligence = 0;
    public int charisma = 0;



}
