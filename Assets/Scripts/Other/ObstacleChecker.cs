using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _distanceToCheck;

    public bool IsTouches()
    {
      RaycastHit2D hit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.bounds.size, _collider.direction, 0, _direction.normalized, _distanceToCheck, _layerMask); 
      return hit.collider != null;
        
    }
}
