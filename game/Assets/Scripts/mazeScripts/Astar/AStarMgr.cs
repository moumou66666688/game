using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A* Ѱ·���������̳��� BaseManager
/// </summary>
public class AStarMgr : BaseManager<AStarMgr>
{


    /// <summary>
    /// ����Ѱ·����ʱ�ڵ�����
    /// </summary>
    private class AStarNode
    {
        public int x;
        public int y;

        /// <summary>
        /// ����㵽�˽ڵ�����ģ�ʵ�ʴ��ۣ�
        /// </summary>
        public float gCost;

        /// <summary>
        /// �Ӵ˽ڵ㵽�յ��Ԥ�����ģ�����ʽ��
        /// </summary>
        public float hCost;

        /// <summary>
        /// fCost = gCost + hCost
        /// </summary>
        public float fCost
        {
            get { return gCost + hCost; }
        }

        /// <summary>
        /// ���ڵ㣬���ڻ���·��
        /// </summary>
        public AStarNode parent;

        public AStarNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // Ϊ����ʾ�������ֶ������ã�Ҳ����������ʽ��ȡ
    [Header("�ο��ĵ�ͼ�������")]
    public TilemapNavigator tilemapNavigator;
    private Tilemap tilemap; // ���� Tilemap ����

    // �洢��һ��Ѱ·���
    private List<Vector2Int> lastPath = new List<Vector2Int>();

    public bool showPathGizmos = true;



    private void Start()
    {
        // ��ʼ�� Tilemap ����
        if (tilemapNavigator != null)
            tilemap = tilemapNavigator.GetComponent<Tilemap>();
    }


    public void TestFindPath(Vector2Int startPos, Vector2Int endPos)
    {
        // ����ʵ�ʵ�FindPath�㷨
        var path = FindPath(startPos, endPos);

        // �����������������OnDrawGizmos����
        lastPath = path;
    }

    /// <summary>
    /// Ѱ·����������������� �� �յ����꣨�������꣬���� (0,0) �� (5,10)�����������յ����������б�
    /// </summary>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> resultPath = new List<Vector2Int>();

        // �����жϣ�����㡢�յ㱾�����ߣ�ֱ�ӷ��ؿ�
        if (!tilemapNavigator.IsWalkable(start.x, start.y))
        {
            Debug.LogWarning($"AStar�����({start.x},{start.y})�����ߣ�");
            return resultPath;
        }
        if (!tilemapNavigator.IsWalkable(end.x, end.y))
        {
            Debug.LogWarning($"AStar���յ�({end.x},{end.y})�����ߣ�");
            return resultPath;
        }

        // ��Ҫһ�������б��һ������б�
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

        // ���������� -> AStarNode���Ŀ��ٲ�ѯ������Ƶ��new
        Dictionary<Vector2Int, AStarNode> allNodes = new Dictionary<Vector2Int, AStarNode>();

        // ��ʼ�ڵ�
        AStarNode startNode = new AStarNode(start.x, start.y);
        startNode.gCost = 0;
        startNode.hCost = GetHeuristic(start.x, start.y, end.x, end.y);

        openList.Add(startNode);
        allNodes[new Vector2Int(start.x, start.y)] = startNode;

        // A* ��ѭ��
        while (openList.Count > 0)
        {
            // 1. �� openList ���ҳ� fCost ��С�Ľڵ�
            AStarNode currentNode = GetLowestFCostNode(openList);

            // 2. ����ǰ�ڵ�����յ㣬����ݲ���������·��
            if (currentNode.x == end.x && currentNode.y == end.y)
            {
                // ����·��
                resultPath = RetracePath(currentNode);
                return resultPath;
            }

            // 3. ����ǰ�ڵ��Ƴ� openList������ closedSet
            openList.Remove(currentNode);
            closedSet.Add(currentNode);

            // 4. ������ǰ�ڵ����ڵ� 8 �� 4 �����򣨱�ʾ������ 8���򣬼����Ź��񡱺����е㣩
            foreach (Vector2Int neighborPos in GetNeighbors(currentNode.x, currentNode.y))
            {
                // ��������ߣ�����
                if (!tilemapNavigator.IsWalkable(neighborPos.x, neighborPos.y))
                    continue;

                // ����Ѿ��ڹر��б��У�Ҳ����
                if (IsInClosedSet(neighborPos, closedSet))
                    continue;

                // ����ӵ�ǰ�ڵ㵽���ھӽڵ�� gCost
                float tentativeGCost = currentNode.gCost + GetDistanceCost(currentNode, neighborPos);

                // ��� neighbor ֮ǰû��¼�����ʹ������������
                if (!allNodes.TryGetValue(neighborPos, out AStarNode neighborNode))
                {
                    neighborNode = new AStarNode(neighborPos.x, neighborPos.y);
                    allNodes[neighborPos] = neighborNode;
                }

                // ����ýڵ㲻�� openList����ֱ�Ӽ��벢��������
                if (!openList.Contains(neighborNode))
                {
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = GetHeuristic(neighborPos.x, neighborPos.y, end.x, end.y);
                    neighborNode.parent = currentNode;
                    openList.Add(neighborNode);
                }
                else
                {
                    // ����� openList �У����һ���µ�·���Ƿ����
                    if (tentativeGCost < neighborNode.gCost)
                    {
                        neighborNode.gCost = tentativeGCost;
                        neighborNode.parent = currentNode;
                        // hCost ����
                    }
                }
            }
        }

        // ��� openList �ľ���û�ҵ��յ㣬��˵����·����
        Debug.LogWarning($"AStar��δ���ҵ���({start.x},{start.y})��({end.x},{end.y})��·����");
        return resultPath;
    }

    /// <summary>
    /// ��ȡ�Ź����ھӣ����ԽǷ���
    /// </summary>
    private List<Vector2Int> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // ���Ը����Լ���Ҫ���޳�б���Ƿ�����
        // �·��ǰ˷���д��
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // ���� (0,0) �Լ�
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                neighbors.Add(new Vector2Int(nx, ny));
            }
        }

        return neighbors;
    }

    /// <summary>
    /// ���ݾ������ gCost ��������Ҫ����б���ֱ�򣬿��Զ���
    /// </summary>
    private float GetDistanceCost(AStarNode currentNode, Vector2Int neighbor)
    {
        // ���磺ֱ�� cost = 1��б�� cost = 1.414f
        int dx = Mathf.Abs(neighbor.x - currentNode.x);
        int dy = Mathf.Abs(neighbor.y - currentNode.y);
        if (dx + dy == 1)
        {
            // ˮƽor��ֱ
            return 1f;
        }
        else
        {
            // б����
            return 1.414f;
        }
    }

    /// <summary>
    /// ����ʽ��hCost��Ҳ����ֱ�Ӳ��������پ���/ŷ����þ���/�б�ѩ������
    /// </summary>
    private float GetHeuristic(int x1, int y1, int x2, int y2)
    {
        // ŷ����þ���
        float dx = x2 - x1;
        float dy = y2 - y1;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// ���յ㿪ʼ��ͨ�� parent һֱ���ݵ���㣻�ٷ�ת�õ�����·��
    /// </summary>
    private List<Vector2Int> RetracePath(AStarNode endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        AStarNode current = endNode;

        while (current != null)
        {
            path.Add(new Vector2Int(current.x, current.y));
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// ���б��л�ȡ fCost ��͵Ľڵ�
    /// </summary>
    private AStarNode GetLowestFCostNode(List<AStarNode> nodeList)
    {
        AStarNode lowest = nodeList[0];
        for (int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < lowest.fCost)
            {
                lowest = nodeList[i];
            }
        }
        return lowest;
    }

    /// <summary>
    /// �ж�ĳ�����Ӧ�Ľڵ��Ƿ��� closedSet ��
    /// </summary>
    private bool IsInClosedSet(Vector2Int pos, HashSet<AStarNode> closedSet)
    {
        foreach (var node in closedSet)
        {
            if (node.x == pos.x && node.y == pos.y)
                return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        if (!showPathGizmos || lastPath == null || lastPath.Count < 2)
            return;

        Gizmos.color = Color.blue;

        if (tilemap == null && tilemapNavigator != null)
            tilemap = tilemapNavigator.GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogWarning("Tilemap δ�ҵ���");
            return;
        }

        for (int i = 0; i < lastPath.Count - 1; i++)
        {
            Vector3 startWorldPos = tilemap.CellToWorld(new Vector3Int(lastPath[i].x, lastPath[i].y, 0));
            Vector3 endWorldPos = tilemap.CellToWorld(new Vector3Int(lastPath[i + 1].x, lastPath[i + 1].y, 0));
            Gizmos.DrawLine(startWorldPos, endWorldPos);
        }
    }

}
