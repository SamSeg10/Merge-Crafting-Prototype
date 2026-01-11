using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeScript : MonoBehaviour
{
    public GameObject leftItem;
    public GameObject rightItem;
    private RecipeList recipeList;
    private bool giveAttributes;
    private bool crafted;
    private bool recipeFound;

    public GameObject MergeBarUI;
    public GameObject rightItemUI;
    public GameObject leftItemUI;
    private bool filling;
    public float fillSpeed;

    private GameObject recipeLog;

    [Header("Sound effects")]
    private AudioSource audioSource;
    //public AudioClip mergeSFX;
    public AudioClip mergeCompleteSFX;
    public AudioClip mergeNewItemSFX;
    public AudioClip mergeGiveStatsSFX;
    public AudioClip mergeFailedSFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        recipeList = GameObject.Find("CraftingManager").GetComponent<RecipeList>();
        crafted = false;
        filling = false;
        MergeBarUI.GetComponent<Slider>().value = 0f;
    }

    void Start()
    {
        recipeLog = GameObject.FindGameObjectWithTag("RecipeLog");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && leftItem.GetComponent<HeldItem>().item != null && rightItem.GetComponent<HeldItem>().item != null)
        {
            filling = true;
            audioSource.Play();
        }

        if (Input.GetKey(KeyCode.Space) && leftItem.GetComponent<HeldItem>().item != null && rightItem.GetComponent<HeldItem>().item != null)
        {
            FillMergeBar();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            filling = false;
            MergeBarUI.GetComponent<Slider>().value = 0f;
        }

    }

    private void Merge()
    {
        if (rightItem.GetComponent<HeldItem>().itemName != "" && leftItem.GetComponent<HeldItem>().itemName != "")
        {
            bool attributesGiven = false;
            recipeFound = false;
            giveAttributes = true; // giveAttributes is always true at first, can become false when checking for recipe
            if (crafted == false)
            {
                VerifyCraftingRecipe();
            }
            string attributesAdjectives = "";
            foreach (ScriptableAttribute attribute in leftItem.GetComponent<HeldItem>().attributes)
            {

                if (!rightItem.GetComponent<HeldItem>().attributes.Contains(attribute) && giveAttributes == true)
                {
                    Debug.Log("Give attributes");
                    rightItem.GetComponent<HeldItem>().attributes.Add(attribute);
                    rightItem.GetComponent<HeldItem>().value += attribute.valueAdded;
                    rightItem.GetComponent<HeldItem>().damage += attribute.damageAdded;
                    rightItem.GetComponent<HeldItem>().durability += attribute.durabilityAdded;
                    attributesAdjectives += attribute.adjective + " ";
                    attributesGiven = true;
                }
            }

            if (recipeFound == true || attributesGiven == true)
            {
                this.GetComponent<PlayerController>().ClearLeftItem();
                this.GetComponent<PlayerController>().UpdateUIRight();
                rightItem.GetComponent<HeldItem>().itemName = attributesAdjectives + rightItem.GetComponent<HeldItem>().itemName;
                this.GetComponent<PlayerController>().heldItemUINameR.text = rightItem.GetComponent<HeldItem>().itemName;
                MergeBarUI.SetActive(false);
            }

            // Play sound according to result
            if (recipeFound)
            {
                audioSource.PlayOneShot(mergeNewItemSFX, 0.7f);
                StartCoroutine(Shake(rightItemUI, 0.7f, 5));
            }
            else 
            {
                if (attributesGiven == true)
                {
                    audioSource.PlayOneShot(mergeGiveStatsSFX, 0.7f);
                    StartCoroutine(Shake(rightItemUI, 0.7f, 5));
                }
                else
                {
                    audioSource.PlayOneShot(mergeFailedSFX, 1);
                    StartCoroutine(Shake(MergeBarUI, 0.7f, 5));
                }
            }

            crafted = false;
            
        }
    }

    public void Copy()
    {
        if (rightItem.GetComponent<HeldItem>().itemName != "" && leftItem.GetComponent<HeldItem>().itemName != "")
        {
            leftItem.GetComponent<HeldItem>().item = rightItem.GetComponent<HeldItem>().item;
            leftItem.GetComponent<HeldItem>().itemName = rightItem.GetComponent<HeldItem>().itemName;
            leftItem.GetComponent<HeldItem>().value = rightItem.GetComponent<HeldItem>().value;
            leftItem.GetComponent<HeldItem>().damage = rightItem.GetComponent<HeldItem>().damage;
            leftItem.GetComponent<HeldItem>().durability = rightItem.GetComponent<HeldItem>().durability;
            leftItem.GetComponent<HeldItem>().attributes = rightItem.GetComponent<HeldItem>().attributes;
            leftItem.GetComponent<HeldItem>().sprite = rightItem.GetComponent<HeldItem>().sprite;

            leftItem.GetComponent<SpriteRenderer>().sprite = leftItem.GetComponent<HeldItem>().sprite;
            audioSource.PlayOneShot(mergeNewItemSFX, 0.7f);
            StartCoroutine(Shake(leftItemUI, 0.7f, 5));
            this.GetComponent<PlayerController>().UpdateUILeft();
        }
    }

    public void PhilosopherStone()
    {
        bool attributesGiven = false;
        string attributesAdjectives = "";
        foreach (ScriptableAttribute attribute in leftItem.GetComponent<HeldItem>().attributes)
        {
            if (!rightItem.GetComponent<HeldItem>().attributes.Contains(attribute))
            {
                Debug.Log("Give attributes");
                rightItem.GetComponent<HeldItem>().attributes.Add(attribute);
                rightItem.GetComponent<HeldItem>().value += attribute.valueAdded;
                rightItem.GetComponent<HeldItem>().damage += attribute.damageAdded;
                rightItem.GetComponent<HeldItem>().durability += attribute.durabilityAdded;
                attributesAdjectives += attribute.adjective + " ";
                attributesGiven = true;
            }
        }
        this.GetComponent<PlayerController>().UpdateUIRight();
        rightItem.GetComponent<HeldItem>().itemName = attributesAdjectives + rightItem.GetComponent<HeldItem>().itemName;
        this.GetComponent<PlayerController>().heldItemUINameR.text = rightItem.GetComponent<HeldItem>().itemName;

        if (attributesGiven)
        {
            audioSource.PlayOneShot(mergeGiveStatsSFX, 0.7f);
            StartCoroutine(Shake(rightItemUI, 0.7f, 5));
        }
        else
        {
            audioSource.PlayOneShot(mergeFailedSFX, 1);
        }
    }

    private void VerifyCraftingRecipe()
    {
        foreach (ScriptableRecipe recipe in recipeList.recipes)
        {
            // If recipe is found
            if (leftItem.GetComponent<HeldItem>().item == recipe.item1 && rightItem.GetComponent<HeldItem>().item == recipe.item2)
            {
                Debug.Log("Recipe Found: " + recipe.name);
                rightItem.GetComponent<HeldItem>().item = recipe.result.GetComponent<ItemScript>().item;
                rightItem.GetComponent<HeldItem>().itemName = recipe.result.GetComponent<ItemScript>().itemName;
                rightItem.GetComponent<HeldItem>().value = recipe.result.GetComponent<ItemScript>().value;
                rightItem.GetComponent<HeldItem>().damage = recipe.result.GetComponent<ItemScript>().damage;
                rightItem.GetComponent<HeldItem>().durability = recipe.result.GetComponent<ItemScript>().durability;
                if (recipe.giveAttributes == false)
                {
                    giveAttributes = false;
                }
                rightItem.GetComponent<HeldItem>().attributes = new List<ScriptableAttribute>();
                foreach (ScriptableAttribute attribute in recipe.result.GetComponent<ItemScript>().attributes)
                {
                    rightItem.GetComponent<HeldItem>().attributes.Add(attribute);
                }
                //else
                //{
                //    rightItem.GetComponent<HeldItem>().attributes = recipe.result.GetComponent<ItemScript>().attributes;
                //}
                rightItem.GetComponent<HeldItem>().sprite = recipe.result.GetComponent<SpriteRenderer>().sprite;
                rightItem.GetComponent<SpriteRenderer>().sprite = rightItem.GetComponent<HeldItem>().sprite;

                if (recipe.discovered == false)
                {
                    recipe.discovered = true;
                    //Code pour notification

                    recipeLog.GetComponent<RecipeLog>().newRecipes++;
                    recipeLog.GetComponent<RecipeLog>().ShowNotification();
                    recipeLog.GetComponent<RecipeLog>().ShowNewRecipePrompt();
                }

                recipeFound = true;
                break;
            }
        }
        crafted = true;
        
    }

    private void FillMergeBar()
    {
        if (filling == true)
        {
            MergeBarUI.GetComponent<Slider>().value += Mathf.Lerp(0, 1, fillSpeed * Time.deltaTime);
        }
        
        if (MergeBarUI.GetComponent<Slider>().value >= 1f)
        {
            if (leftItem.GetComponent<HeldItem>().item.itemName == "Copy Stone")
            {
                Copy();
            }
            else if (leftItem.GetComponent<HeldItem>().item.itemName == "Philosopher's Stone")
            {
                PhilosopherStone();
            }
            else
            {
                Merge();
            }

            filling = false;
            
            MergeBarUI.GetComponent<Slider>().value = 0f;
            StartCoroutine(StopMergeAudio());
        }
    }

    public IEnumerator StopMergeAudio()
    {
        if (audioSource.volume > 0)
        {
            audioSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(StopMergeAudio());
        }
        else
        {
            audioSource.Stop();
            audioSource.volume = 1;
            yield return null;
        }
    }

    public IEnumerator Shake(GameObject shakeObject, float duration, float magnitude)
    {
        Vector3 originalPos = shakeObject.GetComponent<Transform>().position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            shakeObject.GetComponent<Transform>().position = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        shakeObject.GetComponent<Transform>().position = originalPos;
    }
}
