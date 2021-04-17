using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    Pink,
    LightBrown,
    Green,
    Cyan,
    GreyishGreen,
}

public class Dice
{
    //1 - 6
    public int Number;
    public Color DiceColor;

    public static Dice GetRandomDice()
    {
        Dice newDice = new Dice();
        newDice.Number = Random.Range(1, 7);
        string[] colors = System.Enum.GetNames(typeof(Color));
        newDice.DiceColor = (Color)System.Enum.Parse(typeof(Color), colors[Random.Range(0, colors.Length)]);
        return newDice;
    }
}
