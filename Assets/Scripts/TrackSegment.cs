using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSegment : MonoBehaviour {
    [System.Serializable]
    public class Settings
    {
        
        public int Diff;
        public List<int> enemiesToActivate;
        public List<int> RockstoActivate;
    }
    public List<Settings> multSettings;
    public float segLength;
    public List<Enemy_man> Enemies;
    public GameObject[] rocks;
    public bool InUse;
    public bool Reset;
    public bool IsEmpty;
    private void Awake()
    {
        if(Reset)
        ResetSegment();
    }
    private void Start()
    {
    }
    public void ResetSegment()
    {
        InUse = false;
        foreach (Enemy_man en in Enemies)
            en.gameObject.SetActive(false);
        foreach (GameObject rock in rocks)
            rock.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SelectStuff(int Difficulty)
    {
        List<Settings> tempsettings = new List<Settings>(multSettings);
        InUse = true;

        for (int i = 0; i< tempsettings.Count;i++)
        {
            if (tempsettings[i].Diff != Difficulty)
            {
                tempsettings.RemoveAt(i);
                i--;
            }

        }
        Debug.Log(tempsettings.Count);
        int ID = Random.Range(0, tempsettings.Count);
        
        ActivateChallenges(tempsettings[ID]);
        gameObject.SetActive(true);

    }
    void ActivateChallenges(Settings Toset)
    {
        foreach (int en in Toset.enemiesToActivate) {
            Enemies[en].resetMan();
            Enemies[en].gameObject.SetActive(true);
            }
        foreach(int en in Toset.RockstoActivate)
        {
            rocks[en].SetActive(true);
        }
    }
    
}
