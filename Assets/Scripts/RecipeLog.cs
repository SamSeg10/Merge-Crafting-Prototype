using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeLog : MonoBehaviour
{
    public GameObject recipe;
    public GameObject recipeListUILeft;
    public GameObject recipeListUIRight;
    public GameObject craftingManager;
    public Canvas canvasUI;
    public List<ScriptableRecipe> discoveredRecipes;
    private int discoveredCount;

    public GameObject notification;
    public TextMeshProUGUI notificationAmount;
    public int newRecipes;
    public GameObject recipePrompt;

    [Header("Display info")]
    public GameObject displayCanvas;
    public Image displayImage;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI displayValue;
    public TextMeshProUGUI displayDamage;
    public TextMeshProUGUI displayDurability;
    public TextMeshProUGUI displayAttributes;

    [Header("Sound effects")]
    private AudioSource audioSource;
    public AudioClip openSFX;
    public AudioClip closeSFX;
    public AudioClip newRecipeSFX;

    

    private void Awake()
    {
        canvasUI.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateRecipeLog();
        discoveredCount = 0;
        newRecipes = 0;
        HideNotification();
        recipePrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvasUI.enabled = !canvasUI.enabled;
            if (canvasUI.enabled)
            {
                HideNotification();
                audioSource.PlayOneShot(openSFX);
                UpdateRecipeLog();
            }
            else
            {
                audioSource.PlayOneShot(closeSFX);
            }
            
        }
    }

    public void UpdateRecipeLog()
    {
        foreach (ScriptableRecipe Recipe in craftingManager.GetComponent<RecipeList>().recipes)
        {
            if (Recipe.discovered && !discoveredRecipes.Contains(Recipe))
            {
                
                discoveredRecipes.Add(Recipe);
                if (discoveredCount % 2 == 0) // Decides wether recipe shows on the left or right based on if the amount of recipes is even or odd
                {
                    Debug.Log("Left");
                    GameObject recipeLogItem = Instantiate(recipe, recipeListUILeft.transform);
                    recipeLogItem.GetComponent<RecipeLogEntry>().recipe = Recipe;
                    recipeLogItem.GetComponent<RecipeLogEntry>().SetRecipeInfo();
                }
                else
                {
                    Debug.Log("Right");
                    GameObject recipeLogItem = Instantiate(recipe, recipeListUIRight.transform);
                    recipeLogItem.GetComponent<RecipeLogEntry>().recipe = Recipe;
                    recipeLogItem.GetComponent<RecipeLogEntry>().SetRecipeInfo();
                }
                discoveredCount++;
                Debug.Log(discoveredCount);
            }
        }
    }

    public void ClearRecipeLog()
    {
        foreach (ScriptableRecipe recipe in craftingManager.GetComponent<RecipeList>().recipes)
        {
            recipe.discovered = false;
            foreach(Transform child in recipeListUILeft.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in recipeListUIRight.transform)
            {
                Destroy(child.gameObject);
            }
        }
        discoveredRecipes.Clear();
    }

    public void ShowNotification()
    {
        notificationAmount.text = newRecipes.ToString();
        notification.SetActive(true);
    }

    public void HideNotification()
    {
        newRecipes = 0;
        notification.SetActive(false);
    }



    public void ShowNewRecipePrompt()
    {
        recipePrompt.SetActive(true);
        audioSource.PlayOneShot(newRecipeSFX);
        recipePrompt.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        StartCoroutine(RecipeUpdatePrompt());
    }
    public IEnumerator RecipeUpdatePrompt()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(HideRecipeUpdatePrompt());
    }
    public IEnumerator HideRecipeUpdatePrompt()
    {
        Color color = recipePrompt.gameObject.GetComponent<TextMeshProUGUI>().color;
        for (float alpha = 1f; alpha > 0; alpha -= 0.01f)
        {
            color.a = alpha;
            recipePrompt.gameObject.GetComponent<TextMeshProUGUI>().color = color;
            yield return null;
        }

    }
}
