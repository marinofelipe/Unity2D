﻿using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{

	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;


	// Use this for initialization
	protected override void Start ()
	{
		GameManager.instance.addEnemyToList (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		if (skipMove) {
			skipMove = false;
			return;
		}

		base.AttemptMove<T> (xDir, yDir);
		skipMove = true;
	}

	public void moveEnemy ()
	{
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;

		AttemptMove<Player> (xDir, yDir);
	}

	protected override void onCantMove<T> (T component)
	{
		Player hitPlayer = component as Player;
		animator.SetTrigger ("enemyAttack");
		hitPlayer.looseFood (playerDamage); 
	}
}
