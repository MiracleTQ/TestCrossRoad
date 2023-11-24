using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float offsetY;
    
    public List<GameObject> terrainObjects;

    private GameObject spawnObject;

    private int lastIndex;

    private void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

    private void OnGetPointEvent(int obj)
    {
        CheckPosition();
    }

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }
    private void Start()
    {
        CheckPosition(); 
    }

    private void CheckPosition()
    {
        if (transform.position.y - Camera.main.transform.position.y < offsetY/2)
        {
            transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
            SpawnTerrain();
        }
    }
    private void SpawnTerrain()
    {
        var randomIndex=Random.Range(0,terrainObjects.Count);

        while (lastIndex == randomIndex)
        {
            randomIndex = Random.Range(0, terrainObjects.Count);
        }
        lastIndex = randomIndex;
        spawnObject = terrainObjects[randomIndex];

        Instantiate(spawnObject, transform.position, Quaternion.identity);
    }
}
