using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class StringRippleController : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    public class RippleSettings
    {
        public Color untunedColor = new Color(0, 0, 0, 0.8f);  // δ������ɫ
        public Color tunedColor = new Color(1, 0.84f, 0, 0.8f); // �ѵ�����ɫ
        public float duration = 0.8f;       // ��Ч����ʱ��
        public float maxSize = 25f;         // ����Ƴߴ�
        public float fadeSpeed = 2f;        // �����ٶ�
    }

    public RippleSettings settings = new RippleSettings();

    private int stringIndex;

    void Start()
    {
        // �Զ��������ұ��
        string[] nameParts = gameObject.name.Split('_');
        if (nameParts.Length >= 2 && int.TryParse(nameParts[1], out int index))
        {
            stringIndex = index;
        }
        else
        {
            Debug.LogError($"����������ʽ����{gameObject.name}");
            enabled = false;
        }

        // �Զ�������ײ��
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
            // ���ø������λ��
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = pos;

            // ��Ӳ���������ϵͳ
            ParticleSystem ps = obj.AddComponent<ParticleSystem>();
            ConfigureParticleSystem(ps);

            // �����������ڹ���
            StartCoroutine(RippleLifecycle(ps));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"��������ʧ�ܣ�{e.Message}");
            Destroy(obj);
        }
    }

    void ConfigureParticleSystem(ParticleSystem ps)
    {
        if (ps == null) return;

        // ֹͣ�����ϵͳ
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // ��ģ������
        ParticleSystem.MainModule main = ps.main;
        main.loop = false;
        main.startLifetime = settings.duration;
        main.startSpeed = 0f;
        main.startSize = settings.maxSize;
        main.startColor = GetCurrentColor();

        // ����ģ��
        ParticleSystem.EmissionModule emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 1) });

        // ��״ģ��
        ParticleSystem.ShapeModule shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.01f;

        // ��Ⱦ������
        ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
        if (renderer != null)
        {
            renderer.material = CreateDynamicMaterial();
            renderer.sortingOrder = 1000;
        }
        else
        {
            Debug.LogError("������Ⱦ��ȱʧ");
        }

        // ����ϵͳ
        ps.Play();
    }

    Material CreateDynamicMaterial()
    {
        Material baseMat = Resources.Load<Material>("RippleBaseMat");
        if (baseMat == null)
        {
            Debug.LogError("��������δ�ҵ���ʹ��Ĭ�ϲ���");
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
            Debug.LogError("GameManagerδ��ʼ��");
            return Color.white;
        }
        return GameManager.Instance.IsStringTuned(stringIndex)
            ? settings.tunedColor
            : settings.untunedColor;
    }

    IEnumerator RippleLifecycle(ParticleSystem ps)
    {
        if (ps == null) yield break;

        // �ȴ�ϵͳ����
        yield return new WaitUntil(() => ps.isPlaying);

        float elapsed = 0f;
        while (elapsed < settings.duration && ps != null)
        {
            ParticleSystem.MainModule main = ps.main;
            main.startSize = Mathf.Lerp(settings.maxSize, 0, elapsed / settings.duration);

            elapsed += Time.deltaTime * settings.fadeSpeed;
            yield return null;
        }

        // ��ȫ����
        if (ps != null)
        {
            ps.Stop();
            Destroy(ps.gameObject);
        }
    }
}