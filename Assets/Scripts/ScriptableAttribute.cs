using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attribute", menuName = "Attribute")]
public class ScriptableAttribute : ScriptableObject
{
    public string attributeName;
    public float valueAdded;
    public int damageAdded;
    public int durabilityAdded;
    public string adjective;
}
