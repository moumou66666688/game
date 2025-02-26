using UnityEngine;
//背景滚动
public class Parallax : MonoBehaviour
{
    public float animationSpeed = 0f;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }

}
