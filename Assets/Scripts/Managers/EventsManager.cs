using System.Collections;
using TMPro;
using UnityEngine;



public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    [SerializeField] private GameObject eventsPanel;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private AudioClip sound;
    
    private float panelTime = 4;
    private bool investmentEventDone = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Hide panel at the start of the game
        eventsPanel.SetActive(false);
    }



    private IEnumerator ShowEvent(string message)
    {
        eventText.text = message;

        eventsPanel.SetActive(true);

        yield return new WaitForSeconds(panelTime);
        
        eventsPanel.SetActive(false);
    }



    public void InvokeInvestmentEvent()
    {
        investmentEventDone = true;
        // Show panel for 3 seconds
        StartCoroutine(ShowEvent("<b>Bank</b>\n" + "Monthly bank account recap. Current balance: " + Invest.MONEY_AFTER_INVESTING + "$"));
        SoundManager.instance.PlaySound(sound, transform, 1, 1);
    }



    public bool IsInvestmentEventDone()
    {
        return investmentEventDone;
    }



    public void InvokeWarningEvent()
    {
        // Show panel
        StartCoroutine(ShowEvent("<b><color=\"red\">Warning</color></b>\n" + "Trying to jump from this zone might not end well. Retry at your own risk."));
        SoundManager.instance.PlaySound(sound, transform, 1, 1);
    }
}