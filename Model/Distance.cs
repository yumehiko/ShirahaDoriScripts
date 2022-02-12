using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using VContainer;

namespace yumehiko.ShirahaDori
{
    public class Distance : System.IDisposable
    {
        public IReadOnlyReactiveProperty<int> Count => count;
        public bool LessThanBlock4 => count.Value <= 240;
        public bool LessThanBlock3 => count.Value <= 180;
        public bool LessThanBlock2 => count.Value <= 120;
        public bool LessThanBlock1 => count.Value <= 60;
        public bool IsZero => count.Value <= 0;

        private int maxCount = 1200;
        private readonly ReactiveProperty<int> count = new ReactiveProperty<int>();
        private Tween countTween;

        [Inject]
        public Distance()
        {

        }

        public void Dispose()
        {
            countTween?.Kill();
        }

        public void SetCount(int count = 4000)
        {
            maxCount = count;
            this.count.Value = count;
        }

        public UniTask StartCountDown(float duration, CancellationToken token)
        {
            countTween?.Kill();

            countTween = DOTween.To(() => count.Value,
                num => count.Value = num,
                endValue: 0,
                duration: duration)
                .SetEase(Ease.Linear);

            return countTween.ToUniTask(cancellationToken: token);
        }

        public UniTask StartCountUp(float duration, CancellationToken token)
        {
            countTween?.Kill();

            countTween = DOTween.To(() => count.Value,
                num => count.Value = num,
                endValue: maxCount,
                duration: duration)
                .SetEase(Ease.Linear);

            return countTween.ToUniTask(cancellationToken: token);
        }
    }
}
