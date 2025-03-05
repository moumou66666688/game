using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerDirectionIndicator : MonoBehaviour
{
    [Header("UI ָʾ��")]
    // UI ������ָʾ����ļ�ͷ��Image���������Ҫ��ǰ������ Canvas ��
    public Image arrowImage;

    [Header("Ŀ��� (��������)")]
    // Ŀ������ͨ�� Inspector ���ã�Ҳ����������ʱ��̬����
    public Vector2Int targetGridPos;

    private Transform playerTransform;
    private AStarMgr aStarMgr;

    // ����ˢ��Ƶ�ʣ�����ÿ֡���� A* ·����������������
    public float updateInterval = 0.2f;
    private float timer;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("δ�ҵ���Ҷ�������Player��ǩ��");

        aStarMgr = AStarMgr.Instance;
        if (aStarMgr == null)
            Debug.LogError("AStarMgr.Instance δ��ʼ�������鵥�����ã�");
    }

    void Update()
    {
        bool isTargetWalkable = AStarMgr.Instance.tilemapNavigator.IsWalkable(targetGridPos.x, targetGridPos.y);
        Debug.Log($"�յ�({targetGridPos.x},{targetGridPos.y})�Ƿ���ߣ�{isTargetWalkable}");
        if (!isTargetWalkable)
        {
            // ������Խ�һ����ӡ nodeGrid �ж�Ӧ�������Ϣ
            if (!AStarMgr.Instance.tilemapNavigator.nodeGrid.ContainsKey(targetGridPos))
            {
                Debug.LogWarning($"nodeGrid�в�����Ŀ������ {targetGridPos}");
            }
            else
            {
                var node = AStarMgr.Instance.tilemapNavigator.nodeGrid[targetGridPos];
                Debug.LogWarning($"Ŀ������ {targetGridPos} �� isWalkable ״̬Ϊ {node.isWalkable}");
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

        // ����ҵ���������ת������������
        Vector2Int playerGridPos = new Vector2Int(
            Mathf.RoundToInt(playerTransform.position.x),
            Mathf.RoundToInt(playerTransform.position.y)
        );

        // ʹ�� A* �㷨����ҵ�ǰ���������굽Ŀ���Ѱ��·��
        List<Vector2Int> path = aStarMgr.FindPath(playerGridPos, targetGridPos);
        if (path != null && path.Count > 1)
        {

            // ע�⣺path[0] ����ҵ�ǰλ�ã�path[1] ������һ��Ӧ���ߵĽڵ�
            Vector2Int nextNode = path[1];

            // ���㷽������������ҵ�ǰλ��ָ����һ���ڵ㣩
            Vector2 direction = new Vector2(nextNode.x - playerGridPos.x, nextNode.y - playerGridPos.y);

            // ������ת�Ƕȣ����ұ�Ϊ 0 �ȣ���ʱ����ת��
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ����UI��ͷ����ת��ʹָ֮��Ŀ�귽��
            arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // ����Ҳ���·��������ѡ�����ػ����ü�ͷ
            // arrowImage.enabled = false;
        }
    }
}
