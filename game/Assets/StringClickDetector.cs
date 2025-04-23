using UnityEngine;

public class StringClickDetector : MonoBehaviour
{
    [Header("对应上方的千纸鹤")]
    public GameObject targetCrane; // 拖拽对应的千纸鹤到这里

    private void OnMouseDown() // 当鼠标点击时触发
    {
        if (targetCrane != null)
        {
            // 触发千纸鹤跳跃动画
            targetCrane.GetComponent<CraneJump>().Jump();
        }
    }
}