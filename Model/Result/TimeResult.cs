using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace yumehiko.ShirahaDori
{
    public class TimeResult : IResult
    {
        private float recordedScore = 0.0f;

        public TimeResult(float time)
        {
            recordedScore = time;
        }

        public override string ToString()
        {
            return recordedScore.ToString();
        }
    }
}
