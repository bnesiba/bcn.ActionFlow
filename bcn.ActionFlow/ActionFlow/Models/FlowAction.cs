namespace ActionFlow.Models
{
    public class FlowAction<T> : FlowActionBase
    {
        public required T Parameters { get; init; }
    }

    public class FlowAction : FlowActionBase
    {
    }

    //TODO: restructure action setup to allow empty executions or make them delgates or something
    //to avoid having to make them all nullable or make effects harder to read
    public abstract class FlowActionBase
    {
        public required string Name { get; init; }

        public static FlowActionBase GetBaseActionFromDerived(FlowActionBase action)
        {
            return action;
        }
    }
}
