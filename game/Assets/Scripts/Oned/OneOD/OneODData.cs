using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OneODLine
{
    public string speaker;  // 
    public string content;  //
}

[CreateAssetMenu(fileName = "NewOneODData", menuName = "OneOD/OneOD Data")]
public class OneODData : ScriptableObject
{
    public List<OneODLine> dialogues = new List<OneODLine>();
}
