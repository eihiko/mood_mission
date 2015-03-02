using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UniExtensions.Async;

public class Swarm : MonoBehaviour
{

    public int agentCount = 50;
    public float spawnRadius = 100f;
    public float swarmRadius = 300;
    public float speed = 1f;
    public float maxSteer = .01f;
    public float neighborhoodRadius = 30f;
    public float separation = 10f;
    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float boundsWeight = 1f;
    public Transform swarmFocus;
    public bool lookAtFocus = true;
    public float lookAtDamping = 0.5f;
    public SwarmAgent prefab;
    public SwarmAgent[] agents;
    public bool multithreaded = false;
    public int numThreads = 4;
    public bool splitTasksAcrossFrames = true;

    [HideInInspector]
    public float deltaTime;

    [HideInInspector]
    public Vector3 position;

    UniExtensions.Async.MapReduce mr;

    void Start ()
    {
        Application.runInBackground = true;
        if (swarmRadius <= neighborhoodRadius) {
            Debug.LogError ("Neighbour Radius must be less than Swarm Radius");
            return;
        }

        var agents = new List<SwarmAgent> ();
        for (int i = 0; i < agentCount; i++) {
            var agent = GameObject.Instantiate (prefab) as SwarmAgent;
            agent.swarm = this;
            agent.transform.parent = transform;
            agent.transform.localPosition = Random.onUnitSphere * spawnRadius;
            agents.Add (agent);
        }
        this.agents = agents.ToArray ();
        if (multithreaded) {
            mr = new UniExtensions.Async.MapReduce (numThreads, agentCount);
            StartCoroutine (UpdateSwarmThreaded ());
        } else {
            StartCoroutine (UpdateSwarm ());
        }
    }
    
    IEnumerator UpdateSwarmThreaded ()
    {
        var wfeof = new WaitForEndOfFrame ();
        while (true) {
            yield return wfeof;
            position = swarmFocus.position;
            this.deltaTime = Time.deltaTime;
            for (var i=0; i<agents.Length; i++) {
                mr.Enqueue (agents [i].UpdateAgent);
            }
            mr.Execute ();
            if(splitTasksAcrossFrames) {
                while (!mr.Done) {
                    yield return wfeof;
                }
            } else {
                while (!mr.Done);
            }
            for (var i=0; i<agents.Length; i++) {
                var a = agents [i];
                a.transform.position = a.txposition;
                a.transform.rotation = a.txrotation;
            }
        }

    }

    IEnumerator UpdateSwarm ()
    {
        var wfeof = new WaitForEndOfFrame ();
        while (true) {
            yield return wfeof;
            position = swarmFocus.position;
            this.deltaTime = Time.deltaTime;
            for (var i=0; i<agents.Length; i++) {
                var a = agents [i];
                a.UpdateAgent ();
                a.transform.position = a.txposition;
                a.transform.rotation = a.txrotation;
            }
        }
    }


}
