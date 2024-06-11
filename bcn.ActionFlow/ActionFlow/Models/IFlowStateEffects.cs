using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ActionFlow.Models
{
    public interface IFlowStateEffects
    {
        public List<IFlowEffectBase> SideEffects { get; }
    }

    public class FlowEffect : IFlowEffectBase
    {
        public FlowAction[] TriggeringActions { get; init; }
        public Func<FlowAction, FlowActionBase> SideEffect { get; init;  }
        public FlowEffect(Func<FlowAction, FlowActionBase> sideEffect, params FlowAction[] triggeringActions)
        {
            TriggeringActions = triggeringActions;
            SideEffect = sideEffect;
        }
        public FlowActionBase[] GetTriggeringActions()
        {
            return TriggeringActions;
        }

        public Func<FlowActionBase, FlowActionBase> GetSideEffect()
        {
            Func<FlowActionBase, FlowActionBase> flowAct = (baseAction) =>
            {
                if (baseAction is FlowAction)
                {
                    return SideEffect((FlowAction)baseAction);
                }
                else
                {
                    return new FlowAction() { Name = "SIDE EFFECT TYPE ERROR!" };
                }
            };

            return flowAct;
        }
    }

    public class FlowEffect<T> : IFlowEffectBase
    {
        public FlowAction<T>[] TriggeringActions { get; init; }
        public Func<FlowAction<T>, FlowActionBase> SideEffect { get; init; }

        public FlowEffect(Func<FlowAction<T>, FlowActionBase> sideEffect, params FlowAction<T>[] triggeringActions)
        {
            TriggeringActions = triggeringActions;
            SideEffect = sideEffect;
        }

        public FlowActionBase[] GetTriggeringActions()
        {
            return TriggeringActions;
        }

        public Func<FlowActionBase, FlowActionBase> GetSideEffect()
        {
            Func<FlowActionBase, FlowActionBase> flowAct = (baseAction) =>
            {
                if (baseAction is FlowAction<T>)
                {
                    return SideEffect((FlowAction<T>)baseAction);
                }
                else
                {
                    return new FlowAction() { Name = "SIDE EFFECT TYPE ERROR!" };
                }
            };

            return flowAct;
        }
    }

    public interface IFlowEffectBase
    {
        
        public FlowActionBase[] GetTriggeringActions();

        public Func<FlowActionBase, FlowActionBase> GetSideEffect();
    }

    public static class FlowEffectUtil
    {

        public static IFlowEffectBase effect<T>(this IFlowStateEffects effects, Func<FlowAction<T>, FlowActionBase> sideEffect, params FlowAction<T>[] triggeringActions)
        {
            return new FlowEffect<T>(sideEffect, triggeringActions);
        }

        public static IFlowEffectBase effect(this IFlowStateEffects effects, Func<FlowAction, FlowActionBase> sideEffect, params FlowAction[] triggeringActions)
        {
            return new FlowEffect(sideEffect, triggeringActions);
        }

    }
}
