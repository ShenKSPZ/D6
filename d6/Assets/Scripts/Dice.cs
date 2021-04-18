using UnityEngine;

public enum D_Color
{
    Pink,
    Brown,
    Green,
    Cyan,
    DarkGreen,
    Purple,
}

[System.Serializable]
public class Dice
{
    //1 - 6
    public int Number;
    public D_Color DiceColor;

    public static Dice RandomDice()
    {
        //Get a new Dice
        Dice newDice = new Dice();

        //Get a random color from enum
        string[] colors = System.Enum.GetNames(typeof(D_Color));
        newDice.DiceColor = (D_Color)System.Enum.Parse(typeof(D_Color), colors[Random.Range(0, colors.Length)]);

        //Get a random number from 1 to 6
        newDice.Number = Random.Range(1, 7);

        return newDice;
    }
}
