﻿#nullable enable

#if __WASM__ || __MACOS__
#pragma warning disable CS0067, CS0414
#endif

using System;
using System.Net.Http;
using Microsoft.Web.WebView2.Core;
using Uno.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.Foundation;
using System.Globalization;
using System.Linq;

namespace Windows.UI.Xaml.Controls;

#if __WASM__ || __SKIA__
[NotImplemented]
#endif
public partial class WebView : Control, IWebView
{
	/// <summary>
	/// Initializes a new instance of the WebView class.
	/// </summary>
	public WebView()
	{
		DefaultStyleKey = typeof(WebView);

		CoreWebView2 = new CoreWebView2(this);
		CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
		CoreWebView2.HistoryChanged += CoreWebView2_HistoryChanged;
		CoreWebView2.ContentLoading += CoreWebView2_ContentLoading;
		CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
		CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
		CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
		CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
		CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
	}

	internal CoreWebView2 CoreWebView2 { get; }

	protected override void OnApplyTemplate() => CoreWebView2.OnOwnerApplyTemplate();

	public void Navigate(global::System.Uri source) => CoreWebView2.Navigate(source.ToString());

	public void NavigateToString(string text) => CoreWebView2.NavigateToString(text);

	public void GoForward() => CoreWebView2.GoForward();

	public void GoBack() => CoreWebView2.GoBack();

	public void Refresh() => CoreWebView2.Reload();

	public void Stop() => CoreWebView2.Stop();

	public IAsyncOperation<string?> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
	{
		var argumentString = ConcatenateJavascriptArguments(arguments);
		var javaScript = string.Format(CultureInfo.InvariantCulture, "{0}(\"{1}\")", scriptName, argumentString);
		return CoreWebView2.ExecuteScriptAsync(javaScript);
	}

	internal void NavigateWithHttpRequestMessage(HttpRequestMessage requestMessage) =>
		CoreWebView2.NavigateWithHttpRequestMessage(requestMessage);

	private void CoreWebView2_DocumentTitleChanged(CoreWebView2 sender, object args) =>
		DocumentTitle = sender.DocumentTitle;

	private void CoreWebView2_HistoryChanged(CoreWebView2 sender, object args) =>
		(CanGoBack, CanGoForward) = (sender.CanGoBack, sender.CanGoForward);

	private void CoreWebView2_ContentLoading(CoreWebView2 sender, CoreWebView2ContentLoadingEventArgs args) =>
		ContentLoading?.Invoke(this, args.ToWebViewArgs());

	private void CoreWebView2_DOMContentLoaded(CoreWebView2 sender, CoreWebView2DOMContentLoadedEventArgs args) =>
		DOMContentLoaded?.Invoke(this, args.ToWebViewArgs());

	private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs args)
	{
		var webViewArgs = args.ToWebViewArgs();
		NavigationStarting?.Invoke(this, webViewArgs);
		args.Cancel = webViewArgs.Cancel;
	}

	private void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
	{
		NavigationCompleted?.Invoke(this, args.ToWebViewArgs());
		if (!args.IsSuccess)
		{
			NavigationFailed?.Invoke(this, new WebViewNavigationFailedEventArgs(args.Uri, args.WebErrorStatus.ToWebErrorStatus()));
		}
	}

	private void CoreWebView2_NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
	{
		var webViewArgs = args.ToWebViewArgs();
		NewWindowRequested?.Invoke(this, webViewArgs);
		args.Handled = webViewArgs.Handled;
	}

	private void CoreWebView2_SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args) =>
		Source = Uri.TryCreate(sender.Source, UriKind.Absolute, out var uri) ? uri : CoreWebView2.BlankUri;

	private static string ConcatenateJavascriptArguments(IEnumerable<string> arguments)
	{
		var argument = string.Empty;
		if (arguments != null && arguments.Any())
		{
			argument = string.Join(",", arguments);
		}

		return argument;
	}
}
