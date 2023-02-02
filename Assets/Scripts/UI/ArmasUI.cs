using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ArmasUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nombreArma;
    public TMPro.TextMeshProUGUI indicadorMunicion;


    private void OnEnable()
    {
        DisparoJugador.cambioArma += CambioArma;
        DisparoJugador.disparo += Disparo;
        DisparoJugador.recarga += Recarga;
    }
    private void OnDisable()
    {
        DisparoJugador.cambioArma -= CambioArma;
        DisparoJugador.disparo -= Disparo;
        DisparoJugador.recarga -= Recarga;
    }

    void CambioArma(Arma arma)
    {
        nombreArma.text = arma.nombre;
        string muncion = arma.balasActuales + "/" + arma.cargador;
        indicadorMunicion.text = muncion;
    }
    void Recarga(int actual, int cargador)
    {
        string muncion = actual + "/" + cargador;
        indicadorMunicion.text = muncion;
    }
    void Disparo(int actual, int cargador)
    {
        string muncion = actual + "/" + cargador;
        indicadorMunicion.text = muncion;
    }
}
