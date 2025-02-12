# Project gist
This project is a test to see if I can adapt and learn Unity's ECS framework by application.

## Features
- Car spawners
  - Randomly affected colours on startup
  - 2+ segment routes to be created via the scene editor
  - By default, will spawn a car on startup. Later will be affected a shortcut to spawn multiple cars
- Cars
  - Inherit the colour/route from their spawner
  - Follow the given route. Once the route is completed, destroy the car 

# How it works
## Components
- **ColourSet** : authoring component to add a buffer of available colours to an entity
- **Markers** :
  - _SpawnerMarker_ : Distinguishes spawner from entities with common components (namely, the entity they spawn)
  - _DestroyOnPathDone_ : Once it arrives at the end of its current path, the marked entity will be destroyed by a cleanup system
### Movement
- **CurrentMovementGoal** : Gives the entity float2 goal coordinates (x,z) to go towards. Starts disabled and has to be enabled by the system initiating the entity's first goal
- **CurrentPathStep** : If an entity goes along a segmented path, this component saves at what step of the path we are to allow finding the next goal
- **FollowPathParameters** : Currently only sets the entity movement speed, but can be extended to max turn angle, acceleration etc
- **PathGameObject** : Uses a game object array to bake a dynamic array of path steps into a dynamic buffer. We can use any game object, even empty as path step. It"s just easier to place around from the scene than bare coordinates
### Spawn Entity
- **EntityPrefab** : Container for an Entity Prefab that can be used as a model or to get data from (used by spawners currently)
- **SpawnerReference** : Reference to the entity that spawned the EntityPrefab. Currently used as a colour set source and a path source as we wish to inherit both from the spawner, but if need be could be split with a PathReference and ColourReference (that could even be the Entity itself if it's its own source)
- **SpawnRate** : Gives an editable spawn rate and holds the current updated timeout value to be updated by timer related systems


## Entities/Prefabs
Two prefab entities are given in this test project : 
- **SpawnerWithPathPrefab** : A spawner with a colour, a prefab entity to have spawn, and a path that spawned entity will have to follow
- **CarEntityPrefab** : A coloured car with pathing parameters, allowing it to parametrically follow its given path

## Systems
- **DestroyPathCompleteEntities** : If an entity has a DestroyOnPathDoneMarker and has achieved the last goal in its current path, this system will destroy it
- **EnableCurrentGoalIfNeeded** : If an entity has its current goal component disabled (no current goal) and has a new step available in its current path (added a new step or a whole new path) then enable its goal component and give it a new step
- **MoveToGoalSystem** : Moves the entity towards its current goal and snaps it to goal if it's close enough
- **SpawnEntitySystem** : Spawns entities from the _EntityPrefabComponent_ for each _SpawnerMarker_ entities if the spawner's spawn timeout is at 0
- **UpdateNextStepSystem** : If an entity is at its current goal, set its current step and next goal according to its path
- **UpdateTimers** : Updates entities' timeout components by decrementing their timeout values according to delta time (snaps to 0 if needed)
