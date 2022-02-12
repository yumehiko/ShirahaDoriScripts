using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.ShirahaDori
{
    public class DistanceView : MonoBehaviour, IDisposable
    {
        [SerializeField] private List<Image> gaugeBlocks;
        [SerializeField] private Text text;
        [SerializeField] private Color gaugeDeafultColor;
        [SerializeField] private Color gaugeInactiveColor;
        [SerializeField] private Color gaugeActiveColor;
        private IDisposable countDisposable;

        public void Dispose()
        {
            countDisposable?.Dispose();
        }

        public void Initialize()
        {
            foreach(Image block in gaugeBlocks)
            {
                block.color = gaugeDeafultColor;
            }
        }

        public void SubscribeAmount(IReadOnlyReactiveProperty<int> distanceCount)
        {
            countDisposable = distanceCount
                .Subscribe(value => text.text = $"{value}");
        }

        public async UniTaskVoid SubscribeBlockGauge(Distance distance, CancellationToken token)
        {
            await UniTask.WaitUntil(() => distance.LessThanBlock4, cancellationToken: token);
            gaugeBlocks[0].color = gaugeActiveColor;
            await UniTask.WaitUntil(() => distance.LessThanBlock3, cancellationToken: token);
            gaugeBlocks[0].color = gaugeInactiveColor;
            gaugeBlocks[1].color = gaugeActiveColor;
            await UniTask.WaitUntil(() => distance.LessThanBlock2, cancellationToken: token);
            gaugeBlocks[1].color = gaugeInactiveColor;
            gaugeBlocks[2].color = gaugeActiveColor;
            await UniTask.WaitUntil(() => distance.LessThanBlock1, cancellationToken: token);
            gaugeBlocks[2].color = gaugeInactiveColor;
            gaugeBlocks[3].color = gaugeActiveColor;
            await UniTask.WaitUntil(() => distance.IsZero, cancellationToken: token);
            gaugeBlocks[3].color = gaugeInactiveColor;
        }
    }
}
