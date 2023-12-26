using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttandManager : MonoBehaviour
{
    static protected AttandManager s_AttandInstance;
    static public AttandManager AttandInstance { get { return s_AttandInstance; } }

    public bool[] attandDay = new bool[31];

    private void Awake()
    {
        s_AttandInstance = this;
    }
    
    
}
