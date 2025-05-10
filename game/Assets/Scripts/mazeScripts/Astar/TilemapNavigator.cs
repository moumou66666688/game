using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapNavigator : MonoBehaviour
{
    [Header("Tilemaps (Multiple)")]
    [Tooltip("���п��ߵ�Tilemap")]
    public Tilemap[] walkableTilemaps;
    [Tooltip("�����ϰ����Tilemap")]
    public Tilemap[] obstacleTilemaps;

    [Header("Tile Name Settings")]
    public List<string> walkableTileNames = new List<string> { "Grass", "Grass", "Grass", "Stone Ground" };
    public List<string> obstacleTileNames = new List<string> { "Wall", "Wall", "Wall" };

    [Header("Debug Visualization")]
    public bool visualizeGrid = true;
    [Tooltip("Scene��ͼ�л������������С")]
    public float gizmoCubeSize = 0.8f;
    //
    /// <summary>
    /// �����������ڵ㡣KeyΪ��ά����(x,y)��
    /// </summary>
    public Dictionary<Vector2Int, GridNode> nodeGrid = new Dictionary<Vector2Int, GridNode>();

    private void Start()
    {
        BuildNavigationGrid();
    }

    /// <summary>
    /// �����Tilemap�����ݺϲ�����������ڵ㡣
    /// </summary>
    public void BuildNavigationGrid()
    {
        nodeGrid.Clear();

        // -------------------------
        // 1. ɨ������ obstacleTilemaps
        // -------------------------

        Debug.Log("***obstcale����:" + obstacleTilemaps.Length + "***");
        if (obstacleTilemaps != null && obstacleTilemaps.Length > 0)
        {
            foreach (var obstacleTilemap in obstacleTilemaps)
            {
                if (obstacleTilemap == null) continue;

                BoundsInt bounds = obstacleTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    if (!obstacleTilemap.HasTile(pos)) continue;

                    // ��ȡTile���ж��Ƿ�ƥ��"obstacleTileNames"
                    TileBase tile = obstacleTilemap.GetTile(pos);
                    if (tile == null) continue;
                    string tileName = tile.name;

                    bool isObstacle = obstacleTileNames.Exists(name => tileName.Contains(name));
                    if (isObstacle)
                    {
                        Vector2Int coord = new Vector2Int(pos.x, pos.y);

                        // �����δ��ӣ��򴴽�һ���ڵ㲢���Ϊ��������
                        if (!nodeGrid.ContainsKey(coord))
                        {
                            nodeGrid[coord] = new GridNode(pos.x, pos.y, false);
                        }
                        else
                        {
                            // ����Ϊ��������
                            nodeGrid[coord].isWalkable = false;
                        }
                    }
                }
            }
        }

        // -------------------------
        // 2. ɨ������ walkableTilemaps
        // -------------------------
        Debug.Log("***����tilemap�ĳ���:" + walkableTilemaps.Length + "***");
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

                    //��û��ӣ����½������򸲸����еĽڵ�
                    if (!nodeGrid.ContainsKey(coord))
                    {
                        nodeGrid[coord] = new GridNode(pos.x, pos.y, isWalkable);
                    }
                    else
                    {
                        //���֮ǰ����ǳ� false��������Ը���Ϊtrue��ֻҪʶ������walkable Tile��
                        nodeGrid[coord].isWalkable = isWalkable;
                    }
                }
            }
        }

        // -------------------------
        // 3. ����TilemapCollider2D
        //    ���Tilemap����ײ����Ĭ����Ϊ�ϰ�
        // -------------------------

        // ��ObstacleTilemaps����һ�飬����Collider�򸲸�
        if (obstacleTilemaps != null && obstacleTilemaps.Length > 0)
        {
            foreach (var obstacleTilemap in obstacleTilemaps)
            {
                if (obstacleTilemap == null) continue;
                var collider = obstacleTilemap.GetComponent<TilemapCollider2D>();
                if (collider == null) continue; // û��Collider������

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

        // ��WalkableTilemaps����һ��
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

        Debug.Log($"[TilemapNavigator] BuildNavigationGrid ��ɣ������� {nodeGrid.Count} ���ڵ㡣");
    }

    /// <summary>
    /// ����ĳ�������Ƿ����
    /// </summary>
    public bool IsWalkable(int gridX, int gridY)
    {
        Vector2Int coord = new Vector2Int(gridX, gridY);
        return nodeGrid.ContainsKey(coord) && nodeGrid[coord].isWalkable;
    }

    /// <summary>
    /// ��Scene��ͼ�л���Gizmos�����ӻ����߸��ӡ������߸���
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!visualizeGrid || nodeGrid == null) return;

        foreach (var kvp in nodeGrid)
        {
            Vector2Int coord = kvp.Key;
            GridNode node = kvp.Value;

            Gizmos.color = node.isWalkable ? Color.green : Color.red;

            // ����������ת��Ϊ��������
            // ����ֱ�Ӵ�walkable��obstacle����һ��Tilemap������ת���ο�
            Vector3 worldPos = Vector3.zero;
            bool foundTilemap = false;

            // ����walkableTilemaps����
            if (walkableTilemaps != null)
            {
                foreach (var walkableTilemap in walkableTilemaps)
                {
                    if (walkableTilemap == null) continue;
                    // ������ЩTilemapֻ���ǲ�����������ֱ���õ�һ���ҵ��Ľ�������ת��
                    worldPos = walkableTilemap.GetCellCenterWorld(new Vector3Int(coord.x, coord.y, 0));
                    foundTilemap = true;
                    break;
                }
            }

            //��walkableû�ҵ�������obstacle����
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

            //����û��Tilemap���ã�����coordֱ������������(������)
            if (!foundTilemap)
            {
                worldPos = new Vector3(coord.x, coord.y, 0);
            }

            // ��Scene��ͼ�л���һ��������
            Gizmos.DrawCube(worldPos, Vector3.one * gizmoCubeSize);
        }
    }
}
