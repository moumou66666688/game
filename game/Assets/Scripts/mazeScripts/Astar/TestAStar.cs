using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public Vector2Int targetPos; // 固定终点
    private Vector2Int startPos; // 起点（玩家位置）

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 假设玩家当前位置转成网格坐标得到 startPos
            startPos = new Vector2Int(
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y)
            );

            // 调用 A* 寻路
            List<Vector2Int> path = AStarMgr.Instance.FindPath(startPos, targetPos);
            if (path.Count > 0)
            {
                //Debug.Log("寻路成功，路径点数：" + path.Count);
                // 你可以在这里让角色逐点移动，或者绘制 Gizmos
            }
            else
            {
                Debug.Log("寻路失败或没有路径可走。");
            }
        }
    }
}
