using System;

[Serializable]
public class Expression
{
    public enum Blushies
    {
        None = 0,
        Low = 1,
        Med = 2, 
        High = 3
    }

    public enum EyeEdits
    {
        None = 0,
        Hearts = 1,
        Starts = 2,
        Tears = 3
    }

    public string name;
    public Blushies blushies;
    public EyeEdits eyeEdits;
    public int[] expressionArray;
}
