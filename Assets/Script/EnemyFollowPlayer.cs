using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float Speed = 2f;
    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Node node = FlowFieldManager.Instance.NodeFromWorld(transform.position);
        
        if (node == null) return;
        
        Vector3 direction = node.Direction;
        
        transform.position += direction * Speed * Time.deltaTime;
        Debug.Log(_rigidBody.linearVelocity);
        
    }
}