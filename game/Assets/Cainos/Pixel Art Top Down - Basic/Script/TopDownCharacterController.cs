using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }

        //处理收集逻辑
        public void OnCollect()
        {
            Debug.Log("玩家收集了一个碎片！");            

            // 示例：播放音效
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                Debug.Log("播放音效......");
                audio.Play();
            }
            else
            {
                Debug.Log($"AudioSource 状态：{GetComponent<AudioSource>().isActiveAndEnabled}");
                //Debug.Log("没有音效");
            }
        }
    }
}
