using VContainer.Unity;

public interface IDictionaryProvider: IInitializable
{
  public char[] GetAlphabet();
  public string GetWord(int length, char startingChar);
  
  public delegate void DictionaryReady();
  public DictionaryReady OnDictionaryReady { get; set; }
}
