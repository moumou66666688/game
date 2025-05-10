using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManagers.Instance.GameOver(); // ¥•∑¢”Œœ∑Ω· ¯
        }
    }
}
