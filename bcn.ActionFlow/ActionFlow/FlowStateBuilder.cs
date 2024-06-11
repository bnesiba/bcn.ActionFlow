using Microsoft.Extensions.DependencyInjection;
using SessionStateFlow.package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionStateFlow.package
{
    public static class FlowStateBuilder
    {
        //TODO: allow builder methods for scoped or singleton + allow complex scoping
        public static IServiceCollection UseFlowState(this IServiceCollection services)
        {
            services.AddScoped<FlowActionHandler>();
            services.AddScoped<FlowState>();
            return services;
        }

        public static IServiceCollection UseEffects<T>(this IServiceCollection services) 
            where T: class, IFlowStateEffects
        {
             services.AddScoped<IFlowStateEffects, T>();
            return services;
        }

        //TODO: Allow applications without reducers? Currently must have at least one reducer.
        public static IServiceCollection UseReducer<S,T>(this IServiceCollection services) 
            where S : class, IFlowStateReducer<T>
            where T : class
        {
            services.AddScoped<IFlowStateReducer<T>, S>();
            services.AddScoped<FlowStateData<T>>();
            services.AddScoped<IFlowStateDataCore>(sp => sp.GetService<FlowStateData<T>>());

            return services;
        }
    }
}
