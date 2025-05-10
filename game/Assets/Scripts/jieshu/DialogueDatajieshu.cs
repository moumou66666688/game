using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLinejieshu
{
    public string speaker;  // ��ɫ����
    public string content;  // �Ի�����
}

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialoguejieshu/Dialoguejieshu Data")]
public class DialogueDatajieshu : ScriptableObject
{
    public List<DialogueLinejieshu> dialogues = new List<DialogueLinejieshu>();
}
