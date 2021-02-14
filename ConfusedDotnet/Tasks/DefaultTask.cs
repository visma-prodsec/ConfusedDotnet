using Cake.Frosting;
using JetBrains.Annotations;

namespace ConfusedDotnet.Tasks
{
    [TaskName("Default")]
    [IsDependentOn(typeof(CheckSolution))]
    [UsedImplicitly]
    public class DefaultTask : FrostingTask
    {
    }
}