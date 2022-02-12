using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using VContainer;

namespace yumehiko.ShirahaDori
{
    public class RoundPresenter : System.IDisposable
    {
        private readonly Round round;
        private readonly RoundUI roundCallUI;
        private readonly KatanaCatchPresenter katanaCatchPresenter;
        private readonly KatanaDamagePresenter katanaDamagePresenter;
        private readonly ShirahaDoriAnimation shirahaDoriAnimation;

        private readonly float fallDuration = 10.0f;
        private readonly float ascendDuration = 10.0f;

        private CancellationTokenSource roundCancellationTokenSource;
        private CancellationTokenSource ascendCancellationTokenSource;
        private System.IDisposable successDisposable;

        [Inject]
        public RoundPresenter(
            Round round,
            RoundUI roundCallUI,
            KatanaCatchPresenter katanaCatchPresenter,
            KatanaDamagePresenter katanaDamagePresenter,
            ShirahaDoriAnimation shirahaDoriAnimation)
        {
            this.round = round;
            this.roundCallUI = roundCallUI;
            this.katanaCatchPresenter = katanaCatchPresenter;
            this.katanaDamagePresenter = katanaDamagePresenter;
            this.shirahaDoriAnimation = shirahaDoriAnimation;
        }

        public void Dispose()
        {
            successDisposable?.Dispose();
            roundCancellationTokenSource?.Dispose();
            ascendCancellationTokenSource?.Dispose();
        }

        public async UniTask StartRound()
        {
            int roundCount = round.StartNextRound();
            Dispose();
            katanaCatchPresenter.Initialize();
            katanaDamagePresenter.Initialize();
            shirahaDoriAnimation.Initialize();
            shirahaDoriAnimation.RoundStart();
            roundCancellationTokenSource = new CancellationTokenSource();
            await roundCallUI.CallRoundTitle(roundCount, roundCancellationTokenSource.Token);

            bool fallSuccessed = await Fall();
            if (!fallSuccessed)
            {
                return;
            }
            await Ascend();
        }

        private async UniTask<bool> Fall()
        {
            shirahaDoriAnimation.StartFall(fallDuration);
            bool isSuccessed = await katanaCatchPresenter.StartFall(fallDuration);
            if (!isSuccessed)
            {
                await FailureCatch();
            }
            return isSuccessed;
        }

        private async UniTask Ascend()
        {
            shirahaDoriAnimation.StartAscend(ascendDuration);
            float recordStartTime = Time.time;
            ascendCancellationTokenSource = new CancellationTokenSource();

            UniTask ascendTask = katanaDamagePresenter.StartAscend(ascendDuration, ascendCancellationTokenSource.Token);
            UniTask brokeTask = UniTask.WaitUntil(() => katanaDamagePresenter.IsBroke, cancellationToken: ascendCancellationTokenSource.Token);
            await UniTask.WhenAny(ascendTask, brokeTask);
            ascendCancellationTokenSource.Cancel();
            if (katanaDamagePresenter.IsBroke)
            {
                await Success(recordStartTime);
            }
            else
            {
                await FailureBreak();
            }
        }

        private void EndRound()
        {
            Debug.Log("Round End.");
        }

        private async UniTask Success(float recordStartTime)
        {
            shirahaDoriAnimation.Success();
            await roundCallUI.CallResult("大成功！", roundCancellationTokenSource.Token);
            float recordTime = Time.time - recordStartTime;
            IResult result = round.RecordResult(recordTime);
            roundCallUI.SetResult(round.RoundCount - 1, result);
            EndRound();
        }

        private async UniTask FailureCatch()
        {
            shirahaDoriAnimation.FailureCatch();
            await roundCallUI.CallResult("大失敗！", roundCancellationTokenSource.Token);
            var result = round.RecordFailureResult();
            roundCallUI.SetResult(round.RoundCount - 1, result);
            EndRound();
        }

        private async UniTask FailureBreak()
        {
            shirahaDoriAnimation.FailureBreak();
            await roundCallUI.CallResult("大失敗！", roundCancellationTokenSource.Token);
            var result = round.RecordFailureResult();
            roundCallUI.SetResult(round.RoundCount - 1, result);
            EndRound();
        }
    }
}
