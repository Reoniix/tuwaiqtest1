using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour
{
    private NavMeshAgent Agent;
    private PlayerControl playerControl;

    private NavMeshPath path;
    public float updateRate = 0.2f;
    private float timer;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        playerControl = FindFirstObjectByType<PlayerControl>();

        path = new NavMeshPath();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateRate)
        {
            timer = 0f;
            UpdatePath();
        }
    }

    void UpdatePath()
    {
        if (playerControl == null)
        {
            return;

        }

        Vector3 targetPos = playerControl.transform.position;

        // calculate path from enemy to player
        if (NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete ||
                path.status == NavMeshPathStatus.PathPartial)
            {
                Agent.SetPath(path);
            }
        }
    }
}
