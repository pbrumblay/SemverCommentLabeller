SemverCommentLabeller
========================

Based off CruiseControl.NET's AssemblyVersionLabeller, this Semver inspired labeller increments build numbers by looking for keyword prefixes in comments.

Inspiration
===========
I was using the assemblyVersionLabeller which was auto-incrementing the patch and updating the revision number and then I read this: http://blog.ploeh.dk/2013/12/10/semantic-versioning-with-continuous-deployment/. Re: "Versioning is a programmer decision". Damn. He's right. This was the quickest way I could think of to change our process and make it easy to put this decision into our developers' hands.

Usage
=====
When typing a comment add a prefix with the following case-insensitive strings if you want the corresponding number to change: "major:", "minor:", or "patch:".

Examples:
- "major: We've rolled out V3 of the interfaces which have major breaking changes for the Funny Stuff sprint."
- "minor: Sprint Crazy Dude completed. Removing seams and activating feature 'X'."
- "patch: bug fix ticket number PROJ-333. Added null checks to ISomeService implementors for calls against DoSomething()".

Example config:
```xml
<labeller type="semverCommentLabeller">
  <major>1</major>
  <minor>0</minor>
  <patch>111</patch>
</labeller>	
```

Notes
=====
- If you don't include a prefix in the comment the only part of the number which is updated is the revision (the 4th part).
- Updating "major" zeros out "minor" and "patch"; updating "minor" zeros out "patch".
- Written with VisualStudio 2012, CruiseControl.NET 1.8.3, and SVN.
- DLL goes into Cruise Control's "server" directory. E.g. C:\Program Files (x86)\CruiseControl.NET\server (restart the service)
- Numbers specified in config file *seed* the version number ONLY. All subsequent numbers use the previous build number.
- Handles the silly UInt16 issue with .NET version number components for the 4th part of the version number (in case you have more than 65535 commits in your source control repo).
