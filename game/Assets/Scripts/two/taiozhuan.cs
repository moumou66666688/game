using UnityEngine;
using UnityEngine.SceneManagement; // ���볡�����������ռ�

public class taiozhuan : MonoBehaviour
{
    // ����������ڰ�ť���ʱ������
    public void LoadSceneTwo()
    {
        // ������Ϊ"two"�ĳ���
        SceneManager.LoadScene("two");
    }
}