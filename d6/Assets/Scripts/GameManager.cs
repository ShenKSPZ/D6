using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    //First List use to store row
    //Second List use to store Column
    public List<List<Dice>> DiceList = new List<List<Dice>>();
    public Dice Slot;
    public int Score;
    public int Chain;
    public int HP;

    [Space(10)]
    public UIManager UIMgr;
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
            EventSystem.current.currentSelectedGameObject.TryGetComponent(out DiceUI clickedDice);
            if (clickedDice != null)
            {
                if (clickedDice.isInSlot)
                    Discard();
                else
                    Replace();
            }
        }
    }


    void Replace()
    {
        print("Replace");
    }


    void Discard()
    {
        print("Discard");
    }


    void Check()
    {

    }

    void GameOver()
    {

    }

    void GameWin()
    {

    }
}
