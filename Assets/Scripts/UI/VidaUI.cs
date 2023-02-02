using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI texto;

    void OnEnable()
    {
        ControladorJugador.Curando += ActualizarUI;
        ControladorJugador.EnDmg += ActualizarUI;
    }
    void OnDisable()
    {
        ControladorJugador.Curando -= ActualizarUI;
        ControladorJugador.EnDmg -= ActualizarUI;
    }
    void ActualizarUI(float vida)
    {
        texto.text = ((int)vida).ToString();
    }
}
