using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // �������������
        {
            SceneManager.LoadScene("Book"); // ������Ϊ "sc1" �ĳ���
        }
    }
}
