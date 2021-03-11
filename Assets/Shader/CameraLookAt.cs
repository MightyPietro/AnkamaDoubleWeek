//	CameraFacing.cs 
//	original by Neil Carter (NCarter)
//	modified by Hayden Scott-Baron (Dock) - http://starfruitgames.com
//  allows specified orientation axis


using UnityEngine;
using System.Collections;

var cameraToLookAt: Camera;
     
    function Update()
{
    var v: Vector3 = cameraToLookAt.transform.position - transform.position;
    v.x = v.z = 0.0;
    transform.LookAt(cameraToLookAt.transform.position - v);
}
