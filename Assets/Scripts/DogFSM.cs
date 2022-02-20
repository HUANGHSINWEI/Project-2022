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
    private Vector3 InitDic;
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
    //�̫�ʧ@���ɶ�
    private float lastActTime;
    //�H���ʧ@�v��
    private float[] actionWeight = { 3000, 4000 };
    //�Ǫ����ؼд¦V
    private Quaternion targetRotation;         



    public enum DogFSMState
    {
        NONE = -1,
        Idle_Battle,
        Attack01,
        Attack02,
        Block,
        Wander,
        Chase,
        GetHit,
        Die
    }

    void Start()
    {
        InitDic = this.transform.position;
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
                RandomAction();
            }
        }      
        if (mCurrentState == DogFSMState.Attack01)
        {

        }
        if (mCurrentState == DogFSMState.Attack02)
        {

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
                RandomAction();
            }
        }
        if (mCurrentState == DogFSMState.Chase)
        {

        }
        if (mCurrentState == DogFSMState.GetHit)
        {

        }
        if (mCurrentState == DogFSMState.Die)
        {

        }
    }
    void Update()
    {
        Debug.Log(mCurrentState);
        CheckNowState();
    }

    private void RandomAction()
    {
        lastActTime = Time.time;

        float name = Random.Range(0, actionWeight[0]+ actionWeight[1]);
        if(name<= actionWeight[0])
        {
            mCurrentState = DogFSMState.Idle_Battle;
            anim.SetTrigger("Idle");
        }
        else if(actionWeight[0] < name&&name <= actionWeight[0]+ actionWeight[1])
        {
            mCurrentState = DogFSMState.Wander;
            anim.SetTrigger("Wander");
            targetRotation = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
        }
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
        Gizmos.DrawWireSphere(InitDic, dogBackToInitDic);
    }
}
