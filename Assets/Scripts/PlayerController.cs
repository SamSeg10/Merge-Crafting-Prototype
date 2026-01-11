using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;
    public float movementSpeed;

    [HideInInspector]
    public bool holdingItemLeft;
    [HideInInspector]
    public bool holdingItemRight;
    public GameObject heldItemLeft;
    public GameObject heldItemRight;
    public GameObject heldItemSaved; // For item hand switch
    public GameObject dropItem;
    private ItemScript pickedItem;
    public GameObject closestItem;

    [Header("Left held item UI elements")]
    public GameObject heldItemUIL;
    public TextMeshProUGUI heldItemUINameL;
    public TextMeshProUGUI heldItemUIValueL;
    public TextMeshProUGUI heldItemUIDamageL;
    public TextMeshProUGUI heldItemUIDurabilityL;
    public TextMeshProUGUI heldItemAttributesL;
    public Image heldItemUIImageL;

    [Header("Right held item UI elements")]
    public GameObject heldItemUIR;
    public TextMeshProUGUI heldItemUINameR;
    public TextMeshProUGUI heldItemUIValueR;
    public TextMeshProUGUI heldItemUIDamageR;
    public TextMeshProUGUI heldItemUIDurabilityR;
    public TextMeshProUGUI heldItemAttributesR;
    public Image heldItemUIImageR;

    [Header("Merge bar UI elements")]
    public GameObject mergeBarUI;

    [Header("Sound effects")]
    private AudioSource audioSource;
    public AudioClip itemPickupSFX;
    public AudioClip itemDropSFX;
    public AudioClip itemSwitchSFX;

    // Start is called before the first frame update
    void Start()
    {
        heldItemUIL.SetActive(false);
        heldItemUIR.SetActive(false);
        mergeBarUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        
        //Drop item left hand
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (holdingItemLeft && dropItem != null)
            {
                HeldItem itemInHand = heldItemLeft.GetComponent<HeldItem>();
                Vector2 dropLocation = new Vector2(transform.position.x - 0.3f, transform.position.y + 0.3f);
                GameObject droppedItem = Instantiate(dropItem, dropLocation, transform.rotation);
                droppedItem.GetComponent<ItemScript>().initializeStatsFromPlayer(itemInHand.item, itemInHand.itemName, itemInHand.value, itemInHand.damage, itemInHand.durability, /*itemInHand.attribute1, itemInHand.attribute2*/ itemInHand.attributes, itemInHand.sprite);
                ClearLeftItem();
                mergeBarUI.SetActive(false);
                audioSource.PlayOneShot(itemDropSFX, 1);
            }
        }

        //Drop item right hand
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (holdingItemRight && dropItem != null)
            {
                HeldItem itemInHand = heldItemRight.GetComponent<HeldItem>();
                Vector2 dropLocation = new Vector2(transform.position.x + 0.3f, transform.position.y + 0.3f);
                GameObject droppedItem = Instantiate(dropItem, dropLocation, transform.rotation);
                droppedItem.GetComponent<ItemScript>().initializeStatsFromPlayer(itemInHand.item, itemInHand.itemName, itemInHand.value, itemInHand.damage, itemInHand.durability, /*itemInHand.attribute1, itemInHand.attribute2*/ itemInHand.attributes, itemInHand.sprite);
                ClearRightItem();
                mergeBarUI.SetActive(false);
                audioSource.PlayOneShot(itemDropSFX, 1);
            }
        }

        // Switch item hands
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (holdingItemLeft || holdingItemRight)
            {
                SwitchItemHands();
            }
        }

        heldItemLeft.GetComponent<SpriteRenderer>().enabled = holdingItemLeft;
        heldItemRight.GetComponent<SpriteRenderer>().enabled = holdingItemRight;
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            closestItem = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Pick up item left hand
            if (Input.GetMouseButton(0) && !holdingItemLeft && closestItem != null)
            {
                holdingItemLeft = true;
                //pickedItem = collision.gameObject.GetComponent<ItemScript>();
                pickedItem = closestItem.GetComponent<ItemScript>();
                pickedItem.pickupItem();
                heldItemLeft.GetComponent<HeldItem>().updateHeldItemStats(pickedItem.item, pickedItem.itemName, pickedItem.value, pickedItem.damage, pickedItem.durability, /*pickedItem.attribute1, pickedItem.attribute2*/ pickedItem.attributes, pickedItem.sprite);

                // Update UI to show stats of held item
                UpdateUILeft();

                //Play audio
                audioSource.PlayOneShot(itemPickupSFX, 1);

                // Update UI to show merge bar if holding two items
                if (heldItemLeft.GetComponent<HeldItem>().item != null && heldItemRight.GetComponent<HeldItem>().item != null)
                {
                    mergeBarUI.SetActive(true);
                }
            }

            // Pick up item right hand
            if (Input.GetMouseButton(1) && !holdingItemRight && closestItem != null)
            {
                holdingItemRight = true;
                //pickedItem = collision.gameObject.GetComponent<ItemScript>();
                pickedItem = closestItem.GetComponent<ItemScript>();
                pickedItem.pickupItem();
                heldItemRight.GetComponent<HeldItem>().updateHeldItemStats(pickedItem.item, pickedItem.itemName, pickedItem.value, pickedItem.damage, pickedItem.durability, /*pickedItem.attribute1, pickedItem.attribute2*/ pickedItem.attributes, pickedItem.sprite);

                // Update UI to show stats of held item
                UpdateUIRight();

                //Play audio
                audioSource.PlayOneShot(itemPickupSFX, 1);

                // Update UI to show merge bar if holding two items
                if (heldItemLeft.GetComponent<HeldItem>().item != null && heldItemRight.GetComponent<HeldItem>().item != null)
                {
                    mergeBarUI.SetActive(true);
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            if (closestItem == collision.gameObject)
            {
                closestItem = null;
            }
        }  
    }


    public void UpdateBothUI()
    {
        UpdateUILeft();
        UpdateUIRight();
    }

    public void UpdateUILeft()
    {
        heldItemUIL.SetActive(true);
        heldItemUINameL.text = heldItemLeft.GetComponent<HeldItem>().itemName;
        heldItemUIValueL.text = "Value: " + heldItemLeft.GetComponent<HeldItem>().value.ToString();
        heldItemUIDamageL.text = "Damage: " + heldItemLeft.GetComponent<HeldItem>().damage.ToString();
        heldItemUIDurabilityL.text = "Durability: " + heldItemLeft.GetComponent<HeldItem>().durability.ToString();
        string attributesStringL = "";
        foreach (ScriptableAttribute attribute in heldItemLeft.GetComponent<HeldItem>().attributes)
        {
            attributesStringL += attribute.attributeName + ", ";
        }
        heldItemAttributesL.text = "Attributes: " + attributesStringL;
        heldItemUIImageL.sprite = heldItemLeft.GetComponent<HeldItem>().sprite;
    }

    public void UpdateUIRight()
    {
        heldItemUIR.SetActive(true);
        heldItemUINameR.text = heldItemRight.GetComponent<HeldItem>().itemName;
        heldItemUIValueR.text = "Value: " + heldItemRight.GetComponent<HeldItem>().value.ToString();
        heldItemUIDamageR.text = "Damage: " + heldItemRight.GetComponent<HeldItem>().damage.ToString();
        heldItemUIDurabilityR.text = "Durability: " + heldItemRight.GetComponent<HeldItem>().durability.ToString();
        string attributesStringR = "";
        foreach (ScriptableAttribute attribute in heldItemRight.GetComponent<HeldItem>().attributes)
        {
            attributesStringR += attribute.attributeName + ", ";
        }
        heldItemAttributesR.text = "Attributes: " + attributesStringR;
        heldItemUIImageR.sprite = heldItemRight.GetComponent<HeldItem>().sprite;
    }

    public void ClearBothHands()
    {
        ClearLeftItem();
        ClearRightItem();
    }

    public void ClearLeftItem()
    {
        heldItemLeft.GetComponent<HeldItem>().updateHeldItemStats(null, "", 0, 0, 0, null, null);
        holdingItemLeft = false;
        heldItemUIL.SetActive(false);
    }

    public void ClearRightItem()
    {
        heldItemRight.GetComponent<HeldItem>().updateHeldItemStats(null, "", 0, 0, 0, null, null);
        holdingItemRight = false;
        heldItemUIR.SetActive(false);
    }

    public void SwitchItemHands()
    {
        var leftItem = heldItemLeft.GetComponent<HeldItem>();
        var rightItem = heldItemRight.GetComponent<HeldItem>();
        var savedItem = heldItemSaved.GetComponent<HeldItem>();

        heldItemSaved.GetComponent<HeldItem>().updateHeldItemStats(leftItem.item, leftItem.itemName, leftItem.value, leftItem.damage, leftItem.durability, leftItem.attributes, leftItem.sprite); //Save left item stats
        heldItemLeft.GetComponent<HeldItem>().updateHeldItemStats(rightItem.item, rightItem.itemName, rightItem.value, rightItem.damage, rightItem.durability, rightItem.attributes, rightItem.sprite); //Change left item's stats to right item's
        heldItemRight.GetComponent<HeldItem>().updateHeldItemStats(savedItem.item, savedItem.itemName, savedItem.value, savedItem.damage, savedItem.durability, savedItem.attributes, savedItem.sprite); //Change right item's stats to left item's saved stats

        audioSource.PlayOneShot(itemSwitchSFX, 1);
        if (heldItemLeft.GetComponent<HeldItem>().item == null) // If right hand was empty, move left object to right hand
        {
            holdingItemLeft = false;
            holdingItemRight = true;
            ClearLeftItem();
            UpdateUIRight();
        }
        else if (heldItemRight.GetComponent<HeldItem>().item == null) // If left hand was empty, move right object to left hand
        {
            holdingItemLeft = true;
            holdingItemRight = false;
            ClearRightItem();
            UpdateUILeft();
        }
        else // If both hands were full, simply update UI
        {
            UpdateBothUI();
        }
    }
}
