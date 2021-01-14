using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private LayerMask shootableLayer;
    private GameObject _instantiatedBullet;

    public delegate void OnShoot();
    public static event OnShoot onShoot;

    public delegate void NoMoreBullets();
    public static event NoMoreBullets noMoreBullets;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, shootableLayer))
            { 
                Vector3 temp = hit.point;
                temp.x += 0.5f;
                targetPoint.position = temp;
                
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if(_instantiatedBullet && _instantiatedBullet.activeInHierarchy) // if bullet is flying right now - can not shoot;
            return;
        
        GameObject bullet = Pool.singleton.Get("Bullet");
        _instantiatedBullet = bullet;
        
        if (bullet)
        {
            bullet.transform.position = spawnPos.position;
            bullet.SetActive(true);
            Pool.singleton.pooledItems.Remove(bullet);
            
            onShoot?.Invoke();
            CheckForAmmo();
            
            if (bullet.TryGetComponent(out ParabolaController parabolaController))
                parabolaController.FollowParabola();
        }
    }

    private void CheckForAmmo()
    {
        if(Pool.singleton.Get("Bullet") == null)
            noMoreBullets?.Invoke();
            
    }
}
