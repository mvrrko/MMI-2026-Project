using System;

namespace MMI2026.LabEscape.Core
{
    [Serializable]
    public class CommandData
    {
        public GameActionType Action;
        public string TargetId;
        public string RawCommand;
        public ModalityType SourceModality;
        public DateTime TimestampUtc;

        public bool IsDeicticTarget =>
            string.Equals(TargetId, "this", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(TargetId, "there", StringComparison.OrdinalIgnoreCase);
    }
}
