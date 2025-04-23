using UnityEngine;

public class PanelSwitcherMiji : MonoBehaviour
{
    [Tooltip("�����ť��Ҫ��ʾ�� Panel")]
    public GameObject targetPanel;

    public void ShowPanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("δ���� targetPanel��");
        }
    }
}
