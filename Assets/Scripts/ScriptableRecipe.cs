using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class ScriptableRecipe : ScriptableObject
{
    public ScriptableItem item1;
    public ScriptableItem item2;
    public GameObject result;
    public bool giveAttributes;
    public bool discovered;
}
