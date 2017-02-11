private void InitialisePersonalisation()
{
	WSANativePersonalisation._GetAccentColour += () =>
	{
		Color colour;

		UnityEngine.WSA.Application.InvokeOnUIThread(() =>
		{
			colour = (Color)Resources["SystemAccentColor"];
		}, true);

		return new UnityEngine.Color(colour.R, colour.G, colour.B, colour.A);
	};
}

using CI.WSANative.Personalisation;