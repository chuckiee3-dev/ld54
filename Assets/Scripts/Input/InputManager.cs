using System;
using System.Collections.Generic;
using VContainer.Unity;

public class InputManager : IInitializable, IDisposable
{
    private readonly IInputEvents inputEvents;
    private readonly IWordEvents wordEvents;
    private List<string> allWordsUsed = new List<string>();
    private string focusedWord = "";
    private string writtenStr = "";
    private List<bool> correctness = new List<bool>();
    private Tower tower;
    public InputManager(IInputEvents inputEvents, IWordEvents wordEvents, Tower tower)
    {
        this.inputEvents = inputEvents;
        this.wordEvents = wordEvents;
        this.tower = tower;
    }

    public void Initialize()
    {
        inputEvents.OnCharPressed += ProcessChar;
        inputEvents.OnSpacePressed += ProcessSpace;
        inputEvents.OnBackspaceUsed += ProcessBackspace;
        wordEvents.OnWordSpawned += AddWordToList;
        wordEvents.OnWordDestroyed += RemoveFromList;
    }

    private void ProcessSpace()
    {
        ProcessChar('·');
    }

    private void RemoveFromList(string word)
    {
        allWordsUsed.Remove(word);
        focusedWord = "";
        writtenStr = "";
    }

    private void AddWordToList(string word)
    {
        allWordsUsed.Add(word);
    }

    public void Dispose()
    {
        if (inputEvents != null)
        {
            inputEvents.OnCharPressed -= ProcessChar;
            inputEvents.OnBackspaceUsed -= ProcessBackspace;
            inputEvents.OnSpacePressed -= ProcessSpace;
        }

        if (wordEvents != null)
        {
            wordEvents.OnWordSpawned -= AddWordToList;
            wordEvents.OnWordDestroyed -= RemoveFromList;
        }
    }

    private void ProcessChar(char inputChar)
    {
        if (!tower.HasWorker())
        {
            return;
        }
        if (string.IsNullOrEmpty(focusedWord))
        {
            correctness.Clear();

            foreach (var word in allWordsUsed)
            {
                if (word[0] == inputChar)
                {
                    focusedWord = word;
                    writtenStr = "";
                    correctness.Add(true);
                    if (correctness[correctness.Count - 1])
                    {
                        inputEvents.OnCorrectCharacter?.Invoke();
                    }
                    else
                    {
                        inputEvents.OnWrongCharacter?.Invoke();
                    }
                    writtenStr += inputChar;
                    wordEvents.OnWordProgressUpdated?.Invoke(focusedWord, correctness);

                    break;
                }
            }
        }
        else
        {
            //b
            //band
            if (writtenStr.Length < focusedWord.Length)
            {
                writtenStr += inputChar;
                int index = writtenStr.Length - 1;
                correctness.Add(writtenStr[index] == focusedWord[index]);
                if (correctness[correctness.Count - 1])
                {
                    inputEvents.OnCorrectCharacter?.Invoke();
                }
                else
                {
                    inputEvents.OnWrongCharacter?.Invoke();
                }
                wordEvents.OnWordProgressUpdated?.Invoke(focusedWord, correctness);
            }
        }

        if (focusedWord == writtenStr)
        {
            wordEvents.OnWordDestroyed?.Invoke(focusedWord);
        }
    }

    private void ProcessBackspace()
    {
        if (!tower.HasWorker())
        {
            return;
        }
        if (string.IsNullOrEmpty(focusedWord) || string.IsNullOrEmpty(writtenStr))
        {
            return;
        }
        inputEvents.OnDeleteCharacter?.Invoke();
        //b
        //band
        char lastChar = writtenStr[^1];
        if (lastChar == '·')
        {
            tower.EarnSpace(1);
        }
        writtenStr = writtenStr.Substring(0, writtenStr.Length - 1);
        if (correctness.Count > 0)
        {
            correctness.RemoveAt(correctness.Count - 1);
        }

        
        wordEvents.OnWordProgressUpdated?.Invoke(focusedWord, correctness);
        if (writtenStr.Length == 0)
        {
            focusedWord = "";
        }
        if (focusedWord == writtenStr)
        {
            wordEvents.OnWordDestroyed?.Invoke(focusedWord);
        }
    }
}