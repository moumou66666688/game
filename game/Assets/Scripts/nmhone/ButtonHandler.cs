using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ڰ�ť�ϣ��������ʾ targetPanel��
/// ���� stringIndex ���������� Rotator
/// </summary>
[DisallowMultipleComponent]
public class ButtonHandler : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("��Ӧ��������� 0-6")]
    public int stringIndex = 0;

    [Tooltip("Ҫ��ʾ��Ŀ����� (�� CanvasGroup)")]
    public CanvasGroup targetPanel;

    // ���� �������� ���� //
    Button btn;
    Rotator rotator;      // ָ�������Ψһ�� Rotator
    bool wired;        // �Ƿ��Ѱ󶨵��

    /*---------------- �������� ----------------*/
    void Awake()
    {
        // 1. ���� Button����ȱʧ��������
        btn = GetComponent<Button>();
        if (!btn)
        {
            Debug.LogError($"{name} ȱ�� Button �����ButtonHandler ʧЧ");
            enabled = false;
            return;
        }

        // 2. У��Ŀ������Ƿ��ѷ���
        if (!targetPanel)
        {
            Debug.LogError($"{name} δ���� targetPanel��");
            enabled = false;
            return;
        }

        // 3. �������� Rotator������/�����ض����ҵ���
        rotator = targetPanel.GetComponentInChildren<Rotator>(true);
        if (!rotator)
            Debug.LogWarning($"{targetPanel.name} ��δ�ҵ� Rotator�����ֻ����ʾ���");

        // 4. ֻ��һ�μ���
        if (!wired)
        {
            btn.onClick.AddListener(OnButtonClick);
            wired = true;
        }
    }

    /*---------------- ����ص� ----------------*/
    void OnButtonClick()
    {
        // Panel �ɼ�
        targetPanel.alpha = 1f;
        targetPanel.interactable = true;
        targetPanel.blocksRaycasts = true;

        // Panel ���� SetActive(false) ����Ҳȷ������
        if (!targetPanel.gameObject.activeSelf)
            targetPanel.gameObject.SetActive(true);

        // �� Rotator
        rotator?.Initialize(stringIndex);
    }

    /*---------------- ��ѡ��ͳһ���� ----------------*/
    public void HidePanel()
    {
        if (!targetPanel) return;
        targetPanel.alpha = 0f;
        targetPanel.interactable = false;
        targetPanel.blocksRaycasts = false;
    }
}
