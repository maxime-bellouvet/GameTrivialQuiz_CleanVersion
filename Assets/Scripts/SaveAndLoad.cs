using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SavedData
{

    #region fields

    public List<PlayerStats> _players;
    public int _numberOfRounds;
    public int _nextRound;
    public List<string> _specialCategory;

    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    public SavedData()
    {
        _players = new List<PlayerStats>();
        _specialCategory = new List<string>();
        _numberOfRounds = 5;
        _nextRound = 1;
    }

    #endregion
}

public class SaveAndLoad : MonoBehaviour
{

    #region fields

    private readonly string identifier = "SavedGame";

    #endregion

    #region properties

    public bool CanLoad { get; private set; }
    public SavedData SavedData { get; private set; }

    #endregion

    #region constructors


    #endregion

    #region methods


    public void Load()
    {
        if (!CanLoad)
        {
            Debug.LogError("Impossible to load the game.");
            return;
        }

        SavedData = SaveGame.Load(identifier, new SavedData());
        GameManager.Instance.numberOfRounds = SavedData._numberOfRounds;
        GameManager.Instance.CurrentRound = SavedData._nextRound;
        GameManager.Instance.players = SavedData._players;
        GameManager.Instance.specialCategories = SavedData._specialCategory;
        SceneManager.LoadScene(1);

    }

    public void SaveFinal()
    {
        GetComponent<QuestionCollection>().SerializeQuestions();
        SavedData = new SavedData();
        SaveGame.Save(identifier, SavedData);
        CanLoad = false;
    }

    public void Save()
    {

        SavedData._nextRound = GameManager.Instance.CurrentRound;
        SavedData._numberOfRounds = GameManager.Instance.numberOfRounds;
        SavedData._players = GameManager.Instance.players;
        SavedData._specialCategory = GameManager.Instance.specialCategories;

        SaveGame.Save(identifier, SavedData);
        CanLoad = true;

        SceneManager.LoadScene(0);
    }

    public void NewGame()
    {
        SavedData = new SavedData();
        CanLoad = false;
    }

    #endregion

}
