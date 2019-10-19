using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    #region fields

    public GameObject firstScreenPanel;
    public GameObject endScreenPanel;
    public GameObject timerTimer;

    private string textTimer;
    private bool wasMusicStopped;
    private float durationFirstScreen = 2f;

    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    public void Start()
    {
        wasMusicStopped = false;
        GetComponent<AudioSource>().Play();

        StartCoroutine(FirstScreenCo());
    }
    public void Update()
    {

        if (!wasMusicStopped && endScreenPanel.activeSelf)
        {
            GetComponent<AudioSource>().Stop();
            wasMusicStopped = true;
        }

        if (GameManager.Instance.FinaleRound)
        {
            return;
        }

        if (endScreenPanel.activeSelf)
        {
            return;
        }
        
        if ((timerTimer.activeSelf && !wasMusicStopped))
        {
            GetComponent<AudioSource>().Stop();
            wasMusicStopped = true;
        }
        else if ((!timerTimer.activeSelf && wasMusicStopped))
        {
            GetComponent<AudioSource>().Play();
            wasMusicStopped = false;
        }
    }

    private IEnumerator FirstScreenCo()
    {
        yield return new WaitForSeconds(durationFirstScreen);
        firstScreenPanel.SetActive(false);
    }

    #endregion
}
