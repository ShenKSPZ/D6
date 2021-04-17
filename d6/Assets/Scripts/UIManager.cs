using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public Transform DiceListUIParent;
    public Text Score;
    public Text Chain;
    public Text HP;

    public GameObject Row;
    public GameObject Dice;

    public List<List<DiceUI>> diceUIList = new List<List<DiceUI>>();
    public List<List<Vector2>> gridPos = new List<List<Vector2>>();

    [HideInInspector]
    public List<GameObject> Rows = new List<GameObject>();

    public void ShowList(List<List<Dice>> DiceList)
    {
        for (int y = 0; y < DiceList.Count; y++)
        {
            Rows.Add(Instantiate(Row, DiceListUIParent, false));

            diceUIList.Add(new List<DiceUI>());

            for (int x = 0; x < DiceList[y].Count; x++)
            {
                GameObject currentDice = Instantiate(Dice, Rows[y].transform, false);
                DiceUI diceUI = currentDice.GetComponent<DiceUI>();

                diceUI.SetDice(DiceList[y][x]);

                diceUI.DicePointer.y = y;
                diceUI.DicePointer.x = x;

                diceUIList[y].Add(diceUI);
            }
        }

        StartCoroutine(IE_AssignPos());
    }

    IEnumerator IE_AssignPos()
    {
        //Wait For Next Frame
        yield return null;
        //Wait For End of Frame
        yield return new WaitForEndOfFrame();
        for (int y = 0; y < diceUIList.Count; y++)
        {
            gridPos.Add(new List<Vector2>());
            for (int x = 0; x < diceUIList[y].Count; x++)
            {
                gridPos[y].Add(diceUIList[y][x].transform.position);
            }
        }
        EnableLayoutGroup(false);
    }

    public Vector2 GetPos(Vector2Int Pos)
    {
        return gridPos[Pos.y][Pos.x];
    }

    public void AddDice(Dice newDice, Vector2Int Pos)
    {
        GameObject currentDice = Instantiate(Dice, Rows[Pos.y].transform, false);
        currentDice.transform.SetSiblingIndex(Pos.x);
        DiceUI diceUI = currentDice.GetComponent<DiceUI>();
        diceUI.SetDice(newDice);
        diceUI.DicePointer = Pos;
        Vector3 TargetPos = GetPos(Pos);
        diceUI.transform.position = new Vector3(TargetPos.x, 1080 + 400);
        diceUI.transform.DOMove(TargetPos, 0.5f);
        diceUIList[Pos.y].Insert(Pos.x, diceUI);
    }

    public void EnableLayoutGroup(bool enable)
    {
        ContentSizeFitter[] fitters = GetComponentsInChildren<ContentSizeFitter>();
        HorizontalLayoutGroup[] hor = GetComponentsInChildren<HorizontalLayoutGroup>();
        VerticalLayoutGroup[] ver = GetComponentsInChildren<VerticalLayoutGroup>();

        for (int i = 0; i < fitters.Length; i++)
        {
            fitters[i].enabled = enable;
        }

        for (int i = 0; i < hor.Length; i++)
        {
            hor[i].enabled = enable;
        }

        for (int i = 0; i < ver.Length; i++)
        {
            ver[i].enabled = enable;
        }
    }
}
