using System.Collections.Generic;
using Cake.Core;
using Cake.Frosting;
using JetBrains.Annotations;

namespace ConfusedDotnet
{
    [UsedImplicitly]
    public class BuildContext : FrostingContext
    {
        public bool CheckForPackagesConfig { get; }
        public string SolutionFile { get; }

        public HashSet<string> AllPackageReferences { get; } = new HashSet<string>();
        public HashSet<string> PotentiallyUnsafePackagesReferences { get; } = new HashSet<string>();

        public BuildContext(ICakeContext context)
            : base(context)
        {
            SolutionFile = context.Arguments.GetArgument("solution");
            CheckForPackagesConfig = context.Arguments.HasArgument("checkforpackagesconfig");
        }
    }
}