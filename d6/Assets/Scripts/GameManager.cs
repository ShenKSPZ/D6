using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RectTransform SlotUI;
    public AudioSource AuPlayer;
    public AudioSource WinOverPlayer;
    public AudioClip ChainAudio;
    public AudioClip DiscardAudio;
    public AudioClip WinSound;
    public AudioClip GameOverSound;

    DiceUI Slot;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            UIMgr.Score.text = score.ToString() + "/";
            UIMgr.Bar.SetFill(score, 50);
        }
    }
    int score;

    public int Chain
    {
        get
        {
            return chain;
        }
        set
        {
            chain = value;
            UIMgr.Chain.text = Chain.ToString();
        }
    }
    int chain;

    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            UIMgr.HP.SetHP(value);
        }
    }
    int hp;

    [Space(10)]
    public UIManager UIMgr;

    public bool IsInAnimation;

    void Awake()
    {
        HP = 6;
        Construct();
    }

    void Update()
    {
        PlayerInput();
    }

    //Randomly generate a list of dice
    void Construct()
    {
        //Add double times to have two rows. 
        List<List<Dice>> DiceList = new List<List<Dice>>();
        for (int i = 0; i < 2; i++)
            DiceList.Add(new List<Dice>());

        for (int y = 0; y < DiceList.Count; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                DiceList[y].Add(Dice.RandomDice());
            }
        }

        UIMgr.ShowList(DiceList);
    }

    void PlayerInput()
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
            AuPlayer.pitch = 1f + (Chain - 1) / 20f * 2f;
            AuPlayer.PlayOneShot(ChainAudio);
            //Change Parent and do animation moving
            Vector3 pos = clicked.transform.position;
            clicked.transform.SetParent(SlotUI, false);
            clicked.transform.position = pos;

            clicked.transform.DOMove(SlotUI.transform.position, 0.5f);

            //Move the element in DiceList down
            Vector2Int clickedPos = new Vector2Int(clicked.DicePointer.x, clicked.DicePointer.y);

            //--Move in List
            UIMgr.DiceUIList[clickedPos.y].RemoveAt(clickedPos.x);
            DiceUI temp = UIMgr.DiceUIList[clickedPos.y - 1][clickedPos.x];
            UIMgr.DiceUIList[clickedPos.y].Insert(clickedPos.x, temp);
            UIMgr.DiceUIList[clickedPos.y - 1].RemoveAt(clickedPos.x);
            UIMgr.DiceUIList[clickedPos.y][clickedPos.x].DicePointer = clickedPos;
            UIMgr.DiceUIList[clickedPos.y][clickedPos.x].transform.SetParent(UIMgr.Rows[clickedPos.y].transform);
            UIMgr.DiceUIList[clickedPos.y][clickedPos.x].transform.SetSiblingIndex(clickedPos.x);

            //--Move in Animation
            UIMgr.DiceUIList[clickedPos.y][clickedPos.x].transform.DOMove(UIMgr.GetPos(clickedPos), 0.5f);

            //Generate a new Dice
            UIMgr.AddDice(Dice.RandomDice(), new Vector2Int(clickedPos.x, clickedPos.y - 1));

            yield return new WaitForSeconds(0.5f);

            if (Slot != null)
                Destroy(Slot.gameObject);
            Slot = clicked;
        }
        else
        {
            clicked.transform.DOShakePosition(0.5f, 15, 90, 50);
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(Check());
        IsInAnimation = false;
    }

    IEnumerator Discard()
    {
        IsInAnimation = true;

        Slot.transform.DOMove(new Vector3(Slot.transform.position.x, -500), 0.5f);

        Chain = 0;
        AuPlayer.pitch = 1;
        AuPlayer.PlayOneShot(DiscardAudio);
        HP--;

        yield return new WaitForSeconds(0.5f);

        Destroy(Slot.gameObject);
        Slot = null;

        StartCoroutine(Check());
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

    IEnumerator Check()
    {
        if(Score >= 50)
        {
            if (!UIMgr.Bar.IsShinny)
            {
                UIMgr.Bar.SetShinny();
                WinOverPlayer.PlayOneShot(WinSound);
            }
        }

        if(HP < 0 && Score >= 50)
        {
            IsInAnimation = true;
            for (int y = 0; y < UIMgr.DiceUIList.Count; y++)
            {
                for (int x = 0; x < UIMgr.DiceUIList[y].Count; x++)
                {
                    UIMgr.DiceUIList[y][x].transform.DOMoveX(-1400, 0.5f);
                    yield return new WaitForSeconds(0.04f);
                }
            }
            UIMgr.Win.DOMoveX(960, 0.5f);
            UIMgr.Retry.DOMoveY(165, 0.5f);
            WinOverPlayer.PlayOneShot(WinSound);
        }
        else if(HP < 0)
        {
            IsInAnimation = true;
            for (int y = 0; y < UIMgr.DiceUIList.Count; y++)
            {
                for (int x = 0; x < UIMgr.DiceUIList[y].Count; x++)
                {
                    UIMgr.DiceUIList[y][x].transform.DOMoveX(-1400, 0.5f);
                    yield return new WaitForSeconds(0.04f);
                }
            }
            UIMgr.Lose.DOMoveX(960, 0.5f);
            UIMgr.Retry.DOMoveY(165, 0.5f);
            WinOverPlayer.PlayOneShot(GameOverSound);
        }
    }
}
