using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A* 寻路管理器，继承自 BaseManager
/// </summary>
public class AStarMgr : BaseManager<AStarMgr>
{


    /// <summary>
    /// 单次寻路的临时节点数据
    /// </summary>
    private class AStarNode
    {
        public int x;
        public int y;

        /// <summary>
        /// 从起点到此节点的消耗（实际代价）
        /// </summary>
        public float gCost;

        /// <summary>
        /// 从此节点到终点的预估消耗（启发式）
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
        /// 父节点，用于回溯路径
        /// </summary>
        public AStarNode parent;

        public AStarNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // 为了演示，这里手动拖引用，也可用其他方式获取
    [Header("参考的地图导航组件")]
    public TilemapNavigator tilemapNavigator;
    private Tilemap tilemap; // 新增 Tilemap 引用

    // 存储上一次寻路结果
    private List<Vector2Int> lastPath = new List<Vector2Int>();

    public bool showPathGizmos = true;



    private void Start()
    {
        // 初始化 Tilemap 引用
        if (tilemapNavigator != null)
            tilemap = tilemapNavigator.GetComponent<Tilemap>();
    }


    public void TestFindPath(Vector2Int startPos, Vector2Int endPos)
    {
        // 调用实际的FindPath算法
        var path = FindPath(startPos, endPos);

        // 将结果保存下来，供OnDrawGizmos绘制
        lastPath = path;
    }

    /// <summary>
    /// 寻路方法：传入起点坐标 和 终点坐标（网格坐标，例如 (0,0) 到 (5,10)），返回最终的网格坐标列表
    /// </summary>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> resultPath = new List<Vector2Int>();

        // 首先判断：若起点、终点本身不可走，直接返回空
        if (!tilemapNavigator.IsWalkable(start.x, start.y))
        {
            Debug.LogWarning($"AStar：起点({start.x},{start.y})不可走！");
            return resultPath;
        }
        if (!tilemapNavigator.IsWalkable(end.x, end.y))
        {
            Debug.LogWarning($"AStar：终点({end.x},{end.y})不可走！");
            return resultPath;
        }

        // 需要一个开放列表和一个封闭列表
        List<AStarNode> openList = new List<AStarNode>();
        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();

        // 建立“坐标 -> AStarNode”的快速查询，减少频繁new
        Dictionary<Vector2Int, AStarNode> allNodes = new Dictionary<Vector2Int, AStarNode>();

        // 起始节点
        AStarNode startNode = new AStarNode(start.x, start.y);
        startNode.gCost = 0;
        startNode.hCost = GetHeuristic(start.x, start.y, end.x, end.y);

        openList.Add(startNode);
        allNodes[new Vector2Int(start.x, start.y)] = startNode;

        // A* 主循环
        while (openList.Count > 0)
        {
            // 1. 从 openList 中找出 fCost 最小的节点
            AStarNode currentNode = GetLowestFCostNode(openList);

            // 2. 若当前节点就是终点，则回溯并生成最终路径
            if (currentNode.x == end.x && currentNode.y == end.y)
            {
                // 回溯路径
                resultPath = RetracePath(currentNode);
                return resultPath;
            }

            // 3. 将当前节点移出 openList，加入 closedSet
            openList.Remove(currentNode);
            closedSet.Add(currentNode);

            // 4. 遍历当前节点相邻的 8 或 4 个方向（本示例采用 8方向，即“九宫格”忽略中点）
            foreach (Vector2Int neighborPos in GetNeighbors(currentNode.x, currentNode.y))
            {
                // 如果不可走，跳过
                if (!tilemapNavigator.IsWalkable(neighborPos.x, neighborPos.y))
                    continue;

                // 如果已经在关闭列表中，也跳过
                if (IsInClosedSet(neighborPos, closedSet))
                    continue;

                // 计算从当前节点到此邻居节点的 gCost
                float tentativeGCost = currentNode.gCost + GetDistanceCost(currentNode, neighborPos);

                // 如果 neighbor 之前没记录过，就创建；否则更新
                if (!allNodes.TryGetValue(neighborPos, out AStarNode neighborNode))
                {
                    neighborNode = new AStarNode(neighborPos.x, neighborPos.y);
                    allNodes[neighborPos] = neighborNode;
                }

                // 如果该节点不在 openList，就直接加入并更新数据
                if (!openList.Contains(neighborNode))
                {
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = GetHeuristic(neighborPos.x, neighborPos.y, end.x, end.y);
                    neighborNode.parent = currentNode;
                    openList.Add(neighborNode);
                }
                else
                {
                    // 如果在 openList 中，检查一下新的路径是否更好
                    if (tentativeGCost < neighborNode.gCost)
                    {
                        neighborNode.gCost = tentativeGCost;
                        neighborNode.parent = currentNode;
                        // hCost 不变
                    }
                }
            }
        }

        // 如果 openList 耗尽还没找到终点，就说明无路可走
        Debug.LogWarning($"AStar：未能找到从({start.x},{start.y})到({end.x},{end.y})的路径！");
        return resultPath;
    }

    /// <summary>
    /// 获取九宫格邻居（含对角方向）
    /// </summary>
    private List<Vector2Int> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // 可以根据自己需要，剔除斜向是否允许
        // 下方是八方向写法
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // 忽略 (0,0) 自己
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                neighbors.Add(new Vector2Int(nx, ny));
            }
        }

        return neighbors;
    }

    /// <summary>
    /// 根据距离计算 gCost 增量，若要区分斜向和直向，可自定义
    /// </summary>
    private float GetDistanceCost(AStarNode currentNode, Vector2Int neighbor)
    {
        // 例如：直走 cost = 1，斜走 cost = 1.414f
        int dx = Mathf.Abs(neighbor.x - currentNode.x);
        int dy = Mathf.Abs(neighbor.y - currentNode.y);
        if (dx + dy == 1)
        {
            // 水平or垂直
            return 1f;
        }
        else
        {
            // 斜方向
            return 1.414f;
        }
    }

    /// <summary>
    /// 启发式（hCost）也可以直接采用曼哈顿距离/欧几里得距离/切比雪夫距离等
    /// </summary>
    private float GetHeuristic(int x1, int y1, int x2, int y2)
    {
        // 欧几里得距离
        float dx = x2 - x1;
        float dy = y2 - y1;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// 从终点开始，通过 parent 一直回溯到起点；再反转得到最终路径
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
    /// 从列表中获取 fCost 最低的节点
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
    /// 判断某坐标对应的节点是否在 closedSet 中
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
            Debug.LogWarning("Tilemap 未找到！");
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
