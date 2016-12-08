#if GENERATED_PROJECT
#define PLUGIN_API __declspec(dllimport)
#else
#define PLUGIN_API __declspec(dllexport)
#endif

typedef void(__stdcall *AdCallback)();
typedef void(__stdcall *AdCallbackWithAdType)(const wchar_t*);

extern PLUGIN_API void (*_BannerAdCreateAction)(wchar_t*,wchar_t*,int,int,wchar_t*,wchar_t*,AdCallback,AdCallback);
extern PLUGIN_API void (*_BannerAdSetVisibilityAction)(bool);
extern PLUGIN_API void (*_BannerAdDestroyAction)();

extern PLUGIN_API void (*_InterstitialAdInitialiseAction)(AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdType);
extern PLUGIN_API void (*_InterstitialAdRequestAction)(wchar_t*,wchar_t*,wchar_t*);
extern PLUGIN_API void (*_InterstitialAdShowAction)(wchar_t*);