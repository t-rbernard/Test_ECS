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
- **ColourComponent** : simply contains a Color type attribute to be used to colour the material appropriately
- **EntityPrefab** : Container for an Entity Prefab that can be used as a model or to get data from (used by spawners currently)
- **FollowPathParameters** : Currently only contains the speed of the entity while following a path. Currently it's attached directly to the prefab entity that will spawn, but the spawner could instantiate this component if we want different movement speeds for a same prefab
- **PathGameObject** : Container game object, consisting of a list of float2 (x,z) checkpoints for the entity to check. This component is serializable and allows to add (x,z) checkpoints from the scene as required (TODO: update for an easier to edit representation)
- **Markers** :
  - _SpawnerMarker_ : Distinguishes spawner from entities with common components (namely, the entity they spawn)
  - _ShouldSpawnOnceMarker_ : Marks an entity as having to spawn only one entity (either itself, or an entity prefab). It is then disabled
  - _ShouldRandomizeColourOnceMarker_ :  Marks an entity as having its colour randomized once, then disable this marker

## Entities/Prefabs
Two prefab entities are given in this test project : 
- **SpawnerWithPathPrefab** : A spawner with a colour, a prefab entity to have spawn, and a path that spawned entity will have to follow
- **CarEntityPrefab** : A coloured car with pathing parameters, allowing it to parametrically follow its given path

## Systems
- **RandomizeColourSystem** : If necessary, randomizes an entity's ColourComponent with a pseudorandom colour
- **UpdateColourSystem** : iterates over components with a MaterialBaseColor and a ColourComponent, updating the material colour in accordance with the colour component if necessary
- **SpawnEntitySystem** : Spawns a given EntityPrefab on all spawners with the appropriate components, having the entity inherit the spawner's route and colour. Currently only spawns one car each, but could be expanded to spawn on key press or at intervals for example.
- **FollowPathAndDestroySystem** : Moves a given entity according to its given path and path parameters components, cleaning/destroying the entity once it gets to its last checkpoint in the route.
