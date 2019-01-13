### Creating Release
	
1. Make sure changes are merged into master branch
2. Update version number on MindMate & MindMate.Win.exe assembly files
3. Update version number on Inno Setup file
    1. Parameter to update: MyAppVersion
    2. Setup Files ([MindMateInnoSetup.iss](https://github.com/umaranis/MindMate/blob/master/MindMate.Setup/MindMateInnoSetup.iss), [MindMateInnoSetup - Win7.iss](https://github.com/umaranis/MindMate/blob/master/MindMate.Setup/MindMateInnoSetup%20-%20Win7.iss))
4. Build project in release
5. Compile Inno Setup
6. Create Portable version from Program files
7. Build project in RelWin7
8. Compile Inno Setup for RelWin7
9. Create Portable version from Program files - Win7
10. Test build on VM
11. Check in code to Github
12. Put release tag on Github
13. Update download link at blog
14. Update the link at Github Readme
15. Update the main screen shot on GitHub
    1. Use the big monitor
    2. Don't maximize (Use full height, least possible width)
    3. Place at MindMate - Screen Shot.png
16. Write a new blog post
