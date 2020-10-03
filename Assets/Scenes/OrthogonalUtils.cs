using System;
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

    public static IEnumerator Turn(this GameObject obj, Quaternion targetRot, InterruptibleCoroutine.Callback callback = null, float rotationSec = 0.1f){
		while(Quaternion.Angle(obj.transform.rotation, targetRot) > 1f){
			float deltaAngle = 90f / rotationSec * Time.deltaTime;
			obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, targetRot, deltaAngle);
			yield return null;
		}
        if (callback != null){
            callback(true);
        }
    }

    public static InterruptibleCoroutine WrapAction(this Coroutine self, InterruptibleCoroutine.Callback action){
        return new InterruptibleCoroutine{coroutine = self, callback = action};
    }

    public static object StopCoroutine(this InterruptibleCoroutine self, MonoBehaviour mono, bool execute = true){
        if (self is null){
            return null;
        }
        mono.StopCoroutine(self.coroutine);
        return self.callback(execute);
    }
}
public class InterruptibleCoroutine{
    public Coroutine coroutine {get;set;} = default;
    public delegate object Callback(bool execute);
    public Callback callback;
}