using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvasPrefab;
    [SerializeField] private GameObject uiManagerPrefab;
    [SerializeField] private GameObject poolPrefab;

    public static int Level = 1;

    [HideInInspector]
    public int bulletsCount;
    private bool _haveBullets = true;

    [HideInInspector]
    public List<GameObject> cubesToDestroyList;

    private void Awake()
    {
        SpawnManagers();

        Level = SceneManager.GetActiveScene().buildIndex + 1;

        AddBulletsAccordingLevelNumber();
        
        FindAllTargets();
    }

    private void OnEnable()
    {
        Cannon.noMoreBullets += ChangeHaveBulletsFlag;
        CubePositionChecker.cubeIsDead += RemoveCubeFromList;
        CubePositionChecker.cubeIsDead += CheckForWinOrGameOver;
        Bullet.onBulletDestroyed += CheckForWinOrGameOver;
    }

    private void OnDisable()
    {
        Cannon.noMoreBullets -= ChangeHaveBulletsFlag;
        CubePositionChecker.cubeIsDead -= RemoveCubeFromList;
        CubePositionChecker.cubeIsDead -= CheckForWinOrGameOver;
        Bullet.onBulletDestroyed -= CheckForWinOrGameOver;
    }

    private void SpawnManagers()
    {
        Instantiate(canvasPrefab, transform.position, Quaternion.identity);
        Instantiate(uiManagerPrefab, transform.position, Quaternion.identity);
        Instantiate(poolPrefab, transform.position, Quaternion.identity);
    }

    private void AddBulletsAccordingLevelNumber()
    {
        Pool.singleton.items[0].amount = 3 * Level;
        bulletsCount = Pool.singleton.items[0].amount;
    }

    private void FindAllTargets()
    {
        cubesToDestroyList.Clear();
        
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            cubesToDestroyList.Add(cube);
        }
    }

    private void GameOver()
    {
        //print("gg");
        StartCoroutine(ReloadScene());
        FindAllTargets();
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(Level - 1);
    }
    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2);
        Level++;
        if (Level > SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(Level - 1);
        }
    }

    void RemoveCubeFromList(GameObject cube)
    {
        cubesToDestroyList.Remove(cube);
    }

    void CheckForWinOrGameOver(GameObject nothing)
    {
        if (cubesToDestroyList.Count == 0)
        {
            //print("WIN!!!");
            StartCoroutine(LoadNextScene());
        }
        else
        {
            if (!_haveBullets && CheckIfCubeIsNotMoving())
            {
                GameOver();
            }
        }
    }

    private bool CheckIfCubeIsNotMoving()
    {
        foreach (GameObject cube in cubesToDestroyList)
        {
            if (cube)
            {
                if (cube.TryGetComponent(out Rigidbody rb))
                {
                    if (rb.velocity.magnitude < 1f)
                    {
                        //print(rb.velocity.magnitude);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void ChangeHaveBulletsFlag()
    {
        _haveBullets = false;
    }
}
