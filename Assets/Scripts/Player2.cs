using System.Collections;
using UnityEngine;

public class Player2 : Player
{
    void Awake()
    {
        InitComponents();
       // InitLineRenderer();
        InitStatusVariables();
        Instantiate(dragging).TouchHalfRightScreen=true;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "nube")
        {
            collider.gameObject.GetComponent<ParticleSystem>().Play();
            audio.PlayOneShot(cloudSound);
        }
        if (collider.gameObject.tag == "escudoP1")
        {
            Explosion e = Instantiate(explosion);
            GameManager.sharedInstance.HitEscudoP1();
            e.transform.position = transform.position;
            e.transform.parent = collider.transform;
            int currentArrow = ArrowGenerator.sharedInstance.Arrows.Count - 1;
            Debug.Log("Count:" + ArrowGenerator.sharedInstance.Arrows.Count);
            if (currentArrow + 1 > 0)
            {
                ArrowGenerator.sharedInstance.DestroyArrow(ArrowGenerator.sharedInstance.Arrows[currentArrow]);
                //Destroy(ArrowGenerator.sharedInstance.Arrows[currentArrow]);
                Destroy(gameObject);
            }

        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLISIÓN tag enemy: " + collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            case "obstacle":
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                GetComponent<AudioSource>().PlayOneShot(popSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("obstacle").transform;
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "suelo":
                GetComponent<AudioSource>().PlayOneShot(popSound);
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                Collided = true;
                break;
            //Colliders con el cuerpo
            case "person":
                GameManager.sharedInstance.AddHitP1();
                break;
            case "cabeza":
                GameObject.FindGameObjectWithTag("cabeza").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(headshotSound);
                GameManager.sharedInstance.HeadshotP1();
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.HeadshotP1();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "torsoAlto":
                GameObject.FindGameObjectWithTag("torsoAlto").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "torsoBajo":
                GameObject.FindGameObjectWithTag("torsoBajo").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "piernaIzq":
                GameObject.FindGameObjectWithTag("piernaIzq").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
                break;
            case "piernaDer":
                GameObject.FindGameObjectWithTag("piernaDer").GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                ArrowRB.position = collision.transform.position;
                ArrowRB.transform.parent = GameObject.FindGameObjectWithTag("Player1").transform;
                ArrowRB.constraints = RigidbodyConstraints2D.FreezeAll;
                timeSinceLastArrow = 0;
                Collided = true;
                if (!ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger)
                    GameManager.sharedInstance.AddHitP1();
                ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = true;
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
    new void ColocateArms()
    {
        ArrowRB.transform.position = codoDireccion.transform.position;
        manoFlecha.transform.position = codoDireccion.transform.position;
        brazoHombro.transform.position = codoFlecha.transform.position;
        ArrowRB.gameObject.GetComponent<Collider2D>().isTrigger = false;
    }
}
