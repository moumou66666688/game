using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("���¿�ʼ��Ϸ���ص���ʼ����");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
