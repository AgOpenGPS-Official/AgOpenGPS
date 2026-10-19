using System;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Something that delivers RTCM corrections: an NTRIP connection or a serial radio. The manager
    /// runs one or two of them and decides which one is allowed to pass its data on.
    /// </summary>
    internal interface ICorrectionSource
    {
        event NtripClient.FrameHandler FrameReceived;
        event NtripClient.RawHandler RawDataReceived;
        event Action<NtripStatus> StatusChanged;

        NtripStatus Status { get; }

        /// <summary>Where the corrections come from, for messages: "MOUNT on caster.example.org" or "Radio COM5".</summary>
        string Description { get; }

        /// <summary>Runs until cancelled, or until the source gives up.</summary>
        Task RunAsync(CancellationToken ct);
    }
}
