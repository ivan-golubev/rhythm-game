using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{

    [SerializeField] private GameObject gameplayRoot;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private int startDelaySec, spawnDelaySec;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float spawnZcoordinate = 300.0f;
    [SerializeField] private int totalLanes = 4;
    [SerializeField] private string gameItemTag;

    private float spawnYcoordinate, destroyYcoordinate;
    private float[] spawnXCoordinates;
    private bool spawning;        
    private List<GameObject> gameObjects = new List<GameObject>();

    private int score = 0;

	void Start ()
	{
	    RectTransform rt = gameplayRoot.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
	    spawnYcoordinate = rt.rect.height / 2;
	    destroyYcoordinate = -spawnYcoordinate;

	    spawnXCoordinates = new float[totalLanes];
	    float width = rt.rect.width * 0.8f;
        float xStep = width / totalLanes, xCoordinate = -width / 2;

        for (int i = 0; i < totalLanes; i++)
	    {
	        spawnXCoordinates[i] = xCoordinate;
	        xCoordinate += xStep;
	    }

	    spawning = true;
        StartCoroutine(GameLoop());
	}
	
	void Update () {
	    if (spawning)
	    {
	        DetectClicks();
            List<int> gameObjectsToDelete = UpdateYCoordinates();
	        Delete(gameObjectsToDelete);
	        scoreLabel.text = score.ToString("D6");
	    }
	}

    private void DetectClicks()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 500.0f))
        {
            if (hit.collider.tag == gameItemTag && Input.GetMouseButtonDown(0))
            {
                SphereGameItem sphere = hit.collider.gameObject.GetComponent<SphereGameItem>();
                if (!sphere.wasClicked())
                {
                    score++;
                    sphere.onClick();
                }
            }
        }
    }

    private List<int> UpdateYCoordinates()
    {
        List<int> toDelete = new List<int>();
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            Vector3 position = gameObjects[i].transform.position;
            float y = position.y - fallingSpeed * Time.deltaTime;
            gameObjects[i].transform.SetPositionAndRotation(
                new Vector3(position.x, y, position.z),
                Quaternion.identity
            );
            if (y < destroyYcoordinate) { toDelete.Add(i); }
        }
        return toDelete;
    }

    private void Delete(List<int> gameObjectsIxsToDelete)
    {
        foreach (var ix in gameObjectsIxsToDelete)
        {
            GameObject go = gameObjects[ix];
            gameObjects.RemoveAt(ix);
            go.transform.parent = null;
            Destroy(go);
        }
    }

    IEnumerator GameLoop()
    {
        yield return new WaitForSeconds(startDelaySec);
        
        while (spawning)
        {
            yield return new WaitForSeconds(spawnDelaySec);
            var sphere = Instantiate(spherePrefab);
            sphere.gameObject.SetActive(true);
            sphere.transform.SetParent(gameplayRoot.gameObject.transform, false);
            sphere.transform.SetPositionAndRotation(
                new Vector3(spawnXCoordinates[Random.Range(0, totalLanes)], spawnYcoordinate, spawnZcoordinate),
                Quaternion.identity
            );
            gameObjects.Add(sphere);
        }
        yield return null;
    }
}
