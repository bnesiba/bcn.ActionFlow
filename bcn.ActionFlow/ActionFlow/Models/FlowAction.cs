using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package.Models
{
    public class FlowAction<T> : FlowActionBase
    {
        public required T Parameters { get; init; }
    }

    public class FlowAction : FlowActionBase
    {
    }

    public abstract class FlowActionBase
    {
        public required string Name { get; init; }

        public static FlowActionBase GetBaseActionFromDerived(FlowActionBase action)
        {
            return action;
        }
    }
}
