using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour {

    // maximal distance the player can have to the object in order to use it
    public float interactableDistance = 0.5f;

    // current agent (usually, this is the player) interacting with the object
    protected NavMeshAgent currentAgent;

    // is set to true in order to avoid multiple interactions with one click
    protected bool bHasInteracted = false;

    // to store time stemps
    protected float temp;

    public virtual void MoveToInteractable(NavMeshAgent agent)
    {
        // agent wants to interact with this object
        currentAgent = agent;

        // move agent to this object
        agent.stoppingDistance = interactableDistance;
        agent.destination = this.transform.position;

        // agent has not arrived yet, interaction was not performed yet
        bHasInteracted = false;
    }

    public virtual void Update()
    {
        // Interact with the object provided we haven´t already interacted, we are close enough and the path has been calculated successfully
        if (!bHasInteracted && currentAgent != null && !currentAgent.pathPending)
        {
            if (currentAgent.remainingDistance <= interactableDistance)
            {
                temp = Time.time;
                bHasInteracted = true;
                Interact();
            }
        }
  
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with Interactable.");
    }

    public virtual void ResetAgent()
    {
        // this object is not occupied by the previous agent any more and can be used again later
        currentAgent = null;
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, interactableDistance);
    //}
}
