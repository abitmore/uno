﻿#nullable enable
using System;
using Uno.UI.RemoteControl.Messaging.IdeChannel;

namespace Uno.UI.RemoteControl.Messaging.IDEChannel;

/// <summary>
/// A message sent by the IDE to the dev-server when a command is issued (like a menuitem invoked).
/// </summary>
/// <param name="Command">The name/id of the command (e.g. uno.hotreload.open_hotreload_window).</param>
/// <param name="CommandParameter">A json serialized parameter to execute the command.</param>
public record CommandRequestIdeMessage(string Command, string? CommandParameter = null) : IdeMessage(WellKnownScopes.Ide)
{
	/// <summary>
	/// A unique identifier of this command execution request that could be used to track the response (if any produced by the remote handler).
	/// </summary>
	public Guid Id {get; } = Guid.NewGuid();
}
