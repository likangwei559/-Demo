using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dieOption : MonoBehaviour
{
    private int choice = 0;
    public Transform[] pos;
    // Start is called before the first frame update
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
                choice = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            choice++;
            choice %= 2;
        }
        transform.position = pos[choice].position;
        if (choice == 0 && Input.GetKeyDown(KeyCode.Space))//重新开始
        {
            SceneManager.LoadScene(1);
        }
        if (choice == 1 && Input.GetKeyDown(KeyCode.Space))//退出游戏
        {

            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();

        }
    }
}
