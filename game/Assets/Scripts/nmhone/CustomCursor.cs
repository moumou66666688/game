using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursorTexture;  // ��ק�����ʽͼƬ�����ֶ�
    public Vector2 hotSpot = new Vector2(16, 16);  // ����������λ�ã�Ĭ����ͼ������ģ�

    void Start()
    {
        // ���ù��Ϊ�Զ������ʽ
        SetCustomCursor();
    }

    void Update()
    {
        // �����ҪĳЩ����»ָ�Ĭ�Ϲ�꣬�����������Ⲣ�ָ�
        // ���磬����ĳ�������л���Ĭ�Ϲ��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetCursor();
        }
    }

    // �����Զ�����
    public void SetCustomCursor()
    {
        Cursor.SetCursor(customCursorTexture, hotSpot, CursorMode.ForceSoftware);
    }

    // ����ΪĬ�Ϲ��
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
