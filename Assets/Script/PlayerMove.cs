using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        FlowFieldManager.Instance.BuildIntegrationField(transform.position);
        FlowFieldManager.Instance.BuildFlowField();
    }
}
