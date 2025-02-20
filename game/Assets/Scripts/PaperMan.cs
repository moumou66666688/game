using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMan : MonoBehaviour
{
    public float resonanceFrequency; // 纸人的固有频率
    public float frequencyChangeRange = 10f; // 频率变化范围
    public Vector2 targetPosition; // 目标地点
    public float moveDistance = 1f; // 每次跳动的移动距离
    public float completionDistance = 0.1f; // 任务完成的距离阈值
    private Rigidbody2D rb;
    private bool hasJumped = false; // 是否已经跳动

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float waveFrequency = StringController.Instance.GetCurrentFrequency();
        if (Mathf.Abs(waveFrequency - resonanceFrequency) < 0.1f && !hasJumped)
        {
            // 纸人跳动
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            hasJumped = true; // 标记为已跳动

            // 改变纸人的固有频率
            resonanceFrequency += Random.Range(-frequencyChangeRange, frequencyChangeRange);
            Debug.Log("新的固有频率: " + resonanceFrequency);

            // 向目标地点移动
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.position += direction * moveDistance;

            // 检测是否到达目标地点
            if (Vector2.Distance(transform.position, targetPosition) < completionDistance)
            {
                Debug.Log("任务完成！");
            }
        }

        // 重置跳动状态（例如按下空格键）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasJumped = false;
        }
    }
}