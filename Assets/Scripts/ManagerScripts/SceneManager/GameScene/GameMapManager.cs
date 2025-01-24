using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMapManager : MonoSingleton<GameMapManager>
{
    #region INITIALIZE COMPONENTS
    public Grid currentGrid;

    public List<Tilemap> mapList;
    private Dictionary<int, Tilemap> dictionaryMap;

    public Camera cam;

    public GameObject playerPrefab, GatePrefab;

    private float cooldown = 0.0f;

    private bool checkMapLoaded = false;

    private const float delayTime = 0.35f;
    #endregion

    private void Start()
    {
        UnityEngine.Camera.main.fieldOfView = 200f;
        this.SetUpMapDictionary();
    }

    private void Update()
    {
        if (!this.checkMapLoaded && this.cooldown >= delayTime)
        {
            this.checkMapLoaded = true;
            this.Load();
            this.cam.GetCurrentPosition();
        }
        if (!this.checkMapLoaded) this.cooldown += Time.deltaTime;
    }

    private void SetUpMapDictionary()
    {
        this.dictionaryMap = new Dictionary<int, Tilemap>();
        for (int i = 0; i < this.mapList.Count; i++)
        {
            this.dictionaryMap.Add(i + 1, this.mapList[i]);
        }
    }

    private void Load()
    {
        this.LoadGate();

        PlayerScript player = this.LoadPlayer();

        GameManager.Instance.StartGame(player);

        this.cam.GetCurrentPosition();
    }

    private void LoadGate()
    {
        Vector3 gateWorldPos = new Vector3(0.618f, -0.67f, 0f);
        Instantiate(this.GatePrefab, gateWorldPos, Quaternion.identity);
    }

    private PlayerScript LoadPlayer()
    {
        Vector3 StartPosition = new Vector3(0.618f, -0.67f, 0f);
        return Instantiate(playerPrefab, StartPosition, Quaternion.identity).GetComponent<PlayerScript>();
    }
}
