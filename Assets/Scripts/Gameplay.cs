using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{

    [SerializeField] private GameObject gameplayRoot;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private int startDelaySec, spawnDelaySec;

    private bool spawning;

	void Start ()
	{
	    spawning = true;
        StartCoroutine(GameLoop());
	}
	
	void Update () {
		
	}

    IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(startDelaySec);
        Vector3 spawnPosition = new Vector3(0.0f, 0.0f, 300.0f);
        while (spawning)
        {
            yield return new WaitForSeconds(spawnDelaySec);
            var sphere = Instantiate(spherePrefab);
            sphere.gameObject.SetActive(true);
            sphere.transform.SetParent(gameplayRoot.gameObject.transform, false);
            sphere.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        }
        yield return null;
    }
}
