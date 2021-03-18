#if GENERATED_PROJECT
#define PLUGIN_API __declspec(dllimport)
#else
#define PLUGIN_API __declspec(dllexport)
#endif

typedef void(__stdcall *AdCallbackWithAdType)(const wchar_t*, const wchar_t*);
typedef void(__stdcall *AdCallbackWithAdTypeAndErrorMessage)(const wchar_t*, const wchar_t*);

extern PLUGIN_API void (*_BannerAdInitialiseAction)(AdCallbackWithAdType,AdCallbackWithAdTypeAndErrorMessage);
extern PLUGIN_API void (*_BannerAdCreateAction)(wchar_t*,wchar_t*,wchar_t*,int,int,wchar_t*,wchar_t*);
extern PLUGIN_API void (*_BannerAdSetVisibilityAction)(wchar_t*,bool);
extern PLUGIN_API void (*_BannerAdReconfigureAction)(wchar_t*,int,int,wchar_t*,wchar_t*);
extern PLUGIN_API void (*_BannerAdDestroyAction)(wchar_t*);

extern PLUGIN_API void (*_InterstitialAdInitialiseAction)(wchar_t*,wchar_t*,AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdTypeAndErrorMessage);
extern PLUGIN_API void (*_InterstitialAdRequestAction)(wchar_t*,wchar_t*);
extern PLUGIN_API void (*_InterstitialAdShowAction)(wchar_t*, wchar_t*);