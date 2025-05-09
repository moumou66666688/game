using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("�������")]
    public Transform playerTransform; // ������ Inspector ���ֶ�����

    private void Update()
    {
        if (playerTransform != null)
        {
            // ֻ���¿������ X ���꣬���� Y �� Z ����
            transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        }
    }
}
