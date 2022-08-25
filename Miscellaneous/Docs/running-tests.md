## Running tests locally

Tests can be executed from Visual Studio.
Somehow running them from Visual Studio is slower and sometimes UI tests fail from Visual Studio UI based runner. 

The fastest way to run the tests is to run from CommandLine (make sure you are in the root directory). 
`packages\OpenCover.4.7.922\tools\OpenCover.Console.exe -filter:"+[MindMate*]*" -target:"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -targetargs:"MindMate.Tests/bin/debug/MindMate.Tests.dll" -excludebyattribute:*.ExcludeFromCodeCoverage* `

To capture the code coverage, run the following (it would be a lot slower though):
`packages\OpenCover.4.7.922\tools\OpenCover.Console.exe -register:user -filter:"+[MindMate*]*" -target:"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -targetargs:"MindMate.Tests/bin/debug/MindMate.Tests.dll" -excludebyattribute:*.ExcludeFromCodeCoverage* -output:coverage.xml`

At the time of writing (25-Aug-2022), tests take 35 seconds to run from command line but capturing code coverage increases the time to 2.26 minutes.

## Running Tests in the pipeline

On each commit, tests are executed in AppVeyor and the code coverage is uploaded to Coveralls.io (see appveyor.xml)