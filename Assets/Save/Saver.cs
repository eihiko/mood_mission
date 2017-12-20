using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Saver : MonoBehaviour {

    public MissionManager missionManager;
    public Transform player;
    public Transform torkana;
    public Transform playerCam;
    public Transform mapCam;
    private SaveData loadData;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FindConnections();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Destroy(GameObject.Find("Save Game Manager"));
            Destroy(GameObject.Find("EventHandler"));
            Destroy(GameObject.Find("User"));
            Destroy(GameObject.Find("MissionManager"));
            Destroy(GameObject.Find("ControllerBody"));
            Destroy(GameObject.Find("Saver"));
            Application.LoadLevel(0);
        }
    }

    private void FindConnections()
    {
        missionManager = GameObject.Find("MissionManager").GetComponent<MissionManager>();
        player = GameObject.Find("ControllerBody").transform;
        torkana = GameObject.Find("Torkana").transform;
        playerCam = Camera.main.transform;
        mapCam = GameObject.Find("MapCamera").transform;
        GetComponent<PauseMenu>().Reset();
        GameObject.Find("EventHandler").GetComponent<EventHandler>().saveManagerObject = gameObject;
    }

    void OnLevelWasLoaded(int level)
    {
        if (null != loadData)
        {
            foreach(Saver s in GameObject.FindObjectsOfType<Saver>())
            {
                if(gameObject != s.gameObject)
                {
                    Destroy(s.gameObject);
                }
            }
            FindConnections();
            Debug.Log("Found loadData!");
            loadData.UpdatePlayer(player);
            loadData.UpdateTorkana(torkana);
			loadData.UpdatePlayerCam(playerCam);
            loadData.UpdateMapCam(mapCam);
            missionManager.StartMission(loadData.mission, loadData.currentEvent);
            loadData = null;
			this.GetComponent<PauseMenu>().setPaused(true);
			Time.timeScale = 0;
			EventHandler handler = GameObject.Find("EventHandler").GetComponent<EventHandler>();
			//handler.currState = GameState.PAUSE;
			handler.updateGame(EventHandler.GameState.PAUSE);
        }
    }

    public void Save(string filename)
    {
        SaveData data = new SaveData();
        data.StorePlayer(player);
        data.StoreTorkana(torkana);
        data.StorePlayerCam(playerCam);
        data.StoreMapCam(mapCam);
        data.mission = missionManager.MissionsCompleted();
		data.currentEvent = (int)missionManager.getCurrentMission().getCurrentMissionEvent ();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Path(filename));
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Saved game at: " + Path(filename));
    }

    public SaveData Load(string filename)
    {
        if (File.Exists(Path(filename)))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Path(filename), FileMode.Open);
            loadData = (SaveData)formatter.Deserialize(file);
            file.Close();
            Debug.Log("Loaded game from: " + Path(filename));
            Destroy(GameObject.Find("Save Game Manager"));
            Destroy(GameObject.Find("EventHandler"));
            Destroy(GameObject.Find("User"));
            Destroy(GameObject.Find("MissionManager"));
            Destroy(GameObject.Find("ControllerBody"));
            this.GetComponent<PauseMenu>().setPaused(false);
            Application.LoadLevel(0);
            return loadData;
        }
        Debug.Log("Could not find savefile: " + Path(filename));
        return null;
    }

    private string Path(string filename)
    {
        return Application.persistentDataPath + "/Saves/" + filename;
    }
}
