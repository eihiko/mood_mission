The Swarm Agent package includes the UniExtensions dll, source is available
here: http://u3d.as/content/different-methods/uni-extender/2zx

SwarmAgent is an implementation of the Boids flocking algorithm.

To use, create a swarm agent prefab, and assign the SwarmAgent component
to this prefab.

Create a new component which will contain the swarm, and assign the Swarm
component. This component has several parameters you will need to adjust.

Agent Count: the number of prefabs to instantiate which become the swarm

Spawn Radius: the sphere size in which all agents will be instantiated.

Swarm Radius: the sphere size in which the agents will stay inside.

Speed: the overall speed of the swarm movement.

Max Steer: the max turning angle per frame.

Neighboorhood Radius: The space around an agent which it wil use to determin
who is a neighbour.

Separation: The amount of distance an agent will try to keep from other
agents.

Separation Weight: A measure of the importance of separation between agents.

Alignment Weight: A measure of the importance of alignment between agents.

Cohesion Weight: A measure of the importance of cohesion between agents.

Bounds Weight: A measure of the importance of staying in bounds.

Swarm Focus: This is a transform which the swarm will move towards and
flock around. It can be moved at runtime.

Prefab: This is the SwarmAgent prefab which will be instantiated to create
the swarm.

Multithreaded: If checked, multiple threads will be used to calculate swarm positions.

Num Threads: If multithreaded, the number of threads to use.

Split Tasks Across Frame: If multithreaded, workload will be split across multiple frames if it cannot be completed within one frame.

