1) Use Unity to build a windows store visual studio solution
2) Open the solution and add a reference to the correct ad sdk
3) Build the solution (you don't have to but it resolves any references that visual studio says are missing)
4) Open MainPage.xaml.cs and add the following using statements
using CI.WSANative.Advertising;
5) Add the correct ad manager to the solution - it must be in the same directory as MainPage.xaml.cs
6) Call Initialise from the bottom of the MainPage constructor
7) Open the apps manifest file and add the internet client capability