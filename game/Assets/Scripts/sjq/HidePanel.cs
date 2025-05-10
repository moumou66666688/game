using UnityEngine;
//Òþ²Øpanel
public class HidePanel : MonoBehaviour
{
    public GameObject panel;  

    public void Hide()
    {
        panel.SetActive(false);
    }
}
