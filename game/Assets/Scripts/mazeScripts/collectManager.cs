using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;


public class collectManager : MonoBehaviour
{
    //public Image arrowImage;
    public int currentPanelIndex = -1;  // 记录当前显示的面板，-1表示目前没显示任何面板

    [SerializeField] private Button button;
    [SerializeField] private GameObject[] panles;
    

    public static collectManager Instance;

    [SerializeField] public Text fragmentCountText;
    private Dictionary<string, bool> collectedFragments = new Dictionary<string, bool>();
    public bool collecteFinish = false;

    private List<countMember> allFragments = new List<countMember>();

    public bool jitan1 = false;

    // 收集完毕大门生成
    [SerializeField] public GameObject exitDoorPrefab;

    // 添加对TilemapNavigator的引用
    private TilemapNavigator navigator;

    // 添加玩家引用
    private Transform playerTransform;

    //最近的路径
    private List<Vector2Int> lastPath = new List<Vector2Int>();

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
        //arrowImage.gameObject.SetActive(false);
        for (int i = 0; i <panles.Length; i++)
        {
            panles[i].SetActive(false);
        }
        button.onClick.AddListener(ClosePanelAndButton);
        button.gameObject.SetActive(false);
        Debug.Log(button);
        Debug.Log(panles[0]);
        

        // 初始化玩家位置
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("未找到玩家对象！");
        }

        // 初始化TilemapNavigator
        navigator = FindObjectOfType<TilemapNavigator>();
        if (navigator == null)
        {
            Debug.LogError("未找到TilemapNavigator组件！");
        }

        InitializeFragments();
        exitDoorPrefab.SetActive(false);
        Debug.Log("门关闭");
    }
    public void ClosePanelAndButton()
    {
        // 如果 currentPanelIndex 有效，就只关闭这个面板
        if (currentPanelIndex >= 0 && currentPanelIndex < panles.Length)
        {
            panles[currentPanelIndex].SetActive(false);
        }
        // 隐藏按钮
        button.gameObject.SetActive(false);

        // 如果你也想重置 currentPanelIndex:
        currentPanelIndex = -1;
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

        // 统计已收集的碎片数量
        int collected = collectedFragments.Values.Count(v => v);
        fragmentCountText.text = $"{collected}/{allFragments.Count}";
        Debug.Log("展示现在的碎片文本：" + fragmentCountText.text);


        if (collected == 1)
        {
            Debug.Log("开启panel1");
            currentPanelIndex = 0;
            panles[currentPanelIndex].SetActive(true);
            button.gameObject.SetActive(true);
        }
        if (collected == 2)
        {

            currentPanelIndex = 1;
            panles[currentPanelIndex].SetActive(true);
            button.gameObject.SetActive(true);
        }
        if (collected == 3)
        {

            currentPanelIndex = 2;
            panles[currentPanelIndex].SetActive(true);
            button.gameObject.SetActive(true);
        }
        if (collected == 4)
        {

            currentPanelIndex = 3;
            panles[currentPanelIndex].SetActive(true);
            button.gameObject.SetActive(true);
            Debug.Log("运行测试发现路径");
           // arrowImage.gameObject.SetActive(true);
            TestFindPath();

            exitDoorPrefab.SetActive(true);
            collecteFinish = true;  // 标记为收集完毕，启动实时指导

        }
    }
    private void TestFindPath()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform 还没初始化！");
            return;
        }

        // 取玩家当前位置作为起点（网格坐标）
        Vector2Int startPos = new Vector2Int(
            Mathf.RoundToInt(playerTransform.position.x),
            Mathf.RoundToInt(playerTransform.position.y)
        );

        // 让玩家走到的目标点（网格坐标）
        // 这里随便举例 (5,5)
        Vector2Int endPos = new Vector2Int(-9, -6);
        Debug.Log("起点和终点的坐标" + startPos + " " + endPos);

        // AStarMgr.Instance.FindPath 是你自己的AStar逻辑
        List<Vector2Int> path = AStarMgr.Instance.FindPath(startPos, endPos);

        if (path.Count > 0)
        {
            Debug.Log($"寻路成功，点数：{path.Count}");
        }
        else
        {
            Debug.LogWarning("寻路失败或没有路径可走。");
        }

        // 把结果保存一下，供 OnDrawGizmos() 绘制
        lastPath = path;
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

    private void OnDrawGizmos()
    {
        if (lastPath == null || lastPath.Count < 2) return;

        Gizmos.color = Color.blue;

        // 假设网格坐标与世界坐标一致，如有偏移/缩放要改成 tilemap 的转换
        for (int i = 0; i < lastPath.Count - 1; i++)
        {
            Vector3 startWorldPos = new Vector3(lastPath[i].x, lastPath[i].y, 0);
            Vector3 endWorldPos = new Vector3(lastPath[i + 1].x, lastPath[i + 1].y, 0);
            Gizmos.DrawLine(startWorldPos, endWorldPos);
        }
    }
}