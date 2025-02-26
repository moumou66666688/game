using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
//collectManager
//countMember
public class collectManager : MonoBehaviour
{
    public static collectManager Instance;

    [SerializeField] public Text fragmentCountText;
    //第x个是否被收集
    private Dictionary<string, bool> collectedFragments = new Dictionary<string, bool>();
    public bool collecteFinish = false;

    private List<countMember> allFragments = new List<countMember>();

    public  bool jitan1=false ;
    // public bool jitan2 =false;

    //收集完毕大门生成
    [SerializeField] public GameObject exitDoorPrefab;
    

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
        exitDoorPrefab.SetActive(false);
    }

    // 初始化所有碎片
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
            Debug.LogWarning("未找到任何碎片对象！");
        }

        UpdateUI();
    }

    // 更新UI
    void UpdateUI()
    {
        if (fragmentCountText == null)
        {
            Debug.LogError("fragmentCountText 未赋值！");
            return;
        }

        if (allFragments == null)
        {
            Debug.LogError("allFragments 未初始化！");
            return;
        }
        
        //统计几个collect了，然后show
        int collected = collectedFragments.Values.Count(v => v);
        fragmentCountText.text = $"{collected}/{allFragments.Count}";
        Debug.Log("展示现在的碎片文本："+fragmentCountText.text);
        if (fragmentCountText.text == "4/4") collecteFinish = true;
        //捡到播放
        //GetComponent<AudioSource>().Play();
        


    }

    // 收集碎片
    public void CollectFragment(string id)
    {
        Debug.Log("收集id:" + id);
        
        
        if (!collectedFragments.ContainsKey(id))
        {
            collectedFragments[id] = true; // 标记为已收集
            Debug.Log(id + "的状态是" + true);
            UpdateUI(); // 更新UI
        }
    }

    // 检查是否已收集
    public bool IsCollected(string id)
    {
        return collectedFragments.ContainsKey(id) && collectedFragments[id];
    }

    // 重置所有状态
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
        
        
    }
  

}
