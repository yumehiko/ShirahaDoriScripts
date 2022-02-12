using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace yumehiko.ShirahaDori
{
    public class Catcher : MonoBehaviour
    {
        public float InitY { get; private set; }
        public float HandOffset => handOffset;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float handOffset;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite slashedSprite;
        private float skyPoint;
        private Tween tween;

        private void Awake()
        {
            InitY = transform.position.y;
        }

        public void Initialize(float skyPoint)
        {
            this.skyPoint = skyPoint;
            tween?.Kill();
            transform.position = new Vector3(transform.position.x, InitY);
            spriteRenderer.sprite = normalSprite;
        }

        public void StartAscend(float duration)
        {
            tween = transform.DOMoveY(skyPoint, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void BreakKatana()
        {
            tween.Kill();
            tween = transform.DOMoveY(InitY, 1.0f)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void FailureCatch()
        {
            spriteRenderer.sprite = slashedSprite;
        }
    }
}
