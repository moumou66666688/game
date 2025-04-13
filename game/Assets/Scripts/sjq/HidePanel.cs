using UnityEngine;

public class HidePanel : MonoBehaviour
{
    public GameObject panel;  // ÐèÒªÒþ²ØµÄ Panel

    public void Hide()
    {
        panel.SetActive(false);
    }
}
