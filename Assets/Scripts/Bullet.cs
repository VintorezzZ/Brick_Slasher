using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[RequireComponent(typeof(ParabolaController))]
public class Bullet : MonoBehaviour
{
    private ParabolaController _parabolaController;
    
    [SerializeField] private float explosionForce = 20000;

    public delegate void OnBulletDestroyed(GameObject nothing);
    public static event OnBulletDestroyed onBulletDestroyed;

    void Awake()
    {
        _parabolaController = GetComponent<ParabolaController>();
        _parabolaController.ParabolaRoot = GameObject.FindGameObjectWithTag("ParabolaRoot");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6) // Shootable layer
        {
            if (other.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, 1, 1);
            }
        }
    }

    private void OnDisable()
    {
        onBulletDestroyed?.Invoke(gameObject);
    }
}
