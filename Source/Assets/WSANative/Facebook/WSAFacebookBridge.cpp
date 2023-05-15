#include "WSAFacebookBridge.h"

void (*_FacebookDialogShowAction)(FacebookCallbackDialogResponse,int,int,wchar_t*,wchar_t*,bool);

extern "C" void __stdcall _FacebookDialogShow(FacebookCallbackDialogResponse callback, int screenWidth, int screenHeight, wchar_t* requestUri, wchar_t* responseUri, bool delayDialog)
{
	_FacebookDialogShowAction(callback, screenWidth, screenHeight, requestUri, responseUri, delayDialog);
}