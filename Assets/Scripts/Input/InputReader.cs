using UnityEngine;
using VContainer;

public class InputReader : MonoBehaviour
{
    private IInputEvents inputEvents;

    [Inject]
    public void Construct(IInputEvents inputEvents)
    {
        this.inputEvents = inputEvents;
    }
    
    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                
                inputEvents.OnBackspaceUsed?.Invoke();
            }
            else if ((c == '\n') || (c == '\r')|| (c == '\t')) // enter/return
            {
            }
            else
            {
                inputEvents.OnCharPressed?.Invoke(c);
            }
        }

    }
}