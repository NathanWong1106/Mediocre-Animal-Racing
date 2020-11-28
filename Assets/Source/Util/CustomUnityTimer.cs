using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Util
{
    /// <summary>
    /// Timer that can be instantiated in Unity
    /// </summary>
    public class CustomUnityTimer : MonoBehaviour
    {
        public event Action<float> OnTimeStepInterval;
        public event Action OnTimerFinish;
        public float Milliseconds;
        public float TimeStep;

        /// <summary>
        /// Initializes the timer with the given parameters
        /// </summary>
        /// <param name="milliseconds">Amount of time the timer will be run in milliseconds</param>
        /// <param name="timeStep">Interval in milliseconds where the OnTimeStepInterval Action will be called</param>
        public void Initialize(float milliseconds, float timeStep, Action<float> onTimeStepInterval = null, Action onTimerFinish = null)
        {
            this.Milliseconds = milliseconds;
            this.TimeStep = timeStep;
            this.OnTimeStepInterval += onTimeStepInterval;
            this.OnTimerFinish += onTimerFinish;
        }

        public void StartTimer()
        {
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(Converter.MillisecondsToSeconds(TimeStep));
            this.Milliseconds -= TimeStep;

            if (OnTimeStepInterval != null)
            {
                OnTimeStepInterval.Invoke(Milliseconds);
            }
            if (Milliseconds <= 0)
            {
                if(OnTimerFinish != null) OnTimerFinish.Invoke();
                Destroy(this);
            }
            else
            {
                StartCoroutine("Timer");
            }
        }
    }
}