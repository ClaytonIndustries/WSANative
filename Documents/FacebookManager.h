#pragma once

using namespace Windows::Foundation;
using namespace Windows::UI;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Controls;
using namespace Microsoft::UI::Xaml::Controls;

ref class FacebookManager
{
public:
    static void Initialise(Grid^);
private:
    static Grid^ _grid;
    static void Close(FacebookCallbackDialogResponse callback, Grid^ control, const wchar_t* response);
};

#include "pch.h"
#include "WSAFacebookBridge.h"

Grid^ FacebookManager::_grid;

bool _isMounted;
bool _isClosed;

inline void FacebookManager::Initialise(Grid^ grid)
{
    _grid = grid;

    _FacebookDialogShowAction = [](FacebookCallbackDialogResponse callback, int screenWidth, int screenHeight, wchar_t* requestUri, wchar_t* responseUri, bool delayDialog)
    {
        auto _iframe = ref new WebView2();
        _iframe->SetValue(Grid::RowProperty, safe_cast<Object^>(0));

        auto _closeButton = ref new Button();
        _closeButton->Content = "Close";
        _closeButton->Height = 40;
        _closeButton->HorizontalAlignment = HorizontalAlignment::Stretch;
        _closeButton->Background = ref new SolidColorBrush(Colors::White);
        _closeButton->BorderBrush = ref new SolidColorBrush(Colors::Black);
        _closeButton->BorderThickness = Thickness(0, 1, 0, 0);
        _closeButton->Foreground = ref new SolidColorBrush(Colors::Black);

        _closeButton->SetValue(Grid::RowProperty, 1);

        int horizontalMargin = screenWidth / 10;
        int verticalMargin = screenHeight / 10;

        auto container = ref new Grid();
        container->Background = ref new SolidColorBrush(Colors::White);
        container->Margin = Thickness(horizontalMargin, verticalMargin, horizontalMargin, verticalMargin);
        auto row1 = ref new RowDefinition();
        row1->Height = GridLength(1, GridUnitType::Star);
        container->RowDefinitions->Append(row1);
        auto row2 = ref new RowDefinition();
        row2->Height = GridLength(1, GridUnitType::Auto);
        container->RowDefinitions->Append(row2);
        container->Children->Append(_iframe);
        container->Children->Append(_closeButton);

        auto backgroundColour = Colors::Black;
        backgroundColour.A = 128;

        auto background = ref new Grid();
        background->Background = ref new SolidColorBrush(backgroundColour);
        background->Children->Append(container);

        _isMounted = false;
        _isClosed = false;

        _iframe->NavigationStarting += ref new Windows::Foundation::TypedEventHandler<WebView2^, Microsoft::Web::WebView2::Core::CoreWebView2NavigationStartingEventArgs^>([delayDialog, background, requestUri, responseUri, callback](WebView2^ sender, Microsoft::Web::WebView2::Core::CoreWebView2NavigationStartingEventArgs^ args)
        {
            if (!_isClosed)
            {
                auto currentUri = ref new Uri(args->Uri);

                if (currentUri->Path == ref new Platform::String(responseUri))
                {
                    Close(callback, background, currentUri->ToString()->Data());
                }

                if (delayDialog && currentUri->Path == "/login" && !_isMounted)
                {
                    _grid->Children->Append(background);
                    _isMounted = true;
                }
            }
        });

        _closeButton->Click += ref new Windows::UI::Xaml::RoutedEventHandler([callback, background](Object^ sender, RoutedEventArgs^ e)
        {
            Close(callback, background, nullptr);
        });

        _iframe->Source = ref new Uri(ref new Platform::String(requestUri));

        if (!delayDialog)
        {
            _grid->Children->Append(background);
            _isMounted = true;
        }
    };
}

inline void FacebookManager::Close(FacebookCallbackDialogResponse callback, Grid^ control, const wchar_t* response)
{
    for (unsigned int i = 0; i < _grid->Children->Size; i++)
    {
        if (_grid->Children->GetAt(i) == control)
        {
            _grid->Children->RemoveAt(i);
            break;
        }
    }

    callback(response);
    _isClosed = true;
}