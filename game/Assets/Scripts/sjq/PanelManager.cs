using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject panel1; // ���� panel1 ������

    // ��ʼ��ʱע�ᰴť�¼�
    void Start()
    {
        // �����İ�ť��ͨ�������ȡ�ģ���ѡ��
        // Button button = GetComponent<Button>();
        // button.onClick.AddListener(ShowPanel1);
    }

    // ��ʾ panel1 �ķ���
    public void ShowPanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(true); // ���� panel1
        }
    }
}