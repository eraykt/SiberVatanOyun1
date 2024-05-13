using UnityEngine;

public class CarController : MonoBehaviour
{
   private float moveSpeed = 15f; // arabanin hareket hizi

   private void Update()
   {
      transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); // arabayi ileriye dogru hareket ettir.
   }
}
