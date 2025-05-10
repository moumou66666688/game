using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform), typeof(BoxCollider2D))]
public class StringClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("��Ч����")]
    public Color untunedColor = new Color(0, 0, 0, 0.8f); // δ������ɫ
    public Color tunedColor = new Color(1, 0.84f, 0, 0.8f); // �ѵ�����ɫ
    public float effectDuration = 0.8f;  // ����ʱ��
    public float maxSize = 25f;         // ���ߴ�
    public float fadeSpeed = 2f;        // �����ٶ�

    private int stringIndex; // ���ұ��

    void Start()
    {
        // �Զ���ȡ����������������ʽ��String_0 ~ String_6��
        string[] parts = name.Split('_');
        if (parts.Length < 2 || !int.TryParse(parts[1], out stringIndex))
        {
            Debug.LogError($"������������{name}");
            enabled = false;
        }

        // �Զ�������ײ��
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        RectTransform rt = GetComponent<RectTransform>();
        col.size = rt.rect.size;
        col.offset = rt.rect.center;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsClickValid(eventData)) return;
        Vector2 clickPos = GetClickPosition(eventData);
        StartCoroutine(CreateRipple(clickPos));
    }

    bool IsClickValid(PointerEventData eventData)
    {
        return eventData.pointerCurrentRaycast.gameObject == gameObject;
    }

    Vector2 GetClickPosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPos
        );
        return localPos;
    }

    IEnumerator CreateRipple(Vector2 pos)
    {
        // �������ƶ���
        GameObject rippleObj = new GameObject("RippleEffect");
        rippleObj.transform.SetParent(transform, false);
        rippleObj.transform.localPosition = pos;

        // ��������ϵͳ
        ParticleSystem ps = ConfigureParticleSystem(rippleObj);
        if (ps == null) yield break;

        // �������ڹ���
        float elapsed = 0f;
        while (elapsed < effectDuration)
        {
            UpdateRipple(ps, elapsed);
            elapsed += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        Destroy(rippleObj);
    }

    ParticleSystem ConfigureParticleSystem(GameObject obj)
    {
        ParticleSystem ps = obj.AddComponent<ParticleSystem>();
        if (ps == null) return null;

        // ֹͣ�����ϵͳ
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // ��ģ������
        var main = ps.main;
        main.loop = false;
        main.startLifetime = effectDuration;
        main.startSpeed = 0f;
        main.startSize = maxSize;
        main.startColor = GetCurrentColor();

        // ����ģ��
        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 1) });

        // ��״ģ��
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.01f;

        // ��Ⱦ������
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = CreateDynamicMaterial();
        renderer.sortingOrder = 1000;

        ps.Play();
        return ps;
    }

    Material CreateDynamicMaterial()
    {
        Material mat = new Material(Shader.Find("Particles/Standard Unlit"));
        mat.color = GetCurrentColor();
        return mat;
    }

    Color GetCurrentColor()
    {
        return GameManager.Instance.IsStringTuned(stringIndex)
            ? tunedColor
            : untunedColor;
    }

    void UpdateRipple(ParticleSystem ps, float elapsed)
    {
        var main = ps.main;
        main.startSize = Mathf.Lerp(maxSize, 0, elapsed / effectDuration);
    }
}