//this snippet allows users to send input through microphone in Unity.
//my suggestion is to set sensitivity with a slider so that this will work on many different smartphones.
//this snippet is an updated and enhanced version of a script found somewhere with no copyrights.
//the owner can contact me if he claims the property of this script

bool flag = false;

void MicrophoneInput()
{
    //get mic volume
    int dec = 128;
    float[] waveData = new float[dec];
    int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
    microphoneInput.GetData(waveData, micPosition);

    // Getting a peak on the last 128 samples
    float levelMax = 0;

    for (int i = 0; i < dec; i++)
    {
        float wavePeak = waveData[i] * waveData[i];
        if (levelMax < wavePeak) levelMax = wavePeak;
    }

    float level = Mathf.Sqrt(Mathf.Sqrt(levelMax));

    //debug statements
    if (debugMode)
    {
        //use some ui texts to tune the parameters
        debugText.text = "level:" + level + "\nsensitivity: " + voiceSensitivity;
        if (!flag) debugText2.text = "flag = False";
        if (flag) debugText2.text = "flag = True";
    }

    if (level > voiceSensitivity && !flag)
    {
        doSomething();
        flag = true;
    }

    if (level < voiceSensitivity && flag) flag = false;
}
