NI-Digital TMU Arm Types Example

What does this example show?
NIDigital.TmuArmTypes demonstrates the difference between Immediate and Edge arm types for TMU measurements using the NI-Digital Pattern Driver API. Both parts measure the same quantity (duty cycle high) on the same channel using two separate TMUs so the behavioral difference is clearly visible. It shows that Immediate arm can produce negative measurements when start and stop events differ, while Edge arm guarantees deterministic positive results.

What are the prerequisites for running this example?
- NI-Digital Pattern Driver software
- .NET Framework 4.0 or 4.5.
- A compatible digital pattern instrument (PXIe-6571 or similar with TMU support)

Where can I find more information?
<Public Documents>\National Instruments\NI-Digital\Documentation
