using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AgentController : Agent
{
    [SerializeField]
    float speed = 2f;

    Rigidbody rigidbody;

    [SerializeField]
    GunController gunObject;

    bool canShoot, hitTarget, hasShot = false;

    int timeUntilNextBullet = 0;

    int minTimeUntilNextBullet = 30;

    [SerializeField]
    WorldBehaviors worldBehaviors;

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        worldBehaviors.spawnAgent();
        worldBehaviors.callSpawnZombie();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        canShoot = false;

        float rotate = actions.ContinuousActions[0];
        float forward = actions.ContinuousActions[1];

        bool shoot = actions.DiscreteActions[0] > 0;

        rigidbody.MovePosition(transform.position + transform.forward * forward * speed * Time.deltaTime);
        transform.Rotate(0f, rotate * speed, 0f, Space.Self);

        if (shoot && !hasShot)
        {
            canShoot = true;
        }
        if (canShoot)
        {
            hitTarget = gunObject.shootGun();
            timeUntilNextBullet = minTimeUntilNextBullet;
            hasShot = true;
            if (hitTarget)
            {
                AddReward(1);
                if (worldBehaviors.getZombieCount() <= 0)
                {
                    EndEpisode();
                }
            } else
            {
                AddReward(-0.3f);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.5f);
            EndEpisode();
        } else if (collision.gameObject.tag == "Obstacle")
        {
            AddReward(-0.3f);
            EndEpisode();
        }
        else if (collision.gameObject.tag == "Zombie")
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    private void FixedUpdate()
    {
        if (hasShot)
        {
            timeUntilNextBullet--;
            if (timeUntilNextBullet <= 0)
            {
                hasShot = false;
            }
        }
    }
}
