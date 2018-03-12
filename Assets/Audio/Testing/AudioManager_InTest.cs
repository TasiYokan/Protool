using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TasiYokan.Audio;

public class AudioManager_InTest {

	[Test]
	public void AudioManager_InTestSimplePasses() {
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator AudioManager_InTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}

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
}
