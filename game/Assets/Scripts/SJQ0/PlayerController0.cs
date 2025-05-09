using UnityEngine;

public class PlayerController0 : MonoBehaviour
{
    [Header("�����ƶ�����")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool isOnGround = false;

    [Header("����ѧ����")]
    public float jumpForce = 5f;
    public float airDensity = 1.225f;
    public float volume = 0.02f;
    public float gravity = 9.81f;
    public float T_air = 293f;
    public float T_lamp = 400f;
    public float fuel = 100f;
    public float fuelBurnRate = 5f;
    public float heatingEfficiency = 2f;

    [Header("�������")]
    public FireController fireController;  // ���� FireController

    private void Awake()
    {
        // �� Awake �л�ȡ Rigidbody2D ���
        rb = GetComponent<Rigidbody2D>();

        // ȷ�� Rigidbody2D �� FireController ����ȷ��ֵ
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this GameObject");
        }

        if (fireController == null)
        {
            Debug.LogError("FireController is not assigned!");
        }

        rb.gravityScale = 0; // ��������
    }

    private void Update()
    {
        // ��Ϸδ��ʼ��δ�����ƶ�ʱ����ִ�п���
        if (!isMoving || MainGameController.Instance == null || !MainGameController.Instance.IsGameStarted())
            return;

        // �����������Ծ
        if (Input.GetMouseButtonDown(0) && !isOnGround)
        {
            Jump();
        }

        // ȼ������߼�
        if (fuel > 0)
        {
            fuel -= fuelBurnRate * Time.deltaTime;
            T_lamp += heatingEfficiency * Time.deltaTime;
        }
        else
        {
            T_lamp = Mathf.Max(T_air, T_lamp - 10f * Time.deltaTime);
        }

        // ������ƣ�ֻ���ڿ���ʱ�Ż���ƻ����С
        if (!isOnGround)
        {
            if (rb.velocity.y > 0)  // ����ʱ���Ŵ����
            {
                if (fireController != null)
                {
                    fireController.GrowFire();
                }
            }
            else if (rb.velocity.y < 0)  // �½�ʱ����С����
            {
                if (fireController != null)
                {
                    fireController.ShrinkFire();
                }
            }
        }
    }

    public void Jump()
    {
        // �뿪����
        isOnGround = false;

        float liftForce = airDensity * volume * gravity * (T_lamp / T_air - 1f);
        float verticalSpeed = Mathf.Max(0f, liftForce + jumpForce);

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
        if (rb != null)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else
        {
            Debug.LogError("Rigidbody2D is null, cannot disable movement.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            rb.velocity = Vector2.zero;  // ֹͣ�ƶ�
            rb.gravityScale = 0;         // �����������־�ֹ
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            rb.gravityScale = 1;         // �뿪����ָ�����
        }
    }

    public float GetVerticalSpeed()
    {
        return rb.velocity.y; // ��ȡ��������ҵĴ�ֱ�ٶ�
    }
}
