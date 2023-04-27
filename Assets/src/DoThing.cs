using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoThing : MonoBehaviour
{
    public bool Idle => ThingsImDoing.Count == 0;

    Interactor interactor;
    Movement movement;
    Pathfinder pathfinder;
    Tilemap walls;
    LayerMask wallMask;

    List<ThingToDo> ThingsImDoing = new List<ThingToDo>();
    List<Vector3Int> Path;
    int currentNode;

    void Start()
    {
        interactor = GetComponent<Interactor>();
        movement = GetComponent<Movement>();
        pathfinder = GetComponent<Pathfinder>();
        pathfinder.NavMesh = FindObjectOfType<GridNavMesh>();
        walls = GridManager.GetLayer(TileLayer.wall);
        wallMask = LayerMask.GetMask(new string[] { "Walls" });
    }

    public void DoThingImmediately(ThingToDo thing)
    {
        if (!Idle && ThingsImDoing[0].what == Things.waitfortask)
            interactor.Cancel();
        ThingsImDoing.Clear();
        ThingsImDoing.Add(thing);
        Path = null;
    }

    public void QueueThingToDo(ThingToDo thing)
    {
        ThingsImDoing.Add(thing);
    }

    private void FixedUpdate()
    {
        if (ThingsImDoing.Count > 0)
        {
            var thing = ThingsImDoing[0];
            switch (thing.what)
            {
                case Things.walkhere:
                    // if distance is futher than 1.5 and we have no clear path, use pathfinder.
                    var delta = thing.where - (Vector2)transform.position;
                    if (delta.sqrMagnitude < thing.distance * thing.distance)
                        ThingsImDoing.RemoveAt(0);
                    else if (delta.sqrMagnitude > 2.25f && GotClearPath(thing.where) == false)
                        PathTo(thing.where);
                    else
                        movement.Direction = delta * 2f;
                    break;
                case Things.pathhere:
                    if (Path == null)
                    {
                        Path = FindPath(thing.where);
                        currentNode = 0;
                        if(Path == null)
                            ThingsImDoing.RemoveAt(0);
                        else
                            Debug.Log($"Made path with {Path.Count} nodes");
                        break;
                    }
                    // see if we can skip a node due to having a clear path to the next
                    if (currentNode+1 < Path.Count && GotClearPath(Path[currentNode + 1]))
                        currentNode++;
                    // if we reach the current last node, step forward in the node list
                    if (walls.WorldToCell(transform.position) == Path[currentNode])
                        currentNode++;
                    // if we reach the last node, end the task and reset the pathfinder.
                    if(currentNode >= Path.Count)
                    {
                        Path = null;
                        ThingsImDoing.RemoveAt(0);
                    }
                    // go to the next node in the path
                    delta = (Vector2)walls.CellToWorld(Path[currentNode]) + new Vector2(.5f, .5f) - (Vector2)transform.position;

                    movement.Direction = delta * 2f;
                    break;
                case Things.interact:
                    var interactable = thing.who.GetComponent<IInteractable>();
                    var distance = interactable.MinimumDistance;
                    var inRange = (interactable.GetInteractLocation(thing.where) - (Vector2)transform.position).sqrMagnitude <= distance * distance;
                    if (inRange)
                    {
                        movement.Direction = Vector2.zero;
                        GetComponent<Interactor>().InteractWith(interactable, thing.where);
                        ThingsImDoing.RemoveAt(0);
                        ThingsImDoing.Add(new ThingToDo() { what = Things.waitfortask });
                    }
                    else
                        MoveCloser(interactable.GetInteractLocation(thing.where));
                    break;
                case Things.waitfortask:
                    if (interactor.Busy == false)
                        ThingsImDoing.RemoveAt(0);
                    break;
            }
        }
        else
        {
            movement.Direction = Vector2.zero;
        }
    }

    void MoveCloser(Vector2 where)
    {
        ThingsImDoing.Insert(0, new ThingToDo()
        {
            what = Things.walkhere,
            where = where,
            distance = .8f,
        });
    }

    [System.Serializable]
    public class ThingToDo
    {
        public Things what;
        public Vector2 where;
        public GameObject who;
        public float distance;
    }

    public enum Things
    {
        walkhere = 0,
        interact = 1,
        waitfortask = 2,
        pathhere = 3
    }

    void PathTo(Vector2 where)
    {
        ThingsImDoing.Insert(0, new ThingToDo()
        {
            what = Things.pathhere,
            where = where,
        });
    }

    private List<Vector3Int> FindPath(Vector2 to)
    {
        // is the place reachable? Do we have to go to a nearby spot?
        Vector3Int v3i = walls.WorldToCell(to);
        var tile = walls.GetTile(v3i);
        if (tile != null)
        {
            var adjacent = GetAdjacentOpenTile(to);
            if(adjacent == to)
                return new List<Vector3Int>();

            to = adjacent;
            v3i = walls.WorldToCell(to);
            
        }
        // Can we cut straight to where we wish to go?
        if(GotClearPath(to))
            return new List<Vector3Int>(new Vector3Int[]{ walls.WorldToCell(to) });
        return pathfinder.FindPath(walls.WorldToCell(transform.position), v3i);
    }

    private Vector2 GetAdjacentOpenTile(Vector2 to)
    {
        Vector3Int v3i = walls.WorldToCell(to);
        var list = new List<Vector3Int>();
        if (walls.GetTile(v3i + Vector3Int.up) == null)
            list.Add(v3i + Vector3Int.up);
        if (walls.GetTile(v3i + Vector3Int.right) == null)
            list.Add(v3i + Vector3Int.right);
        if (walls.GetTile(v3i + Vector3Int.down) == null)
            list.Add(v3i + Vector3Int.down);
        if (walls.GetTile(v3i + Vector3Int.left) == null)
            list.Add(v3i + Vector3Int.left);
        if (list.Count == 0)
        {
            Debug.Log("No nearby open tiles");
        }
        else
        {
            v3i = list[0];
            System.Func<Vector3Int, Vector3Int, bool> isCloser = (Vector3Int a, Vector3Int b) => (transform.position - walls.CellToWorld(a)).sqrMagnitude < (transform.position - walls.CellToWorld(b)).sqrMagnitude;
            for (int i = 0; i < list.Count; i++)
                if (isCloser(list[i], v3i))
                    v3i = list[i];
            to = walls.CellToWorld(v3i);
        }
        return to;
    }


    bool GotClearPath(Vector2 to)
    {
        // var hit = Physics2D.Linecast(transform.position, to, wallMask);
        var hit = Physics2D.CircleCast(origin: transform.position, radius: .25f, direction: to - (Vector2)transform.position, layerMask: wallMask, distance: (to - (Vector2)transform.position).magnitude);
        return hit.collider == null;
    }
    bool GotClearPath(Vector3Int to) => GotClearPath(walls.CellToWorld(to));
}


