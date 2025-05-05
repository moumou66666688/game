using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine.UI; // 引入 UI 相关组件

public class ThumbIndexClick : MonoBehaviour
{
    [SerializeField] private HandLandmarkerRunner handLandmarkerRunner; // 拖拽绑定 HandLandmarkerRunner
    [SerializeField] private float clickThreshold = 0.06f; // 距离阈值，单位是 NormalizedLandmark 的归一化坐标
    private bool isClicking = false;

    private PlayerController playerController; // PlayerController 引用

    // 在 Inspector 中设置这三个 UI 面板引用
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject victoryPanel;

    void Start()
    {
        // 获取 PlayerController 脚本组件
        playerController = FindObjectOfType<PlayerController>();
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
    }

    // 手势识别后触发的行为
    private void OnThumbIndexClick()
    {
        // 使用 Inspector 设置的引用判断是否显示
        if ((startMenu != null && startMenu.activeInHierarchy) ||
            (gameOverUI != null && gameOverUI.activeInHierarchy) ||
            (victoryPanel != null && victoryPanel.activeInHierarchy))
        {
            Debug.Log("UI Panel active, jump action blocked.");
            return;
        }

        Debug.Log(" Thumb and Index close — trigger jump!");
        if (playerController != null)
        {
            playerController.Jump();
            Debug.Log("Player jumped!");
        }
        else
        {
            Debug.LogError("PlayerController reference is not set!");
        }
    }
}