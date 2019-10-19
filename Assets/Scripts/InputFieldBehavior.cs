using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldBehavior : MonoBehaviour
{

    #region fields

    private bool isAnswerSubmitted;
    private InputField inputField;


    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    private void Awake()
    {
        inputField = transform.GetComponent<InputField>();
        isAnswerSubmitted = false;
    }

    public void Update()
    {
        if (!isAnswerSubmitted && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            isAnswerSubmitted = true;
            inputField.interactable = !isAnswerSubmitted;

            if (!GameManager.Instance.FinaleRound)
            {
                GameManager.Instance.SubmitCashAnswer(inputField.text);
            }
            else
            {
                GameManager.Instance.SubmitFinaleCashAnswer(inputField.text);
            }

            StartCoroutine(ValidateAnswer());
        }
    }


    private IEnumerator ValidateAnswer()
    {
        float time = 2.9f;
        if (GameManager.Instance.FinaleRound)
        {
            time = 0.9f;
        }
        yield return new WaitForSeconds(time);
        inputField.text = "";
        isAnswerSubmitted = false;
        inputField.interactable = !isAnswerSubmitted;
    }


    #endregion
}
