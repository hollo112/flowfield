using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        BFSManager.Instance.BuildDistanceField(transform.position);
    }
}
