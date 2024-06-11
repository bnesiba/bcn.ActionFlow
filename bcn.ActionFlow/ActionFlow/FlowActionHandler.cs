using Newtonsoft.Json;
using ActionFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionFlow
{
    public class FlowActionHandler
    {
        internal delegate void FlowStateActions(FlowActionBase action);
        internal Action<FlowActionBase> flowStateActions = (action) => { System.Diagnostics.Debug.WriteLine($"action: {JsonConvert.SerializeObject(action)}\n"); };

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
