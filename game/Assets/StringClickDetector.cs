using UnityEngine;

public class StringClickDetector : MonoBehaviour
{
    [Header("��Ӧ�Ϸ���ǧֽ��")]
    public GameObject targetCrane; // ��ק��Ӧ��ǧֽ�׵�����

    private void OnMouseDown() // �������ʱ����
    {
        if (targetCrane != null)
        {
            // ����ǧֽ����Ծ����
            targetCrane.GetComponent<CraneJump>().Jump();
        }
    }
}