using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieController : MonoBehaviour
{

    public Transform target;

    [SerializeField]
    float updateSpeed = 0.1f;

    private NavMeshAgent zombie;

    private void Awake()
    {
        zombie = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(updateSpeed);
        while (enabled) 
        {
            zombie.SetDestination(target.transform.position);
            yield return Wait;
        }
    }

    public void moveZombie(Vector3 position)
    {
        zombie.Warp(position);
    }

}
