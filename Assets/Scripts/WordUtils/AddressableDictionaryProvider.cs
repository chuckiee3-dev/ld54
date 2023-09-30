using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

public class AddressableDictionaryProvider : IDictionaryProvider
{
    private char[] alphabet;
    private readonly SortedDictionary<int, int> lengthCounts = new();
    private readonly SortedDictionary<int, SortedDictionary<char, List<string>>> words = new ();
    public IDictionaryProvider.DictionaryReady OnDictionaryReady { get; set; }
    private int minLength;
    private int maxLength;
    public char[] GetAlphabet()
    {
        return alphabet;
    }

    public string GetWord(int length, char startingChar)
    {
        int maxTries = 5;
        int tries = 0;
        int initialLength = length;
        int increments = 0;
        while (tries < maxTries)
        {
            length = Math.Clamp(length, minLength, maxLength);
            bool lengthExists = words.Keys.Contains(length);
            if (lengthExists)
            {
                bool charExists = words[length].Keys.Contains(startingChar);
                if (charExists)
                {
                    var list = words[length][startingChar];
                    return list[Random.Range(0, list.Count)];
                }

                var wordsLengthList = words.Keys.AsReadOnlyList();
                var indexOf = wordsLengthList.IndexOf(initialLength);
                //Try decreasing number of characters to not increase difficulty
                if (indexOf - tries >= 0)
                {
                    length = wordsLengthList[indexOf - tries];
                }
                //if we couldn't find shortest length one then we get the larger length closest to the initial length
                else
                {
                    increments++;
                    length = wordsLengthList[indexOf + increments];
                }
                continue;
            }

            int closestLen = -1;
            int diff = 1000;
            foreach (var len in words.Keys)
            {
                int currDiff = Math.Abs(len - length);
                if (diff > currDiff)
                {
                    closestLen = len;
                    diff = currDiff;
                }
            }

            length = closestLen;
            tries++;
        }
        //We failed to find a word not every language has e r and o so we send !
        return "!!!!!";
    }


    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private async UniTask InitializeAsync()
    {
        List<char> allStartingCharacters = new List<char>();
        var opHandle = Addressables.LoadAssetAsync<TextAsset>("words");
        await opHandle.Task;
        var arrayString = opHandle.Task.Result.text.Split('\n');
        Debug.Log(arrayString.Length);
        foreach (var word in arrayString)
        {
            int len = word.Length;
            if (len == 0)
            {
                continue;
            }
            char firstChar = word[0];
            if (!allStartingCharacters.Contains(firstChar))
            {
                allStartingCharacters.Add(firstChar);
            }

            if (lengthCounts.Keys.Contains(len))
            {
                lengthCounts[len] += 1;
            }
            else
            {
                lengthCounts.Add(len, 1);
                words.Add(len, new SortedDictionary<char, List<string>>());
            }

            if (!words[len].Keys.Contains(firstChar))
            {
                words[len].Add(firstChar, new List<string>());
            }
            words[len][firstChar].Add(word);
        }

        maxLength = -1;
        minLength = 1000;
        foreach (var lengthKey in words.Keys)
        {
            if (maxLength < lengthKey)
            {
                maxLength = lengthKey;
            }
            if (minLength > lengthKey)
            {
                minLength = lengthKey;
            }

            foreach (var charKey in words[lengthKey].Keys)
            {
                words[lengthKey][charKey].Sort();
            }
        }
        Addressables.Release(opHandle);
        alphabet = allStartingCharacters.ToArray();
        OnDictionaryReady?.Invoke();
    }
}