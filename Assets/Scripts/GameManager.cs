using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager sharedInstance;
    AudioSource audio_source;
    public AudioClip mainSong;
    int hitsP1, hitsP2, pointsP1, pointsP2;
    public Text ptsp1, ptsp2, timerText;
    public GameObject healthBarP1, healthBarP2, turnPointerSprite, headshotSprite,mitadP1,mitadP2;
    public static GameObject EscudoP1 { get; set; }
    public static GameObject EscudoP2 { get; set; }
    private bool gameOver;
    private float time;
    private bool isTurnP1, isTurnP2, movePlatforms;
    public bool moveObstacle = false, showObstacle = false , rotateObstacle = false;

    public float fixedSpeed = 4;

    GameObject player1, player2, obstacle;
    GameObject bonusLife,bonusExplosion;
    Vector3 startP1, endP1, startP2, endP2, startO, endO;
    public float speed;
    public Transform target, target2, targetObstacle;
    private Vector2 currentTouch;
    private Vector2 endPointLineP1, endPointLineP2;
    private Vector2 startPointLineP1, startPointLineP2;
    float screenCenter;
    Arrow flechaP1;
    Player2 flechaP2;

    void Start()
    {
        EscudoP1 = GameObject.FindGameObjectWithTag("escudoP1");
        EscudoP2 = GameObject.FindGameObjectWithTag("escudoP2");
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        obstacle = GameObject.FindGameObjectWithTag("obstacle");
        bonusLife = GameObject.FindGameObjectWithTag("bonusLife");
        bonusExplosion = GameObject.FindGameObjectWithTag("bonusExplosion");

        obstacle.SetActive(false);
        sharedInstance = this;
        gameOver = false;
        isTurnP1 = true;
        movePlatforms = false;
        isTurnP2 = false;
        hitsP1 = 0;
        hitsP2 = 0;
        if (target != null)
        {
            target.parent = null;
            startP1 = new Vector2(player1.transform.position.x, player1.transform.position.y + 2.2f);
            endP1 = target.position;
        }
        if (target2 != null)
        {
            target2.parent = null;
            startP2 = new Vector2(player2.transform.position.x, player2.transform.position.y + 3f);
            endP2 = target2.position;
        }
        if (targetObstacle != null)
        {
            targetObstacle.parent = null;
            startO = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y + 2.2f);
            endO = targetObstacle.position;
        }
        pointsP1 = 0;
        pointsP2 = 0;
        StartCoundownTimer();
        flechaP1 = ArrowGenerator.sharedInstance.CurrentArrow.GetComponent<Arrow>();

    }
    void Awake()
    {
        audio_source = GetComponent<AudioSource>();
        audio_source.PlayOneShot(mainSong);
    }
    void Update()
    {
        // Debug.Log("Position current touch -> " + currentTouch.ToString());
        //Debug.Log(" im  NOT toichiiiing Position Current touch -> " + currentTouch.ToString());
        /*
        int touchesNum = Input.touchCount;
        if (touchesNum > 0)
            {
             Debug.Log("AAAAAAAAA " + currentTouch.ToString());

            for (int i = 0; i < touchesNum; i++)
            {
                Debug.Log(" im toichiiiing Position Current touch -> " + currentTouch.ToString());
                currentTouch = Input.GetTouch(0).position;
                if (Input.GetTouch(i).phase == TouchPhase.Began && currentTouch.x < screenCenter)
                {
                    startPointLineP1 = currentTouch;
                }
                else
                {
                    startPointLineP2 = currentTouch;
                }

                flechaP1.UpdateRope();
                flechaP2.UpdateRope();

                if (Input.GetTouch(i).phase == TouchPhase.Moved && currentTouch.x < screenCenter)
                {
                    endPointLineP1 = currentTouch;
                    // isYourTurn = GameManager.sharedInstance.IsTurnP2();

                    if (!GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().PlayOneShot(flechaP1.dragSound);
                    flechaP1.Dragging();

                }
                else// player 2
                {
                    endPointLineP2 = currentTouch;
                    // isYourTurn = GameManager.sharedInstance.IsTurnP2();
                    if (!GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().PlayOneShot(flechaP2.dragSound);
                    flechaP2.Dragging();
                }

                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    flechaP1.ClearLine();
                    //Suelto la flecha del brazo
                    HingeJoint2D joint = GetComponent<HingeJoint2D>();
                    joint.enabled = false;
                    GetComponent<AudioSource>().Stop();
                    if (!flechaP1.IsArrowThrown && flechaP1.CanThrow)
                        flechaP1.ThrowArrow();
                    // Reasigno el valor a la nueva flecha que se acaba de generar  
                    flechaP1 = ArrowGenerator.sharedInstance.CurrentArrow.GetComponent<Arrow>();
                }
                movingArrowP1();
                movingArrowP2();
            }
        }*/
        CheckGameStatus();
    }



    void movingArrowP1()
    {
        if (flechaP1.IsArrowThrown && !flechaP1.Collided)
        {
            Vector2 vectorAngle = flechaP1.ArrowRB.velocity;

            float angle = (Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg + 180);
            // Debug.Log("Angulo flecha volando: " + angle);
            flechaP1.ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void movingArrowP2()
    {
        if (flechaP2.IsArrowThrown && !flechaP2.Collided)
        {
            Vector2 vectorAngle = flechaP2.ArrowRB.velocity;

            float angle = (Mathf.Atan2(vectorAngle.y, vectorAngle.x) * Mathf.Rad2Deg + 180);
            // Debug.Log("Angulo flecha volando: " + angle);
            flechaP2.ArrowRB.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    void FixedUpdate()
    {
        if (movePlatforms)
        {
            MovePlatform1();
            MovePlatform2();
        }
        if (moveObstacle)
        {
            MoveObstacle();
        }
        if (rotateObstacle)
        {
            RotateObstacle();
        }
    }

    void CheckGameStatus()
    {
        if (hitsP1 >= 3)
        {
            RespawnP1();
            StartCoroutine(FullHealthP1());
        }
        if (hitsP2 >= 3)
        {
            RespawnP2();
            StartCoroutine(FullHealthP2());

        }
        if (gameOver)
        {
            GameOver();
        }
        if (showObstacle)
            obstacle.SetActive(true);
        else
            obstacle.SetActive(false);

        if (moveObstacle)
            MoveObstacle();
        if (rotateObstacle)
            RotateObstacle();
        if (movePlatforms)
            MovePlatforms();
        
    }

    private void MovePlatforms()
    {
        MovePlatform1();
        MovePlatform2();
    }
    private void StopPlatforms()
    {
        StopPlatform1();
        StopPlatform2();
    }

    private void MovePlatform1()
    {
        if (target != null)
        {
            fixedSpeed = 4 * Time.deltaTime;
            player1.transform.position = Vector3.MoveTowards(player1.transform.position, target.position, fixedSpeed);
        }
        if (player1.transform.position == target.position)
        {
            target.position = (target.position == startP1) ? endP1 : startP1;
        }
    }
    private void MovePlatform2()
    {
        if (target2 != null)
        {
            float fixedSpeed = speed * Time.deltaTime;
            player2.transform.position = Vector3.MoveTowards(player2.transform.position, target2.position, fixedSpeed);
        }
        if (player2.transform.position == target2.position)
        {
            target2.position = (target2.position == startP2) ? endP2 : startP2;
        }
    }
    private void StopPlatform1()
    {
        player1.transform.position = Vector3.zero;
    }
    private void StopPlatform2()
    {
        player2.transform.position = Vector3.zero;
    }

    private void RotateObstacle()
    {
        float speedRotation = 30;
        obstacle.transform.Rotate(Vector3.forward * speedRotation * Time.deltaTime);
    }
    private void MoveObstacle()
    {
        float platformSpeed = 4;
        if (targetObstacle != null)
        {

            obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, targetObstacle.position, fixedSpeed);
        }
        if (obstacle.transform.position == targetObstacle.position)
        {
            targetObstacle.position = (target2.position == startP2) ? endO : startO;
        }
    }
    private void StopObstacle()
    {
        obstacle.transform.position = Vector3.zero;
    }
    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("PauseMenu");
    }
    public void RespawnP1()
    {
        player1.transform.position = new Vector2(UnityEngine.Random.Range(startP1.x, endP1.x),
            UnityEngine.Random.Range(startP1.y, endP1.y));
        hitsP1 = 0;
        healthBarP1.transform.localScale = new Vector3(1.01f, 1, 1);
        pointsP2++;
        ptsp2.text = pointsP2 + "";

    }
    public void RespawnP2()
    {
        hitsP2 = 0;
        player2.transform.position = new Vector2(UnityEngine.Random.Range(startP2.x, endP2.x),
             UnityEngine.Random.Range(startP2.y, endP2.y));
        pointsP1++;
        ptsp1.text = pointsP1.ToString();
    }
    public void SetTurnP1(bool isTurnP1)
    {
        if (isTurnP1)
        {
            turnPointerSprite.transform.position = new Vector2(player1.transform.position.x,
                                                                turnPointerSprite.transform.position.y);
        }
        else
        {
            turnPointerSprite.transform.position = new Vector2(player2.transform.position.x - 3,
                                                                turnPointerSprite.transform.position.y);
        }
        this.isTurnP1 = isTurnP1;
        isTurnP2 = !isTurnP1;
    }
    public bool IsTurnP2()
    {
        return isTurnP2;
    }
    public bool IsTurnP1()
    {
        return isTurnP1;
    }
    public void AddWeakHitP2()
    {
        //POR IMPLEMENTAR... FALTA DARLE MÁS PUNTOS A LA VIDA......
        hitsP2++;

        if (hitsP2 == 1)
            healthBarP2.transform.localScale = new Vector3(0.70f, 1, 1);
        else if (hitsP2 == 2)
            healthBarP2.transform.localScale = new Vector3(0.40f, 1, 1);
        else if (hitsP2 >= 3)
            healthBarP2.transform.localScale = new Vector3(0f, 1, 1);
        ptsp2.text = pointsP2 + "";

    }
    public void AddWeakHitP1()
    {
        //POR IMPLEMENTAR... FALTA DARLE MÁS PUNTOS A LA VIDA......
        hitsP1++;
        if (hitsP1 == 1)
            healthBarP1.transform.localScale = new Vector3(0.70f, 1, 1);
        else if (hitsP1 == 2)
            healthBarP1.transform.localScale = new Vector3(0.40f, 1, 1);
        else if (hitsP1 >= 3)
            healthBarP1.transform.localScale = new Vector3(0f, 1, 1);
        ptsp1.text = pointsP1 + "";
    }
    public void AddHitP2()
    {
        hitsP2++;
        StartCoroutine(MoveHitP2());
        if (hitsP2 == 1)
            healthBarP2.transform.localScale = new Vector3(0.70f, 1, 1);
        else if (hitsP2 == 2)
            healthBarP2.transform.localScale = new Vector3(0.40f, 1, 1);
        else if (hitsP2 >= 3)
            healthBarP2.transform.localScale = new Vector3(0f, 1, 1);
        ptsp2.text = pointsP2 + "";

    }
    public void AddHitP1()
    {
        hitsP1++;
        StartCoroutine(MoveHitP1());

        if (hitsP1 == 1)
            healthBarP1.transform.localScale = new Vector3(0.70f, 1, 1);
        else if (hitsP1 == 2)
            healthBarP1.transform.localScale = new Vector3(0.40f, 1, 1);
        else if (hitsP1 >= 3)
            healthBarP1.transform.localScale = new Vector3(0f, 1, 1);
        ptsp1.text = pointsP1 + "";
    }
    public IEnumerator MoveHitP1()
    {
        GameObject cabeza = GameObject.FindGameObjectWithTag("cabeza");
        for(float i = 0; i < 10; i++)
        {
            cabeza.transform.Rotate(cabeza.transform.rotation.x,cabeza.transform.rotation.y,1,Space.Self);
          
            mitadP1.transform.Rotate(
            mitadP1.transform.rotation.x,
            mitadP1.transform.rotation.y,
            2, Space.Self);
            yield return new WaitForSeconds(0.01f);
        }
        for(float i =0; i < 10; i++)
        {
            cabeza.transform.Rotate(cabeza.transform.rotation.x, cabeza.transform.rotation.y, -1, Space.Self);
            mitadP1.transform.Rotate(
           mitadP1.transform.rotation.x,
           mitadP1.transform.rotation.y,
           -2, Space.Self);
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator MoveHitP2()
    {
        GameObject cabeza = GameObject.FindGameObjectWithTag("cabeza2");
        for (float i = 0; i < 10; i++)
        {
            mitadP2.transform.Rotate(
                                    mitadP2.transform.rotation.x,
                                    mitadP2.transform.rotation.y,
                                    -2, Space.Self);
            cabeza.transform.Rotate(cabeza.transform.rotation.x, cabeza.transform.rotation.y, -1, Space.Self);

            yield return new WaitForSeconds(0.01f);
        }
        for (float i = 0; i < 10; i++)
        {
            mitadP2.transform.Rotate(
                                   mitadP2.transform.rotation.x,
                                   mitadP2.transform.rotation.y,
                                   2, Space.Self);
            cabeza.transform.Rotate(cabeza.transform.rotation.x, cabeza.transform.rotation.y, 1, Space.Self);

            yield return new WaitForSeconds(0.01f);
        }
    }
    public void GameOver()
    {
        if (pointsP1 > pointsP2)
            PlayerPrefs.SetInt("winner", 1);
        else
            PlayerPrefs.SetInt("winner", 2);

        PlayerPrefs.SetInt("p1Result", pointsP1);
        PlayerPrefs.SetInt("p2Result", pointsP2);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("GameOverMenu");
    }
    public void HeadshotP1()
    {
        headshotSprite.transform.position = new Vector2(player1.transform.position.x + 1,
                                                    player1.transform.position.y + 1.5f);
        for (float i = 0; i < 1; i = i + 0.001f)
        {
            headshotSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
        }
        hitsP1 += 3;
        StartCoroutine(HideHeadshotP1());
        healthBarP1.transform.localScale = new Vector3(0f, 1, 1);
    }
    private IEnumerator HideHeadshotP1()
    {
        yield return new WaitForSeconds(1);
        for (float i = 1; i >= 0; i = i - 0.001f)
        {
            headshotSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
        }
    }
    public void HeadshotP2()
    {
        headshotSprite.transform.position = new Vector2(player2.transform.position.x - 1,
                                            player2.transform.position.y + 1.5f);
        for (float i = 0; i < 1; i = i + 0.001f)
        {
            headshotSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
        }
        hitsP2 += 3;
        healthBarP2.transform.localScale = new Vector3(0f, 1, 1);
        StartCoroutine(HideHeadshotP2());

    }
    private IEnumerator HideHeadshotP2()
    {
        yield return new WaitForSeconds(1);
        for (float i = 1; i >= 0; i = i - 0.01f)
        {
            headshotSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
        }
    }
    private IEnumerator FullHealthP1()
    {
        yield return new WaitForSeconds(0.9f);
        healthBarP1.transform.localScale = new Vector3(1.01f, 1, 1);
    }
    private IEnumerator FullHealthP2()
    {
        yield return new WaitForSeconds(0.9f);
        healthBarP2.transform.localScale = new Vector3(1.01f, 1, 1);
    }
    private void KillP1()
    {
        Debug.Log("kill p1");
        GameObject.FindGameObjectWithTag("cabeza").GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.FindGameObjectWithTag("cabeza").GetComponent<CapsuleCollider2D>().isTrigger = true;
        GameObject.FindGameObjectWithTag("torsoAlto").GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.FindGameObjectWithTag("torsoAlto").GetComponent<BoxCollider2D>().isTrigger = true;
        GameObject.FindGameObjectWithTag("torsoBajo").GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.FindGameObjectWithTag("torsoBajo").GetComponent<BoxCollider2D>().isTrigger = true;
        GameObject.FindGameObjectWithTag("piernaIzq").GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.FindGameObjectWithTag("piernaIzq").GetComponent<BoxCollider2D>().isTrigger = true;
        GameObject.FindGameObjectWithTag("piernaDer").GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject.FindGameObjectWithTag("piernaDer").GetComponent<BoxCollider2D>().isTrigger = true;
    }
    public void HitEscudoP1()
    {
        EscudoP1.GetComponent<ParticleSystem>().Play();
    }
    public void HitEscudoP2()
    {
        EscudoP2.GetComponent<ParticleSystem>().Play();
    }
    private void InitializeWeapon(bool isP1,string weaponGeneratorTag)
    {
        if (isP1)
        {
           // weaponP1 = GameObject.FindGameObjectWithTag(weaponGeneratorTag);
        }
        else
        {
           
                
            
        }
     
    }
    //BONUS
    public void BonusLifeHit(bool isP1)
    {
        if (isP1)
            FullHealthP1();
        else
            FullHealthP2();

        bonusLife.transform.position.Set(bonusLife.transform.position.x,
                                          22.5f,
                                          bonusLife.transform.position.z
                                        );
    }
    public void BonusExplosionHit(bool isP1)
    {
        float distance = Vector2.Distance(bonusExplosion.transform.position, player1.transform.position);
        if (isP1)         /*****************/
        {                   ///****************
            if (distance < 3) // ----   ----  |
            {                 // |@ |  |@_|   |
                if (distance < 2)//-  oo   -- |
                    AddHitP2();  //|  ___>    |¬¬¬¬¬
                else if (distance < 1)       //   ||
                {              //|############    ||
                    AddHitP2();//|############    ()          |/__
                    AddHitP2();//|############    ============= --
                }    /****%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%*****/
                else/*|***%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%***|*/
                {   /*|***%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%***|*/
                    /*|***%%%%%%%%%%%%%%*/ HeadshotP2(); /*%%%%%%%%%%%%%***|*/
                    /*|***%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%***|*/
                }  /*|***%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%***|*/
                   /*|||||||||||||||||||||||||||||||||||||||||||||||||||||*/
            }      //()()()                                         ()()()||
        }
        else
        { 

        }


        bonusExplosion.transform.position.Set(bonusExplosion.transform.position.x,
                                          22.5f,
                                          bonusExplosion.transform.position.z
                                        );

    }
    private void CheckOptions()
    {
        int volume = PlayerPrefs.GetInt("volume");
    }
    void StartCoundownTimer()
    {
        if (timerText != null)
        {
            time = 180;
            timerText.text = "03:00:00";
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
    }
    void UpdateTimer()
    {
        if (timerText != null)
        {
            time -= Time.deltaTime;
            string minutes = Mathf.Floor(time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");
            string fraction = ((time * 100) % 100).ToString("00");
            timerText.text = minutes + ":" + seconds + ":" + fraction;
            if (minutes == "02" && seconds == "30")
                movePlatforms = true;
            if (minutes == "02" && seconds == "10")
                showObstacle = true;
            if (minutes == "01" && seconds == "50")
                moveObstacle = true;
            if (minutes == "01" && seconds == "15")
                rotateObstacle = true;
            if (minutes == "00" && seconds == "40")
            {
                movePlatforms = true;
                speed *= 1.0199f;
                startP1 = new Vector2(startP1.x, startP1.y + 0.09f);
                startP2 = new Vector2(startP2.x, startP2.y + 0.08f);
                endP1 = new Vector2(endP1.x, endP1.y - 0.05f);
                endP2 = new Vector2(endP2.x, endP2.y - 0.05f);
            }
            if (minutes == "00" && seconds == "00")
                gameOver = true;
        }
    }

}
