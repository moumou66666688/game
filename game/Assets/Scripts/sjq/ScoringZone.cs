using UnityEngine;
//触发器加分逻辑
public class ScoringZone : MonoBehaviour
{
    private bool passed = false; // 防止重复得分

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!passed && collision.CompareTag("Player"))
        {
            Debug.Log("触发器 ");
            passed = true;
            GameManagers.Instance.AddScore(); // 加分

        }

    }
}
