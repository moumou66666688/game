using UnityEngine;
using UnityEngine.UI;
//��ʾpanel
public class wanfa : MonoBehaviour
{
    public GameObject panel1;

    // ��ʼ��ʱע�ᰴť�¼�
    void Start()
    {

    }

    public void ShowPanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(true); // ���� panel1
        }
    }
}