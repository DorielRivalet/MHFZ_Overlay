// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

using DiscordRPC;
using System;
using DiscordRPC.Events;
using DiscordRPC.Message;
using DiscordRPC.Logging;

public interface IDiscordRpcClient : IDisposable
{
    bool HasRegisteredUriScheme { get; }

    string ApplicationID { get; }

    string SteamID { get; }

    int ProcessID { get; }

    int MaxQueueSize { get; }

    bool IsDisposed { get; }

    ILogger Logger { get; set; }

    bool AutoEvents { get; }

    bool SkipIdenticalPresence { get; set; }

    int TargetPipe { get; }

    RichPresence CurrentPresence { get; }

    EventType Subscription { get; }

    User CurrentUser { get; }

    Configuration Configuration { get; }

    bool IsInitialized { get; }

    bool ShutdownOnly { get; set; }

    event OnReadyEvent OnReady;

    event OnCloseEvent OnClose;

    event OnErrorEvent OnError;

    event OnPresenceUpdateEvent OnPresenceUpdate;

    event OnSubscribeEvent OnSubscribe;

    event OnUnsubscribeEvent OnUnsubscribe;

    event OnJoinEvent OnJoin;

    event OnSpectateEvent OnSpectate;

    event OnJoinRequestedEvent OnJoinRequested;

    event OnConnectionEstablishedEvent OnConnectionEstablished;

    event OnConnectionFailedEvent OnConnectionFailed;

    event OnRpcMessageEvent OnRpcMessage;

    void Respond(JoinRequestMessage request, bool acceptRequest);

    void SetPresence(RichPresence presence);

    RichPresence UpdateButtons(Button[] button = null);

    RichPresence SetButton(Button button, int index = 0);

    RichPresence UpdateDetails(string details);

    RichPresence UpdateState(string state);

    RichPresence UpdateParty(Party party);

    RichPresence UpdatePartySize(int size);

    RichPresence UpdatePartySize(int size, int max);

    RichPresence UpdateLargeAsset(string key = null, string tooltip = null);

    RichPresence UpdateSmallAsset(string key = null, string tooltip = null);

    RichPresence UpdateSecrets(Secrets secrets);

    RichPresence UpdateStartTime();

    RichPresence UpdateStartTime(DateTime time);

    RichPresence UpdateEndTime();

    RichPresence UpdateEndTime(DateTime time);

    RichPresence UpdateClearTime();

    void ClearPresence();

    bool RegisterUriScheme(string steamAppID = null, string executable = null);

    void Subscribe(EventType type);

    void Unsubscribe(EventType type);

    void SetSubscription(EventType type);

    void SynchronizeState();

    bool Initialize();

    void Deinitialize();
}
