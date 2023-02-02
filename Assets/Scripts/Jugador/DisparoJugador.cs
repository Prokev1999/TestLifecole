using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    public Inventario referencia;
    List<Arma> inventario;
    Arma armaSeleccionada = null;
    float tiempo = 0;
    int indiceSelecionado = 0;
    Coroutine recargando;
    public static Action<int, int> recarga;
    public static Action<int, int> disparo;
    public static Action<Arma> cambioArma;
    void Start()
    {
        inventario = referencia.armas;
        if(inventario.Count == 0)
        {
            Debug.LogError("No hay armas en el inventario selecionado");
            return;
        }
        armaSeleccionada = inventario[0];
        cambioArma?.Invoke(armaSeleccionada);
    }
    void Update()
    {
        //Cuando la rueda del ratón sube
        if(Input.mouseScrollDelta.y > 0)
        {
            indiceSelecionado--;
            if(indiceSelecionado < 0)
            {
                indiceSelecionado = inventario.Count - 1;
            }
            armaSeleccionada = inventario[indiceSelecionado];
            cambioArma?.Invoke(armaSeleccionada);
        }
        //Cuando la rueda del ratón baja
        if(Input.mouseScrollDelta.y  < 0)
        {
            indiceSelecionado++;            
            if(indiceSelecionado >= inventario.Count)
            {
                indiceSelecionado = 0;
            }
            armaSeleccionada = inventario[indiceSelecionado];
            cambioArma?.Invoke(armaSeleccionada);
        }
        switch(armaSeleccionada.tipoDisparo)
        {
            case TipoDisparo.Semiautomatico:
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Disparar();
                }
            }break;
            case TipoDisparo.Automatico:
            {
                if (Input.GetMouseButton(0))
                {
                    Disparar();
                }
            }break;
        }
        tiempo += Time.deltaTime;

        //Recargar
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(recargando == null)
            {
                recargando = StartCoroutine(Recargar());
            }
        }
    }
    IEnumerator Recargar()
    {
        yield return new WaitForSeconds(armaSeleccionada.tiempoRecarga);
        armaSeleccionada.balasActuales = armaSeleccionada.cargador;
        recargando = null;
        recarga?.Invoke(armaSeleccionada.balasActuales, armaSeleccionada.cargador);
    }
    void Disparar()
    {
        if(armaSeleccionada.cadencia < tiempo)
        {
            if(armaSeleccionada.balasActuales >= 0)
            {
                tiempo = 0;
                disparo?.Invoke(armaSeleccionada.balasActuales, armaSeleccionada.cargador);
                RaycastHit impacto;
                Ray rayo = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                Physics.Raycast(rayo.origin, rayo.direction, out impacto, armaSeleccionada.alcance);
                if(impacto.collider != null)
                {
                    Debug.Log("Impacto en: " + impacto.collider.gameObject.name);
                }
                armaSeleccionada.balasActuales--;
            }
        }
    }
}
