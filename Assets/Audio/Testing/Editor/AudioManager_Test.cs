using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TasiYokan.Audio;
using System.Collections.Generic;

public class AudioManager_Test
{
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator AudioManager_TestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }

    [TearDown]
    public void MapDestroy()
    {
        GameObject.DestroyImmediate(GameObject.FindObjectOfType<AudioManager>());
    }

    [Test]
    public void GetInstance_Inited()
    {
        AudioManager mgr = AudioManager.Instance;

        Assert.IsNotNull(mgr);

        Assert.AreEqual(6, mgr.AllAudioNames.Count);
        Assert.AreEqual(2, mgr.InbuiltAudioPlayers.Count);
    }

    [Test]
    public void GetAudioClip_FirstTime_UpdateAudioDict()
    {
        string clipName = "BunnyHelp";
        AudioClip clip = AudioManager.Instance.GetAudioClip(clipName);

        Assert.IsNotNull(clip);
        Assert.IsNotNull(AudioManager.Instance.AudioDict[clipName]);
    }

    [Test]
    public void GetAudioClips_FirstTime_UpdateAudioDict()
    {
        List<string> clipNames = new List<string>(){ "Random_Jelly_1", "Random_Jelly_2", "Random_Jelly_3", "Random_Jelly_4" };
        List<AudioClip> clips = AudioManager.Instance.GetAudioClips(clipNames);

        Assert.IsTrue(clips.Count == 4);
    }

    [Test]
    public void GetAudioPlayer_Undefined_SpawnNew()
    {
        Assert.AreEqual(0, AudioManager.Instance.RuntimeAudioPlayers.Count);

        AudioPlayer player = AudioManager.Instance.GetAudioPlayer();
        Assert.IsNotNull(player);

        Assert.AreEqual(1, AudioManager.Instance.RuntimeAudioPlayers.Count);
    }

    [Test]
    public void GetAudioPlayer_Undefined_GetExisting()
    {
        AudioPlayer player = AudioManager.Instance.GetAudioPlayer();
        Assert.IsNotNull(player);

        Assert.AreEqual(1, AudioManager.Instance.RuntimeAudioPlayers.Count);

        AudioPlayer anotherPlayer = AudioManager.Instance.GetAudioPlayer();
        Assert.IsNotNull(anotherPlayer);
        Assert.AreEqual(1, AudioManager.Instance.RuntimeAudioPlayers.Count);
    }

    [Test]
    public void GetAudioPlayer_Undefined_GetExistingPlaying_Fail()
    {
        new SingleAudio("BunnyHelp").Play();

        Assert.AreEqual(1, AudioManager.Instance.RuntimeAudioPlayers.Count);

        AudioPlayer anotherPlayer = AudioManager.Instance.GetAudioPlayer();
        Assert.IsNotNull(anotherPlayer);
        Assert.AreEqual(2, AudioManager.Instance.RuntimeAudioPlayers.Count);
    }

    [Test]
    public void GetAudioPlayer_InbuiltLayer_Success()
    {
        Assert.IsNotNull(AudioManager.Instance.GetAudioPlayer(AudioLayer.Bgm));
    }

    [Test]
    public void GetAudioPlayer_RuntimeLayer_Success()
    {
        new SingleAudio("BunnyHelp").Play();

        //Assert.IsNotNull(AudioManager.Instance.GetAudioPlayer());
    }
}