using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class ScriptableQuest : ScriptableObject
{
    public int id;
    public string description;
    public string hint;
    public int difficulty;
}
