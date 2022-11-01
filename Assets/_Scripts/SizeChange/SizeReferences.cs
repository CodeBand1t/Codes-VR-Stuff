using UnityEngine;

namespace _Scripts.SizeChange
{
    public class SizeReferences : MonoBehaviour
    {
        public static SizeReferences Instance;
        
        [Header("SFX Clips")]
        [SerializeField] private AudioClip scaleDownClip;
        [SerializeField] private AudioClip scaleUpClip;
        
        [Header("Camera Effects")] 
        [SerializeField] private CameraModalController cameraModalController;

        private void Awake()
        {
            Instance = this;
        }

        public AudioClip GetScaleUpClip() { return scaleUpClip; }
        public AudioClip GetScaleDownClip() { return scaleDownClip; }

        public CameraModalController GetCameraModalController() { return cameraModalController; }
    }
}
