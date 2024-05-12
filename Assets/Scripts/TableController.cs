using UnityEngine;

public class TableController : MonoBehaviour
{
    public GameObject goldObject; // masadaki altin objesi

    public bool IsGoldCollectable => goldObject.activeSelf; // masadaki altin objesi acik mi kapali mi?

    private void OnCollisionEnter(Collision other) // carpismaya basladigi zaman calisacak olan unity fonksiyonu
    {
        if (!IsGoldCollectable) return; // eger masadaki altin objem kapali ise fonksiyondan cik.
        
        if (other.gameObject.tag != "Player") return; // carpisma objesinin (other) tag'ini kontrol et.
        var player = other.gameObject.GetComponent<PlayerController>(); // other objesinden karakter kontrol scriptine ulas.

        if (player.CollectGold()) // collect gold fonksiyonum eger calistiysa
        {
            goldObject.SetActive(false); // altin objesini kapat.
            
            // 5 ile 15 saniye arasinda rastgele bir sure icinde ReloadGold fonksiyonunu calistir.
            Invoke(nameof(ReloadGold),Random.Range(5f,15f)); 
        }
    }

    private void ReloadGold() // altin objesini tekrar acan fonksiyon
    {
        goldObject.SetActive(true); // altin objesini ac.
    }
}