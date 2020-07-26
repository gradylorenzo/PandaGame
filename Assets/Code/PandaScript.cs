using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class PandaScript : MonoBehaviour
{
    public string pandaName = "Pumphrey";
    public float thirst = 1000;
    public float hunger = 1000;
    public float health = 60;

    public float eatRate = 1;
    public float drinkRate = 1;
    public float healRate = 1;

    public Canvas meterCanvas;
    public Image thirstMeter;
    public Image hungerMeter;
    public Image healthMeter;

    private float lastTick = 0.0f;
    private bool showMeters = false;

    private bool showingHungerWarning = false;
    private bool showingThirstWarning = false;
    private bool showingHealthWarning = false;

    private Camera mainCamera;

    private NavMeshAgent nma;
    private Vector3 target;

    private void Start()
    {
        mainCamera = Camera.main;
        meterCanvas.enabled = false;
        nma = GetComponent<NavMeshAgent>();
        SetDestination(transform.position);
    }

    private void Update()
    {
        doMeterUpdate();
    }

    private void doMeterUpdate()
    {
        thirstMeter.fillAmount = thirst / 1000.0f;
        hungerMeter.fillAmount = hunger / 1000.0f;
        healthMeter.fillAmount = health / 60.0f;

        meterCanvas.transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position);

        meterCanvas.enabled = ((PandaManager.activePanda == this) || showMeters);
    }

    private void FixedUpdate()
    {
        doTick();
    }

    private void doTick()
    {
        if (lastTick + 1 < Time.time)
        {
            lastTick = Time.time;
            hunger = Mathf.Clamp(hunger - (eatRate / 2), 0, 1000);
            thirst = Mathf.Clamp(thirst - (drinkRate / 2), 0, 1000);

            if(hunger < 1 || thirst < 1)
            {
                health = Mathf.Clamp(health - 1, 0, 60);
            }
            
            if(hunger > 250 && thirst > 250)
            {
                health = Mathf.Clamp(health + healRate, 0, 60);
            }

            if(hunger < 250)
            {
                if (!showingHungerWarning)
                {
                    PandaEvents.pandaHungry(this);
                    showingHungerWarning = true;
                }
            }
            else
            {
                showingHungerWarning = false;
            }

            if (thirst < 250)
            {
                if (!showingThirstWarning)
                {
                    PandaEvents.pandaThirsty(this);
                    showingThirstWarning = true;
                }
            }
            else
            {
                showingThirstWarning = false;
            }

            if (health < 60)
            {
                if (!showingHealthWarning)
                {
                    PandaEvents.pandaHurting(this);
                    showingHealthWarning = true;
                }
            }
            else
            {
                showingHealthWarning = false;
            }
        }
    }

    private void OnMouseOver()
    {
        showMeters = true;
    }

    private void OnMouseExit()
    {
        showMeters = false;
    }

    public void SetDestination(Vector3 t)
    {
        target = t;
        transform.position = transform.position + new Vector3(0, 0.01f, 0);
        nma.SetDestination(target);
    }

    public void eat()
    {
        hunger = Mathf.Clamp(hunger + eatRate, 0, 1000);
    }

    public void drink()
    {
        thirst = Mathf.Clamp(thirst + drinkRate, 0, 1000);
    }
}
