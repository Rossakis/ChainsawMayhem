using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChainsawMan
{
    ///Class for making World-Space UI elements that are shown to the player upon entering their trigger area (e.g. Player In-Game Controls Instructions) 
    public class TriggerAreaUI : MonoBehaviour
    {
        [Tooltip("Select this if this UI element should have animation, instead of a static image.")]
        [SerializeField] private bool animated;
        [SerializeField] private AnimationClip animationClip;
        
        private SpriteRenderer spriteRenderer;
        private Animator animator;//in case it needs to be animated
        private int animHash;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            
            if (animated)
            {
                animator = GetComponent<Animator>();
                animHash = Animator.StringToHash(animationClip.name);
                animator.Play(animHash);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                spriteRenderer.enabled = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
