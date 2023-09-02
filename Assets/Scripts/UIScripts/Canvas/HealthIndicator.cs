using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace ChainsawMan
{
    public class HealthIndicator : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private HealthSystem health;

        void Update()
        {
            healthBar.fillAmount = health.CurrentHP / 200f;
        }
    }
}
