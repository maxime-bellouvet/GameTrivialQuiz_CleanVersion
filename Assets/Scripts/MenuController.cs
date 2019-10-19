using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private SaveAndLoad _saveAndLoad;

    private void Start()
    {
        _saveAndLoad = FindObjectOfType<SaveAndLoad>();
    }

    public void LoadGame()
    {
        if (!_saveAndLoad.CanLoad)
            return;
        GameManager.Instance.LoadGame();
    }
    public void NewGame()
    {
        GameManager.Instance.NewGame();
    }
    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
