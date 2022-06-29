using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rotorController : MonoBehaviour
{
    public GameObject core;

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

    private Vector3 coreLookDirection;

    private float ax;
    private float ay;
    private float az;

    private Vector3 currentOrientation;


    float[] measurements;
    float[] outputs;

    string measurements_path;
    string outputs_path;


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


        measurements = new float[4];
        outputs = new float[4];

        string current_dir = @"C:\Andi_Arbeit\Programmieren\Drone\Drone-Simulation\Assets";
        string path_to_interface = File.ReadAllText(System.IO.Path.Combine(current_dir, "sim_interface_dir_path.txt"));
        string interface_path = System.IO.Path.Combine(path_to_interface, "interface_sim-control");

        measurements_path = System.IO.Path.Combine(interface_path, "measurements.txt");
        outputs_path = System.IO.Path.Combine(interface_path, "outputs.txt");
        Debug.Log(measurements_path);
        Debug.Log(outputs_path);
    }
    
    private void  writeMeasurements()
    {
        try
        {
            using (StreamWriter file = new StreamWriter(measurements_path))
            {
                foreach (double measurement in measurements)
                {
                    file.WriteLine(measurement);
                }
            };
        }
        catch (IOException)
        {
            Debug.Log("retry accessing file at: " + measurements_path);
        }
    }

    private void updateOutputs()
    {
        try
        {
            int i = 0;
            foreach (string line in File.ReadLines(outputs_path))
            {
                outputs[i] = float.Parse(line);
                i += 1;
            }
        }
        catch (IOException)
        {
            Debug.Log("retry accessing file at: " + outputs_path);
        }
    }

    // Update is called once per frame
    void Update()
    {
        measurements[0] = frr.transform.position.y - core.transform.position.y;
        measurements[1] = brr.transform.position.y - core.transform.position.y;
        measurements[2] = blr.transform.position.y - core.transform.position.y;
        measurements[3] = flr.transform.position.y - core.transform.position.y;

        writeMeasurements();
        updateOutputs();

        frrF = outputs[0];
        brrF = outputs[1];
        blrF = outputs[2];
        flrF = outputs[3];


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
        frr.GetComponent<ConstantForce>().force = core.transform.rotation * Vector3.up * frrF;
        brr.GetComponent<ConstantForce>().force = core.transform.rotation * Vector3.up * brrF;
        blr.GetComponent<ConstantForce>().force = core.transform.rotation * Vector3.up * blrF;
        flr.GetComponent<ConstantForce>().force = core.transform.rotation * Vector3.up * flrF;



        //Gegenkraft zum sich drehenden Rotor an Rotoren weitergeben
        frr.GetComponent<ConstantForce>().torque = new Vector3(0, (frrF-19.62f)*10, 0); // + (frrF-19.81f)*10, damit sich Änderungen stärker auswirken
        brr.GetComponent<ConstantForce>().torque = new Vector3(0, (-brrF + 19.62f) * 10, 0); //brr & flr drehen sich anders herum, damit sich die Drohne nicht dreht
        blr.GetComponent<ConstantForce>().torque = new Vector3(0, (blrF - 19.62f) * 10, 0);
        flr.GetComponent<ConstantForce>().torque = new Vector3(0, (-flrF + 19.62f) * 10, 0);

        Debug.Log(core.transform.rotation * Vector3.up * frrF);
    }
}
