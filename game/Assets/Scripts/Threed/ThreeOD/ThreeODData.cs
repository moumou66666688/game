using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThreeODLine
{
    public string speaker;  // 
    public string content;  //
}

[CreateAssetMenu(fileName = "NewThreeODData", menuName = "ThreeOD/ThreeOD Data")]
public class ThreeODData : ScriptableObject
{
    public List<ThreeODLine> dialogues = new List<ThreeODLine>();
}
