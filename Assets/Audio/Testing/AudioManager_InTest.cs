using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TasiYokan.Audio;

public class AudioManager_InTest
{
    [UnityTest]
    public IEnumerator Play_ActualStart_AfterSeconds()
    {
        bool flag = false;
        float delayTime = 0f;

        new SingleAudio("Warning")
            .OnStart(() => flag = true)
            .Play();

        Assert.AreEqual(false, flag);

        yield return new WaitForSeconds(delayTime);
        yield return null;

        Assert.AreEqual(true, flag);
    }

    [UnityTest]
    public IEnumerator SetDelay_ActualStart_AfterSeconds()
    {
        bool flag = false;
        float delayTime = 0.1f;

        new SingleAudio("Warning")
            .SetDelay(delayTime)
            .OnStart(() => flag = true)
            .Play();

        Assert.AreEqual(false, flag);

        yield return new WaitForSeconds(delayTime);
        yield return null;
        yield return null;

        Assert.AreEqual(true, flag);
    }

    [UnityTest]
    public IEnumerator Fade_From0To1_FadeIn()
    {
        SingleAudio audio = new SingleAudio("Warning").SetLoop(-1);
        audio.Play();

        audio.Fade(0, 1, 2);

        Assert.AreEqual(0, audio.AudioPlayer.MainSource.volume.Sgn());

        yield return new WaitForSeconds(2);
        
        Assert.AreEqual(0, (audio.AudioPlayer.MainSource.volume - 1).Sgn());
    }
}
