using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    //This class controling spec of path, collectible spawn and loop of paths.
    [SerializeField] private PathsSpecs _pathsSpecs;
    [SerializeField] private GameObject _nextPathObject;
    [SerializeField] private GameObject _previousPathObject;
    [SerializeField] private List<GameObject> _collectibleObjects;
    [SerializeField] private GameObject _collectibleParentObject;
    [SerializeField] private GameObject _lastCollectiblePointObject;
    [SerializeField] private GameObject _startPointObject;
    [SerializeField] private GameObject _checkPointObject;
    private bool _activePath = false;
    private bool _collectiblesSpawned = false;
    private List<GameObject> _spawnedCollectibleObjects = new List<GameObject>();
    private string _numberRule = "Number", _shapeRule = "Shape", _colorRule = "Color";
    private float _minZSpawnValue = -10f, _maxZSpawnValue = 10f, _minNextZSpawnValue = -4f, _maxNextZSpawnValue = 4f;
    private float _basketBasePosition;

    public PathsSpecs GetPathSpecs() { return _pathsSpecs; }

    private void Start()
    {
        _activePath = _pathsSpecs.GetFirsPathInfo();

        #region collor assign

        gameObject.GetComponent<Renderer>().material.color = _pathsSpecs.GetGroundColor();
        transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = _pathsSpecs.GetBarrierColor();
        transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = _pathsSpecs.GetBarrierColor();
        transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color = _pathsSpecs.GetBasketColor();

        #endregion

        _basketBasePosition = transform.GetChild(2).gameObject.transform.position.y;
    }

    private void Update()
    {
        if (_activePath && !_collectiblesSpawned)
        {
            //When path activated and collectible not spwaned yet, this will be spawn collectibles according to path level and target.

            transform.GetChild(2).gameObject.GetComponent<Renderer>().material.color = _pathsSpecs.GetBasketColor();
            transform.GetChild(2).gameObject.transform.position = new Vector3(transform.GetChild(2).gameObject.transform.position.x, _basketBasePosition, transform.GetChild(2).gameObject.transform.position.z);

            int spawningObjectNumber = (int)(_pathsSpecs.GetTargetNumber() * _pathsSpecs.GetTargetMultiplayer());
            float currentSpawningXLocation = _collectibleParentObject.transform.position.x;
            float nextXSpawnValue = Mathf.Abs(currentSpawningXLocation - _lastCollectiblePointObject.transform.position.x) / spawningObjectNumber;
            float currentSpawningZLocation = 0;

            for (int i = 0; i < spawningObjectNumber; i++)
            {
                //Generating random X position for object
                currentSpawningXLocation += nextXSpawnValue;
                int objectShapeNumber = 0;

                //Generating random Z position for object
                if (_spawnedCollectibleObjects == null || _spawnedCollectibleObjects.Count == 0)
                {
                    currentSpawningZLocation += Random.Range(_minNextZSpawnValue, _maxNextZSpawnValue);
                }
                else
                {
                    do
                    {
                        currentSpawningZLocation = _spawnedCollectibleObjects[_spawnedCollectibleObjects.Count - 1].transform.position.z + Random.Range(_minNextZSpawnValue, _maxNextZSpawnValue);
                    } while (currentSpawningZLocation < _minZSpawnValue || currentSpawningZLocation > _maxZSpawnValue);
                }

                if (_pathsSpecs.GetTarget().Contains(_shapeRule))
                {
                    objectShapeNumber = Random.Range(0, _collectibleObjects.Count - 1);
                }
                else
                    objectShapeNumber = 0;


                var position = new Vector3(currentSpawningXLocation, 0, currentSpawningZLocation);
                _spawnedCollectibleObjects.Add(Instantiate(_collectibleObjects[objectShapeNumber], position, Quaternion.identity, _collectibleParentObject.transform));


                if (_pathsSpecs.GetTarget().Contains(_colorRule))
                {
                    if (_spawnedCollectibleObjects[_spawnedCollectibleObjects.Count - 1].GetComponent<ColletableController>() != null)
                    {
                        _spawnedCollectibleObjects[_spawnedCollectibleObjects.Count - 1].GetComponent<ColletableController>().ChangeColor(new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)));
                    }
                    else
                    {
                        Debug.Log("Collectible null!");
                    }
                }
            }





            SetCollectibleSpawnedInfo(true);
        }
    }

    public void DestroyCollectibles()
    {
        foreach (GameObject temp in _spawnedCollectibleObjects)
        {
            Destroy(temp);
        }
        _spawnedCollectibleObjects.Clear();

        SetCollectibleSpawnedInfo(false);
    }

    public void ActivateNextPath()
    {
        SetActivePathInfo(false);
        _nextPathObject.GetComponent<PathController>().SetActivePathInfo(true);
        DestroyCollectibles();
        ActivateCheckPoint(false);
        _nextPathObject.GetComponent<PathController>().ActivateCheckPoint(true);
        float verticalPathSize = _nextPathObject.transform.position.x - transform.position.x;
        _previousPathObject.transform.position = new Vector3(_nextPathObject.transform.position.x + verticalPathSize, _nextPathObject.transform.position.y, _nextPathObject.transform.position.z);
    }


    public bool GetActivePathInfo()
    {
        return _activePath;
    }

    public void SetActivePathInfo(bool on)
    {
        _activePath = on;
    }

    public bool GetCollectibleSpawnedInfo()
    {
        return _collectiblesSpawned;
    }

    public void SetCollectibleSpawnedInfo(bool on)
    {
        _collectiblesSpawned = on;
    }

    public void ActivateCheckPoint(bool on)
    {
        _checkPointObject.SetActive(on);
    }

}
