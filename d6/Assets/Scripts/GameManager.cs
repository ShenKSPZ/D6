using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //First List use to store row
    //Second List use to store Column
    public DiceUI Slot;
    public RectTransform SlotUI;
    public int Score;
    public int Chain;
    public int HP = 6;

    [Space(10)]
    public UIManager UIMgr;

    bool IsInAnimation;

    void Awake()
    {
        Generate();
    }

    void Update()
    {
        InputEveryFrame();
    }

    void Generate()
    {
        //Add double times to have two rows. 
        List<List<Dice>> DiceList = new List<List<Dice>>();
        DiceList.Add(new List<Dice>());
        DiceList.Add(new List<Dice>());

        for (int y = 0; y < DiceList.Count; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                DiceList[y].Add(Dice.GetRandomDice());
            }
        }

        UIMgr.ShowList(DiceList);
    }

    void InputEveryFrame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.currentSelectedGameObject.TryGetComponent(out DiceUI clickedDice);
                if (clickedDice != null && !IsInAnimation)
                {
                    if (clickedDice.isInSlot)
                        StartCoroutine(Discard());
                    else
                        StartCoroutine(Replace(clickedDice));
                }
            }
        }
    }

    IEnumerator Replace(DiceUI clicked)
    {
        IsInAnimation = true;
        //TODO: Check is the clicked dice is moveable or not
        if(clicked.DicePointer.y == 1 && IsVaild(Slot, clicked))
        {
            //Score plus one
            Score++;
            Chain++;

            //Change UI
            UIMgr.Score.text = Score.ToString() + "/50";//TODO: Change to stringbuilder
            UIMgr.Chain.text = Chain.ToString();

            //Change Parent and do animation moving
            Vector3 pos = clicked.transform.position;
            clicked.transform.SetParent(SlotUI, false);
            clicked.transform.position = pos;

            clicked.transform.DOMove(SlotUI.transform.position, 0.5f);

            //把DiceList内的Dice向下移动
            Vector2Int clickedPos = new Vector2Int(clicked.DicePointer.x, clicked.DicePointer.y);

            //--Move in List
            UIMgr.diceUIList[clickedPos.y].RemoveAt(clickedPos.x);
            DiceUI temp = UIMgr.diceUIList[clickedPos.y - 1][clickedPos.x];
            UIMgr.diceUIList[clickedPos.y].Insert(clickedPos.x, temp);
            UIMgr.diceUIList[clickedPos.y - 1].RemoveAt(clickedPos.x);
            UIMgr.diceUIList[clickedPos.y][clickedPos.x].DicePointer = clickedPos;
            UIMgr.diceUIList[clickedPos.y][clickedPos.x].transform.SetParent(UIMgr.Rows[clickedPos.y].transform);
            UIMgr.diceUIList[clickedPos.y][clickedPos.x].transform.SetSiblingIndex(clickedPos.x);

            //--Move in Animation
            UIMgr.diceUIList[clickedPos.y][clickedPos.x].transform.DOMove(UIMgr.GetPos(clickedPos), 0.5f);

            //Generate a new Dice
            UIMgr.AddDice(Dice.GetRandomDice(), new Vector2Int(clickedPos.x, clickedPos.y - 1));

            yield return new WaitForSeconds(0.5f);

            if (Slot != null)
                Destroy(Slot.gameObject);
            Slot = clicked;
        }
        else
        {
            clicked.transform.DOPunchPosition(Vector3.one * 30, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        Check();
        IsInAnimation = false;
    }

    IEnumerator Discard()
    {
        IsInAnimation = true;

        Slot.transform.DOMove(new Vector3(Slot.transform.position.x, -500), 0.5f);

        Chain = 0;
        HP--;
        //Change UI
        UIMgr.Chain.text = Chain.ToString();
        UIMgr.HP.text = HP.ToString();

        yield return new WaitForSeconds(0.5f);

        Destroy(Slot.gameObject);
        Slot = null;

        Check();
        IsInAnimation = false;
    }

    bool IsVaild(DiceUI Slot, DiceUI Clicked)
    {
        if (Slot == null)
            return true;
        if (
            (Slot.dice.Number <= 5 && Slot.dice.Number >= 1 && Slot.dice.Number == Clicked.dice.Number - 1)
            || (Slot.dice.Number == 6 && Clicked.dice.Number == 1)
            || (Slot.dice.DiceColor == Clicked.dice.DiceColor)
            )
            return true;
        else
            return false;
    }

    void Check()
    {
        if(HP < 0 && Score >= 50)
        {
            SceneManager.LoadScene("GameWin");
        }
        else if(HP < 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }


}
