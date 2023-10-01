using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordTracker: IInitializable, IDisposable
{
    private readonly List<char> startCharactersOnScreen;
    private List<char> allCharacters;
    private List<char> unusedCharacters;
    private readonly IDictionaryProvider dictionaryProvider;
    private readonly IWordEvents wordEvents;

    public WordTracker(IWordEvents wordEvents, IDictionaryProvider dictionaryProvider)
    {
        startCharactersOnScreen = new List<char>();
        this.dictionaryProvider = dictionaryProvider;
        dictionaryProvider.OnDictionaryReady += SetupLetters;
        this.wordEvents = wordEvents;
    }

    private void SetupLetters()
    {
        unusedCharacters = dictionaryProvider.GetAlphabet().ToList();
        allCharacters = dictionaryProvider.GetAlphabet().ToList();;
    }

    public void Initialize()
    {
        wordEvents.OnWordSpawned += WordSpawned;
        wordEvents.OnWordDestroyed += WordDestroyed;       

    }

    public void Dispose()
    {
        if (wordEvents != null)
        {
            wordEvents.OnWordSpawned -= WordSpawned;
            wordEvents.OnWordDestroyed -= WordDestroyed;
        }

        if (dictionaryProvider != null)
        {
            dictionaryProvider.OnDictionaryReady -= SetupLetters;
        }
    }
    private void WordDestroyed(string word)
    {
        char firstChar = word[0];
        if (!unusedCharacters.Contains(firstChar))
        {
            unusedCharacters.Add(firstChar);
        }
        if (startCharactersOnScreen.Contains(firstChar))
        {
            startCharactersOnScreen.Remove(firstChar);
        }
    }

    private void WordSpawned(string word)
    {
        char firstChar = word[0];
        if (!startCharactersOnScreen.Contains(firstChar))
        {
            startCharactersOnScreen.Add(firstChar);
        }
        if (unusedCharacters.Contains(firstChar))
        {
            unusedCharacters.Remove(firstChar);
        }
    }

    public char GetUnusedChar()
    {
        if (unusedCharacters.Count > 0)
        {
            return unusedCharacters[Random.Range(0, unusedCharacters.Count)];
        }
        Debug.Log("All characters are used!");
        return startCharactersOnScreen[0];
    }
    public char GetRandomChar()
    { 
        return allCharacters[Random.Range(0, allCharacters.Count)];
    }

    public string GetRandomUnusedWord(int length)
    {
        return dictionaryProvider.GetWord(length, GetUnusedChar());
    }
    public string GetRandomUnusedWordWithSpaces(int length, int spaceCount)
    {
        if (spaceCount == 0)
        {
            return GetRandomUnusedWord(length);
        }
        string text = dictionaryProvider.GetWord(length, GetUnusedChar());
        for (int i = 0; i < spaceCount; i++)
        {
            text += "·";
            text += dictionaryProvider.GetWord(length, GetRandomChar());
        }

        return text;
    }
    
}
