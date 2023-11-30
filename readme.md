Budgetiir
==========

About
-----
This project is a personal budgeting application. Users are prompted for saving and spending goals, can track whether those goals are being met, and are helped deciding whether money is available within those goals to spend.

This project avoids using npm or nodejs because once I was bit by a hijacked npm account and unsafe package updates.

This project avoids using HTTPS because typical users will only use self-signed certs, and those aren't secure so don't bother.

This project doesn't have any unit tests because I don't pay me enough to write them for my personal projects.

Building
--------
Download .NET 7.0 SDK from https://dotnet.microsoft.com/en-us/download/dotnet/7.0
Do `dotnet run` to compile and run the project.
Launch a web browser at http://localhost:5000 and enjoy the ride.

Licensing
---------
Except for the third party libraries named below, the contents of this repo are free and unencumbered software released into the public domain under The Unlicense. You have complete freedom to do anything you want with the software, for any purpose. Please refer to <http://unlicense.org/> .

Third Party Libraries
---------------------
- ASP.NET Core - MIT License - see https://github.com/dotnet/aspnetcore/tree/v7.0.0
- Newtonsoft.Json - MIT License - see https://github.com/JamesNK/Newtonsoft.Json/tree/13.0.3
- bootstrap - MIT License - see https://github.com/twbs/bootstrap/tree/v4.3.1
- jquery - MIT License - see https://github.com/jquery/jquery/tree/3.5.1
- jquery-validation - MIT License - see https://github.com/jquery-validation/jquery-validation/tree/1.17.0
- bignumber.js - MIT license - see https://github.com/MikeMcl/bignumber.js/tree/v9.0.2
- vanillajs-datepicker - MIT license - see https://github.com/mymth/vanillajs-datepicker/tree/v1.2.0