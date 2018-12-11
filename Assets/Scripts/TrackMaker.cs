using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TrackMaker : MonoBehaviour {

//[HideInInspector]
    public List<TrackSegment> CurrentTrack;
    public int Difficulty = 1;
    public int MinLengthForward;// o minimo de distancia até o fim
    public int InitialMinLength;// o minimo de distancia da primeira geração
    public int InitialLength;
    private int CurrentLength;
    public TrackSegment emptysegment;

    public List<TrackSegment> SegmentList;

    private List<TrackSegment> EmptyTrack_pool = new List<TrackSegment>();
    private Transform player;

    #region singleton

    public static TrackMaker maker;

    private void Awake()
    {
        maker = this;
    }
    #endregion

    // Use this for initialization
    void Start () {
        player = Player.player.transform;
        for (int i = 0; i < 4; i++)
        {
            TrackSegment seg = Instantiate(emptysegment,Vector3.zero,Quaternion.identity);
            EmptyTrack_pool.Add(seg);
            // GameObject seg =  EmptyTrack_pool[i].gameObject;
            seg.gameObject.SetActive(false);
            //seg.SetActive(false);
        }
        StartCoroutine(SelectTrack(InitialLength,InitialMinLength));
       
        
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.F5))
        {
            ResetAll();
            //SceneManager.LoadScene(0);

        }
        if (Player.player.Active)
        {
            if (player.position.x - CurrentTrack[0].transform.position.x > 30)
            {
                if (CurrentTrack[0].IsEmpty)
                {
                    EmptyTrack_pool.Add(CurrentTrack[0]);
                    CurrentTrack[0].gameObject.SetActive(false);
                }
                else
                    CurrentTrack[0].ResetSegment();
                CurrentTrack.RemoveAt(0);
            }
            if (CurrentTrack[CurrentTrack.Count - 1].transform.position.x - player.position.x < MinLengthForward)
            {
                Debug.Log("Reset");
                StartCoroutine(SelectTrack(CurrentLength, CurrentLength + MinLengthForward));
            }
        }

    }
    IEnumerator SelectTrack(int Initial,int TargetLength)
    {
        int Length = Initial;
        List<TrackSegment> AvailableSegments =new List<TrackSegment>(SegmentList);
        foreach(TrackSegment seg in SegmentList)
        {
            if (seg.InUse)
            {
                AvailableSegments.Remove(seg);
 
            }
        }
        
        Debug.Log("Inicio " + Initial + " Alvo " + TargetLength);
        while(Length<TargetLength && AvailableSegments.Count> 0)
        {
            int ID = Random.Range(0, AvailableSegments.Count);
            
            
            AvailableSegments[ID].transform.position = new Vector2(AvailableSegments[ID].segLength/2 + Length , 0);
            
            Length += (int)AvailableSegments[ID].segLength;
            AvailableSegments[ID].SelectStuff(Difficulty);
            
            //AvailableSegments[ID].gameObject.SetActive(true);
            CurrentTrack.Add(AvailableSegments[ID]);
            AvailableSegments.RemoveAt(ID);
            int gap = Random.Range(0, 4);
            if (gap <3)
            {
                Vector3 pos = new Vector2(emptysegment.segLength / 2 + Length , 0);

                Length += (int)emptysegment.segLength;
                EmptyTrack_pool[0].transform.position = pos;
                EmptyTrack_pool[0].gameObject.SetActive(true);
                CurrentTrack.Add(EmptyTrack_pool[0]);

                EmptyTrack_pool.RemoveAt(0);
                // Instantiate(emptysegment, pos, Quaternion.identity);
            }
            else
                Length +=6;
            yield return null;
        }
        CurrentLength = Length;
        yield return null;
    }
    public void ResetAll()
    {
        while(CurrentTrack.Count >0)
        {
            if (CurrentTrack[0].IsEmpty)
            {
                EmptyTrack_pool.Add(CurrentTrack[0]);
                CurrentTrack[0].gameObject.SetActive(false);
            }
            else
                CurrentTrack[0].ResetSegment();
            CurrentTrack.RemoveAt(0);
        }
        StartCoroutine(SelectTrack(InitialLength, InitialMinLength));

    }

}
