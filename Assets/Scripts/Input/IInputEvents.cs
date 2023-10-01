
public interface IInputEvents
{
   
   
    public delegate void CharPressed(char input);
    public CharPressed OnCharPressed { get; set; }
    public delegate void BackspaceUsed();
    public BackspaceUsed OnBackspaceUsed { get; set; }
    public delegate void SpacePressed();
    public SpacePressed OnSpacePressed { get; set; }
    public delegate void CorrectCharacter();
    public CorrectCharacter OnCorrectCharacter { get; set; }
    public delegate void WrongCharacter();
    public WrongCharacter OnWrongCharacter { get; set; }
    public delegate void DeleteCharacter();
    public DeleteCharacter OnDeleteCharacter { get; set; }
}