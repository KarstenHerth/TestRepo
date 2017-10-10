using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : Interactable {

    // reference to current agent's character (usually, this is the player)
    private Character currentAgentCharacter;

    // This is used in order to make sure that the first attack by the player is performed, even if the mouse button is not clicked anymore
    private bool bFirstAttack = true;


    public override void MoveToInteractable(NavMeshAgent agent)
    {
        currentAgent = agent;
        currentAgentCharacter = currentAgent.GetComponent<Character>();
        agent.stoppingDistance = interactableDistance;
        agent.destination = this.transform.position;

        // we only reset bHasInteracted to false, if enough time is over in order to avoid that the player can attack faster than his attack speed allows
        if (bHasInteracted)
        {
            if (Time.time - temp > currentAgentCharacter.GetAttackSpeed ()) 
            {
                bHasInteracted = false;
                temp = Time.time;
            }
        }
    }

    public override void Update()
    {
        /* as Interactables, we want to Interact (Attack) when we are close to the enemy. But for enemies it is possible to interact several times, as long
         * as the player keeps the mouse button clicked */
        if (currentAgent != null && currentAgent.remainingDistance <= interactableDistance)
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.time - temp > currentAgentCharacter.GetAttackSpeed ())
                {
                    BaseUpdate();
                }
            }
            // even if the player is not clicking anymore - the first attack will be executed, provided the player hasn´t decided otherwise
            else if (bFirstAttack)
            {
                BaseUpdate();
            }
        }

        // possibility to interact again is resetted according to the player's attack speed
        if (currentAgent != null)
        {
            // We allow the player to attack again if enough time is over
            if (Time.time - temp > currentAgentCharacter.GetAttackSpeed ()) 
            {
                bHasInteracted = false;
                temp = Time.time;
            }
        }
    }

    public override void Interact()
    {
        // get player who is attacking
		Character attacker = currentAgent.GetComponent<Character> ();

        // store this enemy that is being attacked
		Character defender = GetComponent<Character> ();

        // let the player attack the enemy
		attacker.Attack (defender);
		Debug.Log("Attack!");
    }

    public override void ResetAgent()
    {
        base.ResetAgent();
        currentAgentCharacter = null;
    }

    public void ResetFirstAttack()
    {
        bFirstAttack = true;
    }

    void BaseUpdate()
    {
        // call the update procedure of the base class. In particular, this will cause an Interaction
        base.Update();

        // we have reached the enemy
        currentAgent.destination = currentAgent.transform.position;
        bFirstAttack = false;
    }
}
