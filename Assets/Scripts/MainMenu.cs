using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public static string path;
    public static string pathdata;
    public static double[,] split;

    public void StartSim()
    {
        Debug.Log("Start!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Также в скобках может быть указан номер сцены или название, например ("Scene1")
    }

    public void StopSim()
    {
        Debug.Log("Stop!");
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void OpenExplorerVideo()
    {
        path = EditorUtility.OpenFilePanel("video", "", "mp4");
        if (path != null)
        {
            Debug.Log("Video loaded");
        }

    }

    
}
    

