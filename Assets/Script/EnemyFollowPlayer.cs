//using UnityEngine;

////public class EnemyFollowPlayer : MonoBehaviour
////{
////    public float Speed = 2f;
////    private Rigidbody2D _rigidBody;

////    private void Start()
////    {
////        _rigidBody = GetComponent<Rigidbody2D>();
////    }
////    void Update()
////    {
////        Node node = FlowFieldManager.Instance.NodeFromWorld(transform.position);
        
////        if (node == null) return;
        
////        Vector3 direction = node.Direction;
        
////        transform.position += direction * Speed * Time.deltaTime;
////        Debug.Log(_rigidBody.linearVelocity);
        
////    }
////}

//public class EnemyFollowPlayer : MonoBehaviour
//{
//    public float Speed = 2f;
//    public float HitCheckDist = 0.25f;
//    public LayerMask WallMask;

//    Rigidbody2D rb;

//    // 디버그용 레이 방향 저장
//    Vector2 lastRayDir;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//    }

//    void FixedUpdate()
//    {

//        Node node = FlowFieldManager.Instance.NodeFromWorld(transform.position);

//        if (node == null) return;

//        Vector3 direction = node.Direction;

//        //Vector2 flowDir = FlowFieldManager.Instance.GetFlowDirection(transform.position);
//        //if (flowDir.sqrMagnitude < 0.01f)
//        //    return;

//        Vector2 finalDir = direction;

//        // 저장: 디버그용
//        lastRayDir = direction.normalized;

//        // 1️⃣ 전방 충돌 체크
//        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, HitCheckDist, WallMask);

//        if (hit)
//        {
//            // 벽의 노멀 기반으로 슬라이드 방향 계산
//            Vector2 wallNormal = hit.normal;
//            Vector2 slideDir = Vector2.Perpendicular(wallNormal).normalized;

//            // FlowField와 가장 비슷한 방향 선택
//            if (Vector2.Dot(slideDir, direction) < 0)
//                slideDir = -slideDir;

//            finalDir = slideDir;
//        }

//        // 2️⃣ 물리 이동
//        rb.MovePosition(rb.position + finalDir * Speed * Time.fixedDeltaTime);
//    }


//    // ---------------------------------------------------------
//    // ⭐⭐⭐ Scene Gizmo로 Ray 시각화 ⭐⭐⭐
//    // ---------------------------------------------------------
//    void OnDrawGizmos()
//    {
//        // 적의 좌표가 없으면 패스
//        if (rb == null) return;

//        // Ray 색상: 파란색
//        Gizmos.color = Color.cyan;

//        // 레이 시작점
//        Vector3 start = transform.position;

//        // 레이 방향 * 거리
//        Vector3 end = start + (Vector3)lastRayDir * HitCheckDist;

//        // 선 그리기
//        Gizmos.DrawLine(start, end);

//        // 끝 점 표시
//        Gizmos.DrawSphere(end, 0.05f);
//    }
//}