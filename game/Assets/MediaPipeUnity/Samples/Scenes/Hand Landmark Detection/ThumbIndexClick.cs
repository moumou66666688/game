using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine.UI;

public class ThumbIndexClick : MonoBehaviour
{
    [SerializeField] private HandLandmarkerRunner handLandmarkerRunner;
    [SerializeField] private float clickThreshold = 0.06f;
    private bool isClicking = false;
    private bool hasMovedFire = false;

    private PlayerController playerController;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject victoryPanel;

    [Header("UI Fire ����")]
    [SerializeField] private RectTransform fireObject; // fire �� UI Ԫ�أ����� Canvas ��
    //[SerializeField] private Vector2 uiTargetPosition = new Vector2(500f,-300f); // ���Ͻ����꣨���� Canvas ��С������
    [SerializeField] private Vector2 uiTargetPosition = new Vector2(800f, 200f);

    [SerializeField] private float scaleFactor = 1.5f; // �Ŵ���
    [SerializeField] private float animationDuration = 1f;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        HandLandmarkerResult result = handLandmarkerRunner.GetHandLandmarkResult();

        if (result.handLandmarks == null || result.handLandmarks.Count == 0)
            return;

        NormalizedLandmarks normalized = result.handLandmarks[0];
        if (normalized.landmarks == null || normalized.landmarks.Count <= 8)
            return;

        NormalizedLandmark thumbTip = normalized.landmarks[4];
        NormalizedLandmark indexTip = normalized.landmarks[8];

        float dx = thumbTip.x - indexTip.x;
        float dy = thumbTip.y - indexTip.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        if (distance < clickThreshold)
        {
            if (!isClicking)
            {
                isClicking = true;
                OnThumbIndexClick();
            }
        }
        else
        {
            isClicking = false;
        }
    }

    private void OnThumbIndexClick()
    {
        if ((startMenu != null && startMenu.activeInHierarchy) ||
            (gameOverUI != null && gameOverUI.activeInHierarchy) ||
            (victoryPanel != null && victoryPanel.activeInHierarchy))
        {
            Debug.Log("UI Panel active, jump action blocked.");
            return;
        }

        Debug.Log("Thumb and Index close �� trigger jump!");

        if (playerController != null)
        {
            playerController.Jump();
        }

        // ��� fire ��û���ƶ���������������
        if (!hasMovedFire && fireObject != null)
        {
            hasMovedFire = true;
            StartCoroutine(AnimateFireUI());
        }
    }

    private IEnumerator AnimateFireUI()
    {
        // ��ʼ����
        Vector2 startPos = fireObject.anchoredPosition;
        Vector3 startScale = fireObject.localScale;
        Vector3 targetScale = startScale * scaleFactor;

        float t = 0f;

        // ���Ž׶�
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float progress = t / animationDuration;
            fireObject.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        // �ƶ��׶�
        t = 0f;
        Vector2 currentPos = fireObject.anchoredPosition;
        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float progress = t / animationDuration;
            fireObject.anchoredPosition = Vector2.Lerp(currentPos, uiTargetPosition, progress);
            yield return null;
        }
    }
}
