using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public int keysNeeded = 0;

    public GameObject playerObj;

    private Player _playerScript;

    // Start is called before the first frame update
    void Start()
    {
        _playerScript = playerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadLevel(string sceneName)
    {
        LoadLevel(sceneName, true);
    }

    public void LoadLevel(string sceneName, bool ignoreKeys)
    {
        if (ignoreKeys)
        {
            SceneManager.LoadScene(sceneName);
        }
        else if (_playerScript.keysCollected >= keysNeeded)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
    public void LoadNextLevel()
    {
        var cur = int.Parse(SceneManager.GetActiveScene().name);
        LoadLevel((cur + 1).ToString());
    }

    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name, true);
    }
}