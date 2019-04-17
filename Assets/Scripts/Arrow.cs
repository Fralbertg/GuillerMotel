using System.Collections;
using System.Drawing;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region sonidos 
    public AudioSource audio;
    public AudioClip popSound;
    public AudioClip headshotSound;
    public AudioClip hitSound;
    public AudioClip dragSound;
    public AudioClip throwSound;
    public AudioClip cloudSound;
    #endregion


    #region Cuerpo
    public Rigidbody2D brazos;
    public Rigidbody2D ArrowRB { get; set; }
    public GameObject manoFlecha;
    public GameObject codoFlecha;
    public GameObject codoDireccion;
    public GameObject brazoHombro;
    public GameObject hombro;
    public GameObject arco;
    public LineRenderer cuerda;
    public GameObject p1, p2;
    #endregion



    #region Variables de estado
    public float maxForceArrow = 25f;
    public float minForceArrow = 10f;
    private float maxDragDistance = 2.8f;
    private float distance;
    private float timeSinceLastArrow;
    public bool Collided { get; set; }
    public bool IsArrowThrown { get; set; }
    public bool IsTurnP1 { get; set; }
    public bool CanThrow { get; set; }
    #endregion

    #region UI
    public GameObject explosion;
    public LineRenderer line_renderer;
    public GameObject circle, circle2;
    #endregion

    #region Vectores
    private Vector2 endPointLine;
    private Vector2 startPointLine;
    private Vector2 mousePosition;
    private Vector2 direction;
    #endregion


    // Start is called before the first frame update
    private void Awake()
    {
        InitComponents();
        InitStatusVariables();
        InitLineRenderer();
    }

    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        /*
        UpdateRope();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)
           )
        {
            startPointLine = mousePosition;
            audio.PlayOneShot(dragSound);

        }
        if (Input.GetMouseButton(0))
        {
            endPointLine = mousePosition;
            IsTurnP1 = GameManager.sharedInstance.IsTurnP1();
            // modifico la línea en función de donde di el primer click y hacia donde llevo el mouse
            if (startPointLine.x < GameObject.FindGameObjectWithTag("MainCamera").transform.position.x + 3f / 2
               // && isTurnP1
                )
            {
                if(!audio.isPlaying)
                    audio.PlayOneShot(dragSound);

                Dragging();
                CanThrow = true;
            }
            else
                CanThrow = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            ClearLine();
            //Suelto la flecha del brazo
            HingeJoint2D joint = GetComponent<HingeJoint2D>();
            joint.enabled = false;
            direction = (startPointLine - endPointLine);
            audio.Stop();

            if (!IsArrowThrown && CanThrow && timeSinceLastArrow > 0.4f)
                ThrowArrow();
        }
        if (IsArrowThrown && !Collided)
        {
            Vector2 vectorAngle = ArrowRB.velocity;

            float angle = (Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg)+180;
            // Debug.Log("Angulo flecha volando: " + angle);
            ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        */
        timeSinceLastArrow += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "nube")
        {
            collider.gameObject.GetComponent<ParticleSystem>().Play();
            
            audio.PlayOneShot(cloudSound);
        }
        if (collider.gameObject.tag == "escudoP2")
        {
            GameManager.sharedInstance.HitEscudoP2();
            Instantiate(explosion, transform.position, Quaternion.identity);
            Debug.Log("ArrowP1 touch EscudoP2");
            Destroy(gameObject);
            //gameObject.GetComponent<TrailRenderer>().autodestruct = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        Debug.Log("COLISIÓN tag: " + collision.gameObject.tag);
        switch (collision.gameObject.tag)
        {
            case "obstacle":
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                audio.PlayOneShot(popSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("obstacle").transform;
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "suelo":
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                audio.PlayOneShot(popSound);
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;

            //Colliders con el cuerpo
            case "person":
                GameManager.sharedInstance.AddHitP2();
                break;
            case "cabeza2":
                GameObject.FindGameObjectWithTag("cabeza2").GetComponent<ParticleSystem>().Play();
                audio.PlayOneShot(headshotSound);
                GameManager.sharedInstance.HeadshotP2();
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player2").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP2();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "torsoAlto2":
                GameObject.FindGameObjectWithTag("torsoAlto2").GetComponent<ParticleSystem>().Play();
                audio.PlayOneShot(hitSound);
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player2").transform;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP2();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "torsoBajo2":
                GameObject.FindGameObjectWithTag("torsoBajo2").GetComponent<ParticleSystem>().Play();
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player2").transform;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP2();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                audio.PlayOneShot(hitSound);
                break;
            case "piernaIzq2":
                GameObject.FindGameObjectWithTag("piernaIzq2").GetComponent<ParticleSystem>().Play();
                audio.PlayOneShot(hitSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player2").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP2();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "piernaDer2":
                GameObject.FindGameObjectWithTag("piernaDer2").GetComponent<ParticleSystem>().Play();
                audio.PlayOneShot(hitSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player2").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if(!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP2();
                GetComponent<Collider2D>().isTrigger = true;
                break;
            /***/
            case "escudoP2":


                break;
            case "explosionBonus":
                GameManager.sharedInstance.BonusExplosionHit(true);
                break;
            case "bonusLife":
                GameManager.sharedInstance.BonusLifeHit(false);
                break;
        }
    }
  
    public void Dragging()
    {
       
        cuerda.positionCount = 3;
        distance = Vector2.Distance(endPointLine, startPointLine);
        //Debug.Log("Distancia " + distance);
        //arrowRB.isKinematic = false;
        line_renderer.positionCount = 2;

        circle.transform.position = startPointLine;
        circle2.transform.position = endPointLine;

        circle.gameObject.SetActive(true);
        circle2.gameObject.SetActive(true);

        line_renderer.SetPosition(0, startPointLine);
        line_renderer.SetPosition(1, endPointLine);

        Vector2 vectorAngle = endPointLine - startPointLine;
        float angle =(Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg);
        if (!IsArrowThrown)
        {
            if(distance > 0.2f)
            {
                if (distance > 3)
                {
                    ArrowRB.transform.position = codoDireccion.transform.position;
                    manoFlecha.transform.position = hombro.transform.position;
                    brazoHombro.transform.position = codoFlecha.transform.position;
                    ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    brazos.MoveRotation(angle + 180);
                }
                else
                {
                    ArrowRB.transform.position = manoFlecha.transform.position;
                    manoFlecha.transform.position = codoDireccion.transform.position;
                    brazoHombro.transform.position = codoFlecha.transform.position;

                    ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    brazos.MoveRotation(angle + 180);
                }

                cuerda.SetPosition(0, p1.transform.position);
                cuerda.SetPosition(2, p2.transform.position);
                cuerda.SetPosition(1, manoFlecha.transform.position);
            }
        }
            
    }

    public void ThrowArrow()
    {
        ColocateArms();
        float minDinstance = 0.7f;
        if (distance > minDinstance)
        {
            ArrowRB.isKinematic = false;
            
            Vector2 direction = (startPointLine - endPointLine).normalized;

            if (distance > maxDragDistance)
                ArrowRB.velocity = direction * maxForceArrow;
            else
                ArrowRB.velocity = direction * distance * minForceArrow;

            ArrowRB.GetComponent<TrailRenderer>().enabled = true;
            IsArrowThrown = true;

            audio.PlayOneShot(throwSound);

            // AÑADO FLECHA Y SI HAY MÁS DE 10 ELIMINO LA PRIMERA DE LA LISTA
            ArrowGenerator.sharedInstance.AddArrow(gameObject);
            if (ArrowGenerator.sharedInstance.Arrows.Count >= 15)
            {
                DestroyLastArrow();
            }

            cuerda.positionCount = 0;
            GameManager.sharedInstance.SetTurnP1(false);
        }
    }
    public void DestroyLastArrow()
    {
        Destroy(ArrowGenerator.sharedInstance.Arrows[0]);
        ArrowGenerator.sharedInstance.DestroyArrow(ArrowGenerator.sharedInstance.Arrows[0]);
    }
    public void ClearLine()
    {
        line_renderer.positionCount = 0;
        circle.gameObject.SetActive(false);
        circle2.gameObject.SetActive(false);
    }
    private void ColocateArms()
    {
        ArrowRB.transform.position = codoDireccion.transform.position;
        manoFlecha.transform.position = codoDireccion.transform.position;
        brazoHombro.transform.position = codoFlecha.transform.position;
        ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = false;
    }
    public void UpdateRope()
    {
        //updaterope and arrow position in the arm

        cuerda.positionCount = 3;
        cuerda.SetPosition(0, p1.transform.position);
        cuerda.SetPosition(1, manoFlecha.transform.position);
        cuerda.SetPosition(2, p2.transform.position);

        if (!IsArrowThrown)
            ArrowRB.position = codoDireccion.transform.position;
    }

    private void InitComponents()
    {
        ArrowRB = GetComponent<Rigidbody2D>();
        ArrowRB.isKinematic = true;
        ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;

        cuerda.SetPosition(0, p1.gameObject.transform.position);
        cuerda.SetPosition(2, p2.gameObject.transform.position);
        cuerda.SetPosition(1, manoFlecha.transform.position);

        circle.SetActive(false);
        circle2.SetActive(false);
    }

    private void InitStatusVariables()
    {
        Collided = false;
        CanThrow = false;
        IsArrowThrown = false;
        timeSinceLastArrow = 0;
    }

    private void InitLineRenderer()
    {
        //Modify the LineRenderer
        line_renderer.startColor = UnityEngine.Color.white;
        line_renderer.startWidth = 0.05f;
        line_renderer.positionCount = 2;
    }

}
