using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    //public ScriptableItem scriptableItem;
    public GameObject statsCanvas;

    [Header("Stats display")]
    public TextMeshProUGUI nameStat;
    public TextMeshProUGUI valueStat;
    public TextMeshProUGUI damageStat;
    public TextMeshProUGUI durabilityStat;
    public TextMeshProUGUI attributesStat;

    [Header("Button prompts display")]
    public GameObject leftMousePrompt;
    public GameObject rightMousePrompt;

    [Header("Stats variables")]
    public ScriptableItem item;
    public string itemName;
    public float value;
    public int damage;
    public int durability;
    public List<ScriptableAttribute> attributes = new List<ScriptableAttribute>();
    public Sprite sprite;

    // Start is called before the first frame update
    private void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>().sprite;
        if (statsCanvas != null)
        {
            statsCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("canvas reference not correctly set for showing stats on this item");
        }
        updateDisplayStats();

        leftMousePrompt.SetActive(false);
        rightMousePrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            if (collision.gameObject.GetComponent<PlayerController>().closestItem == this.gameObject)
            {
                if (collision.gameObject.GetComponent<PlayerController>().holdingItemLeft == false)
                {
                    leftMousePrompt.SetActive(true);
                }
                if (collision.gameObject.GetComponent<PlayerController>().holdingItemRight == false)
                {
                    rightMousePrompt.SetActive(true);
                }
            }
            else
            {
                leftMousePrompt.SetActive(false);
                rightMousePrompt.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            leftMousePrompt.SetActive(false);
            rightMousePrompt.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        if (statsCanvas != null)
        {
            statsCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("canvas reference not correctly set for showing stats on this item");
        }
        
    }
    private void OnMouseExit()
    {
        if (statsCanvas != null)
        {
            statsCanvas.SetActive(false);
        }
    }


    public void pickupItem()
    {
        Destroy(gameObject);
    }


    public void initializeStatsFromPlayer(ScriptableItem Item, string ItemName, float Value, int Damage, int Durability, /*string Attribute1, string Attribute2*/List<ScriptableAttribute> Attributes, Sprite Sprite)
    {
        item = Item;
        itemName = ItemName;
        value = Value;
        damage = Damage;
        durability = Durability;
        attributes = Attributes;
        sprite = Sprite;
        updateDisplayStats();
    }

    public void updateDisplayStats()
    {

        nameStat.text = itemName;
        valueStat.text = "Value: " + value.ToString();
        damageStat.text = "damage: " + damage.ToString();
        durabilityStat.text = "durability: " + durability.ToString();
        string attributesString = "";
        foreach (ScriptableAttribute attribute in attributes)
        {
            attributesString += attribute.attributeName + ", ";
        }
        if (attributesString.Length <= 0)
        {
            attributesString = "No attributes";
        }
        attributesStat.text = "Attributes: " + attributesString;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        name = itemName;
    }
}
