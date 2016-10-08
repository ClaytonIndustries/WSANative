private void InitialiseFeedbackHub()
{
	WSANativeEngagement._IsFeedbackHubSupported += () => 
	{
		return StoreServicesFeedbackLauncher.IsSupported();
	};
	WSANativeEngagement._ShowFeedbackHub += async (feedbackProperties) =>
	{
		if (StoreServicesFeedbackLauncher.IsSupported())
		{
			if (feedbackProperties != null)
			{
				await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync(feedbackProperties);
			}
			else
			{
				await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
			}
		}
	};
}

Add a reference to the Store Engagement SDK
Add using CI.WSANative.Engagement;
Add using Microsoft.Services.Store.Engagement;
Add the code above
Call the above function from the bottom of the MainPage.xaml.cs constructor