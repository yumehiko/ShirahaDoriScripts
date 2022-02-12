using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace yumehiko.ShirahaDori
{
    public class KatanaDamageView : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetFillAmount(float amount)
        {
            image.fillAmount = amount;
        }
    }
}
