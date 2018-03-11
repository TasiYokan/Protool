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

        new RandomAudio("Alarm", AudioLayerType.Bgm)
            .SetForce(false)
            .SetLoop(3)
            .OnEveryComplete(() => print("Finish a loop!"))
            .OnComplete(() =>
            {
                print("Complete 3 random loops");
            })
            .Play();

        //new SingleAudio("Warning")
        //    .OnEveryComplete(() => print("Finish a loop!"))
        //    .OnComplete(() =>
        //    {
        //        print("say hi after complete");
        //    })
        //    .SetLoop(3)
        //    .Play();

        // Test pause
        //BaseAudio audio = new SingleAudio("Voice_Ringing");
        //audio.Play();
        //StartCoroutine(Pause(audio));
    }

    IEnumerator Pause(BaseAudio _audio)
    {
        yield return new WaitForSeconds(0.4f);
        _audio.Pause();
        yield return new WaitForSeconds(2);
        _audio.Unpause();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
