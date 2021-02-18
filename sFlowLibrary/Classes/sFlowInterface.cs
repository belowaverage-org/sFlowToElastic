namespace BelowAverage.sFlow
{
    public class sFlowInterface
    {
        public Formats Format = Formats.SingleInterface;
        public DiscardedReasonCodes DiscardReason = DiscardedReasonCodes.NotDiscarded;
        public uint Value = 0;
        public sFlowInterface(uint buffer)
        {
            Format = (Formats)((buffer & 0b11000000000000000000000000000000) >> 30);
            if (Format == Formats.PacketDiscarded)
            {
                DiscardReason = (DiscardedReasonCodes)(buffer & 0b0011111111111111111111111111111);
            }
            Value = buffer & 0b0011111111111111111111111111111;
        }
        public enum Formats : byte
        {
            SingleInterface = 0x0,
            PacketDiscarded = 0x1,
            MultipleInterfaces = 0x2
        }
        public enum DiscardedReasonCodes : uint
        {
            ICMPNetUnreachable = 0,
            ICMPHostUnreachable = 1,
            ICMPProtocolUnreachable = 2,
            ICMPPortUnreachable = 3,
            ICMPFragmentationNeededAndDontFragmentWasSet = 4,
            ICMPSourceRouteFailed = 5,
            ICMPDestinationNetworkUnknown = 6,
            ICMPDestinationHostUnknown = 7,
            ICMPSourceHostIsolated = 8,
            ICMPCommunicationWithDestinationNetworkIsAdministrativelyProhibited = 9,
            ICMPCommunicationWithDestinationHostIsAdministrativelyProhibited = 10,
            ICMPDestinationNetworkUnreachableForTypeOfService = 11,
            ICMPDestinationHostUnreachableForTypeOfService = 12,
            ICMPCommunicationAdministrativelyProhibited = 13,
            ICMPHostPrecedenceViolation = 14,
            ICMPPrecedenceCutoffInEffect = 15,
            Unknown = 256,
            TTLExceeded = 257,
            ACL = 258,
            NoBufferSpace = 259,
            RED = 260,
            TrafficShapingRateLimiting = 261,
            PacketTooBig = 262,
            NotDiscarded = 500
        }
    }
    public static class sFlowInterfaceExtensions
    {
        public static sFlowInterface ToSflowInterface(this uint buffer)
        {
            return new sFlowInterface(buffer);
        }
    }
}