using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionFlow.Models
{
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
