using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDmg : MonoBehaviour
{
    public GameObject monsterHpbar;
    // Start is called before the first frame update
    private  int zAtack;


    public  GameObject objPlayer;
    public  GameObject objMonster;

    public  Image hpImage;
    float monsterHp;
    float monsterDistance;//�H���P�Ǫ����Z��

    private Animator dogAnimator;
   
    /// <summary>
    /// ����p�ɥ�
    /// </summary>
    private float nowTimeAtk;
    private bool atkStatus = false;
    private bool atkCd = false;
    
    /// <summary>
    /// �ˮ`����p��
    /// </summary>
    private float nowTimeHurt = 0;
    private bool hertDelay = false;
    private bool hertWait = false;
    // private bool  

    /// <summary>
    /// getHit�ʵe�p�ɥ�
    /// </summary>
    private float hitAnimTime = 0;
    private bool hertAnimDelay = false;
    private bool hertAnimWait = false;

    void Start()
    {
        monsterHp = 300.0f;
        objPlayer = GameObject.Find("Character(Clone)");
        objMonster = this.gameObject;
        dogAnimator = gameObject.GetComponent<Animator>();
    //Debug.Log(objPlayer.transform.position);
}

    // Update is called once per frame
    void Update()
    {
        
        zAtack = Main.zAtack;
       

        monsterDistance = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position);

        
        if (monsterDistance <= 6.0f && hpImage.fillAmount > 0)
        {
            //�������j�P�_
            if (atkStatus == false)
            {
                dogAnimator.SetBool("Attack01", false);
                nowTimeAtk = Time.time;
                atkStatus = true;              
            }
            atkCd = Timer(2.0f,nowTimeAtk);
            //�������j�P�_

            gameObject.transform.LookAt(objPlayer.transform.position);//���V�D��

            if (monsterDistance >= 2.0f)
            {  
                gameObject.transform.position += gameObject.transform.forward * 3.0f * Time.deltaTime;                  
                dogAnimator.SetBool("Attack01", false);
                dogAnimator.SetBool("chase", true);
            }
            else if (monsterDistance < 2.0f)
            {   
                dogAnimator.SetBool("chase", false);
                if (atkCd)
                {
                    gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime; //�������e����
                    dogAnimator.SetBool("Attack01", true);

                    //���ˮ`����p�� �P�ʵe�ʧ@�X��
                    if (hertDelay == false)
                    {
                        nowTimeHurt = Time.time;
                        hertDelay = true;
                    }
                    //���ˮ`����p�� �P�ʵe�ʧ@�X��

                    atkStatus = false;
                }

                //���ˮ`����p�� �P�ʵe�ʧ@�X��
                if (nowTimeHurt != 0) //�P�_���S�������nowTimeHurt����
                {
                    hertWait = Timer(0.3f, nowTimeHurt);
                }
                if (hertWait)
                {
                    PlayerInfo.PlayerHpCal();
                    hertDelay = false;
                    hertWait = false;
                    nowTimeHurt = 0;
                }
                //���ˮ`����p�� �P�ʵe�ʧ@�X��
               
            }
        }
        else if(monsterDistance >= 6.0f && hpImage.fillAmount > 0)
        {
            dogAnimator.SetBool("chase", false);            
        }

        if (zAtack != 0)
        {
            PlayerAttack(monsterDistance);
        }

        monsterHpbar.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //�Ǫ�Hp�����V��v��
        if(hpImage.fillAmount<=0)
        {
            dogAnimator.Play("Die");
        }
    }

    public void PlayerAttack(float monsterDistance)
    {
        
        float a;//�⨤�פ��l

        float b;//�⨤�פ���

        float cosValue;//cos��

        a = Vector3.Dot((objMonster.transform.position - objPlayer.transform.position), objPlayer.transform.forward * 2);
        b = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position) * (objPlayer.transform.forward * 2).magnitude;
        cosValue = a / b;

        dogAnimator.SetBool("gethit", false);
        if (zAtack == 1)
        {
            
            if (cosValue >= 0.7 && monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f/ monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 40");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            //else if (cosValue < 0.7 || monsterDistance > 2.3f)
            //{
            //    Debug.Log("�S����");
            //    dogAnimator.SetBool("gethit", false);
            //}
        }
        else if (zAtack == 2)
        {
          
            if (cosValue >= 0.7 && monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 60");
            }
            //else if (cosValue < 0.7 || monsterDistance > 2.3f)
            //{
            //    Debug.Log("�S����");
            //    dogAnimator.SetBool("gethit", false);
            //}
        }
        else if (zAtack == 3)
        {
           
            if (monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 20");
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            //else if (monsterDistance > 2.3f)
            //{
            //    Debug.Log("�S����");
            //    dogAnimator.SetBool("gethit", false);
            //}
        }
        if(hpImage.fillAmount<0.3)
        {
            hpImage.fillAmount = 1;
        }
    }

    public static bool Timer(float cdTime,float nowTime)
    {
         
        if(Time.time-nowTime >= cdTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
