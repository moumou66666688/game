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
        //����Ƿ��ռ�
        isCollected = collectManager.Instance.IsCollected(collectibleID);
        gameObject.SetActive(!isCollected);
    }
    //player trigger with SiNan
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"��������{other.name}");
        if (isCollected || !other.CompareTag("Player")) return;
        
        Debug.Log("��ҽ��봥������");  //�����ռ�
        Collect();
    }
    
    
    /////////////////////////////////////////////////////////////////////////////////////////////////
    
    void Collect()
    {
        Debug.Log("�ռ�����Ƭ��"+collectibleID);

        isCollected = true;
        
        // ������Ч
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            Debug.Log($"AudioSource ״̬��{GetComponent<AudioSource>().isActiveAndEnabled}");
            audio.Play();
        }

        onCollect.Invoke();
        collectManager.Instance.CollectFragment(collectibleID); // ������ȷ��ID
        gameObject.SetActive(false); 


    }
    public void Reset()
    {
        isCollected = false; // �ĳ�û�ռ�
        gameObject.SetActive(true); // ���¼�����Ƭ
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // ��ʾ��������
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
