using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressHandler : MonoBehaviour
{
    private GameManager _gameManager;
    private Slider _slider;
    private float _cubesOnLevel;
    private float _deadCubes = 0;

    private void OnEnable()
    {
        CubePositionChecker.cubeIsDead += RefreshProgress;
    }

    private void OnDisable()
    {
        CubePositionChecker.cubeIsDead -= RefreshProgress;
    }

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        _slider = GetComponent<Slider>();

        _cubesOnLevel = _gameManager.cubesToDestroyList.Count;
    }

    private void RefreshProgress(GameObject cube)
    {
        _deadCubes++;

        float progressPercent = _deadCubes / _cubesOnLevel;
        
        _slider.value = progressPercent;
    }
}
