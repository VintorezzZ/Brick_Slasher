using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager singleton;

    private static int level = 1;
    private int bulletsCount;
    private bool haveBullets = true;
    [SerializeField] private List<GameObject> cubesToDestroyList;
    private bool cubeIsStatic;

    private void Awake()
    {
        // if (!singleton)
        // {
        //     singleton = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }

        level = SceneManager.GetActiveScene().buildIndex + 1;

        AddBulletsAccordingLevelNumber();

        //print("Level: " + level); // 9
        //print("Bullets on level: " + bulletsCount);
        //print("Scene in build settings: " + SceneManager.sceneCountInBuildSettings); // 10
    }

    private void AddBulletsAccordingLevelNumber()
    {
        Pool.singleton.items[0].amount = 3 * level;
        bulletsCount = Pool.singleton.items[0].amount;
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

    void Start()
    {
        FindAllTargets();
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
        SceneManager.LoadScene(level - 1);
    }
    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2);
        level++;
        if (level > SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(level - 1);
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
            if (!haveBullets && CheckIfCubeIsNotMoving())
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
        haveBullets = false;
    }
}
