using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUI : MonoBehaviour
{
    public Image DiceColor;
    public Image DiceImage;

    public List<Sprite> DiceImageList;

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
        DiceImage.sprite = DiceImageList[dice.Number - 1];

        switch (dice.DiceColor)
        {
            case D_Color.Pink:
                DiceColor.color = newColor(154, 34, 92);
                break;
            case D_Color.Brown:
                DiceColor.color = newColor(184, 172, 142);
                break;
            case D_Color.Green:
                DiceColor.color = newColor(152, 169, 66);
                break;
            case D_Color.Cyan:
                DiceColor.color = newColor(28, 130, 116);
                break;
            case D_Color.DarkGreen:
                DiceColor.color = newColor(96, 109, 96);
                break;
            case D_Color.Purple:
                DiceColor.color = newColor(161, 118, 181);
                break;
            default:
                break;
        }

        this.dice = dice;
    }

    public static Color newColor(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }
}
