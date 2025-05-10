using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class CopyPropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;
        private bool state = false;

        private Color curColor;
        private Color targetColor;

        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor.a = 1.0f;
            ///collectManager.Instance.jitan2 = true;
            if (collectManager.Instance.collecteFinish && collectManager.Instance.jitan1)
            {
                collectManager.Instance.exitDoorPrefab.SetActive(true);
                Debug.Log("³É¹¦ÏÔÊ¾");
                state = true;


            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor.a = 0.0f;
            collectManager.Instance.exitDoorPrefab.SetActive(false);
            //collectManager.Instance.jitan2 = false;
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
            Debug.Log("×´Ì¬£º" + state);
        }
    }
}
