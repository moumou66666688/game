using UnityEngine;

public class HidePanel : MonoBehaviour
{
    public GameObject panel;  // ��Ҫ���ص� Panel

    public void Hide()
    {
        panel.SetActive(false);
    }
}
