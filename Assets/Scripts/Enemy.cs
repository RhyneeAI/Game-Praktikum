using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public void Attack(Player player)
    {
        int damage = Random.Range(19, 26);
        Debug.Log("Enemy Memberi damage Sebesar : " + damage);
        player.TerkenaSerangan(damage);
    }

}
