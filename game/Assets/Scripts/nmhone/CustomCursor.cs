using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursorTexture;  // 拖拽鼠标样式图片到此字段
    public Vector2 hotSpot = new Vector2(16, 16);  // 鼠标光标的热区位置（默认是图标的中心）

    void Start()
    {
        // 设置光标为自定义的样式
        SetCustomCursor();
    }

    void Update()
    {
        // 如果想要某些情况下恢复默认光标，可以在这里检测并恢复
        // 例如，按下某个键来切换回默认光标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetCursor();
        }
    }

    // 设置自定义光标
    public void SetCustomCursor()
    {
        Cursor.SetCursor(customCursorTexture, hotSpot, CursorMode.ForceSoftware);
    }

    // 重置为默认光标
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
