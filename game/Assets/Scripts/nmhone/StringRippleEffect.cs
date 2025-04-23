using UnityEngine;

public class StringRippleEffect : MonoBehaviour
{
    private bool isRippleActive = false;  // ��ֹ�ظ����
    private float rippleSpeed = 20f;      // ������չ�ٶȣ��ǳ����ٵ���չ��
    private float maxRadius = 4.5f;      // ���������뾶���Ŵ��ƣ�
    private int numPoints = 100;          // ÿ��Բ���ĵ���
    private float ringSpacing = 0.6f;     // ����ÿ��Բ��֮��ļ����ʹ���Ƹ���
    private float fadeTime = 0.1f;        // ������ʧ��ʱ�䣨�ǳ����ٵ���ʧ��
    private float ringExpandTime = 0.05f;  // ÿ��Բ����չʱ�䣨�������չ��

    // �����������ʱ����
    private void OnMouseDown()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ��ȡ���λ��
        clickPosition.z = 0f;  // ȷ�����λ���� 2D ƽ����
        Debug.Log("���ұ������");  // �������
        if (!isRippleActive)  // ��ֹ�ظ����
        {
            StartCoroutine(ShowRippleEffect(clickPosition));  // �ڵ��λ����ʾ����Ч��
        }
    }

    // ��ʾˮ����Ч��
    private System.Collections.IEnumerator ShowRippleEffect(Vector3 position)
    {
        isRippleActive = true;  // �����״̬

        // ��ʼ������Ȧ�İ뾶
        float radius = 0f;
        int maxCircles = 3;  // ���� 3 ��Բ������������Ȧ��

        // �������Բ��
        for (int i = 0; i < maxCircles; i++)
        {
            // ����ÿ���µ�Բ��
            GameObject rippleObject = new GameObject("RippleEffect" + i);
            LineRenderer lineRenderer = rippleObject.AddComponent<LineRenderer>();

            // ���� LineRenderer ����
            lineRenderer.startWidth = 0.1f;  // ��������ʼ���
            lineRenderer.endWidth = 0.1f;    // �����ƽ������
            lineRenderer.material = new Material(Shader.Find("Unlit/Transparent"));
            rippleObject.transform.position = new Vector3(position.x, position.y, 10f);  // ȷ�� Z ֵ�㹻���������
            lineRenderer.positionCount = numPoints;

            // ����ˮīɫ������ɫ����͸����ˮī��ɫ������͸������ʹˮ���Ƹ���
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.clear, 0.0f), new GradientColorKey(new Color(0.2f, 0.2f, 0.2f), 1.0f) },  // ˮīɫ
                new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(0.3f, 1.0f) }  // ����͸����ʹ���Ƹ���
            );
            lineRenderer.colorGradient = gradient;
            lineRenderer.sortingLayerName = "Foreground";  // ��ˮ���Ʒŵ� "Foreground" ��
            lineRenderer.sortingOrder = 10;  // ������ʾ�������棬����ֵԽ��Խ���ϲ�

            // ˮ������չ
            float expandTime = 0f;
            while (expandTime < ringExpandTime)
            {
                // ʹ�� Mathf.Lerp ��ƽ����չ������С�뾶���
                float currentRadius = Mathf.Lerp(0f, radius + ringSpacing, expandTime / ringExpandTime);  // ������չ
                // ȷ��������չ�����뾶
                currentRadius = Mathf.Min(currentRadius, maxRadius);
                // ����ÿ��Բ���ĵ�
                for (int j = 0; j < numPoints; j++)
                {
                    float angle = j * Mathf.PI * 2f / numPoints;
                    Vector3 ripplePoint = position + new Vector3(Mathf.Cos(angle) * currentRadius, Mathf.Sin(angle) * currentRadius, 0);
                    lineRenderer.SetPosition(j, ripplePoint);
                }
                expandTime += Time.deltaTime;  // ������չʱ��
                yield return null;  // �ȴ���һ֡
            }

            // ��չ��ɺ�ʼ����
            float fadeStartTime = Time.time;

            // ������ʧ����
            while (Time.time < fadeStartTime + fadeTime)
            {
                // ���㵱ǰ��ʧ��͸����
                float fadeProgress = (Time.time - fadeStartTime) / fadeTime;
                Color currentColor = lineRenderer.startColor;
                currentColor.a = Mathf.Lerp(1.0f, 0.0f, fadeProgress);  // ����ȫ��͸������ȫ͸��
                lineRenderer.startColor = currentColor;
                lineRenderer.endColor = currentColor;

                yield return null;  // �ȴ���һ֡
            }

            // ��ȫ͸�������ٲ���
            Destroy(rippleObject);
            radius += ringSpacing;  // ����Բ���İ뾶
        }

        isRippleActive = false;  // �����´ε��
    }
}
