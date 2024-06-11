using Newtonsoft.Json;
using ActionFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActionFlow
{
    //TODO: will probably need to break up some of the logic in this file eventually
    public class FlowState
    {
        internal delegate void FlowStateActions(FlowActionBase action);
        internal FlowStateActions _flowStateActions = (action) => { System.Diagnostics.Debug.WriteLine($"action: {JsonConvert.SerializeObject(action.Name)}\n"); };

        public FlowState(IEnumerable<IFlowStateEffects> effects, IEnumerable<IFlowStateDataCore> flowDatas, FlowActionHandler flowActionHandler)
        {
            flowActionHandler.flowStateActions += (action) => _flowStateActions(action);

            foreach (IFlowStateDataCore flowData in flowDatas)
            {
                _flowStateActions += flowData.FlowReduce;
            }

            foreach (IFlowStateEffects effect in effects)
            {
                if (effect.SideEffects.Any())
                {
                    foreach (var sideEffect in effect.SideEffects)
                    {
                        RegisterEffect(sideEffect.GetSideEffect(), sideEffect.GetTriggeringActions());
                    }
                }
            }
        }


        //trigger action
        //TODO: allow actions to be triggered more cleanly from effects (effects must use FlowActionHandler)

        /// <summary>
        /// Cause an action to be resolved. Currently not for use in effect files
        /// NOTE: Call FlowActionHandler.ResolveAction when in effect files
        /// </summary>
        /// <param name="action">action to resolve</param>
        public void ResolveAction(FlowAction action)
        {
            _flowStateActions(action);
        }

        /// <summary>
        /// Cause an action to be resolved. Currently not for use in effect files
        /// NOTE: Call FlowActionHandler.ResolveAction when in effect files
        /// </summary>
        /// <param name="action">action to resolve</param>
        public void ResolveAction<T>(FlowAction<T> action)
        {
            _flowStateActions(action);
        }

        private void ResolveAction(FlowActionBase action)
        {
            _flowStateActions(action);
        }

        //Register various kinds of effects


        internal void RegisterEffect(Func<FlowActionBase, FlowActionBase> effect, params FlowActionBase[] actions)
        {
            foreach (var flowAction in actions)
            {
                _flowStateActions += (action) =>
                {
                    if (onAction(flowAction)(action))
                    {
                        _flowStateActions(effect(action));
                    }

                };
            }
        }


        public static Predicate<FlowActionBase> onAction(params FlowActionBase[] actions)
        {
            return (flowAction) => actions.Any(a => a.Name == flowAction.Name);
        }

        public static bool IsResolvingAction<T>(FlowActionBase currentAction, FlowAction<T> matchingAction, out FlowAction<T> castAction)
        {
            if (onAction(matchingAction)(currentAction))
            {
                castAction = (FlowAction<T>)currentAction;
                return true;
            }

            castAction = matchingAction;
            return false;
        }

        public static FlowAction<T>? GetMatchingActionsResolving<T>(FlowActionBase currentAction, params FlowAction<T>[] matchingActions)
        {
            if (onAction(matchingActions)(currentAction))
            {
                return (FlowAction<T>)currentAction;
            }
            return null;
        }

        public static FlowAction? GetMatchingActionsResolving(FlowActionBase currentAction, params FlowAction[] matchingActions)
        {
            if (onAction(matchingActions)(currentAction))
            {
                return (FlowAction)currentAction;
            }
            return null;
        }

        public static bool MatchingActionsResolving(FlowActionBase currentAction, params FlowActionBase[] matchingActions)
        {
            if (onAction(matchingActions)(currentAction))
            {
                return true;
            }
            return false;
        }

    }
}
