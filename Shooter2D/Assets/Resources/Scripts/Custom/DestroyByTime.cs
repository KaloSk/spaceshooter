﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
    public float lifeTime = 0.0f;
	void Start () {
        Destroy(gameObject, lifeTime);
    }
}
