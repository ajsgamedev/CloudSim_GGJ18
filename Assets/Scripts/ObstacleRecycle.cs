using UnityEngine;
using System.Collections;

public class ObstacleRecycle : MonoBehaviour {

	float travelDistance = 100f;
	float speed = 20f;
	void OnEnable()
	{
		Invoke ("Destory", 2f);

		StartCoroutine(Move());
	}

	void Destory()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		CancelInvoke ();
		StopAllCoroutines();
	}

	IEnumerator Move()
	{
		float total = 0;
		while(total < travelDistance) {
			total += (speed * Time.deltaTime);
			this.transform.Translate(0f, 0f, (speed * Time.deltaTime), Space.Self);
			//transform.Rotate(Vector3.right, speed * Time.deltaTime);
			yield return 0;
		}
	}
}
