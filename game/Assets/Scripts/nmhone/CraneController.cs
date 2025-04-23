using UnityEngine;

public class CraneController : MonoBehaviour
{
    public float jumpHeight = 0.5f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    public void Jump()
    {
        StartCoroutine(JumpRoutine());
    }

    private System.Collections.IEnumerator JumpRoutine()
    {
        transform.position += Vector3.up * jumpHeight;
        yield return new WaitForSeconds(0.5f);
        transform.position = originalPos;
    }
}