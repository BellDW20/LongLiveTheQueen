using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class KnightSwordScript : MonoBehaviour {

    private float DAMAGE = 25f;
    [SerializeField] private GameObject BLOOD_PARTICLES;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            PlayerController pc = collision.collider.gameObject.GetComponent<PlayerController>();
            //As long as the player hasn't died, try to damage them
            //NOTE: this does not hurt the player each frame, as the player Damage() function
            //      takes into account whether or not the player is invulnerable (has been damaged recently)
            if (!pc.IsDead()) {
                if (pc.Damage(DAMAGE)) {
                    Instantiate(BLOOD_PARTICLES, transform.position, Quaternion.identity);
                }
            }
        }
    }

}
