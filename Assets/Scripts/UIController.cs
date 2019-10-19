using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    #region fields

    public Text buttonRegularCategoryText;
    public Text buttonSpecialCategoryText;
    public Text buttonRoundText;
    public Text roundText;
    public GameObject ScorePanel;
    public GameObject SmallScorePanel;
    public GameObject CategoriesPanel;
    public GameObject EndScreen;
    public GameObject duoPanel;
    public GameObject carrePanel;
    public GameObject cashPanel;
    public GameObject answerChoicePanel;
    public GameObject pointsMultiplierText;
    public GameObject jokersPanel;
    public GameObject[] validationPanels;
    public GameObject timerPanel;
    public Text categoryText;
    public GameObject shifumiWinner;
    public GameObject prefabShifumiButton;
    [SerializeField]
    private Text questionText;
    [SerializeField]
    private Button[] answerButtonsTetra;
    [SerializeField]
    private Button[] answerButtonsBinaire;
    private int[] indicesTetra = new int[4];
    private int[] indicesBinary = new int[4];
    private string _currentCorrectAnswer;
    private Button clickedButton;
    private Transform pointsToScore;
    private UICommon uiCommon = new UICommon();
    private RaycastHit hit;
    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    private void Start()
    {
        StartCoroutine(UpdateScoresCo());
        pointsToScore = this.gameObject.transform.Find("Question Proposition/Question Panel/Answer Choices/Points to Score");
    }


    IEnumerator UpdateScoresCo()
    {
        yield return new WaitForSeconds(.1f);
        UpdateScorePanel(GameManager.Instance.numberOfPlayers);
        UpdateRound(GameManager.Instance.CurrentRound);
    }

    public void UpdateScorePanel(int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            ScorePanel.transform.Find("Score Panel/Scores").GetChild(i).gameObject.SetActive(true);
            ScorePanel.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.Instance.players[i].name;
            ScorePanel.transform.Find("Score Panel/Scores").GetChild(i).GetChild(1).GetComponent<Text>().text = GameManager.Instance.players[i].score.ToString();
        }
        UpdateSmallScorePanel();
    }

    public void UpdateFinalScorePanel(bool isSomeoneFirst)
    {
        for (int i = 0; i < GameManager.Instance.numberOfPlayers; i++)
        {
            EndScreen.transform.Find("Score Panel/Scores").GetChild(i).gameObject.SetActive(true);
            EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.Instance.players[i].name;
            if (GameManager.Instance.FindBestPlayer() == i && isSomeoneFirst)
            {
                EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().color = Color.yellow;
                EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
            }
            EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(1).GetComponent<Text>().text = GameManager.Instance.players[i].score.ToString();
        }
    }


    public void UpdateSmallScorePanel()
    {
        for (int i = 0; i < GameManager.Instance.numberOfPlayers; i++)
        {
            SmallScorePanel.transform.Find("Scores").GetChild(i).gameObject.SetActive(true);
            SmallScorePanel.transform.Find("Scores").GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.Instance.players[i].name;
            SmallScorePanel.transform.Find("Scores").GetChild(i).GetChild(1).GetComponent<Text>().text = GameManager.Instance.players[i].score.ToString();
        }
    }

    public void SetupUIForCategories(string regular, string special)
    {
        buttonRegularCategoryText.text = regular;
        buttonSpecialCategoryText.text = special;
    }


    public void SetupCurrentPlayer(int indexPlayer)
    {
        CategoriesPanel.transform.Find("Question/Question Text").GetComponent<Text>().text = GameManager.Instance.players[indexPlayer].name + ", choississez une catégorie:";
    }

    public void SetupUIForQuestion(Text _category, QuizQuestion question)
    {
        RemoveAnswersIcons();
        ToggleAnswerButtons(true);
        jokersPanel.SetActive(true);

        categoryText.text = _category.text;
        questionText.text = question.Question;
        questionText.fontSize = uiCommon.FixFontSizeForQuestion(questionText.text.Length);

        _currentCorrectAnswer = question.CorrectAnswer[0];
        //Binaire répartition
        indicesBinary = uiCommon.ChooseRandomIndices(2);
        answerButtonsBinaire[indicesBinary[0]].GetComponentInChildren<Text>().text = _currentCorrectAnswer;
        answerButtonsBinaire[indicesBinary[1]].GetComponentInChildren<Text>().text = question.WrongAnswers[Random.Range(0, 3)];

        //Tetra répartition
        indicesTetra = uiCommon.ChooseRandomIndices(4);
        int[] indicesWrongAnswers = uiCommon.ChooseRandomIndices(3);
        answerButtonsTetra[indicesTetra[0]].GetComponentInChildren<Text>().text = question.CorrectAnswer[0];
        answerButtonsTetra[indicesTetra[1]].GetComponentInChildren<Text>().text = question.WrongAnswers[indicesWrongAnswers[0]];
        answerButtonsTetra[indicesTetra[2]].GetComponentInChildren<Text>().text = question.WrongAnswers[indicesWrongAnswers[1]];
        answerButtonsTetra[indicesTetra[3]].GetComponentInChildren<Text>().text = question.WrongAnswers[indicesWrongAnswers[2]];

        for (int i = 0; i < answerButtonsBinaire.Length; i++)
        {
            answerButtonsBinaire[i].GetComponentInChildren<Text>().fontSize = uiCommon.FixFontSizeForAnswers(answerButtonsBinaire[i].GetComponentInChildren<Text>().text.Length);
        }
        for (int i = 0; i < answerButtonsTetra.Length; i++)
        {
            answerButtonsTetra[i].GetComponentInChildren<Text>().fontSize = uiCommon.FixFontSizeForAnswers(answerButtonsTetra[i].GetComponentInChildren<Text>().text.Length);
        }

    }

    public void UpdateUIJoker(string _choiceForAnswer)
    {
        if (_choiceForAnswer == "BINAIRE")
        {
            answerButtonsBinaire[indicesBinary[1]].GetComponentInChildren<Text>().text = string.Empty;
            answerButtonsBinaire[indicesBinary[1]].interactable = false;
        }
        else if (_choiceForAnswer == "TETRA")
        {
            answerButtonsTetra[indicesTetra[1]].GetComponentInChildren<Text>().text = string.Empty;
            answerButtonsTetra[indicesTetra[2]].GetComponentInChildren<Text>().text = string.Empty;
            answerButtonsTetra[indicesTetra[1]].interactable = false;
            answerButtonsTetra[indicesTetra[2]].interactable = false;
        }
    }

    public void HandleSubmittedAnswer(bool isCorrect, Button _clickedButton = null)
    {
        for (int i = 0; i < 4; i++)
        {
            ToggleJokerValidation("all", false);
        }

        DisplayTimer(false);

        if (_clickedButton != null)
        {
            clickedButton = _clickedButton;
        }
        else
        {
            clickedButton = null;
        }

        ToggleAnswerButtons(false);

        if (isCorrect)
        {
            ShowCorrectAnswerPopup();
        }
        else
        {
            ShowWrongAnswerPopup();
        }
    }

    private void ToggleAnswerButtons(bool value)
    {
        for (int i = 0; i < answerButtonsBinaire.Length; i++)
        {
            answerButtonsBinaire[i].gameObject.GetComponent<Button>().enabled = value;
            answerButtonsBinaire[i].gameObject.GetComponent<Button>().interactable = value;
        }
        for (int i = 0; i < answerButtonsTetra.Length; i++)
        {
            answerButtonsTetra[i].gameObject.GetComponent<Button>().enabled = value;
            answerButtonsTetra[i].gameObject.GetComponent<Button>().interactable = value;
        }
    }

    private void RemoveAnswersIcons()
    {
        for (int i = 0; i < answerButtonsBinaire.Length; i++)
        {
            answerButtonsBinaire[i].transform.Find("Wrong").gameObject.SetActive(false);
            answerButtonsBinaire[i].transform.Find("Correct").gameObject.SetActive(false);
        }
        for (int i = 0; i < answerButtonsTetra.Length; i++)
        {
            answerButtonsTetra[i].transform.Find("Wrong").gameObject.SetActive(false);
            answerButtonsTetra[i].transform.Find("Correct").gameObject.SetActive(false);
        }

        cashPanel.transform.Find("Wrong").gameObject.SetActive(false);
        cashPanel.transform.Find("Correct").gameObject.SetActive(false);
    }

    private void ShowCorrectAnswerPopup()
    {
        if (clickedButton != null)
        {
            clickedButton.transform.Find("Correct").gameObject.SetActive(true);
            clickedButton.transform.Find("Correct").GetComponent<AudioSource>().Play();
        }
        else
        {
            cashPanel.transform.Find("Correct").gameObject.SetActive(true);
            cashPanel.transform.Find("Correct").GetComponent<AudioSource>().Play();
            cashPanel.transform.Find("Correct/Text").GetComponent<Text>().text = _currentCorrectAnswer;
        }
    }

    private void ShowWrongAnswerPopup()
    {
        if (clickedButton != null)
        {
            clickedButton.transform.Find("Wrong").gameObject.SetActive(true);
            clickedButton.transform.Find("Wrong").GetComponent<AudioSource>().Play();
            ShowCorrectAnswer();
        }
        else
        {
            cashPanel.transform.Find("Wrong").gameObject.SetActive(true);
            cashPanel.transform.Find("Wrong").GetComponent<AudioSource>().Play();
            cashPanel.transform.Find("Wrong/Text").GetComponent<Text>().text = _currentCorrectAnswer;
        }
    }

    public void ShowCorrectAnswer()
    {
        if (carrePanel.activeSelf)
        {
            answerButtonsTetra[indicesTetra[0]].transform.Find("Correct").gameObject.SetActive(true);
        }
        else if (duoPanel.activeSelf)
        {
            answerButtonsBinaire[indicesBinary[0]].transform.Find("Correct").gameObject.SetActive(true);
        }
    }

    public void UpdateRound(int _round)
    {
        buttonRoundText.text = "Round " + _round;
        roundText.text = "Round " + (_round - 1) + "/" + GameManager.Instance.numberOfRounds;
    }

    public void DisplayNextScreen(bool _isRoundOver)
    {
        if (_isRoundOver)
        {
            ScorePanel.SetActive(true);
        }
        else
        {
            CategoriesPanel.SetActive(true);
        }
        duoPanel.SetActive(false);
        carrePanel.SetActive(false);
        cashPanel.SetActive(false);
        jokersPanel.SetActive(false);
        answerChoicePanel.SetActive(true);
    }

    public void DisplayEndScreen(List<PlayerStats> _players, List<int> _indicesPlayers, int indicePlaceExaequo = 0)
    {
        EndScreen.SetActive(true);
        UpdateFinalScorePanel((indicePlaceExaequo == 0 || indicePlaceExaequo == 2));
        if (indicePlaceExaequo == 0)
        {
            EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = "Félicitations " + _players[_indicesPlayers[0]].name + ", vous avez gagné cette manche !\n" + _players[_indicesPlayers[0]].name + " et " + _players[_indicesPlayers[1]].name + " sont qualifiés pour la finale.";
        }
        else if (indicePlaceExaequo == 1)
        {
            EndScreen.transform.Find("Finale Button").gameObject.SetActive(false);

            if (_indicesPlayers.Count == 2)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + " et " + _players[_indicesPlayers[1]].name
                    + " vous êtes ex aequo à la 1ère place !\n Bon bah faîtes un Shifumi pour savoir qui prendra la main à la finale.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 65;

                shifumiWinner.SetActive(true);
                shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnant du Shifumi ?";
                for (int i = 0; i < _indicesPlayers.Count; i++)
                {
                    GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                    go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
                }
            }
            else if (_indicesPlayers.Count == 3)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + " et " + _players[_indicesPlayers[2]].name + " vous êtes ex aequo à la 1ère place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 70;

                shifumiWinner.SetActive(true);
                shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnants du Shifumi ?\n (le 1er puis le 2nd)";
                for (int i = 0; i < _indicesPlayers.Count; i++)
                {
                    GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                    go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
                }
            }
            else if (_indicesPlayers.Count == 4)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + " et " + _players[_indicesPlayers[3]].name + " vous êtes ex aequo à la 1ère place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 67;
                shifumiWinner.SetActive(true);
                shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnants du Shifumi ?\n (le 1er puis le 2nd)";
                for (int i = 0; i < _indicesPlayers.Count; i++)
                {
                    GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                    go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
                }
            }
            else if (_indicesPlayers.Count == 5)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + ", " + _players[_indicesPlayers[3]].name + " et " + _players[_indicesPlayers[4]].name + " vous êtes ex aequo à la 1ère place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 65;
                shifumiWinner.SetActive(true);
                shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnants du Shifumi ?\n (le 1er puis le 2nd)";
                for (int i = 0; i < _indicesPlayers.Count; i++)
                {
                    GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                    go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
                }
            }
            else if (_indicesPlayers.Count == 6)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + ", " + _players[_indicesPlayers[3]].name + ", " + _players[_indicesPlayers[4]].name
                    + " et " + _players[_indicesPlayers[5]].name + " vous êtes ex aequo à la 1ère place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 65;
                shifumiWinner.SetActive(true);
                shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnants du Shifumi ?\n (le 1er puis le 2nd)";
                for (int i = 0; i < _indicesPlayers.Count; i++)
                {
                    GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                    go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
                }
            }
            else if (_indicesPlayers.Count == 7)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + ", " + _players[_indicesPlayers[3]].name + ", " + _players[_indicesPlayers[4]].name
                     + ", " + _players[_indicesPlayers[5]].name + " et " + _players[_indicesPlayers[6]].name + " vous êtes ex aequo à la 1ère place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 63;
                shifumiWinner.SetActive(true);
                shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnants du Shifumi ?\n (le 1er puis le 2nd)";
                for (int i = 0; i < _indicesPlayers.Count; i++)
                {
                    GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                    go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
                }
            }


        }
        else if (indicePlaceExaequo == 2)
        {
            EndScreen.transform.Find("Finale Button").gameObject.SetActive(false);

            if (_indicesPlayers.Count == 2)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + " et " + _players[_indicesPlayers[1]].name
                    + " vous êtes ex aequo à la 2nde place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 72;

            }
            else if (_indicesPlayers.Count == 3)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + " et " + _players[_indicesPlayers[2]].name + " vous êtes ex aequo à la 2nde place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 70;

            }
            else if (_indicesPlayers.Count == 4)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + " et " + _players[_indicesPlayers[3]].name + " vous êtes ex aequo à la 2nde place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 67;

            }
            else if (_indicesPlayers.Count == 5)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + ", " + _players[_indicesPlayers[3]].name + " et " + _players[_indicesPlayers[4]].name + " vous êtes ex aequo à la 2nde place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 65;

            }
            else if (_indicesPlayers.Count == 6)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + ", " + _players[_indicesPlayers[3]].name + ", " + _players[_indicesPlayers[4]].name +
                    " et " + _players[_indicesPlayers[5]].name + " vous êtes ex aequo à la 2nde place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 63;

            }
            else if (_indicesPlayers.Count == 7)
            {
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = _players[_indicesPlayers[0]].name + ", " + _players[_indicesPlayers[1]].name
                    + ", " + _players[_indicesPlayers[2]].name + ", " + _players[_indicesPlayers[3]].name + ", " + _players[_indicesPlayers[4]].name +
                     ", " + _players[_indicesPlayers[5]].name + " et " + _players[_indicesPlayers[6]].name + " vous êtes ex aequo à la 2nde place !\n Bon bah faîtes un Shifumi hein.";
                EndScreen.transform.Find("Winner Text").GetComponent<Text>().fontSize = 60;

            }

            shifumiWinner.SetActive(true);
            shifumiWinner.transform.Find("Text").GetComponent<Text>().text = "Gagnant du Shifumi ?";
            for (int i = 0; i < _indicesPlayers.Count; i++)
            {
                GameObject go = (GameObject)Instantiate(prefabShifumiButton);
                go.transform.SetParent(shifumiWinner.transform.Find("Buttons"));
                go.transform.localScale = new Vector3(1f, 1f, 1f);
                go.transform.Find("Text").GetComponent<Text>().text = _players[_indicesPlayers[i]].name;
            }
        }
    }

    public void ToggleJokerValidation(string _joker, bool _active)
    {
        for (int i = 0; i < GameManager.Instance.jokers.Length; i++)
        {
            if (GameManager.Instance.jokers[i] == _joker)
                validationPanels[i].SetActive(_active);
            else
                validationPanels[i].SetActive(false);
        }
    }

    public void DisplayFinaleButton()
    {
        EndScreen.transform.Find("Finale Button").gameObject.SetActive(true);
    }


    public void UndisplayShifumiButton()
    {
        shifumiWinner.SetActive(false);
    }

    public void DisplayJokerValidation(string _joker)
    {
        ToggleJokerValidation(_joker, true);
    }



    public void DisplayTimer(bool active)
    {
        timerPanel.SetActive(active);
        if (active == true)
            timerPanel.GetComponent<Timer>().Initialize();
    }

    public void UpdatePointsToScore(int _multiplier)
    {
        pointsToScore.GetChild(0).Find("Text").GetComponent<Text>().text = "+" + (1 * _multiplier).ToString();
        pointsToScore.GetChild(1).Find("Text").GetComponent<Text>().text = "+" + (3 * _multiplier).ToString();
        pointsToScore.GetChild(2).Find("Text").GetComponent<Text>().text = "+" + (5 * _multiplier).ToString();
    }

    public void UpdateJokers(PlayerStats player, float timeToRefresh)
    {
        StartCoroutine(UpdateJokersCo(player, timeToRefresh));
    }

    public IEnumerator UpdateJokersCo(PlayerStats player, float timeToRefresh)
    {
        yield return new WaitForSeconds(timeToRefresh);
        for (int i = 0; i < player.jokers.Length; i++)
        {
            ColorBlock colorButton = jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().colors;
            if (player.jokers[i] == true)
            {
                jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().interactable = true;
                colorButton.normalColor = Color.white;
                jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().colors = colorButton;
                jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().transform.Find("Text").GetComponent<Text>().color = Color.white;
            }
            else
            {
                jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().interactable = false;
                colorButton.normalColor = new Color(0.6f, 0.6f, 0.6f, 1);
                jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().colors = colorButton;
                jokersPanel.transform.GetChild(i).Find("Button").GetComponent<Button>().transform.Find("Text").GetComponent<Text>().color = new Color(0.6f, 0.6f, 0.6f, 1);
            }
        }
    }

    public void JokerSwiitchUI()
    {
        duoPanel.SetActive(false);
        carrePanel.SetActive(false);
        cashPanel.SetActive(false);
        answerChoicePanel.SetActive(true);
    }

    public void TogglePointMultiplier(bool _enabled)
    {
        pointsMultiplierText.SetActive(_enabled);
    }

    #endregion
}


