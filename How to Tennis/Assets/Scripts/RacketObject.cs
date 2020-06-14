using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Racket", menuName ="Racket")]
public class RacketObject : ScriptableObject
{
    public string racketName = "Default name";
    public int cost = 50;
    public string description = "Default description";

    public Mesh racketHandle;
    public Mesh racketNet;
    public Mesh racketPlastic;
}
