﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static Vector3 TurnLeft(this Vector3 vector){
        var result = Vector3.zero;
        if (vector == Vector3.forward){
            result = -Vector3.right;
        }
        else if(vector == Vector3.right){
            result = Vector3.forward;
        }
        else if(vector == -Vector3.right){
            result = -Vector3.forward;
        }
        else if(vector == -Vector3.forward){
            result = Vector3.right;
        }
        else{
            throw new InvalidOperationException("Current direction is not orthogonal vector");
        }
        return result;
    }

    public static Vector3 TurnRight(this Vector3 vector){
        var result = Vector3.zero;
        if (vector == Vector3.forward){
            result = Vector3.right;
        }
        else if(vector == Vector3.right){
            result = -Vector3.forward;
        }
        else if(vector == -Vector3.right){
            result = Vector3.forward;
        }
        else if(vector == -Vector3.forward){
            result = -Vector3.right;
        }
        else{
            throw new InvalidOperationException("Current direction is not orthogonal vector");
        }
        return result;
    }

    public static IEnumerator Turn(this GameObject obj, Quaternion targetRot, Action callback = null, float rotationSec = 0.1f){
		while(Quaternion.Angle(obj.transform.rotation, targetRot) > 1f){
			float deltaAngle = 90f / rotationSec * Time.deltaTime;
			obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetRot, deltaAngle);
			yield return null;
		}
        if (callback != null){
            callback();
        }
    }

    public static CoroutineContainer WrapAction(this Coroutine self, Action action){
        return new CoroutineContainer{coroutine = self, callback = action};
    }

    public static void StopCoroutine(this CoroutineContainer self){
        if (self is null){
            return;
        }
        self.callback();
    }
}
public class CoroutineContainer{
    public Coroutine coroutine {get;set;} = default;
    public Action callback = default;
}