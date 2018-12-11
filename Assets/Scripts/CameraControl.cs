using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl: MonoBehaviour {
    Vector3 VelRef;
    Vector3 targetposition;
    public float Miny, Maxy;
    public float MaxDistanceX,MaxDistanceY;
    private Player player;
    public float CameraSpeed;

    #region singleton
    public static CameraControl control;
    private void Awake()
    {
        control = this;
    }
    #endregion

    [HideInInspector]
    public float zaxis;
    // Use this for initialization
    void Start () {
        player = Player.player;
        zaxis = transform.position.z;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 target = new Vector3(MaxDistanceX,MaxDistanceY );
        targetposition = player.transform.position + target;
        targetposition.z = zaxis;
        targetposition.y = Mathf.Clamp(targetposition.y, Miny, Maxy);
        // transform.position = targetposition;
        transform.position = Vector3.SmoothDamp(transform.position, targetposition, ref VelRef, CameraSpeed);
	}
    public void ResetAll()
    {
        transform.position = new Vector3(0, 1, transform.position.z);
    }
}
