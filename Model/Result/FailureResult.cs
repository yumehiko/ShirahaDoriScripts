using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.ShirahaDori
{
    public class FailureResult : IResult
    {
        public override string ToString()
        {
            return "大失敗";
        }
    }
}
