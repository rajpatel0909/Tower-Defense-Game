using UnityEngine;
using System.Collections;

public abstract class DefenseEntity : Entity {

    protected LayerMask offenseLayer = 9;
    public enum Levels { Level1, Level2, Level3, Level4 };
    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
