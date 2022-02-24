using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFSM : MonoBehaviour
{
    //�ؼ�
    private GameObject player;
    /// <summary>
    /// Dog����
    /// </summary>
    //�Ǫ���l��m
    private Vector3 initDic;
    [Header("�����t��")]
    public float dogWalkSpeed;
    [Header("�]�B�t��")]
    public float dogRunSpeed;
    /// <summary>
    /// Dog �ʵe����
    /// </summary>
    private Animator anim;
    /// <summary>
    /// ��e���A
    /// </summary>
    public static DogFSMState mCurrentState;
    /// <summary>
    /// �U�ذ����Z��
    /// </summary>
    [Header("�����Z��")]
    public float dogAtkDic;
    [Header("�l�v�Z��")]
    public float dogChaseDic;
    [Header("ĵ�ٶZ��")]
    public float dogWarnDic;
    [Header("�^�k�Z��")]
    public float dogBackToInitDic;
    [Header("�󴫫ݾ����O�����j�ɶ�")]
    public float actRestTme;
    [Header("�l��ζW�X�d�򪺶��j�ɶ�")]
    public float reRestTme;
    [Header("�Ǫ���Үɶ�")]
    public float mobThinkTme;
    //�̫�ʧ@���ɶ�
    private float lastActTime;
    //�H���ʧ@�v��
    private float[] actionWeight = { 3000, 4000 };
    //�Ǫ����ؼд¦V
    private Quaternion targetRotation;
    //isRuning?
    private bool is_Running = false;
    //ReturnIdle Waittime
    private float returnIdleWaittime;
    //AttackIdle Waittime
    private float attackIdleTime;



    public enum DogFSMState
    {
        NONE = -1,
        Idle_Battle,
        AttackIdle,
        Attack01,
        Attack02,
        Block,
        Wander,
        Chase,
        GetHit,
        Die,
        Return,
        ReturnIdle,
        LookPlayer
    }

    void Start()
    {
        initDic = this.transform.position;
        player =GameObject.Find("Character");
        //player=GameObject.Find("Character(Clone)");
        mCurrentState = DogFSMState.Idle_Battle;
        anim = GetComponent<Animator>();
    }


    private void CheckNowState()
    {
        if (mCurrentState == DogFSMState.Idle_Battle)
        {
            if(Time.time-lastActTime>actRestTme)
            {
                anim.SetBool("Idle", false);
                RandomAction();
            }
            TargetDicChenk();
        }
        if(mCurrentState==DogFSMState.AttackIdle)
        {
            Vector3 vLookForword = player.transform.position - gameObject.transform.position;
            vLookForword.y = 0;
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, vLookForword,0.1f);
            
            if (CanNextActionCheck(attackIdleTime))
            {
                AttackIdleCheck();             
            }
        }
        if (mCurrentState == DogFSMState.Attack01)
        {
            float fToPlayerDic = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1)
            {
                //Debug.Log("��������");
                anim.SetBool("Attack1", false);
                attackIdleTime = Time.time;
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.AttackIdle;
            }
            else
            {
                if(fToPlayerDic<=dogAtkDic)
                {
                    Debug.Log("������F");
                }
            }
        }
        if (mCurrentState == DogFSMState.Attack02)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                //Debug.Log("��������");
                anim.SetBool("Attack2", false);
                attackIdleTime = Time.time;
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.AttackIdle;
            }
        }
        if (mCurrentState == DogFSMState.Block)
        {

        }
        if (mCurrentState == DogFSMState.Wander)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * dogWalkSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);

            if (Time.time - lastActTime > actRestTme)
            {
                anim.SetBool("Wander", false);
                RandomAction();
            }
            WanderRadiusCheck();
        }
        if (mCurrentState == DogFSMState.Chase)
        {
            float fPlayerToDogInit = Vector3.Distance(player.transform.position, initDic);
            if(Vector3.Distance(gameObject.transform.position,initDic)< dogBackToInitDic)
            {
                //�¦V���a��m
                targetRotation = Quaternion.LookRotation(player.transform.position - gameObject.transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
                transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
                ChaseCancelCheck();
            }
            else
            {
                if (fPlayerToDogInit > dogBackToInitDic)
                {
                    is_Running = false;
                    anim.SetBool("Chase", false);
                    anim.SetBool("Idle", true);
                    mCurrentState = DogFSMState.LookPlayer;
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(player.transform.position - gameObject.transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
                    transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
                    ChaseCancelCheck();
                }
            }
        }
        if (mCurrentState == DogFSMState.GetHit)
        {

        }
        if (mCurrentState == DogFSMState.Die)
        {

        }
        if(mCurrentState == DogFSMState.Return)
        {
            targetRotation = Quaternion.LookRotation(initDic - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * dogRunSpeed);
            ReturnCheck();
        }
        if(mCurrentState == DogFSMState.ReturnIdle)
        {
            if (ReturnIdleCheck(returnIdleWaittime))
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Wander", true);
                mCurrentState = DogFSMState.Return;
            }
            ReturnIdleCheck();
        }
        if(mCurrentState==DogFSMState.LookPlayer)
        {
            Vector3 vLookForword = player.transform.position - gameObject.transform.position;
            vLookForword.y = 0;
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, vLookForword, 0.1f);
            float fPlayerToDoInit = Vector3.Distance(player.transform.position, initDic);

            if (Vector3.Distance(player.transform.position, gameObject.transform.position) > dogChaseDic)
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Wander", true);
                mCurrentState = DogFSMState.Return;
            }
            else
            {
                if (fPlayerToDoInit < dogBackToInitDic)
                {
                    is_Running = true;
                    anim.SetBool("Idle", false);
                    anim.SetBool("Chase", true);
                    mCurrentState = DogFSMState.Chase;
                }
            }
            
        }
    }

    private bool CanNextActionCheck(float thinktime)
    {
        if (Time.time - thinktime > mobThinkTme)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AttackIdleCheck()
    {
        //Debug.Log("�i�H���");
        float fToPlayerDic = Vector3.Distance(player.transform.position, gameObject.transform.position);

        int num = UnityEngine.Random.Range(1, 3);
        if(num==1)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Attack1", true);
            mCurrentState = DogFSMState.Attack01;
        }
        if(num==2)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Attack2", true);
            mCurrentState = DogFSMState.Attack02;
        }

    }

    
    private void ReturnIdleCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (diatanceToPlayer < dogChaseDic)
        {
            anim.SetBool("Idle", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;
        }
    }

    private bool ReturnIdleCheck(float waitTime)
    {
        if(Time.time-waitTime>1.2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void WanderRadiusCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        var diatanceToInitial = Vector3.Distance(transform.position, initDic);

        if (diatanceToPlayer < dogChaseDic)
        {
            if (diatanceToInitial >= dogBackToInitDic)
            {
                anim.SetBool("Wander", false);
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.LookPlayer;
            }
            else
            {
                anim.SetBool("Wander", false);
                is_Running = true;
                anim.SetBool("Chase", true);
                mCurrentState = DogFSMState.Chase;
            }
        }

        if (diatanceToInitial> dogBackToInitDic)
        {
            targetRotation = Quaternion.LookRotation(initDic - transform.position, Vector3.up);
        }
    }
    /// <summary>
    /// �p�G�w�g�����l��m�A�h�H���@�ӫݾ����A
    /// </summary>
    private void ReturnCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);
        if(diatanceToPlayer< dogChaseDic)
        {
            if (diatanceToInitial >= dogBackToInitDic)
            {
                anim.SetBool("Idle", true);
                mCurrentState = DogFSMState.LookPlayer;
            }
            else
            {
                anim.SetBool("Wander", false);
                is_Running = true;
                anim.SetBool("Chase", true);
                mCurrentState = DogFSMState.Chase;
            }
        }
        if (diatanceToInitial < 0.5f)
        {
            anim.SetBool("Wander", false);
            RandomAction();
        }
    }
    /// <summary>
    /// �l�v�W�X�d��/�l�v��᪺�ˬd
    /// </summary>
    private void ChaseCancelCheck()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);

        if(diatanceToPlayer<=dogAtkDic)
        {
            is_Running = false;
            anim.SetBool("Chase", false);
            attackIdleTime = Time.time;
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.AttackIdle;
        }
        if (diatanceToInitial > dogBackToInitDic || diatanceToPlayer > dogChaseDic)
        {
            is_Running = false;
            anim.SetBool("Chase", false);
            returnIdleWaittime = Time.time;
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.ReturnIdle;
        }
    }
    /// <summary>
    /// �O�_�n�l�v
    /// </summary>
    private void TargetDicChenk()
    {
        var diatanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
        var diatanceToInitial = Vector3.Distance(gameObject.transform.position, initDic);
        if (diatanceToPlayer < dogChaseDic && diatanceToInitial < dogBackToInitDic)
        {
            anim.SetBool("Idle", false);
            is_Running = true;
            anim.SetBool("Chase", true);
            mCurrentState = DogFSMState.Chase;          
        }
        if (diatanceToPlayer < dogChaseDic && diatanceToInitial >= dogBackToInitDic)
        {
            anim.SetBool("Idle", true);
            mCurrentState = DogFSMState.LookPlayer;
        }
    }
    /// <summary>
    /// ��ܫݾ�/�C��
    /// </summary>
    private void RandomAction()
    {
        lastActTime = Time.time;

        float name = UnityEngine.Random.Range(0, actionWeight[0] + actionWeight[1]);
        if (name <= actionWeight[0])
        {
            mCurrentState = DogFSMState.Idle_Battle;
            anim.SetBool("Idle", true);
        }
        else if (actionWeight[0] < name && name <= actionWeight[0] + actionWeight[1])
        {
            mCurrentState = DogFSMState.Wander;
            anim.SetBool("Wander", true);
            targetRotation = Quaternion.Euler(0, UnityEngine.Random.Range(1, 5) * 90, 0);
        }
    }
    void Update()
    {
        //Debug.Log(mCurrentState);
        CheckNowState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, dogAtkDic);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, dogChaseDic);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, dogWarnDic);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(initDic, dogBackToInitDic);
    }
}
