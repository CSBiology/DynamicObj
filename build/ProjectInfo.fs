module ProjectInfo

open Fake.Core
open Helpers

let project = "DynamicObj"

let summary = "F# library supporting Dynamic Objects including inheritance in functional style."

let testProjects = 
    [
        "tests/DynamicObject.Tests"
        "tests/DynamicObject.Immutable.Tests"
        "tests/CSharpTests"
    ]

let solutionFile  = $"{project}.sln"

let configuration = "Release"

let gitOwner = "CSBiology"

let gitHome = $"https://github.com/{gitOwner}"

let projectRepo = $"https://github.com/{gitOwner}/{project}"

let netPkgDir = "./dist/net"
let npmPkgDir = "./dist/js"
let pyPkgDir = "./dist/py"

let release = ReleaseNotes.load "RELEASE_NOTES.md"

let stableVersion = SemVer.parse release.NugetVersion

let stableVersionTag = (sprintf "%i.%i.%i" stableVersion.Major stableVersion.Minor stableVersion.Patch )

let assemblyVersion = $"{stableVersion.Major}.0.0"

let assemblyInformationalVersion = $"{stableVersion.Major}.{stableVersion.Minor}.{stableVersion.Patch}"

let mutable prereleaseSuffix = PreReleaseFlag.Alpha

let mutable prereleaseSuffixNumber = 0

let mutable isPrerelease = false