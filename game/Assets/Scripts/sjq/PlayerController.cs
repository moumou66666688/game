using UnityEngine;

// �����ƿ��ƣ�������ѧ����ģ�⣩
public class PlayerController : MonoBehaviour
{
    [Header("�����ƶ�����")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMoving = false;

    [Header("����ѧ����")]
    public float jumpForce = 5f;              // Ĭ����Ծ����ֵ����΢����
    public float airDensity = 1.225f;         // ��_air��kg/m?
    public float volume = 0.02f;              // V���������ݻ���m?
    public float gravity = 9.81f;             // g���������ٶ�
    public float T_air = 293f;                // T_air�������¶� (K)
    public float T_lamp = 400f;               // T_lamp����ʼ�ڲ��¶� (K)
    public float fuel = 100f;                 // ��ʼȼ��
    public float fuelBurnRate = 5f;           // ÿ������ȼ��
    public float heatingEfficiency = 2f;      // ÿ��λȼ��ת��Ϊ�¶ȵı���

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // ����ǰ������
    }

    private void Update()
    {
        // ��Ϸ������δ���ÿ���ʱ��ִ��
        if (!isMoving || GameManagers.Instance == null || GameManagers.Instance.IsGameOver())
            return;

        // �����������Ծ��ʹ������ѧ������
        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // ȼ���������¶ȱ仯
        if (fuel > 0)
        {
            fuel -= fuelBurnRate * Time.deltaTime;
            T_lamp += heatingEfficiency * Time.deltaTime;
        }
        else
        {
            // ȼ�Ϻľ�������ȴ
            T_lamp = Mathf.Max(T_air, T_lamp - 10f * Time.deltaTime);
        }

        // ���� -5f �ж�ʧ��
        if (transform.position.y < -5f)
        {
            GameManagers.Instance.GameOver();
        }
    }

    public void Jump()
    {
        // ʹ������ѧ������ʽ���㴹ֱ�ٶ�
        float liftForce = airDensity * volume * gravity * (T_lamp / T_air - 1f);
        float verticalSpeed = Mathf.Max(0f, liftForce + jumpForce); // ���� jumpForce ������Ӧ��

        // �����ٶ�
        rb.velocity = new Vector2(moveSpeed, verticalSpeed);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManagers.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GameManagers.Instance.GameOver();
        }
    }
}
