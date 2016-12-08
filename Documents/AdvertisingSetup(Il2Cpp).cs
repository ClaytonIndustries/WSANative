Configuration Properties -> Linker -> Command Line
$(OutDir)\GameAssembly.lib

#include "BannerAdManager.h"
#include "IntersitialAdManager.h"

BannerAdManager::Initialise(m_DXSwapChainPanel);
IntersitialAdManager::Initialise();

For interstitial ads enable
MICROSOFT_ENABLED and/or VUNGLE_ENABLED

1) Use Unity to build a windows 10 universal visual studio solution
2) Open the solution and add a reference to the correct ad sdk(s)
3) Add header file(s) to project
4) Open MainPage.xaml.cpp and add header includes
5) Call the initialise function(s) from the bottom of the MainPage constructor
6) Open the apps manifest file and add the internet client capability (you must do this or the ads won't show)