using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    public GameObject player;
    private FSMSystem fsm;
    public Vector2 goalPos;

    public void SetTransition(Transition t) { fsm.PerformTransition(t); }

    public void Start()
    {
        goalPos = transform.position;
        MakeFSM();
    }

    public void Update()
    {
        fsm.CurrentState.Reason(player, gameObject);
        fsm.CurrentState.Act(player, gameObject);
    }

    // The NPC has two states: idle and moving
    // If it's on idle and userSwipe transition is fired, it changes to moving
    // If it's on moving and reachedGoal transition is fired, it returns to idle
    private void MakeFSM()
    {
        MoveState moving  = new MoveState(this);
        moving.AddTransition(Transition.ReachedGoal, StateID.Idle);

        IdleState idle = new IdleState(this);
        idle.AddTransition(Transition.UserSwiped, StateID.Moving);

        fsm = new FSMSystem();
        fsm.AddState(idle);
        fsm.AddState(moving);
    }
}

public class MoveState : FSMState
{
    public NPCControl controlref;
    private float speed = .05f;

    public MoveState(NPCControl control)
    {
        stateID = StateID.Moving;
        controlref = control;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (npc.transform.position.x == controlref.goalPos.x && npc.transform.position.y == controlref.goalPos.y)
        {
            npc.GetComponent<NPCControl>().SetTransition(Transition.ReachedGoal);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        npc.transform.position = Vector2.MoveTowards(npc.transform.position, controlref.goalPos, speed);
    }

} //MoveState

public class IdleState : FSMState
{
    NPCControl controlref;

    public IdleState(NPCControl control)
    {
        stateID = StateID.Idle;
        controlref = control;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if(npc.transform.position.x != controlref.goalPos.x || npc.transform.position.y != controlref.goalPos.y)
        {
            npc.GetComponent<NPCControl>().SetTransition(Transition.UserSwiped);
        }
        
    }

    public override void Act(GameObject player, GameObject npc)
    {
    }

} // IdleState

public class WrapState : FSMState
{
    NPCControl controlref;

    public WrapState(NPCControl control)
    {
        stateID = StateID.Wrapping;
        controlref = control;
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        //reached goal?
        //fire reached goal trans
    }

    public override void Act(GameObject player, GameObject npc)
    {
        //whatever we want for wrapping, that moves towards goalpos
    }

} // ChasePlayerState