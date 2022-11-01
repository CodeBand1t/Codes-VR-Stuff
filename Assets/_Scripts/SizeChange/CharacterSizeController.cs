using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.SizeChange
{
    public class CharacterSizeController : MonoBehaviour
    {
        [Header("Lerp Scaling")]
        [SerializeField] private float lerpScalingDuration;
        [SerializeField] private float lerpTargetScale;
        [SerializeField] private AnimationCurve animationCurve;

        [Header("Continuous Scaling")]
        // [SerializeField] private float continuousScalingCoefficient;
        [SerializeField] private float scalingAdjustmentDuration = 1;
        [SerializeField] private float scalingDuration;
        [SerializeField] private float targetScale;

        [Header("Character Parts")] 
        [SerializeField] private Transform characterParentTransform;
        [SerializeField] private Transform[] characterPartsTransforms;

        [Header("Events")] 
        [SerializeField] private UnityEvent startScalingEvent;
        [SerializeField] private UnityEvent endScalingEvent;
        
        [Header("References")]
        [SerializeField] private AudioSource scalingAudioSource;

        private Transform _thisTransform;
        private bool scaling = false, _continuousScaling = false, _partScaling = false;
        private Vector3 _startSize, _endSize;
        private float _currentContinuousScalingRate  = 0;
        private float _currentScale, _scaleRatio;
        private float _timer;

        private float _continuousScalingCoefficient;

        private UnityEvent endScaleEvents;

        private void Awake()
        {
            _thisTransform = transform;
        }

        // Update is called once per frame
        void Update()
        {
            ScalingLogic();
        }

        public void ScalingLogic()
        {
            CheckToTriggerEndScaleEvent();

            if (_continuousScaling)
            {
                Vector3 scaleAmount = Vector3.one * Time.deltaTime * _currentContinuousScalingRate;
                
                if (_partScaling)
                {
                    foreach (Transform partTransform in characterPartsTransforms)
                    {
                        partTransform.localScale += scaleAmount;
                    }

                    return;
                }
                transform.localScale += scaleAmount;
            }
        }

        public void SetContinuousScaling(bool enable)
        {
            _continuousScaling = enable;
            CalculateScalingCoefficient();
            StartCoroutine(LerpContinuousScalingRate(enable));

            if (enable)
            {
                if (scalingAudioSource.clip == null) 
                    scalingAudioSource.clip = SizeReferences.Instance.GetScaleUpClip();
                scalingAudioSource.Play();
                startScalingEvent.Invoke();
                
                // if (scalingAudioSource.clip.name == "Grow2")
                //     CharacterSoundManager.Instance.PlayGrowthClip();
                // else 
                //     CharacterSoundManager.Instance.PlayShrinkClip();
            }
            else
            {
                scalingAudioSource.Stop();
                // CharacterSoundManager.Instance.StopClip();
            }
        }

        public void StartLerpScaling()
        {
            StartCoroutine(LerpScale());
        }

        public IEnumerator LerpContinuousScalingRate(bool enable)
        {
            float t = 0;
            float endRate = enable ? _continuousScalingCoefficient : 0;

            while (t < scalingAdjustmentDuration)
            {
                _currentContinuousScalingRate = Mathf.Lerp(_currentContinuousScalingRate, endRate, t);
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime;
            }
        }

        IEnumerator LerpScale()
        {
            bool tempPartScaling = _partScaling;
            
            float t = 0;
            float endScale = Mathf.Clamp(lerpTargetScale, 0.05f, 100);
            float startScale = tempPartScaling
                ? characterPartsTransforms[0].localScale.x
                : characterParentTransform.localScale.x;
            float currentScale = startScale;
            
            scalingAudioSource.clip = startScale > endScale ? SizeReferences.Instance.GetScaleDownClip()
                : SizeReferences.Instance.GetScaleUpClip();
            scalingAudioSource.Play();

            while (t < lerpScalingDuration)
            {
                t += Time.deltaTime;
                t = Mathf.Clamp(t, 0, lerpScalingDuration);
                currentScale = Mathf.Lerp(startScale, endScale, animationCurve.Evaluate(t / lerpScalingDuration));
                
                Vector3 vectorScale = Vector3.one * currentScale;
                if (tempPartScaling)
                    foreach (Transform partTransform in characterPartsTransforms)
                        partTransform.localScale = vectorScale;
                else
                    transform.localScale = vectorScale;
                
                yield return new WaitForEndOfFrame();
            }
            
            scalingAudioSource.Stop();
        }

        /// <summary>
        /// Set the target scale for scaling logic
        /// </summary>
        public void SetTargetScale(float target)
        {
            targetScale = target;
        }

        /// <summary>
        /// Set duration (and therefore rate) of scaling.
        /// Is an approximation due to time spent increasing/decreasing rate from/to zero
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetScalingDurating(float newDuration)
        {
            scalingDuration = newDuration;
        }
        
        /// <summary>
        /// Set duration of lerp scaling.
        /// Is an exact duration compared to continuous scaling
        /// </summary>
        /// <param name="newDuration">seconds for scale to take place</param>
        public void SetLerpScalingDurating(float newDuration)
        {
            lerpScalingDuration = newDuration;
        }

        /// <summary>
        /// Set target (end) scale for lerp scaling
        /// </summary>
        /// <param name="newTargetScale"></param>
        public void SetLerpTargetScale(float newTargetScale)
        {
            lerpTargetScale = newTargetScale;
        }

        void CheckToTriggerEndScaleEvent()
        {
            if (!_partScaling)
            {
                if (_continuousScalingCoefficient < 0 && transform.localScale.x <= targetScale)
                    TriggerEndScaleEvent();
                else if (_continuousScalingCoefficient > 0 && transform.localScale.x >= targetScale)
                    TriggerEndScaleEvent();

                return;
            }
            
            if (_continuousScalingCoefficient < 0 && characterPartsTransforms[0].localScale.x <= targetScale)
                TriggerEndScaleEvent();
            else if (_continuousScalingCoefficient > 0 && characterPartsTransforms[0].localScale.x >= targetScale)
                TriggerEndScaleEvent();

        }

        void TriggerEndScaleEvent()
        {
            //endScaleEvents.Invoke();
            SetContinuousScaling(false);
            //this.enabled = false;
        }

        public void CalculateScalingCoefficient()
        {
            // based on duration, current scale, & target scale
            _currentScale = _thisTransform.lossyScale.x;

            float scaleDifference = _currentScale - targetScale;

            _continuousScalingCoefficient = (scaleDifference / scalingDuration) * -1;
        }

        public void SetToPartScaling(bool setToPart)
        {
            _partScaling = setToPart;
        }

        public bool GetIsScaling()
        {
            return scaling;
        }

        public void SetScalingSound(AudioClip clip)
        {
            scalingAudioSource.clip = clip;
        }
    }
}
