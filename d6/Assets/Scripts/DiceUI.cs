using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUI : MonoBehaviour
{
    public Image DiceColor;
    public Text DiceNumber;

    public Vector2Int DicePointer;

    public Dice dice;

    public bool isInSlot
    {
        get
        {
            return transform.parent.name == "Slot" ? true : false;
        }
    }

    public void SetDice(Dice dice)
    {
        DiceNumber.text = dice.Number.ToString();

        switch (dice.DiceColor)
        {
            case Color.Pink:
                DiceColor.color = newColor(154, 34, 92);
                break;
            case Color.LightBrown:
                DiceColor.color = newColor(184, 172, 142);
                break;
            case Color.Green:
                DiceColor.color = newColor(152, 169, 66);
                break;
            case Color.Cyan:
                DiceColor.color = newColor(28, 130, 116);
                break;
            case Color.GreyishGreen:
                DiceColor.color = newColor(96, 109, 96);
                break;
            default:
                break;
        }

        this.dice = dice;
    }

    public static UnityEngine.Color newColor(float r, float g, float b)
    {
        return new UnityEngine.Color(r / 255, g / 255, b / 255);
    }
}
