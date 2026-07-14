using System;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    // Stats
    private int culture = 0;
    private int study = 0;
    private int body = 0;

    public event Action<int> OnBodyUpdate;
    public event Action<int> OnCultureUpdate;
    public event Action<int> OnStudyUpdate;

    public void AddCulture(int amount)
    {
        culture += amount;
        Debug.Log($"Added {amount} culture. Total culture: {culture}");

        // Notify listeners
        OnCultureUpdate?.Invoke(culture);
    }

    public void AddStudy(int amount)
    {
        study += amount;
        Debug.Log($"Added {amount} study. Total study: {study}");
        
        // Notify listeners
        OnStudyUpdate?.Invoke(study);

    }

    public void AddBody(int amount)
    {
        body += amount;
        Debug.Log($"Added {amount} body. Total body: {body}");

        // Notify listeners
        OnBodyUpdate?.Invoke(body);
    }

    public int GetCulture()
    {
        return culture;
    }

    public int GetStudy()
    {
        return study;
    }

    public int GetBody()
    {
        return body;
    }
}
