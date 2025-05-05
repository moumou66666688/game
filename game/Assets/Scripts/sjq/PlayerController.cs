using UnityEngine;

// 孔明灯控制（带热力学升力模拟）
public class PlayerController : MonoBehaviour
{
    [Header("基础移动参数")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private bool isMoving = false;

    [Header("热力学参数")]
    public float jumpForce = 5f;              // 默认跳跃基础值（可微调）
    public float airDensity = 1.225f;         // ρ_air，kg/m?
    public float volume = 0.02f;              // V，孔明灯容积，m?
    public float gravity = 9.81f;             // g，重力加速度
    public float T_air = 293f;                // T_air，环境温度 (K)
    public float T_lamp = 400f;               // T_lamp，初始内部温度 (K)
    public float fuel = 100f;                 // 初始燃料
    public float fuelBurnRate = 5f;           // 每秒消耗燃料
    public float heatingEfficiency = 2f;      // 每单位燃料转化为温度的比例

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 启动前不下落
    }

    private void Update()
    {
        // 游戏结束或未启用控制时不执行
        if (!isMoving || GameManagers.Instance == null || GameManagers.Instance.IsGameOver())
            return;

        // 鼠标左键点击跳跃（使用热力学升力）
        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // 燃料消耗与温度变化
        if (fuel > 0)
        {
            fuel -= fuelBurnRate * Time.deltaTime;
            T_lamp += heatingEfficiency * Time.deltaTime;
        }
        else
        {
            // 燃料耗尽后，逐渐冷却
            T_lamp = Mathf.Max(T_air, T_lamp - 10f * Time.deltaTime);
        }

        // 低于 -5f 判定失败
        if (transform.position.y < -5f)
        {
            GameManagers.Instance.GameOver();
        }
    }

    public void Jump()
    {
        // 使用热力学升力公式计算垂直速度
        float liftForce = airDensity * volume * gravity * (T_lamp / T_air - 1f);
        float verticalSpeed = Mathf.Max(0f, liftForce + jumpForce); // 加上 jumpForce 提升响应感

        // 设置速度
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
