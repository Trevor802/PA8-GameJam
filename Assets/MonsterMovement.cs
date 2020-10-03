using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    public Transform chasedGoal;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = chasedGoal.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collided fuck");
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white);
        // }
        
    }

    private void PlayerCaught()
    {
        Debug.Log("Player caught and Game over");
        // TODO: call the function in the game manager for fail state
    }


}
