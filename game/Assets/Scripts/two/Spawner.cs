using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pipes prefab;
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;
    public float verticalGap = 3f;
    public int maxPipes = 10;  // 最大管道数量

    private int pipesSpawned = 0;  // 已生成的管道数量

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        if (pipesSpawned >= maxPipes)  // 检查是否已达到最大管道数量
        {
            return;  // 不再生成管道，但游戏继续进行
        }

        Pipes pipes = Instantiate(prefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        pipes.gap = verticalGap;
        pipesSpawned++;  // 增加已生成的管道数量

        // 当生成完第10个管道后，延迟一段时间再结束游戏
        if (pipesSpawned == maxPipes)
        {
            Invoke(nameof(EndGame), 5f);  // 延迟5秒结束游戏
        }
    }

    private void EndGame()
    {
        GameManager0.Instance.GameOver();  // 结束游戏
    }

    // 新增方法，用于重置管道生成计数
    public void Reset()
    {
        pipesSpawned = 0;  // 重置已生成的管道数量
        CancelInvoke(nameof(EndGame));  // 取消延迟结束游戏的调用
    }
}