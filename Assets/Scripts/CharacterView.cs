using UnityEngine;

public class CharacterView : MonoBehaviour
{
   private readonly int KeyGrounded = Animator.StringToHash("IsGrounded");
   private readonly int KeyVelocityX = Animator.StringToHash("VelocityX");
   private readonly int KeyVelocityY = Animator.StringToHash("VelocityY");

   [SerializeField] private Character _character;
   [SerializeField] private Animator _animator;

    private void Update()
    {
        _animator.SetFloat(KeyVelocityX, Mathf.Abs(_character.Velocity.x));
        _animator.SetFloat(KeyVelocityY, _character.Velocity.y);
        _animator.SetBool(KeyGrounded, _character.IsGrounded);
    }
}
