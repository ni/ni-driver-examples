using System;
using NationalInstruments.DAQmx;
using System.Diagnostics;

namespace NationalInstruments.Examples
{
    public enum WaveformType {
        SineWave     = 0,
    }

	public class FunctionGenerator
	{
        public FunctionGenerator(
            string desiredFrequency,
            string samplesPerBuffer,
            string cyclesPerBuffer,
            string type,
            string amplitude)
        {
            WaveformType t = new WaveformType();

            if (type == "Sine")
                t = WaveformType.SineWave;
            else
                Debug.Assert(false,"Invalid Waveform Type");

            Init(
                Double.Parse(desiredFrequency),
                Double.Parse(samplesPerBuffer),
                Double.Parse(cyclesPerBuffer),
                t,
                Double.Parse(amplitude));
        }

        public FunctionGenerator(
            double desiredFrequency,
            double samplesPerBuffer,
            double cyclesPerBuffer,
            WaveformType type,
            double amplitude)
        {
            Init(
                desiredFrequency,
                samplesPerBuffer,
                cyclesPerBuffer,
                type,
                amplitude);
        }

        private void Init(
            double desiredFrequency,
            double samplesPerBuffer,
            double cyclesPerBuffer,
            WaveformType type,
            double amplitude)
    {
        if (desiredFrequency <= 0)
            throw new ArgumentOutOfRangeException("desiredFrequency",desiredFrequency,"This parameter must be a positive number");
        if (samplesPerBuffer <= 0)
            throw new ArgumentOutOfRangeException("samplesPerBuffer",samplesPerBuffer,"This parameter must be a positive number");
        if (cyclesPerBuffer <= 0)
            throw new ArgumentOutOfRangeException("cyclesPerBuffer",cyclesPerBuffer,"This parameter must be a positive number");

        _resultingSampleClockRate = (desiredFrequency * samplesPerBuffer) / cyclesPerBuffer;
        _samplesPerCycle = samplesPerBuffer / cyclesPerBuffer;

        // Determine the actual sample clock rate
        _resultingFrequency = _resultingSampleClockRate / (samplesPerBuffer / cyclesPerBuffer);

            switch(type)
            {
                case WaveformType.SineWave:
                    _data = GenerateSineWave(_resultingFrequency, amplitude, _resultingSampleClockRate, samplesPerBuffer);
                    break;
                default:
                    // Invalid type value
                    Debug.Assert(false);
                    break;
            }
        }

        public double[] Data
        {
            get
            {
                return _data;
            }
        }

        public static double[] GenerateSineWave(
            double frequency, 
            double amplitude,
            double sampleClockRate,
            double samplesPerBuffer)
        {
            double deltaT = 1/sampleClockRate; // sec./samp
            int intSamplesPerBuffer = (int)samplesPerBuffer;

            double[] rVal = new double[intSamplesPerBuffer];

            for(int i = 0; i < intSamplesPerBuffer; i++)
                rVal[i] = amplitude * Math.Sin( (2.0 * Math.PI) * frequency * (i*deltaT) );

            return rVal;
        }

        private double[] _data;
        private double _resultingSampleClockRate;
        private double _resultingFrequency;
        private double _samplesPerCycle;
    }
}
