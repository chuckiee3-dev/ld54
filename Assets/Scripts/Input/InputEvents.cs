public class InputEvents : IInputEvents
{
    public IInputEvents.UserEnteredString OnUserEnteredString { get; set; }
    public IInputEvents.CharPressed OnCharPressed { get; set; }
    public IInputEvents.BackspaceUsed OnBackspaceUsed { get; set; }
}