using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSpawner : MonoBehaviour
{
    public List<GameObject> carPrefabs; // araba prefablarinin tutuldugu liste
    public float minTime, maxTime; // spawn edilecek minimum ve max sure

    public float timer; // sayac
    public float spawnTime; // min ve max arasindaki rastgele sure

    private void Start()
    {
        spawnTime = Random.Range(minTime, maxTime); // olusucak ilk arabanin rastgele suresi
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime) // spawn time => 3.253612 ------ timer == spawnTime ------- 3.253612 3.254
        {
            timer = 0; // bu kosulun tek bir kere calismasi icin ifin icine girer girmez timer'i 0 a esitledim.
            var car = carPrefabs[Random.Range(0, carPrefabs.Count)]; // listeden rastgele araba sec

            // olusacak arabayi BU SCRIPTIN BAGLI OLDUGU TRANSFORMU parent yapmak icin en sona transform yazdim.
            var spawnedCar = Instantiate(car, transform.position, transform.rotation, transform);

            spawnedCar.AddComponent<CarController>(); // olusturdugum arabaya car controller scriptini ekliyorum

            Destroy(spawnedCar.gameObject, 5f); // olusturulan araba 5 saniye sonra kendini imha edecek

            spawnTime = Random.Range(minTime, maxTime); // olusacak sonraki arabanin rastgele suresi
        }
    }
}