using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int hp;

    public void TerkenaSerangan(int dmg)
    {
        hp -= dmg;
        Debug.Log(GetType().Name + " Terkena serangan : " + dmg);
        Debug.Log(GetType().Name + " Sisa HP : " + hp);
    }

    public bool IsDead()
    {
        return hp <= 0;
    }

}
