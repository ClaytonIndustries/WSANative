#include <windows.ui.xaml.controls.h>
#include <wrl.h>

using namespace ABI::Windows::UI::Xaml;
using namespace ABI::Windows::UI::Xaml::Controls;
using namespace Microsoft::WRL;

extern "C" HRESULT __stdcall GetPageContent(IInspectable* frame, IUIElement** pageContent)
{
   *pageContent = nullptr;
 
   ComPtr<IContentControl> frameContentControl;
   auto hr = frame->QueryInterface(__uuidof(frameContentControl), &frameContentControl);
   if (FAILED(hr))
       return hr;
 
   ComPtr<IInspectable> frameContentInspectable;
   hr = frameContentControl->get_Content(&frameContentInspectable);
   if (FAILED(hr))
       return hr;
 
   if (frameContentInspectable == nullptr)
       return S_OK;
 
   ComPtr<IUserControl> frameContent;
   hr = frameContentInspectable.As(&frameContent);
   if (FAILED(hr))
       return hr;
 
   return frameContent->get_Content(pageContent);
}