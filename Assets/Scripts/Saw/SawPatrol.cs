using System.Collections;
using UnityEngine;

public class SawPatrol : MonoBehaviour
{
    [SerializeField] private Transform _targetA;
    [SerializeField] private Transform _targetB;
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private float _duration = 2f;

    private void Start()
    {
        StartCoroutine(PatrolProcess());
        _targetA.SetParent(null);
        _targetB.SetParent(null);
    }
    private IEnumerator PatrolProcess()
    {
        while (true)
        {
            yield return MoveTo(_targetA);

            yield return new WaitForSeconds(_waitTime);

            yield return MoveTo(_targetB);

            yield return new WaitForSeconds(_waitTime);
        }
    }

    private IEnumerator MoveTo(Transform target)
    {
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = target.position;

        float elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / _duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector2.Lerp(startPosition, targetPosition, smoothT);
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character player = collision.GetComponent<Character>();

        if (player != null)
            player.Die();
    }
}
        
            
        



