using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform DiceListUI;

    public GameObject Row;
    public GameObject Dice;

    public void ShowList(List<List<Dice>> DiceList)
    {
        for (int y = 0; y < DiceList.Count; y++)
        {
            GameObject currentRow = Instantiate(Row, DiceListUI, false);
            for (int x = 0; x < DiceList[y].Count; x++)
            {
                GameObject currentDice = Instantiate(Dice, currentRow.transform, false);
                DiceUI diceUI = currentDice.GetComponent<DiceUI>();
                diceUI.SetDice(DiceList[y][x]);
            }
        }
    }
}
