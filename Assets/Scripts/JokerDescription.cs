using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JokerDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    #region fields

    public GameObject descriptionPanel;
    public GameObject validationPanel;
    public Text nameJoker;

    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!validationPanel.activeSelf && transform.Find("Button").GetComponent<Button>().interactable)
        {
            descriptionPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }

    public void UseJoker()
    {
        GameManager.Instance.UseJoker(nameJoker.text);
        validationPanel.SetActive(false);
    }

    public void CloseValidation()
    {
        validationPanel.SetActive(false);
    }

    public void JokerButton()
    {
        if (GameManager.Instance.ValidateJoker(nameJoker.text))
        {
            descriptionPanel.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.ValidateJoker(nameJoker.text))
        {
            descriptionPanel.SetActive(false);
        }
    }


    #endregion

}
