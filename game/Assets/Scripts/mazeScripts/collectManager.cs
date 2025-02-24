using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
//collectManager
//countMember
public class collectManager : MonoBehaviour
{
    public static collectManager Instance;

    [SerializeField] private Text fragmentCountText;
    //��x���Ƿ��ռ�
    private Dictionary<string, bool> collectedFragments = new Dictionary<string, bool>();

    private List<countMember> allFragments = new List<countMember>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeFragments();
    }

    // ��ʼ��������Ƭ
    public void InitializeFragments()
    {
        if (allFragments == null)
        {
            allFragments = new List<countMember>();
        }
        allFragments.Clear();

        var fragments = FindObjectsOfType<countMember>();
        if (fragments != null && fragments.Length > 0)
        {
            Debug.Log(fragments);
            allFragments.AddRange(fragments);
        }
        else
        {
            Debug.LogWarning("δ�ҵ��κ���Ƭ����");
        }

        UpdateUI();
    }

    // ����UI
    void UpdateUI()
    {
        if (fragmentCountText == null)
        {
            Debug.LogError("fragmentCountText δ��ֵ��");
            return;
        }

        if (allFragments == null)
        {
            Debug.LogError("allFragments δ��ʼ����");
            return;
        }
        
        //ͳ�Ƽ���collect�ˣ�Ȼ��show
        int collected = collectedFragments.Values.Count(v => v);
        fragmentCountText.text = $"{collected}/{allFragments.Count}";
        Debug.Log("չʾ���ڵ���Ƭ�ı���"+fragmentCountText.text);//
        //�񵽲���
        //GetComponent<AudioSource>().Play();
        
    }

    // �ռ���Ƭ
    public void CollectFragment(string id)
    {
        Debug.Log("�ռ�id:" + id);
        
        
        if (!collectedFragments.ContainsKey(id))
        {
            collectedFragments[id] = true; // ���Ϊ���ռ�
            Debug.Log(id + "��״̬��" + true);
            UpdateUI(); // ����UI
        }
    }

    // ����Ƿ����ռ�
    public bool IsCollected(string id)
    {
        return collectedFragments.ContainsKey(id) && collectedFragments[id];
    }

    // ��������״̬
    public void ResetAll()
    {
        collectedFragments.Clear();
        foreach (var fragment in allFragments)
        {
            fragment.Reset();
        }
        InitializeFragments();
    }
    private void Update()
    {
        //Debug.Log("չʾ��" + fragmentCountText.text);
    }
}
