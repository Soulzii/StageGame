using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;

public class ProgressManager : MonoBehaviour
{

    [XmlAttribute("lastLevel")]
    public int lastLevel;

    // Use this for initialization
    void Start()
    {
       
    }

}
