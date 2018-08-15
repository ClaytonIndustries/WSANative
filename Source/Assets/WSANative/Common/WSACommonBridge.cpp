#include <wrl.h>
 
Microsoft::WRL::ComPtr<IInspectable> _swapChainPanelInspectable;
 
extern "C" IInspectable* GetSwapChainPanel()
{
    auto result = _swapChainPanelInspectable.Get();
    result->AddRef();
    return result;
}
 
__declspec(dllexport) extern "C" void SetSwapChainPanel(IInspectable* swapChainPanel)
{
    _swapChainPanelInspectable = swapChainPanel;
}