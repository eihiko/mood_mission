using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwarmAgent : MonoBehaviour
{

    public Swarm swarm;
    public Vector3 txposition;
    public Quaternion txrotation;

    void Start ()
    {
        if (swarm == null) {
            Debug.Log ("Swarm Agent must be created by a Swarm component.");
        } else {
            tx = transform;
			txposition = tx.position;
			txrotation = tx.rotation;
            separation2 = swarm.separation * swarm.separation;
            neighborhoodRadius2 = swarm.neighborhoodRadius * swarm.neighborhoodRadius;
            swarmRadius2 = swarm.swarmRadius * swarm.swarmRadius;
        }

    }

    public void UpdateAgent ()
    {
        var deltaTime = swarm.deltaTime;
        CalculateVelocityInfluences (deltaTime);
        newVelocity = Vector3.zero;
        newVelocity += separation * swarm.separationWeight;
        newVelocity += alignment * swarm.alignmentWeight;
        newVelocity += cohesion * swarm.cohesionWeight;
        newVelocity += bounds * swarm.boundsWeight;
        newVelocity = newVelocity * swarm.speed;

        newVelocity = rigidbodyVelocity + newVelocity;

        velocity = rigidbodyVelocity = Vector3.ClampMagnitude (newVelocity, swarm.speed);
        position = txposition;

        deltaToFocus = (position - swarm.position);
        distanceFromFocus2 = deltaToFocus.sqrMagnitude;

        txposition = txposition + (rigidbodyVelocity * deltaTime);

        if(swarm.lookAtFocus) {
            if(rigidbodyVelocity.sqrMagnitude > 0) {
                txrotation = Quaternion.Slerp(txrotation, Quaternion.LookRotation(-deltaToFocus), deltaTime * swarm.lookAtDamping);
            }
        }
    }

    void CalculateVelocityInfluences (float deltaTime)
    {

        separation = Vector3.zero;
        alignment = Vector3.zero;
        cohesion = Vector3.zero;
        bounds = Vector3.zero;

        var sc = 0;
        var ac = 0;
        var cc = 0;
        var bc = 0;
        var agents = swarm.agents;
        for (var i=0; i<agents.Length; i++) {
        
            var agent = agents [i];
            //if (agent == null)
            //    continue;

            var delta = position - agent.position;
            var distance2 = delta.sqrMagnitude;

            if (this.distanceFromFocus2 > swarmRadius2) {
                bounds += swarm.position;
                bc++;
            }

            if (distance2 > 0 && distance2 < neighborhoodRadius2) {

                alignment += agent.velocity;
                ac++;
                cohesion += agent.position;
                cc++;

                if (distance2 < separation2) {
                    var distance = Mathf.Sqrt (distance2);
                    separation += (delta / distance / distance);
                    sc++;
                }
            }
        }

        if (sc > 0)
            separation /= sc;
        if (ac > 0)
            alignment = Vector3.ClampMagnitude (alignment / ac, swarm.maxSteer);

        if (cc > 0) 
            cohesion = Steer (cohesion / cc, true, swarm.neighborhoodRadius);
        if (bc > 0)
            bounds = Steer (bounds / bc);

    }

	

    Vector3 Steer (Vector3 target, bool slowDown=false, float slowDownRange=0)
    {

        var steer = Vector3.zero;
        var targetDirection = target - position;
        var targetDistance2 = targetDirection.sqrMagnitude;

        if (targetDistance2 > 0) {
            var targetDistance = Mathf.Sqrt (targetDistance2);
            targetDirection /= Mathf.Sqrt (targetDistance);

            if (slowDown && targetDistance < slowDownRange) {
                targetDirection *= (swarm.speed * (targetDistance / slowDownRange));
            } else {
                targetDirection *= swarm.speed;
            }

            steer = targetDirection - velocity;
            steer = Vector3.ClampMagnitude (steer, swarm.maxSteer);
        }

        return steer;
    }

    Transform tx;
    Vector3 position, velocity, newVelocity, separation, alignment, cohesion, bounds, deltaToFocus, rigidbodyVelocity;
    float separation2, neighborhoodRadius2, swarmRadius2, distanceFromFocus2;


}

