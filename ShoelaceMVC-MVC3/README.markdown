ShoelaceMVC
==================

Why not an "official" Visual Studio Project Template?
----------------------------------

Microsoft has made MVC project templates a little different than most.  It 
doesn't allow Nuget packages to be included with a project template unless
it's officially sanctioned and distributed along with Visual Studio.  This means any Nuget packages we'd want to 
include wouldn't be with the project template when you created a new project.  
This means broken code all over, and that's no fun for everyone.  So, for MVC 4, I've decided change the way you use it as
a starter project by just making a project you can clone and continue on.  All you have to do is refactor the project name
to your liking.  The tradeoff is you're getting a complete, working starter project that builds without errors and includes
all the necessary Nuget packages to make things awesome...all this in exchange for having to know git to clone this repository
or simply click "Zip" button to download the repository completely.  We think that's worth it.
