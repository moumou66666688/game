using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // 强制要求Image组件
public class StringEffect : MonoBehaviour
{
    [Header("发光设置")]
    public Color flashColor = new Color(1, 0.8f, 0, 0.5f);
    public float flashSpeed = 2f;

    private Image stringImage;
    private Color originalColor;
    private bool isFlashing;

    void Awake()
    {
        // 安全获取组件
        stringImage = GetComponent<Image>();

        if (stringImage == null)
        {
            Debug.LogError($"琴弦物体 {gameObject.name} 缺少Image组件！", this);
            enabled = false; // 禁用脚本
            return;
        }

        originalColor = stringImage.color;
        Debug.Log($"琴弦 {name} 初始化完成");
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