using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePositionChecker : MonoBehaviour
{
    public delegate void CubeIsDead(GameObject cube);
    public static event CubeIsDead cubeIsDead;
    private bool died;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.localPosition.y < -2 || 
            transform.localPosition.z < -4 || 
            transform.localPosition.x < -12 ||
            transform.localPosition.x > 12) && !died)
        {
            cubeIsDead?.Invoke(gameObject);
            died = true;
        }
    }
}
