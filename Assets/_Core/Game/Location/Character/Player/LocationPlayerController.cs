using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocationPlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundMask;


    private void Start()
    {
        agent.speed = speed;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 1000, groundMask))
            {
                agent.SetDestination(hit.point);
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

}
