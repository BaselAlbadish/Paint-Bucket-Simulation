using UnityEngine;
using System;

public class Pendulum : MonoBehaviour
{
    private Vector3 bob1Pos;
    private Vector3 bob2Pos;

    // Total Movement velocity
    public Vector3 TotalVelocity;
    public double TotalVelocityMag;


    // game Objects
    public GameObject bob1;
    public GameObject bob2;
    public GameObject center;
    
    // Dumption
    public float dumption;

    // lengths
    private double r1;
    private double r2;
    private double radius;

    // masses
    public double m1;
    public double m2;

    // angles
    double a1 = Math.PI / 4;
    double a2 = Math.PI / 4;
    double phi = 0;

    // angular accelerations
    double a1_a;
    double a2_a;
    private double Alpha;
    private double AlphaTheta ;

    // accelerations caused by the push Force
    Vector3 aPhi;
    Vector3 aTheta;

    // angular Velocities
    double a1_v;
    double a2_v;
    double W = 0;

    // Push Force 
    private Vector3 pushForce;
    public float force;
    public float forceAngle;

    private Vector3 ThetaAxisPoint;
    private Vector3 forceOnPhiAxis;
    private Vector3 preBob;
    private Vector3 forceOnThetaAxis;

    // Axis's
    private Vector3 ThetaAxis;
    private Vector3 PhiAxis;

    // Robe
    private Vector3[] robesPositions;
    private double[] robesLengths;
    public GameObject[] robes;

    // Temp Vars
    private Vector3 o1;
    private Vector3 o2;
    private Vector3 o3;

    // constants
    double g = 0.1;

    private void Start()
    {
        // main points
        o1 = center.transform.position;
        o2 = bob1.transform.position;
        o3 = bob2.transform.position;

        // initializing
        robesPositions = new Vector3[robes.Length];
        robesLengths = new double[robes.Length];
        for (int i = 0; i < robes.Length; i++)
        {
            robesPositions[i] = robes[i].transform.position;
            robesLengths[i] = Math.Sqrt(Math.Pow(robesPositions[i].x - o1.x, 2) +
                                        Math.Pow(robesPositions[i].y - o1.y, 2) +
                                        Math.Pow(robesPositions[i].z - o1.z, 2));
        }

        bob1Pos = new Vector3();
        bob2Pos = new Vector3();
        preBob = new Vector3();
        ThetaAxis = new Vector3();

        // calculate distances        
        r1 = Math.Sqrt(Math.Pow(o1.x - o2.x, 2) + Math.Pow(o1.y - o2.y, 2) + Math.Pow(o1.z - o2.z, 2));
        r2 = Math.Sqrt(Math.Pow(o2.x - o3.x, 2) + Math.Pow(o3.y - o2.y, 2) + Math.Pow(o3.z - o2.z, 2));
        pushForce = new Vector3(0, 0, force);
        pushForce = Quaternion.AngleAxis(forceAngle, Vector3.up) * pushForce;


        ThetaAxisPoint = new Vector3(o1.x, o1.y - (float) r1, o1.z);
        o1.y = -o1.y;

        
        bob2.transform.position =
            new Vector3((float) (o1.x + r1 * Math.Cos(a1)), (float) (o1.y + r1 * Math.Cos(a1)), o1.z);

        // Calculate Axis
        ThetaAxis = bob2.transform.position - ThetaAxisPoint;
        PhiAxis = new Vector3(0, 0, 1) - new Vector3(0, 0, 0);
        
        // Calculate Projections of the Push Force
        forceOnThetaAxis = Vector3.Project(pushForce, ThetaAxis);
        aTheta = forceOnThetaAxis / (float) (m2);
        print(forceOnThetaAxis);

        forceOnPhiAxis = Vector3.Project(pushForce, PhiAxis);
        aPhi = forceOnPhiAxis / (float) (m2);
        print(forceOnPhiAxis);


        // Debug.DrawLine(pop2.transform.position, pushForce + pop2.transform.position, Color.red, 2.5f);
        // Debug.DrawLine(pop2.transform.position, forceOnThetaAxis  + pop2.transform.position, Color.green, 2.5f);
        // Debug.DrawLine(pop2.transform.position, forceOnPhiAxis + pop2.transform.position , Color.white, 2.5f);
        // Debug.DrawLine(pop2.transform.position, ThetaAxisPoint   , Color.blue, 2.5f);
    }

    private void FixedUpdate()
    {
        
        
        o2 = bob1.transform.position;

        ////// Phi
        radius = r1 * Math.Sin(a1);
        aPhi = aPhi / 3;
        Alpha = aPhi.magnitude / radius;
        W += Alpha;
        phi += W;
        
        ////// Theta
        aTheta = aTheta / 5;
        AlphaTheta = aTheta.magnitude / r1;
        a1_v +=   AlphaTheta;
        
        
        /////// a1_a
        double num1 = -g * (2 * m1 + m2) * Math.Sin(a1);
        double num2 = -m2 * g * Math.Sin(a1 - 2 * a2);
        double num3 = -2 * Math.Sin(a1 - a2) * m2;
        double num4 = a2_v * a2_v * r2 + a1_v * a1_v * r1 * Math.Cos(a1 - a2);
        double den = r1 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2));

        a1_a = (num1 + num2 + num3 * num4) / den;

        /////// a2_a
        num1 = 2 * Math.Sin(a1 - a2);
        num2 = (a1_v * a1_v * r1 * (m1 + m2));
        num3 = g * (m1 + m2) * Math.Cos(a1);
        num4 = a2_v * a2_v * r2 * m2 * Math.Cos(a1 - a2);
        den = r2 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2));

        a2_a = (num1 * (num2 + num3 + num4)) / den;

        //////// Bob1Pos
        var x1 = r1 * Math.Sin(a1) * Math.Cos(phi) + o1.x;
        var y1 = r1 * Math.Cos(a1) + o1.y;
        var z1 = -r1 * Math.Sin(a1) * Math.Sin(phi) + o1.z;
        bob1Pos.x = (float) x1;
        bob1Pos.y = (float) -y1;
        bob1Pos.z = (float) z1;

        //////// Bob2Pos
        var x2 = x1 + r2 * Math.Sin(a2);
        var y2 = y1 + r2 * Math.Cos(a2);
        var z2 = z1;
        bob2Pos.x = (float) x2;
        bob2Pos.y = (float) -y2;
        bob2Pos.z = (float) z2;

        //////// Robe
        for (int i = 0; i < robes.Length; i++)
        {
            robesPositions[i].x = (float) (robesLengths[i] * Math.Sin(a1) * Math.Cos(phi) + o1.x);
            robesPositions[i].y = -(float) (robesLengths[i] * Math.Cos(a1) + o1.y);
            robesPositions[i].z = (float) (-robesLengths[i] * Math.Sin(a1) * Math.Sin(phi) + o1.z);
            robes[i].transform.position = robesPositions[i];
        }

        /////// dumption
        // a1_a *= 0.99;
        // a2_a *=0.99 ;
        
        // velocities
        a1_v += a1_a ;
        a2_v += a2_a;

        a1_v *= 1 - dumption;

        // angles
        a1 += a1_v;
        a2 += a2_v;
      
        // bob1
        bob1.transform.position = bob1Pos;

        // bob2
        bob2.transform.position = bob2Pos;
        bob2.transform.Rotate(0, 0, (float) a2);

        // update axis
        ThetaAxis = bob2.transform.position - ThetaAxisPoint;

        // update Total Velocity Vector
        TotalVelocity = bob2Pos - preBob;
        TotalVelocityMag = TotalVelocity.magnitude;
        
        
        Debug.DrawLine(bob2Pos, bob2Pos + TotalVelocity * 10, Color.yellow, 0.001f);


        preBob = bob2Pos;
    }
}