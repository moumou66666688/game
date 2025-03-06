using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//collectManager
public class countMember : MonoBehaviour
{
    // Start is called before the first frame update
    public string collectibleID;
    public UnityEvent onCollect;

    private bool isCollected = false;

    

    void Start()
    {
        //检查是否收集
        isCollected = collectManager.Instance.IsCollected(collectibleID);
        gameObject.SetActive(!isCollected);
    }
    //player trigger with SiNan
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"触发对象：{other.name}");
        if (isCollected || !other.CompareTag("Player")) return;
        
        Debug.Log("玩家进入触发区域");  //进行收集
        Collect();
    }
    
    
    /////////////////////////////////////////////////////////////////////////////////////////////////
    
    void Collect()
    {
        Debug.Log("收集到碎片："+collectibleID);

        isCollected = true;
        
        // 播放音效
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            Debug.Log($"AudioSource 状态：{GetComponent<AudioSource>().isActiveAndEnabled}");
            audio.Play();
        }

        onCollect.Invoke();
        collectManager.Instance.CollectFragment(collectibleID); // 传递正确的ID
        gameObject.SetActive(false); 


    }
    public void Reset()
    {
        isCollected = false; // 改成没收集
        gameObject.SetActive(true); // 重新激活碎片
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // 显示触发区域
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
