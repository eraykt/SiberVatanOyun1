using System.Collections;
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

    public float reduceSpeed = 0.5f; // altin tasidikca azalacak olan hiz miktarim.
    private float baseMovementSpeed; // oyun basladiginda karakterin sahip oldugu hareket hizi.
    
    public int CarryLimit => goldList.Count; // tasima limitim

    public Transform boneParent; // kemiklerin parenti
    public bool CanMove = true; // karakterim hareket edebilir mi?
    public Transform spinePosition;

    private void Start()
    {
        baseMovementSpeed = movementSpeed; // base hizin degerini aldik.
        rb = GetComponent<Rigidbody>(); // ayni gameobject uzerindeki rigidbody'e
        // ulasmak icin GetComponent fonksiyonunu kullandik

        animator = GetComponent<Animator>(); // animatore ulastik.
        
        Ragdoll(false); // oyun basladiginda ragdoll kapali olacak.
    }

    private void Update()
    {
        if (!CanMove) return;   // hareket boolum false ise hareket etme
        
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
        goldList[carry].SetActive(true);
        carry++; // carry degerini 1 arttiriyoruz.

        movementSpeed -= reduceSpeed; // hareket hizimi azalttim.
        
        return true; // butun islem basarili bir sekilde gerceklestigi icin true return ediyoruz.
    }

    public int DropGoldsFromHand()
    {
        var carryingGold = carry; // tasidigimiz altin sayisini kopyaladik
        if (carryingGold == 0) return 0; // eger altin tasimiyorsak ugrasma
        
        foreach (var gold in goldList) 
            gold.SetActive(false); // elimizdeki butun altinlari kapattik
        
        carry = 0; // tasidigimiz altin sayisini 0 ladik
        movementSpeed = baseMovementSpeed; // hareket hizimi default degere set ediyorum.
        // movementSpeed += carryingGold * reduceSpeed; // hareket hizimi default degere set ediyorum.
        
        return carryingGold; // tasidigimiz altin sayisini return ettik.
    }

    public void Ragdoll(bool isActive)
    {
        animator.enabled = !isActive; // animatoru ac kapat

        var colliders = boneParent.GetComponentsInChildren<Collider>(); // kemiklerdeki colliderlarin hepsine eris
        var rigidbodies = boneParent.GetComponentsInChildren<Rigidbody>(); // kemiklerdeki rigidbodylere eris

        foreach (var coll in colliders)
            coll.enabled = isActive; 

        foreach (var rig in rigidbodies)
            rig.isKinematic = !isActive;

        // rb.isKinematic = isActive;

        GetComponent<Collider>().enabled = !isActive;
        CanMove = !isActive; // ragdoll oldugu zaman hareketi engelle

        if (!isActive)
            StartCoroutine(CloseRagdoll());
    }

    public IEnumerator CloseRagdoll()
    {
        yield return new WaitForSeconds(5f); // 5 saniye bekle
        Ragdoll(false); // ragdoll kapa
        // omurganin oldugu konuma playerimi isinla. y konumu haric cunku karakterin y konumu asla degismiyor.
        transform.position = new Vector3(spinePosition.position.x, 0, spinePosition.position.z); 
    }
}