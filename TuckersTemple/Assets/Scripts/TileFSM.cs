using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TileFSM : MonoBehaviour
{
    public GameObject gm;
    public FSMSystem fsm;
    public Vector2 goalPos;
    public Vector2 wrapPos;
    public Vector2 wrapGoalPos;
    public bool offGrid;
    public GameObject boundary;
    public void SetTransition(Transition t) { fsm.PerformTransition(t); }

    public void Start()
    {
        goalPos = transform.position;
        offGrid = false;
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
        MoveState moving = new MoveState(this);
        moving.AddTransition(Transition.ReachedGoal, StateID.Idle);
        moving.AddTransition(Transition.OffGrid, StateID.Wrapping);

        IdleState idle = new IdleState(this);
        idle.AddTransition(Transition.UserSwiped, StateID.Moving);

        WrapState wrap = new WrapState(this);
        wrap.AddTransition(Transition.FinishedWrap, StateID.Idle);

        fsm = new FSMSystem();
        fsm.AddState(idle);
        fsm.AddState(moving);
    }
}

public class MoveState : FSMState
{
    public TileFSM controlref;
    private float speed = .05f;

    public MoveState(TileFSM control)
    {
        stateID = StateID.Moving;
        controlref = control;
    }

    public override void Reason(GameObject gm, GameObject npc)
    {
        if (npc.transform.position.x == controlref.goalPos.x && npc.transform.position.y == controlref.goalPos.y)
        {
            if (controlref.offGrid)
            {
                //do before leaving
                controlref.offGrid = false;
                npc.transform.position = controlref.wrapPos;
                controlref.goalPos = controlref.wrapGoalPos;

                npc.GetComponent<TileFSM>().SetTransition(Transition.OffGrid);
            }
            else
            {
                npc.GetComponent<TileFSM>().SetTransition(Transition.ReachedGoal);
            }
        }
    }

    public override void Act(GameObject gm, GameObject npc)
    {
        npc.transform.position = Vector2.MoveTowards(npc.transform.position, controlref.goalPos, speed);
    }

} //MoveState

public class IdleState : FSMState
{
    TileFSM controlref;

    public IdleState(TileFSM control)
    {
        stateID = StateID.Idle;
        controlref = control;
    }

    public override void Reason(GameObject gm, GameObject npc)
    {
        if(npc.transform.position.x != controlref.goalPos.x || npc.transform.position.y != controlref.goalPos.y)
        {
            npc.GetComponent<TileFSM>().SetTransition(Transition.UserSwiped);
        }
        
    }

    public override void Act(GameObject gm, GameObject npc)
    {
    }

} // IdleState

public class WrapState : FSMState
{
    TileFSM controlref;
    private float spd = .08f;

    public WrapState(TileFSM control)
    {
        stateID = StateID.Wrapping;
        controlref = control;
    }

    public override void Reason(GameObject gm, GameObject npc)
    {
        //magic number hack for tile scale
        if (npc.transform.position.x == controlref.goalPos.x && npc.transform.position.y == controlref.goalPos.y)
        {
            npc.GetComponent<TileFSM>().SetTransition(Transition.FinishedWrap);
        }
    }

    public override void Act(GameObject gm, GameObject npc)
    {
        npc.transform.position = Vector2.MoveTowards(npc.transform.position, controlref.goalPos, spd);
    }

} // ChasePlayerState