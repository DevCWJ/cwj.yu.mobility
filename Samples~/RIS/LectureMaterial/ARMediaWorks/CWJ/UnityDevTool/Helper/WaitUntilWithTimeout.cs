using System;

using UnityEngine;

namespace CWJ
{
    public sealed class WaitUntilWithTimeout : CustomYieldInstruction
    {
        private Func<bool> predicate = null;
        private Func<bool> forceStopPredicate = null;

        public float limitTime { get; private set; }
        public float remainingTime { get; private set; }
        public bool isTimeout { get; private set; }

        public readonly bool hasForceStopPredicate;
        public bool isForcedStop { get; private set; }

        private bool isWaitEnd;

        public bool isSucceed => isWaitEnd && !isTimeout && !isForcedStop;

        private readonly bool isFrameInterval;
        private float timeInterval;

        public WaitUntilWithTimeout(Func<bool> predicate, float timeout, Func<bool> forceStopPredicate = null, float timeInterval = 0)
        {
            this.predicate = predicate;
            this.forceStopPredicate = forceStopPredicate;
            hasForceStopPredicate = forceStopPredicate != null;
            remainingTime = limitTime = timeout;
            isTimeout = false;
            isForcedStop = false;
            isWaitEnd = false;
            this.timeInterval = timeInterval;
            isFrameInterval = timeInterval == 0;
        }

        public void ChangeLimitTime(float timeout)
        {
            if (isWaitEnd)
                ResetCondition();

            limitTime = timeout;
        }

        public void ResetCondition()
        {
            remainingTime = limitTime;
            isTimeout = false;
            isForcedStop = false;
            isWaitEnd = false;
        }

        private bool CheckIsWaitOver()
        {
            if ((remainingTime -= (isFrameInterval ? Time.deltaTime : timeInterval)) <= 0f)
                return isTimeout = true;
            if (hasForceStopPredicate && forceStopPredicate.Invoke())
                return isForcedStop = true;
            return predicate();
        }

        public override bool keepWaiting
        {
            get
            {
                return !(isWaitEnd = CheckIsWaitOver());
            }
        }
    }
}