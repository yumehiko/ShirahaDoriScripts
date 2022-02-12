using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cinemachine;

namespace yumehiko.ShirahaDori
{
    public class ShirahaDoriAnimation : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera normalCamera;
        [SerializeField] private CinemachineVirtualCamera slasherCamera;
        [SerializeField] private Slasher slasher;
        [SerializeField] private Catcher catcher;

        public void Initialize()
        {
            slasherCamera.Priority = 10;
            normalCamera.Priority = 12;
            slasher.Initialize(catcher.InitY + catcher.HandOffset);
            catcher.Initialize(slasher.InitY - catcher.HandOffset);
        }

        public void RoundStart()
        {
            slasherCamera.Priority = 12;
            normalCamera.Priority = 10;
        }

        public void StartFall(float duration)
        {
            slasher.StartFall(duration);
        }

        public void StartAscend(float duration)
        {
            slasher.StartAscend(duration);
            catcher.StartAscend(duration);
        }

        public void Success()
        {
            catcher.BreakKatana();
            slasherCamera.Priority = 10;
            normalCamera.Priority = 12;
        }

        public void FailureCatch()
        {
            catcher.FailureCatch();
        }

        public void FailureBreak()
        {
            //TODO:破壊失敗アニメーション
        }

    }
}
