using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.SizeChange
{
    public interface ISizeController
    {
        /// <summary>
        /// Logic called on update to alter scale.
        /// Turned on/off by bool value(s)
        /// </summary>
        public void ScalingLogic();
        
        /// <summary>
        /// Call to start/stop continuous scaling
        /// </summary>
        /// <param name="enable"></param>
        public void SetContinuousScaling(bool enable);
        
        /// <summary>
        /// Lerps the rate of scaling to avoid jerky start & stops of scaling
        /// </summary>
        /// <param name="enable">bool to tell script to increase/decrease rate from/to zero</param>
        /// <returns></returns>
        public IEnumerator LerpContinuousScalingRate(bool enable);

        /// <summary>
        /// Set the target scale for scaling logic
        /// </summary>
        public void SetTargetScale(float target);

        /// <summary>
        /// Set duration (and therefore rate) of scaling.
        /// Is an approximation due to time spent increasing/decreasing rate from/to zero
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetScalingDurating(float newDuration);

        /// <summary>
        /// Calculates & sets the rate of scaling
        /// </summary>
        public void CalculateScalingCoefficient();
    }
}

