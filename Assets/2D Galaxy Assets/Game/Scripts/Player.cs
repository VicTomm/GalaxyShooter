using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canTripleShot = false;
    public bool isSpeedBoostActive = false;
    public bool isShieldActive = false;
    public int lives = 3;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShot;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _shieldGameObject;

    [SerializeField]
    private GameObject[] _engines;

    //fireRate is 0.25f
    //nextFire -- has the amount of time between firing passed?
    [SerializeField]
    private float _fireRate = 0.25f;

    private float _nextFire = 0.0f;


    [SerializeField]
    private float _speed = 5.0f;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;
    private int hitCount = 0;

    private void Start()
    {
        //Current pos = new position
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(lives);
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;
    }

    private void Update()
    {
        Movement();

        //if space key pressed
        //spawn laser at player position
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time > _nextFire)
        {
            _audioSource.Play();
            if (canTripleShot)
            {
                Instantiate(_tripleShot, transform.position + new Vector3(0.03f, 0.38f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.93f, 0), Quaternion.identity);
            }
            _nextFire = Time.time + _fireRate;
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (isSpeedBoostActive)
        {
            transform.Translate(Vector3.right * (_speed * 1.5f) * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * (_speed * 1.5f) * verticalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }

        //if player on the y is greater than 0
        //set player position to 0

        if (transform.position.y > 4.2f)
        {
            transform.position = new Vector3(transform.position.x, 4.2f, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        //if player on the x is greater tha 9.5
        //position on the x need to be -9.5

        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }

    public void Damage()
    {

        hitCount++;

        if (isShieldActive)
        {
            isShieldActive = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        if (hitCount == 1)
        {
            //turn left engine_failure on
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        lives--;
        _uiManager.UpdateLives(lives);
        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostPowerupOn()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldPowerupOn()
    {
        isShieldActive = true;
        _shieldGameObject.SetActive(true);
    }

    //method to enable to powerup
    //couroutine method (ienumerator) to powerDown the speed boost
    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isSpeedBoostActive = false;
    }
}
