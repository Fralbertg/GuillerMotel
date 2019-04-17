using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region sonido
    protected AudioSource audio;
    public AudioClip popSound;
    public AudioClip headshotSound;
    public AudioClip hitSound;
    public AudioClip dragSound;
    public AudioClip throwSound;
    public AudioClip cloudSound;
    #endregion

    #region Variables de estado
    public float maxForceArrow = 25f;
    public float minForceArrow = 10f;
    protected float maxDragDistance = 2.8f;
    protected bool isPressed;
    protected float releaseDelay;
    protected float minDragDistance = 2.8f;
    protected float timeSinceLastArrow;
    protected float distance;
    protected float minX, minY, maxY;
    protected bool canThrow, isYourTurn;
    public bool Collided { get; set; }
    public bool IsArrowThrown { get; set; }
    #endregion

    #region cuerpo
    public Rigidbody2D brazos;
    protected Transform player;
    public GameObject arco;
    public GameObject manoFlecha;
    public GameObject codoFlecha;
    public GameObject codoDireccion;
    public GameObject brazoHombro;
    public GameObject hombro;
    public GameObject p1, p2;
    public Rigidbody2D ArrowRB { get; set; }
    #endregion

    #region UI
    public Explosion explosion;
    public ProjectileDragging dragging;
    public LineRenderer line_renderer;
    public LineRenderer cuerda;
    public GameObject circle, circle2;
    #endregion

    #region vectores
    protected Vector2 endPointLine;
    protected Vector2 startPointLine;
    protected Vector2 mousePosition;
    protected Vector2 direction;
    #endregion

    void Awake()
    {
        InitComponents();
        //InitLineRenderer();
        InitStatusVariables();
    }
    // Update is called once per frame
    void Update()
    {
        //lo dejo aquí porque si lo dejo en el primer if 
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateRope();
      
        timeSinceLastArrow += Time.deltaTime;
    }
   
    public void Dragging()
    {
        cuerda.positionCount = 3;
        distance = Vector2.Distance(endPointLine, startPointLine);
        // Debug.Log("Player 2 distancia " + distance);
        line_renderer.positionCount = 2;

        circle.transform.position = startPointLine;
        circle2.transform.position = endPointLine;

        circle.gameObject.SetActive(true);
        circle2.gameObject.SetActive(true);

        line_renderer.SetPosition(0, startPointLine);
        line_renderer.SetPosition(1, endPointLine);

        //Debug.Log("Distancia " + distance);
        Vector2 vectorAngle = endPointLine - startPointLine;
        float angle = (Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg);
        if (!IsArrowThrown)
        {
            if (distance > 0.2f)
            {

                if (distance > 3)
                {
                    ArrowRB.transform.position = codoDireccion.transform.position;
                    manoFlecha.transform.position = hombro.transform.position;
                    brazoHombro.transform.position = codoFlecha.transform.position;
                    ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    brazos.MoveRotation(angle);
                }
                else
                {
                    ArrowRB.transform.position = manoFlecha.transform.position;
                    manoFlecha.transform.position = codoDireccion.transform.position;
                    brazoHombro.transform.position = codoFlecha.transform.position;

                    ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    brazos.MoveRotation(angle);
                }

                cuerda.SetPosition(0, p1.transform.position);
                cuerda.SetPosition(2, p2.transform.position);
                cuerda.SetPosition(1, manoFlecha.transform.position);
            }
        }

    }

    protected void throwArrow()
    {
        

        if (distance > 1f)
        {
            Debug.Log("Throwing arrow...");
            //lo dejo dinamico
            ArrowRB.isKinematic = false;
            ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = false;

            Vector2 direction = (startPointLine - endPointLine).normalized;

            //Fuerza de la flecha
            if (distance > minDragDistance)
                ArrowRB.velocity = direction * 25;
            else
                ArrowRB.velocity = direction * distance * 10;

            GetComponent<AudioSource>().PlayOneShot(throwSound);

            ArrowRB.GetComponent<TrailRenderer>().enabled = true;
            IsArrowThrown = true;
            GameManager.sharedInstance.SetTurnP1(true);
            // AÑADO FLECHA Y SI HAY MÁS DE 10 ELIMINO LA PRIMERA DE LA LISTA
            ArrowEnemyGenerator.sharedInstance.AddArrow(gameObject);

            if (ArrowEnemyGenerator.sharedInstance.GetArrows().Count >= 15)
            {
                DestroyArrow();
            }
            //reseteo el LineRenderer de la cuerda del arco para reposicionarla
            cuerda.positionCount = 0;
        }
    }
    protected void ColocateArms()
    {
        ArrowRB.transform.position = codoDireccion.transform.position;
        manoFlecha.transform.position = codoDireccion.transform.position;
        brazoHombro.transform.position = codoFlecha.transform.position;
        ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = false;
    }
    //Desintegro, elimino, destruyo la primera flecha del buffer de flechas.
    protected void DestroyArrow()
    {
        Destroy(ArrowGenerator.sharedInstance.Arrows[0]);
        ArrowGenerator.sharedInstance.DestroyArrow(ArrowGenerator.sharedInstance.Arrows[0]);
    }

    protected void ClearLine()
    {
        line_renderer.positionCount = 0;
        circle.gameObject.SetActive(false);
        circle2.gameObject.SetActive(false);
    }

    //
    public void UpdateRope()
    {
        cuerda.positionCount = 3;
        cuerda.SetPosition(0, p1.transform.position);
        cuerda.SetPosition(1, manoFlecha.transform.position);
        cuerda.SetPosition(2, p2.transform.position);
        if (!IsArrowThrown)
            ArrowRB.position = codoDireccion.transform.position;
    }
    protected void InitComponents()
    {
        ArrowRB = GetComponent<Rigidbody2D>();
        ArrowRB.isKinematic = true;
        ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;

        audio = gameObject.GetComponent<AudioSource>();
    }
    protected void InitStatusVariables()
    {
        Collided = false;
        canThrow = false;
        IsArrowThrown = false;
        timeSinceLastArrow = 0;
    }
  
    void DragIA()
    {
        Vector2 origin, destiny;
        origin = player.transform.position;
        float randomX = UnityEngine.Random.Range(origin.x, maxForceArrow);
        float randomY = UnityEngine.Random.Range(transform.position.y - 5, player.transform.position.y + 5);
        destiny = new Vector2(randomX, randomY);
        ///// player.transform.position = Vector2.MoveTowards(origin, destiny, fixedSpeed);
    }
}
