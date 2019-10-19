using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;



public class PlayersFinalAnswers
{
    #region fields

    public string _question;
    public string _answersPlayer;
    public string _correctAnswer;
    public bool _isCorrect;

    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods


    #endregion
}

public class GameManager : MonoBehaviour
{

    #region fields

    public int numberOfPlayers;
    public int numberOfRounds = 5;
    public string[] jokers = new string[4] { "Super Mouette-Mouette", "Ok Gogole", "Appel Ô Public", "Swiitch" };
    public AudioClip soundJokerSuperMouetteMouette;
    public AudioClip soundJokerSwiitch;
    public List<string> regularCategories = new List<string>();
    public List<string> specialCategories = new List<string>();
    public List<PlayerStats> players;
    public GameObject HUD;

    private QuizQuestion currentQuestion;
    private string _choiceForAnswer = string.Empty;
    private QuestionCollection questionCollection;
    private List<PlayerStats> finalPlayers = new List<PlayerStats>();
    private int[] oldScoresForFinalPlayers = new int[2] { 0, 0 };
    private string _regularCategory;
    private string _specialCategory;
    private string _specialCategoryFinale1;
    private string _specialCategoryFinale2;
    private string _specialCategoryFinale3;
    private TMP_Dropdown.OptionData[] specialCategoriesData;
    private List<string> listSpecialCategories = new List<string>();
    private int indexRegular;
    private int indexSpecial;
    private int currentPlayer = 0;
    private int cashContainer;
    private bool isCorrect;
    private int pointsMultiplier = 1;
    private bool ArePointsMultiplied = false;
    private bool exAequo1 = false;
    private int exAequo1_nb = 2;
    private bool exAequo2 = false;
    private int exAequo2_nb = 2;
    private List<int> exAequo1_indices = new List<int>();
    private List<int> exAequo2_indices = new List<int>();

    private UIController uiController;
    private HUDFinale _uiFinale;
    private int numberQuestionsFinaleRound = 5;
    private Text _currentCategory;
    private List<PlayersFinalAnswers> _player1FinalAnswers = new List<PlayersFinalAnswers>();
    private List<PlayersFinalAnswers> _player2FinalAnswers = new List<PlayersFinalAnswers>();
    private List<string> listNamesShifumi = new List<string>();
    private float delayBetweenQuestions = 3f;
    #endregion

    #region properties
    public static GameManager Instance { get; private set; }
    public int CurrentRound { get; set; }
    public bool IsRoundOver { get { return (currentPlayer >= numberOfPlayers); } }
    public bool FinaleRound { get; private set; }
    public bool IsGameOver { get { return (IsRoundOver && CurrentRound > numberOfRounds); } }
    public int CurrentQuestionFinale { get; private set; } = 1;
    public bool IsFinaleRoundOver { get { return (CurrentQuestionFinale > numberQuestionsFinaleRound); } }

    #endregion

    #region constructors


    #endregion

    #region methods

    private void Awake()
    {
        #region singleton

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        #endregion

        DontDestroyOnLoad(this.gameObject);

        InitializeParameters();
    }

    private void Start()
    {
        HUD = GameObject.Find("HUD");

        specialCategoriesData = HUD.transform.Find("Menu Players/Liste des joueurs").GetChild(0).GetChild(1).GetComponent<TMP_Dropdown>().options.ToArray();
        for (int i = 0; i < specialCategoriesData.Length; i++)
        {
            listSpecialCategories.Add(specialCategoriesData[i].text);
        }
    }

    private void Update()
    {
        if (uiController == null && (SceneManager.GetActiveScene().buildIndex == 1))
        {
            uiController = FindObjectOfType<UIController>();
        }
        else if (HUD == null && (SceneManager.GetActiveScene().buildIndex == 0))
        {
            HUD = GameObject.Find("HUD");
        }
        else if (_uiFinale == null && (SceneManager.GetActiveScene().buildIndex == 2))
        {
            _uiFinale = FindObjectOfType<HUDFinale>();
        }
    }

    public void InitializeParameters()
    {
        specialCategories = new List<string>();
        players = new List<PlayerStats>();
        oldScoresForFinalPlayers = new int[] { 0, 0 };
        exAequo1 = false;
        exAequo2 = false;
        FinaleRound = false;
        numberOfRounds = 5;
        CurrentRound = 1;
    }

    public void AddPlayers()
    {
        players = new List<PlayerStats>();
        Transform listPlayers = HUD.transform.Find("Menu Players/Liste des joueurs");
        int indexPlayer = 0;
        foreach (Transform child in listPlayers)
        {
            if (child.GetChild(0).Find("Text").GetComponent<Text>().text != string.Empty)
            {
                PlayerStats newPlayer = new PlayerStats();
                newPlayer.name = child.GetChild(0).Find("Text").GetComponent<Text>().text.ToString();
                newPlayer.id = indexPlayer;
                newPlayer.position = newPlayer.id + 1;
                newPlayer.specialCategory = child.Find("Dropdown Special Category/Label").GetComponent<TextMeshProUGUI>().text;
                specialCategories.Add(child.Find("Dropdown Special Category/Label").GetComponent<TextMeshProUGUI>().text);
                players.Add(newPlayer);
                indexPlayer++;
            }
        }
        numberOfPlayers = players.Count;
    }

    public void StartRound()
    {
        CurrentRound++;
        uiController.UpdateRound(CurrentRound);
        currentPlayer = 0;
        uiController.UpdateJokers(players[currentPlayer], 0.5f);
        UpdateCategories();
    }

    public void UpdateCategories()
    {
        if (IsRoundOver)
        {
            return;
        }

        uiController.SetupCurrentPlayer(currentPlayer);

        _regularCategory = _specialCategory = string.Empty;

        //pick one theme from regular categories
        _regularCategory = PickARegularCategory();
        //pick one theme from special categories
        _specialCategory = PickASpecialCategory();

        if (_specialCategory != players[currentPlayer].specialCategory)
        {
            ArePointsMultiplied = true;
            uiController.TogglePointMultiplier(ArePointsMultiplied);
        }
        else
        {
            ArePointsMultiplied = false;
            uiController.TogglePointMultiplier(ArePointsMultiplied);
        }

        uiController.SetupUIForCategories(_regularCategory, _specialCategory);
    }


    public void SetChoice(Text choiceForAnswer)
    {
        _choiceForAnswer = choiceForAnswer.text.ToString();
    }

    private string PickARegularCategory()
    {
        indexRegular = Random.Range(0, regularCategories.Count);
        return regularCategories[indexRegular];
    }

    private string PickASpecialCategory()
    {
        indexSpecial = Random.Range(0, specialCategories.Count);
        return specialCategories[indexSpecial];
    }

    public void NextPlayer()
    {
        _choiceForAnswer = string.Empty;
        currentPlayer++;
        if (!IsRoundOver)
            uiController.UpdateJokers(players[currentPlayer], delayBetweenQuestions);
    }

    public void PresentQuestion(Text category)
    {
        if (category.text != players[currentPlayer].specialCategory && listSpecialCategories.Contains(category.text))
            pointsMultiplier = 2;
        else
            pointsMultiplier = 1;

        uiController.UpdatePointsToScore(pointsMultiplier);

        questionCollection = gameObject.GetComponent<QuestionCollection>();

        if (category.text == "Surprise")
        {
            category.text = PickASpecialCategory();
        }

        currentQuestion = questionCollection.GetUnaskedQuestion(category.text);

        uiController.SetupUIForQuestion(category, currentQuestion);

    }

    public void SubmitAnswer(Button clickedButton)
    {
        Text buttonText = clickedButton.transform.GetChild(0).GetComponent<Text>();

        isCorrect = buttonText.text == currentQuestion.CorrectAnswer[0];

        uiController.HandleSubmittedAnswer(isCorrect, clickedButton);


        if (isCorrect)
            RefreshScore();

        NextPlayer();
        if (!IsGameOver)
        {
            StartCoroutine(ShowNextQuestionAfterDelay());
        }
        else
        {
            StartCoroutine(ShowFinalScoresAfterDelay());
        }

        UpdateCategories();

    }
    public void SubmitCashAnswer(string _answer)
    {
        int percentageToReachToValidateAnswer;
        if (currentQuestion.CorrectAnswer.Length > 45)
            percentageToReachToValidateAnswer = 75;
        else
            percentageToReachToValidateAnswer = 94;

        _answer = _answer.RemoveDiacritics();
        for (int i = 0; i < currentQuestion.CorrectAnswer.Length; i++)
        {
            string _correctAnwser = currentQuestion.CorrectAnswer[i].ToLower();
            int maxCashContainer = Mathf.Max(_correctAnwser.Length, _answer.Length);
            _correctAnwser = Strings.RemoveDiacritics(_correctAnwser);

            cashContainer = 0;
            foreach (char letter in _answer.ToLower())
            {

                if (_correctAnwser.Contains(letter.ToString()))
                {
                    _correctAnwser = _correctAnwser.Remove(_correctAnwser.IndexOf(letter.ToString()), 1);
                    cashContainer++;
                }
            }

            float percentageCorrectLetters = Mathf.Round((float)cashContainer / (float)maxCashContainer * 100f);
            percentageCorrectLetters = Mathf.Clamp(percentageCorrectLetters, 0f, 100f);

            isCorrect = (percentageCorrectLetters > percentageToReachToValidateAnswer) ? true : false;
            if (isCorrect)
                break;
        }
        uiController.HandleSubmittedAnswer(isCorrect);

        if (isCorrect)
            RefreshScore();

        NextPlayer();

        if (!IsGameOver)
        {
            StartCoroutine(ShowNextQuestionAfterDelay());
        }
        else
        {
            StartCoroutine(ShowFinalScoresAfterDelay());
        }

        UpdateCategories();

    }

    private void RefreshScore()
    {
        if (_choiceForAnswer == "BINAIRE")
            players[currentPlayer].score = players[currentPlayer].score + (1 * pointsMultiplier);
        else if (_choiceForAnswer == "TETRA")
            players[currentPlayer].score = players[currentPlayer].score + (3 * pointsMultiplier);
        else if (_choiceForAnswer == "CASH-CASH")
            players[currentPlayer].score = players[currentPlayer].score + (5 * pointsMultiplier);

        uiController.UpdateScorePanel(numberOfPlayers);
    }

    private IEnumerator ShowNextQuestionAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);
        uiController.DisplayNextScreen(IsRoundOver);
    }

    private IEnumerator ShowFinalScoresAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenQuestions);

        FindIfExAequo();
        if (exAequo1)
        {
            uiController.DisplayEndScreen(players, exAequo1_indices, 1);

        }
        else if (exAequo2)
        {
            uiController.DisplayEndScreen(players, exAequo2_indices, 2);
        }
        else
        {
            int[] indicesBestPlayers = FindBothBestPlayers();
            uiController.DisplayEndScreen(players, indicesBestPlayers.ToList<int>(), 0);
            oldScoresForFinalPlayers[0] = players[indicesBestPlayers[0]].score;
            oldScoresForFinalPlayers[1] = players[indicesBestPlayers[1]].score;
            SetFinalists(indicesBestPlayers[0], indicesBestPlayers[1]);
        }

    }

    public void AddShifumiWinner(string _playerName)
    {
        listNamesShifumi.Add(_playerName);

        if (exAequo1 && exAequo1_nb == 2)
        {
            string nameSecond = (players[exAequo1_indices[0]].name == _playerName) ? players[exAequo1_indices[1]].name : players[exAequo1_indices[0]].name;
            listNamesShifumi.Add(nameSecond);
        }

        if (listNamesShifumi.Count == 2)
        {
            SetWinnerShifumi(listNamesShifumi[0], listNamesShifumi[1]);
            uiController.UndisplayShifumiButton();
        }
    }

    public void SetWinnerShifumi(string _playerName1, string _playerName2)
    {
        int indexFirst = 0;
        int indexSecond = 1;
        if (exAequo1)
        {
            if (exAequo1_nb == 2)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].name == _playerName1)
                    {
                        indexFirst = i;
                        continue;
                    }
                    else if (players[i].name == _playerName2)
                    {
                        indexSecond = i;
                        continue;
                    }
                }
            }
            else
            {

            }
        }
        else if (exAequo2)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].name == _playerName1)
                {
                    indexFirst = i;
                    continue;
                }
                else if (players[i].name == _playerName2)
                {
                    indexSecond = i;
                    continue;
                }

            }

            Debug.Log(exAequo2_indices.Count + " joueurs à la seconde place.");

        }
        oldScoresForFinalPlayers[0] = players[indexFirst].score;
        oldScoresForFinalPlayers[1] = players[indexSecond].score;
        SetFinalists(indexFirst, indexSecond);
        uiController.DisplayFinaleButton();
    }

    public void FindIfExAequo()
    {
        int[] _arrayIndices = FindBothBestPlayers();
        int scoreFirst = players[_arrayIndices[0]].score;

        if (players[_arrayIndices[1]].score == scoreFirst)
        {
            exAequo1 = true;
            exAequo1_indices = _arrayIndices.ToList<int>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (i == _arrayIndices[0] || i == _arrayIndices[1])
                    continue;

                if (players[i].score == scoreFirst)
                {
                    exAequo1_nb++;
                    exAequo1_indices.Add(i);
                }
            }
        }
        else
        {
            listNamesShifumi.Add(players[_arrayIndices[0]].name);
            exAequo2_indices.Add(_arrayIndices[1]);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (i == _arrayIndices[1])
                    continue;

                if (players[i].score == players[_arrayIndices[1]].score)
                {
                    exAequo2 = true;
                    exAequo2_nb++;
                    exAequo2_indices.Add(i);

                }
            }
        }

    }

    public void LoadQuestionsScene()
    {
        AddPlayers();
        SceneManager.LoadScene(1);
    }

    public void UseJoker(string joker)
    {
        switch (joker)
        {
            case "Super Mouette-Mouette":
                UseJokerMouetteMouette();
                players[currentPlayer].jokers[0] = false;
                break;
            case "Ok Gogole":
                UseJokerGogole();
                players[currentPlayer].jokers[1] = false;
                break;
            case "Appel Ô Public":
                UseJokerPublic();
                players[currentPlayer].jokers[2] = false;
                break;
            case "Swiitch":
                UseJokerSwiitch();
                players[currentPlayer].jokers[3] = false;
                break;
            default:
                return;
        }
        uiController.UpdateJokers(players[currentPlayer], 0.1f);

    }

    public bool ValidateJoker(string joker)
    {

        switch (joker)
        {
            case "Super Mouette-Mouette":
                if (_choiceForAnswer == "CASH-CASH" || _choiceForAnswer == string.Empty)
                {
                    return false;
                }
                break;
            case "Ok Gogole":
                if (_choiceForAnswer == string.Empty)
                {
                    return false;
                }
                break;
            case "Appel Ô Public":
                if (_choiceForAnswer == string.Empty)
                {
                    return false;
                }
                break;
            case "Swiitch":
                if (_choiceForAnswer != string.Empty)
                {
                    return false;
                }
                break;
            default:
                return false;
        }
        uiController.DisplayJokerValidation(joker);

        return true;
    }

    public void UseJokerMouetteMouette()
    {
        GetComponent<AudioSource>().clip = soundJokerSuperMouetteMouette;
        GetComponent<AudioSource>().Play();
        uiController.UpdateUIJoker(_choiceForAnswer);
    }

    public void UseJokerGogole()
    {
        uiController.DisplayTimer(true);
    }

    public void UseJokerPublic()
    {
        uiController.DisplayTimer(true);
    }

    public void UseJokerSwiitch()
    {
        GetComponent<AudioSource>().clip = soundJokerSwiitch;
        GetComponent<AudioSource>().Play();
        uiController.JokerSwiitchUI();
        currentQuestion = questionCollection.GetUnaskedQuestion(uiController.categoryText.text);
        uiController.SetupUIForQuestion(uiController.categoryText, currentQuestion);
    }

    public int FindBestPlayer()
    {
        int maxScore = players[0].score;
        int bestPlayerIndex = players[0].id;
        //TODO : Select best scores in players list
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].score > maxScore)
            {
                bestPlayerIndex = i;
                maxScore = players[i].score;
            }
        }
        return bestPlayerIndex;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        GetComponent<SaveAndLoad>().NewGame();
        HUD.transform.Find("Menu Title").gameObject.SetActive(false);
        InitializeParameters();
    }

    public void LoadGame()
    {
        GetComponent<SaveAndLoad>().Load();
    }

    public void SaveGame()
    {
        GetComponent<SaveAndLoad>().Save();
    }

    public void BackToMenu()
    {
        GetComponent<SaveAndLoad>().NewGame();
        SceneManager.LoadScene(0);
    }


    // FINALE
    public int[] FindBothBestPlayers()
    {
        int maxScore = players[0].score;
        int firstPlayerIndex = players[0].id;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].score > maxScore)
            {
                firstPlayerIndex = i;
                maxScore = players[i].score;
            }
        }
        maxScore = 0;
        int secondPlayerIndex = players[0].id;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].score >= maxScore && i != firstPlayerIndex)
            {
                secondPlayerIndex = i;
                maxScore = players[i].score;
            }
        }
        int[] array = new int[] { firstPlayerIndex, secondPlayerIndex };
        return array;
    }

    public void SetUpFinale()
    {
        StartCoroutine(SetUpFinaleCo());
    }

    public IEnumerator SetUpFinaleCo()
    {
        FinaleRound = true;
        CurrentQuestionFinale = 1;
        numberOfRounds = 2;
        CurrentRound = 1;
        currentPlayer = 0;

        yield return new WaitForSeconds(1f);
        SetUpCategoriesFinale();
        _uiFinale.SetUpUIForPlayer(finalPlayers[0]);
    }

    public void SetFinalists(int indiceFirst, int indiceSecond)
    {
        numberOfPlayers = 2;
        if (finalPlayers.Count != 0)
            finalPlayers.Clear();

        _player1FinalAnswers.Clear();
        _player2FinalAnswers.Clear();

        finalPlayers.Add(players[indiceFirst]);
        finalPlayers.Add(players[indiceSecond]);

        finalPlayers[0].score = 0;
        finalPlayers[1].score = 0;
        finalPlayers[0].id = 0;
        finalPlayers[1].id = 1;
    }

    public void SetUpCategoriesFinale()
    {
        int index1 = Random.Range(0, listSpecialCategories.Count);
        int index2 = Random.Range(0, listSpecialCategories.Count);
        while (index2 == index1)
        {
            index2 = Random.Range(0, listSpecialCategories.Count);
        }
        int index3 = Random.Range(0, listSpecialCategories.Count);
        while (index3 == index2 || index3 == index1)
        {
            index3 = Random.Range(0, listSpecialCategories.Count);
        }
        _specialCategoryFinale1 = listSpecialCategories[index1];
        _specialCategoryFinale2 = listSpecialCategories[index2];
        _specialCategoryFinale3 = listSpecialCategories[index3];

        _uiFinale.SetUIForFinaleCategories(_specialCategoryFinale1, _specialCategoryFinale2, _specialCategoryFinale3);

    }


    public void PresentFinalQuestion(Text category)
    {
        _currentCategory = category;
        questionCollection = gameObject.GetComponent<QuestionCollection>();

        currentQuestion = questionCollection.GetUnaskedQuestion(category.text);

        _uiFinale.SetupUIForQuestion(category, currentQuestion);

    }
    public void SetChoiceFinale(Text choiceForAnswer)
    {
        _choiceForAnswer = choiceForAnswer.text.ToString();
    }

    public void SubmitFinaleAnswer(Button clickedButton)
    {
        Text buttonText = clickedButton.transform.GetChild(0).GetComponent<Text>();

        isCorrect = buttonText.text == currentQuestion.CorrectAnswer[0];

        _uiFinale.HandleSubmittedAnswer(buttonText);



        if (currentPlayer == 0)
            _player1FinalAnswers.Add(new PlayersFinalAnswers { _question = currentQuestion.Question, _answersPlayer = buttonText.text, _correctAnswer = currentQuestion.CorrectAnswer[0], _isCorrect = isCorrect });
        else
            _player2FinalAnswers.Add(new PlayersFinalAnswers { _question = currentQuestion.Question, _answersPlayer = buttonText.text, _correctAnswer = currentQuestion.CorrectAnswer[0], _isCorrect = isCorrect });


        if (isCorrect)
            RefreshScoreFinale();

        CurrentQuestionFinale++;
        if (!IsFinaleRoundOver)
        {
            StartCoroutine(ShowNextFinaleQuestionAfterDelay());
        }
        else if (IsFinaleRoundOver)
        {
            CurrentRound++;
            currentPlayer++;

            if (!IsGameOver)
                StartCoroutine(ShowNextFinaleRound());
            else
                StartCoroutine(ShowFinaleScoresAfterDelay());
        }

    }

    public void SubmitFinaleCashAnswer(string _answer)
    {
        int percentageToReachToValidateAnswer;
        if (currentQuestion.CorrectAnswer.Length > 45)
            percentageToReachToValidateAnswer = 75;
        else
            percentageToReachToValidateAnswer = 94;

        _answer = _answer.RemoveDiacritics();
        for (int i = 0; i < currentQuestion.CorrectAnswer.Length; i++)
        {
            string _correctAnwser = currentQuestion.CorrectAnswer[i].ToLower();
            int maxCashContainer = Mathf.Max(_correctAnwser.Length, _answer.Length);
            _correctAnwser = Strings.RemoveDiacritics(_correctAnwser);

            cashContainer = 0;
            foreach (char letter in _answer.ToLower())
            {
                if (_correctAnwser.Contains(letter.ToString()))
                {
                    // _correctAnwser.Replace(letter.ToString(), "");
                    _correctAnwser = _correctAnwser.Remove(_correctAnwser.IndexOf(letter.ToString()), 1);

                    cashContainer++;
                }
            }

            float percentageCorrectLetters = Mathf.Round((float)cashContainer / (float)maxCashContainer * 100f);

            percentageCorrectLetters = Mathf.Clamp(percentageCorrectLetters, 0f, 100f);

            isCorrect = (percentageCorrectLetters > percentageToReachToValidateAnswer) ? true : false;
            if (isCorrect)
                break;
        }


        if (currentPlayer == 0)
            _player1FinalAnswers.Add(new PlayersFinalAnswers { _question = currentQuestion.Question, _answersPlayer = _answer, _correctAnswer = currentQuestion.CorrectAnswer[0], _isCorrect = isCorrect });
        else
            _player2FinalAnswers.Add(new PlayersFinalAnswers { _question = currentQuestion.Question, _answersPlayer = _answer, _correctAnswer = currentQuestion.CorrectAnswer[0], _isCorrect = isCorrect });


        if (isCorrect)
            RefreshScoreFinale();

        CurrentQuestionFinale++;
        if (!IsFinaleRoundOver)
        {
            StartCoroutine(ShowNextFinaleQuestionAfterDelay());
        }
        else if (IsFinaleRoundOver)
        {
            CurrentRound++;
            currentPlayer++;
            if (!IsGameOver)
                StartCoroutine(ShowNextFinaleRound());
            else
                StartCoroutine(ShowFinaleScoresAfterDelay());
        }

    }

    private void RefreshScoreFinale()
    {

        if (_choiceForAnswer == "BINAIRE")
            finalPlayers[currentPlayer].score += 1;
        else if (_choiceForAnswer == "TETRA")
            finalPlayers[currentPlayer].score += 3;
        else if (_choiceForAnswer == "CASH-CASH")
            finalPlayers[currentPlayer].score += 5;
    }

    public IEnumerator ShowNextFinaleQuestionAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        PresentFinalQuestion(_currentCategory);
    }
    public IEnumerator ShowNextFinaleRound()
    {
        CurrentQuestionFinale = 1;
        yield return new WaitForSeconds(1f);
        _uiFinale.SetUpUIForPlayer(finalPlayers[currentPlayer]);

    }
    public IEnumerator ShowFinaleScoresAfterDelay()
    {
        GetComponent<SaveAndLoad>().SaveFinal();
        PlayerStats bestPlayer = null;
        if (finalPlayers[0].score > finalPlayers[1].score)
        {
            bestPlayer = finalPlayers[0];
        }
        else if (finalPlayers[0].score < finalPlayers[1].score)
        {
            bestPlayer = finalPlayers[1];
        }
        else if (finalPlayers[0].score == finalPlayers[1].score)
        {
            finalPlayers[0].score += oldScoresForFinalPlayers[0];
            finalPlayers[1].score += oldScoresForFinalPlayers[1];
            bestPlayer = (oldScoresForFinalPlayers[0] > oldScoresForFinalPlayers[1]) ? finalPlayers[0] : finalPlayers[1];
        }

        yield return new WaitForSeconds(1f);
        _uiFinale.DisplayFinalScreen(finalPlayers, _player1FinalAnswers, _player2FinalAnswers, bestPlayer);
    }
    #endregion

}