using UnityEngine;
//�����ƿ���
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // ��ʼʱ��������
    }

    private void Update()
    {
        // ��Ϸ�������ܲ���
        if (!isMoving || GameManagers.Instance == null || GameManagers.Instance.IsGameOver()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // **��� Player ���� -5f��������Ļ�����򴥷�ʧ��**
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

    // **�����������ͨ Collider**
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManagers.Instance.GameOver();
        }
    }

    // **��������� Trigger**
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GameManagers.Instance.GameOver();
        }
    }
}
