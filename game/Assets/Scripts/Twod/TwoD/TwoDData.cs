using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TwoDLine
{
    public string speaker;  // 
    public string content;  //
}

[CreateAssetMenu(fileName = "NewTwoDData", menuName = "TwoD/TwoD Data")]
public class TwoDData : ScriptableObject
{
    public List<TwoDLine> dialogues = new List<TwoDLine>();
}