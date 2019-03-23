using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowEnemyGenerator : MonoBehaviour
{
    public static ArrowEnemyGenerator sharedInstance;
    private GameObject firstArrow;
    private GameObject arrow;
    public Rigidbody2D bow;
    private List<GameObject> arrows = new List<GameObject>();
    void Awake()
    {
        sharedInstance = this;
        firstArrow = GameObject.FindGameObjectWithTag("arrow2");
        arrows.Add(firstArrow);
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {

    }
    public void DestroyArrow(GameObject arrow)
    {
        arrows.Remove(arrow);
    }
    public void AddArrow(GameObject currentArrow)
    {
        if (currentArrow == LastArrow())
        {
            GameObject newArrow;
            newArrow = Instantiate(currentArrow);
            //le quito FreezePosition y FreezeRotation
            newArrow.GetComponent<TrailRenderer>().enabled = false;
            newArrow.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            //coloco la flecha en el brazo
            newArrow.transform.position = bow.transform.position;
            newArrow.transform.rotation = bow.transform.rotation;

            arrows.Add(newArrow);
        }
    }
    private GameObject LastArrow()
    {
        return arrows[arrows.Count - 1];
    }
    public List<GameObject> GetArrows()
    {
        return arrows;
    }
}
