using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor; // 自定义鼠标图片
    public Vector2 hotSpot = Vector2.zero; // 鼠标点击点的位置
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // 设置鼠标样式
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
    }
}
