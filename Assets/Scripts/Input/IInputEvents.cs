using System;

public interface IInputEvents
{
   
    public delegate void UserEnteredString(string input);
    public UserEnteredString OnUserEnteredString { get; set; }
   
    public delegate void CharPressed(char input);
    public CharPressed OnCharPressed { get; set; }
    public delegate void BackspaceUsed();
    public BackspaceUsed OnBackspaceUsed { get; set; }
}