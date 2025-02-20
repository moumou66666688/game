using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMan : MonoBehaviour
{
    public float resonanceFrequency; // ֽ�˵Ĺ���Ƶ��
    public float frequencyChangeRange = 10f; // Ƶ�ʱ仯��Χ
    public Vector2 targetPosition; // Ŀ��ص�
    public float moveDistance = 1f; // ÿ���������ƶ�����
    public float completionDistance = 0.1f; // ������ɵľ�����ֵ
    private Rigidbody2D rb;
    private bool hasJumped = false; // �Ƿ��Ѿ�����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float waveFrequency = StringController.Instance.GetCurrentFrequency();
        if (Mathf.Abs(waveFrequency - resonanceFrequency) < 0.1f && !hasJumped)
        {
            // ֽ������
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            hasJumped = true; // ���Ϊ������

            // �ı�ֽ�˵Ĺ���Ƶ��
            resonanceFrequency += Random.Range(-frequencyChangeRange, frequencyChangeRange);
            Debug.Log("�µĹ���Ƶ��: " + resonanceFrequency);

            // ��Ŀ��ص��ƶ�
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.position += direction * moveDistance;

            // ����Ƿ񵽴�Ŀ��ص�
            if (Vector2.Distance(transform.position, targetPosition) < completionDistance)
            {
                Debug.Log("������ɣ�");
            }
        }

        // ��������״̬�����簴�¿ո����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasJumped = false;
        }
    }
}