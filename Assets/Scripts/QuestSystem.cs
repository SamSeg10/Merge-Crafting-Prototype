using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public ScriptableQuest currentQuest;
    public List<ScriptableQuest> easyQuests = new List<ScriptableQuest>();
    public List<ScriptableQuest> mediumQuests = new List<ScriptableQuest>();
    public List<ScriptableQuest> hardQuests = new List<ScriptableQuest>();
    public int score;
    public int questsCompleted;

    private int quest; // Temporary while quest randomisation is disabled

    [Header("Interface/retroaction")]
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questHint;
    public ParticleSystem winParticle;

    [Header("Sound effects")]
    private AudioSource audioSource;
    public AudioClip questCompleteSFX;
    public AudioClip destroyItemSFX;

    // Start is called before the first frame update
    void Start()
    {
        quest = 0; // Temporary while quest randomisation is disabled
        audioSource = GetComponent<AudioSource>();
        questsCompleted = 0;
        score = 0;
        ChooseQuest();
    }

    public void ChooseQuest()
    {
        if (questsCompleted < 3 && easyQuests.Count > 0)
        {
            //int quest = Random.Range(0, easyQuests.Count);
            //currentQuest = easyQuests[quest];
            //easyQuests.RemoveAt(quest);
            currentQuest = easyQuests[0];
            easyQuests.RemoveAt(0);
            questDescription.text = "Current quest: " + currentQuest.description;
            questHint.text = currentQuest.hint;
        }
        if (questsCompleted >= 3 && questsCompleted < 6 && mediumQuests.Count > 0)
        {
            //int quest = Random.Range(0, mediumQuests.Count);
            //currentQuest = mediumQuests[quest];
            //mediumQuests.RemoveAt(quest);
            currentQuest = mediumQuests[0];
            mediumQuests.RemoveAt(0);
            questDescription.text = "Current quest: " + currentQuest.description;
            questHint.text = currentQuest.hint;
        }
        if (questsCompleted >= 6 && hardQuests.Count > 0)
        {
            //int quest = Random.Range(0, hardQuests.Count);
            //currentQuest = hardQuests[quest];
            //hardQuests.RemoveAt(quest);
            currentQuest = hardQuests[0];
            hardQuests.RemoveAt(0);
            questDescription.text = "Current quest: " + currentQuest.description;
            questHint.text = currentQuest.hint;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("Verify quest");
            VerifyQuest(collision.gameObject);
        }
    }

    public void VerifyQuest(GameObject item)
    {
        if (currentQuest.id == 1)
        {
            if (item.GetComponent<ItemScript>().item.itemName == "Sword")
            {
                score += 10;
                winParticle.Play();
                Destroy(item, 1);
                questsCompleted++;
                quest++; // Temporary while quest randomisation is disabled

                audioSource.PlayOneShot(questCompleteSFX, 1);
                ChooseQuest();

                
            }
            

        }
        else if (currentQuest.id == 2)
        {
            if (item.GetComponent<ItemScript>().attributes.Count >= 2)
            {
                score += 10;
                winParticle.Play();
                Destroy(item, 1);
                questsCompleted++;
                quest++; // Temporary while quest randomisation is disabled

                audioSource.PlayOneShot(questCompleteSFX, 1);
                ChooseQuest();
            }
        }
        else if (currentQuest.id == 3)
        {
            if (item.GetComponent<ItemScript>().durability >= 12)
            {
                score += 10;
                winParticle.Play();
                Destroy(item, 1);
                questsCompleted++;
                quest++; // Temporary while quest randomisation is disabled

                audioSource.PlayOneShot(questCompleteSFX, 1);
                ChooseQuest();
            }
        }
        else if (currentQuest.id == 4)
        {
            if (item.GetComponent<ItemScript>().item.itemName == "Wood")
            {
                foreach(var attribute in item.GetComponent<ItemScript>().attributes)
                {
                    if (attribute.attributeName == "fire")
                    {
                        score += 20;
                        winParticle.Play();
                        Destroy(item, 1);
                        questsCompleted++;
                        quest++; // Temporary while quest randomisation is disabled

                        audioSource.PlayOneShot(questCompleteSFX, 1);
                        ChooseQuest();
                        break;
                    }
                }
            }
        }
        else if (currentQuest.id == 5)
        {
            foreach (var attribute in item.GetComponent<ItemScript>().attributes)
            {
                if (attribute.attributeName == "magic")
                {
                    score += 20;
                    winParticle.Play();
                    Destroy(item, 1);
                    questsCompleted++;
                    quest++; // Temporary while quest randomisation is disabled

                    audioSource.PlayOneShot(questCompleteSFX, 1);
                    ChooseQuest();
                    break;
                }
            }
        }
        else if (currentQuest.id == 6)
        {
            if (item.GetComponent<ItemScript>().value >= 25)
            {
                score += 20;
                winParticle.Play();
                Destroy(item, 1);
                questsCompleted++;
                quest++; // Temporary while quest randomisation is disabled

                audioSource.PlayOneShot(questCompleteSFX, 1);
                ChooseQuest();
            }
        }
        else if (currentQuest.id == 7)
        {
            if (item.GetComponent<ItemScript>().item.itemName == "Dark Essence")
            {
                score += 30;
                winParticle.Play();
                Destroy(item, 1);
                questsCompleted++;
                quest++; // Temporary while quest randomisation is disabled

                audioSource.PlayOneShot(questCompleteSFX, 1);
                ChooseQuest();
            }
        }
        else if (currentQuest.id == 8)
        {
            if (item.GetComponent<ItemScript>().damage >= 30)
            {
                score += 30;
                winParticle.Play();
                Destroy(item, 1);
                questsCompleted++;
                quest++; // Temporary while quest randomisation is disabled

                audioSource.PlayOneShot(questCompleteSFX, 1);
                ChooseQuest();
            }
        }
        else if (currentQuest.id == 9)
        {
            foreach (var attribute in item.GetComponent<ItemScript>().attributes)
            {
                if (attribute.attributeName == "golden")
                {
                    score += 20;
                    winParticle.Play();
                    Destroy(item, 1);
                    questsCompleted++;
                    quest++; // Temporary while quest randomisation is disabled


                    audioSource.PlayOneShot(questCompleteSFX, 1);
                    ChooseQuest();
                    break;
                }
            }
        }
        if(questsCompleted >= 9)
        {
            currentQuest = null;
            questDescription.text = "Congratulations!";
            questHint.text = "";
        }
    }
}
