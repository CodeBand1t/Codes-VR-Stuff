using System;
using UnityEngine;

namespace _Scripts.SizeChange
{
    public class SizeManager : MonoBehaviour
    {
        public static SizeManager Instance;
        
        [Header("Size Changers")]  
        [SerializeField] private UserSizeController userSizeController;
        [SerializeField] private CharacterSizeController characterSizeController;

        private void Awake()
        {
            Instance = this;
        }

        // User
        
        /// <summary>
        /// Set the target scale for User
        /// </summary>
        public void SetUserTargetScale(float target)
        {
            userSizeController.SetTargetScale(target);
        }

        /// <summary>
        /// Set duration (and therefore rate) of scaling for User
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetUserScalingDurating(float newDuration)
        {
            userSizeController.SetScalingDurating(newDuration);
        }

        /// <summary>
        /// Start/End User scaling
        /// </summary>
        /// <param name="enable">start if true,end if false</param>
        public void SetUserContinuousScaling(bool enable)
        {
            Debug.Log($"Setting User Scaling {enable}");
            userSizeController.SetContinuousScaling(enable);
        }
        
        /// <summary>
        /// Set the target lerp scale for User
        /// </summary>
        public void SetUserLerpTargetScale(float target)
        {
            userSizeController.SetLerpTargetScale(target);
        }

        /// <summary>
        /// Set duration of lerp scaling for Character
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetUserLerpDuration(float newDuration)
        {
            userSizeController.SetLerpScalingDurating(newDuration);
        }
        
        /// <summary>
        /// Start/End Character lerp scaling
        /// </summary>
        public void StartUserLerpScaling()
        {
            userSizeController.StartLerpScaling();
        }
        
        // Character
        
        /// <summary>
        /// Set the target scale for Character
        /// </summary>
        public void SetCharacterTargetScale(float target)
        {
            characterSizeController.SetTargetScale(target);
        }

        /// <summary>
        /// Set duration (and therefore rate) of scaling for Character
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetCharacterScalingDuration(float newDuration)
        {
            characterSizeController.SetScalingDurating(newDuration);
        }

        /// <summary>
        /// Start/End Character scaling
        /// </summary>
        /// <param name="enable">start if true,end if false</param>
        public void SetCharacterContinuousScaling(bool enable)
        {
            characterSizeController.SetContinuousScaling(enable);
        }
        
        /// <summary>
        /// Switch to/from scaling parts vs full body
        /// </summary>
        /// <param name="enable"></param>
        public void SetCharacterToPartScaling(bool enable) {
            characterSizeController.SetToPartScaling(enable);
        }
        
        /// <summary>
        /// Set the target lerp scale for Character
        /// </summary>
        public void SetCharacterLerpTargetScale(float target)
        {
            characterSizeController.SetLerpTargetScale(target);
        }

        /// <summary>
        /// Set duration of lerp scaling for Character
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetCharacterLerpDuration(float newDuration)
        {
            characterSizeController.SetLerpScalingDurating(newDuration);
        }

        /// <summary>
        /// Start/End Character lerp scaling
        /// </summary>
        public void StartCharacterLerpScaling()
        {
            characterSizeController.StartLerpScaling();
        }

        public void SetCharacterScalingSound(string clipName)
        {
            AudioClip newClip = clipName == "Grow" ? SizeReferences.Instance.GetScaleUpClip() 
                : clipName == "Shrink" ? SizeReferences.Instance.GetScaleDownClip() : null;

            if (newClip == null)
            {
                Debug.LogWarning($"Clip {clipName} is invalid");
                return;
            }
            
            characterSizeController.SetScalingSound(newClip);
        }
    }
}
