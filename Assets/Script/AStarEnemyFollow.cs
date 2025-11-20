using UnityEngine;
using System.Collections.Generic;

public class AstarEnemyFollow : MonoBehaviour
{
    public float Speed = 2f;
    public Transform player;

    List<AStarNode> path;
    int currentIndex = 0;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        // 플레이어가 이동하면 재계산
        path = AStarPathfinder.Instance.FindPath(transform.position, player.position);
        currentIndex = 0;

        if (path == null || path.Count == 0)
            return;

        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (currentIndex >= path.Count)
            return;

        Vector3 target = path[currentIndex].WorldPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            Speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.05f)
            currentIndex++;
    }
}