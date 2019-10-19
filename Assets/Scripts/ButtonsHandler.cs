using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsHandler : MonoBehaviour
{


    #region fields

    private UIController uiController;
    private Transform answerChoices;
    private Transform scoreRecapPanel;
    private HUDFinale uiFinale;
    private Toggle toggle;

    public GameObject binairePanel;
    public GameObject tetraPanel;
    public GameObject cacheCachePanel;
    public GameObject categoryPanel;
    public GameObject themeChoicePanel;

    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            answerChoices = uiController.gameObject.transform.Find("Question Proposition/Question Panel/Answer Choices");
            scoreRecapPanel = uiController.gameObject.transform.Find("Score Recap Panel");
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            toggle = GetComponent<Toggle>();
            if (toggle!=null)
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void Update()
    {
        if (uiController == null && SceneManager.GetActiveScene().buildIndex == 1)
        {
            uiController = FindObjectOfType<UIController>();
            answerChoices = uiController.gameObject.transform.Find("Question Proposition/Question Panel/Answer Choices");
            scoreRecapPanel = uiController.gameObject.transform.Find("Score Recap Panel");
        }
        else if (uiFinale==null && SceneManager.GetActiveScene().buildIndex == 2)
        {
            uiFinale = FindObjectOfType<HUDFinale>();
            answerChoices = uiFinale.gameObject.transform.Find("Question Proposition/Question Panel/Answer Choices");
        }
    }

    public void OnAnswerClick(Button button)
    {
        if (GameManager.Instance.FinaleRound)
        {
            GameManager.Instance.SubmitFinaleAnswer(button);
        }
        else
        {
            GameManager.Instance.SubmitAnswer(button);
        }
    }

    public void OnAnswerChoiceClick(Text _buttonText)
    {
        if (GameManager.Instance.FinaleRound)
        {
            GameManager.Instance.SetChoiceFinale(_buttonText);
        }
        else
        {
            GameManager.Instance.SetChoice(_buttonText);
        }

        if (_buttonText.text.ToString() == "BINAIRE")
        {
            binairePanel.SetActive(true);
        }
        else if (_buttonText.text.ToString() == "TETRA")
        {
            tetraPanel.SetActive(true);
        }
        else if (_buttonText.text.ToString() == "CASH-CASH")
        {
            cacheCachePanel.SetActive(true);
        }
        answerChoices.gameObject.SetActive(false);
    }



    public void OnCategoryClick(Text _buttonText)
    {
        GameManager.Instance.PresentQuestion(_buttonText);
        categoryPanel.SetActive(false);
    }

    public void OnFinalCategoryClick(Button button)
    {
        button.interactable = false;
        GameManager.Instance.PresentFinalQuestion(button.transform.Find("Text").GetComponent<Text>());
        themeChoicePanel.SetActive(false);
    }


    public void OnRoundClick()
    {
        categoryPanel.SetActive(true);
        GameManager.Instance.StartRound();
        scoreRecapPanel.gameObject.SetActive(false);
    }

    public void OnPlayButtonClick()
    {
        GameManager.Instance.LoadQuestionsScene();
    }

    public void SetNumberOfRounds(Text _roundText)
    {
        GameManager.Instance.numberOfRounds = int.Parse(_roundText.text.ToString());
    }

    public void SaveGame()
    {
        GameManager.Instance.GetComponent<QuestionCollection>().SerializeQuestions();
        GameManager.Instance.SaveGame();
    }

    public void BackToMenu()
    {
        GameManager.Instance.GetComponent<QuestionCollection>().SerializeQuestions();
        GameManager.Instance.BackToMenu();
    }

    public void StartFinale()
    {
        GameManager.Instance.GetComponent<QuestionCollection>().SerializeQuestions();
        SceneManager.LoadScene(2);
        GameManager.Instance.SetUpFinale();
    }

    public void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = Color.gray;
            cb.highlightedColor = Color.gray;
            SetNumberOfRounds(transform.Find("Text").GetComponent<Text>());
        }
        else
        {
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
        }
        toggle.colors = cb;
    }

    public void ShifumiWinner()
    {
        GetComponent<Button>().interactable = false;
        GameManager.Instance.AddShifumiWinner(transform.Find("Text").GetComponent<Text>().text);
    }


    #endregion
}
