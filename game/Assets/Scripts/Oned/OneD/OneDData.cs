using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneDLine
{
    public string speaker;  // 
    public string content;  //
}

[CreateAssetMenu(fileName = "NewOneDData", menuName = "OneD/OneD Data")]
public class OneDData : ScriptableObject
{
    public List<OneDLine> dialogues = new List<OneDLine>();
}
