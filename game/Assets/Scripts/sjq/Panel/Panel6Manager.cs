using UnityEngine;
using UnityEngine.UI;

public class Panel6Manager : MonoBehaviour
{
    public GameObject panel1; 

   
    void Start()
    {
        
    }

    public void ShowPanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(true); 
        }
    }
}