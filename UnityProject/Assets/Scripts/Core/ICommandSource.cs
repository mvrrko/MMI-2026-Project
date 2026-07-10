using System;

namespace MMI2026.LabEscape.Core
{
    public interface ICommandSource
    {
        event Action<CommandData> OnCommand;
        void StartListening();
        void StopListening();
    }
}
