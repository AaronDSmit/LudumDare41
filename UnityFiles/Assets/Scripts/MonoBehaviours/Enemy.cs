using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity {
    protected override void Die () {
        base.Die();

        Debug.Log("Dies");
        Destroy(gameObject);
    }
}
