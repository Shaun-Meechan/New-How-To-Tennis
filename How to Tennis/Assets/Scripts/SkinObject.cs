﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class SkinObject : ScriptableObject
{
    public string skinName = "Default name";
    public int cost = 50;
    public string description = "Defailt description";
    public int ID = 12345;
    public Material skin;
}
