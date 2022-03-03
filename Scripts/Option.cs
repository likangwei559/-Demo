using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour
{

    private int choice = 0;
    public Transform[] pos;
    public GameObject audio;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            choice--;
            if (choice < 0)
            {
                choice = 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            choice ++;
            choice %= 3;
        }
        transform.position = pos[choice].position;
        if (choice == 0 && Input.GetKeyDown(KeyCode.Space))//开始游戏
        {
            SceneManager.LoadScene(1);
        }
        if (choice == 1 && Input.GetKeyDown(KeyCode.Space))//调整音量
        {
            audio.active = true;
        }
        if (choice == 2 && Input.GetKeyDown(KeyCode.Space))//退出游戏
        {
      
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        
        }
    }
}