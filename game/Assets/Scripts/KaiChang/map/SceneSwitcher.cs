using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        InitializeButtons();
    }

    void InitializeButtons()
    {
        button1.onClick.AddListener(LoadScene1);
        button2.onClick.AddListener(LoadScene2);
        button3.onClick.AddListener(LoadScene3);
    }

    void LoadScene1()
    {
        SceneManager.LoadScene("OneD");
    }

    void LoadScene2()
    {
        SceneManager.LoadScene("TwoD");
    }

    void LoadScene3()
    {
        SceneManager.LoadScene("ThreeD");
    }
}
