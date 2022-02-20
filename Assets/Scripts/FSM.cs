using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
	/// <summary>
	/// �ˬd���A��delegate
	/// </summary>
	private delegate void CheckState();
	private CheckState mCheckState;
	/// <summary>
	/// �����A��delegate
	/// </summary>
	private delegate void DoState();
	private DoState mDoState;
	/// <summary>
	/// ��e���A
	/// </summary>
	private FSMState mCurrentState;
	/// <summary>
	/// �������
	/// </summary>
	//����ʧ@
	private Animator anim;
	//���⪺�Z�h�M
	private GameObject katana;
	/// <summary>
	/// ��������
	/// </summary>
	//is Battle?
	public bool isBattle;
    //is Attack?
    public bool isAttack;
	//is Skill?
	public bool isSkill;
	//Attack Count
	int atkCount;
	//�̫���U�ɶ�
	private float lastClick;
	//�^��
	public static int zAttack;
	/// <summary>
	/// ���ˬ���
	/// </summary>
	public static bool isGitHit;
	public static int gitHitCount;
	/// <summary>
	/// ���ʬ���
	/// </summary>
	//is Move?
	public bool isMove;
	public float moveSpeed;
	//is Dodge?
	public bool isDodge;
	/// <summary>
	/// ���`����
	/// </summary>
	public static bool isDeath;
 
	// Start is called before the first frame update
	public enum FSMState
	{
		NONE = -1,
		Idle,
		BattleIdle,
		Move,
		Attack,
		Skill,
		Dodge,
		GetHit,
		Die
	}

	private void Start()
	{
		isGitHit = false;
		isDeath = false;
		katana = GameObject.Find("Hoshi_katana");
		katana.SetActive(false);
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();
	}

	#region check
	private void CheckIdleState()
	{
		//Idle->Skill
		if (isSkill ==true)
		{		
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//Idle->Attack
		if (isAttack == true)
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
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
	}
	private void CheckBattleIdleState()
	{
		//BIdle->Skill
		if (isSkill == true)
		{
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//BIdle->Attack
		if (isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//BIdle->Move
		if (isMove == true)
		{
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//BIdle ->GetHit
		if (isGitHit == true)
		{
			anim.Play("GetHit");
			isGitHit = false;
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
			anim.SetBool("isBattle", true);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//�������^�kI
		if (isAttack == false && isBattle == false)
		{
			anim.SetBool("isAttack", false);
			anim.SetBool("isBattle", false);
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
		////��������Skill
		if(isSkill==true)
		{
			//isAttack = false;
			anim.SetBool("isAttack", false);
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
	}
	private void CheckSkillState()
	{
		if(isSkill==false &&isAttack ==true)
		{
			anim.SetBool("isSkill", false);
			anim.SetBool("Skill1", false);
			anim.SetBool("Skill2", false);
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		if (isSkill == false && isMove ==true)
		{
			anim.SetBool("isSkill", false);
			anim.SetBool("Skill1", false);
			anim.SetBool("Skill2", false);
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		if (isSkill == false)
		{
			anim.SetBool("isSkill", false);
			anim.SetBool("Skill1", false);
			anim.SetBool("Skill2", false);
			anim.SetBool("isBattle", true);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
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
			anim.SetBool("isBattle", true);
			mCurrentState = FSMState.BattleIdle;
			mCheckState = CheckBattleIdleState;
			mDoState = DoBattleIdleState;
		}
		//�����ʥB�D�԰��^I
		if (isMove == false && isBattle == false)
		{
			anim.SetBool("isWalkF", false);
			anim.SetBool("isBattle", false);
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
		//�q���ʤ������
		if(isMove==true && isAttack==true)
        {
			anim.SetBool("isAttack", true);
			isMove = false;
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//�q���ʤ���ޯ�
		if (isSkill==true)
		{
			isMove = false;
			anim.SetBool("isSkill", true);
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
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
			isBattle = true;
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
			//var lastClick = Time.time;
			isAttack = true;
			isBattle = true;
			zAttack = 1;
			atkCount = 1;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			isAttack = false;
			zAttack = 0;
			atkCount = 0;
		}
		//Ĳ�oSkill
		if(Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{
			isBattle = true;
			isSkill = true;
			if(Input.GetKey(KeyCode.X))
			{
				isAttack = false;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				isAttack = false;
				anim.SetBool("Skill2", true);
			}
		}
	}
	private void DoBattleIdleState()
	{
		
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		//�j��Idle
		if (Input.GetKeyDown(KeyCode.B))
		{
			isBattle = false;
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
			isBattle = true;
			isAttack = true;
			zAttack = 1;
			atkCount = 1;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			isAttack = false;
			zAttack = 0;
			atkCount = 0;
		}
		//Ĳ�oSkill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{
			isBattle = true;
			isSkill = true;
			if (Input.GetKey(KeyCode.X))
			{
				isAttack = false;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				isAttack = false;
				anim.SetBool("Skill2", true);
			}
		}
	}
	private void DoAttackState()
	{
		isGitHit = false;
		#region ������
		//�P�_�ĤG�U
		if (Input.GetKeyDown(KeyCode.Z) && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01"))
		{
			anim.SetInteger("combo2", anim.GetInteger("combo2") + 1);
			if (anim.GetInteger("combo2") <= 1)
			{
				zAttack = 2;
			}
			atkCount = 2;
		}
		//�P�_�ĤT�U
		if (Input.GetKeyDown(KeyCode.Z) && anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02"))
		{
			anim.SetInteger("combo3", anim.GetInteger("combo3") + 1);

			if (anim.GetInteger("combo3") <= 1)
			{
				zAttack = 3;
			}
			atkCount = 3;
		}
		//�����M���^�k
		//�Ĥ@�U�M��
		if (atkCount == 1
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK01")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
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
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
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
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
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
		////Ĳ�oSkill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{
			isBattle = true;
			isSkill = true;
			if (Input.GetKey(KeyCode.X))
			{
				isAttack = false;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				isAttack = false;
				anim.SetBool("Skill2", true);
			}
		}
		#endregion
		#region ����
		//if (Input.GetKeyDown(KeyCode.Z) && CheckCombo(1, lastClick))
		//{
		//	lastClick = Time.time;
		//	Debug.Log("�s��Ĳ�o");
		//}
		//Debug.Log("atkCount               "+ atkCount);
		//Debug.Log("zAtack                 "+ zAtack);
		#endregion
	}
    private void DoSkillState()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1 &&(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill2")))
		{
			isSkill = false;			
		}

		//���U����
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//var lastClick = Time.time;
			isAttack = true;
			isBattle = true;
			zAttack = 1;
			atkCount = 1;
		}
		//��������V��
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
        {
            isMove = true;
            isAttack = false;
            zAttack = 0;
            atkCount = 0;
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
			isMove = false;
		}
		//���ʤ�Ĳ�o����
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{				
				isAttack = true;
				isBattle = true;
				zAttack = 1;
				atkCount = 1;
			}
		}
		//Ĳ�oSkill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
		{			
			if (Input.GetKey(KeyCode.X))
			{
				
				isAttack = false;
				isBattle = true;
				isSkill = true;
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
			
				isAttack = false; 
				isBattle = true;
				isSkill = true;
				anim.SetBool("Skill2", true);
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
		isDeath = false;
		isMove = false;
		isAttack = false;
		isGitHit = false;
		isSkill = false;
		
		anim.Play("Die");
	}
	#endregion


	// Update is called once per frame
	void Update()
	{
		//�������A
		//Debug.Log("�ثe���A          " + mCurrentState);
		//Debug.Log(isGitHit);
		//�P�_���@��Attack
		zAttack = 0;
		//�p�G���`�F
		if(isDeath)
        {
			mCurrentState = FSMState.Die;
			mCheckState = CheckDieState;
			mDoState = DoDieState;
		}
		//�n���n�ޤM
		if (isBattle || isAttack)
		{
			katana.SetActive(true);
		}
        else 
		{
			katana.SetActive(false);
		}
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

	private bool CheckCombo(float cdTime, float lastClickTime)
	{

		if (Time.time - lastClickTime < cdTime)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

