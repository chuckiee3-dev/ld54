using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wordTmp;
    public Action OnWordCompleted;
    private bool isActive;

    public void UpdateStatus(string word, List<bool> correctness)
    {
        if (!isActive)
        {
            return;
        }

        if (correctness.Count == word.Length)
        {
            bool isAllCorrect = true;
            foreach (var letterCorrectness in correctness)
            {
                isAllCorrect &= letterCorrectness;
            }

            if (isAllCorrect)
            {
                OnWordCompleted?.Invoke();
                return;
            }
        }

        string resultWithColors = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (i < correctness.Count)
            {
                resultWithColors += correctness[i]
                    ? "<color=green>" + word[i] + "</color>"
                    : "<color=red>" + word[i] + "</color>";
            }
            else
            {
                resultWithColors += word[i];
            }
        }

        wordTmp.text = resultWithColors;
    }

    public void Show()
    {
        wordTmp.enabled = true;
        isActive = true;
    }

    public void Hide()
    {
        wordTmp.enabled = false;
        isActive = false;
    }
}