#r "paket:
nuget BlackFox.Fake.BuildTask
nuget Fake.Core.Target
nuget Fake.Core.Process
nuget Fake.Core.ReleaseNotes
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.DotNet.MSBuild
nuget Fake.DotNet.AssemblyInfoFile
nuget Fake.DotNet.Paket
nuget Fake.DotNet.FSFormatting
nuget Fake.DotNet.Fsi
nuget Fake.DotNet.NuGet
nuget Fake.Api.Github
nuget Fake.DotNet.Testing.Expecto 
nuget Fake.Tools.Git //"


#if !FAKE
#load "./.fake/build.fsx/intellisense.fsx"
#r "netstandard" // Temp fix for https://github.com/dotnet/fsharp/issues/5216
#endif

open BlackFox.Fake
open System.IO
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Tools

let configuration = "Release"

[<AutoOpen>]
module MessagePrompts =

    let prompt (msg:string) =
        System.Console.Write(msg)
        System.Console.ReadLine().Trim()
        |> function | "" -> None | s -> Some s
        |> Option.map (fun s -> s.Replace ("\"","\\\""))

    let rec promptYesNo msg =
        match prompt (sprintf "%s [Yn]: " msg) with
        | Some "Y" | Some "y" -> true
        | Some "N" | Some "n" -> false
        | _ -> System.Console.WriteLine("Sorry, invalid answer"); promptYesNo msg

    let releaseMsg = """This will stage all uncommitted changes, push them to the origin and bump the release version to the latest number in the RELEASE_NOTES.md file. 
        Do you want to continue?"""

    let releaseDocsMsg = """This will push the docs to gh-pages. Remember building the docs prior to this. Do you want to continue?"""



let clean = BuildTask.create "Clean" [] {
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ "pkg"
    ++ "bin"
    |> Shell.cleanDirs 
}

let build = BuildTask.create "Build" [clean.IfNeeded] {
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
}

let copyBinaries = BuildTask.create "CopyBinaries" [clean; build] {
    let targets = 
        !! "src/**/*.??proj"
        -- "src/**/*.shproj"
        |>  Seq.map (fun f -> ((Path.getDirectory f) </> "bin" </> configuration, "bin" </> (Path.GetFileNameWithoutExtension f)))
    for i in targets do printfn "%A" i
    targets
    |>  Seq.iter (fun (fromDir, toDir) -> Shell.copyDir toDir fromDir (fun _ -> true))
}


let release = ReleaseNotes.load "RELEASE_NOTES.md"

let stableVersion = SemVer.parse release.NugetVersion

let stableVersionTag = (sprintf "%i.%i.%i" stableVersion.Major stableVersion.Minor stableVersion.Patch )

let mutable prereleaseSuffix = ""

let mutable prereleaseTag = ""

let mutable isPrerelease = false

let pkgDir          = "pkg"


let authors         = "Timo Muehlhaus"
let title           = "DynamicObj"
let owners          = "Timo Muehlhaus"
let description     = "F# library supporting Dynamic Objects including inheritance in functional style."
let licenseUrl      = "https://github.com/CSBiology/DynamicObj/blob/main/LICENSE"
let projectUrl      = "https://github.com/CSBiology/DynamicObj"

let tags            = "F# FSharp dotnet dynamic object"

let repositoryUrl   = "https://github.com/CSBiology/DynamicObj"



let setPrereleaseTag = BuildTask.create "SetPrereleaseTag" [] {
    printfn "Please enter pre-release package suffix"
    let suffix = System.Console.ReadLine()
    prereleaseSuffix <- suffix
    prereleaseTag <- (sprintf "%s-%s" release.NugetVersion suffix)
    isPrerelease <- true
}

let pack = BuildTask.create "Pack" [clean; build; copyBinaries] {
    if promptYesNo (sprintf "creating stable package with version %s OK?" stableVersionTag ) 
        then
            !! "src/**/*.*proj"
            |> Seq.iter (Fake.DotNet.DotNet.pack (fun p ->
                let msBuildParams =
                    {p.MSBuildParams with 
                        Properties = ([
                            "Version",stableVersionTag
                            "PackageReleaseNotes",  (release.Notes |> String.concat "\r\n")
                            
                            "Authors",              authors
                            "Title",                title
                            "Owners",               owners
                            "Description",          description
                            "PackageLicenseUrl",    licenseUrl
                            "PackageProjectUrl",    projectUrl
                            "PackageTags",          tags
                            "RepositoryUrl",        repositoryUrl
                            "RepositoryType",       "git"
                        ] @ p.MSBuildParams.Properties)
                    }
                {
                    p with 
                        MSBuildParams = msBuildParams
                        OutputPath = Some pkgDir
                }
            ))
    else failwith "aborted"
}

let packPrerelease = BuildTask.create "PackPrerelease" [setPrereleaseTag; clean; build; copyBinaries] {
    if promptYesNo (sprintf "package tag will be %s OK?" prereleaseTag )
        then 
            !! "src/**/*.*proj"
            //-- "src/**/Plotly.NET.Interactive.fsproj"
            |> Seq.iter (Fake.DotNet.DotNet.pack (fun p ->
                        let msBuildParams =
                            {p.MSBuildParams with 
                                Properties = ([
                                    "Version", prereleaseTag
                                    "PackageReleaseNotes",  (release.Notes |> String.toLines )
                                ] @ p.MSBuildParams.Properties)
                            }
                        {
                            p with 
                                VersionSuffix = Some prereleaseSuffix
                                OutputPath = Some pkgDir
                                MSBuildParams = msBuildParams
                        }
            ))
    else
        failwith "aborted"
}


let _all = BuildTask.createEmpty "All" [clean; build; pack]

BuildTask.runOrDefaultWithArguments _all
