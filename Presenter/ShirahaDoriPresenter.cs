using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer.Unity;
using Cysharp.Threading.Tasks;
using yumehiko.Resident;

namespace yumehiko.ShirahaDori
{
    public class ShirahaDoriPresenter : IStartable
    {
        private readonly RoundPresenter roundPresenter;

        public ShirahaDoriPresenter(RoundPresenter roundPresenter)
        {
            this.roundPresenter = roundPresenter;
        }

        public void Start()
        {
            ShirahaDoriStart().Forget();
        }

        private async UniTaskVoid ShirahaDoriStart()
        {
            await roundPresenter.StartRound();
            await roundPresenter.StartRound();
            ShirahaDoriEnd();
        }

        private void ShirahaDoriEnd()
        {
            Debug.Log("終了。");
            //LoadManager.RequireLoadScene("Title");
        }
    }
}
