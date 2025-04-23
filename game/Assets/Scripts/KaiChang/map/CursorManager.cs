using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D customCursor; // �Զ������ͼƬ
    public Vector2 hotSpot = Vector2.zero; // ��������λ��
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // ���������ʽ
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
    }
}
