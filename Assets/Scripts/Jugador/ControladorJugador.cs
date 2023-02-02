using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJugador : MonoBehaviour
{

    public bool PuedeCorrer => puedeCorrer && controlador.isGrounded && Input.GetKey(KeyCode.LeftShift);

    float rotacionX = 0;
    [Header("Opciones de camara")]
    public float velX = 2.0f;
    public float velY = 2.0f;
    public float limiteSuperior = 80.0f;
    public float limiteInferior = 80.0f;
    public Camera camara;

    [Header("Opciones de movimento")]
    public float velocidadCaminar = 6.0f;
    public float velocidadCorrer = 10.0f;
    public CharacterController controlador;

    [Header("Opciondes de salto")]
    public float fuerzaSalto = 8.0f;
    public float gravedad = 30.0f;

    [Header("Opciones de headbob")]
    public float velocidadHeadbobCaminar = 14f;
    public float velocidadHeadbobCorrer = 18f;
    public float cantidadHeadbobCaminar = 0.05f;
    public float cantidadHeadbobCorrer = 0.1f;
    float posicionInicialY = 0;
    float contador;

    [Header("Opciones de stamina")]
    public float staminaMax = 100f;
    public float staminaPorSegundo = 5f;
    public float timepoDeRegeneracionStamina = 5f;
    public float incrementoPorSegundoStamina = 2f;
    public float incrementoTiempoStamina = 0.1f;
    private float staminaActual;
    private Coroutine regenerandoStamina;
    public static Action<float> CambioStamina;
    bool puedeCorrer = true;

    [Header("Opciones de vida")]
    public float vidaMax = 100f;
    public float tiempoDeRegeneracionVida = 3f;
    public float incrementoPorSegundoVida = 1f;
    public float incrementoTiempoVida = 0.1f;
    float vidaActual;
    Coroutine regenerandoVida;
    public static Action<float> HacerDmg;
    public static Action<float> EnDmg;
    public static Action<float> Curando;


    public Vector3 direccionMovimiento;
    Vector2 inputMovimiento;

    void OnEnable()
    {
        HacerDmg += AplicarDmg;
    }
    void OnDisable()
    {
        HacerDmg -= AplicarDmg;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        staminaActual = staminaMax;
        vidaActual = vidaMax; //
        posicionInicialY = camara.transform.localPosition.y;
        CambioStamina?.Invoke(staminaActual);
        Curando?.Invoke(vidaActual);   
    }

    // Update is called once per frame
    void Update()
    {
        MoverCamara();
        ControlarInput();

        ControlarSalto();

        ControlarStamina();

        ControlarHeadBob();

        AplicarMovimiento();
    }
    void AplicarDmg(float dmg)
    {
        vidaActual -= dmg;
        EnDmg?.Invoke(vidaActual);

        if (vidaActual <= 0)
        {
            EliminarJugador();
        }
        else if (regenerandoVida != null)
        {
            StopCoroutine(regenerandoVida);
        }

        regenerandoVida = StartCoroutine(RegenerarVida());
    }
    IEnumerator RegenerarVida()
    {
        yield return new WaitForSeconds(tiempoDeRegeneracionVida);
        WaitForSeconds tiempoEspera = new WaitForSeconds(incrementoTiempoVida);
        while (vidaActual < vidaMax)
        {
            vidaActual += incrementoPorSegundoVida;
            if (vidaActual > vidaMax)
            {
                vidaActual = vidaMax;
            }
            Curando?.Invoke(vidaActual);
            yield return tiempoEspera;
        }
        regenerandoVida = null;
    }
    void EliminarJugador()
    {
        vidaActual = 0;

        if (regenerandoVida != null)
        {
            StopCoroutine(regenerandoVida);
        }
        //Resetar la partida
        Debug.Log("Has muerto");
    }
    void ControlarHeadBob()
    {
        if (!controlador.isGrounded)
        {
            return;
        }
        if (Mathf.Abs(direccionMovimiento.x) > 0.1f || Mathf.Abs(direccionMovimiento.z) > 0.1f)
        {
            float velocidad = PuedeCorrer ? velocidadHeadbobCorrer : velocidadHeadbobCaminar;
            float cantidad = PuedeCorrer ? cantidadHeadbobCorrer : cantidadHeadbobCaminar;
            contador += Time.deltaTime * velocidad;
            camara.transform.localPosition = new Vector3(
                camara.transform.localPosition.x,
                posicionInicialY + Mathf.Sin(contador) * cantidad,
                camara.transform.localPosition.z
            );
        }
    }

    void ControlarStamina()
    {
        if (PuedeCorrer && inputMovimiento != Vector2.zero)
        {
            //Si estabamos regenerando stamina paramos de regenerar
            if (regenerandoStamina != null)
            {
                StopCoroutine(regenerandoStamina);
                regenerandoStamina = null;
            }

            staminaActual -= staminaPorSegundo * Time.deltaTime;

            if (staminaActual < 0f)
            {
                staminaActual = 0f;
            }

            CambioStamina?.Invoke(staminaActual);

            if (staminaActual <= 0f)
            {
                puedeCorrer = false;
            }
        }
        if (!PuedeCorrer && staminaActual < staminaMax && regenerandoStamina == null)
        {
            regenerandoStamina = StartCoroutine(RegenerarStamina());
        }

    }
    IEnumerator RegenerarStamina()
    {
        yield return new WaitForSeconds(timepoDeRegeneracionStamina);
        WaitForSeconds tiempoDeEspera = new WaitForSeconds(incrementoTiempoStamina);
        while (staminaActual < staminaMax)
        {
            if (staminaActual > 0)
            {
                //Dejar que el jugador pueda esprintar
                puedeCorrer = true;
            }
            staminaActual += incrementoPorSegundoStamina;

            if (staminaActual > staminaMax)
            {
                staminaActual = staminaMax;
            }

            CambioStamina?.Invoke(staminaActual);

            yield return tiempoDeEspera;
        }
        regenerandoStamina = null;
    }


    void ControlarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controlador.isGrounded)
        {
            direccionMovimiento.y = fuerzaSalto;
        }
    }

    void AplicarMovimiento()
    {
        if (!controlador.isGrounded)
        {
            direccionMovimiento.y -= gravedad * Time.deltaTime;
        }

        controlador.Move(direccionMovimiento * Time.deltaTime);
    }

    void ControlarInput()
    {
        float velocidad = PuedeCorrer ? velocidadCorrer : velocidadCaminar;

        inputMovimiento = new Vector2(velocidad * Input.GetAxis("Vertical"), velocidad * Input.GetAxis("Horizontal"));

        float direccionMovimientoY = direccionMovimiento.y;
        direccionMovimiento = (transform.TransformDirection(Vector3.forward) * inputMovimiento.x) +
            (transform.TransformDirection(Vector3.right) * inputMovimiento.y);
        direccionMovimiento.y = direccionMovimientoY;
    }

    void MoverCamara()
    {
        rotacionX -= Input.GetAxis("Mouse Y") * velX;
        rotacionX = Mathf.Clamp(rotacionX, -limiteSuperior, limiteInferior);
        camara.transform.localRotation = Quaternion.Euler(rotacionX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * velY, 0);
    }
}
