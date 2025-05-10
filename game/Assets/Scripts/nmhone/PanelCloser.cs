using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class PanelCloser : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log("�ر������ʼ�����");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPressRaycast.gameObject == gameObject)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Debug.Log($"�ѹر���壺{name}");
        }
    }
}