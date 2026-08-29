using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void OnExit()
    {
        Application.Quit();
    }
    public void OnStartScenes()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void OnScene1()
    {
        SceneManager.LoadScene("Night");
    }

    public void OnScene2()
    {
        SceneManager.LoadScene("ClassRoom");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
