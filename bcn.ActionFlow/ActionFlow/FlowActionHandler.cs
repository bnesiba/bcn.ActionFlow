using ActionFlow.Models;
using System.Text.Json;

namespace ActionFlow
{
    public class FlowActionHandler
    {
        internal delegate void FlowStateActions(FlowActionBase action);
        internal Action<FlowActionBase> flowStateActions = (action) => { System.Diagnostics.Debug.WriteLine($"action: {JsonSerializer.Serialize(action)}\n"); };

        //trigger action
        public void ResolveAction(FlowAction action)
        {
            flowStateActions(action);
        }
        public void ResolveAction<T>(FlowAction<T> action)
        {
            flowStateActions(action);
        }
    }
}
