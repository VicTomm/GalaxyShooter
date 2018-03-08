using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    //variable for your speed
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _clip;

    private UIManager _uiManager;

    // Use this for initialization
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Laser")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.5f)
        {
            float randomX = Random.Range(-7.7f, 7.7f);
            transform.position = new Vector3(randomX, 6.5f, 0);
        }
    }
}
