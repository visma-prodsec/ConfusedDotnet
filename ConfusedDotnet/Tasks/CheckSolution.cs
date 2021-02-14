using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace ConfusedDotnet.Tasks
{
    [TaskName("CheckSolution")]
    [IsDependentOn(typeof(CheckNugetForPackages))]
    public sealed class CheckSolution : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            if (context.PotentiallyUnsafePackagesReferences.Count > 0)
            {
                foreach (var package in context.PotentiallyUnsafePackagesReferences)
                {
                    context.Log.Warning("Package id: {0} was not found on nuget.org repository", package);
                }
            }
            else
            {
                context.Log.Information("No packages were found that did not already exist on nuget.org, if you know you are consuming dependencies from an internal feed - please check nuget.org for those packages.");
            }
        }
    }
}