namespace NationalInstruments.Examples.NIDigital.MultiInstrumentMatchFailCombination
{
    using System;
    using System.Runtime.InteropServices;
    using Ivi.Driver;

    /// <summary>
    /// Neither an NI Native .NET API nor a .NET Source Code Wrapper API exists for the NI-Sync
    /// driver. For the purposes of the Mutli-Instrument Match Fail Combination example, this simple
    /// wrapper around the NI-Sync C DLL was created to enable just enough NI-Sync functionality to
    /// work with the match fail combination feature of the NI-Digital Pattern Driver API.
    /// </summary>
    internal static class NISyncNativeMethods
    {
        private const string NativeDLL = "niSync.dll";

        /// <summary>
        /// Creates a new NI-Sync instrument driver session.
        /// </summary>
        /// <param name="resourceName">
        /// Specifies the resource name of the module you would like to initialize. You can assign
        /// the resource name of a device in Measurement & Automation Explorer (MAX).
        /// </param>
        /// <param name="idQuery">
        /// Determines whether or not to query the device to determine which device is installed.
        /// </param>
        /// <param name="resetDevice">
        /// Specifies whether you want to reset the NI-Sync module to its default state—including
        /// the external time reference, any connected terminals, and all scheduled future time
        /// events—during the initialization procedure.
        /// </param>
        /// <returns>Returns the instrument handle IntPtr.</returns>
        public static IntPtr Initialize(string resourceName, bool idQuery, bool resetDevice)
        {
            IntPtr session;
            int errorCode = niSync_init(resourceName, Convert.ToUInt16(idQuery), Convert.ToUInt16(resetDevice), out session);

            if (errorCode < 0)
            {
                throw new IviCDriverException("Failed to initialize the NI-Sync session.", errorCode);
            }

            return session;
        }

        [DllImport(NativeDLL, EntryPoint = "niSync_init", CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true, ExactSpelling = true)]
        private static extern int niSync_init(string resourceName, ushort idQuery, ushort resetDevice, out IntPtr newVi);

        /// <summary>
        /// Ends an NI-Sync I/O session and frees the device for other operations.
        /// </summary>
        /// <param name="session">
        /// Specifies the instrument handle that you obtain from <see cref="Initialize(string, bool, bool)"/>.
        /// </param>
        public static void Close(IntPtr session)
        {
            int errorCode = niSync_close(session);

            if (errorCode < 0)
            {
                throw new IviCDriverException("Failed to close the NI-Sync session.", errorCode);
            }
        }

        [DllImport(NativeDLL, EntryPoint = "niSync_close", ExactSpelling = true)]
        private static extern int niSync_close(IntPtr vi);
    }
}