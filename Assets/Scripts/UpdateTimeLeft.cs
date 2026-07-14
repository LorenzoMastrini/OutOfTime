using UnityEngine;
using TMPro;

public class UpdateTimeLeft : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLeftText;



    private void Start()
    {
        // Register to the event assuring that GameManager exists (it is created inside Awake())
        if (GameManager.instance != null)
        {
            GameManager.instance.OnTimeChanged += UpdateUI; // Subscribe to the event

            // Initial time value
            UpdateUI(GameManager.instance.TimeLeft);
        }
    }



    private void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnTimeChanged -= UpdateUI; // Unsubscribe from the event
    }



    private void UpdateUI(int time)
    {
        timeLeftText.text = $"Time left: {time}";
    }
}
