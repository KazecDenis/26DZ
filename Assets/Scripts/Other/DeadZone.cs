using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character player = collision.GetComponent<Character>();

        if (player != null)
            player.DieFromFall();
    }
}
