using UnityEngine;
using System.Collections;

public class SimpleTimer : MonoBehaviour {

	public float timer = 0;
	public float secondsToAddToTimer =1f;
	
	// Update is called once per frame
	void Update () 
	{
		//Subtract 1 from timer every second 
		timer -= Time.deltaTime;
	
		//If timer is less that 0
		if (timer < 0) 
		{
			//Do Your thing
			YourThingToDo();

			//Add more time to the timer
			timer +=secondsToAddToTimer;
		}
	}

	public void YourThingToDo()
	{
		Debug.Log("This message repeats every second!");
	}
}