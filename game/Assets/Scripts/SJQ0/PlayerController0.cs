using UnityEngine;

public class PlayerController0 : MonoBehaviour
{
    [Header("基础移动参数")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool isOnGround = false;

    [Header("热力学参数")]
    public float jumpForce = 5f;
    public float airDensity = 1.225f;
    public float volume = 0.02f;
    public float gravity = 9.81f;
    public float T_air = 293f;
    public float T_lamp = 400f;
    public float fuel = 100f;
    public float fuelBurnRate = 5f;
    public float heatingEfficiency = 2f;

    [Header("火焰控制")]
    public FireController fireController;  // 引用 FireController

    private void Awake()
    {
        // 在 Awake 中获取 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();

        // 确保 Rigidbody2D 和 FireController 被正确赋值
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this GameObject");
        }

        if (fireController == null)
        {
            Debug.LogError("FireController is not assigned!");
        }

        rb.gravityScale = 0; // 禁用重力
    }

    private void Update()
    {
        // 游戏未开始或未允许移动时，不执行控制
        if (!isMoving || MainGameController.Instance == null || !MainGameController.Instance.IsGameStarted())
            return;

        // 点击鼠标左键跳跃
        if (Input.GetMouseButtonDown(0) && !isOnGround)
        {
            Jump();
        }

        // 燃料相关逻辑
        if (fuel > 0)
        {
            fuel -= fuelBurnRate * Time.deltaTime;
            T_lamp += heatingEfficiency * Time.deltaTime;
        }
        else
        {
            T_lamp = Mathf.Max(T_air, T_lamp - 10f * Time.deltaTime);
        }

        // 火焰控制：只有在空中时才会控制火焰大小
        if (!isOnGround)
        {
            if (rb.velocity.y > 0)  // 上升时，放大火焰
            {
                if (fireController != null)
                {
                    fireController.GrowFire();
                }
            }
            else if (rb.velocity.y < 0)  // 下降时，缩小火焰
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
        // 离开地面
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
            rb.velocity = Vector2.zero;  // 停止移动
            rb.gravityScale = 0;         // 禁用重力保持静止
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            rb.gravityScale = 1;         // 离开地面恢复重力
        }
    }

    public float GetVerticalSpeed()
    {
        return rb.velocity.y; // 获取并返回玩家的垂直速度
    }
}
