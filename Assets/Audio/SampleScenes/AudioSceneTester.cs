using System.Collections;
using System.Collections.Generic;
using System.IO;
using TasiYokan.Audio;
using UnityEngine;

public class AudioSceneTester : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //SingleAudio sound = new SingleAudio("BunnyHelp", AudioLayer.Dialogue)
        //    .OnComplete(() =>
        //    {
        //        print("wow, completed!");

        //        new SingleAudio("BunnyHelp", AudioLayer.Dialogue)
        //        .OnComplete(() =>
        //        {
        //            print("wow, completed again!");
        //        })
        //        .SetDelay(2)
        //        .SetLoop(3)
        //        .SetForce(false)
        //        .Play();
        //    })
        //    .SetLoop(1);

        //sound.Play();

        //new RandomAudio("Alarm", AudioLayerType.Bgm)
        //    .SetForce(false)
        //    .SetLoop(3)
        //    .OnEveryComplete(() => print("Finish a loop!"))
        //    .OnComplete(() =>
        //    {
        //        print("Complete 3 random loops");
        //    })
        //    .Play();

        //new SingleAudio("Warning")
        //    .OnEveryComplete(() => print("Finish a loop!"))
        //    .OnComplete(() =>
        //    {
        //        print("say hi after complete");
        //    })
        //    .SetLoop(3)
        //    .Play();

        // Test pause
        //BaseAudio audio = new SingleAudio("Voice_Ringing")
        //    .SetDelay(2f)
        //    .OnStart(() => print("Start from delay"));
        //audio.Play();
        //StartCoroutine(Pause(audio));

        // Test Fade
        //BaseAudio fadedAudio = new SingleAudio("Voice_Ringing").SetLoop(-1);
        //fadedAudio.Play();
        ////fadedAudio.Fade(0, 1, 5);
        //StartCoroutine(YoyoFade(fadedAudio));

        //// Layer pool test
        //StartCoroutine(TestPool());

        //// Test Fade
        //BaseAudio crossFadeAudio = new SingleAudio("Voice_Ringing")
        //    .SetLoop(-1);
        //crossFadeAudio.Play();
        //StartCoroutine(CrossFade(crossFadeAudio));

        new SingleAudio("Voice_Ringing").SetVolume(0.12f)
            .SetLoop(-1)
            .Play();
    }

    IEnumerator Pause(BaseAudio _audio)
    {
        yield return new WaitForSeconds(0.5f);
        print("Pause");
        _audio.Pause();
        yield return new WaitForSeconds(2);
        print("Unpause");
        _audio.Unpause();
    }

    IEnumerator YoyoFade(BaseAudio _audio)
    {
         _audio.Fade(0, 1, 3);

        yield return new WaitForSeconds(3);

        _audio.Fade(1, 0, 3);

        yield return new WaitForSeconds(2);

        _audio.Fade(0, 1, 3);
    }

    IEnumerator CrossFade(BaseAudio _audio)
    {
        yield return new WaitForSeconds(3);

        _audio.SetCrossFade();
        _audio.FeedNewAudioPlayer(AudioManager.Instance.GetAudioClip("Warning"));
        _audio.CrossFade(0, 1, 5);
    }

    IEnumerator TestPool()
    {
        new SingleAudio("Alarm05").Play();
        new SingleAudio("Alarm05").Play();
        new SingleAudio("Alarm05").Play();        
        new SingleAudio("Alarm05").Play();
        yield return new WaitForSeconds(0.5f);

        new SingleAudio("Alarm05").Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
