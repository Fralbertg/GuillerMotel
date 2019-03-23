using System.Collections;
using UnityEngine;

public class ArrowEnemie : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip popSound;
    public AudioClip headshotSound;
    public AudioClip hitSound;
    public AudioClip dragSound;
    public AudioClip throwSound;
    public AudioClip cloudSound;

    private bool isPressed;
    private float releaseDelay;
    private float minDragDistance = 2.8f;
    private float timeSinceLastArrow;

    public Rigidbody2D brazos;
    public GameObject arco;
    public GameObject manoFlecha;
    public GameObject codoFlecha;
    public GameObject codoDireccion;
    public GameObject brazoHombro;
    public GameObject hombro;
    public GameObject p1, p2;
    float distance;
    private float minX,minY,maxY;
    bool collided=false;
    //private float timeSinceLastArrow;
    private Rigidbody2D arrowRB;
    public LineRenderer line_renderer;
    public LineRenderer cuerda;
    public GameObject circle,circle2;
    private Vector2 endPointLine;
    private Vector2 startPointLine;
    private Vector2 mousePosition;
    private Vector2 direction;
    private Transform player;
    bool isArrowThrown;
    bool canThrow;
    bool isYourTurn;
    // Start is called before the first frame 

    void Start()
    {
        audio = GetComponent<AudioSource>();
        cuerda.positionCount = 3;
        cuerda.SetPosition(0, p1.gameObject.transform.position);
        cuerda.SetPosition(1, manoFlecha.transform.position);
        cuerda.SetPosition(2, p2.gameObject.transform.position);


        canThrow = false;
        arrowRB = GetComponent<Rigidbody2D>();
        arrowRB.constraints = RigidbodyConstraints2D.None;
        arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;

        isArrowThrown = false;
        circle.SetActive(false);
        circle2.SetActive(false);
        collided = false;
        arrowRB.isKinematic = true;
        //arrowRB.position = bow.transform.position;

        InitLineRenderer();
        timeSinceLastArrow = 0;
        Debug.Log("Arrow spawn P2 in -> " + arrowRB.transform.position.ToString());
    }
    // Update is called once per frame
    void Update()
    {
        //lo dejo aquí porque si lo dejo en el primer if 
        /*
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) 
            )
        {
            startPointLine = mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            UpdateRope();
        
            endPointLine = mousePosition;
            isYourTurn = GameManager.sharedInstance.IsTurnP2();
            if (startPointLine.x > GameObject.FindGameObjectWithTag("MainCamera").transform.position.x + 3f / 2
                //&& isYourTurn
                )
            {
                if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().PlayOneShot(dragSound);
                Dragging();
                canThrow = true;
            }
            else
                canThrow = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            ClearLine();
            //Suelto la flecha del brazo
            HingeJoint2D joint = GetComponent<HingeJoint2D>();
            joint.enabled = false;
            GetComponent<AudioSource>().Stop();
            direction = (startPointLine - endPointLine);
            if (!isArrowThrown && canThrow )
                throwArrow();
        }
        if(isArrowThrown && !collided)
        {
            Vector2 vectorAngle = arrowRB.velocity;

            float angle = (Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg +180);
            // Debug.Log("Angulo flecha volando: " + angle);
            arrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }*/
        timeSinceLastArrow += Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "nube")
        {
            collider.gameObject.GetComponent<ParticleSystem>().Play();
            audio.PlayOneShot(cloudSound);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLISIÓN tag enemy: " + collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            case "obstacle":
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                collided = true;
                GetComponent<AudioSource>().PlayOneShot(popSound);
                arrowRB.position = collision.transform.position;
                arrowRB.transform.parent = GameObject.FindGameObjectWithTag("obstacle").transform;
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "suelo":
                GetComponent<AudioSource>().PlayOneShot(popSound);
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                collided = true;
                break;
            //Colliders con el cuerpo
            case "person":
                GameManager.sharedInstance.AddHitP1();
                break;
            case "cabeza":
                GameObject.FindGameObjectWithTag("cabeza").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(headshotSound);
                GameManager.sharedInstance.HeadshotP1();
                arrowRB.position = collision.transform.position;
                arrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                collided = true;
                if (!arrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.HeadshotP1();
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "torsoAlto":
                GameObject.FindGameObjectWithTag("torsoAlto").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                arrowRB.position=collision.transform.position;
                arrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                collided = true;
                if (!arrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "torsoBajo":
                GameObject.FindGameObjectWithTag("torsoBajo").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                arrowRB.position = collision.transform.position;
                arrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                collided = true;
                if (!arrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "piernaIzq":
                GameObject.FindGameObjectWithTag("piernaIzq").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                arrowRB.position = collision.transform.position;
                arrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                collided = true;
                if (!arrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "piernaDer":
                GameObject.FindGameObjectWithTag("piernaDer").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                arrowRB.position = collision.transform.position;
                arrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                arrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                collided = true;
                if (!arrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            /***/
            case "escudoP1":
                GameManager.sharedInstance.HitEscudoP1();
                break;
            case "explosionBonus":
                GameManager.sharedInstance.BonusExplosionHit(false);
                break;
            case "bonusLife":
                GameManager.sharedInstance.BonusLifeHit(false);
                break;
           ;
        }
    }

    private void Dragging()
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
        float angle =(Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg);
        if (!isArrowThrown)
        {
            if(distance > 0.2f)
            {

                if (distance > 3)
                {
                    arrowRB.transform.position = codoDireccion.transform.position;
                    manoFlecha.transform.position = hombro.transform.position;
                    brazoHombro.transform.position = codoFlecha.transform.position;
                    arrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    brazos.MoveRotation(angle);
                }
                else
                {
                    arrowRB.transform.position = manoFlecha.transform.position;
                    manoFlecha.transform.position = codoDireccion.transform.position;
                    brazoHombro.transform.position = codoFlecha.transform.position;

                    arrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    brazos.MoveRotation(angle);
                }

                cuerda.SetPosition(0, p1.transform.position);
                cuerda.SetPosition(2, p2.transform.position);
                cuerda.SetPosition(1, manoFlecha.transform.position);
            }
        }
            
    }

    private void throwArrow()
    {
        arrowRB.transform.position = codoDireccion.transform.position;
        manoFlecha.transform.position = codoDireccion.transform.position;
        brazoHombro.transform.position = codoFlecha.transform.position;

        if (distance > 1f)
        {
            Debug.Log("Throwing arrow...");
            //lo dejo dinamico
            arrowRB.isKinematic = false;
            arrowRB.gameObject.GetComponent<Collider2D>().isTrigger = false;

            Vector2 direction = (startPointLine - endPointLine).normalized;

            //Fuerza de la flecha
            if (distance > minDragDistance)
                arrowRB.velocity = direction * 25;
            else
                arrowRB.velocity = direction * distance * 10;

            GetComponent<AudioSource>().PlayOneShot(throwSound);

            arrowRB.GetComponent<TrailRenderer>().enabled = true;
            isArrowThrown = true;
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

    //Desintegro, elimino, destruyo la primera flecha del buffer de flechas.
    private void DestroyArrow()
    {
        Destroy(ArrowGenerator.sharedInstance.Arrows[0]);
        ArrowGenerator.sharedInstance.DestroyArrow(ArrowGenerator.sharedInstance.Arrows[0]);
    }

    private void ClearLine()
    {
        line_renderer.positionCount = 0;
        circle.gameObject.SetActive(false);
        circle2.gameObject.SetActive(false);
    }

    //
    private void UpdateRope()
    {
        cuerda.positionCount = 3;
        cuerda.SetPosition(0, p1.transform.position);
        cuerda.SetPosition(1, manoFlecha.transform.position);
        cuerda.SetPosition(2, p2.transform.position);
        if (!isArrowThrown)
            arrowRB.position = codoDireccion.transform.position;
    }

    //Inicializo el LineRenderer
    public void InitLineRenderer()
    {
        line_renderer.startColor = Color.white;
        line_renderer.startWidth = 0.05f;
        line_renderer.positionCount = 2;
    }

}
