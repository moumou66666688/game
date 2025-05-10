using UnityEngine;
//Obstacle1
public class ObjectTriggerMove : MonoBehaviour
{
    private float moveDistanceX = 3.0f;  // 向左的水平移动距离
    private float moveDistanceY = 3.0f;  // 向上的垂直移动距离

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否是玩家物体
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家与触发器碰撞，开始向左上方移动");

            // 获取玩家物体的 Rigidbody2D 组件
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // 修改 Rigidbody2D 的速度，使物体向左上方移动
                rb.velocity = new Vector2(-moveDistanceX, moveDistanceY);  // 向左和向上移动
            }

            // 触发器与玩家碰撞后销毁触发器物体
            Destroy(gameObject);  // 销毁触发器物体
        }
    }
}
