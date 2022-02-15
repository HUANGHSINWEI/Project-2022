using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
	//�ˬd���A��delegate
	private delegate void CheckState();
	private CheckState mCheckState;
	//�����A��delegate
	private delegate void DoState();
	private CheckState mDoState;

	//����ʧ@
	private Animator anim;
	//
	private GameObject katana;
	#region ��������
	//is Battle?
	public bool isBattle;
    //is Attack?
    public bool isAttack;
	//�^��
	public static int zAtack;
	#region �ˮ`����
	public static bool isGitHit;
	#endregion
	//Attack Count
	int atkCount;
    #endregion

    #region ���ʬ���
    //is Move?
    public bool isMove;
	public float moveSpeed;
	//is Dodge?
	public bool isDodge;
	#endregion

	#region ���`����
	public static bool isDeath;
    #endregion

    


    private FSMState mCurrentState;
	// Start is called before the first frame update
	public enum FSMState
	{
		NONE = -1,
		Idle,
		BattleIdle,
		Move,
		Attack,
		Dodge,
		GetHit
	}

	private void Start()
	{
		isGitHit = false;
		isDeath = false;
		katana = GameObject.Find("katanaA");
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();
	}

	#region check
	private void CheckIdleState()
	{
		//Idle->Attack
		if(isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//Idle->Move
		if (isMove ==true)
        {
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//Idle ->GetHit
		if (isGitHit == true)
		{
			anim.Play("GetHit");
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}

	}
	private void CheckBattleIdleState()
	{
		//Idle->Attack
		if (isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//Idle->Move
		if (isMove == true)
		{
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//Idle ->GetHit
		if (isGitHit == true)
		{
			anim.Play("GetHit");
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
	}	
	private void CheckAttackState()
    {
		if (Input.GetKeyDown(KeyCode.Q))
		{ 
			anim.SetBool("isAttack", false);
			atkCount = 0;
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//�������^�kBI
		if(isAttack==false && isBattle==true)
        {
			anim.SetBool("isAttack", false);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//�������^�kI
		if (isAttack == false && isBattle == false)
		{
			anim.SetBool("isAttack", false);
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//�����ಾ��
		if (isAttack == false && isMove == true)
		{
			anim.SetBool("isAttack", false);
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}	
	}
	private void CheckDodgeState()
	{
		
	}
	private void CheckMoveState()
	{
		//�����ʥB�԰��^BI
		if(isMove==false && isBattle == true)
		{
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//�����ʥB�D�԰��^I
		if (isMove == false && isBattle == false)
		{
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//�q���ʤU�����
		if(isMove==true && isAttack==true)
        {
			anim.SetBool("isAttack", true);
			isMove = false;
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		
	}
	private void CheckGetHitState()
	{
		if(isBattle==true && isGitHit == false)
        {
			anim.SetBool("isBattle", true);
			anim.SetBool("isGetHit", false);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		if(isGitHit==true &&isMove==true)
        {
			isGitHit = false;
			anim.SetBool("isGetHit", false);
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
	}
	private void CheckDieState()
    {

    }
	#endregion

	#region Do
	private void DoIdleState()
	{		
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		//�j��BI
		if (Input.GetKeyDown(KeyCode.B))
		{
			//anim.SetBool("isAttack", false);
			anim.SetBool("isBattle", true);
			isBattle = true;
			atkCount = 0;
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//���U����
		if (Input.GetKeyDown(KeyCode.Z))
		{
			isAttack = true;
			zAtack = 1;
			atkCount = 1;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			isAttack = false;
			zAtack = 0;
			atkCount = 0;
		}
	}
	private void DoBattleIdleState()
	{
		
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		//�j��Idle
		if (Input.GetKeyDown(KeyCode.B))
		{
			//anim.SetBool("isAttack", false);
			anim.SetBool("isBattle", false);
			isBattle = false;
			atkCount = 0;
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//���U����
		if (Input.GetKeyDown(KeyCode.Z))
		{
			isAttack = true;
			zAtack = 1;
			atkCount = 1;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			isAttack = false;
			zAtack = 0;
			atkCount = 0;
		}
	}
	private void DoAttackState()
	{
		isGitHit = false;
		//�P�_�ĤG�U
		if (atkCount == 1
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
			&& Input.GetKeyDown(KeyCode.Z))
		{
			anim.SetInteger("combo2", anim.GetInteger("combo2") + 1);
			zAtack = 2;
			atkCount = 2;
		}
		//�P�_�ĤT�U
		if (atkCount == 2
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
			&& Input.GetKeyDown(KeyCode.Z))
		{
			anim.SetInteger("combo3", anim.GetInteger("combo3") + 1);
			zAtack = 3;
			atkCount = 3;
		}
		//�����M���^�k
		//�Ĥ@�U�M��
		if (atkCount == 1
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			atkCount = 0;
			isAttack = false;
			//�P�_���S�������������V��
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			{
				isMove = true;
			}
		}
		//�ĤG�U�M��
		if (atkCount == 2
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			atkCount = 0;
			anim.SetInteger("combo2", 0);
			isAttack = false;
			//�P�_���S�������������V��
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			{
				isMove = true;
			}
		}
		//�ĤT�U�M��
		if (atkCount == 3
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK03")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			atkCount = 0;
			anim.SetInteger("combo3", 0);
			isAttack = false;
			//�P�_���S�������������V��
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			{
				isMove = true;
			}
		}
	}
	private void DoDodgeState()
	{

	}
	private void DoMoveState()
	{
		isGitHit = false;
		//��������V��
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			if (isBattle)
			{
				isMove = false;
			}
			else
			{
				isMove = false;
			}
		}
		//���ʤ�Ĳ�o����
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{				
				isAttack = true;
				zAtack = 1;
				atkCount = 1;
			}
		}
	}
	private void DoGetHitState()
	{
		isBattle = true;
		
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			isGitHit = false;			
			atkCount = 0;			
		}
		////�P�_���S�������������V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;			
		}
	}
	private void DoDieState()
	{

	}
	#endregion


	// Update is called once per frame
	void Update()
	{
		//�������A
		Debug.Log("�ثe���A          " + mCurrentState);		
		//�P�_���@��Attack
		zAtack = 0;
		//�O�_����ˮ`
		//Debug.Log(MonsterDmg.isGitHit);
		mCheckState();
		//���A���ƻ�
		mDoState();
	}

	private void FixedUpdate()
	{
		if (isMove == true)
		{
			PlayControl.Move(moveSpeed);
		}
	}
}

