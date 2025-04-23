using UnityEngine;

public class StringRippleEffect : MonoBehaviour
{
    private bool isRippleActive = false;  // 防止重复点击
    private float rippleSpeed = 20f;      // 波纹扩展速度（非常快速的扩展）
    private float maxRadius = 4.5f;      // 增大波纹最大半径（放大波纹）
    private int numPoints = 100;          // 每个圆环的点数
    private float ringSpacing = 0.6f;     // 增加每个圆环之间的间隔，使波纹更大
    private float fadeTime = 0.1f;        // 波纹消失的时间（非常快速的消失）
    private float ringExpandTime = 0.05f;  // 每个圆环扩展时间（极快的扩展）

    // 当点击到琴弦时触发
    private void OnMouseDown()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 获取点击位置
        clickPosition.z = 0f;  // 确保点击位置在 2D 平面上
        Debug.Log("琴弦被点击！");  // 调试输出
        if (!isRippleActive)  // 防止重复点击
        {
            StartCoroutine(ShowRippleEffect(clickPosition));  // 在点击位置显示波纹效果
        }
    }

    // 显示水波纹效果
    private System.Collections.IEnumerator ShowRippleEffect(Vector3 position)
    {
        isRippleActive = true;  // 激活波纹状态

        // 初始化波纹圈的半径
        float radius = 0f;
        int maxCircles = 3;  // 生成 3 个圆环，避免过多的圈数

        // 创建多个圆环
        for (int i = 0; i < maxCircles; i++)
        {
            // 创建每个新的圆环
            GameObject rippleObject = new GameObject("RippleEffect" + i);
            LineRenderer lineRenderer = rippleObject.AddComponent<LineRenderer>();

            // 设置 LineRenderer 属性
            lineRenderer.startWidth = 0.1f;  // 增大波纹起始宽度
            lineRenderer.endWidth = 0.1f;    // 增大波纹结束宽度
            lineRenderer.material = new Material(Shader.Find("Unlit/Transparent"));
            rippleObject.transform.position = new Vector3(position.x, position.y, 10f);  // 确保 Z 值足够大，置于最顶层
            lineRenderer.positionCount = numPoints;

            // 设置水墨色渐变颜色：从透明到水墨灰色，增加透明度以使水波纹更淡
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.clear, 0.0f), new GradientColorKey(new Color(0.2f, 0.2f, 0.2f), 1.0f) },  // 水墨色
                new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.3f, 1.0f) }  // 增加透明度使波纹更淡
            );
            lineRenderer.colorGradient = gradient;
            lineRenderer.sortingLayerName = "Foreground";  // 将水波纹放到 "Foreground" 层
            lineRenderer.sortingOrder = 10;  // 让它显示在最上面，排序值越大越在上层

            // 水波纹扩展
            float expandTime = 0f;
            while (expandTime < ringExpandTime)
            {
                // 使用 Mathf.Lerp 来平滑扩展，并减小半径差距
                float currentRadius = Mathf.Lerp(0f, radius + ringSpacing, expandTime / ringExpandTime);  // 快速扩展
                // 确保波纹扩展到最大半径
                currentRadius = Mathf.Min(currentRadius, maxRadius);
                // 更新每个圆环的点
                for (int j = 0; j < numPoints; j++)
                {
                    float angle = j * Mathf.PI * 2f / numPoints;
                    Vector3 ripplePoint = position + new Vector3(Mathf.Cos(angle) * currentRadius, Mathf.Sin(angle) * currentRadius, 0);
                    lineRenderer.SetPosition(j, ripplePoint);
                }
                expandTime += Time.deltaTime;  // 增加扩展时间
                yield return null;  // 等待下一帧
            }

            // 扩展完成后开始消退
            float fadeStartTime = Time.time;

            // 渐渐消失波纹
            while (Time.time < fadeStartTime + fadeTime)
            {
                // 计算当前消失的透明度
                float fadeProgress = (Time.time - fadeStartTime) / fadeTime;
                Color currentColor = lineRenderer.startColor;
                currentColor.a = Mathf.Lerp(1.0f, 0.0f, fadeProgress);  // 从完全不透明到完全透明
                lineRenderer.startColor = currentColor;
                lineRenderer.endColor = currentColor;

                yield return null;  // 等待下一帧
            }

            // 完全透明后销毁波纹
            Destroy(rippleObject);
            radius += ringSpacing;  // 增加圆环的半径
        }

        isRippleActive = false;  // 允许下次点击
    }
}
