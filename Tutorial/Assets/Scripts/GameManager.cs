using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerCtrl player;
    public Text status;
    public Text goldText;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }


    void Update()
    {
        string statusText = "Health : " + player.health + System.Environment.NewLine + "Damage : " + player.damage + System.Environment.NewLine + "AS : " + player.FireCool;
        status.text = statusText;
        goldText.text = "Gold : " + string.Format("{0:n0}", player.gold);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
