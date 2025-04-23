using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct CursorSettings
{
    public Texture2D cursorTexture;
    [Tooltip("箭头光标建议(5,5)，中心点光标建议(16,16)")]
    public Vector2 hotspot;
}

public class FixedCursor : MonoBehaviour
{
    [Header("基础设置")]
    public CursorSettings cursorSettings;

    [Header("高级设置")]
    [Tooltip("建议保持默认关闭")]
    public bool hideSystemCursor = false;

    void Start()
    {
        // 设置自定义光标
        Cursor.SetCursor(cursorSettings.cursorTexture,
                       cursorSettings.hotspot,
                       CursorMode.Auto);

        // 智能处理系统光标
        Cursor.visible = !hideSystemCursor;

        // 自动修复事件系统
        if (!EventSystem.current)
        {
            gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
            Debug.Log("已自动创建输入系统");
        }
    }

    void OnApplicationQuit()
    {
        // 退出时恢复默认光标
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}