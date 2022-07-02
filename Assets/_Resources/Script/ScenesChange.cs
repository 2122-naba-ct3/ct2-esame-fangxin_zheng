using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChange : MonoBehaviour
{
    public void ChangeScene(int num)
    {
        SceneManager.LoadScene(num);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
