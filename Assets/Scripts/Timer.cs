using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region fields

    private float leftSeconds;
    private bool isTimerLaunched;
    private bool wasMusicLaunched;
    public GameObject Button;
    public AudioClip endSound;
    public AudioClip durationSound;

    #endregion

    #region properties


    #endregion

    #region constructors


    #endregion

    #region methods

    public void Start()
    {
        Initialize();
    }

    private void LaunchTimer()
    {
        isTimerLaunched = true;
        Button.SetActive(false);
    }

    /// <summary>
    /// Resets the timer to 30 seconds.
    /// </summary>
    public void Initialize()
    {
        if (wasMusicLaunched)
        {
            GetComponent<AudioSource>().Stop();
            wasMusicLaunched = false;
        }
        Button.SetActive(true);
        isTimerLaunched = false;
        leftSeconds = 30;
        transform.Find("Text").GetComponent<Text>().text = "30";

    }

    private void Update()
    {
        if (isTimerLaunched)
        {
            if (!wasMusicLaunched)
            {
                GetComponent<AudioSource>().clip = durationSound;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();
                wasMusicLaunched = true;
            }

            leftSeconds -= Time.deltaTime;
            if (leftSeconds > 0)
            {
                transform.Find("Text").GetComponent<Text>().text = ((int)leftSeconds).ToString();
            }
            else
            {
                transform.Find("Text").GetComponent<Text>().text = "STOP!";
                isTimerLaunched = false;
                if (wasMusicLaunched)
                {
                    GetComponent<AudioSource>().Stop();
                    GetComponent<AudioSource>().loop = false;
                    GetComponent<AudioSource>().clip = endSound;
                    GetComponent<AudioSource>().Play();
                    wasMusicLaunched = false;
                }
            }

        }

        
           
    }

    #endregion
    
}
