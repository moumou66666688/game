using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class StringRippleController : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    public class RippleSettings
    {
        public Color untunedColor = new Color(0, 0, 0, 0.8f);  // 未调音颜色
        public Color tunedColor = new Color(1, 0.84f, 0, 0.8f); // 已调音金色
        public float duration = 0.8f;       // 特效持续时间
        public float maxSize = 25f;         // 最大波纹尺寸
        public float fadeSpeed = 2f;        // 消退速度
    }

    public RippleSettings settings = new RippleSettings();

    private int stringIndex;

    void Start()
    {
        // 自动解析琴弦编号
        string[] nameParts = gameObject.name.Split('_');
        if (nameParts.Length >= 2 && int.TryParse(nameParts[1], out int index))
        {
            stringIndex = index;
        }
        else
        {
            Debug.LogError($"琴弦命名格式错误：{gameObject.name}");
            enabled = false;
        }

        // 自动配置碰撞体
        if (!TryGetComponent<BoxCollider2D>(out _))
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            RectTransform rt = GetComponent<RectTransform>();
            col.size = new Vector2(rt.rect.width, rt.rect.height);
            col.offset = rt.rect.center;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsClickValid(eventData)) return;

        Vector2 localPos = GetClickPosition(eventData);
        CreateRippleEffect(localPos);
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

    void CreateRippleEffect(Vector2 position)
    {
        GameObject rippleObj = new GameObject("RippleEffect");
        ConfigureRippleObject(rippleObj, position);
    }

    void ConfigureRippleObject(GameObject obj, Vector2 pos)
    {
        try
        {
            // 设置父对象和位置
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = pos;

            // 添加并配置粒子系统
            ParticleSystem ps = obj.AddComponent<ParticleSystem>();
            ConfigureParticleSystem(ps);

            // 启动生命周期管理
            StartCoroutine(RippleLifecycle(ps));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"创建波纹失败：{e.Message}");
            Destroy(obj);
        }
    }

    void ConfigureParticleSystem(ParticleSystem ps)
    {
        if (ps == null) return;

        // 停止并清空系统
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // 主模块配置
        ParticleSystem.MainModule main = ps.main;
        main.loop = false;
        main.startLifetime = settings.duration;
        main.startSpeed = 0f;
        main.startSize = settings.maxSize;
        main.startColor = GetCurrentColor();

        // 发射模块
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 1) });

        // 形状模块
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.01f;

        // 渲染器配置
        ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
        if (renderer != null)
        {
            renderer.material = CreateDynamicMaterial();
            renderer.sortingOrder = 1000;
        }
        else
        {
            Debug.LogError("粒子渲染器缺失");
        }

        // 启动系统
        ps.Play();
    }

    Material CreateDynamicMaterial()
    {
        Material baseMat = Resources.Load<Material>("RippleBaseMat");
        if (baseMat == null)
        {
            Debug.LogError("基础材质未找到，使用默认材质");
            return new Material(Shader.Find("Sprites/Default"));
        }

        Material mat = new Material(baseMat);
        mat.SetColor("_TintColor", GetCurrentColor());
        return mat;
    }

    Color GetCurrentColor()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager未初始化");
            return Color.white;
        }
        return GameManager.Instance.IsStringTuned(stringIndex)
            ? settings.tunedColor
            : settings.untunedColor;
    }

    IEnumerator RippleLifecycle(ParticleSystem ps)
    {
        if (ps == null) yield break;

        // 等待系统启动
        yield return new WaitUntil(() => ps.isPlaying);

        float elapsed = 0f;
        while (elapsed < settings.duration && ps != null)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startSize = Mathf.Lerp(settings.maxSize, 0, elapsed / settings.duration);

            elapsed += Time.deltaTime * settings.fadeSpeed;
            yield return null;
        }

        // 安全销毁
        if (ps != null)
        {
            ps.Stop();
            Destroy(ps.gameObject);
        }
    }
}