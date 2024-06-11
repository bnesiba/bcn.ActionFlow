using SessionStateFlow.package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package
{
    public class FlowStateData<T>: IFlowStateDataCore
    {
        internal delegate void FlowStateReductions(FlowActionBase action, T currentState, out T newState);
        internal FlowStateReductions _flowStateReductions = FlowStateReduce;

        private IFlowStateReducer<T> reducer;
        private T currentState;
        public FlowStateData(IFlowStateReducer<T> reducerDef)
        {
            reducer = reducerDef;
            currentState = reducer.InitialState;

            if (reducer.Reductions.Any())
            {
                foreach (IFlowReductionBase<T> reduction in reducer.Reductions)
                {
                    RegisterReduction(reduction.GetReduction(), reduction.GetTriggeringActions());
                }
            }
            
        }

        public S CurrentState<S>(FlowDataSelector<T, S> selector)
        {
            return selector.selectorFunc(currentState);
        }

        public void FlowReduce(FlowActionBase action)
        {

            System.Diagnostics.Debug.Write($"\nReducing - PreviousState: {currentState}");
            _flowStateReductions(action, this.currentState, out currentState);
            System.Diagnostics.Debug.WriteLine($" NewState: {currentState}");
        }

        private void RegisterReduction(Func<FlowActionBase,T, T> reduction, params FlowActionBase[] actions)
        {
            var reductionWrapper = new ReductionWrapper<T>(reduction, actions);
            _flowStateReductions += reductionWrapper.WrapReduction;
        }

        private class ReductionWrapper<T>
        {
            private Func<FlowActionBase, T, T> _reduction;
            private FlowActionBase[] _flowActions;

            internal ReductionWrapper(Func<FlowActionBase, T, T> reduction, FlowActionBase[] actions)
            {
                _reduction = reduction;
                _flowActions = actions;
            }

            internal void WrapReduction(FlowActionBase flowAction, T currentState, out T newState)
            {
                if (FlowState.onAction(_flowActions)(flowAction))
                {
                    newState = _reduction(flowAction, currentState);
                }
                else
                {
                    newState = currentState;
                }
            }
        }

        private static void FlowStateReduce(FlowActionBase action, T currentState, out T newState)
        {
            newState = currentState;
        }
    }

    public interface IFlowStateDataCore
    {
        public void FlowReduce(FlowActionBase action);
    }
}
