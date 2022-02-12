using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using yumehiko.Resident.Control;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace yumehiko.ShirahaDori
{
    public class KatanaDamagePresenter : System.IDisposable
    {
        public bool IsBroke => model.Amount.Value >= 1.0f;

        private readonly KatanaDamage model;
        private readonly KatanaDamageView view;
        private readonly Distance distance;

        private CompositeDisposable disposables;
        private CancellationTokenSource ascendCancellationTokenSource;

        [Inject]
        public KatanaDamagePresenter(
            KatanaDamage model,
            KatanaDamageView view,
            Distance distance)
        {
            this.model = model;
            this.view = view;
            this.distance = distance;
        }

        public void Dispose()
        {
            disposables?.Dispose();
            ascendCancellationTokenSource?.Cancel();
            ascendCancellationTokenSource?.Dispose();
        }

        public void Initialize()
        {
            disposables?.Dispose();
            ascendCancellationTokenSource?.Cancel();
            ascendCancellationTokenSource?.Dispose();
            disposables = new CompositeDisposable();

            model.Amount
                .Subscribe(value => view.SetFillAmount(value))
                .AddTo(disposables);

            model.Reset();
        }

        public async UniTask StartAscend(float duration, CancellationToken token)
        {
            bool prevIsMaru = false;

            _ = ReactiveInput.OnMaru
                .Where(isTrue => isTrue)
                .Where(_ => !prevIsMaru)
                .Subscribe(_ => DamageToKatana(true))
                .AddTo(disposables);

            _ = ReactiveInput.OnPeke
                .Where(isTrue => isTrue)
                .Where(_ => prevIsMaru)
                .Subscribe(_ => DamageToKatana(false))
                .AddTo(disposables);

            model.SlipHeal().Forget();

            await distance.StartCountUp(duration, token);
            disposables?.Dispose();
            model.StopHeal();

            void DamageToKatana(bool isMaru)
            {
                prevIsMaru = isMaru;
                bool isBroke = model.Damage();
                if (isBroke)
                {
                    model.StopHeal();
                }
            }
        }
    }
}
