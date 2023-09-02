using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    public class OptionsMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject optionsPanel;

        private void Start()
        {
            optionsPanel.SetActive(false);
        }

        public void OpenMenu()
        {
            optionsPanel.SetActive(true);
        }
        
        public void CloseMenu()
        {
            optionsPanel.SetActive(false);
        }
        
    }
}
