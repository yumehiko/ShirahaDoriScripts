using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.ShirahaDori
{
    public class KatanaDamage : System.IDisposable
    {
        public IReadOnlyReactiveProperty<float> Amount => amount;

        private readonly ReactiveProperty<float> amount = new ReactiveProperty<float>();
        private float damageUnit = 0.05f;
        private float healUnit = 0.001f;
        private CancellationTokenSource slipHealCancellationTokenSource;

        public void Dispose()
        {
            slipHealCancellationTokenSource?.Dispose();
        }

        public bool Damage()
        {
            amount.Value = Mathf.Min(amount.Value + damageUnit, 1.0f);
            return amount.Value >= 1.0f;
        }

        public async UniTaskVoid SlipHeal()
        {
            slipHealCancellationTokenSource = new CancellationTokenSource();
            while (!slipHealCancellationTokenSource.IsCancellationRequested)
            {
                Heal();
                await UniTask.Yield(slipHealCancellationTokenSource.Token);
            }
        }

        public void StopHeal()
        {
            slipHealCancellationTokenSource?.Cancel();
        }

        public void Reset()
        {
            amount.Value = 0.0f;
            slipHealCancellationTokenSource?.Dispose();
        }

        private void Heal()
        {
            amount.Value = Mathf.Max(amount.Value - healUnit, 0.0f);
        }
    }
}
