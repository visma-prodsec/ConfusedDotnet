using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cake.Frosting;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace ConfusedDotnet.Tasks
{
    [TaskName("CheckNugetForPackages")]
    [IsDependentOn(typeof(GatherPackageReferences))]
    public sealed class CheckNugetForPackages : AsyncFrostingTask<BuildContext>
    {
        public override async Task RunAsync(BuildContext context)
        {
            var cache = new SourceCacheContext();
            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            foreach (var package in context.AllPackageReferences)
            {
                var versions = await resource.GetAllVersionsAsync(
                    package,
                    cache,
                    NullLogger.Instance,
                    CancellationToken.None);

                if (!versions.Any())
                {
                    context.PotentiallyUnsafePackagesReferences.Add(package);
                }
            }
        }
    }
}