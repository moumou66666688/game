using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform), typeof(BoxCollider2D))]
public class StringClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("特效设置")]
    public Color untunedColor = new Color(0, 0, 0, 0.8f); // 未调音颜色
    public Color tunedColor = new Color(1, 0.84f, 0, 0.8f); // 已调音颜色
    public float effectDuration = 0.8f;  // 持续时间
    public float maxSize = 25f;         // 最大尺寸
    public float fadeSpeed = 2f;        // 消退速度

    private int stringIndex; // 琴弦编号

    void Start()
    {
        // 自动获取琴弦索引（命名格式：String_0 ~ String_6）
        string[] parts = name.Split('_');
        if (parts.Length < 2 || !int.TryParse(parts[1], out stringIndex))
        {
            Debug.LogError($"琴弦命名错误：{name}");
            enabled = false;
        }

        // 自动配置碰撞体
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
        // 创建波纹对象
        GameObject rippleObj = new GameObject("RippleEffect");
        rippleObj.transform.SetParent(transform, false);
        rippleObj.transform.localPosition = pos;

        // 配置粒子系统
        ParticleSystem ps = ConfigureParticleSystem(rippleObj);
        if (ps == null) yield break;

        // 生命周期管理
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

        // 停止并清空系统
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // 主模块配置
        var main = ps.main;
        main.loop = false;
        main.startLifetime = effectDuration;
        main.startSpeed = 0f;
        main.startSize = maxSize;
        main.startColor = GetCurrentColor();

        // 发射模块
        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 1) });

        // 形状模块
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.01f;

        // 渲染器配置
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