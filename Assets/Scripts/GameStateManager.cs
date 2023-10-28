using System;
using UnityEngine;

namespace ChainsawMan
{
    public class GameStateManager : MonoBehaviour
    {
        public GameStates State;
        public static GameStateManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            Instance.State = GameStates.None;
        }

        private void Update()
        {
                   
            switch (Instance.State)
            {
                case GameStates.SplashScreen:
                    SplashScreenManager.Instance.UpdateSplashScreen();
                    break;
                case GameStates.Gameplay:
                    Gameplay();
                    break;
            }
        }

        public void SwitchState(GameStates state)
        {
            Instance.State = state;
        }
        
        private void Gameplay()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
}