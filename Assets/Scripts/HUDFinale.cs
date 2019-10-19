using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUDFinale : MonoBehaviour
{
    #region fields

    public GameObject QuestionPropositionsScreen;
    public GameObject EndScreen;
    public GameObject binairePanel;
    public GameObject tetraPanel;
    public GameObject cashCashPanel;
    public GameObject answerChoicePanel;
    public Text categoryText;
    public GameObject finalThemeChoicePanel;
    public GameObject finalAnswerFirstPlayer;
    public GameObject finalAnswerSecondPlayer;
    public GameObject progressionQuestions;
    public GameObject firstText;
    [SerializeField]
    private Text questionText;
    [SerializeField]
    private Button[] answerButtonsTetra = new Button[4];
    [SerializeField]
    private Button[] answerButtonsBinaire = new Button[2];
    private readonly Transform pointsToScore;
    private UICommon uiCommon = new UICommon();

    #endregion

    #region properties

    public string CurrentCorrectAnswer { get; set; }

    #endregion

    #region constructors


    #endregion

    #region methods

    public void UpdateFinalScorePanel()
    {
        for (int i = 0; i < GameManager.Instance.numberOfPlayers; i++)
        {
            EndScreen.transform.Find("Score Panel/Scores").GetChild(i).gameObject.SetActive(true);
            EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.Instance.players[i].name;
            if (GameManager.Instance.FindBestPlayer() == i)
            {
                EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().color = Color.yellow;
                EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
            }
            EndScreen.transform.Find("Score Panel/Scores").GetChild(i).GetChild(1).GetComponent<Text>().text = GameManager.Instance.players[i].score.ToString();
        }
    }




    public void SetUIForFinaleCategories(string special1, string special2, string special3)
    {

        finalThemeChoicePanel.transform.Find("Buttons Group").GetChild(0).Find("Text").GetComponent<Text>().text = special1;
        finalThemeChoicePanel.transform.Find("Buttons Group").GetChild(1).Find("Text").GetComponent<Text>().text = special2;
        finalThemeChoicePanel.transform.Find("Buttons Group").GetChild(2).Find("Text").GetComponent<Text>().text = special3;
    }


    public void SetUpUIForPlayer(PlayerStats player)
    {
        finalThemeChoicePanel.SetActive(true);

        if (player.id == 0)
        {
            finalThemeChoicePanel.transform.Find("Text").GetComponent<Text>().text = player.name + ", vous avez été le meilleur lors de la manche précédente, veuillez choisir une catégorie :";
        }
        else
        {
            finalThemeChoicePanel.transform.Find("Text").GetComponent<Text>().text = player.name + ", c'est à vous de choisir une catégorie :";
        }

        foreach (Transform child in progressionQuestions.transform)
        {
            child.GetComponent<Image>().color = Color.white;
        }
    }

    public void SetupUIForQuestion(Text _category, QuizQuestion question)
    {
        progressionQuestions.transform.GetChild(GameManager.Instance.CurrentQuestionFinale - 1).GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        binairePanel.SetActive(false);
        tetraPanel.SetActive(false);
        cashCashPanel.SetActive(false);
        answerChoicePanel.SetActive(true);
        ToggleAnswerButtons(true);

        categoryText.text = _category.text;
        questionText.text = question.Question;
        questionText.fontSize = uiCommon.FixFontSizeForQuestion(questionText.text.Length);

        CurrentCorrectAnswer = question.CorrectAnswer[0];
        //Binaire répartition
        int[] indicesBinaire = uiCommon.ChooseRandomIndices(2);
        answerButtonsBinaire[indicesBinaire[0]].GetComponentInChildren<Text>().text = question.CorrectAnswer[0];
        answerButtonsBinaire[indicesBinaire[1]].GetComponentInChildren<Text>().text = question.WrongAnswers[UnityEngine.Random.Range(0, 3)];

        //Tetra répartition
        int[] indicesTetra = uiCommon.ChooseRandomIndices(4);
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

    public void HandleSubmittedAnswer(Text buttonText)
    {
        MakeNonInteractableAnswerButtons(buttonText);
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
    private void MakeNonInteractableAnswerButtons(Text _buttonText)
    {
        for (int i = 0; i < answerButtonsBinaire.Length; i++)
        {
            if (answerButtonsBinaire[i].gameObject.GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text != _buttonText.text)
            {
                answerButtonsBinaire[i].gameObject.GetComponent<Button>().enabled = false;
                answerButtonsBinaire[i].gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                answerButtonsBinaire[i].gameObject.GetComponent<Button>().interactable = false;
            }

        }
        for (int i = 0; i < answerButtonsTetra.Length; i++)
        {
            if (answerButtonsTetra[i].gameObject.GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text != _buttonText.text)
            {
                answerButtonsTetra[i].gameObject.GetComponent<Button>().enabled = false;
                answerButtonsTetra[i].gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                answerButtonsTetra[i].gameObject.GetComponent<Button>().interactable = false;
            }
        }

    }

    public void ShowAnswers(List<PlayerStats> _players, List<PlayersFinalAnswers> _player1Answers, List<PlayersFinalAnswers> _player2Answers)
    {
        finalAnswerFirstPlayer.transform.Find("Title/Text").GetComponent<Text>().text = _players[0].name;
        finalAnswerSecondPlayer.transform.Find("Title/Text").GetComponent<Text>().text = _players[1].name;
        StartCoroutine(DisplayPlayerAnswersCo(_players, _player1Answers, _player2Answers));
    }

    public IEnumerator DisplayPlayerAnswersCo(List<PlayerStats> _players, List<PlayersFinalAnswers> _player1Answers, List<PlayersFinalAnswers> _player2Answers)
    {
        yield return new WaitForSeconds(3f);
        int index = 0;
        foreach (Transform child in finalAnswerFirstPlayer.transform.Find("Questions List"))
        {

            if (_player1Answers[index]._question.Length > 184)
            {
                child.Find("Intitule").GetComponent<Text>().text = _player1Answers[index]._question.Substring(0, 181) + "...";
            }
            else
            {
                child.Find("Intitule").GetComponent<Text>().text = _player1Answers[index]._question;
            }

            if (_player1Answers[index]._answersPlayer.Length > 32)
            {
                child.Find("Player Answer").GetComponent<Text>().text = _player1Answers[index]._answersPlayer.Substring(0, 29) + "...";
            }
            else
            {
                child.Find("Player Answer").GetComponent<Text>().text = _player1Answers[index]._answersPlayer;
            }

            if (_player1Answers[index]._isCorrect)
            {
                child.Find("Correct Icon").gameObject.SetActive(true);

                if (_player1Answers[index]._correctAnswer.Length > 32)
                    child.Find("Correct Icon/Correct Answer").GetComponent<Text>().text = _player1Answers[index]._correctAnswer.Substring(0, 29) + "...";
                else
                    child.Find("Correct Icon/Correct Answer").GetComponent<Text>().text = _player1Answers[index]._correctAnswer;
            }
            else
            {
                child.Find("Wrong Icon").gameObject.SetActive(true);
                if (_player1Answers[index]._correctAnswer.Length > 32)
                    child.Find("Wrong Icon/Correct Answer").GetComponent<Text>().text = _player1Answers[index]._correctAnswer.Substring(0, 29) + "...";
                else
                    child.Find("Wrong Icon/Correct Answer").GetComponent<Text>().text = _player1Answers[index]._correctAnswer;
            }
            index++;
        }
        if (_players[0].score > 1)
            finalAnswerFirstPlayer.transform.Find("Score Text").GetComponent<Text>().text = _players[0].score + " points";
        else
            finalAnswerFirstPlayer.transform.Find("Score Text").GetComponent<Text>().text = _players[0].score + " point";


        yield return new WaitForSeconds(3f);
        index = 0;
        foreach (Transform child in finalAnswerSecondPlayer.transform.Find("Questions List"))
        {
            if (_player2Answers[index]._question.Length > 184)
                child.Find("Intitule").GetComponent<Text>().text = _player2Answers[index]._question.Substring(0, 181) + "...";
            else
                child.Find("Intitule").GetComponent<Text>().text = _player2Answers[index]._question;

            if (_player2Answers[index]._answersPlayer.Length > 32)
                child.Find("Player Answer").GetComponent<Text>().text = _player2Answers[index]._answersPlayer.Substring(0, 29) + "...";
            else
                child.Find("Player Answer").GetComponent<Text>().text = _player2Answers[index]._answersPlayer;

            if (_player2Answers[index]._isCorrect)
            {
                child.Find("Correct Icon").gameObject.SetActive(true);

                if (_player2Answers[index]._correctAnswer.Length > 32)
                    child.Find("Correct Icon/Correct Answer").GetComponent<Text>().text = _player2Answers[index]._correctAnswer.Substring(0, 29) + "...";
                else
                    child.Find("Correct Icon/Correct Answer").GetComponent<Text>().text = _player2Answers[index]._correctAnswer;

            }
            else
            {
                child.Find("Wrong Icon").gameObject.SetActive(true);

                if (_player2Answers[index]._correctAnswer.Length > 32)
                    child.Find("Wrong Icon/Correct Answer").GetComponent<Text>().text = _player2Answers[index]._correctAnswer.Substring(0, 29) + "...";
                else
                    child.Find("Wrong Icon/Correct Answer").GetComponent<Text>().text = _player2Answers[index]._correctAnswer;
            }
            index++;
        }
        if (_players[1].score > 1)
            finalAnswerSecondPlayer.transform.Find("Score Text").GetComponent<Text>().text = _players[1].score + " points";
        else
            finalAnswerSecondPlayer.transform.Find("Score Text").GetComponent<Text>().text = _players[1].score + " point";

        EndScreen.GetComponent<AudioSource>().loop = false;

    }

    public void DisplayFinalScreen(List<PlayerStats> _players, List<PlayersFinalAnswers> _player1Answers, List<PlayersFinalAnswers> _player2Answers, PlayerStats _bestPlayer)
    {
        EndScreen.SetActive(true);
        QuestionPropositionsScreen.SetActive(false);
        StartCoroutine(FirstTextCo());
        ShowAnswers(_players, _player1Answers, _player2Answers);

        StartCoroutine(DisplayWinnerCo(_bestPlayer));

    }

    public IEnumerator FirstTextCo()
    {
        yield return new WaitForSeconds(2f);
        firstText.SetActive(false);
    }

    public IEnumerator DisplayWinnerCo(PlayerStats _bestPlayer)
    {
        yield return new WaitForSeconds(6f);
        if (_bestPlayer.score > 1)
            EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = "Le gagnant est " + _bestPlayer.name + " avec un score de " + _bestPlayer.score + " points !";
        else
            EndScreen.transform.Find("Winner Text").GetComponent<Text>().text = "Le gagnant est " + _bestPlayer.name + " avec un score de " + _bestPlayer.score + " point !";

        EndScreen.transform.Find("Winner Text").GetComponent<AudioSource>().Play();
    }


    #endregion


}


