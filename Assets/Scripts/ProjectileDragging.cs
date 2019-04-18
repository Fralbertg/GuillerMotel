using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDragging : MonoBehaviour
{
    #region UI
    public LineRenderer line_drag;
    public GameObject circle, circle2;
    #endregion
    public bool IsRightDraggingZone { get { return startPointLine.x > screenCenterX; } }
    public bool TouchHalfRightScreen { get; set; }
    private float screenCenterX,screenCenterY;

    #region vectores
    protected Vector2 endPointLine;
    protected Vector2 startPointLine;
    protected Vector2 mousePosition;
    protected Vector2 direction;
    private Vector2 currentTouch;
    #endregion
    int num = -1;
    void Start()
    {
        screenCenterX = GameObject.FindGameObjectWithTag("MainCamera").transform.position.x + 3f / 2;
    
        Debug.Log("Hello man, projectile drag alive -> " + TouchHalfRightScreen);

    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.touchCount > 0)
        { 
            GameManager.sharedInstance.showObstacle = true;
            // TIENES pocas OPCIONES -> 
            //                      1- Script Grande con 4 circles y 2 line renderer 
            //                      2- Controlas la lista de Input.touches para que no colisiones nuevos valores(nuevos touches) 
            //                          2.1- Lo haces guardando el índice o lo haces guardando el puntero.... 
            //      Conforme está ahora puedes hacer una linea a un lado, luego al otro y entonces si no sueltas 
            //      ningún dedo podrás mover la última línea pero no la primera que ha sido creada.
            //
            Debug.Log("More than 0 in Projecticle Draggggging");
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    startPointLine = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    //así me guardo la posición del touch en la lista y me aseguro de trabajar solo con esa
                    if (num == -1)
                        num = i;
                }
                if (Input.GetTouch(i).phase == TouchPhase.Moved && (TouchHalfRightScreen && IsRightDraggingZone))
                {

                    if (Input.touchCount == 1 && num == 1)
                        num--;
                    endPointLine = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position); 
                    Dragging();
                }
                if (Input.GetTouch(i).phase == TouchPhase.Moved && (!TouchHalfRightScreen && !IsRightDraggingZone))
                {
                    if (Input.touchCount == 1 && num == 1)
                        num--;
                    endPointLine = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position); 
                    Dragging();
                }
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    //reseteo el índice del touch
                    num = -1;
                    ClearLine();
                }
            }
        }
        else
        {
            GameManager.sharedInstance.showObstacle = false;

        }
    }

    private void Dragging()
    {
        line_drag.positionCount = 2;

        circle.transform.position = startPointLine;
        circle2.transform.position = endPointLine;

        circle.gameObject.SetActive(true);
        circle2.gameObject.SetActive(true);

        line_drag.SetPosition(0, startPointLine);
        line_drag.SetPosition(1, endPointLine);
    }

    private void ClearLine()
    {
        line_drag.positionCount = 0;
        circle.gameObject.SetActive(false);
        circle2.gameObject.SetActive(false);
    }
}
