using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    private GameObject objPlayer;
    private GameObject objBoss;

    public GameObject atkWave;
    public GameObject jumpWave;
    public GameObject awakeDust;
    public GameObject jumpHint;

    private float bossHp;

    private int bossState;
    private int bossDo;

    bool bossRoll = false;
    bool bossJump = false;
    bool bossTurn = false;

    Vector3 bossJumpVec;
    private void Start()
    {
        objPlayer = GameObject.Find("Character(Clone)");
        objBoss = this.gameObject;
        bossState = 0;
        bossHp = 1000;

        BossFSM.mCurrentState = BossFSM.BossFSMState.Active;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) <= 10.0f)
        //{
        //    objBoss.transform.LookAt(objPlayer.transform.position);
        //    BossFSM.mCurrentState = BossFSM.BossFSMState.Walk;



        //    gameObject.transform.position += gameObject.transform.forward * 2.0f * Time.deltaTime;
        //}
        if(bossRoll == true)
        {
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 4;
        }

        if (bossJump == true)
        {
            //objBoss.transform.LookAt(objPlayer.transform.position);
            //gameObject.transform.forward = bossJumpVec;
            this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * 15;
        }

        if(bossTurn == true)
        {
            gameObject.transform.forward += (objPlayer.transform.position - gameObject.transform.position).normalized * Time.deltaTime * 5;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Idle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Active;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Attack2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Stand;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Walk;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Roll;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.JumpAtk;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            BossFSM.mCurrentState = BossFSM.BossFSMState.Die;
        }
    }

    void BossNormalAtk()
    {
        Vector3 bossAtkPosition0;
        float bossAtkDistance0;

        bossAtkPosition0 = objBoss.transform.position + objBoss.transform.forward * 6.0f;
        bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
        if (bossAtkDistance0 < 3.0f)
        {
            PlayerInfo.PlayerHpCal(10);
        }
        else
        {
            Debug.Log("Boss�����d��~");
        }   
    }

    void BossNAtkWaveOpen()
    {
        atkWave.SetActive(true);
    }

    void BossNAtkWaveClose()
    {
        atkWave.SetActive(false);
    }

    void BossJumpWaveOpen()
    {
        jumpWave.SetActive(true);
    }

    void BossJumpWaveClose()
    {
        jumpWave.SetActive(false);
    }

    void BossAwakeDustOpen()
    {
        awakeDust.SetActive(true);
    }

    void BossAwakeDustClose()
    {
        awakeDust.SetActive(false);
    }

    void BossJumpHintOpen()
    {
        jumpHint.SetActive(true);
    }

    void BossJumpHintClose()
    {
        jumpHint.SetActive(false);
    }
    void BossJumpDamage()
    {
        Vector3 bossAtkPosition0;
        float bossAtkDistance0;

        bossAtkPosition0 = objBoss.transform.position;
        bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
        if (bossAtkDistance0 < 6.0f)
        {
            PlayerInfo.PlayerHpCal(12);
        }
        else
        {
            Debug.Log("Boss�����d��~");
        }
    }

    void BossJumpMove()
    {
        if (bossJump == false)
        {
            bossJump = true;
            bossJumpVec = (objPlayer.transform.position - gameObject.transform.position).normalized;
        }
        else if (bossJump == true)
        {
            bossJump = false;
        }
    }
    void BossJumpTurn()
    {
        if(bossTurn == false)
        {
            bossTurn = true;
        }
        else if(bossTurn == true)
        {
            bossTurn = false;
        }
    }
    //void BossLeftAtk()
    //{
    //    Vector3 bossAtkPosition0;
    //    float bossAtkDistance0;
    //    bossAtkPosition0 = objBoss.transform.position + objBoss.transform.forward * 5.0f; 
    //    bossAtkDistance0 = Vector3.Distance(bossAtkPosition0, objPlayer.transform.position);
    //    if (bossAtkDistance0 < 2.0f)
    //    {
    //        PlayerInfo.PlayerHpCal(20);
    //    }
    //    else
    //    {
    //        Debug.Log("Boss�����d��~");
    //    }
    //}
    void BossRollAtk()
    {
        objBoss.transform.LookAt(objPlayer.transform.position);
        float bossAtkHorizontalDistance;//��V�Z��
        float bossAtkDistance;//���V�Z��

        float a;//�⨤�פ��l
        float b;//�⨤�פ���
        float cosValue;//cos��

        a = Vector3.Dot((objPlayer.transform.position - objBoss.transform.position), objBoss.transform.forward*2);
        b = Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * (objPlayer.transform.forward).magnitude*2;
        cosValue = a / b;

        bossAtkDistance = Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * cosValue;
        bossAtkHorizontalDistance = Mathf.Sqrt(Vector3.Distance(objPlayer.transform.position, objBoss.transform.position) * Vector3.Distance(objPlayer.transform.position, objBoss.transform.position)
                          - bossAtkDistance * bossAtkDistance);

        //�Z���ȫ�
        if(bossAtkDistance< 0)
        {
            bossAtkDistance = -bossAtkDistance;
        }

        if(bossAtkHorizontalDistance < 3.5f && bossAtkDistance <4.0f)
        {
            PlayerInfo.PlayerHpCal(11);
        }
        else if (bossAtkHorizontalDistance >= 3.5f || bossAtkDistance >= 4.0f || bossAtkDistance <= 5.0f)
        {
            Debug.Log("-----�S�u��-----");
        }
    }


    void BossRollMove()
    {
        if (bossRoll == false)
        {
            bossRoll = true;
        }
        else if (bossRoll == true)
        {
            bossRoll = false;
        }               
    }

    private int BossBehavior(int bossState)
    {
        int reBossDo = 0;
        if (bossState == 1)
        {
            reBossDo = Random.Range(0, 3);
        }
        //if (bossState == 2)
        //{
        //    reBossDo = Random.Range(3, 5);
        //}
        return reBossDo;
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;

    //    Gizmos.DrawWireSphere(objBoss.transform.position, 6.0f);
    //}
}
