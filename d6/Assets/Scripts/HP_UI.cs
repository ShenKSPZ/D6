using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    public List<Image> HPDots;

    private void Awake()
    {
        for (int i = 0; i < HPDots.Count; i++)
        {
            HPDots[i].color = DiceUI.newColor(154, 34, 92);
        }
    }

    public void SetHP(int HP)
    {
        if(5 - HP >= 0 && 5 - HP < HPDots.Count)
            HPDots[5 - HP].DOColor(Color.white, 0.2f);
    }
}
