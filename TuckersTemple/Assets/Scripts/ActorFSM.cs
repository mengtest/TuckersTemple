using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ActorFSM : MonoBehaviour
{
	public GameObject gm;
	public FSMSystem fsm;
	public bool doneSliding;
	public int direction;
	private Vector2[] v2Dirs = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };


	public void SetTransition(Transition t) { fsm.PerformTransition(t); }

	public void Start()
	{
		doneSliding = false;
		MakeFSM();
	}

	public void Update()
	{
		fsm.CurrentState.Reason(gm, gameObject);
		fsm.CurrentState.Act(gm, gameObject);
	}

	// The NPC has two states: idle and moving
	// If it's on idle and userSwipe transition is fired, it changes to moving
	// If it's on moving and reachedGoal transition is fired, it returns to idle
	private void MakeFSM()
	{
		IdleAState idle = new IdleAState(this);
		idle.AddTransition(Transition.FoundMove, StateID.CheckA);

		CheckAState check = new CheckAState (this);
		check.AddTransition (Transition.EnemyFound, StateID.EnemyDeadA);
		check.AddTransition (Transition.TrapFound, StateID.TrapDeadA);
		check.AddTransition (Transition.GoalFound, StateID.WinA);
		check.AddTransition (Transition.PathFound, StateID.WalkA);

		WalkAState walk = new WalkAState (this);
		check.AddTransition (Transition.FinishedWalk, StateID.IdleA);

		fsm = new FSMSystem();
		fsm.AddState (idle );
		fsm.AddState (check);
		fsm.AddState (walk );
	}

	public int findNextMove(int dir)
	{
		//order to try in is straight->right->left->back

		//this is the modifies to the directions something can face
		int[] dirMods = { 0, 1, -1, 2 };
		//directions are 0,1,2,3, with 0 being up and going clockwise.
		for (int i = 0; i < 4; i++)
		{
			//make a current direction by adding the direction modifier to the direction
			int currDir = dir + dirMods[i];

			//Normalize currDir within 0 to 3
			if (currDir > 3)
			{
				currDir -= 4;
			}
			else if (currDir < 0)
			{
				currDir += 4;
			}

			//RAYCAST LASER BEAMS ♫♫♫♫♫
			RaycastHit2D ray = Physics2D.Raycast(transform.position, v2Dirs[currDir], Tile.GetComponent<Renderer>().bounds.size.x, LayerMask.GetMask("Wall"));

			if (ray.collider != null && !(ray.collider.tag == "Wall" || ray.collider.tag == "OuterWall"))
			{
				currDirection = currDir;
				return currDir;
			}
		}
		return -1;//walls in all 4 directions, no moves found
	}
}

public class IdleAState : FSMState
{
	public ActorFSM controlref;

	public IdleAState(ActorFSM control)
	{
		stateID = StateID.IdleA;
		controlref = control;
	}

	public override void Reason(GameObject gm, GameObject npc)
	{
		if (controlref.doneSliding && controlref.findNextMove() >= 0) 
		{
			npc.GetComponent<ActorFSM>().SetTransition(Transition.FoundMove); //to Look
		}
	}

	public override void Act(GameObject gm, GameObject npc)
	{
		//idle	
	}

} //MoveState

public class CheckAState : FSMState
{
	ActorFSM controlref;

	public CheckAState(ActorFSM control)
	{
		stateID = StateID.LookA;
		controlref = control;
	}

	public override void Reason(GameObject gm, GameObject npc)
	{

	}

	public override void Act(GameObject gm, GameObject npc)
	{

	}

} // CheckAState

public class WalkAState : FSMState
{
	ActorFSM controlref;

	public WalkAState(ActorFSM control)
	{
		stateID = StateID.LookA;
		controlref = control;
	}

	public override void Reason(GameObject gm, GameObject npc)
	{

	}

	public override void Act(GameObject gm, GameObject npc)
	{

	}

} // WalkAState
	