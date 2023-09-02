using System.Collections;
using System.Collections.Generic;
using ChainsawMan.PlayerClass;
using UnityEngine;

public class ChangeHitbox : MonoBehaviour
{
    [SerializeField] private GameObject airborneHeadHitbox;
    [SerializeField] private GameObject groundedHeadHitbox;
    
    private PlayerController _player;
    
    void Start()
    {
        _player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (_player.characterController.IsGrounded)
        {
            groundedHeadHitbox.SetActive(true);
            airborneHeadHitbox.SetActive(false);
        }
        else
        {
            groundedHeadHitbox.SetActive(false);
            airborneHeadHitbox.SetActive(true);
        }
    }
}
