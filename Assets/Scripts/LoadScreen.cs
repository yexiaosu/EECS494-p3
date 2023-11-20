using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    public string Name;

    public void Load()
    {
        SceneManager.LoadScene(Name);
    }
}
