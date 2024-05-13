using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public List<GameObject> golds; // truckta bulunacak altinlari tuttugumuz liste 
    public GameObject goldsParent; // onceden yerlestirdigimiz altinlarin parenti
    private int currentGold; // su anda acik olan altin sayisi.
    public TextMeshProUGUI scoreText;
    
    
    private void Start()
    {
        golds = new List<GameObject>(); // listeyi initialize ettik
        foreach (Transform gold in goldsParent.transform) // gold parent'indaki butun childlara ulastik
        {
            golds.Add(gold.gameObject); // bunlari listeye ekledik
            gold.gameObject.SetActive(false); // ve gorunurlugunu kapattik.
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player") return; // carpisma objesinin (other) tag'ini kontrol et.
        var player = other.gameObject.GetComponent<PlayerController>(); // other objesinden karakter kontrol scriptine ulas.
        
        var gold = player.DropGoldsFromHand(); // playerda kac tane altin oldugunun sayisini aldik.
        currentGold += gold; // bu altinlari mevcut altin sayisina ekledik
        scoreText.SetText("Collected Gold: " + currentGold);

        for (int i = 0; i < currentGold; i++)
            golds[i].SetActive(true); // sonrasinda arabadaki altinlari actim.
    }
}