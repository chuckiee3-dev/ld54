using UnityEngine;
using VContainer;

public class InputReader : MonoBehaviour
{
    private IInputEvents inputEvents;
    private IBaseEvents baseEvents;

    [Inject]
    public void Construct(IInputEvents inputEvents, IBaseEvents baseEvents)
    {
        this.inputEvents = inputEvents;
        this.baseEvents = baseEvents;
        baseEvents.OnSpaceGranted += ConsumeSpace;
    }

    private void ConsumeSpace(int amount)
    {
        Debug.Log("ConsumeSpace");
        for (int i = 0; i < amount; i++)
        {
            inputEvents.OnSpacePressed?.Invoke();
        }
    }

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                
                inputEvents.OnBackspaceUsed?.Invoke();
            }
            else if ((c == '\n') || (c == '\r')|| (c == '\t') || c== '·') // enter/return
            {
            }else if (c == ' ')
            {
                Debug.Log("baseEvents.OnSpaceRequested");
                baseEvents.OnSpaceRequested?.Invoke(1);
            }
            else
            {
                inputEvents.OnCharPressed?.Invoke(c);
            }
        }

    }
}