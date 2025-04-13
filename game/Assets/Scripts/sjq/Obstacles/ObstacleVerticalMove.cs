using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Obstacle3

public class ObstacleVerticalMove : MonoBehaviour
{
    public float moveDistance = 0.3f;  // 上下移动的最大距离（2厘米）
    public float moveSpeed = 1.0f;  // 移动速度

    private Vector3 startPosition;

    void Start()
    {
        // 记录障碍物的初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 使障碍物在 Y 轴方向上下移动
        float newY = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
}

