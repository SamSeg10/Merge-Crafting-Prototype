using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerScript : MonoBehaviour
{
    public GameObject itemToSpawn;
    public float spawnDelay;
    private GameObject[] itemsOfType;
    public int maxItems;
    private float time;
    public TextMeshProUGUI itemText;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        itemText.text = itemToSpawn.GetComponent<ItemScript>().item.name;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > spawnDelay)
        {
            time = 0f;
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        int itemCount = 0;
        itemsOfType = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in itemsOfType)
        {
            if (item.GetComponent<ItemScript>().item.name == itemToSpawn.GetComponent<ItemScript>().item.name)
            {
                itemCount++;
            }
        }
        if (itemCount < maxItems)
        {
            var position = new Vector2(this.transform.position.x, this.transform.position.y);
            Instantiate(itemToSpawn, position + Random.insideUnitCircle, Quaternion.identity);
        }
    }
}
