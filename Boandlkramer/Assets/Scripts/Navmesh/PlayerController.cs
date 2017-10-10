using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    // reference to the NavAgent component
    UnityEngine.AI.NavMeshAgent agent;

    // Time in which the MouseButton accepts interactables
    float interactableTime = 0.01f;

    // Particle effect at the postion the player clicked on, for testing purposes
    public ParticleSystem movementParticle;

    // Interactable the player wants to interact with
    private Interactable currentTarget = null;

    // for storing time stamps
    private float temp;

    // if set to true, holding mouse button down wont stop interaction mode
    private bool bIsMovingToInteraction = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		movementParticle.GetComponent<ParticleSystem> ().Pause();
    }

    // place a particle effect where the player clicked, for testing purposes
    private void PlayMovementParticle(Vector3 pos)
    {
        movementParticle.transform.position = pos;
        movementParticle.GetComponent<ParticleSystem>().Play();        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            temp = Time.time;
        }

        // keeping left click to walk
        if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            // just began to click - interaction with interactables is possible
            if (Time.time - temp <= interactableTime)
            {
                GetInteraction();
            }
            // the player is holding the mouse button down for a longer time - just move, provided the player is not trying to move to an interactable
            else
            {
                if (!bIsMovingToInteraction)
                {
                    // we have no target anymore
                    ResetTarget();
                    MovePlayer();
                }
            }
        }

        
        if (Input.GetMouseButtonUp(0))
		{
            // if the mouse button is released, the player can give movement commands in order to abort interacting
            bIsMovingToInteraction = false;

            // if the current target is an enemy, the next time we click on it, it will be a first attack again
            if (currentTarget != null && currentTarget.GetComponent<Enemy>() != null)
            {
                currentTarget.GetComponent<Enemy>().ResetFirstAttack();
            }

            // for testing purposes
            movementParticle.transform.position = new Vector3 (0, 0, 1000);
            movementParticle.GetComponent<ParticleSystem>().Simulate(0, true, true);
        }
    }

    void GetInteraction()
    {
        // use a raycast to determine the position in the 3d world
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            GameObject obj = hit.collider.gameObject;

            // if the object is an interactable object, we will interact
            if (obj.tag == "interactable")
            {
                // an interaction has started, only mouse button up can stop this
                bIsMovingToInteraction = true;

                // we found a new target
                SetTarget(obj.GetComponent<Interactable>());

                // move to the interactable (if necessary)
                obj.GetComponent<Interactable>().MoveToInteractable(agent);
            }
            else
            {
                if (!bIsMovingToInteraction)
                {
                    // no target anymore
                    ResetTarget();

                    // Move Player to the postion the player clicked on
                    MovePlayer(hit.point);
                }                
            }

        }
    }

    // move the player to a given postion
    void MovePlayer(Vector3 target)
    {
        agent.stoppingDistance = 0.0f;
        agent.destination = target;
        PlayMovementParticle(target);
    }

    // perform a raycast and move the player
    void MovePlayer()
    {
        // use a raycast to determine the position in the 3d world
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            MovePlayer(hit.point);
        }
    }

    public void ResetTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.ResetAgent();
            currentTarget = null;
        }

    }

   public void SetTarget(Interactable target)
    {
        currentTarget = target;
    }

    public Interactable GetTarget()
    {
        return currentTarget;
    }

    public bool IsMovingToInteractable()
    {
        return bIsMovingToInteraction;
    }
}
