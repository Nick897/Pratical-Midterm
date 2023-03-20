Nicholas Xynos - 100782842
Kai Joseph - 100783670


Step 1 Open the Server: Open up Lecture 10.sln in Networking Midterm Lecture 10 Server -> Lecture 10 
Or you can use the Shortcuts provided for running the "Server" & "ServerUDP" C# scripts
-------------- 
Step 2 Set "Server" as your start up project
Step 3 Open up Client 1 executable
Step 4 Open up Client 2 executable
Step 5 Run "Server" in Lecture10.sln ------ Or run the "Server.exe" - Shorcut
Step 6 Set "ServerUDP" as your startup project
Step 7 Run "ServerUDP" in Lecture10.sln ----------- Or Run the "ServerUDP.exe" - Shortcut
Step 8 Press play on Client 1
Step 9 Press play on Client 2

You can now use both the UDP and TCP functionality of the program. Here is a reccomended order of testing
UDP functionality ("ServerUDP") works best when not having "Server" project running at the same time but it can work

TCP FUNCTIONALITY -----------------------------------------------------------------------------------------------------
Step 10 type in 127.0.0.1 into the server IP input field in both Client 1 & Client 2
Step 11 After typing in the server IP into both clients you can now type anything you like into the "Enter Text Field"
Step 12 When your Ready to send a message press the "F" key to send the message through the server
Step 13 The message sent will be displayed on both clien
 
UDP FUNCTIONALITY -----------------------------------------------------------------------------------------------------
Step 14 Once both clients are playing and the "ServerUDP" script is running you can use WASD to move the cube on either client 1 or client 2 project
Step 15 The opposite client's cube will update its position depending on the position of the active client's cube
