using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI texto;

    private void OnEnable()
    {
        ControladorJugador.CambioStamina += ActualizarUI;
    }
    private void OnDisable()
    {
        ControladorJugador.CambioStamina -= ActualizarUI;
    }

    void ActualizarUI(float stamina)
    {
        texto.text = ((int)stamina).ToString();
    }
}
