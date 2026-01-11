using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeLogEntry : MonoBehaviour
{
    public ScriptableRecipe recipe;
    public ScriptableItem leftItem;
    public ScriptableItem rightItem;
    public GameObject resultItem;

    [Header("UI elements")]
    public Image leftItemSprite;
    public TextMeshProUGUI leftName;
    public Image rightItemSprite;
    public TextMeshProUGUI rightName;
    public Image resultItemSprite;
    public TextMeshProUGUI resultName;

    public GameObject notification;

    public RecipeLog parentMenu;

    


    // Start is called before the first frame update
    private void Start()
    {
        leftName.enabled = false;
        rightName.enabled = false;
        resultName.enabled = false;
        parentMenu = GameObject.FindGameObjectWithTag("RecipeLog").GetComponent<RecipeLog>();
    }

    public void SetRecipeInfo()
    {
        leftItem = recipe.item1;
        rightItem = recipe.item2;
        resultItem = recipe.result;

        leftName.text = leftItem.itemName;
        rightName.text = rightItem.itemName;
        resultName.text = resultItem.GetComponent<ItemScript>().item.itemName;

        leftItemSprite.sprite = leftItem.itemSprite;
        rightItemSprite.sprite = rightItem.itemSprite;
        resultItemSprite.sprite = resultItem.GetComponent<SpriteRenderer>().sprite;
    }


    public void ShowItemLeft()
    {
        
        leftName.enabled = true;
    }
    public void HideItemLeft()
    {
        leftName.enabled = false;
    }


    public void ShowItemRight()
    {
        rightName.enabled = true;
    }
    public void HideItemRight()
    {
        rightName.enabled = false;
    }


    public void ShowItemResult()
    {
        resultName.enabled = true;
    }
    public void HideItemResult()
    {
        resultName.enabled = false;
    }

    public void ShowDisplayInfo()
    {
        var result = resultItem.GetComponent<ItemScript>();

        parentMenu.displayImage.sprite = result.item.itemSprite;
        parentMenu.displayName.text = result.itemName;
        parentMenu.displayValue.text = result.value.ToString();
        parentMenu.displayDamage.text = result.damage.ToString();
        parentMenu.displayDurability.text = result.durability.ToString();
        string attributesString = "";
        foreach (var attribute in result.attributes)
        {
            attributesString += attribute.attributeName + ", ";
        }
        if (attributesString.Length == 0)
        {
            attributesString = "None";
        }
        parentMenu.displayAttributes.text = attributesString;

        parentMenu.displayCanvas.SetActive(true);

    }

    public void HideDisplayInfo()
    {
        parentMenu.displayCanvas.SetActive(false);
    }

    public void HideNotification()
    {
        notification.SetActive(false);
    }
}
