using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Obstacle3

public class ObstacleVerticalMove : MonoBehaviour
{
    public float moveDistance = 0.3f;  // �����ƶ��������루2���ף�
    public float moveSpeed = 1.0f;  // �ƶ��ٶ�

    private Vector3 startPosition;

    void Start()
    {
        // ��¼�ϰ���ĳ�ʼλ��
        startPosition = transform.position;
    }

    void Update()
    {
        // ʹ�ϰ����� Y �᷽�������ƶ�
        float newY = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
}

