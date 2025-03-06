using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerDirectionIndicator : MonoBehaviour
{
    [Header("UI 指示器")]
    // UI 中用来指示方向的箭头（Image组件），需要提前挂载在 Canvas 下
    public Image arrowImage;

    [Header("目标点 (网格坐标)")]
    // 目标点可以通过 Inspector 设置，也可以在运行时动态更新
    public Vector2Int targetGridPos;

    private Transform playerTransform;
    private AStarMgr aStarMgr;

    // 控制刷新频率，避免每帧计算 A* 路径带来的性能问题
    public float updateInterval = 0.2f;
    private float timer;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("未找到玩家对象，请检查Player标签！");

        aStarMgr = AStarMgr.Instance;
        if (aStarMgr == null)
            Debug.LogError("AStarMgr.Instance 未初始化，请检查单例设置！");
    }

    void Update()
    {
        bool isTargetWalkable = AStarMgr.Instance.tilemapNavigator.IsWalkable(targetGridPos.x, targetGridPos.y);
        Debug.Log($"终点({targetGridPos.x},{targetGridPos.y})是否可走：{isTargetWalkable}");
        if (!isTargetWalkable)
        {
            // 这里可以进一步打印 nodeGrid 中对应坐标的信息
            if (!AStarMgr.Instance.tilemapNavigator.nodeGrid.ContainsKey(targetGridPos))
            {
                Debug.LogWarning($"nodeGrid中不包含目标坐标 {targetGridPos}");
            }
            else
            {
                var node = AStarMgr.Instance.tilemapNavigator.nodeGrid[targetGridPos];
                Debug.LogWarning($"目标坐标 {targetGridPos} 的 isWalkable 状态为 {node.isWalkable}");
            }
        }

        if (collectManager.Instance == null || !collectManager.Instance.collecteFinish)
            return;
        if (playerTransform == null || aStarMgr == null || arrowImage == null)
            return;

        timer += Time.deltaTime;
        if (timer < updateInterval)
            return;
        timer = 0;

        // 将玩家的世界坐标转换成网格坐标
        Vector2Int playerGridPos = new Vector2Int(
            Mathf.RoundToInt(playerTransform.position.x),
            Mathf.RoundToInt(playerTransform.position.y)
        );

        // 使用 A* 算法从玩家当前的网格坐标到目标点寻找路径
        List<Vector2Int> path = aStarMgr.FindPath(playerGridPos, targetGridPos);
        if (path != null && path.Count > 1)
        {

            // 注意：path[0] 是玩家当前位置，path[1] 则是下一个应该走的节点
            Vector2Int nextNode = path[1];

            // 计算方向向量（从玩家当前位置指向下一个节点）
            Vector2 direction = new Vector2(nextNode.x - playerGridPos.x, nextNode.y - playerGridPos.y);

            // 计算旋转角度（以右边为 0 度，逆时针旋转）
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 更新UI箭头的旋转，使之指向目标方向
            arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // 如果找不到路径，可以选择隐藏或重置箭头
            // arrowImage.enabled = false;
        }
    }
}
