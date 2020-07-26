using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaCamera : MonoBehaviour
{
    public GameObject indicator;
    public MouseOrbit mo;

    private Transform trackedObject;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && !mo.spinUnlocked)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!hit.collider.gameObject.CompareTag("panda"))
                {
                    Debug.Log(hit.transform.name);
                    Instantiate(indicator, hit.point, transform.rotation);
                    PandaManager.movePanda(hit.point, true);
                    trackedObject = null;
                    mo.target.position = hit.point;
                }
                else
                {
                    PandaManager.changeActivePanda(hit.transform.GetComponent<PandaScript>());
                    trackedObject = hit.transform;
                }
            }
        }

        if(trackedObject != null)
        {
            mo.target.position = trackedObject.position;
        }
    }
}
