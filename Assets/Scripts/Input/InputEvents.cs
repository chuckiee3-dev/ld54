public class InputEvents : IInputEvents
{
    public IInputEvents.CharPressed OnCharPressed { get; set; }
    public IInputEvents.BackspaceUsed OnBackspaceUsed { get; set; }
    public IInputEvents.SpacePressed OnSpacePressed { get; set; }
    public IInputEvents.CorrectCharacter OnCorrectCharacter { get; set; }
    public IInputEvents.WrongCharacter OnWrongCharacter { get; set; }
    public IInputEvents.DeleteCharacter OnDeleteCharacter { get; set; }
}