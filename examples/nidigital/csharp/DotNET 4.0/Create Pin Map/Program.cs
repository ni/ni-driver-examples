namespace NationalInstruments.Examples.NIDigital.CreatePinMap
{
    using System;
    using ModularInstruments.NIDigital;

    /// <summary>
    /// Demonstrates how to use the NI-Digital Pattern Driver API to create a pin map.
    /// </summary>
    /// <remarks>
    /// Using this example:
    /// 1. Expand the Settings and Configuration region and familiarize yourself with the settings.
    /// 2. Configure the number of sites to use in your pin map by setting the SiteCount field.
    /// 3. Create system pins by adding pin names and channel mappings to the SystemPinNames and
    ///    SystemPinChannels fields
    /// 4. Create DUT pins by adding pin names and channel mappings to the DutPinNames and
    ///    DutPinChannels fields. If multiple sites for a DUT pin have the same channel mapping,
    ///    this is referred to as a shared pin. With the default values, CS is a Shared Pin.
    /// 5. (Optional) Add PinGroupDefinition objects to the PinGroupDefinitions property. These
    ///    PinGroupDefinitions are used in the program to create pin groups in the pin map. The
    ///    PinGroupDefinition is a simple class defined in this example file that makes adding and
    ///    removing pin group definitions easier.
    /// 6. Build and run the example.
    /// </remarks>
    public sealed class Program
    {
        #region Settings and Configuration

        // IVI resource name of your digital pattern instrument in MAX. Multiple instruments may be added as a comma-separated list.
        private const string ResourceName = "PXI1Slot2,PXI1Slot3";

        // Option String for initializing the session. Simulates the hardware.
        private const string OptionString = "Simulate=1,DriverSetup=Model:6570";

        // Pass an empty OptionString to run on hardware (disable simulation)
        //private const string OptionString = "";

        // Number of sites to use in the pin map.
        private const int SiteCount = 2;

        // System pin configuration.
        private static readonly string[] SystemPinNames = { "Vcc" };

        private static readonly string[] SystemPinChannels = { "PXI1Slot2/3" };

        // DUT pin configuration.
        private static readonly string[] DutPinNames = { "SCL", "SDA", "CS" };

        private static readonly string[] DutPinChannels = { "PXI1Slot2/0", "PXI1Slot2/1", "PXI1Slot2/30", "PXI1Slot3/0", "PXI1Slot3/1", "PXI1Slot2/30" };

        // Pin group settings.
        private static readonly PinGroupDefinition[] PinGroupDefinitions =
        {
            new PinGroupDefinition
            {
                Name = "I2C_bus",
                PinNames = new string[] { "SCL", "SDA", "CS" }
            }
        };

        // Files to load.
        private const string PinLevelsFilePath = "PinLevels.digilevels";

        private const string TimingSheetFilePath = "Timing.digitiming";
        private const string PatternFilePath = "Pattern.digipat";

        // Pattern label for the pattern to burst.
        private const string PatternStartLabel = "new_pattern";

        // Specifying an empty string as the sites to burst causes the pattern to burst on all
        // defined sites.
        private static readonly string SitesToBurst = string.Empty;

        #endregion Settings and Configuration

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. Initializing the digital pattern instrument session.");
                using (NIDigital session = new NIDigital(ResourceName, true, true, OptionString))
                {
                    // Create a pin map with DUT pin names and system pin names.
                    Console.WriteLine("2. Creating the pin map.");
                    session.PinAndChannelMap.CreatePinMap(DutPinNames, SystemPinNames);

                    // Create a channel map to specify the number of sites to use. This method begins
                    // the mapping. To end the mapping you must call EndChannelMap.
                    Console.WriteLine("3. Creating the channel map.");
                    session.PinAndChannelMap.CreateChannelMap(SiteCount);

                    // Map the system pin names in the pin map to channels. The site parameter is
                    // ignored when mapping system pins.
                    Console.WriteLine("4. Mapping system pin names to channels.");
                    for (int i = 0; i < SystemPinNames.Length; ++i)
                    {
                        session.PinAndChannelMap.MapPinToChannel(SystemPinNames[i], default(int), SystemPinChannels[i]);
                    }

                    // Map the DUT pin names in the pin map to channels.
                    Console.WriteLine("5. Mapping DUT pin names to channels.");
                    for (int site = 0; site < SiteCount; ++site)
                    {
                        for (int i = 0; i < DutPinNames.Length; ++i)
                        {
                            session.PinAndChannelMap.MapPinToChannel(DutPinNames[i], site, DutPinChannels[i + (site * DutPinNames.Length)]);
                        }
                    }

                    session.PinAndChannelMap.EndChannelMap();

                    // Create pin groups from pins defined in the pin map.
                    Console.WriteLine("6. Creating pin groups.");
                    foreach (PinGroupDefinition pinGroupDefinition in PinGroupDefinitions)
                    {
                        string groupName = pinGroupDefinition.Name;
                        string[] pinNames = pinGroupDefinition.PinNames;

                        session.PinAndChannelMap.CreatePinGroup(groupName, pinNames);
                    }

                    Console.WriteLine("7. Loading files to configure the session.");
                    ConfigureSession(session);

                    Console.WriteLine("8. Bursting the pattern.");
                    session.PatternControl.BurstPattern(SitesToBurst, PatternStartLabel, true, true, TimeSpan.FromSeconds(10));

                    // Disconnect selected channels using programmable, onboard switching.
                    Console.WriteLine("9. Cleaning up for session closure.");
                    foreach (PinGroupDefinition pinGroupDefinition in PinGroupDefinitions)
                    {
                        session.PinAndChannelMap.GetPinSet(pinGroupDefinition.Name).SelectedFunction = SelectedFunction.Disconnect;
                    }
                }
            }

            // Handle driver-specific exceptions before the general exception handler that follows.
            // An example of a driver-specific exception is Ivi.Driver.IviCDriverException.
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Program complete. Press any key to close.");
                Console.ReadKey();
            }
        }

        private static void ConfigureSession(NIDigital session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session), "You must create a valid NIDigital session before calling ConfigureSession.");
            }

            // Load levels and timing sheets created using the Digital Pattern Editor. These settings
            // are not applied until you call ApplyLevelsAndTiming on the NIDigital session object.
            session.LoadLevels(PinLevelsFilePath);
            session.LoadTiming(TimingSheetFilePath);

            // Apply the loaded sheets to your digital pattern instrument.
            session.ApplyLevelsAndTiming(string.Empty, PinLevelsFilePath, TimingSheetFilePath);

            // Load a pattern file onto your instrument.
            session.LoadPattern(PatternFilePath);
        }
    }

    internal sealed class PinGroupDefinition
    {
        public string Name { get; set; }
        public string[] PinNames { get; set; }
    }
}