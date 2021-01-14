using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    private GameManager _gameManager;
    
    private Slider _slider;
    private Text _ammoText;

    private Text _currentLevelText;
    private Text _nextLevelText;
    
    private float _cubesOnLevel;
    private float _deadCubes = 0;
    private int _bulletsOnLevel;

    private void OnEnable()
    {
        CubePositionChecker.cubeIsDead += RefreshProgress;
        Cannon.onShoot += RefreshBulletsCount;
    }

    private void OnDisable()
    {
        CubePositionChecker.cubeIsDead -= RefreshProgress;
        Cannon.onShoot -= RefreshBulletsCount;
    }

    private void Awake()
    {
        _slider = GameObject.Find("Progress bar").GetComponent<Slider>();
        _ammoText = GameObject.Find("Ammo Text").GetComponent<Text>();
        _currentLevelText = GameObject.Find("Current Level Text").GetComponent<Text>();
        _nextLevelText = GameObject.Find("Next Level Text").GetComponent<Text>();
    }

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        _cubesOnLevel = _gameManager.cubesToDestroyList.Count;

        _bulletsOnLevel = _gameManager.bulletsCount;
        
        SetBulletsCount();
        
        SetCurrentAndNextLevelText();
    }

    private void SetCurrentAndNextLevelText()
    {
        int currentLevel = GameManager.Level;
        int nextLevel = currentLevel + 1;
        _currentLevelText.text = currentLevel.ToString();
        _nextLevelText.text = nextLevel.ToString();
    }
    private void RefreshBulletsCount()
    {
        _bulletsOnLevel--;
        SetBulletsCount();
    }

    private void SetBulletsCount()
    {
        _ammoText.text = "Ammo x " + _bulletsOnLevel;
    }

    private void RefreshProgress(GameObject cube)
    {
        _deadCubes++;

        float progressPercent = _deadCubes / _cubesOnLevel;
        
        _slider.value = progressPercent;
    }
}
