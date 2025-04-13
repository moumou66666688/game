using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThreeDLine
{
    public string speaker;  // 
    public string content;  //
}

[CreateAssetMenu(fileName = "NewThreeDData", menuName = "ThreeD/ThreeD Data")]
public class ThreeDData : ScriptableObject
{
    public List<ThreeDLine> dialogues = new List<ThreeDLine>();
}
