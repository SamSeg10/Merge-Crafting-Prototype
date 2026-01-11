using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullshit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GetComponent<HorizontalLayoutGroup>().spacing = 0;
            this.GetComponent<HorizontalLayoutGroup>().spacing = -150;
        }
            
    }
}
