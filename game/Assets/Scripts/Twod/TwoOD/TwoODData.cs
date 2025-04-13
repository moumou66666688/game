using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TwoODLine
{
    public string speaker;  // 
    public string content;  //
}

[CreateAssetMenu(fileName = "NewTwoODData", menuName = "TwoOD/TwoOD Data")]
public class TwoODData : ScriptableObject
{
    public List<TwoODLine> dialogues = new List<TwoODLine>();
}
