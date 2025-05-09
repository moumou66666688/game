using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("玩家物体")]
    public Transform playerTransform; // 可以在 Inspector 中手动设置

    private void Update()
    {
        if (playerTransform != null)
        {
            // 只更新空物体的 X 坐标，保持 Y 和 Z 不变
            transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        }
    }
}
