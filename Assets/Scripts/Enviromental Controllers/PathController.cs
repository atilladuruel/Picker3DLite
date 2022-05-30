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
    private bool _activePath = false;
    private bool _collectiblesSpawned = false;
    private List<GameObject> _spawnedCollectibleObjects = new List<GameObject>();
    private string _numberRule = "Number", _shapeRule = "Shape", _colorRule = "Color";
    private float _minXSpawnValue = 10f, _maxXSpawnValue = 120f, _minZSpawnValue = -10f, _maxZSpawnValue = 10f, _minNextXSpawnValue = 2f, _maxNextXSpawnValue = 4f, _minNextZSpawnValue = -4f, _maxNextZSpawnValue = 4f;
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

            int spawningObjectNumber = (int)(_pathsSpecs.GetTargetNumber() * 2.5f);
            float currentSpawningXLocation = _collectibleParentObject.transform.position.x + _minXSpawnValue;
            float currentSpawningZLocation = 0;

            foreach (string rule in _pathsSpecs.GetTarget())
            {
                if (rule == _numberRule)
                {
                    for (int i = 0; i < spawningObjectNumber; i++)
                    {
                        //Generating random X position for object
                        currentSpawningXLocation += Random.Range(_minNextXSpawnValue, _maxNextXSpawnValue);

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


                        var position = new Vector3(currentSpawningXLocation, 0, currentSpawningZLocation);
                        _spawnedCollectibleObjects.Add(Instantiate(_collectibleObjects[0], position, Quaternion.identity, _collectibleParentObject.transform));
                    }
                }

                if (rule == _shapeRule)
                {

                }

                if (rule == _colorRule)
                {

                }
            }

            _collectiblesSpawned = true;
        }
    }

    public void DestroyCollectibles()
    {
        foreach (GameObject temp in _spawnedCollectibleObjects)
        {
            Destroy(temp);
        }
        _spawnedCollectibleObjects.Clear();
    }

    public void ActivateNextPath()
    {
        SetActivePathInfo(false);
        SetCollectibleSpawnedInfo(false);
        _nextPathObject.GetComponent<PathController>().SetActivePathInfo(true);
        DestroyCollectibles();
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

}
