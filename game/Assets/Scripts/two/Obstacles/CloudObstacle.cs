using UnityEngine;

public class CloudObstacle : MonoBehaviour
{
    public float pushBackDistance = 1.5f; // 碰到后退的距离

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 确保碰撞的是玩家
        {
            if (gameObject.name == "TriggerZone1") // 只处理前方的触发器
            {
                PushPlayerBack(other);
            }
        }
    }

    // 让玩家向左后退一定距离
    private void PushPlayerBack(Collider2D player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(-pushBackDistance, playerRb.velocity.y);
        }
    }
}
