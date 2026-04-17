using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Player p1 = new Player();
        p1.hp = 100;

        Enemy e1 = new Enemy();
        e1.hp = 60;

        while (true)
        {
            int bagian = Random.Range(1, 3);
            if (bagian == 1)
            {
                p1.Attack(e1);
            }
            else
            {
                e1.Attack(p1);
            }

            if (p1.IsDead())
            {
                Debug.Log("---GAME OVER---");
                break;
            }
            if (e1.IsDead()) 
            {
                Debug.Log("---YOU WIN---");
                break;
            }
        }
    }

}
