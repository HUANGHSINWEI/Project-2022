using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public bool canMove;
	public static float moveSpeed;
	public bool ChangeForword =false;
	// is AtkToMove
	public bool isAtkToMove;
	//is Dodge?
	public bool isDodge;
	// is SkillToDodge?
	public bool isSkillToDodge;
	/// <summary>
	/// ���`����
	/// </summary>
	public static bool isDeath;
	//BItoI check
	private float BItoITime;

	public static float DizzyCount;

	private GameObject GameOver;
	private GameObject GameOverText;
	private GameObject GameOverText2;

	public static GameObject DeadBody;

	public static bool BossAlive;
	private float disappearTime;
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
		Die,
	}

	private void Start()
	{
		disappearTime = 0;

		isGitHit = false;
		isDeath = false;

		GameOver= GameObject.Find("GameOver");
		GameOver.SetActive(false);
		GameOverText = GameObject.Find("GameOverText");
		GameOverText.SetActive(false);

		BossAlive = true;
		GameOverText2 = GameObject.Find("GameOverText2");
		GameOverText2.SetActive(false);

		katana = GameObject.Find("Hoshi_katana");
		katana.SetActive(false);
		mCurrentState = FSMState.Idle;
		mCheckState = CheckIdleState;
		mDoState = DoIdleState;
		anim = GetComponent<Animator>();

		moveSpeed = 4;
	}

	#region check
	private void CheckIdleState()
	{	
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
		//Idle ->Dodge
		else if (isDodge ==true)
		{
			anim.SetBool("isDodge", true);
			mCurrentState = FSMState.Dodge;
			mCheckState = CheckDodgeState;
			mDoState = DoDodgeState;
		}
		//Idle->Skill
		else if (isSkill == true)
		{
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//Idle->Attack
		else if (isAttack == true)
		{
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//Idle->Move
		else if (isMove == true)
		{
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
	}
	private void CheckBattleIdleState()
	{
		
		//BIdle ->GetHit
		if (isGitHit == true)
		{
			BItoITime = 0;
			anim.Play("GetHit");
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
		//BIdle ->Dodge
		else if (isDodge == true)
		{
			BItoITime = 0;
			anim.SetBool("isDodge", true);
			mCurrentState = FSMState.Dodge;
			mCheckState = CheckDodgeState;
			mDoState = DoDodgeState;
		}
		//BIdle->Skill
		else if (isSkill == true)
		{
			BItoITime = 0;
			anim.SetBool("isSkill", true);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		//BIdle->Attack
		else if (isAttack == true)
		{
			BItoITime = 0;
			anim.SetBool("isAttack", true);
			mCurrentState = FSMState.Attack;
			mCheckState = CheckAttackState;
			mDoState = DoAttackState;
		}
		//BIdle->Move
		else if (isMove == true)
		{
			BItoITime = 0;
			anim.SetBool("isWalkF", true);
			mCurrentState = FSMState.Move;
			mCheckState = CheckMoveState;
			mDoState = DoMoveState;
		}
		//BIt ->I
		if (CheckBItoI())
		{
			BItoITime = 0;
			anim.SetBool("isBattle", false);
			isBattle = false;
			atkCount = 0;
			mCurrentState = FSMState.Idle;
			mCheckState = CheckIdleState;
			mDoState = DoIdleState;
		}
	}	
	private void CheckAttackState()
    {
		
		if(isAttack==false )
        {
			if(isMove == true)//�����ಾ��
			{
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				anim.SetBool("isAttack", false);
				anim.SetBool("isWalkF", true);
				mCurrentState = FSMState.Move;
				mCheckState = CheckMoveState;
				mDoState = DoMoveState;
			}
			else if (isBattle == true)//�������^�kBI
			{
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				anim.SetBool("isAttack", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}
		if (isAttack == true)
		{	
			if(isGitHit==true &&(PlayerInfo.DizzyCount%3==1))
            {
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				isAttack = false;
				anim.SetBool("isAttack", false);
				anim.Play("GetHit");
				isGitHit = false;
				anim.SetBool("isGetHit", true);
				mCurrentState = FSMState.GetHit;
				mCheckState = CheckGetHitState;
				mDoState = DoGetHitState;

			}
			//��������Dodge
			else if (isDodge == true)
            {
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				isAttack = false;
				anim.SetBool("isAttack", false);
				anim.SetBool("isDodge", true);
				mCurrentState = FSMState.Dodge;
				mCheckState = CheckDodgeState;
				mDoState = DoDodgeState;
			}
			//��������Skill
			else if (isSkill == true)
			{
				anim.SetInteger("combo2", 0);
				anim.SetInteger("combo3", 0);
				isAttack = false;
				anim.SetBool("isAttack", false);
				anim.SetBool("isSkill", true);
				mCurrentState = FSMState.Skill;
				mCheckState = CheckSkillState;
				mDoState = DoSkillState;
			}
		}
	}
	private void CheckSkillState()
	{
		//Skill����½�u
		if(isSkill == true)
        {
			if(isDodge == true)
            {
				isSkillToDodge = false;
				anim.SetBool("isSkill", false);
				anim.SetBool("Skill1", false);
				anim.SetBool("Skill2", false);
				anim.SetBool("Skill3", false);
				anim.SetBool("isDodge", true);
				mCurrentState = FSMState.Dodge;
				mCheckState = CheckDodgeState;
				mDoState = DoDodgeState;
			}
        }
		//���㼽���ޯ�
		if (isSkill == false)
		{
			//��첾��
			if (isMove == true)
			{
				anim.SetBool("isSkill", false);
				anim.SetBool("Skill1", false);
				anim.SetBool("Skill2", false);
				anim.SetBool("Skill3", false);
				anim.SetBool("isWalkF", true);
				mCurrentState = FSMState.Move;
				mCheckState = CheckMoveState;
				mDoState = DoMoveState;
			}
			else//�^��BI
			{
				anim.SetBool("isSkill", false);
				anim.SetBool("Skill1", false);
				anim.SetBool("Skill2", false);
				anim.SetBool("Skill3", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}
	}
	private void CheckDodgeState()
	{
		if(isDodge ==false)
		{
			if (isMove == true)
			{
				anim.SetBool("isDodge", false);
				anim.SetBool("isWalkF", true);
				mCurrentState = FSMState.Move;
				mCheckState = CheckMoveState;
				mDoState = DoMoveState;
			}
			else
			{
				anim.SetBool("isDodge", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}

		//if(isGitHit == true)
		//{
		//	isMove = false;
		//	canMove = false;
		//	anim.SetBool("isWalkF", false);
		//	anim.Play("GetHit");
		//	isGitHit = false;
		//	anim.SetBool("isGetHit", true);
		//	mCurrentState = FSMState.GetHit;
		//	mCheckState = CheckGetHitState;
		//	mDoState = DoGetHitState;
		//}
	}
	private void CheckMoveState()
	{
		//���������
		if (isGitHit == true)
		{
			isMove = false;
			canMove = false;
			anim.SetBool("isWalkF", false);
			anim.Play("GetHit");
			isGitHit = false;
			anim.SetBool("isGetHit", true);
			mCurrentState = FSMState.GetHit;
			mCheckState = CheckGetHitState;
			mDoState = DoGetHitState;
		}
		//�q���ʤ���ޯ�
		else if (isSkill == true)
		{
			isMove = false;
			anim.SetBool("isSkill", true);
			canMove = false;
			anim.SetBool("isWalkF", false);
			mCurrentState = FSMState.Skill;
			mCheckState = CheckSkillState;
			mDoState = DoSkillState;
		}
		else if (isMove == true )
		{			
			if(isDodge == true)//�q���ʤ���½�u
			{
				anim.SetBool("isDodge", true);
				isMove = false;
				canMove = false;
				anim.SetBool("isWalkF", false);
				mCurrentState = FSMState.Dodge;
				mCheckState = CheckDodgeState;
				mDoState = DoDodgeState;
			}
			else if (isAttack == true)//�q���ʤ������
			{
				anim.SetBool("isAttack", true);
				isMove = false;
				canMove = false;
				anim.SetBool("isWalkF", false);
				mCurrentState = FSMState.Attack;
				mCheckState = CheckAttackState;
				mDoState = DoAttackState;
			}
		}
		else if (isMove==false)
		{
			if (isBattle == true)//�����ʥB�԰��^BI
			{
				canMove = false;
				anim.SetBool("isWalkF", false);
				anim.SetBool("isBattle", true);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
			else if(isBattle == false)//�����ʥB�D�԰��^I
            {
				canMove = false;
				anim.SetBool("isWalkF", false);
				anim.SetBool("isBattle", false);
				mCurrentState = FSMState.Idle;
				mCheckState = CheckIdleState;
				mDoState = DoIdleState;
			}
		}

		
		
	}
	private void CheckGetHitState()
	{
		if(isGitHit == false)
        {
			if (isBattle == true)
			{
				anim.SetBool("isBattle", true);
				anim.SetBool("isGetHit", false);
				mCurrentState = FSMState.BattleIdle;
				mCheckState = CheckBattleIdleState;
				mDoState = DoBattleIdleState;
			}
		}
		//if(isGitHit==true &&isMove==true)
		//{
		//	isGitHit = false;
		//	anim.SetBool("isGetHit", false);
		//	anim.SetBool("isWalkF", true);
		//	mCurrentState = FSMState.Move;
		//	mCheckState = CheckMoveState;
		//	mDoState = DoMoveState;
		//}
	}
	private void CheckDieState()
    {
		

	}
	#endregion

	#region Do
	private void DoIdleState()
	{
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;

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
		if(Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C)|| Input.GetKey(KeyCode.V))
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
			if(Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				anim.SetBool("Skill3", true);
			}
		}
		//Ĳ�o½�u
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
	}
	private void DoBattleIdleState()
	{	
		isBattle = true;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		anim.SetInteger("combo2", 0);
		anim.SetInteger("combo3", 0);
		anim.SetBool("isAttack", false);
		//���U����
		if (Input.GetKeyDown(KeyCode.Z))
		{			
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
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C)|| Input.GetKey(KeyCode.V))
		{			
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
			if (Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				anim.SetBool("Skill3", true);
			}
		}
		//Ĳ�o½�u
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
	}
	private bool CheckBItoI()
	{
		BItoITime += Time.deltaTime;
		if (BItoITime > 6)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private void DoAttackState()
	{
		isGitHit = false;
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
			////�P�_���S�������������V��
			//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			//{
			//	CheckForward();
			//	if (isAtkToMove)
			//	{
			//		isMove = true;
			//	}
			//}
		}
		//�ĤG�U�M��
		if (atkCount == 2
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK02")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			anim.SetInteger("combo2", 0);
			isAttack = false;
			////�P�_���S�������������V��
			//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			//{
			//	CheckForward();
			//	if (isAtkToMove)
			//	{
			//		isMove = true;
			//	}
			//}
		}
		//�ĤT�U�M��
		if (atkCount == 3
			&& anim.GetCurrentAnimatorStateInfo(0).IsName("ATTACK03")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
		{
			atkCount = 0;
			anim.SetInteger("combo3", 0);
			isAttack = false;
			anim.SetBool("isAttack", false);
			////�P�_���S�������������V��
			//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
			//{
			//	CheckForward();
			//	if (isAtkToMove)
			//	{
			//		isMove = true;
			//	}
			//}
		}
		//Ĳ�oSkill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C)|| Input.GetKey(KeyCode.V))
		{
			isBattle = true;
			isSkill = true;
			if (Input.GetKey(KeyCode.X))
			{
				anim.SetBool("Skill1", true);
			}
			if (Input.GetKey(KeyCode.C))
			{
				anim.SetBool("Skill2", true);
			}
			if (Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				anim.SetBool("Skill3", true);
			}
		}
		//Ĳ�o½�u
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
        {
			CheckForward();		
            //if (isAtkToMove)
            //{
                isMove = true;
            //}
        }

    }
	private void CheckForward()
    {
		var x = -Input.GetAxis("Vertical");
		var z = Input.GetAxis("Horizontal");

		var a = -Camera.main.transform.forward * x;
		a.y = 0;
		var b = Camera.main.transform.right * z;
		b.y = 0;

		gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, new Vector3(a.x, 0, b.z), 0.95f);
	}
    private void DoSkillState()
	{
		isSkillToDodge = false;
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1 &&(anim.GetCurrentAnimatorStateInfo(0).IsName("Skill1") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skill3")))
		{
			isSkill = false;			
		}

		////���U����
		//if (Input.GetKeyDown(KeyCode.Z))
		//{
		//	//var lastClick = Time.time;
		//	isAttack = true;
		//	isBattle = true;
		//	zAttack = 1;
		//	atkCount = 1;
		//}
		//��������V��
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			if (ChangeForword)
			{
				CheckForward();
			}
			isMove = true;
			isAttack = false;
			zAttack = 0;
			atkCount = 0;
		}
		//Ĳ�o½�u
		if (Input.GetKey(KeyCode.Space))
		{
			if (isSkillToDodge == true)
			{
				isDodge = true;
			}
		}

	}
	private void DoDodgeState()
	{
		isBattle = true;
		isAttack = false;
		isMove = false;
		isSkill = false;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		zAttack = 0;
		atkCount = 0;
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("DODGE"))
		{
			isDodge = false;
		}
		//���U��V��
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
		}
	}
	private void DoMoveState()
	{
		//isGitHit = false;
		anim.SetBool("isAttack", false);
		canMove = true;
		//��������V��
		if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
		{
			isMove = false;
		}
		//���ʤ�Ĳ�o����
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		{
			isMove = true;
			if (Input.GetKeyDown(KeyCode.Z))
			{				
				isAttack = true;
				isBattle = true;
				zAttack = 1;
				atkCount = 1;
			}
		}
		//Ĳ�oSkill
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.V))
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
			if (Input.GetKey(KeyCode.V))
			{
				isAttack = false;
				isBattle = true;
				isSkill = true;
				anim.SetBool("Skill3", true);
			}
		}
		//Ĳ�o½�u
		if (Input.GetKey(KeyCode.Space))
		{
			isDodge = true;
		}
	}
	private void DoGetHitState()
	{
		isBattle = true;
		canMove = false;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		anim.SetBool("isAttack", false);
		if (PlayerInfo.DizzyCount>=50)
		{
			PlayerInfo.DizzyCount = 0;
		}
		//Debug.Log("GetHit  "+ isGitHit);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetHit")
			&& anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
		{
			//Debug.Log("�]��");
			isGitHit = false;			
			atkCount = 0;			
		}
		//////�P�_���S�������������V��
		//if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) != false)
		//{
		//	isMove = true;			
		//}
	}
	private void DoDieState()
	{
		isDeath = false;
		isMove = false;
		isAttack = false;
		isGitHit = false;
		isSkill = false;
		isDodge = false;
		canMove = false;
		isSkillToDodge = false;
		ChangeForword = false;
		isAtkToMove = false;
		anim.Play("Die");

		GameOver.SetActive(true);
		StartCoroutine(Wait());
		
	}
	#endregion


	// Update is called once per frame
	void Update()
	{
		//�������A
		//Debug.Log("�ثe���A          " + mCurrentState);
		Debug.Log("DI                      "+PlayerInfo.DizzyCount);
		//�P�_���@��Attack
		zAttack = 0;
		//�p�G���`�F
		if (isDeath)
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
		//Boss ���`
		if(BossAlive == false)
		{
			StartCoroutine(Wait2());
			StartCoroutine(Wait3());
		}
		mCheckState();
		//���A���ƻ�
		mDoState();
	}

	private void FixedUpdate()
	{
		if (canMove == true)
		{
			PlayControl.Move(moveSpeed);
		}
	}
	private void SkillToDodge()
    {
		isSkillToDodge = true;
	}
	private void AtkToMove()
    {
		isAtkToMove = true;
    }

	void PlayerSkillChangeForword()
	{
		if (ChangeForword == false)
		{
			ChangeForword = true;
		}
		else if (ChangeForword == true)
		{
			ChangeForword = false;
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(5.0f);
		GameOverText.SetActive(true);
		if (Input.anyKey)
		{
			SceneManager.LoadScene(0);
		}
	}

	IEnumerator Wait2()
	{
		yield return new WaitForSeconds(5.0f);
		GameOverText2.SetActive(true);
		if (Input.anyKey)
		{
			SceneManager.LoadScene(0);
		}
	}
	IEnumerator Wait3()
	{
		yield return new WaitUntil(AlphaToZero);

	}

	bool AlphaToZero()
	{
		if (true)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

