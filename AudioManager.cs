using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public int CurrentClip = 0;
    private AudioSource mAudio;
    public List<AudioClip> AudioClips = new List<AudioClip>();

    public bool PlayAudio = true;

    private void Awake()
    {
        mAudio = GetComponent<AudioSource>();

        mAudio.volume = 0.025f;

        mAudio.clip = AudioClips[CurrentClip];
        mAudio.Play();
    }

    public void Pause()
    {
        mAudio.Pause();
    }

    public void Unpause()
    {
        mAudio.UnPause();
    }

    private void Update()
    {
        //if (Network.peerType == NetworkPeerType.Connecting || Network.isServer)
        //{
        //    Debug.Log("Connecting");
        //    PlayAudio = false;
        //    mAudio.mute = true;
        //}
        //if (PlayAudio)
        //    if (mAudio.time > mAudio.clip.length)
        //    {
        //        CurrentClip = CurrentClip >= AudioClips.Count ? 0 : ++CurrentClip;
        //        mAudio.clip = AudioClips[CurrentClip];
        //        mAudio.Play();
        //    }
    }

    public void OnConnectedToServer()
    {
        Pause();
    }

    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        CurrentClip = 0;
        mAudio.clip = AudioClips[CurrentClip];
        mAudio.Play();
    }

    [RPC]
    public void GetTrack(NetworkMessageInfo info)
    {
        GetComponent<NetworkView>().RPC("PlayTrack", info.sender, CurrentClip, mAudio.time);
    }

    [RPC]
    public void PlayTrack(int trackID, float trackTime)
    {
        CurrentClip = trackID;
        mAudio.clip = AudioClips[CurrentClip];
        mAudio.time = trackTime;
        mAudio.UnPause();
        mAudio.Play();
    }
}