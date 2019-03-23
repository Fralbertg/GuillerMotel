using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public static ArrowGenerator sharedInstance;

    public Rigidbody2D bow;
    private GameObject firstArrow;
    private GameObject arrow;
    private List<GameObject> arrows;

    public List<GameObject> Arrows { get { return arrows; } }
    public GameObject CurrentArrow { get { return arrows[arrows.Count - 1]; } set { CurrentArrow = value; } }

    void Awake()
    {
        sharedInstance = this;
        firstArrow = GameObject.FindGameObjectWithTag("arrow");
        arrows = new List<GameObject>();
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
        //este if ......
        if(currentArrow == CurrentArrow)
        {
            GameObject newArrow;
            newArrow = Instantiate(currentArrow);
            //le quito FreezePosition y FreezeRotation
            newArrow.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            newArrow.GetComponent<TrailRenderer>().enabled = false;
            //coloco la flecha en el brazo
            newArrow.transform.position = bow.transform.position;
            newArrow.transform.rotation = new Quaternion(
                                                        bow.transform.rotation.x,
                                                        bow.transform.rotation.y,
                                                        bow.transform.rotation.z + 180f,
                                                        bow.transform.rotation.w);
            arrows.Add(newArrow); 
        }
    }
}
