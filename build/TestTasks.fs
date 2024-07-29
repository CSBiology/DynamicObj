module TestTasks

open BlackFox.Fake
open Fake.DotNet

open ProjectInfo
open BasicTasks
open Fake.Core

module RunTests = 


    //let runTestsJsNative = BuildTask.create "runTestsJSNative" [clean; build] {
    //    Trace.traceImportant "Start native JavaScript tests"
    //    for path in ProjectInfo.jsTestProjects do
    //        // transpile library for native access
    //        run dotnet $"fable src/ARCtrl -o {path}/ARCtrl" ""
    //        GenerateIndexJs.ARCtrl_generate($"{path}/ARCtrl")
    //        run npx $"mocha {path} --timeout 20000" "" 
    //}

    let runTestsJs = BuildTask.create "runTestsJS" [clean; build] {
        for path in ProjectInfo.testProjects do
            // transpile js files from fsharp code
            run dotnet $"fable {path} -o {path}/js" ""
            // run mocha in target path to execute tests
            // "--timeout 20000" is used, because json schema validation takes a bit of time.
            run node $"{path}/js/Main.js" ""
    }

    //let runTestsPyNative = BuildTask.create "runTestsPyNative" [clean; build] {
    //    Trace.traceImportant "Start native Python tests"
    //    for path in ProjectInfo.pyTestProjects do
    //        // transpile library for native access
    //        run dotnet $"fable src/ARCtrl -o {path}/ARCtrl --lang python" ""
    //        GenerateIndexPy.ARCtrl_generate($"{path}/ARCtrl")
    //        run python $"-m pytest {path}" "" 
    //}

    let runTestsPy = BuildTask.create "runTestsPy" [clean; build] {
        for path in ProjectInfo.testProjects do
            //transpile py files from fsharp code
            run dotnet $"fable {path} -o {path}/py --lang python" ""
            // run pyxpecto in target path to execute tests in python
            run python $"{path}/py/main.py" ""
    }

    let runTestsDotnet = BuildTask.create "runTestsDotnet" [clean; build] {
        testProjects
        |> Seq.iter (fun testProject ->
            Fake.DotNet.DotNet.test(fun testParams ->
                {
                    testParams with
                        Logger = Some "console;verbosity=detailed"
                        Configuration = DotNet.BuildConfiguration.fromString configuration
                        NoBuild = true
                }
            ) testProject
        )
    }

let runTests = BuildTask.create "RunTests" [clean; build; RunTests.runTestsJs; (*RunTests.runTestsJsNative; *)RunTests.runTestsPy; (*RunTests.runTestsPyNative; *)RunTests.runTestsDotnet] { 
    ()
}


// to do: use this once we have actual tests
let runTestsWithCodeCov = BuildTask.create "RunTestsWithCodeCov" [clean; build] {
    let standardParams = Fake.DotNet.MSBuild.CliArguments.Create ()
    testProjects
    |> Seq.iter(fun testProject -> 
        Fake.DotNet.DotNet.test(fun testParams ->
            {
                testParams with
                    MSBuildParams = {
                        standardParams with
                            Properties = [
                                "AltCover","true"
                                "AltCoverCobertura","../../codeCov.xml"
                                "AltCoverForce","true"
                            ]
                    };
                    Logger = Some "console;verbosity=detailed"
            }
        ) testProject
    )
}