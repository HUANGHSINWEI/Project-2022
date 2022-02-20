using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInfo : MonoBehaviour
{
    private static  GameObject playerHpbar;
    //public Image hpImage;
    static float playerMaxHp = 300;
    public static float playerHp;
    float playerDistance;//�H���P�Ǫ����Z��
    //��Skill���Ǫ�
    public static int skillAttack;
    //��Attack���Ǫ�
    public static int zAttack;
    //public int reBirth;
    bool attack1 = false;
    bool attack2 = false;
    bool attack3 = false;
    void Start()
    {
        playerHp = playerMaxHp;
        playerHpbar = GameObject.Find("PlayerHpBar");

    }

    // Update is called once per frame
    void Update()
    {
        skillAttack = 0;
        zAttack = 0;
        playerHpbar.GetComponent<Image>().fillAmount = playerHp / playerMaxHp;
        playerHp = Mathf.Clamp(playerHp, 0, 300);
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }

        if (attack1)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 4;
        }
        if (attack2)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 2;
        }
        if (attack3)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime*8;
        }
    }

    private void dash(float speed)
    {

    }
    # region AnimationEvent

    private void Attack1Hurt()
    {
        zAttack = 1;
    } 
    private void Attack2Hurt()
    {
        zAttack = 2;
    }
    private void Attack3Hurt()
    {
        zAttack = 3;
    }
    private void Skill1Hurt()
    {
        skillAttack = 1;
    }
    private void Skill2Hurt()
    {
        skillAttack = 2;
    }
    private void Attack1Move()
    {
        if (attack1 == false)
        {
            attack1 = true;
        }
        else if (attack1 == true)
        {
            attack1 = false;
        }
    }
    private void Attack2Move()
    {
        if (attack2 == false)
        {
            attack2 = true;
        }
        else if (attack2 == true)
        {
            attack2 = false;
        }
    }
    private void Attack3Move()
    {
        if(attack3 == false)
        {
            attack3 = true;
        }
        else if (attack3 == true)
        {
            attack3 = false;
        }       
    }

    private void Die()
    {
        attack1 = false;
        attack2 = false;
        attack3 = false;
    }
    #endregion
    public static void PlayerHpCal(int a)
    {
        //��������
        if (a == 1)
        {
            playerHp = playerHp - 10;
        }

        //Boss����
        else if(a == 10)
        {
            playerHp = playerHp - 30;
        }
        //Boss��ʧ�
        else if (a == 11)
        {
            playerHp = playerHp - 50;
        }
    }
    public static void CarrotArrowDamage()
    {
        playerHpbar.GetComponent<Image>().fillAmount = (playerHp - 10) / playerMaxHp;
        playerHp = playerHp - 10;
        if (playerHpbar.GetComponent<Image>().fillAmount <= 0)
        {
            FSM.isDeath = true;
        }
    }
}
