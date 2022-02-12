using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace yumehiko.ShirahaDori
{
    public class RoundUI : MonoBehaviour
    {
        [SerializeField] private Text roundText;
        [SerializeField] private Text resultCallText;
        [SerializeField] private List<Text> results;

        public UniTask CallRoundTitle(int round, CancellationToken token)
        {
            Sequence sequence = DOTween.Sequence();

            roundText.text = $"Round {round}";
            roundText.enabled = true;
            sequence.Append(roundText.DOFade(0.0f, 0.0f));
            sequence.Append(roundText.DOFade(1.0f, 0.5f));
            sequence.AppendInterval(1.0f);
            sequence.Append(roundText.DOFade(0.0f, 1.0f));
            sequence.SetLink(gameObject);
            return sequence.ToUniTask(cancellationToken: token);
        }

        public UniTask CallResult(string result, CancellationToken token)
        {
            Sequence sequence = DOTween.Sequence();

            resultCallText.text = $"{result}";
            resultCallText.enabled = true;
            sequence.Append(resultCallText.DOFade(1.0f, 0.0f));
            sequence.AppendInterval(1.0f);
            sequence.Append(resultCallText.DOFade(0.0f, 1.0f));
            sequence.SetLink(gameObject);
            return sequence.ToUniTask(cancellationToken: token);
        }

        public void SetResult(int id, IResult result)
        {
            results[id].text = result.ToString();
        }
    }
}
