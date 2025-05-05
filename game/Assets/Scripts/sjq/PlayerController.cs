using UnityEngine;
//孔明灯控制
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 开始时禁用重力
    }

    private void Update()
    {
        // 游戏结束后不能操作
        if (!isMoving || GameManagers.Instance == null || GameManagers.Instance.IsGameOver()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // **如果 Player 低于 -5f（超出屏幕），则触发失败**
        if (transform.position.y < -5f)
        {
            GameManagers.Instance.GameOver();
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(moveSpeed, jumpForce);
    }

    public void EnableMovement()
    {
        isMoving = true;
        rb.gravityScale = 1;
        rb.velocity = new Vector2(moveSpeed, 0);
    }

    public void DisableMovement()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    // **如果地面是普通 Collider**
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManagers.Instance.GameOver();
        }
    }

    // **如果地面是 Trigger**
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GameManagers.Instance.GameOver();
        }
    }
}
