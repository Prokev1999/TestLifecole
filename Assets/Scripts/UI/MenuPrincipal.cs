using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuOpciones;
    bool showOptions = false;
    void Start()
    {
        menuOpciones.SetActive(showOptions);
    }
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }
    public void Opciones()
    {
        showOptions = !showOptions;
        menuOpciones.SetActive(showOptions);
    }
    public void Salir()
    {
        Application.Quit();
    }
}
