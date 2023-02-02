using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TipoDisparo
{
    Semiautomatico = 0,
    Automatico
}
[CreateAssetMenu(menuName ="Objetos/Arma")]
public class Arma : ScriptableObject
{
    public string nombre;
    public float damage;
    public float cadencia;
    public float alcance;
    public int cargador;
    public int balasActuales;
    public float tiempoRecarga;
    public TipoDisparo tipoDisparo;
}
