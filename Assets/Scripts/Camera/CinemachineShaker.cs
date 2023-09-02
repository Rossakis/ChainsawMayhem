using Cinemachine;
using UnityEngine;

namespace ChainsawMan
{
    /// <summary>
    /// Class for managing the shaking effect of a Cinemachine Camera
    /// </summary>
    public class CinemachineShaker : MonoBehaviour
    {
        public static CinemachineShaker Instance { get; private set; }
            
        private CinemachineVirtualCamera vCamera;
        private float shakeTimer;//how long the camera will keep shaking
        private CinemachineBasicMultiChannelPerlin cameraPerlinChannel; //the player Camera's basic multi channel perlin, responsible for the shaking magnitudes

        private float shakeTimerTotal;
        private float startingIntensity;//a value need when Lerping the intensity/amplitudeGain variable of the camera
        private void Awake()
        {
            if (Instance == null) 
                Instance = this;
            
            vCamera = GetComponent<CinemachineVirtualCamera>();
            cameraPerlinChannel = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void ShakeCamera(float intensity, float time)
        {
            cameraPerlinChannel.m_AmplitudeGain = intensity;
            startingIntensity = intensity;
            
            shakeTimerTotal = time;
            shakeTimer = time;
        }

        private void Update()
        {
            if(shakeTimer > 0)
                shakeTimer -= Time.deltaTime;
            
            if (shakeTimer <= 0f)//if timer has ended
            {
                cameraPerlinChannel.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - shakeTimer / 1);//a smoother shaking transition 
            }
        }
    }
}
