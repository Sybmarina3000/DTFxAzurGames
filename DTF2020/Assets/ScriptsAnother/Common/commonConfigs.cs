using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct commonConfigs
{

    public string dirAnimConfig;
    public string dirAnimMixConfig;
    public string dirStatConfig;
    public string dirAttackConfig;
    public string dirColliderConfig;

    public GameObject model;

    public GameObject hpObject;
    public GameObject attackObject;
    public GameObject colliderObject;
    public PhysicsMaterial2D colliderMaterial;
}
