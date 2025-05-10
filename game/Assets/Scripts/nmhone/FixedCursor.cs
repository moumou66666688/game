using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct CursorSettings
{
    public Texture2D cursorTexture;
    [Tooltip("��ͷ��꽨��(5,5)�����ĵ��꽨��(16,16)")]
    public Vector2 hotspot;
}

public class FixedCursor : MonoBehaviour
{
    [Header("��������")]
    public CursorSettings cursorSettings;

    [Header("�߼�����")]
    [Tooltip("���鱣��Ĭ�Ϲر�")]
    public bool hideSystemCursor = false;

    void Start()
    {
        // �����Զ�����
        Cursor.SetCursor(cursorSettings.cursorTexture,
                       cursorSettings.hotspot,
                       CursorMode.Auto);

        // ���ܴ���ϵͳ���
        Cursor.visible = !hideSystemCursor;

        // �Զ��޸��¼�ϵͳ
        if (!EventSystem.current)
        {
            gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
            Debug.Log("���Զ���������ϵͳ");
        }
    }

    void OnApplicationQuit()
    {
        // �˳�ʱ�ָ�Ĭ�Ϲ��
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}