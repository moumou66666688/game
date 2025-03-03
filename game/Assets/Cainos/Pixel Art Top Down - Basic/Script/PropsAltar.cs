﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;

        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("显示为进入");
            targetColor.a = 1.0f;
            if (collectManager.Instance.collecteFinish)
            {
                collectManager.Instance.exitDoorPrefab.SetActive(true);
                Debug.Log("成功显示");


            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor.a = 0.0f;
            collectManager.Instance.exitDoorPrefab.SetActive(false);
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
            
        }
    }
}
