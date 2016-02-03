using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SaveData{

    public float[] player = new float[6];
    public float[] torkana = new float[6];
    public float[] playerCam = new float[6];
    public float[] mapCam = new float[6];
    public int mission;

    public void StoreMission(int mission)
    {
        this.mission = mission;
    }

    public void StorePlayer(Transform player)
    {
        ParseTransform(this.player, player);
    }

    public void StoreTorkana(Transform torkana)
    {
        ParseTransform(this.torkana, torkana);
    }

    public void StorePlayerCam(Transform playerCam)
    {
        ParseTransform(this.playerCam, playerCam);
    }

    public void StoreMapCam(Transform mapCam)
    {
        ParseTransform(this.mapCam, mapCam);
    }

    public void UpdatePlayer(Transform player)
    {
        UpdateTransform(this.player, player);
    }

    public void UpdateTorkana(Transform torkana)
    {
        UpdateTransform(this.torkana, torkana);
    }

    public void UpdatePlayerCam(Transform playerCam)
    {
        UpdateTransform(this.playerCam, playerCam);
    }

    public void UpdateMapCam(Transform mapCam)
    {
        UpdateTransform(this.mapCam, mapCam);
    }

    private void ParseTransform(float [] a, Transform t)
    {
        a[0] = t.localPosition.x;
        a[1] = t.localPosition.y;
        a[2] = t.localPosition.z;
        a[3] = t.localEulerAngles.x;
        a[4] = t.localEulerAngles.y;
        a[5] = t.localEulerAngles.z;
    }

    private void UpdateTransform(float [] a, Transform t)
    {
        t.localPosition = new Vector3(a[0], a[1], a[2]);
        t.localEulerAngles = new Vector3(a[3], a[4], a[5]);
    }




}
