using System;
using System.Linq;
using Cake.Common.IO;
using Cake.Common.Solution;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using NuGet.Packaging;

namespace ConfusedDotnet.Tasks
{
    [TaskName("GatherPackageReferences")]
    public sealed class GatherPackageReferences : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var projectFileParser = new ProjectFileParser(context);

            if (context.SolutionFile != null)
            {
                GatherPackagesFromSolution(context, projectFileParser);
            }
                

            if (context.CheckForPackagesConfig)
            {
                GatherPackagesFromPackagesConfig(context);
            }
        }

        private static void GatherPackagesFromPackagesConfig(BuildContext context)
        {
            var packagesConfigParser = new PackagesConfigParser(context);
            var packagesConfigFilePaths = context.GetFiles("./**/packages.config");

            foreach (var packagesConfigFilePath in packagesConfigFilePaths)
            {
                context.Log.Information(
                    @"Packages.config file:
    Path: {0}",
                    packagesConfigFilePath.FullPath
                );
                
                var parsePackageConfig = packagesConfigParser.ParsePackagesConfig(packagesConfigFilePath);
                
                context.AllPackageReferences.AddRange(parsePackageConfig.Select(x => x.Include));
            }
        }

        private static void GatherPackagesFromSolution(BuildContext context, ProjectFileParser projectFileParser)
        {
            context.Log.Information("Parsing {0}", context.SolutionFile);
            var parsedSolution = context.ParseSolution(context.SolutionFile);
            foreach (var project in parsedSolution.Projects)
            {
                context.Log.Information(
                    @"Solution project file:
    Name: {0}
    Path: {1}
    Id  : {2}
    Type: {3}",
                    project.Name,
                    project.Path,
                    project.Id,
                    project.Type
                );

                if (project.Type == "{2150E333-8FDC-42A3-9474-1A3956D46DE8}")
                {
                    // Solution Folder
                    continue;
                }

                try
                {
                    var parseProject = projectFileParser.ParseProjectFile(project.Path);

                    context.AllPackageReferences.AddRange(parseProject.Select(x => x.Include));
                }
                catch (Exception e)
                {
                    context.Log.Error("Failed to parse a project file, feel free to submit an issue.");
                    context.Log.Error(e.ToString());
                }
            }
        }
    }
}