using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine.UI; // 引入 UI 相关组件

public class ThumbIndexClick0 : MonoBehaviour
{
    [SerializeField] private HandLandmarkerRunner handLandmarkerRunner; // 拖拽绑定 HandLandmarkerRunner
    [SerializeField] private float clickThreshold = 0.06f; // 距离阈值，单位是 NormalizedLandmark 的归一化坐标
    private bool isClicking = false;

    private PlayerController0 playerController; // PlayerController 引用
    private FireController fireController; // FireController 引用

    // 在 Inspector 中设置这三个 UI 面板引用
    [SerializeField] private GameObject startMenu;

    void Start()
    {
        // 获取 PlayerController 脚本组件
        playerController = FindObjectOfType<PlayerController0>();

        // 获取 FireController 脚本组件
        fireController = FindObjectOfType<FireController>();
    }

    void Update()
    {
        // 1. 获取当前帧的检测结果
        HandLandmarkerResult result = handLandmarkerRunner.GetHandLandmarkResult();

        // 2. 确保有 handLandmarks 列表，并且至少检测到一只手
        if (result.handLandmarks == null || result.handLandmarks.Count == 0)
        {
            Debug.LogWarning("No handLandmarks available.");
            return;
        }

        // 3. 取第一只手的标准化地标集合
        NormalizedLandmarks normalized = result.handLandmarks[0];

        // 4. 确保这个手的 landmarks 列表有效
        if (normalized.landmarks == null || normalized.landmarks.Count <= 8)
        {
            Debug.LogWarning("Not enough landmark points.");
            return;
        }

        // 5. 获取大拇指尖 (索引 4) 和食指尖 (索引 8)
        NormalizedLandmark thumbTip = normalized.landmarks[4];
        NormalizedLandmark indexTip = normalized.landmarks[8];

        // 6. 计算它们在二维平面上的距离
        float dx = thumbTip.x - indexTip.x;
        float dy = thumbTip.y - indexTip.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        //Debug.Log($"Thumb-Index distance: {distance:F3}");

        // 7. 如果小于阈值，且之前没有触发，就执行点击回调
        if (distance < clickThreshold)
        {
            Debug.Log($"Thumb-Index distance: {distance:F3}");
            if (!isClicking)
            {
                isClicking = true;
                OnThumbIndexClick();  // 调用点击事件
            }
        }
        else
        {
            isClicking = false;
        }

        // 触发火焰大小的变化，根据玩家垂直速度来判断
        if (playerController != null)
        {
            float verticalSpeed = playerController.GetVerticalSpeed(); // 获取玩家的垂直速度

            // 上升时放大火焰
            if (verticalSpeed > 0)
            {
                if (fireController != null)
                {
                    fireController.GrowFire();
                }
            }
            // 下降时恢复火焰
            else if (verticalSpeed < 0)
            {
                if (fireController != null)
                {
                    fireController.ShrinkFire();
                }
            }
        }
    }

    // 手势识别后触发的行为
    private void OnThumbIndexClick()
    {
        // 使用 Inspector 设置的引用判断是否显示
        if ((startMenu != null && startMenu.activeInHierarchy))
        {
            Debug.Log("UI Panel active, jump action blocked.");
            return;
        }

        Debug.Log("Thumb and Index close — trigger jump and fire actions!");

        // 触发 Player 跳跃
        if (playerController != null)
        {
            playerController.Jump();
            Debug.Log("Player jumped!");
        }
        else
        {
            Debug.LogError("PlayerController reference is not set!");
        }

        // 触发 Fire 控制
        if (fireController != null)
        {
            fireController.GrowFire();  // 或者 fireController.ShrinkFire()，根据需要
            Debug.Log("Fire grew!");
        }
        else
        {
            Debug.LogError("FireController reference is not set!");
        }
    }
}
