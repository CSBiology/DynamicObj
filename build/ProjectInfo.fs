module ProjectInfo

open Fake.Core

let project = "DynamicObj"

let summary = "F# library supporting Dynamic Objects including inheritance in functional style."

let testProjects = 
    [
        "tests/UnitTests/UnitTests.fsproj"
        "tests/CSharpTests/CSharpTests.csproj"
    ]

let solutionFile  = $"{project}.sln"

let configuration = "Release"

let gitOwner = "CSBiology"

let gitHome = $"https://github.com/{gitOwner}"

let projectRepo = $"https://github.com/{gitOwner}/{project}"

let pkgDir = "pkg"

let release = ReleaseNotes.load "RELEASE_NOTES.md"

let stableVersion = SemVer.parse release.NugetVersion

let stableVersionTag = (sprintf "%i.%i.%i" stableVersion.Major stableVersion.Minor stableVersion.Patch )

let assemblyVersion = $"{stableVersion.Major}.0.0"

let assemblyInformationalVersion = $"{stableVersion.Major}.{stableVersion.Minor}.{stableVersion.Patch}"

let mutable prereleaseSuffix = ""

let mutable prereleaseTag = ""

let mutable isPrerelease = false