using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace yumehiko.ShirahaDori
{
    public class Slasher : MonoBehaviour
    {
        public float InitY { get; private set; }
        private Tween tween;
        private float targetY;

        private void Awake()
        {
            InitY = transform.position.y;
        }

        public void Initialize(float targetY)
        {
            this.targetY = targetY;
            tween?.Kill();
            transform.position = new Vector3(transform.position.x, InitY, transform.position.z);
        }


        public void StartFall(float duration)
        {
            tween = transform.DOMoveY(targetY, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void StartAscend(float duration)
        {
            tween.Kill(true);
            tween = transform.DOMoveY(InitY, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void Ryoudan()
        {
            //TODO:一刀両断アニメーション。
        }
    }
}
