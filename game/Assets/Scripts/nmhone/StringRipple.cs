using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]          // ��������
public class StringRipple : MonoBehaviour
{
    [Header("��������")]
    public GameObject ripplePrefab;       // �� RippleEffect Ԥ��
    public float rippleSpeed = 3f;
    public float maxSize = 2f;

    void Awake()
    {
        /* ���༭ģʽ�����ֶ���� Collider�������ظ���� */
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (!col)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
        }
        else
        {
            col.isTrigger = true;         // ȷ�����ִ�����״̬
        }
    }

    void OnMouseDown()
    {
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        StartCoroutine(CreateRipple(clickPos));
    }

    IEnumerator CreateRipple(Vector2 center)
    {
        GameObject ripple = Instantiate(ripplePrefab, center, Quaternion.identity);
        float currentSize = 0.1f;

        while (currentSize < maxSize)
        {
            currentSize += Time.deltaTime * rippleSpeed;
            ripple.transform.localScale = new Vector3(currentSize, currentSize, 1f);

            // ͸����˥��
            SpriteRenderer sr = ripple.GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = Mathf.Lerp(0.8f, 0f, currentSize / maxSize);
            sr.color = c;

            yield return null;
        }
        Destroy(ripple);
    }
}
