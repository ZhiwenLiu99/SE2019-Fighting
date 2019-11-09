using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using System.IO;

public class MicroInput : MonoBehaviour
{
    public float volume;
    public double freq;

    AudioClip micRecord;

    string device;
    int sampleFreq = 8000;

    // Start is called before the first frame update
    void Start()
    {
        device = Microphone.devices[0];
        micRecord = Microphone.Start(device, true, 999, sampleFreq);
    }

    // Update is called once per frame
    void Update()
    {
        volume = GetMaxVolume();
        if (volume < 0.1)
        {
            freq = 0;
        }
        else
        {
            freq = GetMaxFreq();
        }

    }

    float GetMaxVolume()
    {
        int N = 128;

        float[] volumeData = new float[N];
        int offset = Microphone.GetPosition(device) - N + 1;
        if (offset < 0)
        {
            return 0;
        }

        micRecord.GetData(volumeData, offset);

        /*
        TextWriter tw = new StreamWriter("/Users/world/data1.txt");
        for (int i = 0; i < N; i++)
        {
            tw.Write(volumeData[i]);
            tw.Write(" ");
        }
        tw.Close();
        
        for (int i = 0; i < 128; i++)
        {
            float temp = volumeData[i];

            if (temp > maxVolume)
            {
                maxVolume = temp;
            }
        }
        */

        float meanVolume = 0;
        for (int i = 0; i < N; i++)
        {
            // meanVolume += (float)Math.Pow(Math.Abs(volumeData[i]),1.0/2);
            meanVolume += Math.Abs(volumeData[i]);
        }

        return meanVolume / N;
    }

    double GetMaxFreq()
    {
        double maxFreq = 0;
        double maxFreqValue = 0;
        int sampleFreq = 8000;
        int N = 128;

        double[] spectrumData = new double[N];
        float[] data = new float[N];

        double herzPerBin = (double)sampleFreq / (double)N;

        int offset = Microphone.GetPosition(device) - N + 1;
        if (offset < 0)
        {
            return 0;
        }

        micRecord.GetData(data, offset);

        double[] sourceData = new double[N];
        for (int i = 0; i < N; i++)
        {
            sourceData[i] = (double)data[i];
        }

        FFT(sourceData, spectrumData);

        /*
        TextWriter tw = new StreamWriter("/Users/world/data.txt");
        for(int i = 0; i < N; i++)
        {
            tw.Write(sourceData[i]);
            tw.Write(" ");
        }
        tw.Close();
        */

        //AudioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 0; i < spectrumData.Length / 2; i++)
        {
            double temp_maxFreqValue = spectrumData[i];

            if (temp_maxFreqValue > maxFreqValue)
            {
                maxFreqValue = temp_maxFreqValue;
                maxFreq = i * herzPerBin;
            }
        }

        return maxFreq;
    }

    void FFT(double[] sourceData, double[] spectrumData)
    {
        int N = spectrumData.Length;
        int r = Convert.ToInt32(Math.Log(N, 2));

        Complex[] interVar1 = new Complex[N];
        Complex[] interVar2 = new Complex[N];

        for (int i = 0; i < N; i++)
        {
            interVar1[i] = new Complex(sourceData[i], 0);
        }
        //interVar1 = (Complex[])sourceData.Clone();

        Complex[] w = new Complex[N / 2];

        for (int i = 0; i < N / 2; i++)
        {
            double angle = -i * Math.PI * 2 / N;
            w[i] = new Complex(Math.Cos(angle), Math.Sin(angle));
        }

        for (int i = 0; i < r; i++)
        {
            int interval = 1 << i;

            int halfN = 1 << (r - i);

            for (int j = 0; j < interval; j++)
            {
                int gap = j * halfN;

                for (int k = 0; k < halfN / 2; k++)
                {
                    interVar2[k + gap] = interVar1[k + gap] + interVar1[k + gap + halfN / 2];
                    interVar2[k + halfN / 2 + gap] = (interVar1[k + gap] - interVar1[k + gap + halfN / 2]) * w[k * interval];
                }
            }

            interVar1 = (Complex[])interVar2.Clone();
        }

        for (uint j = 0; j < N; j++)
        {
            uint rev = 0;
            uint num = j;

            for (int i = 0; i < r; i++)
            {
                rev <<= 1;
                rev |= num & 1;
                num >>= 1;
            }
            interVar2[rev] = interVar1[j];
        }

        for (int i = 0; i < N; i++)
        {
            spectrumData[i] = Complex.Abs(interVar2[i]);
        }
    }
}


