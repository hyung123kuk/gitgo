using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LoadCamera : MonoBehaviour
{
    public List<Transform> Point = new List<Transform>();
   Camera Loadcam;
    
    enum State { WorldMap , Move , End}
    [SerializeField]
    State state;
    public float moveSpeed;
    PostProcessLayer processLayer;

    public Camera EndCamera;
    void Start()
    {
        Point = FindObjectOfType<Gizmo>().Point;
        Loadcam = GetComponent<Camera>();
        processLayer = GetComponent<PostProcessLayer>();
        state = State.WorldMap;



        
    }




    int idx=0;
    public float rotateSpeed;
    public float fieldOfViewSpeed;

    private void Update()
    {
        if (state == State.WorldMap) {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                state = State.Move;
                GetComponent<Animation>().Stop();
                FindObjectOfType<CharacterSelEffect>().PageEffect();
                processLayer.enabled = false;
            }
        }
        else if( state == State.Move)
        {

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {

                PageFlip();
            }

            float dis = Vector3.Distance(transform.position, Point[idx].position);
            Debug.Log(dis);
            if(dis < 3f)
            {
                if(idx<Point.Count-1)
                idx++;
            }
            if(dis < 3f)
            {
                
                if (idx == Point.Count-1)
                    state = State.End;
            }
            

            Vector3 moveVec = (Point[idx].position - transform.position).normalized;


            transform.position = Vector3.Lerp(transform.position, Point[idx].position, Time.deltaTime * moveSpeed);


            Quaternion Rotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, Rotation, Time.deltaTime * rotateSpeed);
           
        }
        else if(state == State.End)
        {
            transform.position = Vector3.Lerp(transform.position, EndCamera.transform.position, Time.deltaTime * moveSpeed * 3);
            transform.rotation = Quaternion.Slerp(transform.rotation, EndCamera.transform.rotation, Time.deltaTime * rotateSpeed);
            Loadcam.fieldOfView = Mathf.Lerp(Loadcam.fieldOfView, EndCamera.fieldOfView, Time.deltaTime* fieldOfViewSpeed);
            if (Vector3.Distance(transform.position, EndCamera.transform.position) < 1f)
            {
                PageFlip();
            }
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
            {
                PageFlip();


            }
        }
    }

    public void PageFlip()
    {
        FindObjectOfType<CharacterSelEffect>().PageEffect();
        Destroy(gameObject);
    }
}
