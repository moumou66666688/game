using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapNavigator : MonoBehaviour
{
    [Header("Tilemaps (Multiple)")]
    [Tooltip("所有可走的Tilemap")]
    public Tilemap[] walkableTilemaps;
    [Tooltip("所有障碍物的Tilemap")]
    public Tilemap[] obstacleTilemaps;

    [Header("Tile Name Settings")]
    public List<string> walkableTileNames = new List<string> { "Grass", "Grass", "Grass", "Stone Ground" };
    public List<string> obstacleTileNames = new List<string> { "Wall", "Wall", "Wall" };

    [Header("Debug Visualization")]
    public bool visualizeGrid = true;
    [Tooltip("Scene视图中画出的立方体大小")]
    public float gizmoCubeSize = 0.8f;
    //
    /// <summary>
    /// 存放所有网格节点。Key为二维坐标(x,y)。
    /// </summary>
    public Dictionary<Vector2Int, GridNode> nodeGrid = new Dictionary<Vector2Int, GridNode>();

    private void Start()
    {
        BuildNavigationGrid();
    }

    /// <summary>
    /// 将多个Tilemap的数据合并，构建网格节点。
    /// </summary>
    public void BuildNavigationGrid()
    {
        nodeGrid.Clear();

        // -------------------------
        // 1. 扫描所有 obstacleTilemaps
        // -------------------------

        Debug.Log("***obstcale长度:" + obstacleTilemaps.Length + "***");
        if (obstacleTilemaps != null && obstacleTilemaps.Length > 0)
        {
            foreach (var obstacleTilemap in obstacleTilemaps)
            {
                if (obstacleTilemap == null) continue;

                BoundsInt bounds = obstacleTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    if (!obstacleTilemap.HasTile(pos)) continue;

                    // 获取Tile并判断是否匹配"obstacleTileNames"
                    TileBase tile = obstacleTilemap.GetTile(pos);
                    if (tile == null) continue;
                    string tileName = tile.name;

                    bool isObstacle = obstacleTileNames.Exists(name => tileName.Contains(name));
                    if (isObstacle)
                    {
                        Vector2Int coord = new Vector2Int(pos.x, pos.y);

                        // 如果还未添加，则创建一个节点并标记为不可行走
                        if (!nodeGrid.ContainsKey(coord))
                        {
                            nodeGrid[coord] = new GridNode(pos.x, pos.y, false);
                        }
                        else
                        {
                            // 覆盖为不可行走
                            nodeGrid[coord].isWalkable = false;
                        }
                    }
                }
            }
        }

        // -------------------------
        // 2. 扫描所有 walkableTilemaps
        // -------------------------
        Debug.Log("***可行tilemap的长度:" + walkableTilemaps.Length + "***");
        if (walkableTilemaps != null && walkableTilemaps.Length > 0)
        {
            foreach (var walkableTilemap in walkableTilemaps)
            {
                if (walkableTilemap == null) continue;

                BoundsInt bounds = walkableTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    if (!walkableTilemap.HasTile(pos)) continue;

                    TileBase tile = walkableTilemap.GetTile(pos);
                    if (tile == null) continue;
                    string tileName = tile.name;

                    bool isWalkable = walkableTileNames.Exists(name => tileName.Contains(name));

                    Vector2Int coord = new Vector2Int(pos.x, pos.y);

                    //还没添加，就新建；否则覆盖已有的节点
                    if (!nodeGrid.ContainsKey(coord))
                    {
                        nodeGrid[coord] = new GridNode(pos.x, pos.y, isWalkable);
                    }
                    else
                    {
                        //如果之前被标记成 false，这里可以覆盖为true（只要识别到它是walkable Tile）
                        nodeGrid[coord].isWalkable = isWalkable;
                    }
                }
            }
        }

        // -------------------------
        // 3. 处理TilemapCollider2D
        //    如果Tilemap有碰撞，就默认视为障碍
        // -------------------------

        // 对ObstacleTilemaps再走一遍，若带Collider则覆盖
        if (obstacleTilemaps != null && obstacleTilemaps.Length > 0)
        {
            foreach (var obstacleTilemap in obstacleTilemaps)
            {
                if (obstacleTilemap == null) continue;
                var collider = obstacleTilemap.GetComponent<TilemapCollider2D>();
                if (collider == null) continue; // 没有Collider就跳过

                BoundsInt bounds = obstacleTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    if (obstacleTilemap.HasTile(pos))
                    {
                        Vector2Int coord = new Vector2Int(pos.x, pos.y);
                        if (!nodeGrid.ContainsKey(coord))
                        {
                            nodeGrid[coord] = new GridNode(pos.x, pos.y, false);
                        }
                        else
                        {
                            nodeGrid[coord].isWalkable = false;
                        }
                    }
                }
            }
        }

        // 对WalkableTilemaps再走一遍
        if (walkableTilemaps != null && walkableTilemaps.Length > 0)
        {
            foreach (var walkableTilemap in walkableTilemaps)
            {
                if (walkableTilemap == null) continue;
                var collider = walkableTilemap.GetComponent<TilemapCollider2D>();
                if (collider == null) continue;

                BoundsInt bounds = walkableTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    if (walkableTilemap.HasTile(pos))
                    {
                        Vector2Int coord = new Vector2Int(pos.x, pos.y);
                        if (!nodeGrid.ContainsKey(coord))
                        {
                            nodeGrid[coord] = new GridNode(pos.x, pos.y, false);
                        }
                        else
                        {
                            nodeGrid[coord].isWalkable = false;
                        }
                    }
                }
            }
        }

        Debug.Log($"[TilemapNavigator] BuildNavigationGrid 完成，共生成 {nodeGrid.Count} 个节点。");
    }

    /// <summary>
    /// 返回某个格子是否可走
    /// </summary>
    public bool IsWalkable(int gridX, int gridY)
    {
        Vector2Int coord = new Vector2Int(gridX, gridY);
        return nodeGrid.ContainsKey(coord) && nodeGrid[coord].isWalkable;
    }

    /// <summary>
    /// 在Scene视图中绘制Gizmos，可视化可走格子、不可走格子
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!visualizeGrid || nodeGrid == null) return;

        foreach (var kvp in nodeGrid)
        {
            Vector2Int coord = kvp.Key;
            GridNode node = kvp.Value;

            Gizmos.color = node.isWalkable ? Color.green : Color.red;

            // 将网格坐标转换为世界坐标
            // 这里直接从walkable或obstacle里找一个Tilemap用来做转换参考
            Vector3 worldPos = Vector3.zero;
            bool foundTilemap = false;

            // 先在walkableTilemaps里找
            if (walkableTilemaps != null)
            {
                foreach (var walkableTilemap in walkableTilemaps)
                {
                    if (walkableTilemap == null) continue;
                    // 可能有些Tilemap只覆盖部分区域，这里直接用第一张找到的进行坐标转换
                    worldPos = walkableTilemap.GetCellCenterWorld(new Vector3Int(coord.x, coord.y, 0));
                    foundTilemap = true;
                    break;
                }
            }

            //若walkable没找到，就在obstacle里找
            if (!foundTilemap && obstacleTilemaps != null)
            {
                foreach (var obstacleTilemap in obstacleTilemaps)
                {
                    if (obstacleTilemap == null) continue;
                    worldPos = obstacleTilemap.GetCellCenterWorld(new Vector3Int(coord.x, coord.y, 0));
                    foundTilemap = true;
                    break;
                }
            }

            //若都没有Tilemap引用，就用coord直接做世界坐标(测试用)
            if (!foundTilemap)
            {
                worldPos = new Vector3(coord.x, coord.y, 0);
            }

            // 在Scene视图中绘制一个立方体
            Gizmos.DrawCube(worldPos, Vector3.one * gizmoCubeSize);
        }
    }
}
