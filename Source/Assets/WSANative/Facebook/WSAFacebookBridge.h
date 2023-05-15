#if GENERATED_PROJECT
#define PLUGIN_API __declspec(dllimport)
#else
#define PLUGIN_API __declspec(dllexport)
#endif

typedef void(__stdcall *FacebookCallbackDialogResponse)(const wchar_t*);

extern PLUGIN_API void (*_FacebookDialogShowAction)(FacebookCallbackDialogResponse,int,int,wchar_t*,wchar_t*,bool);