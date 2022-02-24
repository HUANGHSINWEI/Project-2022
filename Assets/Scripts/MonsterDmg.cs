using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDmg : MonoBehaviour
{
    public GameObject monsterHpbar;

    public GameObject swordLight;
    // Start is called before the first frame update
    private int zAttack;
    private int skillAttack;
    static float playerHp;
    public  GameObject objPlayer;
    public  GameObject objMonster;

    public  Image hpImage;
    public Image hpImage0;
    float monsterHp;
    float monsterDistance;//�H���P�Ǫ����Z��
    bool monsteCollision = true;
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
    /// <summary>
    /// getHit�ʵe�p�ɥ�
    /// </summary>
    private float hitAnimTime = 0;
    private bool attackHertGet = false;
    private bool hertAnimDelay = false;
    private bool hertAnimWait = false;
    /// <summary>
    /// �Ǫ�������
    /// </summary>
    public GameObject dropItem;
    /// <summary>
    /// �Ǫ������欰- �I�����|�קK
    /// </summary>
    private List<GameObject> dogMonsters;
    private bool monsterCollision;

    void Start()
    {
        monsterHp = 300.0f;
        objPlayer = GameObject.Find("Character(Clone)");
        objMonster = this.gameObject;
        dogAnimator = gameObject.GetComponent<Animator>();
        
        
        dogMonsters = new List<GameObject>();
        foreach (var a in GameObject.FindGameObjectsWithTag("Monster"))
        {
            dogMonsters.Add(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHp = PlayerInfo.playerHp;//�������a��q

        zAttack = PlayerInfo.zAttack;
        skillAttack = PlayerInfo.skillAttack;

        monsterDistance = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position);

        if (hpImage.fillAmount <= 0)
        {
            hpImage0.fillAmount = 0;
        }
        #region ������
        //MonsterDis(); 
        //if (monsterDistance <= 6.0f && hpImage.fillAmount > 0 && playerHp >0)
        //{
        //    //MonsteCollision();
        //    gameObject.transform.LookAt(objPlayer.transform.position);//���V�D��

        //    if (monsterDistance >= 2.0f && monsteCollision)
        //    {  
        //        gameObject.transform.position += gameObject.transform.forward * 3.0f * Time.deltaTime;                  
        //        dogAnimator.SetBool("Attack01", false);
        //        dogAnimator.SetBool("chase", true);
        //        nowTimeAtk = Time.time;
        //    }
        //    else if (monsterDistance < 3.0f)
        //    {   
        //        //�������j�P�_
        //        if (atkStatus == false)
        //        {
        //            dogAnimator.SetBool("Attack01", false);
        //            nowTimeAtk = Time.time;
        //            atkStatus = true;              
        //        }
        //        atkCd = Timer(1.4f,nowTimeAtk);
        //        //�������j�P�_

        //        dogAnimator.SetBool("chase", false);
        //        if (atkCd)
        //        {
        //            gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime; //�������e����
        //            dogAnimator.SetBool("Attack01", true);


        //            //���ˮ`����p�� �P�ʵe�ʧ@�X��
        //            if (hertDelay == false)
        //            {
        //                nowTimeHurt = Time.time;
        //                hertDelay = true;
        //            }
        //            //���ˮ`����p�� �P�ʵe�ʧ@�X��

        //            atkStatus = false;
        //        }

        //        //���ˮ`����p�� �P�ʵe�ʧ@�X��
        //        if (nowTimeHurt != 0) //�P�_���S�������nowTimeHurt����
        //        {
        //            hertWait = Timer(0.3f, nowTimeHurt);
        //        }

        //        if (Time.time-nowTimeHurt > 0.4f)
        //        {
        //            hertWait = false;
        //            hertDelay = false;
        //        }

        //        if (hertWait)
        //        {

        //            PlayerInfo.PlayerHpCal(1);
        //            hertDelay = false;
        //            hertWait = false;
        //            nowTimeHurt = 0;
        //        }
        //        //���ˮ`����p�� �P�ʵe�ʧ@�X��

        //    }
        //}
        //else if(monsterDistance >= 6.0f && hpImage.fillAmount > 0)
        //{
        //    dogAnimator.SetBool("chase", false);            
        //}
        #endregion
        if (zAttack != 0 || skillAttack != 0)
        {
            attackHertGet = PlayerAttack(monsterDistance);
        }
        //getHit�ʵe����P�_
        if(attackHertGet)
        {
            hitAnimTime = Time.time;
            attackHertGet = false;
            hertAnimDelay = true;
        }

        if (hertAnimDelay)
        {
            hertAnimWait = Timer(0.5f, hitAnimTime);
        }

        if (hertAnimWait)
        {
            dogAnimator.SetBool("gethit", false);
            hertAnimDelay = false;
            hertAnimWait = false;
        }
       

       
        //getHit�ʵe����P�_

        monsterHpbar.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //�Ǫ�Hp�����V��v��
        if(hpImage.fillAmount<=0)
        {
            StartCoroutine(Die());         
        }
    }

    public bool PlayerAttack(float monsterDistance)
    {
        
        float a;//�⨤�פ��l

        float b;//�⨤�פ���

        float cosValue;//cos��

        a = Vector3.Dot((objMonster.transform.position - objPlayer.transform.position), objPlayer.transform.forward * 2);
        b = Vector3.Distance(objMonster.transform.position, objPlayer.transform.position) * (objPlayer.transform.forward * 2).magnitude;
        cosValue = a / b;

        dogAnimator.SetBool("gethit", false);
        if (zAttack == 1)
        {
            
            if (cosValue >= 0.5 && monsterDistance <= 3.0f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f/ monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 20");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            else //if (cosValue < 0.7 || monsterDistance > 2.3f)
            {
                return false;
            }
        }
        
        else if (zAttack == 2)
        {        
            if (cosValue >= 0.5 && monsterDistance <= 3.0f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 40");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }
        }

        else if (zAttack == 3)
        {
            if (cosValue >= 0.6 && monsterDistance <= 3.0f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 60");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }
        }

        //�H���ޯ�X �ˮ`�Ĥ@�q
        else if (skillAttack == 1)
        {
            if (cosValue >= 0.8f && monsterDistance <= 2.8f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (40.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 40");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }
            
        }

        //�H���ޯ�X �ˮ`�ĤG�q
        else if (skillAttack == 2)
        {
            //�e��@�q�Z������ˮ`�P�w��
            //Vector3 playrerAtkPosition;
            //float dogMonsterkDistance;

            //playrerAtkPosition = objPlayer.transform.position + objPlayer.transform.forward * 1.0f;
            //dogMonsterkDistance = Vector3.Distance(playrerAtkPosition, objMonster.transform.position);
            //�e��@�q�Z������ˮ`�P�w��

            if (cosValue >= 0.7f && monsterDistance <= 3.5f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (60.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 40");
                return true;
                //objMonster.transform.position = objMonster.transform.position + new Vector3(objMonster.transform.position.x - objPlayer.transform.position.x, 0, objMonster.transform.position.z - objPlayer.transform.position.z) * 0.1f; //�����첾
            }
            else //if (monsterDistance > 2.3f)
            {
                return false;
            }

        }
        else if (skillAttack == 3)
        {
            if (monsterDistance <= 2.3f && hpImage.fillAmount > 0)
            {
                hpImage.fillAmount = hpImage.fillAmount - (20.0f / monsterHp);
                dogAnimator.SetBool("gethit", true);
                //dogAnimator.SetBool("Attack01", false);
                //dogAnimator.SetBool("chase", false);
                Debug.Log("�y���ˮ` 20");
                return true;
            }
            else //if (cosValue < 0.7 || monsterDistance > 2.3f)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
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

    //DieĲ�o�������禡
    IEnumerator Die()
    {     
        dogAnimator.Play("Die");
        yield return new WaitForSeconds(2.0f);

        //�����D�㬰�Ǫ���m
        Vector3 itemPosition = this.transform.position;
        itemPosition.y -= 0.5f;
        //itemPosition += new Vector3(Random.Range(-2, 2),0.2f, Random.Range(-2, 2));

        Instantiate(dropItem, itemPosition, dropItem.transform.rotation);
        Destroy(this.gameObject);
    }

    //public void MonsteCollision()
    //{
    //    float a;//�⨤�פ��l

    //    float b;//�⨤�פ���

    //    float cosValue;//cos��

    //    float monsterDis;
    //    float forwardDis;

    //    for (int i = 0; i < dogMonsters.Count; i++)
    //    {
    //        a = Vector3.Dot((dogMonsters[i].transform.position - this.transform.position), this.transform.forward);
    //        b = Vector3.Distance(dogMonsters[i].transform.position, this.transform.position) * (this.transform.forward).magnitude;
    //        cosValue = a / b;
    //        monsterDis = Vector3.Distance(this.transform.position, dogMonsters[i].transform.position);
    //        forwardDis = monsterDis * cosValue;
    //        if (Vector3.Distance(this.transform.position+this.transform.forward*1, dogMonsters[i].transform.position) < 2.0f && Vector3.Distance(this.transform.position, dogMonsters[i].transform.position) != 0
    //            /*&& Vector3.Dot((dogMonsters[i].transform.position - this.transform.position), this.transform.forward) > 0 && Mathf.Sqrt(monsterDis * monsterDis - forwardDis * forwardDis) < 1.5f*/)
    //        {
    //            monsteCollision = false;
    //        }
    //        else
    //        { 
    //            monsteCollision = true; 
    //        }
    //        //else
    //        //{
    //        //    gameObject.transform.LookAt(objPlayer.transform.position);//���V�D��
    //        //}
    //    }
    //}


    void DogNAtkLightOpen()
    {
        swordLight.SetActive(true);
    }

    void DogNAtkLightClose()
    {
        swordLight.SetActive(false);
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3 playrerAtkPosition;
    //    float dogMonsterkDistance;

    //    playrerAtkPosition = objPlayer.transform.position + objPlayer.transform.forward * 2.0f;
    //    dogMonsterkDistance = Vector3.Distance(playrerAtkPosition, objMonster.transform.position);

    //    Gizmos.color = Color.blue;

    //    Gizmos.DrawWireSphere(playrerAtkPosition, 1.0f);
    //}

}
