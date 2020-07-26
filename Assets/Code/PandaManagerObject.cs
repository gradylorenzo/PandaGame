using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PandaManagerObject : MonoBehaviour
{
    private void Awake()
    {
        PandaManager.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PandaManager.changeActivePanda(null);
        }
    }
}

public static class PandaManager
{
    public static PandaScript activePanda = null;

    public static void Initialize()
    {
        PandaEvents.pandaHungry += pandaHungry;
        PandaEvents.pandaThirsty += pandaThirsty;
        PandaEvents.pandaHurting += pandaHurting;
        PandaEvents.changeActivePanda += changeActivePanda;
        PandaEvents.worldPointClicked += worldPointClicked;
    }

    public static void worldPointClicked(Vector3 pos)
    {
        if(activePanda != null)
        {
            Vector3 target = new Vector3(pos.x, 0, pos.z);
            activePanda.GetComponent<NavMeshAgent>().SetDestination(target);
            activePanda = null;
        }
    }

    public static void movePanda(Vector3 pos, bool d)
    {
        if(activePanda != null)
        {
            activePanda.SetDestination(pos, d);
        }
    }

    public static void changeActivePanda(PandaScript ps)
    {
        activePanda = ps;
        if (activePanda != null)
        {
            Debug.Log(activePanda.pandaName + " active");
        }
        else
        {
            Debug.Log("No panda selected");
        }
    }

    public static void pandaHungry(PandaScript ps)
    {

    }

    public static void pandaThirsty(PandaScript ps)
    {

    }

    public static void pandaHurting(PandaScript ps)
    {

    }
}
