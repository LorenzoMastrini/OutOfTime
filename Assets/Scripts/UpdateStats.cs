using TMPro;
using UnityEngine;

public class UpdateStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bodyStatText;
    [SerializeField] private TextMeshProUGUI cultureStatText;
    [SerializeField] private TextMeshProUGUI studyStatText;



    private void Start()
    {
        GameManager.instance.playerStats.OnBodyUpdate += UpdateBodyUI;
        GameManager.instance.playerStats.OnCultureUpdate += UpdateCultureUI;
        GameManager.instance.playerStats.OnStudyUpdate += UpdateStudyUI;

        // Sync iniziale
        UpdateBodyUI(GameManager.instance.playerStats.GetBody());
        UpdateCultureUI(GameManager.instance.playerStats.GetCulture());
        UpdateStudyUI(GameManager.instance.playerStats.GetStudy());
    }



    private void OnDestroy()
    {
        GameManager.instance.playerStats.OnBodyUpdate -= UpdateBodyUI;
        GameManager.instance.playerStats.OnCultureUpdate -= UpdateCultureUI;
        GameManager.instance.playerStats.OnStudyUpdate -= UpdateStudyUI;
    }


    private void UpdateBodyUI(int value)
    {
        bodyStatText.text = $"{value}";
    }

    private void UpdateCultureUI(int value)
    {
        cultureStatText.text = $"{value}";
    }

    private void UpdateStudyUI(int value)
    {
        studyStatText.text = $"{value}";
    }
}
