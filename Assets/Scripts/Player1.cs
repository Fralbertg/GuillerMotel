using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class Player1:Player
    {
        void Awake()
        {
            InitComponents();
            // InitLineRenderer();
            InitStatusVariables();
            Instantiate(dragging).TouchHalfRightScreen = false;
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
                    if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
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
        new void ColocateArms()
        {
            ArrowRB.transform.position = codoDireccion.transform.position;
            manoFlecha.transform.position = codoDireccion.transform.position;
            brazoHombro.transform.position = codoFlecha.transform.position;
            ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
