using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGameController : MonoBehaviour
{
    public static MainGameController Instance;

    [Header("UI 控件")]
    public GameObject startMenu;       // StartMenu UI 页面
    public Button startButton;         // 开始按钮
    public GameObject popupPanel;      // 延迟弹出的 Panel

    [Header("游戏对象")]
    public PlayerController0 player;
    public FireController fireController;  // FireController 引用，用于控制火焰

    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameStarted = false;

        if (player == null)
        {
            Debug.LogError("PlayerController0 reference is not set in MainGameController.");
            return;
        }

        player.DisableMovement();

        if (startMenu != null)
            startMenu.SetActive(true);

        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (popupPanel != null)
            popupPanel.SetActive(false);  // 初始隐藏 Panel
    }

    private void Update()
    {
        if (gameStarted && player != null && fireController != null)
        {
            float verticalSpeed = player.GetVerticalSpeed();

            if (verticalSpeed > 0)
            {
                fireController.GrowFire();
            }
            else if (verticalSpeed < 0)
            {
                fireController.ShrinkFire();
            }
        }
    }

    public void StartGame()
    {
        gameStarted = true;

        if (startMenu != null)
            startMenu.SetActive(false);

        player.EnableMovement();

        if (fireController != null)
        {
            fireController.GrowFire();
            Debug.Log("Fire is growing!");
        }
        else
        {
            Debug.LogError("FireController reference is not set!");
        }

        // 延迟显示弹窗
        StartCoroutine(ShowPopupPanelAfterDelay(26f)); // 26 秒后显示 Panel
    }

    private IEnumerator ShowPopupPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            Debug.Log("Popup Panel displayed!");
        }
        else
        {
            Debug.LogWarning("PopupPanel is not assigned.");
        }
    }

    public void ClosePopupPanel()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            Debug.Log("Popup Panel closed.");
        }
    }

    public void StopFire()
    {
        if (fireController != null)
        {
            fireController.ShrinkFire();
            Debug.Log("Fire is shrinking!");
        }
        else
        {
            Debug.LogError("FireController reference is not set!");
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
}
