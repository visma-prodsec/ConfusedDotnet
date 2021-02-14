# ConfusedDotnet

A tool for checking for lingering free namespaces for private package names referenced in dependency configuration
for Nuget (nuget) `packages.config` or the new PackageReference style.

## What is this all about?

On 9th of February 2021, a security researcher Alex Birsan [published an article](https://medium.com/@alex.birsan/dependency-confusion-4a5d60fec610)
that touched different resolve order flaws in dependency management tools present in multiple programming language ecosystems.

Microsoft [released a whitepaper](https://azure.microsoft.com/en-gb/resources/3-ways-to-mitigate-risk-using-private-package-feeds/)
describing ways to mitigate the impact, while the root cause still remains.

## Interpreting the tool output

`confused` simply reads through a dependency definition file of an application and checks the public package repositories
for each dependency entry in that file. It will proceed to report all the package names that are not found in the public
repositories - a state that implies that a package might be vulnerable to this kind of attack, while this vector has not
yet been exploited.

This however doesn't mean that an application isn't already being actively exploited. If you know your software is using
private package repositories, you should ensure that the namespaces for your private packages have been claimed by a
trusted party (typically yourself or your company).

## Installation

dotnet sdk is a prerequisite for this tool to work. You can download it here: https://dotnet.microsoft.com/download

- git clone https://github.com/visma-prodsec/ConfusedDotnet

## Usage
```
Usage:
 ./confused.ps1 [-w C:\example\directory] [--checkforpackagesconfig] [--solution C:\example\directory\Example.sln]

Usage of ./confused.ps1:
  -w string
        Working directory, this is used for globbing for packages.config files, only needed in conjunction with --checkforpackagesconfig, if not set your current working directory will be used
  --checkforpackagesconfig
        Indicates that the working directory should be globbed for packages.config files
  --solution string
        The .sln file that should be scanned for project files, those project files are then scanned for PackagesReferences

```

## Example
```powershell
.\confused.ps1 -w C:\example\directory\ --solution C:\example\directory\Example.sln --checkforpackagesconfig
```

```
========================================
CheckSolution
========================================
Package id: Example.Internal.Nuget was not found on nuget.org repository
Package id: Example.VeryInternal.Nuget was not found on nuget.org repository
Package id: Example.SuperInternal.Nuget was not found on nuget.org repository

========================================
Default
========================================

Task                          Duration
--------------------------------------------------
GatherPackageReferences       00:00:00.9973370
CheckNugetForPackages         00:01:08.6440376
CheckSolution                 00:00:00.0141526
--------------------------------------------------
Total:                        00:01:09.6577472
```
