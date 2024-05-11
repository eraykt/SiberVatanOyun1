using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f; // hareket hizi
    public float rotationSpeed = 10f; // donme hizi
    private Rigidbody rb; // fizik islemleri icin rigidbodyi tanimladik

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // ayni gameobject uzerindeki rigidbody'e
        // ulasmak icin GetComponent fonksiyonunu kullandik
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // yatay eksende giris aldik
        var vertical = Input.GetAxis("Vertical"); // dikey eksende giris aldik

        // 2 boyutlu olan yatay ve dikey eksenindeki girisleri 3 boyutluya cevirdik
        // x ve y ekseni uzerindeki girisleri x ve z eksenine cevirdik.
        var movementDirection = new Vector3(horizontal, 0, vertical);

        if (movementDirection == Vector3.zero)
        {
            Debug.Log("Su an input yok");
            return;
        }
        
        // fiziksel olarak hizimizi yon eksenimiz ile hareket hizini carparak hareket ettirdik.
        rb.velocity = movementDirection * movementSpeed;

        // movement direction yonunu rotation olarak kaydet
        var rotationDirection = Quaternion.LookRotation(movementDirection);
        // karakterin rotation degerini kaydettigim rotation degerine smooth bir gecis saglar.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationDirection, rotationSpeed * Time.deltaTime);
    }
}
