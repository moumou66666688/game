using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;


public class collectManager : MonoBehaviour
{
    //public Image arrowImage;
    public int currentPanelIndex = -1;  // ��¼��ǰ��ʾ����壬-1��ʾĿǰû��ʾ�κ����

    [SerializeField] private Button button;
    [SerializeField] private GameObject[] panles;
    

    public static collectManager Instance;

    [SerializeField] public Text fragmentCountText;
    private Dictionary<string, bool> collectedFragments = new Dictionary<string, bool>();
    public bool collecteFinish = false;

    private List<countMember> allFragments = new List<countMember>();

    public bool jitan1 = false;

    // �ռ���ϴ�������
    [SerializeField] public GameObject exitDoorPrefab;

    // ��Ӷ�TilemapNavigator������
    private TilemapNavigator navigator;

    // ����������
    private Transform playerTransform;

    //�����·��
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
        

        // ��ʼ�����λ��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("δ�ҵ���Ҷ���");
        }

        // ��ʼ��TilemapNavigator
        navigator = FindObjectOfType<TilemapNavigator>();
        if (navigator == null)
        {
            Debug.LogError("δ�ҵ�TilemapNavigator�����");
        }

        InitializeFragments();
        exitDoorPrefab.SetActive(false);
        Debug.Log("�Źر�");
    }
    public void ClosePanelAndButton()
    {
        // ��� currentPanelIndex ��Ч����ֻ�ر�������
        if (currentPanelIndex >= 0 && currentPanelIndex < panles.Length)
        {
            panles[currentPanelIndex].SetActive(false);
        }
        // ���ذ�ť
        button.gameObject.SetActive(false);

        // �����Ҳ������ currentPanelIndex:
        currentPanelIndex = -1;
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

        // ͳ�����ռ�����Ƭ����
        int collected = collectedFragments.Values.Count(v => v);
        fragmentCountText.text = $"{collected}/{allFragments.Count}";
        Debug.Log("չʾ���ڵ���Ƭ�ı���" + fragmentCountText.text);


        if (collected == 1)
        {
            Debug.Log("����panel1");
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
            Debug.Log("���в��Է���·��");
           // arrowImage.gameObject.SetActive(true);
            TestFindPath();

            exitDoorPrefab.SetActive(true);
            collecteFinish = true;  // ���Ϊ�ռ���ϣ�����ʵʱָ��

        }
    }
    private void TestFindPath()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform ��û��ʼ����");
            return;
        }

        // ȡ��ҵ�ǰλ����Ϊ��㣨�������꣩
        Vector2Int startPos = new Vector2Int(
            Mathf.RoundToInt(playerTransform.position.x),
            Mathf.RoundToInt(playerTransform.position.y)
        );

        // ������ߵ���Ŀ��㣨�������꣩
        // ���������� (5,5)
        Vector2Int endPos = new Vector2Int(-9, -6);
        Debug.Log("�����յ������" + startPos + " " + endPos);

        // AStarMgr.Instance.FindPath �����Լ���AStar�߼�
        List<Vector2Int> path = AStarMgr.Instance.FindPath(startPos, endPos);

        if (path.Count > 0)
        {
            Debug.Log($"Ѱ·�ɹ���������{path.Count}");
        }
        else
        {
            Debug.LogWarning("Ѱ·ʧ�ܻ�û��·�����ߡ�");
        }

        // �ѽ������һ�£��� OnDrawGizmos() ����
        lastPath = path;
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

    private void OnDrawGizmos()
    {
        if (lastPath == null || lastPath.Count < 2) return;

        Gizmos.color = Color.blue;

        // ����������������������һ�£�����ƫ��/����Ҫ�ĳ� tilemap ��ת��
        for (int i = 0; i < lastPath.Count - 1; i++)
        {
            Vector3 startWorldPos = new Vector3(lastPath[i].x, lastPath[i].y, 0);
            Vector3 endWorldPos = new Vector3(lastPath[i + 1].x, lastPath[i + 1].y, 0);
            Gizmos.DrawLine(startWorldPos, endWorldPos);
        }
    }
}