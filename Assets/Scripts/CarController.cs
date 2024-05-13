using UnityEngine;

public class CarController : MonoBehaviour
{
   private float moveSpeed = 10f; // arabanin hareket hizi

   private void Update()
   {
      transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // arabayi ileriye dogru hareket ettir.
   }

   private void OnCollisionEnter(Collision other)
   {
      if(other.gameObject.tag != "Player") return; // carpisma player ile mi yapildi?

      var player = other.gameObject.GetComponent<PlayerController>(); // player controllera eris
      
      player.Ragdoll(true); // ragdollu ac
      player.DropGoldsFromHand();
   }
}
