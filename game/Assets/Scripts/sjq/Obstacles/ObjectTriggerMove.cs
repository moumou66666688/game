using UnityEngine;
//Obstacle1
public class ObjectTriggerMove : MonoBehaviour
{
    private float moveDistanceX = 3.0f;  // �����ˮƽ�ƶ�����
    private float moveDistanceY = 3.0f;  // ���ϵĴ�ֱ�ƶ�����

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����Ƿ����������
        if (other.CompareTag("Player"))
        {
            Debug.Log("����봥������ײ����ʼ�����Ϸ��ƶ�");

            // ��ȡ�������� Rigidbody2D ���
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // �޸� Rigidbody2D ���ٶȣ�ʹ���������Ϸ��ƶ�
                rb.velocity = new Vector2(-moveDistanceX, moveDistanceY);  // ����������ƶ�
            }

            // �������������ײ�����ٴ���������
            Destroy(gameObject);  // ���ٴ���������
        }
    }
}
