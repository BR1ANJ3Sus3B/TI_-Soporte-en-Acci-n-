using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidadCaminar = 3f;
    public float velocidadCorrer = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movimiento;
    private Vector2 ultimaDireccion = Vector2.down;

    private bool corriendo;
    private string animacionActual;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento con WASD
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        movimiento = movimiento.normalized;

        // Shift para correr
        corriendo =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        if (movimiento != Vector2.zero)
        {
            ultimaDireccion = movimiento;

            if (corriendo)
            {
                CambiarAnimacion(
                    "Alex_Run_" + ObtenerDireccion(movimiento)
                );
            }
            else
            {
                CambiarAnimacion(
                    "Alex_Walk_" + ObtenerDireccion(movimiento)
                );
            }
        }
        else
        {
            // Animación cuando Alex está quieto
            CambiarAnimacion("Alex_Still");
        }
    }

    void FixedUpdate()
    {
        float velocidad = corriendo
            ? velocidadCorrer
            : velocidadCaminar;

        rb.MovePosition(
            rb.position +
            movimiento * velocidad * Time.fixedDeltaTime
        );
    }

    string ObtenerDireccion(Vector2 direccion)
    {
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            if (direccion.x > 0)
                return "Right";
            else
                return "Left";
        }
        else
        {
            if (direccion.y > 0)
                return "Up";
            else
                return "Down";
        }
    }

    void CambiarAnimacion(string nombre)
    {
        if (animacionActual == nombre)
            return;

        animator.Play(nombre);
        animacionActual = nombre;
    }
}