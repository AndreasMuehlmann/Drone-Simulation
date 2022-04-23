using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotorController : MonoBehaviour
{
    public GameObject drone;

    public GameObject frr;
    public GameObject flr;
    public GameObject brr;
    public GameObject blr;

    public float allRotorsF;
    private float previousAllRotorsF;

    public float frrAndBlrF;
    private float previousFrrAndBlrF;

    public float frrAndFlrF;
    private float previousFrrAndFlrF;

    public float frrF;
    public float flrF;
    public float brrF;
    public float blrF;

    private Vector3 DroneLookDirection;

    private float ax;
    private float ay;
    private float az;

    private Vector3 currentOrientation;


    
    // Start is called before the first frame update
    void Start()
    {
        allRotorsF = 0;
        previousAllRotorsF = 0;

        frrAndBlrF = 0;
        previousFrrAndBlrF = 0;

        frrF = 19.62f;
        brrF = 19.62f;
        flrF = 19.62f;
        blrF = 19.62f;
    }

    // Update is called once per frame
    void Update()
    {
        if (allRotorsF != previousAllRotorsF)
        {
            frrF += allRotorsF;
            brrF += allRotorsF;
            flrF += allRotorsF;
            blrF += allRotorsF;
            frrF -= previousAllRotorsF;
            brrF -= previousAllRotorsF;
            flrF -= previousAllRotorsF;
            blrF -= previousAllRotorsF;
            previousAllRotorsF = allRotorsF;
        }

        if (frrAndBlrF != previousFrrAndBlrF)
        {
            frrF += frrAndBlrF;
            blrF += frrAndBlrF;
            frrF -= previousFrrAndBlrF;
            blrF -= previousFrrAndBlrF;
            previousFrrAndBlrF = frrAndBlrF;
        }

        if (frrAndFlrF != previousFrrAndFlrF)
        {
            frrF += frrAndFlrF;
            flrF += frrAndFlrF;
            frrF -= previousFrrAndFlrF;
            flrF -= previousFrrAndFlrF;
            previousFrrAndFlrF = frrAndFlrF;
        }


        //ax = drone.GetComponent<Transform>().localRotation.eulerAngles.x;
        //ay = drone.GetComponent<Transform>().localRotation.eulerAngles.y;
        //az = drone.GetComponent<Transform>().localRotation.eulerAngles.z;

        //currentOrientation = 0 * new Vector3(Mathf.Sin(ay + Mathf.Atan(Mathf.Cos(az) / Mathf.Cos(ax))),
        //                                 Mathf.Sin(az) * Mathf.Sin(ax),
        //                                 Mathf.Cos(ay + Mathf.Atan(Mathf.Cos(az) / Mathf.Cos(ax))));

        //Kraft - Vector an die Rotoren weitergeben
        frr.GetComponent<ConstantForce>().force = drone.transform.rotation * Vector3.up * frrF;
        brr.GetComponent<ConstantForce>().force = drone.transform.rotation * Vector3.up * brrF;
        blr.GetComponent<ConstantForce>().force = drone.transform.rotation * Vector3.up * blrF;
        flr.GetComponent<ConstantForce>().force = drone.transform.rotation * Vector3.up * flrF;



        //Gegenkraft zum sich drehenden Rotor an Rotoren weitergeben
        frr.GetComponent<ConstantForce>().torque = new Vector3(0, (frrF-19.62f)*10, 0); // + (frrF-19.81f)*10, damit sich Änderungen stärker auswirken
        brr.GetComponent<ConstantForce>().torque = new Vector3(0, (-brrF + 19.62f) * 10, 0); //brr & flr drehen sich anders herum, damit sich die Drohne nicht dreht
        blr.GetComponent<ConstantForce>().torque = new Vector3(0, (blrF - 19.62f) * 10, 0);
        flr.GetComponent<ConstantForce>().torque = new Vector3(0, (-flrF + 19.62f) * 10, 0);

        Debug.Log(drone.transform.rotation * Vector3.up * frrF);
        
    }
}
