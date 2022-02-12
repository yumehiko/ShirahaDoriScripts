using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using yumehiko.Resident.Control;
using VContainer;

namespace yumehiko.ShirahaDori
{
    public class KatanaCatchPresenter : System.IDisposable
    {
        private readonly Distance distance;
        private readonly DistanceView distanceView;

        private CancellationTokenSource fallCancellationTokenSource;
        private System.IDisposable catchInputDisposable;

        [Inject]
        public KatanaCatchPresenter(
            Distance distance,
            DistanceView distanceView)
        {
            this.distance = distance;
            this.distanceView = distanceView;
            distanceView.SubscribeAmount(distance.Count);
        }

        public void Dispose()
        {
            catchInputDisposable?.Dispose();
            fallCancellationTokenSource?.Dispose();
        }

        public void Initialize()
        {
            catchInputDisposable?.Dispose();
            fallCancellationTokenSource?.Dispose();
            distance.SetCount(4000);
            distanceView.Initialize();
        }

        public async UniTask<bool> StartFall(float duration)
        {
            fallCancellationTokenSource = new CancellationTokenSource();
            distanceView.SubscribeBlockGauge(distance, fallCancellationTokenSource.Token).Forget();

            bool isSuccessed = false;

            catchInputDisposable = ReactiveInput.OnMaru
                .Where(isTrue => isTrue)
                .Where(_ => distance.LessThanBlock4)
                .First()
                .Subscribe(_ =>
                {
                    isSuccessed = true;
                    fallCancellationTokenSource?.Cancel();
                });

            await distance.StartCountDown(duration, fallCancellationTokenSource.Token);
            catchInputDisposable?.Dispose();
            return isSuccessed;
        }
    }
}
