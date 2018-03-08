using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool gameOver = true;
    public GameObject player;
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    //if game over is true
    // space key p[ressed
    //spawn the player
    //gameOver is false
    //hide title screen
    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(player, Vector3.zero, Quaternion.identity);
                gameOver = false;
                _uiManager.HideTitleScreen();
            }
        }
    }
}
