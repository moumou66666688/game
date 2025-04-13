using UnityEngine;

public class ScoringZone : MonoBehaviour
{
    private bool passed = false; // ��ֹ�ظ��÷�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!passed && collision.CompareTag("Player"))
        {
            Debug.Log("�������� ");
            passed = true;
            GameManager.Instance.AddScore(); // �ӷ�

        }

    }
}
