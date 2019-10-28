using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public Dictionary<string, int> ingredientsCollected;
    public int levelCompleted;
    
    public bool[] unlocked = {true, true, true, false, false, false, false, false, false, false };
}

