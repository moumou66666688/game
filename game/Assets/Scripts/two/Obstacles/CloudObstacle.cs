using UnityEngine;

public class CloudObstacle : MonoBehaviour
{
    public float pushBackDistance = 1.5f; // �������˵ľ���

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ȷ����ײ�������
        {
            if (gameObject.name == "TriggerZone1") // ֻ����ǰ���Ĵ�����
            {
                PushPlayerBack(other);
            }
        }
    }

    // ������������һ������
    private void PushPlayerBack(Collider2D player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(-pushBackDistance, playerRb.velocity.y);
        }
    }
}
