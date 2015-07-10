using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothRigidbodyRender : MonoBehaviour {
	
	private Transform thisTransform;	// Reference to this transform, i heard its faster than calling .transform every time
	private Rigidbody2D thisRigid;		// Rigidbody, we need it for getting the velocity (for extrapolation)
	
	//position that the rigidbody of the character should have, this is public, so other scripts can grab it,
	//a camera could follow this position without background stutter:
	public Vector3 renderPos = Vector3.zero;		//position that the rigidbody would have in this Update() frame (if other scripts are getting this value use LateUpdate())
	private Vector3 lastOffset = Vector3.zero;	//needed to restore local positions
	
	//all Children that have Renderers, (Note that if this game object has a renderer, that renderer will not appear smooth, move it to a child)
	private List<Transform> renderChilds = new List<Transform>();
	
	void Awake (){
		// Setting up the reference.
		thisTransform = this.transform;
		thisRigid = this.rigidbody2D;
		
		//save all children that have a Sprite Renderer		
		foreach (Transform child in transform){	
			if((child.GetComponent("SpriteRenderer") as SpriteRenderer) != null){renderChilds.Add(child.transform);}
		}
	}	

	private Vector3 previousPosition = Vector3.zero;
	void FixedUpdate (){
		previousPosition = thisTransform.position; //save the Rigidbodys Position
	}
	
	void Update (){
		
		//Choose Inter, or Extrapolation:
		RenderPosInterpolation();
	//	RenderPosExtrapolation();
		
		ApplyOffset();
	}
	
	void LateUpdate(){
		
	}
	
	void RenderPosInterpolation(){
		float lerpFactor = ((Time.time-Time.fixedTime)/Time.fixedDeltaTime);		
		renderPos = Vector3.Lerp(previousPosition, thisTransform.position, lerpFactor);
	}
	
	void RenderPosExtrapolation(){
		float lerpFactor = Time.time-Time.fixedTime;
		Vector2 temp = thisRigid.velocity*lerpFactor;
		renderPos = (thisTransform.position + new Vector3(temp.x, temp.y, 0F));
	}
	
	void ApplyOffset(){
		
		//Adjust the Childs position to compensate the rigidbudy stutter
		Vector3 offset = renderPos - thisTransform.position;
		
	//Quick workaround Version
		if(true){
			for(int i=0; i<renderChilds.Count; i++){
				renderChilds[i].position = renderPos;		//apply current offset
			}
		}else{
	//Normal Version							
			for(int i=0; i<renderChilds.Count; i++){
				renderChilds[i].position += offset -lastOffset;		//apply current offset
			}
			lastOffset = offset; //save current renderPos
		}
		
		//renderpos:green, rigidbody:red, rigidbody+offset=cyan
		Debug.DrawLine(renderPos-Vector3.down*3F,						renderPos-Vector3.up*3F,						Color.green,0F,false);
		Debug.DrawLine(thisTransform.position-Vector3.down*3F,			thisTransform.position-Vector3.up*3F,			Color.red,0F,false);
		Debug.DrawLine(thisTransform.position+offset-Vector3.down*3F,	thisTransform.position+offset-Vector3.up*3F,	Color.cyan,0F,false);
	}
}
