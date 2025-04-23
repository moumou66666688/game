using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Rotator : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private int stringIndex;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Debug.Log("扳手初始化完成");
    }

    public void Initialize(int index)
    {
        stringIndex = index;
        if (GameManager.Instance != null && GameManager.Instance.stringDatas.Count > index)
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, GameManager.Instance.stringDatas[index].currentPitch * 45);
            Debug.Log($"扳手{index}初始化角度：{GameManager.Instance.stringDatas[index].currentPitch * 45}度");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - (Vector2)rectTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;

        int newValue = Mathf.RoundToInt(angle / 45f) % 8;
        newValue = Mathf.Clamp(newValue, 0, 7);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateCurrentPitch(stringIndex, newValue);
            rectTransform.rotation = Quaternion.Euler(0, 0, newValue * 45);
            Debug.Log($"旋转角度：{angle} → 值：{newValue}");
        }
    }
}