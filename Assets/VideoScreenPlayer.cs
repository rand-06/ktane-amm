using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using KeepCoding;
using Random = UnityEngine.Random;

public class VideoScreenPlayer : MonoBehaviour {

	public VideoPlayer Video;
	public VideoClip[] solveVideos;
	private static VideoClip[] externalSolveVideos;
	public KMAudio Audio;
	public AudioClip[] solveAudios;
	public GameObject videoPlayer;
	
	public AudioClip staticAudio;
	public SpriteRenderer spriteRenderer;
	public Sprite[] images;

	private KMAudio.KMAudioRef _audio;
	private bool lockStatic;

	public int lockImageSpam = 0;

	void Awake()
	{
		videoPlayer.SetActive(false);
		spriteRenderer.enabled = false;
		if (!Application.isEditor && externalSolveVideos == null) 
			externalSolveVideos = PathManager.GetAssets<VideoClip>("ammvideo");
		Video.clip = (!Application.isEditor ? externalSolveVideos : solveVideos).Find(x => x.name == "output");
		videoPlayer.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
		StartCoroutine(spamPictures());
	}
	public IEnumerator playRandomVideo(string MissionID, int videoID = -1)
	{
		
		videoPlayer.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1f);
		lockStatic = true;
		int ind;
		if (videoID == -1)
		{
			if (MissionID == "mod_rand06missions_ammNerfed")
				ind = new List<int>
				{
					0, 1, 2, 3, 4, 5, 6, 9, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 25, 28, 29, 31, 32, 33, 34, 36, 37,
					38, 39, 40, 41, 42, 45, 48, 49, 50, 51, 52, 53, 54, 55, 56, 58, 60, 61, 62, 63
				}.PickRandom();
			else ind = Random.Range(0, 64);
		}
		else ind = videoID % 64;
		print("AMM: selected video "+ind);
		Video.clip = (!Application.isEditor ? externalSolveVideos : solveVideos)[ind + 
			(ind >= (!Application.isEditor ? externalSolveVideos : solveVideos).IndexOf(x => x.name == "output")?1:0)
		];
		print("AMM: assigned video " + Video.clip.name);
		Video.Play();
		print("AMM: playing video");
		videoPlayer.SetActive(true);
		print("AMM: video player activated");
		Audio.PlaySoundAtTransform(solveAudios[ind].name, transform);
		print("AMM: playing audio");
		yield return new WaitForSeconds(solveAudios[ind].length);
		print("AMM: finished playing audio");
		videoPlayer.SetActive(false);
		print("AMM: video player deactivated");
	}

	public IEnumerator flashPicture(Sprite image)
	{
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = image;
		yield return new WaitForSeconds(0.07f);
		spriteRenderer.enabled = false;
		spriteRenderer.sprite = null;
	}

	public IEnumerator spamPictures()
	{
		while (true)
			switch (lockImageSpam)
			{
				case 0: yield return new WaitForSeconds(4f); break;
				case 1: yield return new WaitForSeconds(Random.Range(6f,10f));
					if (lockImageSpam > 0) yield return flashPicture(images.PickRandom()); break;
				case 2: yield return new WaitForSeconds(Random.Range(1f,4f));
					if (lockImageSpam > 0) yield return flashPicture(images.PickRandom()); break;
				default: yield break;
			}
	}
	
	public IEnumerator playStatic(float duration)
	{
		if (lockStatic) yield break;
		Video.Play();
		videoPlayer.SetActive(true);
		_audio = Audio.PlaySoundAtTransformWithRef(staticAudio.name, transform);
		yield return new WaitForSeconds(duration);
		_audio.StopSound();
		Video.Stop();
		videoPlayer.SetActive(false);
	}
	
}
