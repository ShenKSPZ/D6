using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameManager GM;

    public RectTransform Canvas;
    public Transform DiceListUIParent;
    public Text Score;
    public Text Chain;
    public ProgressBar Bar;
    public Image BlackScreen;
    public HP_UI HP;

    [Space(10)]
    public RectTransform Win;
    public RectTransform Lose;
    public RectTransform Retry;

    public GameObject Row;
    public GameObject Dice;

    //First List use to store row
    //Second List use to store Column
    public List<List<DiceUI>> DiceUIList = new List<List<DiceUI>>();

    //Use to store the postion of grid
    public List<List<Vector2>> LayoutPos = new List<List<Vector2>>();

    [HideInInspector]
    public List<GameObject> Rows = new List<GameObject>();

    private void Awake()
    {
        BlackScreen.color = Color.black;
    }

    public void ShowList(List<List<Dice>> DiceList)
    {
        for (int y = 0; y < DiceList.Count; y++)
        {
            Rows.Add(Instantiate(Row, DiceListUIParent, false));

            DiceUIList.Add(new List<DiceUI>());

            for (int x = 0; x < DiceList[y].Count; x++)
            {
                GameObject currentDice = Instantiate(Dice, Rows[y].transform, false);
                DiceUI diceUI = currentDice.GetComponent<DiceUI>();

                diceUI.SetDice(DiceList[y][x]);

                diceUI.DicePointer.y = y;
                diceUI.DicePointer.x = x;

                DiceUIList[y].Add(diceUI);
            }
        }

        //Using coroutine because the unity's layout group only update in next frame, so we have to wait to next frame to get the grid position
        StartCoroutine(IE_AssignPos());
    }

    IEnumerator IE_AssignPos()
    {
        //Wait For Next Frame
        yield return null;
        //Wait For End of Frame
        yield return new WaitForEndOfFrame();
        //Store the current position of all dice to a list
        for (int y = 0; y < DiceUIList.Count; y++)
        {
            LayoutPos.Add(new List<Vector2>());
            for (int x = 0; x < DiceUIList[y].Count; x++)
            {
                Vector2 NeedStore = DiceUIList[y][x].transform.position;
                NeedStore.x /= Canvas.localScale.x;
                NeedStore.y /= Canvas.localScale.y;
                LayoutPos[y].Add(NeedStore);
            }
        }
        //disable layout group
        EnableLayoutGroup(false);
        GM.IsInAnimation = true;

        for (int y = 0; y < DiceUIList.Count; y++)
        {
            for (int x = 0; x < DiceUIList[y].Count; x++)
            {
                DiceUIList[y][x].transform.position = new Vector3(DiceUIList[y][x].transform.position.x, 1080 + 300);
            }
        }

        BlackScreen.DOColor(Color.clear, 0.2f);
        yield return new WaitForSeconds(0.1f);

        for (int y = 1; y >= 0; y--)
        {
            for (int x = 0; x < DiceUIList[y].Count; x++)
            {
                DiceUIList[y][x].transform.DOMoveY(GetPos(new Vector2Int(x, y)).y, 0.5f);
                yield return new WaitForSeconds(0.08f);
            }
        }
        GM.IsInAnimation = false;
    }

    public Vector2 GetPos(Vector2Int Pos)
    {
        Vector2 StoredPos = LayoutPos[Pos.y][Pos.x];
        StoredPos.x *= Canvas.localScale.x;
        StoredPos.y *= Canvas.localScale.y;
        return StoredPos;
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
        DiceUIList[Pos.y].Insert(Pos.x, diceUI);
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
