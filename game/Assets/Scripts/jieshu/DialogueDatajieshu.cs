using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLinejieshu
{
    public string speaker;  // 角色名字
    public string content;  // 对话内容
}

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialoguejieshu/Dialoguejieshu Data")]
public class DialogueDatajieshu : ScriptableObject
{
    public List<DialogueLinejieshu> dialogues = new List<DialogueLinejieshu>();
}
