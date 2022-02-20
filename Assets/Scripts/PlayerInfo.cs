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
    }

    private void dash(float speed)
    {

    }
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
