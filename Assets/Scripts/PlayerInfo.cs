using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    //public Image playerHpbar;
    private int getHit;

    private List<GameObject> dogMonsters;

    //public Image hpImage;
    float playerHp;
    float playerDistance;//�H���P�Ǫ����Z��


    void Start()
    {
        dogMonsters = new List<GameObject>();
        getHit = MonsterDmg.mDogAtk;
        foreach (var a in GameObject.FindGameObjectsWithTag("Monster"))
        {
            dogMonsters.Add(a);
        }  
       
    }

    // Update is called once per frame
    void Update()
    {
        //for (int a = 0; a < dogMonsters.Count; a++)
        //{
            MonsterAtack(dogMonsters[1]);
        //}
    }

    public void MonsterAtack(GameObject objMonster)
    {
        //float a;//�⨤�פ��l

        //float b;//�⨤�פ���

        //float cosValue;//cos��

        //a = Vector3.Dot((this.transform.position - objMonster.transform.position), objMonster.transform.forward * 2);
        //b = Vector3.Distance(this.transform.position, objMonster.transform.position) * (objMonster.transform.forward * 2).magnitude;
        //cosValue = a / b;

        if(Input.GetKeyDown(KeyCode.F))
        {
            //getHit == 1
            //playerHpbar.fillAmount = 0.7f;
               // playerHpbar.fillAmount - (20.0f / playerHpbar);
        }
    }
}
