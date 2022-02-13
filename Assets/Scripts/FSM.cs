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

	//is Battle?
	bool isBattle;
	//is Attack?
	bool isAttack;
	//Attack Count
	int atkCount;
	//is Dodge?
	bool isDodge;
	//is Move?
	bool isMove;


	private FSMState mCurrentState;
	// Start is called before the first frame update
	public enum FSMState
	{
		NONE = -1,
		Idle,
		BattleIdle,
		Move,
		Attack,
		Dodge
	}

	private void Start()
	{
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();
	}

	#region check
	private void CheckIdleState()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{ 
			isAttack = true;
			atkCount = 1;
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//if(isDodge)
		//{

		//}
		//if(isMove)
		//{
		//	mCurrentState = FSMState.Move;
		//	mCheckState = CheckMoveState;
		//	mDoState = DoMoveState;
		//}
		//if(!isBattle)
		//{
		//	mCurrentState = FSMState.Idle;
		//	mCheckState = CheckIdleState; 
		//	mDoState = DoIdleState;
		//}
		//else
		//{
		//	mCurrentState = FSMState.BattleIdle;
		//	mCheckState = CheckBattleIdleState;
		//	mDoState = DoBattleIdleState;
		//}
	}
	private void CheckBattleIdleState()
	{
		
		if (Input.GetKeyDown(KeyCode.Z))
		{
			isAttack = true;
			atkCount = 1;
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		
		//if (isAttack)
		//{

		//}
		//if (isDodge)
		//{

		//}
		//if (isMove)
		//{
		//	mCurrentState = FSMState.Move;
		//	mCheckState = CheckMoveState;
		//	mDoState = DoMoveState;
		//}
		//if (!isBattle)
		//{
		//	mCurrentState = FSMState.Idle;
		//	mCheckState = CheckIdleState;
		//	mDoState = DoIdleState;
		//}
		//else
		//{
		//	mCurrentState = FSMState.BattleIdle;
		//	mCheckState = CheckBattleIdleState;
		//	mDoState = DoBattleIdleState;
		//}
	}
	private void CheckMoveState()
	{
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
	}
    private void CheckDodgeState()
	{
		
	}
	#endregion

	#region Do
	private void DoIdleState()
	{
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
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
	}
	private void DoBattleIdleState()
	{
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
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
	}
	private void DoAttackState()
	{
		//�P�_�ĤG�U
        if (atkCount == 1
            && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
			&& Input.GetKeyDown(KeyCode.Z))
        {
			anim.SetInteger("combo2", anim.GetInteger("combo2") + 1);
			atkCount = 2;
		}
		//�P�_�ĤT�U
		if (atkCount == 2
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
			&& Input.GetKeyDown(KeyCode.Z))
		{
			anim.SetInteger("combo3", anim.GetInteger("combo3") + 1);
			atkCount = 3;
		}
		//�����M���^�k
		//�Ĥ@�U�M��
		if (atkCount == 1
            && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
			atkCount = 0;
			anim.SetBool("isAttack", false);
			if (isBattle)
			{
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
            else
            {
				mCurrentState = FSMState.Idle;
				mCheckState = CheckIdleState;
				mDoState = DoIdleState;
			}
        }
		//�ĤG�U�M��
		if (atkCount == 2
            && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
			atkCount = 0;
			anim.SetInteger("combo2", 0);
			anim.SetBool("isAttack", false);
			if (isBattle)
			{
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
			else
			{
				mCurrentState = FSMState.Idle;
				mCheckState = CheckIdleState;
				mDoState = DoIdleState;
			}
		}
		//�ĤT�U�M��
		if (atkCount == 3
            && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK03")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
			atkCount = 0;
			anim.SetInteger("combo3", 0);
			anim.SetBool("isAttack", false);
			if (isBattle)
			{
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
			else
			{
				mCurrentState = FSMState.Idle;
				mCheckState = CheckIdleState;
				mDoState = DoIdleState;
			}
		}
    }
	private void DoDodgeState()
	{

	}
	private void DoMoveState()
	{
		
	}
	#endregion


	// Update is called once per frame
	void Update()
	{
		//Debug.Log("�ثe���A          " + mCurrentState);
		//�������A
		mCheckState();

		//���A���ƻ�
		mDoState();
	}
}

