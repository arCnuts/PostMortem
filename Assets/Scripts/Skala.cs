using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skala : MonoBehaviour
{
    public GameObject skala;

    public void Start()
    {
        skala.SetActive(true);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skala.SetActive(false);
          
        }
    }
}
