using UnityEngine;
using UnityEngine.SceneManagement; // ���볡�����������ռ�

public class taiozhuan : MonoBehaviour
{
    // ����������ڰ�ť���ʱ������
    public void LoadSceneTwo()
    {
        // ������Ϊ"threesjq"�ĳ���
        SceneManager.LoadScene("threesjq");
    }
}