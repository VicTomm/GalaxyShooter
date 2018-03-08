using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyShipPrefab;

    [SerializeField]
    private GameObject[] _powerups;

    private GameManager _gameManager;
    // Use this for initialization
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerupSpawnRoutine());
    }

    public void StartSpawnRoutines()
    {
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerupSpawnRoutine());
    }

    IEnumerator EnemySpawn()
    {
        while (!_gameManager.gameOver)
        {
            Instantiate(_enemyShipPrefab, new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while (!_gameManager.gameOver)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-7f, 7f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
