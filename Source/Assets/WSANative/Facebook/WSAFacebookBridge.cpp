#include "WSAFacebookBridge.h"

void (*_FacebookLoginShowAction)(FacebookCallbackLoginResponse,int,int,wchar_t*,wchar_t*,bool);

extern "C" void __stdcall _FacebookLoginShow(FacebookCallbackLoginResponse callback, int screenWidth, int screenHeight, wchar_t* requestUri, wchar_t* responseUri, bool delayDialog)
{
	_FacebookLoginShowAction(callback, screenWidth, screenHeight, requestUri, responseUri, delayDialog);
}