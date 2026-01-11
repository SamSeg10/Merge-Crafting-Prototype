using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public ScriptableItem item;
    public string itemName;
    public float value;
    public int damage;
    public int durability;
    public List<ScriptableAttribute> attributes = new List<ScriptableAttribute>();
    public Sprite sprite;

    public void updateHeldItemStats(ScriptableItem Item, string ItemName, float Value, int Damage, int Durability, /*string Attribute1, string Attribute2*/ List<ScriptableAttribute> Attributes, Sprite Sprite)
    {
        item = Item;
        itemName = ItemName;
        value = Value;
        damage = Damage;
        durability = Durability;
        attributes = Attributes;
        sprite = Sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
