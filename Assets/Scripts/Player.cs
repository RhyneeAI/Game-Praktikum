using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public void Attack(Enemy enemy)
    {
        int damage = Random.Range(15, 26);
        Debug.Log("Player Memberi damage Sebesar : " + damage);
        enemy.TerkenaSerangan(damage);
    }

}
