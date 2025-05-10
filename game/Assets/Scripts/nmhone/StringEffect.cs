using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // ǿ��Ҫ��Image���
public class StringEffect : MonoBehaviour
{
    [Header("��������")]
    public Color flashColor = new Color(1, 0.8f, 0, 0.5f);
    public float flashSpeed = 2f;

    private Image stringImage;
    private Color originalColor;
    private bool isFlashing;

    void Awake()
    {
        // ��ȫ��ȡ���
        stringImage = GetComponent<Image>();

        if (stringImage == null)
        {
            Debug.LogError($"�������� {gameObject.name} ȱ��Image�����", this);
            enabled = false; // ���ýű�
            return;
        }

        originalColor = stringImage.color;
        Debug.Log($"���� {name} ��ʼ�����");
    }

    public void StartFlashing()
    {
        if (!isFlashing && gameObject.activeInHierarchy)
        {
            isFlashing = true;
            StartCoroutine(Flashing());
        }
    }

    System.Collections.IEnumerator Flashing()
    {
        while (isFlashing)
        {
            float lerpValue = Mathf.PingPong(Time.time * flashSpeed, 1);
            stringImage.color = Color.Lerp(originalColor, flashColor, lerpValue);
            yield return null;
        }
        stringImage.color = originalColor;
    }

    void OnDisable()
    {
        isFlashing = false;
    }
}