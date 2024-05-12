using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f; // hareket hizi
    public float rotationSpeed = 10f; // donme hizi
    private Rigidbody rb; // fizik islemleri icin rigidbodyi tanimladik
    private Animator animator; // animasyon islemleri icin animatoru tanimladik.
    public List<GameObject> goldList; // karakterin elindeki altinlari tuttugumuz liste.
    public int carry; // karakterin anlik kac adet altin tasidigini tutucak olan field. 

    public int CarryLimit => goldList.Count; // tasima limitim

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // ayni gameobject uzerindeki rigidbody'e
        // ulasmak icin GetComponent fonksiyonunu kullandik

        animator = GetComponent<Animator>(); // animatore ulastik.
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // yatay eksende giris aldik
        var vertical = Input.GetAxis("Vertical"); // dikey eksende giris aldik

        // 2 boyutlu olan yatay ve dikey eksenindeki girisleri 3 boyutluya cevirdik
        // x ve y ekseni uzerindeki girisleri x ve z eksenine cevirdik.
        var movementDirection = new Vector3(horizontal, 0, vertical);

        // movement directionum 0 degilse isRunning animator parametresine true ata. eger 0 ise false ata.
        animator.SetBool("isRunning", movementDirection != Vector3.zero);
        // carry degerim 0 degilse isCarrying animator parametresine true ata.
        animator.SetBool("isCarrying", carry != 0);

        // yukaridakinin movement directionu ile degil de rigidbodydeki hizi kullanarak yaptigimiz sey. ikisi de ayni.
        // animator.SetBool("isRunning",rb.velocity != Vector3.zero);

        if (movementDirection == Vector3.zero) // input yoksa
        {
            rb.velocity = Vector3.zero; // karakterin hizini 0 yap.
            return; // fonksiyonun geri kalanini kontrol etmeye gerek yok.
        }

        // fiziksel olarak hizimizi yon eksenimiz ile hareket hizini carparak hareket ettirdik.
        rb.velocity = movementDirection * movementSpeed;

        // movement direction yonunu rotation olarak kaydet
        var rotationDirection = Quaternion.LookRotation(movementDirection);
        // karakterin rotation degerini kaydettigim rotation degerine smooth bir gecis saglar.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationDirection, rotationSpeed * Time.deltaTime);
    }

    public bool CollectGold() // table scriptinde cagirilacak olan altin toplama fonksiyonu.
    {
        if (carry == CarryLimit)
            return false; // eger tasidigim altin sayisi tasima limitime esit ise fonksiyon false deger return etsin.

        // 0 1 2 -> cary degerlerimi listenin indexi olarak kullanip o indexteki altinlari aktif yaptik.
        goldList[carry].gameObject.SetActive(true); 
        carry++; // carry degerini 1 arttiriyoruz.
        
        return true; // butun islem basarili bir sekilde gerceklestigi icin true return ediyoruz.
    }
}