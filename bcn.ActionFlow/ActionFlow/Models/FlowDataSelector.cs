namespace ActionFlow.Models
{
    //TODO: maybe selectors don't need to exist?
    public class FlowDataSelector<Input, Output>
    {
        internal Func<Input, Output> selectorFunc { get; init; }
        public FlowDataSelector(Func<Input, Output> selector)
        {
            selectorFunc = selector;
        }

    }

    public class FlowDataSelector<Input, Intermediate, Output> : FlowDataSelector<Input, Output>
    {
        public FlowDataSelector(FlowDataSelector<Input, Intermediate> startingSelector, Func<Intermediate, Output> selector) :
            base((input) => selector(startingSelector.selectorFunc(input)))
        {
        }


    }
}
