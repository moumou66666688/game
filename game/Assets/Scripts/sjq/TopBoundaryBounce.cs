using UnityEngine;

public class TopBoundaryBounce : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null && rb.velocity.y > 0)
        {
            // ��ת�ٶȲ���һ������
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y * 0.8f);
        }
    }
}
