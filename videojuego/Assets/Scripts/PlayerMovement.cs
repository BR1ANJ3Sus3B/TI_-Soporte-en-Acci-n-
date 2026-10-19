
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento con WASD
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");

        movimiento = movimiento.normalized;

        // Detectar si Alex está corriendo
        corriendo =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        // Indicar si está en movimiento
        bool estaMoviendose = movimiento != Vector2.zero;

        animator.SetBool("IsMoving", estaMoviendose);

        if (estaMoviendose)
        {
            // Actualizar dirección del Blend Tree
            animator.SetFloat("MoveX", movimiento.x);
            animator.SetFloat("MoveY", movimiento.y);

            ultimaDireccion = movimiento;
        }

        // Ajustar velocidad de las animaciones
        animator.speed = corriendo && estaMoviendose ? 1.5f : 1f;
    }

    private void FixedUpdate()
    {
        float velocidad = corriendo
            ? velocidadCorrer
            : velocidadCaminar;

        rb.MovePosition(
            rb.position +
            movimiento * velocidad * Time.fixedDeltaTime
        );
    }
}
