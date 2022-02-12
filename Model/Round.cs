using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace yumehiko.ShirahaDori
{
    public class Round
    {
        public int RoundCount { get; private set; } = 0;
        private List<IResult> results = new List<IResult>();

        public int StartNextRound()
        {
            RoundCount++;
            return RoundCount;
        }

        public IResult RecordResult(float time)
        {
            var result = new TimeResult(time);
            results.Add(result);
            return result;
        }

        public IResult RecordFailureResult()
        {
            var result = new FailureResult();
            results.Add(result);
            return result;
        }
    }
}
