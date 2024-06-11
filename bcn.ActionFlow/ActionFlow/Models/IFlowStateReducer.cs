using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionFlow.Models
{
    public interface IFlowStateReducer<T>
    {
        public T InitialState { get; }

        List<IFlowReductionBase<T>> Reductions { get; }
    }

    public class FlowReduction<I> : IFlowReductionBase<I>
    {
        public FlowAction[] TriggeringActions { get; init; }
        public Func<FlowAction, I,I> Reduction { get; init; }
        public FlowReduction(Func<FlowAction, I, I> reduction, params FlowAction[] triggeringActions)
        {
            TriggeringActions = triggeringActions;
            Reduction = reduction;
        }
        public FlowActionBase[] GetTriggeringActions()
        {
            return TriggeringActions;
        }

        public Func<FlowActionBase,I, I> GetReduction()
        {
            Func<FlowActionBase,I, I> flowAct = (baseAction, currentState) =>
            {
                if (baseAction is FlowAction)
                {
                    return Reduction((FlowAction)baseAction, currentState);
                }
                else
                {
                    return currentState;
                }
            };

            return flowAct;
        }
    }

    public class FlowReduction<T,I> : IFlowReductionBase<I>
    {
        public FlowAction<T>[] TriggeringActions { get; init; }
        public Func<FlowAction<T>, I, I> Reduction { get; init; }
        public FlowReduction(Func<FlowAction<T>, I, I> reduction, params FlowAction<T>[] triggeringActions)
        {
            TriggeringActions = triggeringActions;
            Reduction = reduction;
        }
        public FlowActionBase[] GetTriggeringActions()
        {
            return TriggeringActions;
        }

        public Func<FlowActionBase, I, I> GetReduction()
        {
            Func<FlowActionBase, I, I> flowAct = (baseAction, currentState) =>
            {
                if (baseAction is FlowAction<T>)
                {
                    return Reduction((FlowAction<T>)baseAction, currentState);
                }
                else
                {
                    return currentState;
                }
            };

            return flowAct;
        }
    }


    public interface IFlowReductionBase<I>
    {

        public FlowActionBase[] GetTriggeringActions();

        public Func<FlowActionBase, I, I> GetReduction();
    }

    public static class FlowReductionUtil
    {

        public static IFlowReductionBase<I> reduce<T,I>(this IFlowStateReducer<I> reducer, Func<FlowAction<T>, I, I> reduction, params FlowAction<T>[] triggeringActions)
        {
            return new FlowReduction<T, I>(reduction, triggeringActions);
        }

        public static IFlowReductionBase<I> reduce<I>(this IFlowStateReducer<I> reducer, Func<FlowAction, I, I> reduction, params FlowAction[] triggeringActions)
        {
            return new FlowReduction<I>(reduction, triggeringActions);
        }

    }
}
