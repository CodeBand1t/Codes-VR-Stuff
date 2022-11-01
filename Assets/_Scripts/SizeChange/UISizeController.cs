using UnityEngine;

namespace _Scripts.SizeChange
{
    public class UISizeController : MonoBehaviour
    {
        [SerializeField] private float maxSizeIncreasePercentage;
    
        [Space(10)]
        [SerializeField] private Transform sizeInfluencerTransform;

        private Transform _thisTransform;
        private float _baseScale;

        // Start is called before the first frame update
        void Start()
        {
            _thisTransform = transform;
            _baseScale = _thisTransform.localScale.x;
        }

        private void Update()
        {
            UpdateScale();
        }

        void UpdateScale()
        {
            if (sizeInfluencerTransform != null)
                _thisTransform.localScale = (Vector3.one * _baseScale * sizeInfluencerTransform.lossyScale.x) * GetPercentEnlargement();
        }

        /// <summary>
        /// Calculates & Returns the percentage to increase UI size, based on the difference b/t baseScale and influencer current scale
        /// </summary>
        /// <returns></returns>
        float GetPercentEnlargement()
        {
            float sizeDifference;

            sizeDifference = (100 + maxSizeIncreasePercentage) / 100 - (sizeInfluencerTransform.localScale.x * (maxSizeIncreasePercentage / 100)); 
        
            return sizeDifference;
        }

        /// <summary>
        /// Same as above, except this will calculate based on the size difference b/t user and speaker
        /// </summary>
        /// <returns></returns>
        float GetPercentEnlargementBasedOnSizeDifference()
        {
            return 0;
        }
    }
}
