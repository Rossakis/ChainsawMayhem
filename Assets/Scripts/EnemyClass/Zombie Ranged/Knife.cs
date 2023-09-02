using System;
using System.Collections;
using ChainsawMan;
using UnityEngine;

[RequireComponent(typeof(AnimationCurve))]
public class Knife : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float rotationSpeed;
    
    [Tooltip("The amount of time that the knife's box collider will be disabled since being thrown.")]
    [SerializeField] private float disabledTime;
    
    private BoxCollider2D collider2D;

    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        collider2D.enabled = false;//disable the collider
        StartCoroutine(EnableCollider());//enable the collider after a set amount of time
        
        if(gameObject != null)//fail safe, to make sure the knife is destroyed after being instantiated 
            Destroy(gameObject, 3.5f);
    }

    private void Update()
    {
        transform.Rotate(0,0, rotationSpeed * Time.deltaTime);
    }

    //Enable the collider few moments after the knife is instantiated, so that player's doesn't get immediately hit by it
    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(disabledTime);
        collider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<IDamage>().ApplyDamage(damage);//the player collider and the IDamage inheriting player class (PlayerHealth) are in different hierarchical positions 
            Destroy(gameObject);//destroy itself if player was damaged
        }
    }
}