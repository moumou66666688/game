using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed = 5f;

        private Animator animator;
        private Rigidbody2D rb;

        [Header("摇杆")]
        public Joystick joystick;  //摇杆

        public TilemapNavigator navigator;
        public Transform target;
        private List<Vector3> currentPath;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 dir = Vector2.zero;

            //优先用摇杆
            if (joystick != null && (Mathf.Abs(joystick.Horizontal) > 0.2f || Mathf.Abs(joystick.Vertical) > 0.2f))
            {
                dir = new Vector2(joystick.Horizontal, joystick.Vertical);
                Debug.Log($"[Joystick] Horizontal: {joystick.Horizontal}, Vertical: {joystick.Vertical}");

                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                    animator.SetInteger("Direction", dir.x > 0 ? 2 : 3);
                else
                    animator.SetInteger("Direction", dir.y > 0 ? 1 : 0);
            }
            else
            {
                //退而用键盘
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
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);
            rb.velocity = speed * dir;
        }

        public void OnCollect()
        {
            Debug.Log("玩家收集了一个碎片！");
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }
        }

        void OnDrawGizmos()
        {
            if (currentPath != null)
            {
                Gizmos.color = Color.blue;
                foreach (var point in currentPath)
                {
                    Gizmos.DrawSphere(point, 0.2f);
                }
            }
        }
    }
}
