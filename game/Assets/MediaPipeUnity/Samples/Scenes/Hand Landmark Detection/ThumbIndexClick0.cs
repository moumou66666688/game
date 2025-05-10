using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine.UI; // ���� UI ������

public class ThumbIndexClick0 : MonoBehaviour
{
    [SerializeField] private HandLandmarkerRunner handLandmarkerRunner; // ��ק�� HandLandmarkerRunner
    [SerializeField] private float clickThreshold = 0.06f; // ������ֵ����λ�� NormalizedLandmark �Ĺ�һ������
    private bool isClicking = false;

    private PlayerController0 playerController; // PlayerController ����
    private FireController fireController; // FireController ����

    // �� Inspector ������������ UI �������
    [SerializeField] private GameObject startMenu;

    void Start()
    {
        // ��ȡ PlayerController �ű����
        playerController = FindObjectOfType<PlayerController0>();

        // ��ȡ FireController �ű����
        fireController = FindObjectOfType<FireController>();
    }

    void Update()
    {
        // 1. ��ȡ��ǰ֡�ļ����
        HandLandmarkerResult result = handLandmarkerRunner.GetHandLandmarkResult();

        // 2. ȷ���� handLandmarks �б��������ټ�⵽һֻ��
        if (result.handLandmarks == null || result.handLandmarks.Count == 0)
        {
            Debug.LogWarning("No handLandmarks available.");
            return;
        }

        // 3. ȡ��һֻ�ֵı�׼���ر꼯��
        NormalizedLandmarks normalized = result.handLandmarks[0];

        // 4. ȷ������ֵ� landmarks �б���Ч
        if (normalized.landmarks == null || normalized.landmarks.Count <= 8)
        {
            Debug.LogWarning("Not enough landmark points.");
            return;
        }

        // 5. ��ȡ��Ĵָ�� (���� 4) ��ʳָ�� (���� 8)
        NormalizedLandmark thumbTip = normalized.landmarks[4];
        NormalizedLandmark indexTip = normalized.landmarks[8];

        // 6. ���������ڶ�άƽ���ϵľ���
        float dx = thumbTip.x - indexTip.x;
        float dy = thumbTip.y - indexTip.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        //Debug.Log($"Thumb-Index distance: {distance:F3}");

        // 7. ���С����ֵ����֮ǰû�д�������ִ�е���ص�
        if (distance < clickThreshold)
        {
            Debug.Log($"Thumb-Index distance: {distance:F3}");
            if (!isClicking)
            {
                isClicking = true;
                OnThumbIndexClick();  // ���õ���¼�
            }
        }
        else
        {
            isClicking = false;
        }

        // ���������С�ı仯��������Ҵ�ֱ�ٶ����ж�
        if (playerController != null)
        {
            float verticalSpeed = playerController.GetVerticalSpeed(); // ��ȡ��ҵĴ�ֱ�ٶ�

            // ����ʱ�Ŵ����
            if (verticalSpeed > 0)
            {
                if (fireController != null)
                {
                    fireController.GrowFire();
                }
            }
            // �½�ʱ�ָ�����
            else if (verticalSpeed < 0)
            {
                if (fireController != null)
                {
                    fireController.ShrinkFire();
                }
            }
        }
    }

    // ����ʶ��󴥷�����Ϊ
    private void OnThumbIndexClick()
    {
        // ʹ�� Inspector ���õ������ж��Ƿ���ʾ
        if ((startMenu != null && startMenu.activeInHierarchy))
        {
            Debug.Log("UI Panel active, jump action blocked.");
            return;
        }

        Debug.Log("Thumb and Index close �� trigger jump and fire actions!");

        // ���� Player ��Ծ
        if (playerController != null)
        {
            playerController.Jump();
            Debug.Log("Player jumped!");
        }
        else
        {
            Debug.LogError("PlayerController reference is not set!");
        }

        // ���� Fire ����
        if (fireController != null)
        {
            fireController.GrowFire();  // ���� fireController.ShrinkFire()��������Ҫ
            Debug.Log("Fire grew!");
        }
        else
        {
            Debug.LogError("FireController reference is not set!");
        }
    }
}
