using System.Collections.Generic;
using UnityEngine;

public class TestAStar : MonoBehaviour
{
    public Vector2Int targetPos; // �̶��յ�
    private Vector2Int startPos; // ��㣨���λ�ã�

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ������ҵ�ǰλ��ת����������õ� startPos
            startPos = new Vector2Int(
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y)
            );

            // ���� A* Ѱ·
            List<Vector2Int> path = AStarMgr.Instance.FindPath(startPos, targetPos);
            if (path.Count > 0)
            {
                //Debug.Log("Ѱ·�ɹ���·��������" + path.Count);
                // ������������ý�ɫ����ƶ������߻��� Gizmos
            }
            else
            {
                Debug.Log("Ѱ·ʧ�ܻ�û��·�����ߡ�");
            }
        }
    }
}
