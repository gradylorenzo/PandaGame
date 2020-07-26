using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PandaAnimation : MonoBehaviour
{
    public PandaScript ps;
    public Animator anim;
    public NavMeshAgent agent;
    public int sleepChance = 1;
    public int sitChance = 1;
    public int idleChance = 1;
    public int moveChance = 1;
    public bool isWalking = false;

    [Serializable]
    public enum animState
    {
        idle,
        sitting,
        eating,
        sleeping,
        drinking
    }
    public animState state;

    private void FixedUpdate()
    {
        AnimationUpdate();
    }

    private void AnimationUpdate()
    {
        isWalking = agent.remainingDistance >= 0.05f;
        anim.SetBool("walking", isWalking);

        if (!isWalking)
        {
            switch (state)
            {
                case animState.idle:
                    anim.SetBool("sitting", false);
                    anim.SetBool("eating", false);
                    anim.SetBool("sleeping", false);
                    anim.SetBool("drinking", false);
                    break;
                case animState.sitting:
                    anim.SetBool("sitting", true);
                    anim.SetBool("eating", false);
                    anim.SetBool("sleeping", false);
                    anim.SetBool("drinking", false);
                    break;
                case animState.eating:
                    anim.SetBool("sitting", true);
                    anim.SetBool("eating", true);
                    anim.SetBool("sleeping", false);
                    anim.SetBool("drinking", false);
                    ps.eat();
                    break;
                case animState.sleeping:
                    anim.SetBool("sitting", false);
                    anim.SetBool("eating", false);
                    anim.SetBool("sleeping", true);
                    anim.SetBool("drinking", false);
                    break;
                case animState.drinking:
                    anim.SetBool("sitting", false);
                    anim.SetBool("eating", false);
                    anim.SetBool("sleeping", false);
                    anim.SetBool("drinking", true);
                    ps.drink();
                    break;
            }
        }
        else
        {
            anim.SetBool("sitting", false);
            anim.SetBool("eating", false);
            anim.SetBool("sleeping", false);
            anim.SetBool("drinking", false);
        }

        if(state == animState.idle)
        {
            if(UnityEngine.Random.Range(0, 10000) < sleepChance)
            {
                state = animState.sleeping;
            }
            else if(UnityEngine.Random.Range(0, 10000) < sitChance)
            {
                state = animState.sitting;
            }
        }
        else if(state == animState.sleeping || state == animState.sitting)
        {
            if(UnityEngine.Random.Range(0, 1000) < idleChance)
            {
                state = animState.idle;
            }
        }

        if (UnityEngine.Random.Range(0, 1000) < moveChance)
        {
            Vector3 newPos = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            agent.SetDestination(newPos);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("pond") && state != animState.drinking)
        {
            if(ps.thirst < 750)
            {
                state = animState.drinking;
            }
        }

        else if (other.gameObject.CompareTag("bamboo") && state != animState.eating)
        {
            if(ps.hunger < 750)
            {
                state = animState.eating;
            }
        }

        if(ps.thirst >= 1000 || ps.hunger >= 1000)
        {
            state = animState.idle;
        }
    }
}
