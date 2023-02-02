using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDmg : MonoBehaviour
{
    void Start()
    {

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            ControladorJugador.HacerDmg(10.0f);
            Debug.Log("Hacer dmg!");
        }
    }
}
