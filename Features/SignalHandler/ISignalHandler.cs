using PollerBox.Features.SignalEmitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollerBox.Features.SignalHandler;
internal interface ISignalHandler
{
	Task HandleSignal(string? data);
}

internal class CardRemovedSignalHandler : ISignalHandler
{
	public Task HandleSignal(string? data)
	{
		throw new NotImplementedException();
	}
}

internal class RestartSignalHandler : ISignalHandler
{
	public Task HandleSignal(string? data)
	{
		throw new NotImplementedException();
	}
}
