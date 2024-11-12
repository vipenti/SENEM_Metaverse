using System;
using System.IO;
using UnityEngine;

public class WAV
{
    public float[] LeftChannel { get; private set; }
    public int SampleCount { get; private set; }
    public int Frequency { get; private set; }

    public WAV(byte[] wav)
    {
        using (MemoryStream stream = new MemoryStream(wav))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            if (wav.Length < 44)
            {
                Debug.LogError("Dati WAV troppo corti per essere un file WAV valido. Lunghezza: " + wav.Length);
                return;
            }

            try
            {
                reader.ReadBytes(22); // Skip to the sample rate
                Frequency = reader.ReadInt32();
                reader.ReadBytes(6); // Skip more header bytes

                int dataChunkSize = reader.ReadInt32();
                SampleCount = dataChunkSize / 2;

                Debug.Log("Frequency: " + Frequency);
                Debug.Log("SampleCount: " + SampleCount);

                if (SampleCount <= 0 || SampleCount > wav.Length)
                {
                    Debug.LogError("Il numero di campioni calcolato è invalido. SampleCount: " + SampleCount);
                    return;
                }

                LeftChannel = new float[SampleCount];

                for (int i = 0; i < SampleCount; i++)
                {
                    if (reader.BaseStream.Position >= reader.BaseStream.Length)
                    {
                        Debug.LogError("Fine del flusso raggiunta inaspettatamente durante la lettura dei dati audio.");
                        break;
                    }

                    short sample = reader.ReadInt16();
                    LeftChannel[i] = sample / 32768.0f; // Normalize 16-bit PCM sample
                }
            }
            catch (EndOfStreamException e)
            {
                Debug.LogError("EndOfStreamException durante la lettura del file WAV: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Errore durante la lettura del file WAV: " + e.Message);
            }
        }
    }
}

