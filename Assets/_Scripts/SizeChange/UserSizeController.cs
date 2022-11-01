using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Scripts.SizeChange
{
    public class UserSizeController : MonoBehaviour, ISizeController
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

        // Internal Vars
        [Header("Interval Scaling")]
        [SerializeField] private float scaleCoefficient;

        private AudioSource _audioSource;

        private Transform _thisTransform;
        private bool _scaling = false, _continuousScaling = false;
        private Vector3 _startSize, _endSize;
        private float _currentContinuousScalingRate  = 0;
        private float _currentScale, _scaleRatio;
        private float _timer;

        private float _continuousScalingCoefficient;

        private UnityEvent _endScaleEvents;
        
        private CameraModalController _cameraModalController;
        private VRRayVisualController _vrRayVisualController;
        private ViewCorrectionVR _viewCorrectionVR;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _thisTransform = transform;
        }

        private void Start()
        {
            _cameraModalController = SizeReferences.Instance.GetCameraModalController();
            _vrRayVisualController = FindObjectOfType<VRRayVisualController>();
            _viewCorrectionVR = GetComponent<ViewCorrectionVR>();
        }

        // Update is called once per frame
        void Update()
        {
            ScalingLogic();
        }
        
        public void StartLerpScaling()
        {
            StartCoroutine(LerpScale());
        }
        
        IEnumerator LerpScale()
        {
            
            if (_cameraModalController != null)
                _cameraModalController.SetShrinkModal(true);
            
            float t = 0;
            float endScale = Mathf.Clamp(lerpTargetScale, 0.05f, 100f);
            float startScale = transform.localScale.x;
            float currentScale = startScale;
            
            //_viewCorrectionVR.ResetView();
            
            _audioSource.clip = startScale > endScale ? SizeReferences.Instance.GetScaleDownClip() 
                : SizeReferences.Instance.GetScaleUpClip();
            _audioSource.Play();

            while (t < lerpScalingDuration)
            {
                t += Time.deltaTime;
                t = Mathf.Clamp(t, 0, lerpScalingDuration);
                currentScale = Mathf.Lerp(startScale, endScale, animationCurve.Evaluate(t / lerpScalingDuration));
                
                Vector3 vectorScale = Vector3.one * currentScale;

                transform.localScale = vectorScale;

                if (_vrRayVisualController != null)
                {
                    _vrRayVisualController.UpdateRayVisualLineWidth();
                }
                
                yield return new WaitForEndOfFrame();
            }
            
            if (_cameraModalController != null)
                _cameraModalController.SetShrinkModal(false);
            
            _audioSource.Stop();
        }

        public void ScalingLogic()
        {
            // if (scaling)
            // {
            //     t += Time.deltaTime / rescaleDuration;
            //
            //     transform.localScale = Vector3.Lerp(startSize, endSize, t);
            //     if (t >= rescaleDuration)
            //     {
            //         scaling = false;
            //     }
            // }

            CheckToTriggerEndScaleEvent();

            if (_continuousScaling)
            {
                transform.localScale += Vector3.one * Time.deltaTime * _currentContinuousScalingRate;
            }
        }

        public void SetContinuousScaling(bool enable)
        {
            _continuousScaling = enable;
            CalculateScalingCoefficient();
            StartCoroutine(LerpContinuousScalingRate(enable));
        
            // TODO add back later
            if (_cameraModalController != null)
                _cameraModalController.SetShrinkModal(enable);

            if (enable)
            {
                _audioSource.clip = SizeReferences.Instance.GetScaleDownClip();
                _audioSource.Play();
            }
            else 
                _audioSource.Stop();
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

        void CheckToTriggerEndScaleEvent()
        {
            if (_continuousScalingCoefficient < 0 && transform.localScale.x <= targetScale)
                TriggerEndScaleEvent();
            else if (_continuousScalingCoefficient > 0 && transform.localScale.x >= targetScale)
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

        public bool GetIsScaling()
        {
            return _scaling;
        }
    }
}
