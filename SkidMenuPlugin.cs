using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using AmongUs.QuickChat;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using InnerNet;
using TMPro;
using UnityEngine;

namespace SkidMenu
{
	// Token: 0x02000002 RID: 2
	[BepInPlugin("com.skid.menu", "SkidMenu", "1.0.7")]
	[BepInProcess("Among Us.exe")]
	public class SkidMenuPlugin : BasePlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load()
		{
			SkidMenuPlugin.Logger = base.Log;
			SkidMenuPlugin.MenuKey = base.Config.Bind<KeyCode>("Menu Settings", "MenuHotkey", KeyCode.Insert, "The keyboard key used to toggle the menu on and off");
			SkidMenuPlugin.SpoofPlatform = base.Config.Bind<string>("Spoofing", "SpoofPlatform", "", "Custom platform (StandaloneSteamPC, StandaloneEpicPC, StandaloneMac, StandaloneWin10, StandaloneItch, IPhone, Android, Switch, Xbox, Playstation). Leave blank to disable");
			SkidMenuPlugin.Config_TeleportToCursorEnabled = base.Config.Bind<bool>("Features", "TeleportToCursor", false, "Teleport to cursor on right click");
			SkidMenuPlugin.Config_AlwaysShowChatEnabled = base.Config.Bind<bool>("Features", "AlwaysShowChat", false, "Always show chat");
			SkidMenuPlugin.Config_RevealRolesEnabled = base.Config.Bind<bool>("Features", "RevealRoles", false, "See everyone's roles");
			SkidMenuPlugin.Config_RevealVotesEnabled = base.Config.Bind<bool>("Features", "RevealVotes", false, "See votes in meetings");
			SkidMenuPlugin.Config_ZoomOutEnabled = base.Config.Bind<bool>("Features", "ZoomOut", false, "Enable zoom out");
			SkidMenuPlugin.Config_SeeGhostsEnabled = base.Config.Bind<bool>("Features", "SeeGhosts", false, "See ghost players");
			SkidMenuPlugin.Config_MoreLobbyInfoEnabled = base.Config.Bind<bool>("Features", "MoreLobbyInfo", false, "Show extended lobby information");
			SkidMenuPlugin.Config_ShowHostEnabled = base.Config.Bind<bool>("Features", "ShowHost", false, "Show host in ping tracker");
			SkidMenuPlugin.Config_SeeModUsersEnabled = base.Config.Bind<bool>("Features", "SeeModUsers", true, "Detect other mod users");
			SkidMenuPlugin.Config_DarkModeEnabled = base.Config.Bind<bool>("Features", "DarkMode", false, "Enable dark mode");
			SkidMenuPlugin.Config_NoShadowsEnabled = base.Config.Bind<bool>("Features", "NoShadows", false, "Disable shadows (fullbright)");
			SkidMenuPlugin.Config_ShowLobbyTimerEnabled = base.Config.Bind<bool>("Host", "ShowLobbyTimer", true, "Show lobby countdown timer");
			SkidMenuPlugin.Config_SpoofMenuEnabled = base.Config.Bind<bool>("Features", "SpoofMenuEnabled", false, "Enable menu spoofing");
			SkidMenuPlugin.Config_SpoofMenuIndex = base.Config.Bind<int>("Features", "SpoofMenuIndex", 0, "Selected spoofed menu index");
			SkidMenuPlugin.Config_EndlessVentTime = base.Config.Bind<bool>("Roles", "EndlessVentTime", false, "Engineer: Endless vent time");
			SkidMenuPlugin.Config_NoVentCooldown = base.Config.Bind<bool>("Roles", "NoVentCooldown", false, "Engineer: No vent cooldown");
			SkidMenuPlugin.Config_NoVitalsCooldown = base.Config.Bind<bool>("Roles", "NoVitalsCooldown", false, "Scientist: No vitals cooldown");
			SkidMenuPlugin.Config_EndlessBattery = base.Config.Bind<bool>("Roles", "EndlessBattery", false, "Scientist: Endless battery");
			SkidMenuPlugin.Config_NoTrackingCooldown = base.Config.Bind<bool>("Roles", "NoTrackingCooldown", false, "Tracker: No tracking cooldown");
			SkidMenuPlugin.Config_NoTrackingDelay = base.Config.Bind<bool>("Roles", "NoTrackingDelay", false, "Tracker: No tracking delay");
			SkidMenuPlugin.Config_EndlessTracking = base.Config.Bind<bool>("Roles", "EndlessTracking", false, "Tracker: Endless tracking duration");
			SkidMenuPlugin.Config_EndlessShapeshiftDuration = base.Config.Bind<bool>("Roles", "EndlessShapeshiftDuration", false, "Shapeshifter: Endless shapeshift duration");
			SkidMenuPlugin.Config_NoShapeshiftAnimation = base.Config.Bind<bool>("Roles", "NoShapeshiftAnimation", false, "Shapeshifter: No shapeshift animation");
			SkidMenuPlugin.Config_UnlimitedKillRange = base.Config.Bind<bool>("Roles", "UnlimitedKillRange", false, "Impostor: Unlimited kill range");
			SkidMenuPlugin.Config_ImpostorTasksEnabled = base.Config.Bind<bool>("Roles", "ImpostorTasks", false, "Impostor: Do tasks as impostor");
			SkidMenuPlugin.Config_UnlimitedInterrogateRange = base.Config.Bind<bool>("Roles", "UnlimitedInterrogateRange", false, "Detective: Unlimited interrogate range");
			SkidMenuPlugin.Config_ShowKillCooldown = base.Config.Bind<bool>("Features", "ShowKillCooldown", false, "Show kill cooldown overlay");
			SkidMenuPlugin.Config_NoClipEnabled = base.Config.Bind<bool>("Features", "NoClip", false, "Enable no clip");
			SkidMenuPlugin.Config_SpinEnabled = base.Config.Bind<bool>("Features", "Spin", false, "Enable spin");
			SkidMenuPlugin.Config_ExileMeEnabled = base.Config.Bind<bool>("Features", "ExileMe", false, "Enable exile me");
			SkidMenuPlugin.Config_AntiOverloadEnabled = base.Config.Bind<bool>("Features", "AntiOverload", false, "Enable anti-overload protection");
			SkidMenuPlugin.Config_KillNotificationEnabled = base.Config.Bind<bool>("Features", "KillNotification", false, "Show kill notifications");
			SkidMenuPlugin.Config_AutoCopyCodeEnabled = base.Config.Bind<bool>("Features", "AutoCopyCode", false, "Auto copy lobby code");
			SkidMenuPlugin.Config_ShowPlayerInfo = base.Config.Bind<bool>("Features", "ShowPlayerInfo", false, "Show player info overlay");
			SkidMenuPlugin.Config_KillOtherImpostersEnabled = base.Config.Bind<bool>("Features", "KillOtherImposters", false, "Allow killing other imposters");
			SkidMenuPlugin.Config_ShowVotekickInfo = base.Config.Bind<bool>("Features", "ShowVotekickInfo", false, "Show votekick info");
			SkidMenuPlugin.Config_RGBMode = base.Config.Bind<bool>("Visual", "RGBMode", false, "RGB menu colors");
			SkidMenuPlugin.Config_StealthMode = base.Config.Bind<bool>("Features", "StealthMode", false, "Stealth mode (hide RPC)");
			SkidMenuPlugin.Config_RandomizeOutfit = base.Config.Bind<bool>("Host", "RandomizeOutfit", false, "Randomize outfit");
			SkidMenuPlugin.Config_BanBlacklistedEnabled = base.Config.Bind<bool>("Host", "BanBlacklisted", false, "Auto-ban blacklisted players");
			SkidMenuPlugin.Config_DisableVotekicks = base.Config.Bind<bool>("Host", "DisableVotekicks", false, "Disable votekicks");
			SkidMenuPlugin.Config_DisableMeetings = base.Config.Bind<bool>("Host", "DisableMeetings", false, "Disable meetings");
			SkidMenuPlugin.Config_GodModeEnabled = base.Config.Bind<bool>("Host", "GodMode", false, "Enable god mode");
			SkidMenuPlugin.Config_DisableSabotagesEnabled = base.Config.Bind<bool>("Host", "DisableSabotages", false, "Disable sabotages");
			SkidMenuPlugin.Config_DisableGameEndEnabled = base.Config.Bind<bool>("Host", "DisableGameEnd", false, "Disable game end");
			SkidMenuPlugin.Config_OverloadInfoEnabled = base.Config.Bind<bool>("Features", "OverloadInfo", false, "Show overload detection info");
			SkidMenuPlugin.Config_AnticheatEnabled = base.Config.Bind<bool>("Anticheat", "Enabled", false, "Enable anticheat");
			SkidMenuPlugin.Config_AutoBanEnabled = base.Config.Bind<bool>("Anticheat", "AutoBan", false, "Auto-ban detected cheaters");
			SkidMenuPlugin.Config_CheckInvalidCompleteTask = base.Config.Bind<bool>("Anticheat", "CheckCompleteTask", true, "Check invalid complete task");
			SkidMenuPlugin.Config_CheckInvalidPlayAnimation = base.Config.Bind<bool>("Anticheat", "CheckPlayAnimation", true, "Check invalid play animation");
			SkidMenuPlugin.Config_CheckInvalidScanner = base.Config.Bind<bool>("Anticheat", "CheckScanner", true, "Check invalid scanner");
			SkidMenuPlugin.Config_CheckInvalidVent = base.Config.Bind<bool>("Anticheat", "CheckVent", true, "Check invalid vent");
			SkidMenuPlugin.Config_CheckInvalidSnapTo = base.Config.Bind<bool>("Anticheat", "CheckSnapTo", true, "Check invalid snap to");
			SkidMenuPlugin.Config_CheckInvalidStartCounter = base.Config.Bind<bool>("Anticheat", "CheckStartCounter", true, "Check invalid start counter");
			SkidMenuPlugin.Config_CheckSpoofedPlatforms = base.Config.Bind<bool>("Anticheat", "CheckSpoofedPlatforms", true, "Check spoofed platforms");
			SkidMenuPlugin.Config_CheckSpoofedLevels = base.Config.Bind<bool>("Anticheat", "CheckSpoofedLevels", true, "Check spoofed levels");
			SkidMenuPlugin.Config_FindDatersEnabled = base.Config.Bind<bool>("NoDating", "FindDaters", false, "Find daters lobby filter");
			SkidMenuPlugin.Config_ExtendedLobbyEnabled = base.Config.Bind<bool>("NoDating", "ExtendedLobby", false, "Extended lobby list");
			SkidMenuPlugin.Config_DestroyLobbyEnabled = base.Config.Bind<bool>("NoDating", "DestroyLobby", false, "Destroy lobby");
			SkidMenuPlugin.Config_SpamChatEnabled = base.Config.Bind<bool>("Chat", "SpamChat", false, "Spam chat");
			SkidMenuPlugin.Config_VotekickAllEnabled = base.Config.Bind<bool>("Votekick", "VotekickAll", false, "Votekick all players");
			SkidMenuPlugin.Config_SpoofLevel = base.Config.Bind<int>("Spoof", "SpoofLevel", 0, "Spoofed level to show to others (0 = disabled)");
			SkidMenuPlugin.ShowKillCooldown = SkidMenuPlugin.Config_ShowKillCooldown.Value;
			SkidMenuPlugin.NoClipEnabled = SkidMenuPlugin.Config_NoClipEnabled.Value;
			SkidMenuPlugin.SpinEnabled = SkidMenuPlugin.Config_SpinEnabled.Value;
			SkidMenuPlugin.ExileMeEnabled = SkidMenuPlugin.Config_ExileMeEnabled.Value;
			SkidMenuPlugin.AntiOverloadEnabled = SkidMenuPlugin.Config_AntiOverloadEnabled.Value;
			SkidMenuPlugin.KillNotificationEnabled = SkidMenuPlugin.Config_KillNotificationEnabled.Value;
			SkidMenuPlugin.AutoCopyCodeEnabled = SkidMenuPlugin.Config_AutoCopyCodeEnabled.Value;
			SkidMenuPlugin.ShowPlayerInfo = SkidMenuPlugin.Config_ShowPlayerInfo.Value;
			SkidMenuPlugin.KillOtherImpostersEnabled = SkidMenuPlugin.Config_KillOtherImpostersEnabled.Value;
			SkidMenuPlugin.ShowVotekickInfo = SkidMenuPlugin.Config_ShowVotekickInfo.Value;
			SkidMenuPlugin.NoShadowsEnabled = SkidMenuPlugin.Config_NoShadowsEnabled.Value;
			SkidMenuPlugin.RGBMode = SkidMenuPlugin.Config_RGBMode.Value;
			SkidMenuPlugin.StealthMode = SkidMenuPlugin.Config_StealthMode.Value;
			SkidMenuPlugin.RandomizeOutfit = SkidMenuPlugin.Config_RandomizeOutfit.Value;
			SkidMenuPlugin.BanBlacklistedEnabled = SkidMenuPlugin.Config_BanBlacklistedEnabled.Value;
			SkidMenuPlugin.DisableVotekicks = SkidMenuPlugin.Config_DisableVotekicks.Value;
			SkidMenuPlugin.DisableMeetings = SkidMenuPlugin.Config_DisableMeetings.Value;
			SkidMenuPlugin.GodModeEnabled = SkidMenuPlugin.Config_GodModeEnabled.Value;
			SkidMenuPlugin.DisableSabotagesEnabled = SkidMenuPlugin.Config_DisableSabotagesEnabled.Value;
			SkidMenuPlugin.DisableGameEndEnabled = SkidMenuPlugin.Config_DisableGameEndEnabled.Value;
			SkidMenuPlugin.OverloadInfoEnabled = SkidMenuPlugin.Config_OverloadInfoEnabled.Value;
			SkidMenuPlugin.AnticheatEnabled = SkidMenuPlugin.Config_AnticheatEnabled.Value;
			SkidMenuPlugin.AutoBanEnabled = SkidMenuPlugin.Config_AutoBanEnabled.Value;
			SkidMenuPlugin.CheckInvalidCompleteTask = SkidMenuPlugin.Config_CheckInvalidCompleteTask.Value;
			SkidMenuPlugin.CheckInvalidPlayAnimation = SkidMenuPlugin.Config_CheckInvalidPlayAnimation.Value;
			SkidMenuPlugin.CheckInvalidScanner = SkidMenuPlugin.Config_CheckInvalidScanner.Value;
			SkidMenuPlugin.CheckInvalidVent = SkidMenuPlugin.Config_CheckInvalidVent.Value;
			SkidMenuPlugin.CheckInvalidSnapTo = SkidMenuPlugin.Config_CheckInvalidSnapTo.Value;
			SkidMenuPlugin.CheckInvalidStartCounter = SkidMenuPlugin.Config_CheckInvalidStartCounter.Value;
			SkidMenuPlugin.CheckSpoofedPlatforms = SkidMenuPlugin.Config_CheckSpoofedPlatforms.Value;
			SkidMenuPlugin.CheckSpoofedLevels = SkidMenuPlugin.Config_CheckSpoofedLevels.Value;
			SkidMenuPlugin.FindDatersEnabled = SkidMenuPlugin.Config_FindDatersEnabled.Value;
			SkidMenuPlugin.ExtendedLobbyEnabled = SkidMenuPlugin.Config_ExtendedLobbyEnabled.Value;
			SkidMenuPlugin.DestroyLobbyEnabled = SkidMenuPlugin.Config_DestroyLobbyEnabled.Value;
			SkidMenuPlugin.SkidMenu.SpamChatEnabled = SkidMenuPlugin.Config_SpamChatEnabled.Value;
			SkidMenuPlugin.VotekickAllEnabled = SkidMenuPlugin.Config_VotekickAllEnabled.Value;
			SkidMenuPlugin.TeleportToCursorEnabled = SkidMenuPlugin.Config_TeleportToCursorEnabled.Value;
			SkidMenuPlugin.AlwaysShowChatEnabled = SkidMenuPlugin.Config_AlwaysShowChatEnabled.Value;
			SkidMenuPlugin.SeeRolesEnabled = SkidMenuPlugin.Config_RevealRolesEnabled.Value;
			SkidMenuPlugin.RevealVotesEnabled = SkidMenuPlugin.Config_RevealVotesEnabled.Value;
			SkidMenuPlugin.ZoomOutEnabled = SkidMenuPlugin.Config_ZoomOutEnabled.Value;
			SkidMenuPlugin.SeeGhostsEnabled = SkidMenuPlugin.Config_SeeGhostsEnabled.Value;
			SkidMenuPlugin.MoreLobbyInfoEnabled = SkidMenuPlugin.Config_MoreLobbyInfoEnabled.Value;
			SkidMenuPlugin.ShowHostEnabled = SkidMenuPlugin.Config_ShowHostEnabled.Value;
			SkidMenuPlugin.SeeModUsersEnabled = SkidMenuPlugin.Config_SeeModUsersEnabled.Value;
			SkidMenuPlugin.DarkModeEnabled = SkidMenuPlugin.Config_DarkModeEnabled.Value;
			SkidMenuPlugin.ShowLobbyTimerEnabled = SkidMenuPlugin.Config_ShowLobbyTimerEnabled.Value;
			SkidMenuPlugin.SpoofMenuEnabled = SkidMenuPlugin.Config_SpoofMenuEnabled.Value;
			SkidMenuPlugin.selectedSpoofMenuIndex = SkidMenuPlugin.Config_SpoofMenuIndex.Value;
			SkidMenuPlugin.EndlessVentTime = SkidMenuPlugin.Config_EndlessVentTime.Value;
			SkidMenuPlugin.NoVentCooldown = SkidMenuPlugin.Config_NoVentCooldown.Value;
			SkidMenuPlugin.NoVitalsCooldown = SkidMenuPlugin.Config_NoVitalsCooldown.Value;
			SkidMenuPlugin.EndlessBattery = SkidMenuPlugin.Config_EndlessBattery.Value;
			SkidMenuPlugin.NoTrackingCooldown = SkidMenuPlugin.Config_NoTrackingCooldown.Value;
			SkidMenuPlugin.NoTrackingDelay = SkidMenuPlugin.Config_NoTrackingDelay.Value;
			SkidMenuPlugin.EndlessTracking = SkidMenuPlugin.Config_EndlessTracking.Value;
			SkidMenuPlugin.EndlessShapeshiftDuration = SkidMenuPlugin.Config_EndlessShapeshiftDuration.Value;
			SkidMenuPlugin.NoShapeshiftAnimation = SkidMenuPlugin.Config_NoShapeshiftAnimation.Value;
			SkidMenuPlugin.UnlimitedKillRange = SkidMenuPlugin.Config_UnlimitedKillRange.Value;
			SkidMenuPlugin.ImpostorTasksEnabled = SkidMenuPlugin.Config_ImpostorTasksEnabled.Value;
			SkidMenuPlugin.UnlimitedInterrogateRange = SkidMenuPlugin.Config_UnlimitedInterrogateRange.Value;
			try
			{
				Harmony harmony = new Harmony("com.skid.menu");
				harmony.PatchAll();
				SkidMenuPlugin.Logger.LogInfo("Harmony patches applied successfully");
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("Harmony patching failed: " + ex.Message);
			}
			base.AddComponent<SkidMenuPlugin.SkidMenu>();
			base.AddComponent<HostCommandPatch.RolesUI>();
			base.AddComponent<HostCommandPatch.ForceColorUI>();
			SkidMenuPlugin.LoadBlacklist(); SkidMenuPlugin.Logger.LogInfo("Credits to Pro");
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002C78 File Offset: 0x00000E78
		public static Color GetRGBColor()
		{
			bool flag = !SkidMenuPlugin.RGBMode;
			Color result;
			if (flag)
			{
				result = new Color(0.05f, 0.05f, 0.05f, 0.98f);
			}
			else
			{
				bool flag2 = !SkidMenuPlugin.colorLocked;
				if (flag2)
				{
					SkidMenuPlugin.colorLocked = true;
					SkidMenuPlugin.UpdateRGBBreathing();
					Color cachedRGBColor = Color.HSVToRGB(SkidMenuPlugin.rgbHue, 0.6f * SkidMenuPlugin.breatheIntensity, 0.4f * SkidMenuPlugin.breatheIntensity);
					cachedRGBColor.a = 0.95f;
					SkidMenuPlugin._cachedRGBColor = cachedRGBColor;
					Color cachedRGBAccent = Color.HSVToRGB(SkidMenuPlugin.rgbHue, 0.5f * SkidMenuPlugin.breatheIntensity, 0.3f * SkidMenuPlugin.breatheIntensity);
					cachedRGBAccent.a = 1f;
					SkidMenuPlugin._cachedRGBAccent = cachedRGBAccent;
					Color cachedRGBText = Color.HSVToRGB(SkidMenuPlugin.rgbHue, 0.4f * SkidMenuPlugin.breatheIntensity, 0.9f * SkidMenuPlugin.breatheIntensity);
					cachedRGBText.a = 1f;
					SkidMenuPlugin._cachedRGBText = cachedRGBText;
				}
				result = SkidMenuPlugin._cachedRGBColor;
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002D74 File Offset: 0x00000F74
		public static Color GetRGBAccent()
		{
			bool flag = !SkidMenuPlugin.RGBMode;
			Color result;
			if (flag)
			{
				result = new Color(0.15f, 0.15f, 0.2f, 1f);
			}
			else
			{
				result = SkidMenuPlugin._cachedRGBAccent;
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public static Color GetRGBText()
		{
			bool flag = !SkidMenuPlugin.RGBMode;
			Color result;
			if (flag)
			{
				result = Color.white;
			}
			else
			{
				result = SkidMenuPlugin._cachedRGBText;
			}
			return result;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002DE0 File Offset: 0x00000FE0
		private static void UpdateRGBBreathing()
		{
			bool flag = SkidMenuPlugin.breatheIncreasing;
			if (flag)
			{
				SkidMenuPlugin.breatheIntensity += Time.deltaTime * 0.8f;
				bool flag2 = SkidMenuPlugin.breatheIntensity >= 1f;
				if (flag2)
				{
					SkidMenuPlugin.breatheIntensity = 1f;
					SkidMenuPlugin.breatheIncreasing = false;
				}
			}
			else
			{
				SkidMenuPlugin.breatheIntensity -= Time.deltaTime * 0.8f;
				bool flag3 = SkidMenuPlugin.breatheIntensity <= 0.3f;
				if (flag3)
				{
					SkidMenuPlugin.breatheIntensity = 0.3f;
					SkidMenuPlugin.breatheIncreasing = true;
					SkidMenuPlugin.currentHueTarget += 0.15f;
					bool flag4 = SkidMenuPlugin.currentHueTarget > 1f;
					if (flag4)
					{
						SkidMenuPlugin.currentHueTarget = 0f;
					}
				}
			}
			SkidMenuPlugin.rgbHue = Mathf.Lerp(SkidMenuPlugin.rgbHue, SkidMenuPlugin.currentHueTarget, Time.deltaTime * 2f);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002EBC File Offset: 0x000010BC
		public static void SendIdentificationRPC()
		{
			try
			{
				SkidMenuPlugin.TrackOwnModUsage();
				bool stealthMode = SkidMenuPlugin.StealthMode;
				if (!stealthMode)
				{
					bool flag = PlayerControl.LocalPlayer == null || AmongUsClient.Instance == null;
					if (!flag)
					{
						bool flag2 = Time.time - SkidMenuPlugin.lastIdentificationRpcTime < SkidMenuPlugin.identificationRpcInterval;
						if (!flag2)
						{
							byte callId = SkidMenuPlugin.SpoofMenuEnabled ? SkidMenuPlugin.spoofMenuRPCs[SkidMenuPlugin.selectedSpoofMenuIndex] : 121;
							MessageWriter msg = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, callId, SendOption.None, -1);
							AmongUsClient.Instance.FinishRpcImmediately(msg);
							string str = SkidMenuPlugin.SpoofMenuEnabled ? SkidMenuPlugin.spoofMenuNames[SkidMenuPlugin.selectedSpoofMenuIndex] : "SkidMenu";
							SkidMenuPlugin.Logger.LogInfo("========================================");
							SkidMenuPlugin.Logger.LogInfo("[Menu Identification] Broadcasting as: " + str);
							SkidMenuPlugin.Logger.LogInfo("[Menu Identification] RPC ID: " + callId.ToString());
							SkidMenuPlugin.Logger.LogInfo("[Menu Identification] Spoof Mode: " + (SkidMenuPlugin.SpoofMenuEnabled ? "ON" : "OFF"));
							SkidMenuPlugin.Logger.LogInfo("========================================");
							SkidMenuPlugin.lastIdentificationRpcTime = Time.time;
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendIdentificationRPC error: " + ex.Message);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00003040 File Offset: 0x00001240
		public static void TrackOwnModUsage()
		{
			try
			{
				bool flag = !SkidMenuPlugin.SeeModUsersEnabled;
				if (flag)
				{
					bool flag2 = PlayerControl.LocalPlayer != null;
					if (flag2)
					{
						byte playerId = PlayerControl.LocalPlayer.PlayerId;
						bool flag3 = SkidMenuPlugin.detectedModUsers.ContainsKey(playerId);
						if (flag3)
						{
							SkidMenuPlugin.detectedModUsers.Remove(playerId);
						}
					}
				}
				else
				{
					bool flag4 = PlayerControl.LocalPlayer == null;
					if (!flag4)
					{
						byte playerId2 = PlayerControl.LocalPlayer.PlayerId;
						bool stealthMode = SkidMenuPlugin.StealthMode;
						if (stealthMode)
						{
							bool flag5 = SkidMenuPlugin.detectedModUsers.ContainsKey(playerId2);
							if (flag5)
							{
								SkidMenuPlugin.detectedModUsers.Remove(playerId2);
							}
						}
						else
						{
							byte value = SkidMenuPlugin.SpoofMenuEnabled ? SkidMenuPlugin.spoofMenuRPCs[SkidMenuPlugin.selectedSpoofMenuIndex] : 121;
							SkidMenuPlugin.detectedModUsers[playerId2] = value;
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("TrackOwnModUsage error: " + ex.Message);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00003148 File Offset: 0x00001348
		public static void SendWeirdQuickChat()
		{
			try
			{
				bool flag = PlayerControl.LocalPlayer == null;
				if (!flag)
				{
					int num = UnityEngine.Random.Range(0, SkidMenuPlugin.weirdMessages.Length);
					string text = SkidMenuPlugin.weirdMessages[num];
					bool flag2 = text.Length >= 100;
					if (flag2)
					{
						text = text.Substring(0, 99);
					}
					PlayerControl.LocalPlayer.RpcSendChat(text);
					SkidMenuPlugin.Logger.LogInfo("Sent random weird chat #" + num.ToString() + ": " + text);
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("Send Weird Chat error: " + ex.Message);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00003204 File Offset: 0x00001404
		public static void ApplyForcedRoles()
		{
			try
			{
				bool flag = !AmongUsClient.Instance.AmHost || SkidMenuPlugin.forcedRoles.Count == 0;
				if (!flag)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, RoleTypes> keyValuePair in SkidMenuPlugin.forcedRoles)
					{
						int playerId = keyValuePair.Key;
						RoleTypes value = keyValuePair.Value;
						PlayerControl playerControl = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault((PlayerControl p) => p != null && (int)p.PlayerId == playerId);
						bool flag2 = playerControl != null && playerControl.Data != null;
						if (flag2)
						{
							playerControl.RpcSetRole(value, true);
							SkidMenuPlugin.Logger.LogInfo("? Forced " + playerControl.Data.PlayerName + " to role: " + value.ToString());
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("ApplyForcedRoles error: " + ex.Message);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000334C File Offset: 0x0000154C
		public static void LoadBlacklist()
		{
			try
			{
				string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				path = Path.Combine(path, "..", "..", "..");
				string path2 = Path.Combine(path, "blacklist.txt");
				SkidMenuPlugin.blacklistFolderPath = path2;
				bool flag = File.Exists(path2);
				if (flag)
				{
					string[] array = File.ReadAllLines(path2);
					foreach (string text in array)
					{
						string text2 = text.Trim();
						bool flag2 = !string.IsNullOrEmpty(text2);
						if (flag2)
						{
							SkidMenuPlugin.BlacklistedCodes.Add(text2.ToLower());
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("LoadBlacklist error: " + ex.Message);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00003428 File Offset: 0x00001628
		public static void SaveToBlacklist(string friendCode)
		{
			try
			{
				bool flag = string.IsNullOrWhiteSpace(friendCode);
				if (!flag)
				{
					friendCode = friendCode.Trim().ToLower();
					bool flag2 = SkidMenuPlugin.BlacklistedCodes.Contains(friendCode);
					if (!flag2)
					{
						SkidMenuPlugin.BlacklistedCodes.Add(friendCode);
						File.AppendAllText(SkidMenuPlugin.blacklistFolderPath, friendCode + System.Environment.NewLine);
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SaveToBlacklist error: " + ex.Message);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000034B8 File Offset: 0x000016B8
		public static void RemoveFromBlacklist(string friendCode)
		{
			try
			{
				bool flag = string.IsNullOrWhiteSpace(friendCode);
				if (!flag)
				{
					friendCode = friendCode.Trim().ToLower();
					SkidMenuPlugin.BlacklistedCodes.Remove(friendCode);
					bool flag2 = File.Exists(SkidMenuPlugin.blacklistFolderPath);
					if (flag2)
					{
						System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(File.ReadAllLines(SkidMenuPlugin.blacklistFolderPath));
						list.RemoveAll((string l) => l.Trim().ToLower() == friendCode);
						File.WriteAllLines(SkidMenuPlugin.blacklistFolderPath, list.ToArray());
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("RemoveFromBlacklist error: " + ex.Message);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003588 File Offset: 0x00001788
		public static bool IsPlayerBlacklisted(PlayerControl player)
		{
			bool result;
			try
			{
				bool flag = player == null || player.Data == null;
				if (flag)
				{
					result = false;
				}
				else
				{
					string text = "";
					try
					{
						text = player.FriendCode;
					}
					catch
					{
					}
					bool flag2 = string.IsNullOrEmpty(text);
					if (flag2)
					{
						try
						{
							text = player.Data.FriendCode;
						}
						catch
						{
						}
					}
					bool flag3 = string.IsNullOrEmpty(text) && AmongUsClient.Instance != null;
					if (flag3)
					{
						try
						{
							ClientData client = AmongUsClient.Instance.GetClient(player.Data.ClientId);
							bool flag4 = client != null;
							if (flag4)
							{
								text = client.FriendCode;
							}
						}
						catch
						{
						}
					}
					bool flag5 = string.IsNullOrEmpty(text);
					if (flag5)
					{
						result = false;
					}
					else
					{
						result = SkidMenuPlugin.BlacklistedCodes.Contains(text.ToLower().Trim());
					}
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("IsPlayerBlacklisted error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000036C4 File Offset: 0x000018C4
		public static void SendColouredChat()
		{
			try
			{
				bool flag = PlayerControl.LocalPlayer == null;
				if (!flag)
				{
					int num = UnityEngine.Random.Range(0, 10);
					switch (num)
					{
					case 0:
						SkidMenuPlugin.SendQuickChatRaw(3, 78, 1, 2, 1912);
						break;
					case 1:
						SkidMenuPlugin.SendQuickChatRaw(3, 78, 1, 2, 197);
						break;
					case 2:
						SkidMenuPlugin.SendQuickChatRaw(3, 78, 1, 2, 198);
						break;
					case 3:
						SkidMenuPlugin.SendQuickChatRaw(3, 78, 1, 2, 1913);
						break;
					case 4:
						SkidMenuPlugin.SendQuickChatRaw(3, 78, 1, 2, 1914);
						break;
					case 5:
						SkidMenuPlugin.SendQuickChatMega3();
						break;
					case 6:
						SkidMenuPlugin.SendQuickChatMega4();
						break;
					case 7:
						SkidMenuPlugin.SendQuickChatMega5();
						break;
					case 8:
						SkidMenuPlugin.SendQuickChatMega6();
						break;
					case 9:
						SkidMenuPlugin.SendQuickChatMega8();
						break;
					default:
						SkidMenuPlugin.SendQuickChatRaw(3, 78, 1, 2, 1912);
						break;
					}
					SkidMenuPlugin.Logger.LogInfo("Sent colored chat (random choice: " + num.ToString() + ")");
					SkidMenuPlugin.coloredChatIndex++;
				}
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("Send Coloured Chat error: " + ex.Message);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000382C File Offset: 0x00001A2C
		private static void SendQuickChatRaw(byte msg1, ushort msg2, byte msg3, byte msg4, ushort msg7)
		{
			try
			{
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, -1);
				messageWriter.Write(msg1);
				messageWriter.Write(msg2);
				messageWriter.Write(msg3);
				messageWriter.Write(msg4);
				messageWriter.Write(msg7);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, PlayerControl.LocalPlayer.OwnerId);
				messageWriter2.Write(msg1);
				messageWriter2.Write(msg2);
				messageWriter2.Write(msg3);
				messageWriter2.Write(msg4);
				messageWriter2.Write(msg7);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendQuickChatRaw error: " + ex.Message);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003914 File Offset: 0x00001B14
		private static void SendQuickChatMega3()
		{
			try
			{
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, -1);
				messageWriter.Write(3);
				messageWriter.Write(78);
				messageWriter.Write(3);
				messageWriter.Write(2);
				messageWriter.Write(1912);
				messageWriter.Write(2);
				messageWriter.Write(197);
				messageWriter.Write(2);
				messageWriter.Write(198);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, PlayerControl.LocalPlayer.OwnerId);
				messageWriter2.Write(3);
				messageWriter2.Write(78);
				messageWriter2.Write(3);
				messageWriter2.Write(2);
				messageWriter2.Write(1912);
				messageWriter2.Write(2);
				messageWriter2.Write(197);
				messageWriter2.Write(2);
				messageWriter2.Write(198);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendQuickChatMega3 error: " + ex.Message);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003A60 File Offset: 0x00001C60
		private static void SendQuickChatMega4()
		{
			try
			{
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, -1);
				messageWriter.Write(3);
				messageWriter.Write(78);
				messageWriter.Write(4);
				messageWriter.Write(2);
				messageWriter.Write(1912);
				messageWriter.Write(2);
				messageWriter.Write(197);
				messageWriter.Write(2);
				messageWriter.Write(198);
				messageWriter.Write(2);
				messageWriter.Write(1913);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, PlayerControl.LocalPlayer.OwnerId);
				messageWriter2.Write(3);
				messageWriter2.Write(78);
				messageWriter2.Write(4);
				messageWriter2.Write(2);
				messageWriter2.Write(1912);
				messageWriter2.Write(2);
				messageWriter2.Write(197);
				messageWriter2.Write(2);
				messageWriter2.Write(198);
				messageWriter2.Write(2);
				messageWriter2.Write(1913);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendQuickChatMega4 error: " + ex.Message);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003BD4 File Offset: 0x00001DD4
		private static void SendQuickChatMega5()
		{
			try
			{
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, -1);
				messageWriter.Write(3);
				messageWriter.Write(78);
				messageWriter.Write(5);
				messageWriter.Write(2);
				messageWriter.Write(1912);
				messageWriter.Write(2);
				messageWriter.Write(197);
				messageWriter.Write(2);
				messageWriter.Write(198);
				messageWriter.Write(2);
				messageWriter.Write(1913);
				messageWriter.Write(2);
				messageWriter.Write(1914);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, PlayerControl.LocalPlayer.OwnerId);
				messageWriter2.Write(3);
				messageWriter2.Write(78);
				messageWriter2.Write(5);
				messageWriter2.Write(2);
				messageWriter2.Write(1912);
				messageWriter2.Write(2);
				messageWriter2.Write(197);
				messageWriter2.Write(2);
				messageWriter2.Write(198);
				messageWriter2.Write(2);
				messageWriter2.Write(1913);
				messageWriter2.Write(2);
				messageWriter2.Write(1914);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendQuickChatMega5 error: " + ex.Message);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003D70 File Offset: 0x00001F70
		private static void SendQuickChatMega6()
		{
			try
			{
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, -1);
				messageWriter.Write(3);
				messageWriter.Write(78);
				messageWriter.Write(6);
				messageWriter.Write(2);
				messageWriter.Write(1912);
				messageWriter.Write(2);
				messageWriter.Write(197);
				messageWriter.Write(2);
				messageWriter.Write(198);
				messageWriter.Write(2);
				messageWriter.Write(1913);
				messageWriter.Write(2);
				messageWriter.Write(1914);
				messageWriter.Write(2);
				messageWriter.Write(1915);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, PlayerControl.LocalPlayer.OwnerId);
				messageWriter2.Write(3);
				messageWriter2.Write(78);
				messageWriter2.Write(6);
				messageWriter2.Write(2);
				messageWriter2.Write(1912);
				messageWriter2.Write(2);
				messageWriter2.Write(197);
				messageWriter2.Write(2);
				messageWriter2.Write(198);
				messageWriter2.Write(2);
				messageWriter2.Write(1913);
				messageWriter2.Write(2);
				messageWriter2.Write(1914);
				messageWriter2.Write(2);
				messageWriter2.Write(1915);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendQuickChatMega6 error: " + ex.Message);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003F34 File Offset: 0x00002134
		private static void SendQuickChatMega8()
		{
			try
			{
				MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, -1);
				messageWriter.Write(3);
				messageWriter.Write(78);
				messageWriter.Write(8);
				messageWriter.Write(2);
				messageWriter.Write(1912);
				messageWriter.Write(2);
				messageWriter.Write(197);
				messageWriter.Write(2);
				messageWriter.Write(198);
				messageWriter.Write(2);
				messageWriter.Write(1913);
				messageWriter.Write(2);
				messageWriter.Write(1914);
				messageWriter.Write(2);
				messageWriter.Write(1915);
				messageWriter.Write(2);
				messageWriter.Write(1912);
				messageWriter.Write(2);
				messageWriter.Write(197);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 33, SendOption.None, PlayerControl.LocalPlayer.OwnerId);
				messageWriter2.Write(3);
				messageWriter2.Write(78);
				messageWriter2.Write(8);
				messageWriter2.Write(2);
				messageWriter2.Write(1912);
				messageWriter2.Write(2);
				messageWriter2.Write(197);
				messageWriter2.Write(2);
				messageWriter2.Write(198);
				messageWriter2.Write(2);
				messageWriter2.Write(1913);
				messageWriter2.Write(2);
				messageWriter2.Write(1914);
				messageWriter2.Write(2);
				messageWriter2.Write(1915);
				messageWriter2.Write(2);
				messageWriter2.Write(1912);
				messageWriter2.Write(2);
				messageWriter2.Write(197);
				AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
			}
			catch (System.Exception ex)
			{
				SkidMenuPlugin.Logger.LogError("SendQuickChatMega8 error: " + ex.Message);
			}
		}

		// Token: 0x04000001 RID: 1
		public const string PluginGuid = "com.skid.menu";

		// Token: 0x04000002 RID: 2
		public const string PluginName = "SkidMenu";

		// Token: 0x04000003 RID: 3
		public const string PluginVersion = "1.0.7";

		// Token: 0x04000004 RID: 4
		public static bool ShowMenu = false;

		// Token: 0x04000005 RID: 5
		public static bool IsScanning = false;

		// Token: 0x04000006 RID: 6
		public static bool AnimShieldsEnabled = false;

		// Token: 0x04000007 RID: 7
		public static bool AnimAsteroidsEnabled = false;

		// Token: 0x04000008 RID: 8
		public static bool AnimEmptyGarbageEnabled = false;

		// Token: 0x04000009 RID: 9
		public static bool AnimCamsInUseEnabled = false;

		// Token: 0x0400000A RID: 10
		public static float GameSpeed = 1f;

		// Token: 0x0400000B RID: 11
		public static string ActiveTab = "About";

		// Token: 0x0400000C RID: 12
		public static bool ShowForceRolesMenu = false;

		// Token: 0x0400000D RID: 13
		public static System.Collections.Generic.Dictionary<int, RoleTypes> forcedRoles = new System.Collections.Generic.Dictionary<int, RoleTypes>();

		// Token: 0x0400000E RID: 14
		public static int selectedForceRolePlayerId = -1;

		// Token: 0x0400000F RID: 15
		public static RoleTypes selectedRoleType = RoleTypes.Crewmate;

		// Token: 0x04000010 RID: 16
		public static bool showRoleDropdown = false;

		// Token: 0x04000011 RID: 17
		public static int dropdownPlayerIndex = -1;

		// Token: 0x04000012 RID: 18
		public static bool FindDatersEnabled = false;

		// Token: 0x04000013 RID: 19
		public static bool ExtendedLobbyEnabled = false;

		// Token: 0x04000014 RID: 20
		public static bool DestroyLobbyEnabled = false;

		// Token: 0x04000015 RID: 21
		public const byte MENU_RPC_ID = 121;

		// Token: 0x04000016 RID: 22
		public const byte TUFFMENU_RPC_ID = 167;

		// Token: 0x04000017 RID: 23
		public const byte SICKOMENU_RPC_ID = 164;

		// Token: 0x04000018 RID: 24
		public const byte AMONGUSMENU_RPC_ID = 85;

		// Token: 0x04000019 RID: 25
		public const byte BETTERAMONGUS_RPC_ID = 150;

		// Token: 0x0400001A RID: 26
		public const byte KILLNETWORK_RPC_ID = 250;

		// Token: 0x0400001B RID: 27
		public const byte HOSTGUARD_RPC_ID = 176;

		// Token: 0x0400001C RID: 28
		public const byte GOATNETCLIENT_RPC_ID = 154;

		// Token: 0x0400001D RID: 29
		public const byte NETMENU_RPC_ID = 162;

		// Token: 0x0400001E RID: 30
		public static bool SpoofMenuEnabled = false;

		// Token: 0x0400001F RID: 31
		public static int selectedSpoofMenuIndex = 0;

		// Token: 0x04000020 RID: 32
		public static readonly string[] spoofMenuNames = new string[]
		{
			"SkidMenu (Default)",
			"TuffMenu",
			"SickoMenu",
			"AmongUsMenu",
			"BetterAmongUs",
			"KillNetwork",
			"HostGuard",
			"GoatNetClient",
			"NetMenu"
		};

		// Token: 0x04000021 RID: 33
		public static readonly byte[] spoofMenuRPCs = new byte[]
		{
			121,
			167,
			164,
			85,
			150,
			250,
			176,
			154,
			162
		};

		// Token: 0x04000022 RID: 34
		public static bool ShowKillCooldown = false;

		// Token: 0x04000023 RID: 35
		public static bool NoClipEnabled = false;

		// Token: 0x04000024 RID: 36
		public static bool SpinEnabled = false;

		// Token: 0x04000025 RID: 37
		public static bool ExileMeEnabled = false;

		// Token: 0x04000026 RID: 38
		public static bool AntiOverloadEnabled = false;

		// Token: 0x04000027 RID: 39
		public static bool KillNotificationEnabled = false;

		// Token: 0x04000028 RID: 40
		public static bool AutoCopyCodeEnabled = false;

		// Token: 0x04000029 RID: 41
		public static float spinAngle = 0f;

		// Token: 0x0400002A RID: 42
		public static bool ShowPlayerInfo = false;

		// Token: 0x0400002B RID: 43
		public static bool KillOtherImpostersEnabled = false;

		// Token: 0x0400002C RID: 44
		public static bool KillAllEnabled = false;

		// Token: 0x0400002D RID: 45
		public static bool ShowVotekickInfo = false;

		// Token: 0x0400002E RID: 46
		public static bool ImpostorTasksEnabled = false;

		// Token: 0x0400002F RID: 47
		public static bool TeleportToCursorEnabled = false;

		// Token: 0x04000030 RID: 48
		public static bool AlwaysShowChatEnabled = false;

		// Token: 0x04000031 RID: 49
		public static bool SeeGhostsEnabled = false;

		// Token: 0x04000032 RID: 50
		public static bool CosmeticsUnlockerEnabled = true;

		// Token: 0x04000033 RID: 51
		public static bool MoreLobbyInfoEnabled = false;

		// Token: 0x04000034 RID: 52
		public static bool AvoidPenaltiesEnabled = true;

		// Token: 0x04000035 RID: 53
		public static bool NoShadowsEnabled = false;

		// Token: 0x04000036 RID: 54
		public static bool ZoomOutEnabled = false;

		// Token: 0x04000037 RID: 55
		public static bool RevealVotesEnabled = false;

		// Token: 0x04000038 RID: 56
		public static bool SeeRolesEnabled = false;

		// Token: 0x04000039 RID: 57
		public static bool ShowHostEnabled = false;

		// Token: 0x0400003A RID: 58
		public static bool DarkModeEnabled = false;

		// Token: 0x0400003B RID: 59
		public static bool SeeModUsersEnabled = true;

		// Token: 0x0400003C RID: 60
		public static System.Collections.Generic.Dictionary<byte, byte> detectedModUsers = new System.Collections.Generic.Dictionary<byte, byte>();

		// Token: 0x0400003D RID: 61
		private static System.Collections.Generic.Dictionary<byte, float> lastDetectionTime = new System.Collections.Generic.Dictionary<byte, float>();

		// Token: 0x0400003E RID: 62
		private const float DETECTION_COOLDOWN = 2f;

		// Token: 0x0400003F RID: 63
		public static bool SetFakeRoleEnabled = false;

		// Token: 0x04000040 RID: 64
		public static RoleTypes SelectedFakeRole = RoleTypes.Crewmate;

		// Token: 0x04000041 RID: 65
		public static RoleTypes? OriginalRole = null;

		// Token: 0x04000042 RID: 66
		private static bool fakeRoleDropdownOpen = false;

		// Token: 0x04000043 RID: 67
		private static Vector2 fakeRoleScrollPos = Vector2.zero;

		// Token: 0x04000044 RID: 68
		public static bool EndlessVentTime = false;

		// Token: 0x04000045 RID: 69
		public static bool NoVentCooldown = false;

		// Token: 0x04000046 RID: 70
		public static bool NoVitalsCooldown = false;

		// Token: 0x04000047 RID: 71
		public static bool EndlessBattery = false;

		// Token: 0x04000048 RID: 72
		public static bool NoTrackingCooldown = false;

		// Token: 0x04000049 RID: 73
		public static bool NoTrackingDelay = false;

		// Token: 0x0400004A RID: 74
		public static bool EndlessTracking = false;

		// Token: 0x0400004B RID: 75
		public static bool EndlessShapeshiftDuration = false;

		// Token: 0x0400004C RID: 76
		public static bool NoShapeshiftAnimation = false;

		// Token: 0x0400004D RID: 77
		public static bool UnlimitedKillRange = false;

		// Token: 0x0400004E RID: 78
		public static bool UnlimitedInterrogateRange = false;

		// Token: 0x0400004F RID: 79
		public const byte SkidMenu_RPC_ID = 121;

		// Token: 0x04000050 RID: 80
		public static bool StealthMode = false;

		// Token: 0x04000051 RID: 81
		private static float lastIdentificationRpcTime = 0f;

		// Token: 0x04000052 RID: 82
		private static float identificationRpcInterval = 10f;

		// Token: 0x04000053 RID: 83
		public static bool RGBMode = false;

		// Token: 0x04000054 RID: 84
		private static float rgbHue = 0f;

		// Token: 0x04000055 RID: 85
		private static float breatheIntensity = 1f;

		// Token: 0x04000056 RID: 86
		private static bool breatheIncreasing = false;

		// Token: 0x04000057 RID: 87
		private static float currentHueTarget = 0f;

		// Token: 0x04000058 RID: 88
		private static bool colorLocked = false;

		// Token: 0x04000059 RID: 89
		private static Color _cachedRGBColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);

		// Token: 0x0400005A RID: 90
		private static Color _cachedRGBAccent = new Color(0.15f, 0.15f, 0.2f, 1f);

		// Token: 0x0400005B RID: 91
		private static Color _cachedRGBText = Color.white;

		// Token: 0x0400005C RID: 92
		public static bool RandomizeOutfit = false;

		// Token: 0x0400005D RID: 93
		public static float nextRandomTime = 0f;

		// Token: 0x0400005E RID: 94
		public static bool DisableVotekicks = false;

		// Token: 0x0400005F RID: 95
		public static bool DisableMeetings = false;

		// Token: 0x04000060 RID: 96
		public static bool GodModeEnabled = false;

		// Token: 0x04000061 RID: 97
		public static bool DisableSabotagesEnabled = false;

		// Token: 0x04000062 RID: 98
		public static bool DisableGameEndEnabled = false;

		// Token: 0x04000063 RID: 99
		public static bool ShowLobbyTimerEnabled = true;

		// Token: 0x04000064 RID: 100
		public static bool ShowForceColorMenu = false;

		// Token: 0x04000065 RID: 101
		public static System.Collections.Generic.Dictionary<int, byte> forcedColors = new System.Collections.Generic.Dictionary<int, byte>();

		// Token: 0x04000066 RID: 102
		public static int selectedForceColorId = 0;

		// Token: 0x04000067 RID: 103
		public static bool showColorDropdown = false;

		// Token: 0x04000068 RID: 104
		public static int dropdownPlayerIndexColor = -1;

		// Token: 0x04000069 RID: 105
		public static byte selectedGlobalColor = 0;

		// Token: 0x0400006A RID: 106
		public static bool OverloadInfoEnabled = false;

		// Token: 0x0400006B RID: 107
		public static bool SpamRepairSabotages = false;

		// Token: 0x0400006C RID: 108
		public static bool reactorActive = false;

		// Token: 0x0400006D RID: 109
		public static bool oxygenActive = false;

		// Token: 0x0400006E RID: 110
		public static bool commsActive = false;

		// Token: 0x0400006F RID: 111
		public static bool lightsActive = false;

		// Token: 0x04000070 RID: 112
		public static bool unfixableLightsActive = false;

		// Token: 0x04000071 RID: 113
		public static bool AnticheatEnabled = false;

		// Token: 0x04000072 RID: 114
		public static bool AutoBanEnabled = false;

		// Token: 0x04000073 RID: 115
		public static bool CheckInvalidCompleteTask = true;

		// Token: 0x04000074 RID: 116
		public static bool CheckInvalidPlayAnimation = true;

		// Token: 0x04000075 RID: 117
		public static bool CheckInvalidScanner = true;

		// Token: 0x04000076 RID: 118
		public static bool CheckInvalidVent = true;

		// Token: 0x04000077 RID: 119
		public static bool CheckInvalidSnapTo = true;

		// Token: 0x04000078 RID: 120
		public static bool CheckInvalidStartCounter = true;

		// Token: 0x04000079 RID: 121
		public static bool CheckSpoofedPlatforms = true;

		// Token: 0x0400007A RID: 122
		public static bool CheckSpoofedLevels = true;

		// Token: 0x0400007B RID: 123
		public static int totalDetections = 0;

		// Token: 0x0400007C RID: 124
		public static System.Collections.Generic.List<string> detectionLog = new System.Collections.Generic.List<string>();

		// Token: 0x0400007D RID: 125
		public static HashSet<string> BlacklistedCodes = new HashSet<string>();

		// Token: 0x0400007E RID: 126
		public static HashSet<string> notifiedBlacklistedPlayers = new HashSet<string>();

		// Token: 0x0400007F RID: 127
		public static string blacklistFolderPath = "";

		// Token: 0x04000080 RID: 128
		public static bool BanBlacklistedEnabled = false;

		// Token: 0x04000081 RID: 129
		public static bool OverloadEnabled = false;

		// Token: 0x04000082 RID: 130
		public static bool OverloadMethod2Enabled = false;

		// Token: 0x04000083 RID: 131
		public static bool TargetedOverloadEnabled = false;

		// Token: 0x04000084 RID: 132
		public static bool BreakCounterEnabled = false;

		// Token: 0x04000085 RID: 133
		public static int selectedTargetId = -1;

		// Token: 0x04000086 RID: 134
		public static int targetedOverloadMethod = 1;

		// Token: 0x04000087 RID: 135
		public static bool OverloadMethod3Enabled = false;

		// Token: 0x04000088 RID: 136
		public static bool OverloadMethod4Enabled = false;

		// Token: 0x04000089 RID: 137
		public static bool LagEveryoneEnabled = false;

		// Token: 0x0400008A RID: 138
		public static bool OverflowMethod1Enabled = false;

		// Token: 0x0400008B RID: 139
		public static bool OverflowMethod2Enabled = false;

		// Token: 0x0400008C RID: 140
		public static int selectedMapId = 0;

		// Token: 0x0400008D RID: 141
		public static int selectedVotekickTargetId = -1;

		// Token: 0x0400008E RID: 142
		public static bool VotekickAllEnabled = false;

		// Token: 0x0400008F RID: 143
		private static HashSet<int> votekickedPlayerIds = new HashSet<int>();

		// Token: 0x04000090 RID: 144
		public static int selectedHostKickTargetId = -1;

		// Token: 0x04000091 RID: 145
		public static ConfigEntry<KeyCode> MenuKey;

		// Token: 0x04000092 RID: 146
		public static ConfigEntry<string> SpoofPlatform;

		// Token: 0x04000093 RID: 147
		public static ConfigEntry<int> Config_SpoofLevel;

		// Token: 0x04000094 RID: 148
		public static bool isChangingKey = false;

		// Token: 0x04000095 RID: 149
		public static ManualLogSource Logger;

		// Token: 0x04000096 RID: 150
		public static ConfigEntry<bool> Config_ShowKillCooldown;

		// Token: 0x04000097 RID: 151
		public static ConfigEntry<bool> Config_NoClipEnabled;

		// Token: 0x04000098 RID: 152
		public static ConfigEntry<bool> Config_SpinEnabled;

		// Token: 0x04000099 RID: 153
		public static ConfigEntry<bool> Config_ExileMeEnabled;

		// Token: 0x0400009A RID: 154
		public static ConfigEntry<bool> Config_AntiOverloadEnabled;

		// Token: 0x0400009B RID: 155
		public static ConfigEntry<bool> Config_KillNotificationEnabled;

		// Token: 0x0400009C RID: 156
		public static ConfigEntry<bool> Config_AutoCopyCodeEnabled;

		// Token: 0x0400009D RID: 157
		public static ConfigEntry<bool> Config_ShowPlayerInfo;

		// Token: 0x0400009E RID: 158
		public static ConfigEntry<bool> Config_KillOtherImpostersEnabled;

		// Token: 0x0400009F RID: 159
		public static ConfigEntry<bool> Config_ShowVotekickInfo;

		// Token: 0x040000A0 RID: 160
		public static ConfigEntry<bool> Config_RGBMode;

		// Token: 0x040000A1 RID: 161
		public static ConfigEntry<bool> Config_StealthMode;

		// Token: 0x040000A2 RID: 162
		public static ConfigEntry<bool> Config_RandomizeOutfit;

		// Token: 0x040000A3 RID: 163
		public static ConfigEntry<bool> Config_BanBlacklistedEnabled;

		// Token: 0x040000A4 RID: 164
		public static ConfigEntry<bool> Config_DisableVotekicks;

		// Token: 0x040000A5 RID: 165
		public static ConfigEntry<bool> Config_DisableMeetings;

		// Token: 0x040000A6 RID: 166
		public static ConfigEntry<bool> Config_GodModeEnabled;

		// Token: 0x040000A7 RID: 167
		public static ConfigEntry<bool> Config_DisableSabotagesEnabled;

		// Token: 0x040000A8 RID: 168
		public static ConfigEntry<bool> Config_DisableGameEndEnabled;

		// Token: 0x040000A9 RID: 169
		public static ConfigEntry<bool> Config_OverloadInfoEnabled;

		// Token: 0x040000AA RID: 170
		public static ConfigEntry<bool> Config_AnticheatEnabled;

		// Token: 0x040000AB RID: 171
		public static ConfigEntry<bool> Config_AutoBanEnabled;

		// Token: 0x040000AC RID: 172
		public static ConfigEntry<bool> Config_CheckInvalidCompleteTask;

		// Token: 0x040000AD RID: 173
		public static ConfigEntry<bool> Config_CheckInvalidPlayAnimation;

		// Token: 0x040000AE RID: 174
		public static ConfigEntry<bool> Config_CheckInvalidScanner;

		// Token: 0x040000AF RID: 175
		public static ConfigEntry<bool> Config_CheckInvalidVent;

		// Token: 0x040000B0 RID: 176
		public static ConfigEntry<bool> Config_CheckInvalidSnapTo;

		// Token: 0x040000B1 RID: 177
		public static ConfigEntry<bool> Config_CheckInvalidStartCounter;

		// Token: 0x040000B2 RID: 178
		public static ConfigEntry<bool> Config_CheckSpoofedPlatforms;

		// Token: 0x040000B3 RID: 179
		public static ConfigEntry<bool> Config_CheckSpoofedLevels;

		// Token: 0x040000B4 RID: 180
		public static ConfigEntry<bool> Config_FindDatersEnabled;

		// Token: 0x040000B5 RID: 181
		public static ConfigEntry<bool> Config_ExtendedLobbyEnabled;

		// Token: 0x040000B6 RID: 182
		public static ConfigEntry<bool> Config_DestroyLobbyEnabled;

		// Token: 0x040000B7 RID: 183
		public static ConfigEntry<bool> Config_SpamChatEnabled;

		// Token: 0x040000B8 RID: 184
		public static ConfigEntry<bool> Config_VotekickAllEnabled;

		// Token: 0x040000B9 RID: 185
		public static ConfigEntry<bool> Config_TeleportToCursorEnabled;

		// Token: 0x040000BA RID: 186
		public static ConfigEntry<bool> Config_AlwaysShowChatEnabled;

		// Token: 0x040000BB RID: 187
		public static ConfigEntry<bool> Config_RevealRolesEnabled;

		// Token: 0x040000BC RID: 188
		public static ConfigEntry<bool> Config_RevealVotesEnabled;

		// Token: 0x040000BD RID: 189
		public static ConfigEntry<bool> Config_ZoomOutEnabled;

		// Token: 0x040000BE RID: 190
		public static ConfigEntry<bool> Config_SeeGhostsEnabled;

		// Token: 0x040000BF RID: 191
		public static ConfigEntry<bool> Config_MoreLobbyInfoEnabled;

		// Token: 0x040000C0 RID: 192
		public static ConfigEntry<bool> Config_ShowHostEnabled;

		// Token: 0x040000C1 RID: 193
		public static ConfigEntry<bool> Config_SeeModUsersEnabled;

		// Token: 0x040000C2 RID: 194
		public static ConfigEntry<bool> Config_DarkModeEnabled;

		// Token: 0x040000C3 RID: 195
		public static ConfigEntry<bool> Config_NoShadowsEnabled;

		// Token: 0x040000C4 RID: 196
		public static ConfigEntry<bool> Config_ShowLobbyTimerEnabled;

		// Token: 0x040000C5 RID: 197
		public static ConfigEntry<bool> Config_SpoofMenuEnabled;

		// Token: 0x040000C6 RID: 198
		public static ConfigEntry<int> Config_SpoofMenuIndex;

		// Token: 0x040000C7 RID: 199
		public static ConfigEntry<bool> Config_EndlessVentTime;

		// Token: 0x040000C8 RID: 200
		public static ConfigEntry<bool> Config_NoVentCooldown;

		// Token: 0x040000C9 RID: 201
		public static ConfigEntry<bool> Config_NoVitalsCooldown;

		// Token: 0x040000CA RID: 202
		public static ConfigEntry<bool> Config_EndlessBattery;

		// Token: 0x040000CB RID: 203
		public static ConfigEntry<bool> Config_NoTrackingCooldown;

		// Token: 0x040000CC RID: 204
		public static ConfigEntry<bool> Config_NoTrackingDelay;

		// Token: 0x040000CD RID: 205
		public static ConfigEntry<bool> Config_EndlessTracking;

		// Token: 0x040000CE RID: 206
		public static ConfigEntry<bool> Config_EndlessShapeshiftDuration;

		// Token: 0x040000CF RID: 207
		public static ConfigEntry<bool> Config_NoShapeshiftAnimation;

		// Token: 0x040000D0 RID: 208
		public static ConfigEntry<bool> Config_UnlimitedKillRange;

		// Token: 0x040000D1 RID: 209
		public static ConfigEntry<bool> Config_ImpostorTasksEnabled;

		// Token: 0x040000D2 RID: 210
		public static ConfigEntry<bool> Config_UnlimitedInterrogateRange;

		// Token: 0x040000D3 RID: 211
		public static int selectedVentId = 0;

		// Token: 0x040000D4 RID: 212
		public static string[] ventNames = new string[]
		{
			"Admin",
			"Hallway",
			"Cafeteria",
			"Electrical",
			"Upper Engine",
			"Security",
			"Medbay",
			"Weapons",
			"Reactor (Bottom)",
			"Lower Engine",
			"Shields",
			"Reactor (Top)",
			"Navigation"
		};

		// Token: 0x040000D5 RID: 213
		public static string[] weirdMessages = new string[]
		{
			"Epstein didn't kill himself, but Diddy's baby oil collection did kill the vibe",
			"I vented into your mom's room last night",
			"The Impostor is actually your dad who left to get milk 10 years ago",
			"I'm not the Impostor, I'm just your sleep paralysis demon",
			"Red sus? Nah, your search history is sus",
			"I saw Blue doing tasks... with your girlfriend",
			"Emergency meeting: Who asked?",
			"If you vote me out, you're adopted",
			"I'm the Impostor and my lawyer advised me to not finish this sentence",
			"Crewmate? More like crew-MISTAKE because you suck",
			"I'm not saying Yellow is sus, but they definitely know what happened in 1989",
			"I didn't vent, I used creative mode",
			"Guys I think the Impostor is inside the walls",
			"If being sus is a crime then call me Jeffrey Dahmer",
			"I saw the Impostor do the Macarena on a dead body in cafeteria",
			"This lobby smells like broke and it's coming from everyone except me",
			"I'm not venting, I'm just visiting my summer home in the vents",
			"Red is gayer than a rainbow in a Pride parade during gay month",
			"If you don't vote Yellow, your pillow will be warm on both sides tonight",
			"I'm the Impostor and I've been trying to reach you about your car's extended warranty",
			"POV: You're in electrical and you hear the vents start speaking Spanish",
			"Fun fact: A group of crows is called a murder, just like what I did in electrical",
			"Did you know? The average person walks past 36 murderers in their lifetime. I'm 12 of them",
			"Statistically, you're more likely to die from me than a vending machine",
			"Fun fact: Dolphins are known to grape other dolphins. Red is a dolphin",
			"The human body contains enough bones to make an entire skeleton. Wanna see?",
			"Octopi have 3 hearts. I stole all of them from my victims",
			"Fun fact: You're breathing manually now. Also I'm the Impostor",
			"Bananas are berries but strawberries aren't. Also I killed in admin",
			"A day on Venus is longer than its year. This meeting is longer than both",
			"Your tongue never sits comfortably in your mouth. Vote Yellow btw",
			"I'm not sus, you're just racist against red people",
			"Yellow was not the Impostor. But they should've been for those shoes",
			"I saw Pink kill... my faith in humanity",
			"If I'm the Impostor then why am I so good looking? Checkmate",
			"Red vented? No, he's just good at parkour",
			"This isn't a democracy, it's a dictatorship and I'm voting Blue",
			"I'm clearing Yellow. We were busy in electrical if you know what I mean",
			"Skip? No, I don't skip leg day unlike you skinny crewmates",
			"Someone called the emergency meeting just to tell us they're vegan",
			"I'm the Impostor and my pronouns are was/were because you're all dead",
			"Green is clear, we compared credit scores in admin",
			"I trust Blue less than I trust gas station sushi",
			"If you're voting on 7 you're dumber than a bag of hammers",
			"This meeting is more pointless than a screen door on a submarine",
			"I've seen better detective work from Scooby Doo on ketamine",
			"I'm not the Impostor, I just have a very particular set of skills",
			"Fun fact: You lose 1% of brain cells every time you play Among Us. You're at -50%",
			"I saw Orange fake tasks harder than my dad faked loving me",
			"This crew has the combined IQ of a participation trophy",
			"I'm voting randomly because democracy is a lie anyway",
			"Red told me he's the Impostor in Morse code using his blinks. Trust me bro",
			"If aliens are watching us play Among Us, they're not coming to Earth",
			"I'm not throwing, I'm just aggressively bad at the game",
			"This lobby is what happens when cousins marry cousins",
			"I'd rather be waterboarded at Guantanamo Bay than finish this game with you",
			"It's giving Impostor. It's giving murder. It's giving electrical body",
			"No thoughts, head empty, just vibes and murder",
			"I'm not like other Impostors, I'm worse",
			"Main character energy but the character is the Impostor",
			"I didn't come here to make friends, I came here to commit vehicular manslaughter",
			"That's not very cash money of you to vote me",
			"I'm baby. I'm baby Impostor. I can't go to jail",
			"This is my villain origin story and you're all extras",
			"I'm not saying I'm Batman, but have you ever seen me and Batman in the same room?",
			"I have the moral backbone of a chocolate eclair but I'm not the Impostor",
			"Your gameplay is what birth control commercials are based on",
			"I've seen more intelligence in a jar of mayonnaise",
			"You play Among Us like you're trying to speedrun unemployment",
			"Your detective skills are on par with Helen Keller playing Where's Waldo",
			"I'd call you a clown but that's an insult to circus performers",
			"You're the human equivalent of a participation award",
			"Your IQ is lower than your credit score and that's saying something",
			"You have the awareness of a blind kid in a dark room looking for a black cat that isn't there",
			"I'm not saying you're dumb, but you make a rock look like Einstein",
			"Your brain is smoother than a marble floor covered in butter",
			"I declare bankruptcy! Oh wait wrong game. Still voting Blue tho",
			"According to my calculations, which are never wrong, Yellow is the Impostor. I can't do math",
			"I'm using my woman's intuition and it says Red. I'm a man btw",
			"My ancestors are smiling at me Imperials, can you say the same? Vote Purple",
			"I had a vision from God and he said skip",
			"I asked my Magic 8 Ball and it said Orange is sus",
			"The voices in my head are saying it's Green. The voices are never wrong except always",
			"I'm voting based on astrology. Mercury is in retrograde so it's Yellow",
			"My therapist says I need to work on trust issues so I'm trusting no one. Vote everyone",
			"I consulted the Elder Scrolls and they said Brown is the Impostor",
			"This joke has been beaten more than my dad beat me. Vote Red",
			"I'm running out of creative insults so just vote randomly",
			"This is my 47th game today. I have no life. Also Blue is sus",
			"I should be studying for my exam tomorrow but instead I'm here. Worth it. Skip",
			"My parents are disappointed in me and they're right. Vote Yellow",
			"I haven't touched grass in 3 weeks and it shows in my gameplay",
			"I'm addicted to Among Us like it's 2020. Someone help me. Vote Purple",
			"This game is proof that democracy doesn't work",
			"I've spent more time in electrical than I have with my family",
			"My social skills peaked in 2019 and it's all downhill from here",
			"I haven't seen this much chaos since the French Revolution. Vote everyone",
			"This is like the Salem Witch Trials but with worse evidence",
			"We're witch hunting harder than McCarthy in the 1950s",
			"This meeting has more drama than the fall of Rome",
			"I trust this crew as much as Julius Caesar trusted the Senate",
			"We're making decisions like it's the Nuremberg Trials but dumber",
			"This is giving Trail of Tears energy and I don't like it",
			"I've seen better teamwork from the Donner Party",
			"This lobby is like the Titanic: full of bad decisions and about to sink",
			"We're fumbling harder than the French at Waterloo",
			"I'm the Walter White of Among Us. I am the danger. I am the one who knocks. Vote Blue",
			"It's Britney, b*tch. And Britney says Yellow is the Impostor",
			"I'm too hot (hot damn) to be the Impostor. Make a dragon wanna retire man",
			"I'm just Ken and I killed in Barbie Land",
			"Oppenheimer didn't feel this bad about mass destruction. Vote Pink",
			"I'm having a Joker moment and society is to blame. Also I vented",
			"What would Scooby Doo? He'd vote Orange and he'd be right",
			"I'm Batman. The Dark Knight. The Impostor. Wait, I mean I'm not the Impostor",
			"I'm lactose intolerant but I'm tolerating this BS even less. Vote someone",
			"This game is more rigged than a carnival game. Skip",
			"I have trust issues and daddy issues. Mostly daddy issues. Vote Red because he looks like my dad",
			"My standards are low but holy f*ck. Vote everyone",
			"I'm not drunk but I wish I was. Yellow sus",
			"This lobby is more toxic than Chernobyl",
			"I've seen better organization from a monkey knife fight",
			"If stupidity was a superpower, this crew would be the Avengers",
			"I'm going to vote and then cry myself to sleep. Standard Tuesday",
			"This game makes me want to uninstall life",
			"I have the emotional stability of a Jenga tower in an earthquake"
		};

		// Token: 0x040000D6 RID: 214
		public static int coloredChatIndex = 0;

		// Token: 0x02000005 RID: 5
		public static class OverloadInfoSystem
		{
			// Token: 0x06000019 RID: 25 RVA: 0x0000519C File Offset: 0x0000339C
			public static void ShowWarning(string message)
			{
				try
				{
					bool flag = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
					if (flag)
					{
						DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=orange>[Overload Info]</color> " + message, true);
					}
					SkidMenuPlugin.Logger.LogWarning("[Overload Info] " + message);
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ShowWarning error: " + ex.Message);
				}
			}

			// Token: 0x0600001A RID: 26 RVA: 0x0000523C File Offset: 0x0000343C
			public static void ShowCriticalWarning(string message)
			{
				try
				{
					bool flag = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
					if (flag)
					{
						DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[?? OVERLOAD DETECTED]</color> " + message, true);
					}
					SkidMenuPlugin.Logger.LogError("[Overload Info - CRITICAL] " + message);
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ShowCriticalWarning error: " + ex.Message);
				}
			}

			// Token: 0x0600001B RID: 27 RVA: 0x000052DC File Offset: 0x000034DC
			public static void Reset()
			{
				SkidMenuPlugin.OverloadInfoSystem.playerRpcTimestamps.Clear();
				SkidMenuPlugin.OverloadInfoSystem.playerTotalRpcs.Clear();
				SkidMenuPlugin.OverloadInfoSystem.clientGameDataTimestamps.Clear();
				SkidMenuPlugin.OverloadInfoSystem.clientGameDataSizes.Clear();
				SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Clear();
				SkidMenuPlugin.OverloadInfoSystem.attackerDetails.Clear();
				SkidMenuPlugin.OverloadInfoSystem.attackerMethods.Clear();
				SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Clear();
				SkidMenuPlugin.OverloadInfoSystem.method3Details.Clear();
				SkidMenuPlugin.OverloadInfoSystem.attackCounts.Clear();
				SkidMenuPlugin.OverloadInfoSystem.firstDetectionTime.Clear();
				SkidMenuPlugin.Logger.LogInfo("Overload Info detections reset");
			}

			// Token: 0x0600001C RID: 28 RVA: 0x00005374 File Offset: 0x00003574
			public static bool CheckOverloadRpc(PlayerControl player, byte callId)
			{
				byte playerId = player.PlayerId;
				NetworkedPlayerInfo data = player.Data;
				string text = ((data != null) ? data.PlayerName : null) ?? "Unknown";
				float time = Time.time;
				bool flag = !SkidMenuPlugin.OverloadInfoSystem.playerRpcTimestamps.ContainsKey(playerId);
				if (flag)
				{
					SkidMenuPlugin.OverloadInfoSystem.playerRpcTimestamps[playerId] = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>();
					SkidMenuPlugin.OverloadInfoSystem.playerTotalRpcs[playerId] = new System.Collections.Generic.Queue<float>();
				}
				bool flag2 = !SkidMenuPlugin.OverloadInfoSystem.playerRpcTimestamps[playerId].ContainsKey(callId);
				if (flag2)
				{
					SkidMenuPlugin.OverloadInfoSystem.playerRpcTimestamps[playerId][callId] = new System.Collections.Generic.Queue<float>();
				}
				System.Collections.Generic.Queue<float> queue = SkidMenuPlugin.OverloadInfoSystem.playerRpcTimestamps[playerId][callId];
				System.Collections.Generic.Queue<float> queue2 = SkidMenuPlugin.OverloadInfoSystem.playerTotalRpcs[playerId];
				while (queue.Count > 0 && queue.Peek() < time - 1f)
				{
					queue.Dequeue();
				}
				while (queue2.Count > 0 && queue2.Peek() < time - 1f)
				{
					queue2.Dequeue();
				}
				queue.Enqueue(time);
				queue2.Enqueue(time);
				bool flag3 = false;
				string text2 = "";
				bool flag4 = queue.Count > 15;
				if (flag4)
				{
					flag3 = true;
					string rpcName = SkidMenuPlugin.OverloadInfoSystem.GetRpcName(callId);
					text2 = rpcName + " spam (" + queue.Count.ToString() + "/sec)";
				}
				bool flag5 = queue2.Count > 50;
				if (flag5)
				{
					flag3 = true;
					bool flag6 = string.IsNullOrEmpty(text2);
					if (flag6)
					{
						text2 = "Mass RPC spam (" + queue2.Count.ToString() + "/sec)";
					}
				}
				int num = 0;
				float num2 = 0.1f;
				foreach (float num3 in queue)
				{
					bool flag7 = num3 > time - num2;
					if (flag7)
					{
						num++;
					}
				}
				bool flag8 = num > 5;
				if (flag8)
				{
					flag3 = true;
					string rpcName2 = SkidMenuPlugin.OverloadInfoSystem.GetRpcName(callId);
					text2 = rpcName2 + " burst (" + num.ToString() + " in 0.1s)";
				}
				bool flag9 = flag3;
				bool result;
				if (flag9)
				{
					bool flag10 = !SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Contains(playerId);
					if (flag10)
					{
						SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Add(playerId);
						SkidMenuPlugin.OverloadInfoSystem.attackCounts[playerId] = 0;
						SkidMenuPlugin.OverloadInfoSystem.firstDetectionTime[playerId] = time;
						SkidMenuPlugin.OverloadInfoSystem.attackerMethods[playerId] = new System.Collections.Generic.List<string>();
					}
					System.Collections.Generic.Dictionary<byte, int> dictionary = SkidMenuPlugin.OverloadInfoSystem.attackCounts;
					byte key = playerId;
					int num4 = dictionary[key];
					dictionary[key] = num4 + 1;
					SkidMenuPlugin.OverloadInfoSystem.attackerDetails[playerId] = text2;
					bool flag11 = !SkidMenuPlugin.OverloadInfoSystem.attackerMethods[playerId].Contains(text2);
					if (flag11)
					{
						SkidMenuPlugin.OverloadInfoSystem.attackerMethods[playerId].Add(text2);
					}
					string text3 = SkidMenuPlugin.OverloadInfoSystem.DetermineAttackMethod(callId, queue.Count);
					SkidMenuPlugin.OverloadInfoSystem.ShowCriticalWarning(string.Concat(new string[]
					{
						"<color=#FF4444>",
						text,
						"</color> ? <color=#FFFF00>",
						text2,
						"</color> <color=#FF8800>[",
						text3,
						"]</color>"
					}));
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}

			// Token: 0x0600001D RID: 29 RVA: 0x000056CC File Offset: 0x000038CC
			public static bool CheckGameDataOverload(int clientId, int messageSize)
			{
				float time = Time.time;
				bool flag = !SkidMenuPlugin.OverloadInfoSystem.clientGameDataTimestamps.ContainsKey(clientId);
				if (flag)
				{
					SkidMenuPlugin.OverloadInfoSystem.clientGameDataTimestamps[clientId] = new System.Collections.Generic.Queue<float>();
					SkidMenuPlugin.OverloadInfoSystem.clientGameDataSizes[clientId] = new System.Collections.Generic.Queue<int>();
				}
				System.Collections.Generic.Queue<float> queue = SkidMenuPlugin.OverloadInfoSystem.clientGameDataTimestamps[clientId];
				System.Collections.Generic.Queue<int> queue2 = SkidMenuPlugin.OverloadInfoSystem.clientGameDataSizes[clientId];
				while (queue.Count > 0 && queue.Peek() < time - 1f)
				{
					queue.Dequeue();
					bool flag2 = queue2.Count > 0;
					if (flag2)
					{
						queue2.Dequeue();
					}
				}
				queue.Enqueue(time);
				queue2.Enqueue(messageSize);
				bool flag3 = false;
				string text = "";
				bool flag4 = messageSize > 5000;
				if (flag4)
				{
					flag3 = true;
					text = "Oversized packet (" + messageSize.ToString() + " bytes)";
				}
				bool flag5 = queue.Count > 30;
				if (flag5)
				{
					flag3 = true;
					text = "GameData spam (" + queue.Count.ToString() + "/sec)";
				}
				int num = 0;
				foreach (int num2 in queue2)
				{
					bool flag6 = num2 > 2000;
					if (flag6)
					{
						num++;
					}
				}
				bool flag7 = num > 5;
				if (flag7)
				{
					flag3 = true;
					text = "Method 3 pattern (" + num.ToString() + " large packets)";
				}
				int num3 = 0;
				foreach (int num4 in queue2)
				{
					num3 += num4;
				}
				bool flag8 = num3 > 50000;
				if (flag8)
				{
					flag3 = true;
					text = "Bandwidth flood (" + (num3 / 1000).ToString() + " KB/sec)";
				}
				bool flag9 = flag3;
				bool result;
				if (flag9)
				{
					bool flag10 = !SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Contains(clientId);
					if (flag10)
					{
						SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Add(clientId);
					}
					SkidMenuPlugin.OverloadInfoSystem.method3Details[clientId] = text;
					string clientName = SkidMenuPlugin.OverloadInfoSystem.GetClientName(clientId);
					SkidMenuPlugin.OverloadInfoSystem.ShowCriticalWarning(string.Concat(new string[]
					{
						"<color=#FF4444>",
						clientName,
						"</color> ? <color=#FFFF00>",
						text,
						"</color> <color=#FF0000>[METHOD 3]</color>"
					}));
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}

			// Token: 0x0600001E RID: 30 RVA: 0x00005964 File Offset: 0x00003B64
			private static string GetRpcName(byte rpcId)
			{
				byte b = rpcId;
				byte b2 = b;
				if (b2 <= 21)
				{
					if (b2 == 0)
					{
						return "PlayAnimation";
					}
					switch (b2)
					{
					case 4:
						return "EnterVent";
					case 5:
						return "CompleteTask";
					case 6:
					case 7:
					case 9:
					case 10:
					case 11:
					case 12:
					case 17:
						break;
					case 8:
						return "SendChat";
					case 13:
						return "SetName";
					case 14:
						return "SetColor";
					case 15:
						return "SetHat";
					case 16:
						return "SetSkin";
					case 18:
						return "SetPet";
					default:
						if (b2 == 21)
						{
							return "SetVisor";
						}
						break;
					}
				}
				else
				{
					if (b2 == 23)
					{
						return "SetLevel";
					}
					if (b2 == 28)
					{
						return "SetPet(Alt)";
					}
					if (b2 == 54)
					{
						return "SetRole";
					}
				}
				return "RPC " + rpcId.ToString();
			}

			// Token: 0x0600001F RID: 31 RVA: 0x00005A60 File Offset: 0x00003C60
			private static string DetermineAttackMethod(byte rpcId, int rpcCount)
			{
				bool flag = rpcId == 54 && rpcCount > 20;
				string result;
				if (flag)
				{
					result = "METHOD 1";
				}
				else
				{
					bool flag2 = rpcId == 18 && rpcCount > 20;
					if (flag2)
					{
						result = "METHOD 2";
					}
					else
					{
						bool flag3 = rpcId == 0 && rpcCount > 10;
						if (flag3)
						{
							result = "ANIMATION OVERLOAD";
						}
						else
						{
							bool flag4 = rpcId == 4;
							if (flag4)
							{
								result = "VENT SPAM";
							}
							else
							{
								bool flag5 = rpcId == 23;
								if (flag5)
								{
									result = "LEVEL SPAM";
								}
								else
								{
									result = "UNKNOWN METHOD";
								}
							}
						}
					}
				}
				return result;
			}

			// Token: 0x06000020 RID: 32 RVA: 0x00005AE8 File Offset: 0x00003CE8
			private static string GetClientName(int clientId)
			{
				try
				{
					ClientData client = AmongUsClient.Instance.GetClient(clientId);
					bool flag = client != null;
					if (flag)
					{
						return client.PlayerName ?? ("Client " + clientId.ToString());
					}
				}
				catch
				{
				}
				return "Client " + clientId.ToString();
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00005B58 File Offset: 0x00003D58
			public static string GetAttackStatistics()
			{
				string result;
				try
				{
					bool flag = SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Count == 0 && SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Count == 0;
					if (flag)
					{
						result = "No attacks detected";
					}
					else
					{
						string text = "<color=#FF8800>Attack Summary:</color>\n";
						foreach (byte b in SkidMenuPlugin.OverloadInfoSystem.detectedAttackers)
						{
							PlayerControl playerById = SkidMenuPlugin.OverloadInfoSystem.GetPlayerById(b);
							string text2;
							if (playerById == null)
							{
								text2 = null;
							}
							else
							{
								NetworkedPlayerInfo data = playerById.Data;
								text2 = ((data != null) ? data.PlayerName : null);
							}
							string text3 = text2 ?? ("Player " + b.ToString());
							int num = SkidMenuPlugin.OverloadInfoSystem.attackCounts.ContainsKey(b) ? SkidMenuPlugin.OverloadInfoSystem.attackCounts[b] : 0;
							float num2 = SkidMenuPlugin.OverloadInfoSystem.firstDetectionTime.ContainsKey(b) ? (Time.time - SkidMenuPlugin.OverloadInfoSystem.firstDetectionTime[b]) : 0f;
							string text4 = SkidMenuPlugin.OverloadInfoSystem.attackerMethods.ContainsKey(b) ? string.Join(", ", SkidMenuPlugin.OverloadInfoSystem.attackerMethods[b].ToArray()) : "Unknown";
							text = string.Concat(new string[]
							{
								text,
								"<color=#FF4444>",
								text3,
								"</color>: ",
								num.ToString(),
								" attacks over ",
								num2.ToString("F1"),
								"s\n  Methods: ",
								text4,
								"\n"
							});
						}
						foreach (int num3 in SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers)
						{
							string clientName = SkidMenuPlugin.OverloadInfoSystem.GetClientName(num3);
							string text5 = SkidMenuPlugin.OverloadInfoSystem.method3Details.ContainsKey(num3) ? SkidMenuPlugin.OverloadInfoSystem.method3Details[num3] : "Unknown";
							text = string.Concat(new string[]
							{
								text,
								"<color=#FF0000>",
								clientName,
								"</color> (Method 3): ",
								text5,
								"\n"
							});
						}
						result = text;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("GetAttackStatistics error: " + ex.Message);
					result = "Error getting statistics";
				}
				return result;
			}

			// Token: 0x06000022 RID: 34 RVA: 0x00005DEC File Offset: 0x00003FEC
			private static PlayerControl GetPlayerById(byte playerId)
			{
				try
				{
					foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
					{
						bool flag = playerControl != null && playerControl.PlayerId == playerId;
						if (flag)
						{
							return playerControl;
						}
					}
				}
				catch
				{
				}
				return null;
			}

			// Token: 0x040000D9 RID: 217
			public static readonly HashSet<byte> OverloadRPCs = new HashSet<byte>
			{
				0,
				4,
				5,
				8,
				13,
				14,
				15,
				16,
				18,
				21,
				23,
				28,
				54
			};

			// Token: 0x040000DA RID: 218
			private static System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>> playerRpcTimestamps = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>>();

			// Token: 0x040000DB RID: 219
			private static System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>> playerTotalRpcs = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>();

			// Token: 0x040000DC RID: 220
			private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<float>> clientGameDataTimestamps = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<float>>();

			// Token: 0x040000DD RID: 221
			private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<int>> clientGameDataSizes = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<int>>();

			// Token: 0x040000DE RID: 222
			private const int MAX_RPC_PER_SECOND = 15;

			// Token: 0x040000DF RID: 223
			private const int MAX_TOTAL_RPC_PER_SECOND = 50;

			// Token: 0x040000E0 RID: 224
			private const int MAX_GAMEDATA_PER_SECOND = 30;

			// Token: 0x040000E1 RID: 225
			private const int SUSPICIOUS_MESSAGE_SIZE = 2000;

			// Token: 0x040000E2 RID: 226
			private const int MAX_MESSAGE_SIZE = 5000;

			// Token: 0x040000E3 RID: 227
			private const float TIME_WINDOW = 1f;

			// Token: 0x040000E4 RID: 228
			public static HashSet<byte> detectedAttackers = new HashSet<byte>();

			// Token: 0x040000E5 RID: 229
			public static System.Collections.Generic.Dictionary<byte, string> attackerDetails = new System.Collections.Generic.Dictionary<byte, string>();

			// Token: 0x040000E6 RID: 230
			public static System.Collections.Generic.Dictionary<byte, System.Collections.Generic.List<string>> attackerMethods = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.List<string>>();

			// Token: 0x040000E7 RID: 231
			public static HashSet<int> detectedMethod3Attackers = new HashSet<int>();

			// Token: 0x040000E8 RID: 232
			public static System.Collections.Generic.Dictionary<int, string> method3Details = new System.Collections.Generic.Dictionary<int, string>();

			// Token: 0x040000E9 RID: 233
			public static System.Collections.Generic.Dictionary<byte, int> attackCounts = new System.Collections.Generic.Dictionary<byte, int>();

			// Token: 0x040000EA RID: 234
			public static System.Collections.Generic.Dictionary<byte, float> firstDetectionTime = new System.Collections.Generic.Dictionary<byte, float>();
		}

		// Token: 0x02000006 RID: 6
		[HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
		public static class OverloadInfoRpcPatch
		{
			// Token: 0x06000024 RID: 36 RVA: 0x00005F6C File Offset: 0x0000416C
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance, byte callId, MessageReader reader)
			{
				bool flag = !SkidMenuPlugin.OverloadInfoEnabled;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool amOwner = __instance.AmOwner;
					if (amOwner)
					{
						result = true;
					}
					else
					{
						try
						{
							bool flag2 = SkidMenuPlugin.OverloadInfoSystem.OverloadRPCs.Contains(callId);
							if (flag2)
							{
								SkidMenuPlugin.OverloadInfoSystem.CheckOverloadRpc(__instance, callId);
							}
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("Overload Info RPC error: " + ex.Message);
						}
						result = true;
					}
				}
				return result;
			}
		}

		// Token: 0x02000007 RID: 7
		[HarmonyPatch(typeof(InnerNetClient), "HandleMessage")]
		public static class OverloadInfo_GameDataPatch
		{
			// Token: 0x06000025 RID: 37 RVA: 0x00005FF0 File Offset: 0x000041F0
			[HarmonyPrefix]
			public static bool Prefix(InnerNetClient __instance, MessageReader reader, SendOption sendOption)
			{
				bool flag = !SkidMenuPlugin.OverloadInfoEnabled;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					try
					{
						bool flag2 = reader.Tag != 6;
						if (flag2)
						{
							return true;
						}
						int num = -1;
						try
						{
							bool flag3 = __instance != null;
							if (flag3)
							{
								foreach (ClientData clientData in AmongUsClient.Instance.allClients.ToArray())
								{
									bool flag4 = clientData != null && clientData.Id != AmongUsClient.Instance.ClientId;
									if (flag4)
									{
										num = clientData.Id;
										break;
									}
								}
							}
						}
						catch
						{
						}
						bool flag5 = num == -1;
						if (flag5)
						{
							return true;
						}
						int length = reader.Length;
						SkidMenuPlugin.OverloadInfoSystem.CheckGameDataOverload(num, length);
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Overload Info GameData error: " + ex.Message);
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000008 RID: 8
		[HarmonyPatch(typeof(PlatformSpecificData), "Serialize")]
		public static class PlatformSpoofPatch
		{
			// Token: 0x06000026 RID: 38 RVA: 0x00006120 File Offset: 0x00004320
			[HarmonyPrefix]
			public static void Prefix(PlatformSpecificData __instance)
			{
				bool flag = !string.IsNullOrEmpty(SkidMenuPlugin.SpoofPlatform.Value);
				if (flag)
				{
					Platforms platform;
					bool flag2 = SkidMenuPlugin.PlatformSpoofPatch.TryParsePlatform(SkidMenuPlugin.SpoofPlatform.Value, out platform);
					if (flag2)
					{
						__instance.Platform = platform;
					}
				}
			}

			// Token: 0x06000027 RID: 39 RVA: 0x00006168 File Offset: 0x00004368
			private static bool TryParsePlatform(string platformString, out Platforms platform)
			{
				return System.Enum.TryParse<Platforms>(platformString, true, out platform);
			}
		}

		// Token: 0x02000009 RID: 9
		[HarmonyPatch(typeof(PlayerControl))]
		[HarmonyPatch("HandleRpc")]
		public static class EnhancedAntiOverloadPatch
		{
			// Token: 0x06000028 RID: 40 RVA: 0x00006184 File Offset: 0x00004384
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance, byte callId, MessageReader reader)
			{
				bool flag = !SkidMenuPlugin.AntiOverloadEnabled;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool amOwner = __instance.AmOwner;
					if (amOwner)
					{
						result = true;
					}
					else
					{
						try
						{
							byte playerId = __instance.PlayerId;
							float time = Time.time;
							bool flag2 = SkidMenuPlugin.EnhancedAntiOverloadPatch.blockedPlayers.Contains(playerId);
							if (flag2)
							{
								bool flag3 = SkidMenuPlugin.EnhancedAntiOverloadPatch.blockExpiry.ContainsKey(playerId) && time < SkidMenuPlugin.EnhancedAntiOverloadPatch.blockExpiry[playerId];
								if (flag3)
								{
									return false;
								}
								SkidMenuPlugin.EnhancedAntiOverloadPatch.blockedPlayers.Remove(playerId);
								SkidMenuPlugin.EnhancedAntiOverloadPatch.blockExpiry.Remove(playerId);
								SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? Player " + playerId.ToString() + " unblocked");
							}
							bool flag4 = !SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps.ContainsKey(playerId);
							if (flag4)
							{
								SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps[playerId] = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>();
								SkidMenuPlugin.EnhancedAntiOverloadPatch.playerTotalMessages[playerId] = new System.Collections.Generic.Queue<float>();
							}
							bool flag5 = !SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps[playerId].ContainsKey(callId);
							if (flag5)
							{
								SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps[playerId][callId] = new System.Collections.Generic.Queue<float>();
							}
							System.Collections.Generic.Queue<float> queue = SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps[playerId][callId];
							System.Collections.Generic.Queue<float> queue2 = SkidMenuPlugin.EnhancedAntiOverloadPatch.playerTotalMessages[playerId];
							while (queue.Count > 0 && queue.Peek() < time - 1f)
							{
								queue.Dequeue();
							}
							while (queue2.Count > 0 && queue2.Peek() < time - 1f)
							{
								queue2.Dequeue();
							}
							queue.Enqueue(time);
							queue2.Enqueue(time);
							bool flag6 = SkidMenuPlugin.EnhancedAntiOverloadPatch.OVERLOAD_RPCS.Contains(callId);
							if (flag6)
							{
								int num = SkidMenuPlugin.EnhancedAntiOverloadPatch.RPC_THRESHOLDS.ContainsKey(callId) ? SkidMenuPlugin.EnhancedAntiOverloadPatch.RPC_THRESHOLDS[callId] : 10;
								bool flag7 = queue.Count > num;
								if (flag7)
								{
									SkidMenuPlugin.EnhancedAntiOverloadPatch.BlockPlayer(playerId, time, string.Concat(new string[]
									{
										"RPC ",
										callId.ToString(),
										" spam (",
										queue.Count.ToString(),
										"/",
										num.ToString(),
										" per sec)"
									}));
									return false;
								}
							}
							bool flag8 = queue2.Count > 50;
							if (flag8)
							{
								SkidMenuPlugin.EnhancedAntiOverloadPatch.BlockPlayer(playerId, time, string.Concat(new string[]
								{
									"Total RPC spam (",
									queue2.Count.ToString(),
									"/",
									50.ToString(),
									" per sec)"
								}));
								result = false;
							}
							else
							{
								int num2 = 0;
								foreach (System.Collections.Generic.KeyValuePair<byte, System.Collections.Generic.Queue<float>> keyValuePair in SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps[playerId])
								{
									bool flag9 = keyValuePair.Value.Count > 0;
									if (flag9)
									{
										num2 += keyValuePair.Value.Count;
									}
								}
								bool flag10 = num2 > 30;
								if (flag10)
								{
									SkidMenuPlugin.EnhancedAntiOverloadPatch.BlockPlayer(playerId, time, "Diverse RPC spam (" + num2.ToString() + " mixed RPCs per sec)");
									result = false;
								}
								else
								{
									int num3 = 0;
									float num4 = 0.1f;
									foreach (float num5 in queue)
									{
										bool flag11 = num5 > time - num4;
										if (flag11)
										{
											num3++;
										}
									}
									bool flag12 = num3 > 3 && SkidMenuPlugin.EnhancedAntiOverloadPatch.OVERLOAD_RPCS.Contains(callId);
									if (flag12)
									{
										SkidMenuPlugin.EnhancedAntiOverloadPatch.BlockPlayer(playerId, time, string.Concat(new string[]
										{
											"Burst attack - RPC ",
											callId.ToString(),
											" (",
											num3.ToString(),
											" in 0.1s)"
										}));
										result = false;
									}
									else
									{
										result = true;
									}
								}
							}
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("Anti-Overload error: " + ex.Message);
							result = true;
						}
					}
				}
				return result;
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00006608 File Offset: 0x00004808
			private static void BlockPlayer(byte playerId, float currentTime, string reason)
			{
				bool flag = !SkidMenuPlugin.EnhancedAntiOverloadPatch.blockedPlayers.Contains(playerId);
				if (flag)
				{
					SkidMenuPlugin.EnhancedAntiOverloadPatch.blockedPlayers.Add(playerId);
					SkidMenuPlugin.EnhancedAntiOverloadPatch.blockExpiry[playerId] = currentTime + 5f;
					string playerName = SkidMenuPlugin.EnhancedAntiOverloadPatch.GetPlayerName(playerId);
					SkidMenuPlugin.Logger.LogWarning("\ud83d\udee1? BLOCKED OVERLOAD from " + playerName + " - " + reason);
					bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null && PlayerControl.LocalPlayer != null;
					if (flag2)
					{
						DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[Anti-Overload]</color> Blocked attack from <color=yellow>" + playerName + "</color>", true);
					}
				}
			}

			// Token: 0x0600002A RID: 42 RVA: 0x000066C8 File Offset: 0x000048C8
			private static string GetPlayerName(byte playerId)
			{
				try
				{
					foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
					{
						bool flag = playerControl != null && playerControl.PlayerId == playerId;
						if (flag)
						{
							NetworkedPlayerInfo data = playerControl.Data;
							return ((data != null) ? data.PlayerName : null) ?? ("Player " + playerId.ToString());
						}
					}
				}
				catch
				{
				}
				return "Player " + playerId.ToString();
			}

			// Token: 0x0600002B RID: 43 RVA: 0x00006788 File Offset: 0x00004988
			public static void Reset()
			{
				SkidMenuPlugin.EnhancedAntiOverloadPatch.playerRpcTimestamps.Clear();
				SkidMenuPlugin.EnhancedAntiOverloadPatch.playerTotalMessages.Clear();
				SkidMenuPlugin.EnhancedAntiOverloadPatch.blockedPlayers.Clear();
				SkidMenuPlugin.EnhancedAntiOverloadPatch.blockExpiry.Clear();
				SkidMenuPlugin.Logger.LogInfo("Anti-Overload reset");
			}

			// Token: 0x040000EB RID: 235
			private static System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>> playerRpcTimestamps = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>>();

			// Token: 0x040000EC RID: 236
			private static System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>> playerTotalMessages = new System.Collections.Generic.Dictionary<byte, System.Collections.Generic.Queue<float>>();

			// Token: 0x040000ED RID: 237
			private static readonly HashSet<byte> OVERLOAD_RPCS = new HashSet<byte>
			{
				0,
				4,
				18,
				23,
				54,
				5,
				8,
				13,
				14,
				15,
				16,
				21,
				28
			};

			// Token: 0x040000EE RID: 238
			private static readonly System.Collections.Generic.Dictionary<byte, int> RPC_THRESHOLDS = new System.Collections.Generic.Dictionary<byte, int>
			{
				{
					0,
					5
				},
				{
					4,
					3
				},
				{
					5,
					8
				},
				{
					8,
					5
				},
				{
					13,
					4
				},
				{
					14,
					4
				},
				{
					15,
					4
				},
				{
					16,
					4
				},
				{
					18,
					8
				},
				{
					21,
					4
				},
				{
					23,
					8
				},
				{
					28,
					8
				},
				{
					54,
					8
				}
			};

			// Token: 0x040000EF RID: 239
			private const int MAX_TOTAL_RPCS_PER_SECOND = 50;

			// Token: 0x040000F0 RID: 240
			private const int MAX_UNIQUE_RPCS_PER_SECOND = 30;

			// Token: 0x040000F1 RID: 241
			private const float TIME_WINDOW = 1f;

			// Token: 0x040000F2 RID: 242
			private static HashSet<byte> blockedPlayers = new HashSet<byte>();

			// Token: 0x040000F3 RID: 243
			private static System.Collections.Generic.Dictionary<byte, float> blockExpiry = new System.Collections.Generic.Dictionary<byte, float>();

			// Token: 0x040000F4 RID: 244
			private const float BLOCK_DURATION = 5f;
		}

		// Token: 0x0200000A RID: 10
		[HarmonyPatch(typeof(InnerNetClient), "HandleMessage")]
		public static class NetworkLevel_AntiOverloadPatch
		{
			// Token: 0x0600002D RID: 45 RVA: 0x00006900 File Offset: 0x00004B00
			[HarmonyPrefix]
			public static bool Prefix(InnerNetClient __instance, MessageReader reader, SendOption sendOption)
			{
				bool flag = !SkidMenuPlugin.AntiOverloadEnabled;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					try
					{
						bool flag2 = AmongUsClient.Instance == null;
						if (flag2)
						{
							return true;
						}
						float time = Time.time;
						foreach (ClientData clientData in AmongUsClient.Instance.allClients.ToArray())
						{
							bool flag3 = clientData == null || clientData.Id == AmongUsClient.Instance.ClientId;
							if (!flag3)
							{
								bool flag4 = !SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps.ContainsKey(clientData.Id);
								if (flag4)
								{
									SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps[clientData.Id] = new System.Collections.Generic.Queue<float>();
								}
							}
						}
						int num = -1;
						int num2 = 0;
						foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.Queue<float>> keyValuePair in SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps)
						{
							while (keyValuePair.Value.Count > 0 && keyValuePair.Value.Peek() < time - 0.1f)
							{
								keyValuePair.Value.Dequeue();
							}
							bool flag5 = keyValuePair.Value.Count > num2;
							if (flag5)
							{
								num2 = keyValuePair.Value.Count;
								num = keyValuePair.Key;
							}
						}
						ClientData[] array = (from c in AmongUsClient.Instance.allClients.ToArray()
						where c != null && c.Id != AmongUsClient.Instance.ClientId
						select c).ToArray<ClientData>();
						bool flag6 = array.Length == 1;
						if (flag6)
						{
							num = array[0].Id;
						}
						bool flag7 = num == -1;
						if (flag7)
						{
							return true;
						}
						bool flag8 = !SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps.ContainsKey(num);
						if (flag8)
						{
							SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps[num] = new System.Collections.Generic.Queue<float>();
						}
						SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps[num].Enqueue(time);
						int count = SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps[num].Count;
						bool flag9 = count > 20;
						if (flag9)
						{
							bool flag10 = !SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.lastNotifyTime.ContainsKey(num) || time - SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.lastNotifyTime[num] >= 3f;
							bool flag11 = flag10;
							if (flag11)
							{
								SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.lastNotifyTime[num] = time;
								string clientName = SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.GetClientName(num);
								SkidMenuPlugin.Logger.LogWarning(string.Concat(new string[]
								{
									"\ud83d\udee1? [Network Anti-Overload] Blocked flood from ",
									clientName,
									" (",
									count.ToString(),
									" msgs/100ms)"
								}));
								HudManager instance = DestroyableSingleton<HudManager>.Instance;
								bool flag12 = ((instance != null) ? instance.Chat : null) != null && PlayerControl.LocalPlayer != null;
								if (flag12)
								{
									DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Concat(new string[]
									{
										"<color=red>[Anti-Overload]</color> Blocking network flood from <color=yellow>",
										clientName,
										"</color> (",
										count.ToString(),
										"/100ms)"
									}), true);
								}
							}
							return false;
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("NetworkLevel_AntiOverload error: " + ex.Message);
					}
					result = true;
				}
				return result;
			}

			// Token: 0x0600002E RID: 46 RVA: 0x00006CA8 File Offset: 0x00004EA8
			private static string GetClientName(int clientId)
			{
				try
				{
					ClientData client = AmongUsClient.Instance.GetClient(clientId);
					return ((client != null) ? client.PlayerName : null) ?? ("Client " + clientId.ToString());
				}
				catch
				{
				}
				return "Client " + clientId.ToString();
			}

			// Token: 0x0600002F RID: 47 RVA: 0x00006D14 File Offset: 0x00004F14
			public static void Reset()
			{
				SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.clientMessageTimestamps.Clear();
				SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.notifiedClients.Clear();
				SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.lastNotifyTime.Clear();
				SkidMenuPlugin.Logger.LogInfo("Network Anti-Overload reset");
			}

			// Token: 0x040000F5 RID: 245
			private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<float>> clientMessageTimestamps = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<float>>();

			// Token: 0x040000F6 RID: 246
			private static HashSet<int> notifiedClients = new HashSet<int>();

			// Token: 0x040000F7 RID: 247
			private const float TIME_WINDOW = 0.1f;

			// Token: 0x040000F8 RID: 248
			private const int MAX_MESSAGES_PER_WINDOW = 20;

			// Token: 0x040000F9 RID: 249
			private const float NOTIFY_COOLDOWN = 3f;

			// Token: 0x040000FA RID: 250
			private static System.Collections.Generic.Dictionary<int, float> lastNotifyTime = new System.Collections.Generic.Dictionary<int, float>();
		}

		// Token: 0x0200000B RID: 11
		[HarmonyPatch(typeof(InnerNetClient), "HandleMessage")]
		public static class AntiOverload_GameDataPatch
		{
			// Token: 0x06000031 RID: 49 RVA: 0x00006D68 File Offset: 0x00004F68
			[HarmonyPrefix]
			public static bool Prefix(InnerNetClient __instance, MessageReader reader, SendOption sendOption)
			{
				bool flag = !SkidMenuPlugin.AntiOverloadEnabled;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					try
					{
						bool flag2 = reader.Tag != 6;
						if (flag2)
						{
							result = true;
						}
						else
						{
							int num = -1;
							try
							{
								foreach (ClientData clientData in AmongUsClient.Instance.allClients.ToArray())
								{
									bool flag3 = clientData != null && clientData.Id != AmongUsClient.Instance.ClientId;
									if (flag3)
									{
										bool flag4 = !SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps.ContainsKey(clientData.Id);
										if (flag4)
										{
											SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps[clientData.Id] = new System.Collections.Generic.Queue<float>();
											SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageSizes[clientData.Id] = new System.Collections.Generic.Queue<int>();
										}
									}
								}
								num = SkidMenuPlugin.AntiOverload_GameDataPatch.GetSenderFromReader(reader);
							}
							catch
							{
							}
							bool flag5 = num == -1;
							if (flag5)
							{
								result = true;
							}
							else
							{
								float time = Time.time;
								bool flag6 = SkidMenuPlugin.AntiOverload_GameDataPatch.blockedClients.Contains(num);
								if (flag6)
								{
									bool flag7 = SkidMenuPlugin.AntiOverload_GameDataPatch.blockExpiry.ContainsKey(num) && time < SkidMenuPlugin.AntiOverload_GameDataPatch.blockExpiry[num];
									if (flag7)
									{
										return false;
									}
									SkidMenuPlugin.AntiOverload_GameDataPatch.blockedClients.Remove(num);
									SkidMenuPlugin.AntiOverload_GameDataPatch.blockExpiry.Remove(num);
									SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? Client " + num.ToString() + " unblocked (Method 3)");
								}
								bool flag8 = !SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps.ContainsKey(num);
								if (flag8)
								{
									SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps[num] = new System.Collections.Generic.Queue<float>();
									SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageSizes[num] = new System.Collections.Generic.Queue<int>();
								}
								System.Collections.Generic.Queue<float> queue = SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps[num];
								System.Collections.Generic.Queue<int> queue2 = SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageSizes[num];
								while (queue.Count > 0 && queue.Peek() < time - 1f)
								{
									queue.Dequeue();
									bool flag9 = queue2.Count > 0;
									if (flag9)
									{
										queue2.Dequeue();
									}
								}
								int length = reader.Length;
								queue.Enqueue(time);
								queue2.Enqueue(length);
								bool flag10 = length > 5000;
								if (flag10)
								{
									SkidMenuPlugin.AntiOverload_GameDataPatch.BlockClient(num, time, "Oversized GameData (" + length.ToString() + " bytes)");
									result = false;
								}
								else
								{
									bool flag11 = queue.Count > 30;
									if (flag11)
									{
										SkidMenuPlugin.AntiOverload_GameDataPatch.BlockClient(num, time, string.Concat(new string[]
										{
											"GameData spam (",
											queue.Count.ToString(),
											"/",
											30.ToString(),
											" per sec)"
										}));
										result = false;
									}
									else
									{
										int num2 = 0;
										foreach (int num3 in queue2)
										{
											bool flag12 = num3 > 2000;
											if (flag12)
											{
												num2++;
											}
										}
										bool flag13 = num2 > 5;
										if (flag13)
										{
											SkidMenuPlugin.AntiOverload_GameDataPatch.BlockClient(num, time, "Method 3 pattern detected (" + num2.ToString() + " large packets)");
											result = false;
										}
										else
										{
											int num4 = 0;
											foreach (int num5 in queue2)
											{
												num4 += num5;
											}
											bool flag14 = num4 > 50000;
											if (flag14)
											{
												SkidMenuPlugin.AntiOverload_GameDataPatch.BlockClient(num, time, "Bandwidth overload (" + (num4 / 1000).ToString() + " KB/sec)");
												result = false;
											}
											else
											{
												result = true;
											}
										}
									}
								}
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Anti-Overload GameData error: " + ex.Message);
						result = true;
					}
				}
				return result;
			}

			// Token: 0x06000032 RID: 50 RVA: 0x000071D8 File Offset: 0x000053D8
			private static void BlockClient(int clientId, float currentTime, string reason)
			{
				bool flag = !SkidMenuPlugin.AntiOverload_GameDataPatch.blockedClients.Contains(clientId);
				if (flag)
				{
					SkidMenuPlugin.AntiOverload_GameDataPatch.blockedClients.Add(clientId);
					SkidMenuPlugin.AntiOverload_GameDataPatch.blockExpiry[clientId] = currentTime + 10f;
					string clientName = SkidMenuPlugin.AntiOverload_GameDataPatch.GetClientName(clientId);
					SkidMenuPlugin.Logger.LogWarning("\ud83d\udee1? BLOCKED METHOD 3 from " + clientName + " - " + reason);
					bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null && PlayerControl.LocalPlayer != null;
					if (flag2)
					{
						DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[Anti-Overload]</color> Blocked Method 3 from <color=yellow>" + clientName + "</color>", true);
					}
				}
			}

			// Token: 0x06000033 RID: 51 RVA: 0x00007298 File Offset: 0x00005498
			private static int GetSenderFromReader(MessageReader reader)
			{
				try
				{
					float time = Time.time;
					int num = -1;
					int num2 = 0;
					foreach (ClientData clientData in AmongUsClient.Instance.allClients.ToArray())
					{
						bool flag = clientData == null || clientData.Id == AmongUsClient.Instance.ClientId;
						if (!flag)
						{
							bool flag2 = !SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps.ContainsKey(clientData.Id);
							if (!flag2)
							{
								int count = SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps[clientData.Id].Count;
								bool flag3 = count > num2;
								if (flag3)
								{
									num2 = count;
									num = clientData.Id;
								}
							}
						}
					}
					int result;
					if (num == -1)
					{
						ClientData clientData2 = AmongUsClient.Instance.allClients.ToArray().FirstOrDefault((ClientData c) => c != null && c.Id != AmongUsClient.Instance.ClientId);
						result = ((clientData2 != null) ? clientData2.Id : -1);
					}
					else
					{
						result = num;
					}
					return result;
				}
				catch
				{
				}
				return -1;
			}

			// Token: 0x06000034 RID: 52 RVA: 0x000073C8 File Offset: 0x000055C8
			private static string GetClientName(int clientId)
			{
				try
				{
					ClientData client = AmongUsClient.Instance.GetClient(clientId);
					bool flag = client != null;
					if (flag)
					{
						return client.PlayerName ?? ("Client " + clientId.ToString());
					}
				}
				catch
				{
				}
				return "Client " + clientId.ToString();
			}

			// Token: 0x06000035 RID: 53 RVA: 0x00007438 File Offset: 0x00005638
			public static void Reset()
			{
				SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageTimestamps.Clear();
				SkidMenuPlugin.AntiOverload_GameDataPatch.clientMessageSizes.Clear();
				SkidMenuPlugin.AntiOverload_GameDataPatch.blockedClients.Clear();
				SkidMenuPlugin.AntiOverload_GameDataPatch.blockExpiry.Clear();
				SkidMenuPlugin.Logger.LogInfo("Anti-Overload Method 3 protection reset");
			}

			// Token: 0x040000FB RID: 251
			private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<float>> clientMessageTimestamps = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<float>>();

			// Token: 0x040000FC RID: 252
			private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<int>> clientMessageSizes = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Queue<int>>();

			// Token: 0x040000FD RID: 253
			private const int MAX_GAMEDATA_PER_SECOND = 30;

			// Token: 0x040000FE RID: 254
			private const int MAX_MESSAGE_SIZE = 5000;

			// Token: 0x040000FF RID: 255
			private const int SUSPICIOUS_MESSAGE_SIZE = 2000;

			// Token: 0x04000100 RID: 256
			private const float TIME_WINDOW = 1f;

			// Token: 0x04000101 RID: 257
			private static HashSet<int> blockedClients = new HashSet<int>();

			// Token: 0x04000102 RID: 258
			private static System.Collections.Generic.Dictionary<int, float> blockExpiry = new System.Collections.Generic.Dictionary<int, float>();

			// Token: 0x04000103 RID: 259
			private const float BLOCK_DURATION = 10f;
		}

		// Token: 0x0200000C RID: 12
		[HarmonyPatch(typeof(MeetingHud), "Start")]
		public static class MeetingHud_RevealRolesPatch
		{
			// Token: 0x06000037 RID: 55 RVA: 0x000074A4 File Offset: 0x000056A4
			[HarmonyPostfix]
			public static void Postfix(MeetingHud __instance)
			{
				bool flag = !SkidMenuPlugin.SeeRolesEnabled && !SkidMenuPlugin.ShowPlayerInfo;
				if (!flag)
				{
					try
					{
						foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
						{
							bool flag2 = playerVoteArea == null;
							if (!flag2)
							{
								NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(playerVoteArea.TargetPlayerId);
								bool flag3 = playerById == null || playerById.Disconnected;
								if (!flag3)
								{
									bool flag4 = playerById.Role == null;
									if (!flag4)
									{
										TextMeshPro nameText = playerVoteArea.NameText;
										bool flag5 = nameText == null;
										if (!flag5)
										{
											string text = playerById.PlayerName ?? "";
											string text2 = ColorUtility.ToHtmlStringRGB(playerById.Role.TeamColor);
											string roleName = SkidMenuPlugin.PlayerNametagsPatch.GetRoleName(playerById);
											string text3 = "";
											bool flag6 = SkidMenuPlugin.SeeRolesEnabled && SkidMenuPlugin.ShowPlayerInfo;
											if (flag6)
											{
												ClientData clientFromPlayerInfo = AmongUsClient.Instance.GetClientFromPlayerInfo(playerById);
												ClientData host = AmongUsClient.Instance.GetHost();
												int num = (int)(playerById.PlayerLevel + 1U);
												string text4 = "?";
												try
												{
													text4 = SkidMenuPlugin.PlayerNametagsPatch.GetPlatformShortName(clientFromPlayerInfo.PlatformData.Platform);
												}
												catch
												{
												}
												string text5 = (clientFromPlayerInfo == host) ? "Host - " : "";
												text3 = string.Format("<color=#{0}>{1}</color>\n{2}\n<size=70%><color=#fb0>{3}Lv:{4} - {5}</color></size>", new object[]
												{
													text2,
													roleName,
													text,
													text5,
													num,
													text4
												});
											}
											else
											{
												bool seeRolesEnabled = SkidMenuPlugin.SeeRolesEnabled;
												if (seeRolesEnabled)
												{
													text3 = string.Concat(new string[]
													{
														"<color=#",
														text2,
														">",
														roleName,
														"</color>\n",
														text
													});
												}
												else
												{
													bool showPlayerInfo = SkidMenuPlugin.ShowPlayerInfo;
													if (showPlayerInfo)
													{
														ClientData clientFromPlayerInfo2 = AmongUsClient.Instance.GetClientFromPlayerInfo(playerById);
														ClientData host2 = AmongUsClient.Instance.GetHost();
														int num2 = (int)(playerById.PlayerLevel + 1U);
														string text6 = "?";
														try
														{
															text6 = SkidMenuPlugin.PlayerNametagsPatch.GetPlatformShortName(clientFromPlayerInfo2.PlatformData.Platform);
														}
														catch
														{
														}
														string text7 = (clientFromPlayerInfo2 == host2) ? "Host - " : "";
														text3 = string.Format("{0}\n<size=70%><color=#fb0>{1}Lv:{2} - {3}</color></size>", new object[]
														{
															text,
															text7,
															num2,
															text6
														});
													}
												}
											}
											nameText.text = text3;
										}
									}
								}
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("MeetingHud RevealRoles error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x0200000D RID: 13
		[HarmonyPatch(typeof(PlayerControl), "RpcSetScanner")]
		public static class SetScannerPatch
		{
			// Token: 0x06000038 RID: 56 RVA: 0x000077C4 File Offset: 0x000059C4
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance, bool value)
			{
				bool flag = __instance.PlayerId != PlayerControl.LocalPlayer.PlayerId || !SkidMenuPlugin.IsScanning;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(__instance.NetId, 15, SendOption.Reliable, -1);
					messageWriter.Write(value);
					messageWriter.Write(__instance.PlayerId);
					AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
					__instance.SetScanner(value, __instance.PlayerId);
					result = false;
				}
				return result;
			}
		}

		// Token: 0x0200000E RID: 14
		public static class LobbyTimerTracker
		{
			// Token: 0x06000039 RID: 57 RVA: 0x00007840 File Offset: 0x00005A40
			public static float GetRemainingTime()
			{
				bool flag = SkidMenuPlugin.LobbyTimerTracker.TimerStartTS == 0L;
				float result;
				if (flag)
				{
					result = 600f;
				}
				else
				{
					long num = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
					float num2 = (float)(num - SkidMenuPlugin.LobbyTimerTracker.TimerStartTS);
					result = Mathf.Max(0f, 600f - num2);
				}
				return result;
			}

			// Token: 0x0600003A RID: 58 RVA: 0x00007890 File Offset: 0x00005A90
			public static void ResetTimer()
			{
				SkidMenuPlugin.LobbyTimerTracker.TimerStartTS = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			}

			// Token: 0x04000104 RID: 260
			public static long TimerStartTS;

			// Token: 0x04000105 RID: 261
			private const float LOBBY_DURATION = 600f;
		}

		// Token: 0x0200000F RID: 15
		[HarmonyPatch(typeof(LobbyBehaviour), "Start")]
		public static class LobbyTimerStartPatch
		{
			// Token: 0x0600003B RID: 59 RVA: 0x000078B0 File Offset: 0x00005AB0
			[HarmonyPostfix]
			public static void Postfix()
			{
				SkidMenuPlugin.LobbyTimerTracker.ResetTimer();
			}
		}

		// Token: 0x02000010 RID: 16
		[HarmonyPatch(typeof(LobbyBehaviour), "FixedUpdate")]
		public static class ShowLobbyTimerPatch
		{
			// Token: 0x0600003C RID: 60 RVA: 0x000078BC File Offset: 0x00005ABC
			[HarmonyPostfix]
			public static void Postfix(LobbyBehaviour __instance)
			{
				bool flag = !SkidMenuPlugin.ShowLobbyTimerEnabled;
				if (flag)
				{
					bool flag2 = SkidMenuPlugin.ShowLobbyTimerPatch.timerText != null;
					if (flag2)
					{
						UnityEngine.Object.Destroy(SkidMenuPlugin.ShowLobbyTimerPatch.timerText.gameObject);
						SkidMenuPlugin.ShowLobbyTimerPatch.timerText = null;
					}
				}
				else
				{
					bool flag3 = !AmongUsClient.Instance.AmHost;
					if (!flag3)
					{
						try
						{
							float remainingTime = SkidMenuPlugin.LobbyTimerTracker.GetRemainingTime();
							bool flag4 = SkidMenuPlugin.ShowLobbyTimerPatch.timerText == null;
							if (flag4)
							{
								GameStartManager gameStartManager = UnityEngine.Object.FindObjectOfType<GameStartManager>();
								TextMeshPro textMeshPro = (gameStartManager != null) ? gameStartManager.GameRoomNameCode : null;
								bool flag5 = textMeshPro != null;
								if (flag5)
								{
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText = UnityEngine.Object.Instantiate<TextMeshPro>(textMeshPro, textMeshPro.transform.parent);
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText.name = "LobbyTimerText";
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText.transform.localPosition = textMeshPro.transform.localPosition + new Vector3(0f, -0.5f, 0f);
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText.fontSize = 3f;
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText.alignment = 514;
								}
							}
							bool flag6 = SkidMenuPlugin.ShowLobbyTimerPatch.timerText != null;
							if (flag6)
							{
								int num = Mathf.FloorToInt(remainingTime / 60f);
								int num2 = Mathf.FloorToInt(remainingTime % 60f);
								bool flag7 = remainingTime >= 180f;
								Color color;
								if (flag7)
								{
									color = new Color(0f, 0.7f, 1f);
								}
								else
								{
									bool flag8 = remainingTime >= 120f;
									if (flag8)
									{
										color = Color.Lerp(Color.yellow, Color.cyan, (remainingTime - 120f) / 60f);
									}
									else
									{
										bool flag9 = remainingTime >= 60f;
										if (flag9)
										{
											color = Color.Lerp(new Color(1f, 0.5f, 0f), Color.yellow, (remainingTime - 60f) / 60f);
										}
										else
										{
											color = Color.red;
										}
									}
								}
								SkidMenuPlugin.ShowLobbyTimerPatch.timerText.color = color;
								SkidMenuPlugin.ShowLobbyTimerPatch.timerText.text = string.Format("<size=70%>Lobby closes in: {0:00}:{1:00}</size>", num, num2);
								bool flag10 = Mathf.FloorToInt(remainingTime) == 60 && Mathf.FloorToInt(remainingTime % 1f * 10f) == 0;
								if (flag10)
								{
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText.fontSize = 3.2f;
								}
								else
								{
									SkidMenuPlugin.ShowLobbyTimerPatch.timerText.fontSize = 3f;
								}
							}
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("ShowLobbyTimer error: " + ex.Message);
						}
					}
				}
			}

			// Token: 0x0600003D RID: 61 RVA: 0x00007B6C File Offset: 0x00005D6C
			public static void ResetTimer()
			{
				bool flag = SkidMenuPlugin.ShowLobbyTimerPatch.timerText != null;
				if (flag)
				{
					UnityEngine.Object.Destroy(SkidMenuPlugin.ShowLobbyTimerPatch.timerText.gameObject);
					SkidMenuPlugin.ShowLobbyTimerPatch.timerText = null;
				}
			}

			// Token: 0x04000106 RID: 262
			private static TextMeshPro timerText;
		}

		// Token: 0x02000011 RID: 17
		[HarmonyPatch(typeof(AmongUsClient), "OnGameEnd")]
		public static class ResetLobbyTimerPatch
		{
			// Token: 0x0600003E RID: 62 RVA: 0x00007BA1 File Offset: 0x00005DA1
			[HarmonyPostfix]
			public static void Postfix()
			{
				SkidMenuPlugin.ShowLobbyTimerPatch.ResetTimer();
				SkidMenuPlugin.LobbyTimerTracker.TimerStartTS = 0L;
			}
		}

		// Token: 0x02000012 RID: 18
		[HarmonyPatch(typeof(global::Console), "CanUse")]
		public static class ImpostorTasksPatch
		{
			// Token: 0x0600003F RID: 63 RVA: 0x00007BB4 File Offset: 0x00005DB4
			[HarmonyPrefix]
			public static void Prefix(global::Console __instance)
			{
				bool impostorTasksEnabled = SkidMenuPlugin.ImpostorTasksEnabled;
				if (impostorTasksEnabled)
				{
					__instance.AllowImpostor = true;
				}
			}
		}

		// Token: 0x02000013 RID: 19
		[HarmonyPatch(typeof(PlayerControl), "MurderPlayer")]
		public static class KillNotificationPatch
		{
			// Token: 0x06000040 RID: 64 RVA: 0x00007BD4 File Offset: 0x00005DD4
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance, PlayerControl target)
			{
				bool flag = !SkidMenuPlugin.KillNotificationEnabled;
				if (!flag)
				{
					try
					{
						bool flag2 = __instance == null || target == null;
						if (!flag2)
						{
							NetworkedPlayerInfo data = __instance.Data;
							string text = ((data != null) ? data.PlayerName : null) ?? "Unknown";
							NetworkedPlayerInfo data2 = target.Data;
							string text2 = ((data2 != null) ? data2.PlayerName : null) ?? "Unknown";
							string playerLocation = SkidMenuPlugin.KillNotificationPatch.GetPlayerLocation(target);
							string chatText = string.Concat(new string[]
							{
								"<color=#FF0000>",
								text,
								"</color> killed <color=#800080>",
								text2,
								"</color> at <color=#00BFFF>",
								playerLocation,
								"</color>"
							});
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, chatText, true);
							}
							SkidMenuPlugin.Logger.LogInfo(string.Concat(new string[]
							{
								"[Kill Notification] ",
								text,
								" killed ",
								text2,
								" at ",
								playerLocation
							}));
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Kill Notification error: " + ex.Message);
					}
				}
			}

			// Token: 0x06000041 RID: 65 RVA: 0x00007D48 File Offset: 0x00005F48
			private static string GetPlayerLocation(PlayerControl player)
			{
				string result;
				try
				{
					bool flag = ShipStatus.Instance == null || player == null;
					if (flag)
					{
						result = "Unknown";
					}
					else
					{
						Il2CppReferenceArray<PlainShipRoom> allRooms = ShipStatus.Instance.AllRooms;
						bool flag2 = allRooms == null || allRooms.Count == 0;
						if (flag2)
						{
							result = "Unknown";
						}
						else
						{
							PlainShipRoom plainShipRoom = null;
							float num = float.MaxValue;
							foreach (PlainShipRoom plainShipRoom2 in allRooms)
							{
								bool flag3 = plainShipRoom2 == null;
								if (!flag3)
								{
									float num2 = Vector2.Distance(player.transform.position, plainShipRoom2.transform.position);
									bool flag4 = num2 < num;
									if (flag4)
									{
										num = num2;
										plainShipRoom = plainShipRoom2;
									}
								}
							}
							bool flag5 = plainShipRoom != null;
							if (flag5)
							{
								result = SkidMenuPlugin.KillNotificationPatch.GetRoomName(plainShipRoom.RoomId);
							}
							else
							{
								result = "Unknown";
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("GetPlayerLocation error: " + ex.Message);
					result = "Unknown";
				}
				return result;
			}

			// Token: 0x06000042 RID: 66 RVA: 0x00007E98 File Offset: 0x00006098
			private static string GetRoomName(SystemTypes roomId)
			{
				string result;
				switch (roomId)
				{
				case SystemTypes.Hallway:
					result = "Hallway";
					break;
				case SystemTypes.Storage:
					result = "Storage";
					break;
				case SystemTypes.Cafeteria:
					result = "Cafeteria";
					break;
				case SystemTypes.Reactor:
					result = "Reactor";
					break;
				case SystemTypes.UpperEngine:
					result = "Upper Engine";
					break;
				case SystemTypes.Nav:
					result = "Navigation";
					break;
				case SystemTypes.Admin:
					result = "Admin";
					break;
				case SystemTypes.Electrical:
					result = "Electrical";
					break;
				case SystemTypes.LifeSupp:
					result = "O2";
					break;
				case SystemTypes.Shields:
					result = "Shields";
					break;
				case SystemTypes.MedBay:
					result = "Medbay";
					break;
				case SystemTypes.Security:
					result = "Security";
					break;
				case SystemTypes.Weapons:
					result = "Weapons";
					break;
				case SystemTypes.LowerEngine:
					result = "Lower Engine";
					break;
				case SystemTypes.Comms:
					result = "Communications";
					break;
				case SystemTypes.ShipTasks:
					result = "Ship Tasks";
					break;
				case SystemTypes.Doors:
					result = "Doors";
					break;
				case SystemTypes.Sabotage:
					result = "Sabotage";
					break;
				case SystemTypes.Decontamination:
					result = "Decontamination";
					break;
				case SystemTypes.Launchpad:
					result = "Launchpad";
					break;
				case SystemTypes.LockerRoom:
					result = "Locker Room";
					break;
				case SystemTypes.Laboratory:
					result = "Laboratory";
					break;
				case SystemTypes.Balcony:
					result = "Balcony";
					break;
				case SystemTypes.Office:
					result = "Office";
					break;
				case SystemTypes.Greenhouse:
					result = "Greenhouse";
					break;
				case SystemTypes.Dropship:
					result = "Dropship";
					break;
				case SystemTypes.Decontamination2:
					result = "Decontamination 2";
					break;
				case SystemTypes.Outside:
					result = "Outside";
					break;
				case SystemTypes.Specimens:
					result = "Specimens";
					break;
				case SystemTypes.BoilerRoom:
					result = "Boiler Room";
					break;
				case SystemTypes.VaultRoom:
					result = "Vault";
					break;
				case SystemTypes.Cockpit:
					result = "Cockpit";
					break;
				case SystemTypes.Armory:
					result = "Armory";
					break;
				case SystemTypes.Kitchen:
					result = "Kitchen";
					break;
				case SystemTypes.ViewingDeck:
					result = "Viewing Deck";
					break;
				case SystemTypes.HallOfPortraits:
					result = "Hall of Portraits";
					break;
				case SystemTypes.CargoBay:
					result = "Cargo Bay";
					break;
				case SystemTypes.Ventilation:
					result = "Ventilation";
					break;
				case SystemTypes.Showers:
					result = "Showers";
					break;
				case SystemTypes.Engine:
					result = "Engine";
					break;
				case SystemTypes.Brig:
					result = "Brig";
					break;
				case SystemTypes.MeetingRoom:
					result = "Meeting Room";
					break;
				case SystemTypes.Records:
					result = "Records";
					break;
				case SystemTypes.Lounge:
					result = "Lounge";
					break;
				case SystemTypes.GapRoom:
					result = "Gap Room";
					break;
				case SystemTypes.MainHall:
					result = "Main Hall";
					break;
				case SystemTypes.Medical:
					result = "Medical";
					break;
				default:
					result = roomId.ToString();
					break;
				}
				return result;
			}
		}

		// Token: 0x02000014 RID: 20
		[HarmonyPatch(typeof(VoteBanSystem))]
		public static class DisableVotekicksPatch
		{
			// Token: 0x06000043 RID: 67 RVA: 0x00008160 File Offset: 0x00006360
			[HarmonyPatch("AddVote")]
			[HarmonyPrefix]
			public static bool Prefix(int srcClient, int clientId)
			{
				bool result;
				try
				{
					bool flag = !SkidMenuPlugin.DisableVotekicks || AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
					if (flag)
					{
						result = true;
					}
					else
					{
						int hostId = AmongUsClient.Instance.HostId;
						bool flag2 = clientId == hostId;
						if (flag2)
						{
							ClientData client = AmongUsClient.Instance.GetClient(srcClient);
							string str = ((client != null) ? client.PlayerName : null) ?? ("Client " + srcClient.ToString());
							SkidMenuPlugin.Logger.LogWarning("\ud83d\udee1? Blocked votekick against host from " + str);
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>" + str + "</color> tried to kick you!", true);
							}
							result = false;
						}
						else
						{
							bool flag4 = srcClient != hostId;
							if (flag4)
							{
								ClientData client2 = AmongUsClient.Instance.GetClient(srcClient);
								ClientData client3 = AmongUsClient.Instance.GetClient(clientId);
								string text = ((client2 != null) ? client2.PlayerName : null) ?? ("Client " + srcClient.ToString());
								string text2 = ((client3 != null) ? client3.PlayerName : null) ?? ("Client " + clientId.ToString());
								SkidMenuPlugin.Logger.LogWarning("\ud83d\udeab Blocked votekick: " + text + " tried to vote " + text2);
								bool flag5 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
								if (flag5)
								{
									DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Concat(new string[]
									{
										"<color=yellow>[VoteLock]</color> ",
										text,
										" tried to vote ",
										text2,
										", but voting is disabled."
									}), true);
								}
								result = false;
							}
							else
							{
								result = true;
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("DisableVotekicks error: " + ex.Message);
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000015 RID: 21
		[HarmonyPatch(typeof(HudManager), "Update")]
		public static class HudManager_Update
		{
			// Token: 0x06000044 RID: 68 RVA: 0x0000839C File Offset: 0x0000659C
			[HarmonyPostfix]
			public static void Postfix(HudManager __instance)
			{
				try
				{
					bool alwaysShowChatEnabled = SkidMenuPlugin.AlwaysShowChatEnabled;
					if (alwaysShowChatEnabled)
					{
						__instance.Chat.gameObject.SetActive(true);
					}
					bool noShadowsEnabled = SkidMenuPlugin.NoShadowsEnabled;
					if (noShadowsEnabled)
					{
						__instance.ShadowQuad.gameObject.SetActive(false);
					}
					bool zoomOutEnabled = SkidMenuPlugin.ZoomOutEnabled;
					if (zoomOutEnabled)
					{
						Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * 5f, 3f, 30f);
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("HudManager_Update error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000016 RID: 22
		[HarmonyPatch(typeof(GameContainer), "SetupGameInfo")]
		public static class MoreLobbyInfoPatch
		{
			// Token: 0x06000045 RID: 69 RVA: 0x00008458 File Offset: 0x00006658
			[HarmonyPostfix]
			public static void Postfix(GameContainer __instance)
			{
				bool flag = !SkidMenuPlugin.MoreLobbyInfoEnabled;
				if (!flag)
				{
					string trueHostName = __instance.gameListing.TrueHostName;
					int age = __instance.gameListing.Age;
					string text = string.Format("Age: {0}:{1}{2}", age / 60, (age % 60 < 10) ? "0" : "", age % 60);
					string text2 = __instance.gameListing.Platform.ToString();
					__instance.capacity.text = string.Concat(new string[]
					{
						"<size=40%><#0000>000000000000000</color>\n",
						trueHostName,
						"\n",
						__instance.capacity.text,
						"\n<#fb0>",
						GameCode.IntToGameName(__instance.gameListing.GameId),
						"</color>\n<#b0f>",
						text2,
						"</color>\n",
						text,
						"\n<#0000>000000000000000</color></size>"
					});
				}
			}
		}

		// Token: 0x02000017 RID: 23
		[HarmonyPatch(typeof(PlayerBanData), "BanMinutesLeft")]
		public static class AvoidPenaltiesPatch
		{
			// Token: 0x06000046 RID: 70 RVA: 0x00008554 File Offset: 0x00006754
			[HarmonyPostfix]
			public static void Postfix(PlayerBanData __instance, ref int __result)
			{
				bool flag = !SkidMenuPlugin.AvoidPenaltiesEnabled;
				if (!flag)
				{
					__instance.BanPoints = 0f;
					__result = 0;
				}
			}
		}

		// Token: 0x02000018 RID: 24
		[HarmonyPatch(typeof(MeetingHud), "Update")]
		public static class RevealVotesPatch
		{
			// Token: 0x06000047 RID: 71 RVA: 0x00008580 File Offset: 0x00006780
			[HarmonyPrefix]
			public static void Prefix(MeetingHud __instance)
			{
				bool flag = !SkidMenuPlugin.RevealVotesEnabled;
				if (!flag)
				{
					try
					{
						bool flag2 = __instance.state < MeetingHud.VoteStates.Results;
						if (flag2)
						{
							foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
							{
								bool flag3 = !playerVoteArea;
								if (!flag3)
								{
									NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(playerVoteArea.TargetPlayerId);
									bool flag4 = playerById != null && !playerById.Disconnected && playerVoteArea.VotedFor != PlayerVoteArea.HasNotVoted && playerVoteArea.VotedFor != PlayerVoteArea.MissedVote && playerVoteArea.VotedFor != PlayerVoteArea.DeadVote && !SkidMenuPlugin.RevealVotesPatch._votedPlayers.Contains((int)playerVoteArea.TargetPlayerId);
									if (flag4)
									{
										SkidMenuPlugin.RevealVotesPatch._votedPlayers.Add((int)playerVoteArea.TargetPlayerId);
										bool flag5 = playerVoteArea.VotedFor != PlayerVoteArea.SkippedVote;
										if (flag5)
										{
											foreach (PlayerVoteArea playerVoteArea2 in __instance.playerStates)
											{
												bool flag6 = playerVoteArea2.TargetPlayerId == playerVoteArea.VotedFor;
												if (flag6)
												{
													__instance.BloopAVoteIcon(playerById, 0, playerVoteArea2.transform);
													break;
												}
											}
										}
										else
										{
											bool flag7 = __instance.SkippedVoting;
											if (flag7)
											{
												__instance.BloopAVoteIcon(playerById, 0, __instance.SkippedVoting.transform);
											}
										}
									}
								}
							}
							foreach (PlayerVoteArea playerVoteArea3 in __instance.playerStates)
							{
								bool flag8 = !playerVoteArea3;
								if (!flag8)
								{
									VoteSpreader component = playerVoteArea3.transform.GetComponent<VoteSpreader>();
									bool flag9 = !component;
									if (!flag9)
									{
										foreach (SpriteRenderer spriteRenderer in component.Votes)
										{
											spriteRenderer.gameObject.SetActive(true);
										}
									}
								}
							}
							bool flag10 = __instance.SkippedVoting;
							if (flag10)
							{
								__instance.SkippedVoting.SetActive(true);
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("RevealVotes error: " + ex.Message);
					}
				}
			}

			// Token: 0x04000107 RID: 263
			internal static System.Collections.Generic.List<int> _votedPlayers = new System.Collections.Generic.List<int>();
		}

		// Token: 0x02000019 RID: 25
		[HarmonyPatch(typeof(MeetingHud), "PopulateResults")]
		public static class RevealVotesCleanupPatch
		{
			// Token: 0x06000049 RID: 73 RVA: 0x00008864 File Offset: 0x00006A64
			[HarmonyPrefix]
			public static void Prefix(MeetingHud __instance)
			{
				bool flag = !SkidMenuPlugin.RevealVotesEnabled;
				if (!flag)
				{
					try
					{
						foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
						{
							bool flag2 = !playerVoteArea;
							if (!flag2)
							{
								VoteSpreader component = playerVoteArea.transform.GetComponent<VoteSpreader>();
								bool flag3 = !component;
								if (!flag3)
								{
									bool flag4 = component.Votes.Count == 0;
									if (!flag4)
									{
										foreach (SpriteRenderer obj in component.Votes)
										{
											UnityEngine.Object.DestroyImmediate(obj);
										}
										component.Votes.Clear();
									}
								}
							}
						}
						bool flag5 = __instance.SkippedVoting;
						if (flag5)
						{
							VoteSpreader component2 = __instance.SkippedVoting.transform.GetComponent<VoteSpreader>();
							foreach (SpriteRenderer obj2 in component2.Votes)
							{
								UnityEngine.Object.DestroyImmediate(obj2);
							}
							component2.Votes.Clear();
						}
						SkidMenuPlugin.RevealVotesPatch._votedPlayers.Clear();
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("RevealVotesCleanup error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x0200001A RID: 26
		[HarmonyPatch(typeof(PlayerControl), "SetName")]
		public static class PlayerNametagsPatch
		{
			// Token: 0x0600004A RID: 74 RVA: 0x000089F4 File Offset: 0x00006BF4
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance)
			{
				SkidMenuPlugin.PlayerNametagsPatch.ApplyNametag(__instance);
			}

			// Token: 0x0600004B RID: 75 RVA: 0x00008A00 File Offset: 0x00006C00
			public static void RefreshAll()
			{
				try
				{
					foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
					{
						bool flag = playerControl != null;
						if (flag)
						{
							SkidMenuPlugin.PlayerNametagsPatch.ApplyNametag(playerControl);
						}
					}
				}
				catch
				{
				}
			}

			// Token: 0x0600004C RID: 76 RVA: 0x00008A78 File Offset: 0x00006C78
			public static void RestoreAll()
			{
				try
				{
					foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
					{
						bool flag = ((playerControl != null) ? playerControl.Data : null) == null;
						if (!flag)
						{
							CosmeticsLayer cosmetics = playerControl.cosmetics;
							bool flag2 = ((cosmetics != null) ? cosmetics.nameText : null) != null;
							if (flag2)
							{
								playerControl.cosmetics.nameText.text = (playerControl.Data.PlayerName ?? "");
							}
						}
					}
				}
				catch
				{
				}
			}

			// Token: 0x0600004D RID: 77 RVA: 0x00008B38 File Offset: 0x00006D38
			public static void ApplyNametag(PlayerControl __instance)
			{
				bool flag = __instance == null || __instance.Data == null || __instance.Data.Disconnected;
				if (!flag)
				{
					bool flag2 = PlayerControl.LocalPlayer == null;
					if (!flag2)
					{
						bool flag3 = SkidMenuPlugin.IsPlayerBlacklisted(__instance);
						bool flag4 = SkidMenuPlugin.SeeModUsersEnabled && SkidMenuPlugin.detectedModUsers.ContainsKey(__instance.PlayerId);
						bool flag5 = !SkidMenuPlugin.ShowPlayerInfo && !SkidMenuPlugin.SeeRolesEnabled && !flag3 && !flag4;
						if (flag5)
						{
							CosmeticsLayer cosmetics = __instance.cosmetics;
							bool flag6 = ((cosmetics != null) ? cosmetics.nameText : null) != null;
							if (flag6)
							{
								__instance.cosmetics.nameText.text = (__instance.Data.PlayerName ?? "");
							}
						}
						else
						{
							try
							{
								NetworkedPlayerInfo data = __instance.Data;
								bool flag7 = data.Role == null;
								if (!flag7)
								{
									string playerName = data.PlayerName;
									bool flag8 = string.IsNullOrEmpty(playerName);
									if (!flag8)
									{
										string text = SkidMenuPlugin.PlayerNametagsPatch.GetNameTag(data, playerName, false);
										bool flag9 = flag4;
										if (flag9)
										{
											byte rpcId = SkidMenuPlugin.detectedModUsers[__instance.PlayerId];
											string modMenuName = SkidMenuPlugin.PlayerNametagsPatch.GetModMenuName(rpcId);
											text = "<color=#FF0000><size=70%>" + modMenuName + " User</size></color>\r\n" + text;
										}
										bool flag10 = flag3;
										if (flag10)
										{
											text = "<color=#FF0000><size=70%>BLACKLIST</size></color>\r\n" + text;
											string item = (data.PlayerName ?? "unknown") + "_" + data.ClientId.ToString();
											bool flag11 = !SkidMenuPlugin.notifiedBlacklistedPlayers.Contains(item);
											if (flag11)
											{
												SkidMenuPlugin.notifiedBlacklistedPlayers.Add(item);
												string text2 = "";
												try
												{
													text2 = __instance.FriendCode;
												}
												catch
												{
												}
												bool flag12 = string.IsNullOrEmpty(text2);
												if (flag12)
												{
													try
													{
														text2 = data.FriendCode;
													}
													catch
													{
													}
												}
												bool flag13 = string.IsNullOrEmpty(text2);
												if (flag13)
												{
													text2 = "unknown";
												}
												HudManager instance = DestroyableSingleton<HudManager>.Instance;
												bool flag14 = ((instance != null) ? instance.Chat : null) != null && PlayerControl.LocalPlayer != null;
												if (flag14)
												{
													DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Concat(new string[]
													{
														"<color=red>[BLACKLIST]</color> <color=orange>",
														data.PlayerName ?? "Unknown",
														"</color> (",
														text2,
														") is in your blacklist!"
													}), true);
												}
											}
										}
										CosmeticsLayer cosmetics2 = __instance.cosmetics;
										bool flag15 = ((cosmetics2 != null) ? cosmetics2.nameText : null) != null;
										if (flag15)
										{
											__instance.cosmetics.nameText.text = text;
										}
									}
								}
							}
							catch (System.Exception ex)
							{
								SkidMenuPlugin.Logger.LogError("PlayerNametags error: " + ex.Message);
							}
						}
					}
				}
			}

			// Token: 0x0600004E RID: 78 RVA: 0x00008E70 File Offset: 0x00007070
			private static string GetModMenuName(byte rpcId)
			{
				bool flag = rpcId == 121;
				string result;
				if (flag)
				{
					result = "SkidMenu";
				}
				else
				{
					bool flag2 = rpcId == 167;
					if (flag2)
					{
						result = "TuffMenu";
					}
					else
					{
						bool flag3 = rpcId == 164;
						if (flag3)
						{
							result = "SickoMenu";
						}
						else
						{
							bool flag4 = rpcId == 85;
							if (flag4)
							{
								result = "AmongUsMenu";
							}
							else
							{
								bool flag5 = rpcId == 150;
								if (flag5)
								{
									result = "BetterAmongUs";
								}
								else
								{
									bool flag6 = rpcId == 250;
									if (flag6)
									{
										result = "KillNetwork";
									}
									else
									{
										bool flag7 = rpcId == 176;
										if (flag7)
										{
											result = "HostGuard";
										}
										else
										{
											bool flag8 = rpcId == 154;
											if (flag8)
											{
												result = "GoatNetClient";
											}
											else
											{
												bool flag9 = rpcId == 162;
												if (flag9)
												{
													result = "NetMenu";
												}
												else
												{
													result = "Unknown Mod";
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return result;
			}

			// Token: 0x0600004F RID: 79 RVA: 0x00008F4C File Offset: 0x0000714C
			public static string GetNameTag(NetworkedPlayerInfo playerInfo, string playerName, bool isChat = false)
			{
				string text = playerName;
				bool flag = playerInfo.Role == null || playerInfo.Disconnected;
				string result;
				if (flag)
				{
					result = text;
				}
				else
				{
					ClientData clientFromPlayerInfo = AmongUsClient.Instance.GetClientFromPlayerInfo(playerInfo);
					ClientData host = AmongUsClient.Instance.GetHost();
					int num = (int)(playerInfo.PlayerLevel + 1U);
					string text2 = "?";
					try
					{
						text2 = SkidMenuPlugin.PlayerNametagsPatch.GetPlatformShortName(clientFromPlayerInfo.PlatformData.Platform);
					}
					catch
					{
					}
					string text3 = (clientFromPlayerInfo == host) ? "Host - " : "";
					string text4 = ColorUtility.ToHtmlStringRGB(playerInfo.Role.TeamColor);
					bool seeRolesEnabled = SkidMenuPlugin.SeeRolesEnabled;
					if (seeRolesEnabled)
					{
						bool showPlayerInfo = SkidMenuPlugin.ShowPlayerInfo;
						if (showPlayerInfo)
						{
							if (isChat)
							{
								text = string.Format("<color=#{0}>{1} <size=70%>{2}</size></color> <size=70%><color=#fb0>{3}Lv:{4} - {5}</color></size>", new object[]
								{
									text4,
									text,
									SkidMenuPlugin.PlayerNametagsPatch.GetRoleName(playerInfo),
									text3,
									num,
									text2
								});
								return text;
							}
							text = string.Format("<size=70%><color=#fb0>{0}Lv:{1} - {2}</color></size>\r\n<color=#{3}><size=70%>{4}</size>\r\n{5}</color>", new object[]
							{
								text3,
								num,
								text2,
								text4,
								SkidMenuPlugin.PlayerNametagsPatch.GetRoleName(playerInfo),
								text
							});
						}
						else
						{
							if (isChat)
							{
								text = string.Concat(new string[]
								{
									"<color=#",
									text4,
									">",
									text,
									" <size=70%>",
									SkidMenuPlugin.PlayerNametagsPatch.GetRoleName(playerInfo),
									"</size></color>"
								});
								return text;
							}
							text = string.Concat(new string[]
							{
								"<color=#",
								text4,
								"><size=70%>",
								SkidMenuPlugin.PlayerNametagsPatch.GetRoleName(playerInfo),
								"</size>\r\n",
								text,
								"</color>"
							});
						}
					}
					else
					{
						bool showPlayerInfo2 = SkidMenuPlugin.ShowPlayerInfo;
						if (showPlayerInfo2)
						{
							bool flag2 = PlayerControl.LocalPlayer.Data.Role.NameColor == playerInfo.Role.NameColor;
							if (flag2)
							{
								if (isChat)
								{
									text = string.Format("<color=#{0}>{1}</color> <size=70%><color=#fb0>{2}Lv:{3} - {4}</color></size>", new object[]
									{
										ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor),
										text,
										text3,
										num,
										text2
									});
									return text;
								}
								text = string.Format("<size=70%><color=#fb0>{0}Lv:{1} - {2}</color></size>\r\n<color=#{3}>{4}</color>", new object[]
								{
									text3,
									num,
									text2,
									ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor),
									text
								});
							}
							else
							{
								if (isChat)
								{
									text = string.Format("{0} <size=70%><color=#fb0>{1}Lv:{2} - {3}</color></size>", new object[]
									{
										text,
										text3,
										num,
										text2
									});
									return text;
								}
								text = string.Format("<size=70%><color=#fb0>{0}Lv:{1} - {2}</color></size>\r\n{3}", new object[]
								{
									text3,
									num,
									text2,
									text
								});
							}
						}
						else
						{
							bool flag3 = PlayerControl.LocalPlayer.Data.Role.NameColor != playerInfo.Role.NameColor || isChat;
							if (flag3)
							{
								return text;
							}
							text = string.Concat(new string[]
							{
								"<color=#",
								ColorUtility.ToHtmlStringRGB(playerInfo.Role.NameColor),
								">",
								text,
								"</color>"
							});
						}
					}
					result = text;
				}
				return result;
			}

			// Token: 0x06000050 RID: 80 RVA: 0x000092D0 File Offset: 0x000074D0
			public static string GetRoleName(NetworkedPlayerInfo playerData)
			{
				string result;
				try
				{
					NetworkedPlayerInfo playerData2 = playerData;
					bool flag = ((playerData2 != null) ? playerData2.Role : null) == null;
					if (flag)
					{
						result = "?";
					}
					else
					{
						string @string = DestroyableSingleton<TranslationController>.Instance.GetString(playerData.Role.StringName, Il2CppSystem.Array.Empty<Il2CppSystem.Object>());
						bool flag2 = @string != "STRMISS";
						if (flag2)
						{
							result = @string;
						}
						else
						{
							bool hasValue = playerData.RoleWhenAlive.HasValue;
							if (hasValue)
							{
								result = DestroyableSingleton<TranslationController>.Instance.GetString(DestroyableSingleton<RoleManager>.Instance.AllRoles.ToArray().First((RoleBehaviour r) => r.Role == playerData.RoleWhenAlive.Value).StringName, Il2CppSystem.Array.Empty<Il2CppSystem.Object>());
							}
							else
							{
								result = "Ghost";
							}
						}
					}
				}
				catch
				{
					result = "?";
				}
				return result;
			}

			// Token: 0x06000051 RID: 81 RVA: 0x000093C4 File Offset: 0x000075C4
			public static string GetPlatformShortName(Platforms platform)
			{
				string result;
				switch (platform)
				{
				case Platforms.StandaloneEpicPC:
					result = "Epic";
					break;
				case Platforms.StandaloneSteamPC:
					result = "Steam";
					break;
				case Platforms.StandaloneMac:
					result = "Mac";
					break;
				case Platforms.StandaloneWin10:
					result = "MS Store";
					break;
				case Platforms.StandaloneItch:
					result = "Itch.io";
					break;
				case Platforms.IPhone:
					result = "iOS/iPad OS";
					break;
				case Platforms.Android:
					result = "Android";
					break;
				case Platforms.Switch:
					result = "Switch";
					break;
				case Platforms.Xbox:
					result = "Xbox";
					break;
				case Platforms.Playstation:
					result = "Playstation";
					break;
				default:
					result = "?";
					break;
				}
				return result;
			}
		}

		// Token: 0x0200001B RID: 27
		[HarmonyPatch(typeof(ChatBubble), "SetName")]
		public static class ChatNametagsPatch
		{
			// Token: 0x06000052 RID: 82 RVA: 0x00009464 File Offset: 0x00007664
			[HarmonyPostfix]
			public static void Postfix(ChatBubble __instance, string playerName)
			{
				bool flag = !SkidMenuPlugin.ShowPlayerInfo && SkidMenuPlugin.BlacklistedCodes.Count == 0;
				if (!flag)
				{
					bool flag2 = PlayerControl.LocalPlayer == null;
					if (!flag2)
					{
						try
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls.ToArray())
							{
								string a;
								if (playerControl2 == null)
								{
									a = null;
								}
								else
								{
									NetworkedPlayerInfo data = playerControl2.Data;
									a = ((data != null) ? data.PlayerName : null);
								}
								bool flag3 = a == playerName;
								if (flag3)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag4 = playerControl == null || playerControl.Data == null;
							if (!flag4)
							{
								NetworkedPlayerInfo data2 = playerControl.Data;
								int playerLevel = (int)data2.PlayerLevel;
								try
								{
									bool flag5 = AmongUsClient.Instance != null;
									if (flag5)
									{
										ClientData client = AmongUsClient.Instance.GetClient(data2.ClientId);
										bool flag6 = ((client != null) ? client.PlatformData : null) != null;
										if (flag6)
										{
											string platformShortName = SkidMenuPlugin.PlayerNametagsPatch.GetPlatformShortName(client.PlatformData.Platform);
										}
										bool flag7 = client != null && client.Id == AmongUsClient.Instance.HostId;
										if (flag7)
										{
										}
									}
								}
								catch
								{
								}
								bool flag8 = SkidMenuPlugin.IsPlayerBlacklisted(playerControl);
								string text = playerName;
								bool flag9 = SkidMenuPlugin.ShowPlayerInfo || SkidMenuPlugin.SeeRolesEnabled;
								if (flag9)
								{
									text = SkidMenuPlugin.PlayerNametagsPatch.GetNameTag(data2, playerName, true);
								}
								bool flag10 = flag8;
								if (flag10)
								{
									text = "<color=#FF0000>[BL]</color> " + text;
								}
								bool flag11 = __instance.NameText != null;
								if (flag11)
								{
									__instance.NameText.text = text;
								}
							}
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("ChatNametags error: " + ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x0200001C RID: 28
		[HarmonyPatch(typeof(PlayerControl))]
		public static class DisableMeetingsPatch
		{
			// Token: 0x06000053 RID: 83 RVA: 0x000096A0 File Offset: 0x000078A0
			[HarmonyPatch("CmdReportDeadBody")]
			[HarmonyPrefix]
			public static bool CmdReportDeadBody_Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
			{
				bool result;
				try
				{
					bool flag = !SkidMenuPlugin.DisableMeetings || AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
					if (flag)
					{
						result = true;
					}
					else
					{
						NetworkedPlayerInfo data = __instance.Data;
						string str = ((data != null) ? data.PlayerName : null) ?? "Unknown";
						SkidMenuPlugin.Logger.LogWarning("\ud83d\udeab Blocked meeting call from " + str);
						bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=yellow>[MeetingLock]</color> Meetings are disabled by host.", true);
						}
						result = false;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("DisableMeetings CmdReportDeadBody error: " + ex.Message);
					result = true;
				}
				return result;
			}

			// Token: 0x06000054 RID: 84 RVA: 0x0000978C File Offset: 0x0000798C
			[HarmonyPatch("ReportDeadBody")]
			[HarmonyPrefix]
			public static bool ReportDeadBody_Prefix(PlayerControl __instance, NetworkedPlayerInfo target)
			{
				bool result;
				try
				{
					bool flag = !SkidMenuPlugin.DisableMeetings || AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
					if (flag)
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("DisableMeetings ReportDeadBody error: " + ex.Message);
					result = true;
				}
				return result;
			}
		}

		// Token: 0x0200001D RID: 29
		[HarmonyPatch(typeof(PlayerControl), "CmdCheckColor")]
		public static class OverflowM1_BlockSendColor
		{
			// Token: 0x06000055 RID: 85 RVA: 0x00009800 File Offset: 0x00007A00
			[HarmonyPrefix]
			public static bool Prefix()
			{
				bool overflowMethod1Enabled = SkidMenuPlugin.OverflowMethod1Enabled;
				bool result;
				if (overflowMethod1Enabled)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked CmdCheckColor");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x0200001E RID: 30
		[HarmonyPatch(typeof(PlayerControl), "CmdCheckName")]
		public static class OverflowM1_BlockSendName
		{
			// Token: 0x06000056 RID: 86 RVA: 0x00009834 File Offset: 0x00007A34
			[HarmonyPrefix]
			public static bool Prefix()
			{
				bool overflowMethod1Enabled = SkidMenuPlugin.OverflowMethod1Enabled;
				bool result;
				if (overflowMethod1Enabled)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked CmdCheckName");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x0200001F RID: 31
		[HarmonyPatch(typeof(PlayerControl), "RpcSetLevel")]
		public static class OverflowM1_BlockSetLevel
		{
			// Token: 0x06000057 RID: 87 RVA: 0x00009868 File Offset: 0x00007A68
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetLevel");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000020 RID: 32
		[HarmonyPatch(typeof(PlayerControl), "RpcSetNamePlate")]
		public static class OverflowM1_BlockSetNamePlate
		{
			// Token: 0x06000058 RID: 88 RVA: 0x000098A4 File Offset: 0x00007AA4
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetNamePlate");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000021 RID: 33
		[HarmonyPatch(typeof(PlayerControl), "RpcSetHat")]
		public static class OverflowM1_BlockSetHat
		{
			// Token: 0x06000059 RID: 89 RVA: 0x000098E0 File Offset: 0x00007AE0
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetHat");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000022 RID: 34
		[HarmonyPatch(typeof(PlayerControl), "RpcSetSkin")]
		public static class OverflowM1_BlockSetSkin
		{
			// Token: 0x0600005A RID: 90 RVA: 0x0000991C File Offset: 0x00007B1C
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetSkin");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000023 RID: 35
		[HarmonyPatch(typeof(PlayerControl), "RpcSetPet")]
		public static class OverflowM1_BlockSetPet
		{
			// Token: 0x0600005B RID: 91 RVA: 0x00009958 File Offset: 0x00007B58
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetPet");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000024 RID: 36
		[HarmonyPatch(typeof(PlayerControl), "RpcSetVisor")]
		public static class OverflowM1_BlockSetVisor
		{
			// Token: 0x0600005C RID: 92 RVA: 0x00009994 File Offset: 0x00007B94
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetVisor");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000025 RID: 37
		[HarmonyPatch(typeof(PlayerControl), "RpcSetRole")]
		public static class OverflowM1_BlockSetRole
		{
			// Token: 0x0600005D RID: 93 RVA: 0x000099D0 File Offset: 0x00007BD0
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetRole");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000026 RID: 38
		[HarmonyPatch(typeof(PlayerControl), "RpcSetScanner")]
		public static class OverflowM1_BlockSetScanner
		{
			// Token: 0x0600005E RID: 94 RVA: 0x00009A0C File Offset: 0x00007C0C
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod1Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M1: Blocked RpcSetScanner");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000027 RID: 39
		[HarmonyPatch(typeof(PlayerControl), "RpcSetNamePlate")]
		public static class OverflowM2_BlockSetNamePlate
		{
			// Token: 0x0600005F RID: 95 RVA: 0x00009A48 File Offset: 0x00007C48
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.OverflowMethod2Enabled && __instance.AmOwner;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.Logger.LogInfo("Overflow M2: Blocked RpcSetNamePlate");
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x02000028 RID: 40
		[HarmonyPatch(typeof(PingTracker), "Update")]
		public static class SkidMenuPingTrackerPatch
		{
			// Token: 0x06000060 RID: 96 RVA: 0x00009A84 File Offset: 0x00007C84
			private static string ApplyShimmer(string text, float pos)
			{
				string text2 = "";
				for (int i = 0; i < text.Length; i++)
				{
					float num = Mathf.Abs((float)i / (float)(text.Length - 1) - pos);
					float t = Mathf.Clamp01(1f - num / 0.3f);
					byte maxValue = byte.MaxValue;
					byte b = (byte)Mathf.Lerp(51f, 153f, t);
					byte b2 = (byte)Mathf.Lerp(51f, 153f, t);
					text2 += string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", new object[]
					{
						maxValue,
						b,
						b2,
						text[i]
					});
				}
				return text2;
			}

			// Token: 0x06000061 RID: 97 RVA: 0x00009B58 File Offset: 0x00007D58
			[HarmonyPostfix]
			[HarmonyPriority(-2147483648)]
			public static void Postfix(PingTracker __instance)
			{
				try
				{
					SkidMenuPlugin.SkidMenuPingTrackerPatch._updateTimer += Time.deltaTime;
					bool flag = SkidMenuPlugin.SkidMenuPingTrackerPatch._updateTimer >= 0.5f;
					if (flag)
					{
						SkidMenuPlugin.SkidMenuPingTrackerPatch._smoothFps = 1f / Time.deltaTime;
						SkidMenuPlugin.SkidMenuPingTrackerPatch._smoothPing = AmongUsClient.Instance.Ping;
						SkidMenuPlugin.SkidMenuPingTrackerPatch._updateTimer = 0f;
					}
					SkidMenuPlugin.SkidMenuPingTrackerPatch._shimmerPos -= Time.deltaTime * 0.8f;
					bool flag2 = SkidMenuPlugin.SkidMenuPingTrackerPatch._shimmerPos < 0f;
					if (flag2)
					{
						SkidMenuPlugin.SkidMenuPingTrackerPatch._shimmerPos = 1f;
					}
					int num = Mathf.RoundToInt(SkidMenuPlugin.SkidMenuPingTrackerPatch._smoothFps);
					int smoothPing = SkidMenuPlugin.SkidMenuPingTrackerPatch._smoothPing;
					string text = (smoothPing < 80) ? "#00FF00" : ((smoothPing < 400) ? "#FFFF00" : "#FF0000");
					string text2 = SkidMenuPlugin.SkidMenuPingTrackerPatch.ApplyShimmer("SkidMenu", SkidMenuPlugin.SkidMenuPingTrackerPatch._shimmerPos);
					string text3 = SkidMenuPlugin.SkidMenuPingTrackerPatch.ApplyShimmer("Skid", SkidMenuPlugin.SkidMenuPingTrackerPatch._shimmerPos);
					string text4 = string.Format("{0} <color=#D2DB42>v1.0.7</color> <color=#FFFFFF>by</color> {1} • <color=#FFFFFF>PING:</color> <color={2}>{3} ms</color> • <color=#FFFFFF>FPS:</color> <color=#00FF00>{4}</color>", new object[]
					{
						text2,
						text3,
						text,
						smoothPing,
						num
					});
					bool flag3 = SkidMenuPlugin.ShowHostEnabled && AmongUsClient.Instance != null;
					if (flag3)
					{
						string hostInfo = SkidMenuPlugin.SkidMenuPingTrackerPatch.GetHostInfo();
						bool flag4 = !string.IsNullOrEmpty(hostInfo);
						if (flag4)
						{
							text4 = text4 + " • " + hostInfo;
						}
					}
					__instance.text.text = text4;
					__instance.text.alignment = 514;
					bool isGameStarted = AmongUsClient.Instance.IsGameStarted;
					if (isGameStarted)
					{
						__instance.aspectPosition.Alignment = AspectPosition.EdgeAlignments.Bottom;
						__instance.aspectPosition.DistanceFromEdge = new Vector3(-0.2f, 0.55f, 0f);
						__instance.aspectPosition.AdjustPosition();
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("PingTracker error: " + ex.Message);
				}
			}

			// Token: 0x06000062 RID: 98 RVA: 0x00009D60 File Offset: 0x00007F60
			private static string GetHostInfo()
			{
				string result;
				try
				{
					bool flag = AmongUsClient.Instance == null;
					if (flag)
					{
						result = "";
					}
					else
					{
						ClientData host = AmongUsClient.Instance.GetHost();
						bool flag2 = host == null;
						if (flag2)
						{
							result = "";
						}
						else
						{
							string str = host.PlayerName ?? "Unknown";
							bool amHost = AmongUsClient.Instance.AmHost;
							bool flag3 = amHost;
							if (flag3)
							{
								result = "<color=#FFFFFF>Host:</color> <color=#FFD700>" + str + "</color> <color=#00FF00>(You)</color>";
							}
							else
							{
								result = "<color=#FFFFFF>Host:</color> <color=#FFD700>" + str + "</color>";
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("GetHostInfo error: " + ex.Message);
					result = "";
				}
				return result;
			}

			// Token: 0x04000108 RID: 264
			private static float _smoothFps = 0f;

			// Token: 0x04000109 RID: 265
			private static int _smoothPing = 0;

			// Token: 0x0400010A RID: 266
			private static float _updateTimer = 0f;

			// Token: 0x0400010B RID: 267
			private static float _shimmerPos = 1f;
		}

		// Token: 0x02000029 RID: 41
		[HarmonyPatch(typeof(FindAGameManager))]
		public static class FindDatersLobbyPatch
		{
			// Token: 0x06000064 RID: 100 RVA: 0x00009E58 File Offset: 0x00008058
			[HarmonyPatch("Update")]
			[HarmonyPostfix]
			public static void Update_Postfix(FindAGameManager __instance)
			{
				try
				{
					bool flag = SkidMenuPlugin.FindDatersEnabled != SkidMenuPlugin.FindDatersLobbyPatch.lastState;
					if (flag)
					{
						bool findDatersEnabled = SkidMenuPlugin.FindDatersEnabled;
						if (findDatersEnabled)
						{
							SkidMenuPlugin.FindDatersLobbyPatch.ApplyFilters(__instance);
							SkidMenuPlugin.Logger.LogInfo("Find Daters enabled - filters applied");
						}
						else
						{
							SkidMenuPlugin.FindDatersLobbyPatch.ClearFilters(__instance);
							SkidMenuPlugin.Logger.LogInfo("Find Daters disabled - filters cleared");
						}
						SkidMenuPlugin.FindDatersLobbyPatch.lastState = SkidMenuPlugin.FindDatersEnabled;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("FindDatersLobby Update error: " + ex.Message);
				}
			}

			// Token: 0x06000065 RID: 101 RVA: 0x00009EF8 File Offset: 0x000080F8
			private static void ApplyFilters(FindAGameManager instance)
			{
				try
				{
					bool flag = instance == null;
					if (!flag)
					{
						instance.ClearAllFilters();
						instance.AddIntFilterValue(1, "NumImpostors", Int32OptionNames.NumImpostors);
						for (int i = 4; i <= 9; i++)
						{
							instance.AddIntFilterValue(i, "MaxPlayers", Int32OptionNames.MaxPlayers);
						}
						instance.AddChatFilterValue(QuickChatModes.FreeChatOrQuickChat, false);
						SkidMenuPlugin.Logger.LogInfo("? Find Daters filters applied");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ApplyFilters error: " + ex.Message);
				}
			}

			// Token: 0x06000066 RID: 102 RVA: 0x00009F9C File Offset: 0x0000819C
			private static void ClearFilters(FindAGameManager instance)
			{
				try
				{
					bool flag = instance == null;
					if (!flag)
					{
						instance.ClearAllFilters();
						SkidMenuPlugin.Logger.LogInfo("? All filters cleared - showing all lobbies");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ClearFilters error: " + ex.Message);
				}
			}

			// Token: 0x06000067 RID: 103 RVA: 0x0000A004 File Offset: 0x00008204
			public static void Reset()
			{
				SkidMenuPlugin.FindDatersLobbyPatch.lastState = false;
			}

			// Token: 0x0400010C RID: 268
			private static bool lastState;
		}

		// Token: 0x0200002A RID: 42
		[HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
		public static class BanBlacklistedPatch
		{
			// Token: 0x06000068 RID: 104 RVA: 0x0000A010 File Offset: 0x00008210
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance)
			{
				try
				{
					bool flag = !SkidMenuPlugin.BanBlacklistedEnabled;
					if (!flag)
					{
						bool flag2 = !AmongUsClient.Instance.AmHost;
						if (!flag2)
						{
							bool flag3 = __instance == null || __instance.Data == null;
							if (!flag3)
							{
								bool amOwner = __instance.AmOwner;
								if (!amOwner)
								{
									int clientId = __instance.Data.ClientId;
									bool flag4 = SkidMenuPlugin.BanBlacklistedPatch.checkedClients.Contains(clientId);
									if (!flag4)
									{
										SkidMenuPlugin.BanBlacklistedPatch.checkedClients.Add(clientId);
										string text = "";
										try
										{
											text = __instance.FriendCode;
										}
										catch
										{
										}
										bool flag5 = string.IsNullOrEmpty(text);
										if (flag5)
										{
											try
											{
												text = __instance.Data.FriendCode;
											}
											catch
											{
											}
										}
										bool flag6 = string.IsNullOrEmpty(text);
										if (flag6)
										{
											try
											{
												ClientData client = AmongUsClient.Instance.GetClient(clientId);
												bool flag7 = client != null;
												if (flag7)
												{
													text = (client.FriendCode ?? "");
												}
											}
											catch
											{
											}
										}
										bool flag8 = string.IsNullOrEmpty(text);
										if (!flag8)
										{
											bool flag9 = SkidMenuPlugin.BlacklistedCodes.Contains(text.ToLower().Trim());
											if (flag9)
											{
												string text2 = __instance.Data.PlayerName ?? "Unknown";
												AmongUsClient.Instance.KickPlayer(clientId, true);
												SkidMenuPlugin.Logger.LogWarning(string.Concat(new string[]
												{
													"[BanBlacklist] Banned: ",
													text2,
													" (",
													text,
													")"
												}));
												bool flag10 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null && PlayerControl.LocalPlayer != null;
												if (flag10)
												{
													DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Concat(new string[]
													{
														"<color=red>[BanBlacklist]</color> Auto-banned <color=orange>",
														text2,
														"</color> (",
														text,
														")"
													}), true);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("BanBlacklistedPatch error: " + ex.Message);
				}
			}

			// Token: 0x06000069 RID: 105 RVA: 0x0000A2AC File Offset: 0x000084AC
			public static void Reset()
			{
				SkidMenuPlugin.BanBlacklistedPatch.checkedClients.Clear();
			}

			// Token: 0x0400010D RID: 269
			private static HashSet<int> checkedClients = new HashSet<int>();
		}

		// Token: 0x0200002B RID: 43
		[HarmonyPatch(typeof(PlayerControl), "FixedUpdate")]
		public static class SeeGhostsPatch
		{
			// Token: 0x0600006B RID: 107 RVA: 0x0000A2C8 File Offset: 0x000084C8
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance)
			{
				bool flag = !SkidMenuPlugin.SeeGhostsEnabled;
				if (!flag)
				{
					bool flag2 = __instance == null || __instance.Data == null;
					if (!flag2)
					{
						bool flag3 = !__instance.Data.IsDead;
						if (!flag3)
						{
							try
							{
								__instance.Visible = true;
								__instance.gameObject.layer = LayerMask.NameToLayer("Players");
							}
							catch
							{
							}
						}
					}
				}
			}
		}

		// Token: 0x0200002C RID: 44
		[HarmonyPatch(typeof(ChatController), "AddChat")]
		public static class ForceShowDeadChatPatch
		{
			// Token: 0x0600006C RID: 108 RVA: 0x0000A350 File Offset: 0x00008550
			[HarmonyPrefix]
			public static void Prefix(ChatController __instance, PlayerControl sourcePlayer, string chatText)
			{
				SkidMenuPlugin.ForceShowDeadChatPatch._stateModified = false;
				bool flag = !SkidMenuPlugin.SeeGhostsEnabled;
				if (!flag)
				{
					bool flag2 = ((sourcePlayer != null) ? sourcePlayer.Data : null) == null;
					if (!flag2)
					{
						bool flag3 = !sourcePlayer.Data.IsDead;
						if (!flag3)
						{
							PlayerControl localPlayer = PlayerControl.LocalPlayer;
							bool flag4 = ((localPlayer != null) ? localPlayer.Data : null) == null;
							if (!flag4)
							{
								bool isDead = PlayerControl.LocalPlayer.Data.IsDead;
								if (!isDead)
								{
									SkidMenuPlugin.ForceShowDeadChatPatch._originalDeadState = PlayerControl.LocalPlayer.Data.IsDead;
									PlayerControl.LocalPlayer.Data.IsDead = true;
									SkidMenuPlugin.ForceShowDeadChatPatch._stateModified = true;
								}
							}
						}
					}
				}
			}

			// Token: 0x0600006D RID: 109 RVA: 0x0000A400 File Offset: 0x00008600
			[HarmonyPostfix]
			public static void Postfix(ChatController __instance, PlayerControl sourcePlayer)
			{
				bool flag = !SkidMenuPlugin.ForceShowDeadChatPatch._stateModified;
				if (!flag)
				{
					PlayerControl localPlayer = PlayerControl.LocalPlayer;
					bool flag2 = ((localPlayer != null) ? localPlayer.Data : null) == null;
					if (!flag2)
					{
						PlayerControl.LocalPlayer.Data.IsDead = SkidMenuPlugin.ForceShowDeadChatPatch._originalDeadState;
						SkidMenuPlugin.ForceShowDeadChatPatch._stateModified = false;
					}
				}
			}

			// Token: 0x0400010E RID: 270
			private static bool _originalDeadState;

			// Token: 0x0400010F RID: 271
			private static bool _stateModified;
		}

		// Token: 0x0200002D RID: 45
		[HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
		public static class GhostChatRpcPatch
		{
			// Token: 0x0600006E RID: 110 RVA: 0x0000A454 File Offset: 0x00008654
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance, byte callId, MessageReader reader)
			{
				bool flag = callId != 8;
				if (!flag)
				{
					bool flag2 = !SkidMenuPlugin.SeeGhostsEnabled;
					if (!flag2)
					{
						bool flag3 = PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data == null;
						if (!flag3)
						{
							bool isDead = PlayerControl.LocalPlayer.Data.IsDead;
							if (!isDead)
							{
								bool flag4 = __instance == null || __instance.Data == null;
								if (!flag4)
								{
									bool flag5 = !__instance.Data.IsDead;
									if (!flag5)
									{
										try
										{
											int position = reader.Position;
											reader.Position = 0;
											string str = reader.ReadString();
											reader.Position = position;
											HudManager instance = DestroyableSingleton<HudManager>.Instance;
											bool flag6 = ((instance != null) ? instance.Chat : null) != null;
											if (flag6)
											{
												DestroyableSingleton<HudManager>.Instance.Chat.AddChat(__instance, "[GHOST] " + str, true);
											}
										}
										catch (System.Exception ex)
										{
											SkidMenuPlugin.Logger.LogError("GhostChatRpc error: " + ex.Message);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0200002E RID: 46
		[HarmonyPatch(typeof(HatManager), "Initialize")]
		public static class CosmeticsUnlockerPatch
		{
			// Token: 0x0600006F RID: 111 RVA: 0x0000A598 File Offset: 0x00008798
			[HarmonyPostfix]
			public static void Postfix(HatManager __instance)
			{
				bool flag = !SkidMenuPlugin.CosmeticsUnlockerEnabled;
				if (!flag)
				{
					try
					{
						foreach (BundleData bundleData in __instance.allBundles)
						{
							bundleData.Free = true;
						}
						foreach (BundleData bundleData2 in __instance.allFeaturedBundles)
						{
							bundleData2.Free = true;
						}
						foreach (CosmicubeData cosmicubeData in __instance.allFeaturedCubes)
						{
							cosmicubeData.Free = true;
						}
						foreach (CosmeticData cosmeticData in __instance.allFeaturedItems)
						{
							cosmeticData.Free = true;
						}
						foreach (HatData hatData in __instance.allHats)
						{
							hatData.Free = true;
						}
						foreach (NamePlateData namePlateData in __instance.allNamePlates)
						{
							namePlateData.Free = true;
						}
						foreach (PetData petData in __instance.allPets)
						{
							petData.Free = true;
						}
						foreach (SkinData skinData in __instance.allSkins)
						{
							skinData.Free = true;
						}
						foreach (StarBundle starBundle in __instance.allStarBundles)
						{
							starBundle.price = 0f;
						}
						foreach (VisorData visorData in __instance.allVisors)
						{
							visorData.Free = true;
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("CosmeticsUnlocker error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x0200002F RID: 47
		[HarmonyPatch(typeof(AmongUsClient), "Update")]
		public static class SpoofLevelPatch
		{
			// Token: 0x06000070 RID: 112 RVA: 0x0000A868 File Offset: 0x00008A68
			[HarmonyPostfix]
			public static void Postfix()
			{
				try
				{
					int value = SkidMenuPlugin.Config_SpoofLevel.Value;
					bool flag = value <= 0;
					if (!flag)
					{
						DataManager.Player.Stats.Level = (uint)value;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("SpoofLevel error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000030 RID: 48
		[HarmonyPatch(typeof(FindAGameManager), "Start")]
		public static class ExtendedLobbyListPatch
		{
			// Token: 0x06000071 RID: 113 RVA: 0x0000A8D4 File Offset: 0x00008AD4
			[HarmonyPrefix]
			public static bool Prefix(FindAGameManager __instance)
			{
				bool flag = !SkidMenuPlugin.ExtendedLobbyEnabled;
				bool result;
				if (flag)
				{
					SkidMenuPlugin.ExtendedLobbyListPatch.Reset();
					result = true;
				}
				else
				{
					try
					{
						bool flag2 = SkidMenuPlugin.ExtendedLobbyListPatch.hasSetupExtendedList;
						if (flag2)
						{
							SkidMenuPlugin.ExtendedLobbyListPatch.hasSetupExtendedList = false;
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller = null;
						}
						bool flag3 = SkidMenuPlugin.ExtendedLobbyListPatch.hasSetupExtendedList;
						if (flag3)
						{
							result = true;
						}
						else
						{
							GameContainer gameContainer = __instance.gameContainers[4];
							GameObject gameObject = new GameObject("GameListScroller");
							gameObject.transform.SetParent(gameContainer.transform.parent);
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller = gameObject.AddComponent<Scroller>();
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.Inner = gameObject.transform;
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.MouseMustBeOverToScroll = true;
							BoxCollider2D boxCollider2D = gameContainer.transform.parent.gameObject.AddComponent<BoxCollider2D>();
							boxCollider2D.size = new Vector2(100f, 100f);
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.ClickMask = boxCollider2D;
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.ScrollWheelSpeed = 0.3f;
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.SetYBoundsMin(0f);
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.SetYBoundsMax(3.5f);
							SkidMenuPlugin.ExtendedLobbyListPatch.scroller.allowY = true;
							foreach (GameContainer gameContainer2 in __instance.gameContainers)
							{
								gameContainer2.transform.SetParent(gameObject.transform);
								Vector3 position = gameContainer2.transform.position;
								gameContainer2.transform.position = new Vector3(position.x, position.y, 25f);
							}
							System.Collections.Generic.List<GameContainer> list = new System.Collections.Generic.List<GameContainer>(__instance.gameContainers);
							for (int i = 0; i < 15; i++)
							{
								GameContainer gameContainer3 = UnityEngine.Object.Instantiate<GameContainer>(gameContainer, gameObject.transform);
								Vector3 position2 = gameContainer3.transform.position;
								gameContainer3.transform.position = new Vector3(position2.x, position2.y - 0.75f * (float)(i + 1), 25f);
								list.Add(gameContainer3);
							}
							__instance.gameContainers = list.ToArray();
							GameObject gameObject2 = new GameObject("CutOffTop");
							SpriteRenderer spriteRenderer = gameObject2.AddComponent<SpriteRenderer>();
							Texture2D texture2D = new Texture2D(100, 100);
							Color[] array = texture2D.GetPixels();
							for (int j = 0; j < array.Length; j++)
							{
								array[j] = Color.black;
							}
							texture2D.SetPixels(array);
							texture2D.Apply();
							Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, 1f, 1f), Vector2.one * 0.5f);
							spriteRenderer.sprite = sprite;
							gameObject2.transform.SetParent(gameObject.transform.parent);
							gameObject2.transform.localPosition = new Vector3(0f, 3f, 1f);
							gameObject2.transform.localScale = new Vector3(1500f, 200f, 100f);
							SkidMenuPlugin.ExtendedLobbyListPatch.hasSetupExtendedList = true;
							result = true;
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("ExtendedLobbyList error: " + ex.Message);
						SkidMenuPlugin.ExtendedLobbyListPatch.Reset();
						result = true;
					}
				}
				return result;
			}

			// Token: 0x06000072 RID: 114 RVA: 0x0000AC6C File Offset: 0x00008E6C
			[HarmonyPatch(typeof(FindAGameManager), "RefreshList")]
			[HarmonyPostfix]
			public static void RefreshList_Postfix()
			{
				try
				{
					bool flag = SkidMenuPlugin.ExtendedLobbyEnabled && SkidMenuPlugin.ExtendedLobbyListPatch.scroller != null;
					if (flag)
					{
						SkidMenuPlugin.ExtendedLobbyListPatch.scroller.ScrollRelative(new Vector2(0f, -100f));
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ExtendedLobbyList RefreshList error: " + ex.Message);
				}
			}

			// Token: 0x06000073 RID: 115 RVA: 0x0000ACE4 File Offset: 0x00008EE4
			public static void Reset()
			{
				SkidMenuPlugin.ExtendedLobbyListPatch.hasSetupExtendedList = false;
				SkidMenuPlugin.ExtendedLobbyListPatch.scroller = null;
			}

			// Token: 0x04000110 RID: 272
			private static Scroller scroller;

			// Token: 0x04000111 RID: 273
			private static bool hasSetupExtendedList;
		}

		// Token: 0x02000031 RID: 49
		[HarmonyPatch(typeof(PlayerControl), "FixedUpdate")]
		public static class DestroyLobbyPatch
		{
			// Token: 0x06000074 RID: 116 RVA: 0x0000ACF4 File Offset: 0x00008EF4
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance)
			{
				bool flag = SkidMenuPlugin.DestroyLobbyEnabled && !SkidMenuPlugin.OverloadEnabled;
				if (flag)
				{
					SkidMenuPlugin.OverloadEnabled = true;
					SkidMenuPlugin.Logger.LogInfo("Destroy Lobby activated - using Overload Method 1");
				}
			}

			// Token: 0x06000075 RID: 117 RVA: 0x0000AD30 File Offset: 0x00008F30
			private static bool IsInLobby()
			{
				bool result;
				try
				{
					GameObject x = GameObject.Find("Lobby(Clone)");
					result = (x != null);
				}
				catch
				{
					result = false;
				}
				return result;
			}
		}

		// Token: 0x02000032 RID: 50
		[HarmonyPatch(typeof(GameManager), "RpcEndGame")]
		public static class DisableGameEndPatch
		{
			// Token: 0x06000076 RID: 118 RVA: 0x0000AD6C File Offset: 0x00008F6C
			[HarmonyPrefix]
			public static bool Prefix()
			{
				bool flag = !SkidMenuPlugin.DisableGameEndEnabled || !AmongUsClient.Instance.AmHost;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					SkidMenuPlugin.Logger.LogInfo("Blocked game end attempt");
					result = false;
				}
				return result;
			}
		}

		// Token: 0x02000033 RID: 51
		[HarmonyPatch(typeof(RoleManager), "SelectRoles")]
		public static class ForceRolePatch
		{
			// Token: 0x06000077 RID: 119 RVA: 0x0000ADB0 File Offset: 0x00008FB0
			[HarmonyPrefix]
			public static bool Prefix(RoleManager __instance)
			{
				bool flag = SkidMenuPlugin.forcedRoles.Count == 0 || !AmongUsClient.Instance.AmHost;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					try
					{
						SkidMenuPlugin.Logger.LogInfo("=== CUSTOM ROLE ASSIGNMENT START ===");
						Il2CppArrayBase<PlayerControl> il2CppArrayBase = PlayerControl.AllPlayerControls.ToArray();
						IGameOptions currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
						System.Collections.Generic.List<byte> list = new System.Collections.Generic.List<byte>();
						foreach (System.Collections.Generic.KeyValuePair<int, RoleTypes> keyValuePair in SkidMenuPlugin.forcedRoles)
						{
							int playerId = keyValuePair.Key;
							RoleTypes value = keyValuePair.Value;
							PlayerControl playerControl = il2CppArrayBase.FirstOrDefault((PlayerControl p) => p != null && (int)p.PlayerId == playerId);
							bool flag2 = playerControl == null;
							if (!flag2)
							{
								playerControl.RpcSetRole(value, false);
								list.Add(playerControl.PlayerId);
								SkidMenuPlugin.Logger.LogInfo("? Forced " + playerControl.Data.PlayerName + " to " + value.ToString());
							}
						}
						SkidMenuPlugin.ForceRolePatch.AssignRemainingRoles(il2CppArrayBase, list, currentGameOptions);
						SkidMenuPlugin.Logger.LogInfo("=== CUSTOM ROLE ASSIGNMENT COMPLETE ===");
						result = false;
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Force role error: " + ex.Message);
						result = true;
					}
				}
				return result;
			}

			// Token: 0x06000078 RID: 120 RVA: 0x0000AF5C File Offset: 0x0000915C
			private static void AssignRemainingRoles(PlayerControl[] allPlayers, System.Collections.Generic.List<byte> assignedPlayers, IGameOptions options)
			{
				try
				{
					int num = 0;
					using (System.Collections.Generic.List<byte>.Enumerator enumerator = assignedPlayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							byte playerId = enumerator.Current;
							PlayerControl playerControl = allPlayers.FirstOrDefault((PlayerControl p) => p != null && p.PlayerId == playerId);
							bool flag = playerControl != null && playerControl.Data != null && playerControl.Data.Role != null && playerControl.Data.Role.IsImpostor;
							if (flag)
							{
								num++;
							}
						}
					}
					int @int = options.GetInt(Int32OptionNames.NumImpostors);
					int num2 = Mathf.Max(0, @int - num);
					SkidMenuPlugin.Logger.LogInfo(string.Concat(new string[]
					{
						"Assigning ",
						num2.ToString(),
						" more impostors (total: ",
						@int.ToString(),
						")"
					}));
					System.Collections.Generic.List<PlayerControl> list = (from p in allPlayers
					where p != null && !assignedPlayers.Contains(p.PlayerId)
					select p).ToList<PlayerControl>();
					for (int i = list.Count - 1; i > 0; i--)
					{
						int index = UnityEngine.Random.Range(0, i + 1);
						PlayerControl value = list[i];
						list[i] = list[index];
						list[index] = value;
					}
					int num3 = 0;
					while (num3 < num2 && num3 < list.Count)
					{
						PlayerControl playerControl2 = list[num3];
						playerControl2.RpcSetRole(RoleTypes.Impostor, false);
						assignedPlayers.Add(playerControl2.PlayerId);
						SkidMenuPlugin.Logger.LogInfo("? Auto-assigned " + playerControl2.Data.PlayerName + " to Impostor");
						num3++;
					}
					foreach (PlayerControl playerControl3 in allPlayers)
					{
						bool flag2 = playerControl3 != null && !assignedPlayers.Contains(playerControl3.PlayerId);
						if (flag2)
						{
							playerControl3.RpcSetRole(RoleTypes.Crewmate, false);
							assignedPlayers.Add(playerControl3.PlayerId);
							SkidMenuPlugin.Logger.LogInfo("? Auto-assigned " + playerControl3.Data.PlayerName + " to Crewmate");
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("AssignRemainingRoles error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000034 RID: 52
		[HarmonyPatch(typeof(VoteBanSystem))]
		public static class VotekickInfoPatch
		{
			// Token: 0x06000079 RID: 121 RVA: 0x0000B23C File Offset: 0x0000943C
			[HarmonyPatch("AddVote")]
			[HarmonyPostfix]
			public static void Postfix(int srcClient, int clientId)
			{
				try
				{
					bool flag = !SkidMenuPlugin.ShowVotekickInfo || AmongUsClient.Instance == null;
					if (!flag)
					{
						ClientData client = AmongUsClient.Instance.GetClient(srcClient);
						string text = ((client != null) ? client.PlayerName : null) ?? ("Client " + srcClient.ToString());
						ClientData client2 = AmongUsClient.Instance.GetClient(clientId);
						string text2 = ((client2 != null) ? client2.PlayerName : null) ?? ("Client " + clientId.ToString());
						string chatText = string.Concat(new string[]
						{
							"<color=orange>[Votekick Info]</color> <color=blue>",
							text,
							"</color> voted to kick <color=red>",
							text2,
							"</color>"
						});
						bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null && PlayerControl.LocalPlayer != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, chatText, true);
						}
						SkidMenuPlugin.Logger.LogInfo("[Votekick Info] " + text + " voted to kick " + text2);
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Votekick Info error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000035 RID: 53
		[HarmonyPatch(typeof(AmongUsClient), "OnGameJoined")]
		public static class AutoCopyCode_Patch
		{
			// Token: 0x0600007A RID: 122 RVA: 0x0000B3A0 File Offset: 0x000095A0
			public static void Postfix(string gameIdString)
			{
				SkidMenuPlugin.notifiedBlacklistedPlayers.Clear();
				SkidMenuPlugin.BanBlacklistedPatch.Reset();
				SkidMenuPlugin.NetworkLevel_AntiOverloadPatch.Reset();
				SkidMenuPlugin.detectedModUsers.Clear();
				try
				{
					bool flag = !SkidMenuPlugin.AutoCopyCodeEnabled;
					if (!flag)
					{
						bool flag2 = string.IsNullOrEmpty(gameIdString);
						if (!flag2)
						{
							GUIUtility.systemCopyBuffer = gameIdString;
							SkidMenuPlugin.Logger.LogInfo("Auto-copied lobby code: " + gameIdString);
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null && PlayerControl.LocalPlayer != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=yellow>\ud83d\udcbe Lobby code copied to clipboard!</color>", true);
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Auto Copy Code error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000036 RID: 54
		[HarmonyPatch(typeof(SabotageSystemType), "UpdateSystem")]
		public static class DisableSabotagesPatch
		{
			// Token: 0x0600007B RID: 123 RVA: 0x0000B490 File Offset: 0x00009690
			[HarmonyPrefix]
			public static bool Prefix()
			{
				return !SkidMenuPlugin.DisableSabotagesEnabled || !AmongUsClient.Instance.AmHost;
			}
		}

		// Token: 0x02000037 RID: 55
		[HarmonyPatch(typeof(ShipStatus), "CloseDoorsOfType")]
		public static class DisableDoorSabotagesPatch
		{
			// Token: 0x0600007C RID: 124 RVA: 0x0000B4C4 File Offset: 0x000096C4
			[HarmonyPrefix]
			public static bool Prefix(ShipStatus __instance, SystemTypes room)
			{
				bool flag = !SkidMenuPlugin.DisableSabotagesEnabled || !AmongUsClient.Instance.AmHost;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					SkidMenuPlugin.Logger.LogInfo("Blocked door sabotage attempt for room: " + room.ToString());
					result = false;
				}
				return result;
			}
		}

		// Token: 0x02000038 RID: 56
		[HarmonyPatch(typeof(SabotageButton), "DoClick")]
		public static class DisableSabotageButtonPatch
		{
			// Token: 0x0600007D RID: 125 RVA: 0x0000B518 File Offset: 0x00009718
			[HarmonyPrefix]
			public static bool Prefix()
			{
				bool flag = !SkidMenuPlugin.DisableSabotagesEnabled || !AmongUsClient.Instance.AmHost;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					SkidMenuPlugin.Logger.LogInfo("Blocked manual sabotage button click");
					bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
					if (flag2)
					{
						DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Sabotages are disabled by host!");
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x02000039 RID: 57
		[HarmonyPatch(typeof(PlayerControl))]
		public static class KillOtherImpostersPatch
		{
			// Token: 0x0600007E RID: 126 RVA: 0x0000B598 File Offset: 0x00009798
			[HarmonyPatch("FixedUpdate")]
			[HarmonyPostfix]
			public static void FixedUpdate_Postfix(PlayerControl __instance)
			{
				bool flag = !SkidMenuPlugin.KillOtherImpostersEnabled;
				if (!flag)
				{
					bool flag2 = __instance != PlayerControl.LocalPlayer;
					if (!flag2)
					{
						bool flag3 = !__instance.Data.Role.IsImpostor;
						if (!flag3)
						{
							bool flag4 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.KillButton != null;
							if (flag4)
							{
								KillButton killButton = DestroyableSingleton<HudManager>.Instance.KillButton;
								PlayerControl playerControl = null;
								float num = float.MaxValue;
								foreach (PlayerControl playerControl2 in PlayerControl.AllPlayerControls)
								{
									bool flag5 = playerControl2 == null || playerControl2 == __instance || playerControl2.Data.IsDead || playerControl2.inVent;
									if (!flag5)
									{
										float num2 = Vector2.Distance(__instance.GetTruePosition(), playerControl2.GetTruePosition());
										bool flag6 = num2 < num;
										if (flag6)
										{
											num = num2;
											playerControl = playerControl2;
										}
									}
								}
								bool flag7 = playerControl != null && playerControl.Data.Role.IsImpostor;
								if (flag7)
								{
									killButton.SetTarget(playerControl);
								}
							}
						}
					}
				}
			}

			// Token: 0x0600007F RID: 127 RVA: 0x0000B6D8 File Offset: 0x000098D8
			[HarmonyPatch("MurderPlayer")]
			[HarmonyPrefix]
			public static bool MurderPlayer_Prefix(PlayerControl __instance, PlayerControl target)
			{
				return true;
			}
		}

		// Token: 0x0200003A RID: 58
		[HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
		public static class AnticheatPlayerControlPatch
		{
			// Token: 0x06000080 RID: 128 RVA: 0x0000B6EC File Offset: 0x000098EC
			[HarmonyPrefix]
			public static void Prefix(PlayerControl __instance, byte callId, MessageReader reader)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || __instance.AmOwner;
				if (!flag)
				{
					int position = reader.Position;
					try
					{
						if (callId <= 1)
						{
							if (callId != 0)
							{
								if (callId == 1)
								{
									uint taskIndex = reader.ReadPackedUInt32();
									SkidMenuPlugin.AnticheatSystem.CheckCompleteTask(__instance, taskIndex);
								}
							}
							else
							{
								byte taskType = reader.ReadByte();
								SkidMenuPlugin.AnticheatSystem.CheckPlayAnimation(__instance, taskType);
							}
						}
						else if (callId != 15)
						{
							if (callId != 18)
							{
								if (callId == 38)
								{
									uint level = reader.ReadPackedUInt32();
									SkidMenuPlugin.AnticheatSystem.CheckSetLevel(__instance, level);
								}
							}
							else
							{
								reader.ReadPackedInt32();
								sbyte counter = reader.ReadSByte();
								SkidMenuPlugin.AnticheatSystem.CheckStartCounter(__instance, (int)counter);
							}
						}
						else
						{
							bool scanning = reader.ReadBoolean();
							reader.ReadByte();
							SkidMenuPlugin.AnticheatSystem.CheckScanner(__instance, scanning);
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Anticheat error: " + ex.Message);
					}
					finally
					{
						reader.Position = position;
					}
				}
			}
		}

		// Token: 0x0200003B RID: 59
		[HarmonyPatch(typeof(PlayerPhysics), "HandleRpc")]
		public static class AnticheatPlayerPhysicsPatch
		{
			// Token: 0x06000081 RID: 129 RVA: 0x0000B808 File Offset: 0x00009A08
			[HarmonyPrefix]
			public static void Prefix(PlayerPhysics __instance, byte callId)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || __instance.myPlayer.AmOwner;
				if (!flag)
				{
					if (callId - 19 <= 1)
					{
						SkidMenuPlugin.AnticheatSystem.CheckVent(__instance.myPlayer);
					}
				}
			}
		}

		// Token: 0x0200003C RID: 60
		[HarmonyPatch(typeof(CustomNetworkTransform), "HandleRpc")]
		public static class AnticheatNetTransformPatch
		{
			// Token: 0x06000082 RID: 130 RVA: 0x0000B84C File Offset: 0x00009A4C
			[HarmonyPrefix]
			public static void Prefix(CustomNetworkTransform __instance, byte callId)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || __instance.myPlayer.AmOwner;
				if (!flag)
				{
					bool flag2 = callId == 21;
					if (flag2)
					{
						SkidMenuPlugin.AnticheatSystem.CheckSnapTo(__instance.myPlayer);
					}
				}
			}
		}

		// Token: 0x0200003D RID: 61
		[HarmonyPatch(typeof(PlayerControl), "MurderPlayer")]
		public static class GodMode_MurderPatch
		{
			// Token: 0x06000083 RID: 131 RVA: 0x0000B88C File Offset: 0x00009A8C
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance, PlayerControl target)
			{
				try
				{
					bool flag = SkidMenuPlugin.GodModeEnabled && target != null && target.AmOwner && AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? God Mode blocked kill attempt");
						return false;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("God Mode Murder patch error: " + ex.Message);
				}
				return true;
			}
		}

		// Token: 0x0200003E RID: 62
		[HarmonyPatch(typeof(MeetingHud), "RpcVotingComplete")]
		public static class GodMode_RpcVotingCompletePatch
		{
			// Token: 0x06000084 RID: 132 RVA: 0x0000B920 File Offset: 0x00009B20
			[HarmonyPrefix]
			public static void Prefix(ref NetworkedPlayerInfo exiled, ref bool tie)
			{
				try
				{
					bool flag = SkidMenuPlugin.GodModeEnabled && PlayerControl.LocalPlayer != null && AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost && exiled != null && exiled.PlayerId == PlayerControl.LocalPlayer.PlayerId;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? God Mode: RpcVotingComplete - Nullifying exile");
						exiled = null;
						tie = false;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("God Mode RpcVotingComplete error: " + ex.Message);
				}
			}
		}

		// Token: 0x0200003F RID: 63
		[HarmonyPatch(typeof(MeetingHud), "VotingComplete")]
		public static class GodMode_VotingCompletePatch
		{
			// Token: 0x06000085 RID: 133 RVA: 0x0000B9CC File Offset: 0x00009BCC
			[HarmonyPrefix]
			public static void Prefix(ref NetworkedPlayerInfo exiled, ref bool tie)
			{
				try
				{
					bool flag = SkidMenuPlugin.GodModeEnabled && PlayerControl.LocalPlayer != null && exiled != null && exiled.PlayerId == PlayerControl.LocalPlayer.PlayerId;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? God Mode: VotingComplete - Nullifying exile");
						exiled = null;
						tie = false;
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("God Mode VotingComplete error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000040 RID: 64
		[HarmonyPatch(typeof(PlayerControl), "RpcSetRole")]
		public static class GodMode_RolePatch
		{
			// Token: 0x06000086 RID: 134 RVA: 0x0000BA60 File Offset: 0x00009C60
			[HarmonyPrefix]
			public static bool Prefix(PlayerControl __instance, RoleTypes roleType)
			{
				try
				{
					bool flag = SkidMenuPlugin.GodModeEnabled && __instance != null && __instance.AmOwner && AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;
					if (flag)
					{
						bool flag2 = roleType == RoleTypes.CrewmateGhost || roleType == RoleTypes.ImpostorGhost || roleType == RoleTypes.GuardianAngel;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? God Mode blocked ghost role conversion");
							return false;
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("God Mode Role patch error: " + ex.Message);
				}
				return true;
			}
		}

		// Token: 0x02000041 RID: 65
		[HarmonyPatch(typeof(PlayerControl), "FixedUpdate")]
		public static class GodMode_ContinuousProtectPatch
		{
			// Token: 0x06000087 RID: 135 RVA: 0x0000BB08 File Offset: 0x00009D08
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance)
			{
				try
				{
					bool flag = __instance != PlayerControl.LocalPlayer;
					if (!flag)
					{
						bool flag2 = !SkidMenuPlugin.GodModeEnabled || AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (!flag2)
						{
							bool flag3 = SkidMenuPlugin.GodMode_ContinuousProtectPatch.protectCooldown > 0f;
							if (flag3)
							{
								SkidMenuPlugin.GodMode_ContinuousProtectPatch.protectCooldown -= Time.fixedDeltaTime;
							}
							else
							{
								PlayerControl localPlayer = PlayerControl.LocalPlayer;
								NetworkedPlayerInfo networkedPlayerInfo = (localPlayer != null) ? localPlayer.Data : null;
								bool flag4 = networkedPlayerInfo != null && !networkedPlayerInfo.IsDead;
								if (flag4)
								{
									bool flag5 = localPlayer.protectedByGuardianId < 0;
									if (flag5)
									{
										NetworkedPlayerInfo.PlayerOutfit defaultOutfit = networkedPlayerInfo.DefaultOutfit;
										bool flag6 = defaultOutfit != null;
										if (flag6)
										{
											localPlayer.RpcProtectPlayer(localPlayer, defaultOutfit.ColorId);
											SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? God Mode: Applied Guardian Angel protection");
											SkidMenuPlugin.GodMode_ContinuousProtectPatch.protectCooldown = 5f;
										}
									}
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("God Mode continuous protect error: " + ex.Message);
				}
			}

			// Token: 0x04000112 RID: 274
			private static float protectCooldown;
		}

		// Token: 0x02000042 RID: 66
		[HarmonyPatch(typeof(ExileController), "ReEnableGameplay")]
		public static class GodMode_AfterMeetingProtectPatch
		{
			// Token: 0x06000088 RID: 136 RVA: 0x0000BC34 File Offset: 0x00009E34
			[HarmonyPostfix]
			public static void Postfix()
			{
				try
				{
					bool flag = SkidMenuPlugin.GodModeEnabled && PlayerControl.LocalPlayer != null && AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;
					if (flag)
					{
						PlayerControl localPlayer = PlayerControl.LocalPlayer;
						NetworkedPlayerInfo data = localPlayer.Data;
						NetworkedPlayerInfo.PlayerOutfit playerOutfit = (data != null) ? data.DefaultOutfit : null;
						bool flag2 = playerOutfit != null;
						if (flag2)
						{
							localPlayer.RpcProtectPlayer(localPlayer, playerOutfit.ColorId);
							SkidMenuPlugin.Logger.LogInfo("\ud83d\udee1? God Mode: Reapplied protection after meeting");
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("God Mode after meeting protect error: " + ex.Message);
				}
			}
		}

		// Token: 0x02000043 RID: 67
		[HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
		public static class ModUserDetectionPatch
		{
			// Token: 0x06000089 RID: 137 RVA: 0x0000BCF0 File Offset: 0x00009EF0
			[HarmonyPostfix]
			public static void Postfix(PlayerControl __instance, byte callId, MessageReader reader)
			{
				bool flag = !SkidMenuPlugin.SeeModUsersEnabled;
				if (!flag)
				{
					try
					{
						bool flag2 = SkidMenuPlugin.ModUserDetectionPatch.knownModRPCs.Contains(callId);
						if (flag2)
						{
							byte playerId = __instance.PlayerId;
							float time = Time.time;
							bool flag3 = SkidMenuPlugin.lastDetectionTime.ContainsKey(playerId);
							if (flag3)
							{
								bool flag4 = time - SkidMenuPlugin.lastDetectionTime[playerId] < 2f;
								if (flag4)
								{
									return;
								}
							}
							SkidMenuPlugin.detectedModUsers[playerId] = callId;
							SkidMenuPlugin.lastDetectionTime[playerId] = time;
							string menuNameFromRPC = SkidMenuPlugin.ModUserDetectionPatch.GetMenuNameFromRPC(callId);
							NetworkedPlayerInfo data = __instance.Data;
							string text = ((data != null) ? data.PlayerName : null) ?? "Unknown";
							SkidMenuPlugin.Logger.LogWarning(string.Concat(new string[]
							{
								"[Mod Detection] ",
								text,
								" is using ",
								menuNameFromRPC,
								" (RPC ",
								callId.ToString(),
								")"
							}));
							bool flag5 = !__instance.AmOwner && DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
							if (flag5)
							{
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Concat(new string[]
								{
									"<color=orange>[Mod Detection]</color> <color=yellow>",
									text,
									"</color> is using <color=red>",
									menuNameFromRPC,
									"</color>"
								}), true);
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("ModUserDetection error: " + ex.Message);
					}
				}
			}

			// Token: 0x0600008A RID: 138 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
			private static string GetMenuNameFromRPC(byte rpcId)
			{
				bool flag = rpcId == 121;
				string result;
				if (flag)
				{
					result = "SkidMenu";
				}
				else
				{
					bool flag2 = rpcId == 167;
					if (flag2)
					{
						result = "TuffMenu";
					}
					else
					{
						bool flag3 = rpcId == 164;
						if (flag3)
						{
							result = "SickoMenu";
						}
						else
						{
							bool flag4 = rpcId == 85;
							if (flag4)
							{
								result = "AmongUsMenu";
							}
							else
							{
								bool flag5 = rpcId == 150;
								if (flag5)
								{
									result = "BetterAmongUs";
								}
								else
								{
									bool flag6 = rpcId == 250;
									if (flag6)
									{
										result = "KillNetwork";
									}
									else
									{
										bool flag7 = rpcId == 176;
										if (flag7)
										{
											result = "HostGuard";
										}
										else
										{
											bool flag8 = rpcId == 154;
											if (flag8)
											{
												result = "GoatNetClient";
											}
											else
											{
												bool flag9 = rpcId == 162;
												if (flag9)
												{
													result = "NetMenu";
												}
												else
												{
													result = "Unknown Mod";
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return result;
			}

			// Token: 0x04000113 RID: 275
			private static readonly HashSet<byte> knownModRPCs = new HashSet<byte>
			{
				121,
				167,
				164,
				85,
				150,
				250,
				176,
				154,
				162
			};
		}

		// Token: 0x02000044 RID: 68
		public static class AnticheatSystem
		{
			// Token: 0x0600008C RID: 140 RVA: 0x0000C000 File Offset: 0x0000A200
			public static void LogDetection(string playerName, string reason)
			{
				string text = string.Concat(new string[]
				{
					"[",
					System.DateTime.Now.ToString("HH:mm:ss"),
					"] ",
					playerName,
					": ",
					reason
				});
				SkidMenuPlugin.detectionLog.Add(text);
				SkidMenuPlugin.totalDetections++;
				SkidMenuPlugin.Logger.LogWarning("[ANTICHEAT] " + text);
				bool flag = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
				if (flag)
				{
					DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[ANTICHEAT]</color> " + playerName + ": " + reason, true);
				}
			}

			// Token: 0x0600008D RID: 141 RVA: 0x0000C0CC File Offset: 0x0000A2CC
			public static void PunishPlayer(PlayerControl player, string reason)
			{
				bool flag = !SkidMenuPlugin.AutoBanEnabled || !AmongUsClient.Instance.AmHost;
				if (!flag)
				{
					SkidMenuPlugin.AnticheatSystem.LogDetection(player.Data.PlayerName, "BANNED - " + reason);
					AmongUsClient.Instance.KickPlayer(player.OwnerId, true);
					SkidMenuPlugin.Logger.LogError("[ANTICHEAT] Banned " + player.Data.PlayerName + " for: " + reason);
				}
			}

			// Token: 0x0600008E RID: 142 RVA: 0x0000C14C File Offset: 0x0000A34C
			public static void CheckCompleteTask(PlayerControl player, uint taskIndex)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckInvalidCompleteTask;
				if (!flag)
				{
					bool flag2 = ShipStatus.Instance == null;
					if (flag2)
					{
						SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Tried completing task {0} in lobby", taskIndex));
					}
					else
					{
						bool isImpostor = player.Data.Role.IsImpostor;
						if (isImpostor)
						{
							SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Imposter tried completing task {0}", taskIndex));
						}
						else
						{
							bool flag3 = (ulong)(taskIndex + 1U) > (ulong)((long)player.Data.Tasks.Count);
							if (flag3)
							{
								SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Tried completing invalid task index {0}", taskIndex));
							}
						}
					}
				}
			}

			// Token: 0x0600008F RID: 143 RVA: 0x0000C204 File Offset: 0x0000A404
			public static void CheckPlayAnimation(PlayerControl player, byte taskType)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckInvalidPlayAnimation;
				if (!flag)
				{
					bool flag2 = LobbyBehaviour.Instance != null;
					if (flag2)
					{
						SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Tried playing animation {0} in lobby", taskType));
					}
					else
					{
						bool isImpostor = player.Data.Role.IsImpostor;
						if (isImpostor)
						{
							SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Imposter tried playing animation {0}", taskType));
						}
						else
						{
							bool flag3 = GameManager.Instance != null && !GameManager.Instance.LogicOptions.GetVisualTasks();
							if (flag3)
							{
								SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Played animation {0} with visuals off", taskType));
							}
						}
					}
				}
			}

			// Token: 0x06000090 RID: 144 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
			public static void CheckScanner(PlayerControl player, bool scanning)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckInvalidScanner;
				if (!flag)
				{
					bool flag2 = !scanning;
					if (!flag2)
					{
						bool flag3 = ShipStatus.Instance == null;
						if (flag3)
						{
							SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, "Tried scanning before map spawned");
						}
						else
						{
							bool isImpostor = player.Data.Role.IsImpostor;
							if (isImpostor)
							{
								SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, "Imposter tried scanning");
							}
							else
							{
								bool flag4 = GameManager.Instance != null && !GameManager.Instance.LogicOptions.GetVisualTasks();
								if (flag4)
								{
									SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, "Tried scanning with visuals off");
								}
							}
						}
					}
				}
			}

			// Token: 0x06000091 RID: 145 RVA: 0x0000C370 File Offset: 0x0000A570
			public static void CheckVent(PlayerControl player)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckInvalidVent;
				if (!flag)
				{
					bool flag2 = ShipStatus.Instance == null;
					if (flag2)
					{
						SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, "Tried venting before map spawned");
					}
					else
					{
						bool flag3 = !player.Data.Role.CanVent && !player.Data.IsDead;
						if (flag3)
						{
							SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Role {0} tried venting", player.Data.Role.Role));
						}
					}
				}
			}

			// Token: 0x06000092 RID: 146 RVA: 0x0000C404 File Offset: 0x0000A604
			public static void CheckSnapTo(PlayerControl player)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckInvalidSnapTo;
				if (!flag)
				{
					bool flag2 = LobbyBehaviour.Instance != null && AmongUsClient.Instance.AmHost;
					if (flag2)
					{
						SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, "Tried teleporting in lobby");
					}
				}
			}

			// Token: 0x06000093 RID: 147 RVA: 0x0000C458 File Offset: 0x0000A658
			public static void CheckStartCounter(PlayerControl player, int counter)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckInvalidStartCounter;
				if (!flag)
				{
					bool flag2 = player.OwnerId == AmongUsClient.Instance.HostId;
					if (!flag2)
					{
						bool flag3 = counter != -1;
						if (flag3)
						{
							SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Non-host tried setting counter to {0}", counter));
							bool flag4 = AmongUsClient.Instance.AmHost && PlayerControl.LocalPlayer != null;
							if (flag4)
							{
								PlayerControl.LocalPlayer.RpcSetStartCounter(-1);
							}
						}
					}
				}
			}

			// Token: 0x06000094 RID: 148 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
			public static void CheckSetLevel(PlayerControl player, uint level)
			{
				bool flag = !SkidMenuPlugin.AnticheatEnabled || !SkidMenuPlugin.CheckSpoofedLevels;
				if (!flag)
				{
					bool flag2 = level > 10000U;
					if (flag2)
					{
						SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, string.Format("Spoofed level too high ({0})", level));
					}
					else
					{
						bool flag3 = ShipStatus.Instance != null;
						if (flag3)
						{
							SkidMenuPlugin.AnticheatSystem.PunishPlayer(player, "Tried changing level mid-game");
						}
					}
				}
			}
		}

		// Token: 0x02000045 RID: 69
		public class SkidMenu : MonoBehaviour
		{
			// Token: 0x06000095 RID: 149 RVA: 0x0000C554 File Offset: 0x0000A754
			public SkidMenu(System.IntPtr ptr) : base(ptr)
			{
			}

			// Token: 0x06000096 RID: 150 RVA: 0x0000C844 File Offset: 0x0000AA44
			private void Update()
			{
				this._playerCacheTimer += Time.deltaTime;
				bool flag = this._playerCacheTimer >= 0.5f;
				if (flag)
				{
					this._playerCacheTimer = 0f;
					try
					{
						bool flag2 = PlayerControl.AllPlayerControls != null;
						if (flag2)
						{
							this._cachedPlayers = PlayerControl.AllPlayerControls.ToArray();
						}
					}
					catch
					{
					}
					bool flag3 = SkidMenuPlugin.ShowPlayerInfo || SkidMenuPlugin.SeeRolesEnabled || SkidMenuPlugin.BlacklistedCodes.Count > 0;
					if (flag3)
					{
						SkidMenuPlugin.PlayerNametagsPatch.RefreshAll();
					}
					else
					{
						SkidMenuPlugin.PlayerNametagsPatch.RestoreAll();
					}
					SkidMenuPlugin.TrackOwnModUsage();
				}
				bool flag4 = this.tabTransitionProgress < 1f;
				if (flag4)
				{
					this.tabTransitionProgress += Time.deltaTime * 8f;
					bool flag5 = this.tabTransitionProgress >= 1f;
					if (flag5)
					{
						this.tabTransitionProgress = 1f;
						SkidMenuPlugin.ActiveTab = this.targetTab;
					}
					float t = 1f - Mathf.Pow(1f - this.tabTransitionProgress, 3f);
					this.contentOffset = Vector2.Lerp(new Vector2(50f, 0f), Vector2.zero, t);
				}
				else
				{
					this.contentOffset = Vector2.zero;
				}
				bool isChangingKey = SkidMenuPlugin.isChangingKey;
				if (isChangingKey)
				{
					foreach (object obj in System.Enum.GetValues(typeof(KeyCode)))
					{
						KeyCode keyCode = (KeyCode)obj;
						bool keyDown = Input.GetKeyDown(keyCode);
						if (keyDown)
						{
							SkidMenuPlugin.MenuKey.Value = keyCode;
							SkidMenuPlugin.isChangingKey = false;
							break;
						}
					}
				}
				else
				{
					bool keyDown2 = Input.GetKeyDown(SkidMenuPlugin.MenuKey.Value);
					if (keyDown2)
					{
						SkidMenuPlugin.ShowMenu = !SkidMenuPlugin.ShowMenu;
					}
				}
				bool flag6 = SkidMenuPlugin.ShowMenu && SkidMenuPlugin.ActiveTab == "Chat" && this.chatBoxFocused;
				if (flag6)
				{
					this.cursorBlinkTime += Time.deltaTime;
					bool flag7 = this.cursorBlinkTime >= 0.5f;
					if (flag7)
					{
						this.showCursor = !this.showCursor;
						this.cursorBlinkTime = 0f;
					}
					bool flag8 = this.cursorPosition > this.chatMessage.Length;
					if (flag8)
					{
						this.cursorPosition = this.chatMessage.Length;
					}
					bool flag9 = this.cursorPosition < 0;
					if (flag9)
					{
						this.cursorPosition = 0;
					}
					bool keyDown3 = Input.GetKeyDown(KeyCode.LeftArrow);
					if (keyDown3)
					{
						bool flag10 = this.cursorPosition > 0;
						if (flag10)
						{
							this.cursorPosition--;
						}
					}
					bool keyDown4 = Input.GetKeyDown(KeyCode.RightArrow);
					if (keyDown4)
					{
						bool flag11 = this.cursorPosition < this.chatMessage.Length;
						if (flag11)
						{
							this.cursorPosition++;
						}
					}
					bool keyDown5 = Input.GetKeyDown(KeyCode.Home);
					if (keyDown5)
					{
						this.cursorPosition = 0;
					}
					bool keyDown6 = Input.GetKeyDown(KeyCode.End);
					if (keyDown6)
					{
						this.cursorPosition = this.chatMessage.Length;
					}
					bool flag12 = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
					if (flag12)
					{
						bool keyDown7 = Input.GetKeyDown(KeyCode.C);
						if (keyDown7)
						{
							bool flag13 = !string.IsNullOrEmpty(this.chatMessage);
							if (flag13)
							{
								GUIUtility.systemCopyBuffer = this.chatMessage;
								SkidMenuPlugin.Logger.LogInfo("Copied to clipboard: " + this.chatMessage);
							}
						}
						bool keyDown8 = Input.GetKeyDown(KeyCode.V);
						if (keyDown8)
						{
							string systemCopyBuffer = GUIUtility.systemCopyBuffer;
							bool flag14 = !string.IsNullOrEmpty(systemCopyBuffer);
							if (flag14)
							{
								bool flag15 = this.chatMessage.Length + systemCopyBuffer.Length <= 100;
								if (flag15)
								{
									this.chatMessage = this.chatMessage.Insert(this.cursorPosition, systemCopyBuffer);
									this.cursorPosition += systemCopyBuffer.Length;
									SkidMenuPlugin.Logger.LogInfo("Pasted from clipboard");
								}
							}
						}
						bool keyDown9 = Input.GetKeyDown(KeyCode.A);
						if (keyDown9)
						{
							this.cursorPosition = this.chatMessage.Length;
						}
					}
					else
					{
						bool flag16 = Input.GetKey(KeyCode.Backspace) && this.cursorPosition > 0;
						if (flag16)
						{
							this.backspaceHoldTime += Time.deltaTime;
							bool keyDown10 = Input.GetKeyDown(KeyCode.Backspace);
							if (keyDown10)
							{
								this.chatMessage = this.chatMessage.Remove(this.cursorPosition - 1, 1);
								this.cursorPosition--;
								this.backspaceHoldTime = 0f;
								this.backspaceRepeatDelay = 0.5f;
							}
							else
							{
								bool flag17 = this.backspaceHoldTime >= this.backspaceRepeatDelay;
								if (flag17)
								{
									this.chatMessage = this.chatMessage.Remove(this.cursorPosition - 1, 1);
									this.cursorPosition--;
									this.backspaceHoldTime = 0f;
									this.backspaceRepeatDelay = 0.05f;
								}
							}
						}
						else
						{
							this.backspaceHoldTime = 0f;
							this.backspaceRepeatDelay = 0f;
						}
						bool flag18 = Input.GetKeyDown(KeyCode.Delete) && this.cursorPosition < this.chatMessage.Length;
						if (flag18)
						{
							this.chatMessage = this.chatMessage.Remove(this.cursorPosition, 1);
						}
						bool flag19 = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Return);
						if (flag19)
						{
							float num = Time.time - this.lastChatSendTime;
							bool flag20 = num >= 3f && !string.IsNullOrWhiteSpace(this.chatMessage);
							if (flag20)
							{
								this.SendChatMessage(this.chatMessage.Trim());
							}
						}
						else
						{
							bool keyDown11 = Input.GetKeyDown(KeyCode.Return);
							if (keyDown11)
							{
								bool flag21 = this.chatMessage.Length < 100;
								if (flag21)
								{
									this.chatMessage = this.chatMessage.Insert(this.cursorPosition, "\n");
									this.cursorPosition++;
								}
							}
						}
						bool flag22 = this.chatMessage.Length < 100;
						if (flag22)
						{
							string inputString = Input.inputString;
							bool flag23 = !string.IsNullOrEmpty(inputString);
							if (flag23)
							{
								foreach (char c in inputString)
								{
									bool flag24 = c >= ' ' && c <= '~' && this.chatMessage.Length < 100;
									if (flag24)
									{
										this.chatMessage = this.chatMessage.Insert(this.cursorPosition, c.ToString());
										this.cursorPosition++;
									}
								}
							}
						}
					}
				}
				bool spamChatEnabled = SkidMenuPlugin.SkidMenu.SpamChatEnabled;
				if (spamChatEnabled)
				{
					this.ExecuteSpamChat();
				}
				bool votekickAllEnabled = SkidMenuPlugin.VotekickAllEnabled;
				if (votekickAllEnabled)
				{
					this.MonitorAndVotekickNewPlayers();
				}
				bool flag25 = !SkidMenuPlugin.ExtendedLobbyEnabled;
				if (flag25)
				{
					SkidMenuPlugin.ExtendedLobbyListPatch.Reset();
				}
				bool overloadEnabled = SkidMenuPlugin.OverloadEnabled;
				if (overloadEnabled)
				{
					this.ExecuteOverload();
				}
				bool overloadMethod2Enabled = SkidMenuPlugin.OverloadMethod2Enabled;
				if (overloadMethod2Enabled)
				{
					this.ExecuteOverloadMethod2();
				}
				bool overloadMethod3Enabled = SkidMenuPlugin.OverloadMethod3Enabled;
				if (overloadMethod3Enabled)
				{
					this.ExecuteOverloadMethod3();
				}
				bool overloadMethod4Enabled = SkidMenuPlugin.OverloadMethod4Enabled;
				if (overloadMethod4Enabled)
				{
					this.ExecuteOverloadMethod4();
				}
				bool lagEveryoneEnabled = SkidMenuPlugin.LagEveryoneEnabled;
				if (lagEveryoneEnabled)
				{
					this.ExecuteLagEveryone();
				}
				bool flag26 = SkidMenuPlugin.TargetedOverloadEnabled && SkidMenuPlugin.selectedTargetId != -1;
				if (flag26)
				{
					this.ExecuteTargetedOverload();
				}
				bool breakCounterEnabled = SkidMenuPlugin.BreakCounterEnabled;
				if (breakCounterEnabled)
				{
					this.ExecuteBreakCounter();
				}
				bool exileMeEnabled = SkidMenuPlugin.ExileMeEnabled;
				if (exileMeEnabled)
				{
					this.ExecuteExileMe();
				}
				bool spamRepairSabotages = SkidMenuPlugin.SpamRepairSabotages;
				if (spamRepairSabotages)
				{
					this.ExecuteRepairSabotages();
				}
				bool killAllEnabled = SkidMenuPlugin.KillAllEnabled;
				if (killAllEnabled)
				{
					this.ExecuteKillAll();
				}
				Time.timeScale = SkidMenuPlugin.GameSpeed;
				bool flag27 = SkidMenuPlugin.ShowMenu && SkidMenuPlugin.ActiveTab == "Anticheat" && this.blacklistInputFocused;
				if (flag27)
				{
					this.blacklistCursorBlink += Time.deltaTime;
					bool flag28 = this.blacklistCursorBlink >= 0.5f;
					if (flag28)
					{
						this.blacklistCursorVisible = !this.blacklistCursorVisible;
						this.blacklistCursorBlink = 0f;
					}
					this.blacklistCursorPos = Mathf.Clamp(this.blacklistCursorPos, 0, this.blacklistInput.Length);
					bool flag29 = Input.GetKeyDown(KeyCode.LeftArrow) && this.blacklistCursorPos > 0;
					if (flag29)
					{
						this.blacklistCursorPos--;
					}
					bool flag30 = Input.GetKeyDown(KeyCode.RightArrow) && this.blacklistCursorPos < this.blacklistInput.Length;
					if (flag30)
					{
						this.blacklistCursorPos++;
					}
					bool flag31 = Input.GetKeyDown(KeyCode.Backspace) && this.blacklistCursorPos > 0;
					if (flag31)
					{
						this.blacklistInput = this.blacklistInput.Remove(this.blacklistCursorPos - 1, 1);
						this.blacklistCursorPos--;
					}
					bool keyDown12 = Input.GetKeyDown(KeyCode.Return);
					if (keyDown12)
					{
						bool flag32 = !string.IsNullOrWhiteSpace(this.blacklistInput);
						if (flag32)
						{
							SkidMenuPlugin.SaveToBlacklist(this.blacklistInput.Trim());
							this.blacklistAddedMessage = "Added: " + this.blacklistInput.Trim();
							this.blacklistMessageTimer = 3f;
							this.blacklistInput = "";
							this.blacklistCursorPos = 0;
							this.blacklistInputFocused = false;
						}
					}
					bool keyDown13 = Input.GetKeyDown(KeyCode.Escape);
					if (keyDown13)
					{
						this.blacklistInputFocused = false;
					}
					bool flag33 = this.blacklistInput.Length < 20;
					if (flag33)
					{
						foreach (char c2 in Input.inputString)
						{
							bool flag34 = c2 >= ' ' && c2 <= '~' && this.blacklistInput.Length < 20;
							if (flag34)
							{
								this.blacklistInput = this.blacklistInput.Insert(this.blacklistCursorPos, c2.ToString());
								this.blacklistCursorPos++;
							}
						}
					}
				}
				bool flag35 = SkidMenuPlugin.VotekickAllEnabled != this._lastVotekickAllState;
				if (flag35)
				{
					bool votekickAllEnabled2 = SkidMenuPlugin.VotekickAllEnabled;
					if (votekickAllEnabled2)
					{
						HudManager instance = DestroyableSingleton<HudManager>.Instance;
						if (instance != null)
						{
							NotificationPopper notifier = instance.Notifier;
							if (notifier != null)
							{
								notifier.AddDisconnectMessage("Votekick sent to many players! Leave and rejoin 2 more times.");
							}
						}
					}
					this._lastVotekickAllState = SkidMenuPlugin.VotekickAllEnabled;
				}
				bool flag36 = SkidMenuPlugin.ShowMenu && SkidMenuPlugin.ActiveTab == "Host" && this.activeHostSection == "Settings" && !string.IsNullOrEmpty(this.focusedSettingKey) && this.settingsBoxFocused;
				if (flag36)
				{
					this.settingCursorBlink += Time.deltaTime;
					bool flag37 = this.settingCursorBlink >= 0.5f;
					if (flag37)
					{
						this.settingCursorVisible = !this.settingCursorVisible;
						this.settingCursorBlink = 0f;
					}
					bool keyDown14 = Input.GetKeyDown(KeyCode.Escape);
					if (keyDown14)
					{
						this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
						this.focusedSettingKey = "";
						this.settingInputBuffer = "";
					}
					bool flag38 = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
					if (flag38)
					{
						this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
						this.focusedSettingKey = "";
						this.settingInputBuffer = "";
					}
					bool flag39 = Input.GetKeyDown(KeyCode.Backspace) && this.settingInputBuffer.Length > 0;
					if (flag39)
					{
						this.settingInputBuffer = this.settingInputBuffer.Substring(0, this.settingInputBuffer.Length - 1);
					}
					foreach (char c3 in Input.inputString)
					{
						bool flag40 = ((c3 >= '0' && c3 <= '9') || c3 == '.' || c3 == '-') && this.settingInputBuffer.Length < 10;
						if (flag40)
						{
							this.settingInputBuffer += c3.ToString();
						}
					}
				}
				bool teleportToCursorEnabled = SkidMenuPlugin.TeleportToCursorEnabled;
				if (teleportToCursorEnabled)
				{
					bool mouseButtonDown = Input.GetMouseButtonDown(1);
					if (mouseButtonDown)
					{
						PlayerControl localPlayer = PlayerControl.LocalPlayer;
						if (localPlayer != null)
						{
							localPlayer.NetTransform.RpcSnapTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
						}
					}
				}
				bool animAsteroidsEnabled = SkidMenuPlugin.AnimAsteroidsEnabled;
				if (animAsteroidsEnabled)
				{
					try
					{
						bool flag41 = PlayerControl.LocalPlayer != null;
						if (flag41)
						{
							this.ForcePlayAnimation(6);
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Asteroids anim error: " + ex.Message);
					}
				}
				bool flag42 = SkidMenuPlugin.ShowMenu && this.searchBoxFocused;
				if (flag42)
				{
					this.searchCursorBlinkTime += Time.deltaTime;
					bool flag43 = this.searchCursorBlinkTime >= 0.5f;
					if (flag43)
					{
						this.searchCursorVisible = !this.searchCursorVisible;
						this.searchCursorBlinkTime = 0f;
					}
					bool flag44 = this.searchCursorPosition > this.searchQuery.Length;
					if (flag44)
					{
						this.searchCursorPosition = this.searchQuery.Length;
					}
					bool flag45 = this.searchCursorPosition < 0;
					if (flag45)
					{
						this.searchCursorPosition = 0;
					}
					bool flag46 = Input.GetKeyDown(KeyCode.LeftArrow) && this.searchCursorPosition > 0;
					if (flag46)
					{
						this.searchCursorPosition--;
					}
					bool flag47 = Input.GetKeyDown(KeyCode.RightArrow) && this.searchCursorPosition < this.searchQuery.Length;
					if (flag47)
					{
						this.searchCursorPosition++;
					}
					bool keyDown15 = Input.GetKeyDown(KeyCode.Home);
					if (keyDown15)
					{
						this.searchCursorPosition = 0;
					}
					bool keyDown16 = Input.GetKeyDown(KeyCode.End);
					if (keyDown16)
					{
						this.searchCursorPosition = this.searchQuery.Length;
					}
					bool flag48 = Input.GetKeyDown(KeyCode.Backspace) && this.searchCursorPosition > 0;
					if (flag48)
					{
						this.searchQuery = this.searchQuery.Remove(this.searchCursorPosition - 1, 1);
						this.searchCursorPosition--;
					}
					bool flag49 = Input.GetKeyDown(KeyCode.Delete) && this.searchCursorPosition < this.searchQuery.Length;
					if (flag49)
					{
						this.searchQuery = this.searchQuery.Remove(this.searchCursorPosition, 1);
					}
					bool keyDown17 = Input.GetKeyDown(KeyCode.Escape);
					if (keyDown17)
					{
						this.searchBoxFocused = false;
						this.searchQuery = "";
						this.searchResults.Clear();
					}
					bool flag50 = this.searchQuery.Length < 50;
					if (flag50)
					{
						string inputString4 = Input.inputString;
						bool flag51 = !string.IsNullOrEmpty(inputString4);
						if (flag51)
						{
							foreach (char c4 in inputString4)
							{
								bool flag52 = c4 >= ' ' && c4 <= '~' && this.searchQuery.Length < 50;
								if (flag52)
								{
									this.searchQuery = this.searchQuery.Insert(this.searchCursorPosition, c4.ToString());
									this.searchCursorPosition++;
								}
							}
						}
					}
				}
				bool flag53 = this.blacklistMessageTimer > 0f;
				if (flag53)
				{
					this.blacklistMessageTimer -= Time.deltaTime;
				}
			}

			// Token: 0x06000097 RID: 151 RVA: 0x0000D824 File Offset: 0x0000BA24
			private void FixedUpdate()
			{
				bool flag = PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.MyPhysics == null;
				if (!flag)
				{
					PlayerControl localPlayer = PlayerControl.LocalPlayer;
					Collider2D component = localPlayer.MyPhysics.GetComponent<Collider2D>();
					bool flag2 = component != null;
					if (flag2)
					{
						component.enabled = !SkidMenuPlugin.NoClipEnabled;
					}
					bool spinEnabled = SkidMenuPlugin.SpinEnabled;
					if (spinEnabled)
					{
						SkidMenuPlugin.spinAngle += 15f;
						localPlayer.transform.localRotation = Quaternion.Euler(0f, 0f, SkidMenuPlugin.spinAngle);
					}
					bool flag3 = SkidMenuPlugin.RandomizeOutfit && AmongUsClient.Instance.AmHost;
					if (flag3)
					{
						bool flag4 = Time.time > SkidMenuPlugin.nextRandomTime;
						if (flag4)
						{
							SkidMenuPlugin.nextRandomTime = Time.time + 0.1f;
							this.RandomizeEverything(localPlayer);
						}
					}
					SkidMenuPlugin.SendIdentificationRPC();
				}
			}

			// Token: 0x06000098 RID: 152 RVA: 0x0000D914 File Offset: 0x0000BB14
			private void OnGUI()
			{
				SkidMenuPlugin.colorLocked = false;
				bool flag = this._styleSettingsNormal == null;
				if (flag)
				{
					this._styleSettingsNormal = new GUIStyle(GUI.skin.label)
					{
						fontSize = 11,
						alignment = TextAnchor.MiddleLeft,
						normal = new GUIStyleState
						{
							textColor = Color.white
						}
					};
					this._styleSettingsFocused = new GUIStyle(GUI.skin.label)
					{
						fontSize = 11,
						alignment = TextAnchor.MiddleLeft,
						normal = new GUIStyleState
						{
							textColor = Color.yellow
						}
					};
				}
				bool showKillCooldown = SkidMenuPlugin.ShowKillCooldown;
				if (showKillCooldown)
				{
					this.DrawKillCooldownOverlay();
				}
				bool flag2 = !SkidMenuPlugin.ShowMenu;
				if (!flag2)
				{
					this.HandleResize();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					this.windowRect = GUI.Window(this.windowId, this.windowRect, new System.Action<int>(this.DrawWindowContents), "");
					this.DrawResizeHandle();
					bool flag3 = this.chatBoxFocused && Event.current.type == EventType.MouseDown;
					if (flag3)
					{
						bool flag4 = !this.windowRect.Contains(Event.current.mousePosition);
						if (flag4)
						{
							this.chatBoxFocused = false;
						}
					}
					bool flag5 = this.searchBoxFocused && Event.current.type == EventType.MouseDown;
					if (flag5)
					{
						Rect rect = new Rect(this.windowRect.x + 10f, this.windowRect.y + 45f, 250f, 230f);
						bool flag6 = !rect.Contains(Event.current.mousePosition);
						if (flag6)
						{
							this.searchBoxFocused = false;
							this.searchResults.Clear();
						}
					}
					bool flag7 = this.settingsBoxFocused && Event.current.type == EventType.MouseDown;
					if (flag7)
					{
						bool flag8 = !this.windowRect.Contains(Event.current.mousePosition);
						if (flag8)
						{
							this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
							this.focusedSettingKey = "";
							this.settingInputBuffer = "";
							this.settingsBoxFocused = false;
						}
					}
				}
			}

			// Token: 0x06000099 RID: 153 RVA: 0x0000DB54 File Offset: 0x0000BD54
			private void DrawWindowContents(int id)
			{
				GUILayout.BeginArea(new Rect(10f, 10f, this.windowRect.width - 20f, 30f));
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label("SkidMenu v1.0.7 _rc2", new GUIStyle(GUI.skin.label)
				{
					fontSize = 16,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUILayout.EndArea();
				this.DrawSearchBar();
				this.DrawSidebar();
				bool flag = !this.searchBoxFocused || string.IsNullOrEmpty(this.searchQuery);
				if (flag)
				{
					this.DrawMainContent();
				}
				else
				{
					this.DrawSearchResults();
				}
				GUILayout.BeginArea(new Rect(10f, this.windowRect.height - 30f, this.windowRect.width - 20f, 25f));
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : Color.gray);
				GUILayout.Label("Credits to Pro", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 10 }, null); GUILayout.Label("Press " + SkidMenuPlugin.MenuKey.Value.ToString() + " to hide", new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = Color.white;
				GUILayout.EndArea();
				this.resizeHandleRect = new Rect(this.windowRect.width - 15f, this.windowRect.height - 15f, 15f, 15f);
				bool flag2 = !this.isResizing && !this.resizeHandleRect.Contains(Event.current.mousePosition);
				if (flag2)
				{
					GUI.DragWindow();
				}
			}

			// Token: 0x0600009A RID: 154 RVA: 0x0000DD1C File Offset: 0x0000BF1C
			private void DrawSearchBar()
			{
				float x = 10f;
				float y = 45f;
				float width = 90f;
				float height = 30f;
				GUILayout.BeginArea(new Rect(x, y, width, height));
				GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
				Rect position = new Rect(0f, 0f, width, height);
				GUI.Box(position, "", GUI.skin.box);
				bool flag = GUI.Button(position, "", GUIStyle.none);
				if (flag)
				{
					this.searchBoxFocused = true;
					this.searchCursorVisible = true;
					this.searchCursorBlinkTime = 0f;
				}
				Rect position2 = new Rect(position.x + 5f, position.y + 6f, position.width - 10f, position.height - 12f);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					alignment = TextAnchor.MiddleLeft,
					normal = new GUIStyleState
					{
						textColor = Color.white
					}
				};
				string text = this.searchQuery;
				bool flag2 = this.searchBoxFocused && this.searchCursorVisible && this.searchCursorPosition >= 0 && this.searchCursorPosition <= this.searchQuery.Length;
				if (flag2)
				{
					text = this.searchQuery.Insert(this.searchCursorPosition, "|");
				}
				bool flag3 = string.IsNullOrEmpty(this.searchQuery) && !this.searchBoxFocused;
				if (flag3)
				{
					GUI.contentColor = Color.gray;
					GUI.Label(position2, "Search...", style);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					GUI.Label(position2, text, style);
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				bool flag4 = this.searchBoxFocused && !string.IsNullOrEmpty(this.searchQuery);
				if (flag4)
				{
					this.PerformSearch();
				}
				GUILayout.EndArea();
			}

			// Token: 0x0600009B RID: 155 RVA: 0x0000DF2C File Offset: 0x0000C12C
			private void DrawSearchResults()
			{
				float x = 110f;
				float y = 85f;
				float width = this.windowRect.width - 120f;
				float height = this.windowRect.height - 125f;
				GUILayout.BeginArea(new Rect(x, y, width, height));
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label("Search Results", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleLeft
				}, null);
				GUILayout.Space(10f);
				bool flag = this.searchResults.Count > 0;
				if (flag)
				{
					this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, null);
					foreach (SkidMenuPlugin.SkidMenu.SearchResult searchResult in this.searchResults)
					{
						GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.95f);
						GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
						GUILayout.Label(searchResult.featureName, new GUIStyle(GUI.skin.label)
						{
							fontSize = 12,
							fontStyle = FontStyle.Bold
						}, null);
						GUI.contentColor = Color.gray;
						GUILayout.Label("Tab: " + searchResult.tabName, new GUIStyle(GUI.skin.label)
						{
							fontSize = 10
						}, null);
						GUILayout.Label(searchResult.description, new GUIStyle(GUI.skin.label)
						{
							fontSize = 10,
							wordWrap = true
						}, null);
						GUILayout.Space(5f);
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						bool flag2 = GUILayout.Button("Go to " + searchResult.tabName + " Tab", new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag2)
						{
							SkidMenuPlugin.ActiveTab = searchResult.tabName;
							try
							{
								System.Reflection.FieldInfo field = typeof(SkidMenuPlugin).GetField("targetTab", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
								System.Reflection.FieldInfo field2 = typeof(SkidMenuPlugin).GetField("tabTransitionProgress", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
								bool flag3 = field != null;
								if (flag3)
								{
									field.SetValue(null, searchResult.tabName);
								}
								bool flag4 = field2 != null;
								if (flag4)
								{
									field2.SetValue(null, 0f);
								}
							}
							catch
							{
							}
							this.searchQuery = "";
							this.searchBoxFocused = false;
							this.searchResults.Clear();
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
						GUILayout.EndVertical();
						GUILayout.Space(5f);
					}
					GUILayout.EndScrollView();
				}
				else
				{
					GUILayout.FlexibleSpace();
					GUI.contentColor = Color.gray;
					GUILayout.Label("No features found matching '" + this.searchQuery + "'", new GUIStyle(GUI.skin.label)
					{
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter,
						fontStyle = FontStyle.Italic,
						wordWrap = true
					}, null);
					GUILayout.Space(10f);
					GUILayout.Label("Try searching for:", new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUILayout.Label("• Feature names (e.g., 'No Clip', 'God Mode')\n• Tab names (e.g., 'Host', 'Self')\n• Descriptions (e.g., 'kill', 'teleport')", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter,
						wordWrap = true
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndArea();
			}

			// Token: 0x0600009C RID: 156 RVA: 0x0000E32C File Offset: 0x0000C52C
			private void PerformSearch()
			{
				this.searchResults.Clear();
				string value = this.searchQuery.ToLower();
				System.Collections.Generic.List<SkidMenuPlugin.SkidMenu.SearchResult> list = new System.Collections.Generic.List<SkidMenuPlugin.SkidMenu.SearchResult>
				{
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Kill Cooldown",
						tabName = "Self",
						description = "Show kill timer overlay"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Kill Notification",
						tabName = "Self",
						description = "Get notified of kills"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "No Clip",
						tabName = "Self",
						description = "Walk through walls"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Spin",
						tabName = "Self",
						description = "Spin your character"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Show Player Info",
						tabName = "Self",
						description = "Display player details"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Anti-Overload",
						tabName = "Self",
						description = "Block RPC attacks"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "See Ghosts",
						tabName = "Self",
						description = "See dead players"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Reveal Roles",
						tabName = "Self",
						description = "See everyone's role"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Reveal Votes",
						tabName = "Self",
						description = "See votes in meetings"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Dark Mode",
						tabName = "Self",
						description = "Enable dark theme"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "See Mod Users",
						tabName = "Self",
						description = "Detect other mod users"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Show Host",
						tabName = "Self",
						description = "Display host name"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Emergency Meeting",
						tabName = "Game",
						description = "Force emergency meeting"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Complete Tasks",
						tabName = "Game",
						description = "Auto-complete all tasks"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Game Speed",
						tabName = "Game",
						description = "Adjust game speed"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Scanner Animation",
						tabName = "Game",
						description = "Show scan effect"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Overload Info",
						tabName = "Game",
						description = "Monitor RPC attacks"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Force Roles",
						tabName = "Host",
						description = "Assign roles to players"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Force Color",
						tabName = "Host",
						description = "Change player colors"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "God Mode",
						tabName = "Host",
						description = "Become unkillable"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Disable Votekicks",
						tabName = "Host",
						description = "Block votekick attempts"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Disable Meetings",
						tabName = "Host",
						description = "Block emergency meetings"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Kick Player",
						tabName = "Host",
						description = "Kick selected player"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Ban Player",
						tabName = "Host",
						description = "Ban selected player"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Kill All",
						tabName = "Host",
						description = "Kill everyone instantly"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Reactor",
						tabName = "Sabotages",
						description = "Trigger reactor sabotage"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Oxygen",
						tabName = "Sabotages",
						description = "Trigger O2 sabotage"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Lights",
						tabName = "Sabotages",
						description = "Turn off lights"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Comms",
						tabName = "Sabotages",
						description = "Break communications"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Close Doors",
						tabName = "Sabotages",
						description = "Lock all doors"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Overload",
						tabName = "Destruct",
						description = "Crash other players"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Overflow",
						tabName = "Destruct",
						description = "Block player data"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Hide from Host",
						tabName = "Destruct",
						description = "Teleport far away"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Manual Chat",
						tabName = "Chat",
						description = "Send custom messages"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Spam Chat",
						tabName = "Chat",
						description = "Auto-spam messages"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Whisper",
						tabName = "Chat",
						description = "Private message player"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Weird Chat",
						tabName = "Chat",
						description = "Send funny messages"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Coloured Chat",
						tabName = "Chat",
						description = "Send colored text"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Anticheat",
						tabName = "Anticheat",
						description = "Enable cheat detection"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Auto-Ban",
						tabName = "Anticheat",
						description = "Auto-ban cheaters"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Blacklist",
						tabName = "Anticheat",
						description = "Manage banned players"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Fake Role",
						tabName = "Roles",
						description = "Change your visible role"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Engineer Cheats",
						tabName = "Roles",
						description = "Unlimited vent time"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Scientist Cheats",
						tabName = "Roles",
						description = "Unlimited vitals"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Impostor Cheats",
						tabName = "Roles",
						description = "Unlimited kill range"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Spawn Lobby",
						tabName = "Map",
						description = "Create lobby instance"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Spawn Map",
						tabName = "Map",
						description = "Load game map"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Votekick All",
						tabName = "Votekick",
						description = "Mass votekick players"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Find Daters",
						tabName = "No Dating",
						description = "Filter dating lobbies"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Extended Lobby",
						tabName = "No Dating",
						description = "Show more lobbies"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Destroy Lobby",
						tabName = "No Dating",
						description = "Crash current lobby"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "RGB Mode",
						tabName = "About",
						description = "Rainbow menu colors"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Stealth Mode",
						tabName = "About",
						description = "Hide menu RPC"
					},
					new SkidMenuPlugin.SkidMenu.SearchResult
					{
						featureName = "Spoof Menu",
						tabName = "About",
						description = "Fake other menus"
					}
				};
				foreach (SkidMenuPlugin.SkidMenu.SearchResult searchResult in list)
				{
					bool flag = searchResult.featureName.ToLower().Contains(value) || searchResult.description.ToLower().Contains(value) || searchResult.tabName.ToLower().Contains(value);
					if (flag)
					{
						this.searchResults.Add(searchResult);
					}
				}
				bool flag2 = this.searchResults.Count > 5;
				if (flag2)
				{
					this.searchResults = this.searchResults.GetRange(0, 5);
				}
			}

			// Token: 0x0600009D RID: 157 RVA: 0x0000ED8C File Offset: 0x0000CF8C
			private void DrawSidebar()
			{
				float width = 90f;
				float x = 10f;
				float y = 85f;
				float height = this.windowRect.height - 125f;
				GUILayout.BeginArea(new Rect(x, y, width, height));
				bool flag = this.DrawTabButton("About", SkidMenuPlugin.ActiveTab == "About");
				if (flag)
				{
					SkidMenuPlugin.ActiveTab = "About";
				}
				GUILayout.Space(5f);
				bool flag2 = this.DrawTabButton("Game", SkidMenuPlugin.ActiveTab == "Game");
				if (flag2)
				{
					SkidMenuPlugin.ActiveTab = "Game";
				}
				GUILayout.Space(5f);
				bool flag3 = this.DrawTabButton("Self", SkidMenuPlugin.ActiveTab == "Self");
				if (flag3)
				{
					SkidMenuPlugin.ActiveTab = "Self";
				}
				GUILayout.Space(5f);
				bool flag4 = this.DrawTabButton("Map", SkidMenuPlugin.ActiveTab == "Map");
				if (flag4)
				{
					SkidMenuPlugin.ActiveTab = "Map";
				}
				GUILayout.Space(5f);
				bool flag5 = this.DrawTabButton("Roles", SkidMenuPlugin.ActiveTab == "Roles");
				if (flag5)
				{
					SkidMenuPlugin.ActiveTab = "Roles";
				}
				GUILayout.Space(5f);
				bool flag6 = this.DrawTabButton("Sabotages", SkidMenuPlugin.ActiveTab == "Sabotages");
				if (flag6)
				{
					SkidMenuPlugin.ActiveTab = "Sabotages";
				}
				GUILayout.Space(5f);
				bool flag7 = this.DrawTabButton("Host", SkidMenuPlugin.ActiveTab == "Host");
				if (flag7)
				{
					SkidMenuPlugin.ActiveTab = "Host";
				}
				GUILayout.Space(5f);
				bool flag8 = this.DrawTabButton("Votekick", SkidMenuPlugin.ActiveTab == "Votekick");
				if (flag8)
				{
					SkidMenuPlugin.ActiveTab = "Votekick";
				}
				GUILayout.Space(5f);
				bool flag9 = this.DrawTabButton("Destruct", SkidMenuPlugin.ActiveTab == "Destruct");
				if (flag9)
				{
					SkidMenuPlugin.ActiveTab = "Destruct";
				}
				GUILayout.Space(5f);
				bool flag10 = this.DrawTabButton("No Dating", SkidMenuPlugin.ActiveTab == "No Dating");
				if (flag10)
				{
					SkidMenuPlugin.ActiveTab = "No Dating";
				}
				GUILayout.Space(5f);
				bool flag11 = this.DrawTabButton("Chat", SkidMenuPlugin.ActiveTab == "Chat");
				if (flag11)
				{
					SkidMenuPlugin.ActiveTab = "Chat";
				}
				GUILayout.Space(5f);
				bool flag12 = this.DrawTabButton("Anticheat", SkidMenuPlugin.ActiveTab == "Anticheat");
				if (flag12)
				{
					SkidMenuPlugin.ActiveTab = "Anticheat";
				}
				GUILayout.EndArea();
			}

			// Token: 0x0600009E RID: 158 RVA: 0x0000F044 File Offset: 0x0000D244
			private bool DrawTabButton(string label, bool isActive)
			{
				Color backgroundColor = GUI.backgroundColor;
				Color backgroundColor2;
				if (isActive)
				{
					bool rgbmode = SkidMenuPlugin.RGBMode;
					if (rgbmode)
					{
						Color rgbcolor = SkidMenuPlugin.GetRGBColor();
						rgbcolor.r = Mathf.Min(rgbcolor.r + 0.2f, 1f);
						rgbcolor.g = Mathf.Min(rgbcolor.g + 0.2f, 1f);
						rgbcolor.b = Mathf.Min(rgbcolor.b + 0.2f, 1f);
						backgroundColor2 = rgbcolor;
					}
					else
					{
						backgroundColor2 = new Color(0.4f, 0.2f, 0.6f, 1f);
					}
				}
				else
				{
					backgroundColor2 = new Color(0.2f, 0.2f, 0.2f, 1f);
				}
				GUI.backgroundColor = backgroundColor2;
				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.button, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				GUI.Box(rect, "", GUI.skin.box);
				GUI.Label(rect, label, new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = (isActive ? Color.white : new Color(0.7f, 0.7f, 0.7f, 1f))
					}
				});
				bool flag = GUI.Button(rect, "", GUIStyle.none);
				bool flag2 = flag && SkidMenuPlugin.ActiveTab != label;
				if (flag2)
				{
					this.targetTab = label;
					this.tabTransitionProgress = 0f;
				}
				GUI.backgroundColor = backgroundColor;
				return flag;
			}

			// Token: 0x0600009F RID: 159 RVA: 0x0000F200 File Offset: 0x0000D400
			private void DrawMainContent()
			{
				float x = 110f + this.contentOffset.x;
				float y = 50f + this.contentOffset.y;
				float width = this.windowRect.width - 120f;
				float height = this.windowRect.height - 90f;
				Color color = GUI.color;
				float a = this.tabTransitionProgress;
				GUI.color = new Color(1f, 1f, 1f, a);
				GUILayout.BeginArea(new Rect(x, y, width, height));
				bool flag = SkidMenuPlugin.ActiveTab == "About";
				if (flag)
				{
					this.DrawAboutTab();
				}
				else
				{
					bool flag2 = SkidMenuPlugin.ActiveTab == "Game";
					if (flag2)
					{
						this.DrawGameTab();
					}
					else
					{
						bool flag3 = SkidMenuPlugin.ActiveTab == "Self";
						if (flag3)
						{
							this.DrawSelfTab();
						}
						else
						{
							bool flag4 = SkidMenuPlugin.ActiveTab == "Map";
							if (flag4)
							{
								this.DrawMapTab();
							}
							else
							{
								bool flag5 = SkidMenuPlugin.ActiveTab == "Roles";
								if (flag5)
								{
									this.DrawRolesTab();
								}
								else
								{
									bool flag6 = SkidMenuPlugin.ActiveTab == "Sabotages";
									if (flag6)
									{
										this.DrawSabotagesTab();
									}
									else
									{
										bool flag7 = SkidMenuPlugin.ActiveTab == "Host";
										if (flag7)
										{
											this.DrawHostTab();
										}
										else
										{
											bool flag8 = SkidMenuPlugin.ActiveTab == "Votekick";
											if (flag8)
											{
												this.DrawVotekickTab();
											}
											else
											{
												bool flag9 = SkidMenuPlugin.ActiveTab == "Destruct";
												if (flag9)
												{
													this.DrawDestructTab();
												}
												else
												{
													bool flag10 = SkidMenuPlugin.ActiveTab == "No Dating";
													if (flag10)
													{
														this.DrawNoDatingTab();
													}
													else
													{
														bool flag11 = SkidMenuPlugin.ActiveTab == "Chat";
														if (flag11)
														{
															this.DrawChatTab();
														}
														else
														{
															bool flag12 = SkidMenuPlugin.ActiveTab == "Chat";
															if (flag12)
															{
																this.DrawChatTab();
															}
															else
															{
																bool flag13 = SkidMenuPlugin.ActiveTab == "Anticheat";
																if (flag13)
																{
																	this.DrawAnticheatTab();
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				GUILayout.EndArea();
				GUI.color = color;
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x0000F43C File Offset: 0x0000D63C
			private void DrawAboutTab()
			{
				bool flag = this.showSpoofMenuDropdown;
				bool flag2 = flag;
				if (flag2)
				{
					this.aboutScrollPosition = GUILayout.BeginScrollView(this.aboutScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				}
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f));
				GUILayout.Label("Welcome!", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					wordWrap = true,
					fontSize = 12
				};
				GUILayout.Label("Thank you for trying out my menu!", style, null);
				GUILayout.Space(8f);
				GUILayout.Label("Don't share this menu with anyone else.", style, null);
				GUILayout.Space(8f);
				GUILayout.Label("I will add more features later, so stay tuned!", style, null);
				GUILayout.Space(8f);
				GUILayout.Label("Enjoy cheating responsibly.", style, null);
				GUILayout.Space(15f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f));
				GUILayout.Label("Visual Settings", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("RGB Mode", ref SkidMenuPlugin.RGBMode);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f));
				GUILayout.Label("Menu Settings", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(null);
				GUILayout.Label("Menu Hotkey:", new GUILayoutOption[]
				{
					GUILayout.Width(100f)
				});
				GUI.backgroundColor = (SkidMenuPlugin.isChangingKey ? new Color(1f, 0.5f, 0f, 1f) : new Color(0.3f, 0.3f, 0.4f, 1f));
				string text = SkidMenuPlugin.isChangingKey ? "Press any key..." : SkidMenuPlugin.MenuKey.Value.ToString();
				bool flag3 = GUILayout.Button(text, new GUILayoutOption[]
				{
					GUILayout.Height(25f)
				});
				if (flag3)
				{
					SkidMenuPlugin.isChangingKey = true;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndHorizontal();
				bool isChangingKey = SkidMenuPlugin.isChangingKey;
				if (isChangingKey)
				{
					GUI.contentColor = Color.yellow;
					GUILayout.Label("Waiting for key press...", new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				GUILayout.Space(10f);
				bool stealthMode = SkidMenuPlugin.StealthMode;
				this.DrawToggleSwitch("Stealth Mode", ref SkidMenuPlugin.StealthMode);
				bool flag4 = stealthMode != SkidMenuPlugin.StealthMode;
				if (flag4)
				{
					bool stealthMode2 = SkidMenuPlugin.StealthMode;
					if (stealthMode2)
					{
						SkidMenuPlugin.Logger.LogInfo("[SkidMenu] \ud83d\udd12 STEALTH MODE ENABLED - Menu is now hidden");
					}
					else
					{
						SkidMenuPlugin.Logger.LogInfo("[SkidMenu] \ud83d\udce1 STEALTH MODE DISABLED - Now broadcasting RPC 121");
					}
				}
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.5f, 0.2f));
				GUILayout.Label("\ud83c\udfad Spoof Menu Usage", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Spoof Menu Identity", ref SkidMenuPlugin.SpoofMenuEnabled);
				bool spoofMenuEnabled = SkidMenuPlugin.SpoofMenuEnabled;
				if (spoofMenuEnabled)
				{
					GUILayout.Space(5f);
					GUI.contentColor = Color.yellow;
					GUILayout.Label("Select Menu to Impersonate:", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.Space(3f);
					GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
					string str = SkidMenuPlugin.spoofMenuNames[SkidMenuPlugin.selectedSpoofMenuIndex];
					bool flag5 = GUILayout.Button("? " + str, new GUILayoutOption[]
					{
						GUILayout.Height(30f)
					});
					if (flag5)
					{
						this.showSpoofMenuDropdown = !this.showSpoofMenuDropdown;
						bool flag6 = this.showSpoofMenuDropdown;
						if (flag6)
						{
							this.aboutScrollPosition = Vector2.zero;
							this.spoofMenuDropdownScrollPosition = Vector2.zero;
						}
					}
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					bool flag7 = this.showSpoofMenuDropdown;
					if (flag7)
					{
						GUILayout.Space(5f);
						GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
						GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
						this.spoofMenuDropdownScrollPosition = GUILayout.BeginScrollView(this.spoofMenuDropdownScrollPosition, new GUILayoutOption[]
						{
							GUILayout.Height(150f)
						});
						for (int i = 0; i < SkidMenuPlugin.spoofMenuNames.Length; i++)
						{
							GUI.backgroundColor = ((SkidMenuPlugin.selectedSpoofMenuIndex == i) ? new Color(1f, 0.5f, 0.2f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f));
							bool flag8 = GUILayout.Button(SkidMenuPlugin.spoofMenuNames[i], new GUILayoutOption[]
							{
								GUILayout.Height(25f)
							});
							if (flag8)
							{
								SkidMenuPlugin.selectedSpoofMenuIndex = i;
								SkidMenuPlugin.Config_SpoofMenuIndex.Value = i;
								this.showSpoofMenuDropdown = false;
								SkidMenuPlugin.Logger.LogInfo(string.Concat(new string[]
								{
									"Spoof menu changed to: ",
									SkidMenuPlugin.spoofMenuNames[i],
									" (RPC ",
									SkidMenuPlugin.spoofMenuRPCs[i].ToString(),
									")"
								}));
							}
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
						GUILayout.EndScrollView();
						GUILayout.EndVertical();
					}
					GUILayout.Space(5f);
					GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.cyan;
					GUILayout.Label("?? Currently Broadcasting As:", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						fontStyle = FontStyle.Bold
					}, null);
					GUI.contentColor = new Color(1f, 0.8f, 0.3f);
					GUILayout.Label(SkidMenuPlugin.spoofMenuNames[SkidMenuPlugin.selectedSpoofMenuIndex], new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("RPC ID: " + SkidMenuPlugin.spoofMenuRPCs[SkidMenuPlugin.selectedSpoofMenuIndex].ToString(), new GUIStyle(GUI.skin.label)
					{
						fontSize = 8,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				}
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.6f, 0.2f, 1f));
				GUILayout.Label("Credits", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(2f);
				GUILayout.Label("Created by Skid", new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontSize = 11
				}, null);
				GUILayout.Label("Testing and Ideator - Bent & Metox", new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontSize = 10
				}, null);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUI.contentColor = Color.white;
				bool flag9 = flag;
				if (flag9)
				{
					GUILayout.EndScrollView();
				}
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x0000FD9C File Offset: 0x0000DF9C
			private void DrawGameTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 0.8f, 1f));
				GUILayout.Label("Game Controls", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
				bool flag = GUILayout.Button("FORCE EMERGENCY", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag)
				{
					this.ExecuteEmergencyRPC();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
				bool flag2 = GUILayout.Button("END MEETING (CS)", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag2)
				{
					this.EndMeetingClientSided();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
				bool flag3 = GUILayout.Button("KICK ALL FROM VENTS", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag3)
				{
					try
					{
						bool flag4 = ShipStatus.Instance != null;
						if (flag4)
						{
							foreach (Vent vent in ShipStatus.Instance.AllVents)
							{
								VentilationSystem.Update(VentilationSystem.Operation.BootImpostors, vent.Id);
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("KickVents error: " + ex.Message);
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
				bool flag5 = GUILayout.Button("RANDOMIZE OUTFIT", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag5)
				{
					try
					{
						HatManager instance = DestroyableSingleton<HatManager>.Instance;
						bool flag6 = instance != null;
						if (flag6)
						{
							byte b = (byte)UnityEngine.Random.Range(0, 15);
							bool amHost = AmongUsClient.Instance.AmHost;
							if (amHost)
							{
								PlayerControl.LocalPlayer.RpcSetColor(b);
							}
							else
							{
								MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 7, SendOption.None, AmongUsClient.Instance.HostId);
								messageWriter.Write(b);
								AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
							}
							PlayerCustomization.EquipSkin(instance.allSkins[UnityEngine.Random.Range(0, instance.allSkins.Length)]);
							PlayerCustomization.EquipHat(instance.allHats[UnityEngine.Random.Range(0, instance.allHats.Length)]);
							PlayerCustomization.EquipVisor(instance.allVisors[UnityEngine.Random.Range(0, instance.allVisors.Length)]);
							PlayerCustomization.EquipPet(instance.allPets[UnityEngine.Random.Range(0, instance.allPets.Length)]);
							PlayerCustomization.EquipNameplate(instance.allNamePlates[UnityEngine.Random.Range(0, instance.allNamePlates.Length)]);
						}
					}
					catch (System.Exception ex2)
					{
						SkidMenuPlugin.Logger.LogError("RandomizeOutfit error: " + ex2.Message);
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
				bool flag7 = GUILayout.Button("COMPLETE ALL TASKS", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag7)
				{
					this.CompleteAllTasks();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.8f, 0.3f));
				GUILayout.Label("Game Speed", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				string text = (SkidMenuPlugin.GameSpeed == 1f) ? "1x (Normal)" : (SkidMenuPlugin.GameSpeed.ToString("F1") + "x");
				GUI.contentColor = ((SkidMenuPlugin.GameSpeed < 1f) ? Color.cyan : ((SkidMenuPlugin.GameSpeed > 1f) ? Color.yellow : Color.green));
				GUILayout.Label(text, new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 13,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				SkidMenuPlugin.GameSpeed = GUILayout.HorizontalSlider(SkidMenuPlugin.GameSpeed, 0.2f, 3f, System.Array.Empty<GUILayoutOption>());
				bool flag8 = Mathf.Abs(SkidMenuPlugin.GameSpeed - 1f) < 0.05f;
				if (flag8)
				{
					SkidMenuPlugin.GameSpeed = 1f;
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 1f);
				bool flag9 = GUILayout.Button("RESET TO NORMAL", new GUILayoutOption[]
				{
					GUILayout.Height(28f)
				});
				if (flag9)
				{
					SkidMenuPlugin.GameSpeed = 1f;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.8f, 0.3f));
				GUILayout.Label("Animations", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				string text2 = SkidMenuPlugin.IsScanning ? "SCANNING - STOP" : "SET SCANNER";
				GUI.backgroundColor = (SkidMenuPlugin.IsScanning ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag10 = GUILayout.Button(text2, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag10)
				{
					SkidMenuPlugin.IsScanning = !SkidMenuPlugin.IsScanning;
					this.SendServerScan(SkidMenuPlugin.IsScanning);
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(3f);
				string text3 = SkidMenuPlugin.AnimShieldsEnabled ? "SHIELDS - STOP" : "PLAY SHIELDS ANIM";
				GUI.backgroundColor = (SkidMenuPlugin.AnimShieldsEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag11 = GUILayout.Button(text3, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag11)
				{
					SkidMenuPlugin.AnimShieldsEnabled = !SkidMenuPlugin.AnimShieldsEnabled;
					bool animShieldsEnabled = SkidMenuPlugin.AnimShieldsEnabled;
					if (animShieldsEnabled)
					{
						this.ForcePlayAnimation(1);
					}
					else
					{
						SkidMenuPlugin.AnimShieldsEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(3f);
				string text4 = SkidMenuPlugin.AnimAsteroidsEnabled ? "ASTEROIDS - STOP" : "PLAY ASTEROIDS";
				GUI.backgroundColor = (SkidMenuPlugin.AnimAsteroidsEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag12 = GUILayout.Button(text4, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag12)
				{
					SkidMenuPlugin.AnimAsteroidsEnabled = !SkidMenuPlugin.AnimAsteroidsEnabled;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(3f);
				string text5 = SkidMenuPlugin.AnimEmptyGarbageEnabled ? "GARBAGE - STOP" : "EMPTY GARBAGE";
				GUI.backgroundColor = (SkidMenuPlugin.AnimEmptyGarbageEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag13 = GUILayout.Button(text5, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag13)
				{
					SkidMenuPlugin.AnimEmptyGarbageEnabled = !SkidMenuPlugin.AnimEmptyGarbageEnabled;
					bool animEmptyGarbageEnabled = SkidMenuPlugin.AnimEmptyGarbageEnabled;
					if (animEmptyGarbageEnabled)
					{
						this.ForcePlayAnimation(10);
					}
					else
					{
						SkidMenuPlugin.AnimEmptyGarbageEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(3f);
				string text6 = SkidMenuPlugin.AnimCamsInUseEnabled ? "CAMS IN USE - STOP" : "FAKE CAMS IN USE";
				GUI.backgroundColor = (SkidMenuPlugin.AnimCamsInUseEnabled ? new Color(0.2f, 0.6f, 0.2f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag14 = GUILayout.Button(text6, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag14)
				{
					SkidMenuPlugin.AnimCamsInUseEnabled = !SkidMenuPlugin.AnimCamsInUseEnabled;
					try
					{
						bool flag15 = ShipStatus.Instance != null;
						if (flag15)
						{
							ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Security, SkidMenuPlugin.AnimCamsInUseEnabled ? 1 : 0);
						}
					}
					catch (System.Exception ex3)
					{
						SkidMenuPlugin.Logger.LogError("Fake Cams error: " + ex3.Message);
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Overload Info", ref SkidMenuPlugin.OverloadInfoEnabled);
				bool overloadInfoEnabled = SkidMenuPlugin.OverloadInfoEnabled;
				if (overloadInfoEnabled)
				{
					GUILayout.Space(5f);
					int num = SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Count + SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Count;
					bool flag16 = num > 0;
					if (flag16)
					{
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
						GUI.contentColor = Color.red;
						GUILayout.Label("?? OVERLOAD ATTACKS DETECTED", new GUIStyle(GUI.skin.label)
						{
							fontStyle = FontStyle.Bold,
							fontSize = 11,
							alignment = TextAnchor.MiddleCenter
						}, null);
						GUI.contentColor = Color.yellow;
						bool flag17 = SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Count > 0;
						if (flag17)
						{
							GUILayout.Label("RPC Attackers: " + SkidMenuPlugin.OverloadInfoSystem.detectedAttackers.Count.ToString(), new GUIStyle(GUI.skin.label)
							{
								fontSize = 10,
								alignment = TextAnchor.MiddleCenter
							}, null);
						}
						bool flag18 = SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Count > 0;
						if (flag18)
						{
							GUILayout.Label("Method 3 Attackers: " + SkidMenuPlugin.OverloadInfoSystem.detectedMethod3Attackers.Count.ToString(), new GUIStyle(GUI.skin.label)
							{
								fontSize = 10,
								alignment = TextAnchor.MiddleCenter
							}, null);
						}
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
						GUILayout.EndVertical();
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					}
					else
					{
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
						GUI.contentColor = Color.green;
						GUILayout.Label("? Monitoring Active", new GUIStyle(GUI.skin.label)
						{
							fontStyle = FontStyle.Bold,
							fontSize = 10,
							alignment = TextAnchor.MiddleCenter
						}, null);
						GUI.contentColor = Color.gray;
						GUILayout.Label("No overload attempts detected", new GUIStyle(GUI.skin.label)
						{
							fontSize = 9,
							alignment = TextAnchor.MiddleCenter
						}, null);
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
						GUILayout.EndVertical();
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					}
					GUILayout.Space(5f);
					GUI.backgroundColor = new Color(0.3f, 0.5f, 0.8f, 1f);
					bool flag19 = GUILayout.Button("VIEW STATISTICS", new GUILayoutOption[]
					{
						GUILayout.Height(25f)
					});
					if (flag19)
					{
						string attackStatistics = SkidMenuPlugin.OverloadInfoSystem.GetAttackStatistics();
						bool flag20 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
						if (flag20)
						{
							DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, attackStatistics, true);
						}
					}
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(5f);
					GUI.backgroundColor = new Color(0.5f, 0.3f, 0.1f, 1f);
					bool flag21 = GUILayout.Button("RESET DETECTIONS", new GUILayoutOption[]
					{
						GUILayout.Height(25f)
					});
					if (flag21)
					{
						SkidMenuPlugin.OverloadInfoSystem.Reset();
					}
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.EndScrollView();
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x00010BB0 File Offset: 0x0000EDB0
			private void DrawSelfTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 1f, 0.5f));
				GUILayout.Label("Self Features", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.selfScrollPosition = GUILayout.BeginScrollView(this.selfScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				this.DrawToggleSwitch("See Kill Cooldown", ref SkidMenuPlugin.ShowKillCooldown);
				this.DrawToggleSwitch("Kill Notification", ref SkidMenuPlugin.KillNotificationEnabled);
				this.DrawToggleSwitch("Show Player Info", ref SkidMenuPlugin.ShowPlayerInfo);
				this.DrawToggleSwitch("Show Votekick Info", ref SkidMenuPlugin.ShowVotekickInfo);
				this.DrawToggleSwitch("No Clip", ref SkidMenuPlugin.NoClipEnabled);
				this.DrawToggleSwitch("Spin", ref SkidMenuPlugin.SpinEnabled);
				this.DrawToggleSwitch("Exile Me", ref SkidMenuPlugin.ExileMeEnabled);
				this.DrawToggleSwitch("Anti-Overload", ref SkidMenuPlugin.AntiOverloadEnabled);
				this.DrawToggleSwitch("Auto Copy Code", ref SkidMenuPlugin.AutoCopyCodeEnabled);
				this.DrawToggleSwitch("Dark Mode", ref SkidMenuPlugin.DarkModeEnabled);
				this.DrawToggleSwitch("See Mod Users", ref SkidMenuPlugin.SeeModUsersEnabled);
				this.DrawToggleSwitch("Show Host", ref SkidMenuPlugin.ShowHostEnabled);
				this.DrawToggleSwitch("See Ghosts", ref SkidMenuPlugin.SeeGhostsEnabled);
				this.DrawToggleSwitch("Always Chat", ref SkidMenuPlugin.AlwaysShowChatEnabled);
				this.DrawToggleSwitch("No Shadows", ref SkidMenuPlugin.NoShadowsEnabled);
				this.DrawToggleSwitch("Reveal Roles", ref SkidMenuPlugin.SeeRolesEnabled);
				this.DrawToggleSwitch("Reveal Votes", ref SkidMenuPlugin.RevealVotesEnabled);
				this.DrawToggleSwitch("Zoom Out", ref SkidMenuPlugin.ZoomOutEnabled);
				this.DrawToggleSwitch("Kill Other Imposters", ref SkidMenuPlugin.KillOtherImpostersEnabled);
				this.DrawToggleSwitch("TP to Cursor", ref SkidMenuPlugin.TeleportToCursorEnabled);
				this.DrawToggleSwitch("Unlock Cosmetics", ref SkidMenuPlugin.CosmeticsUnlockerEnabled);
				this.DrawToggleSwitch("More Lobby Info", ref SkidMenuPlugin.MoreLobbyInfoEnabled);
				this.DrawToggleSwitch("Avoid Penalties", ref SkidMenuPlugin.AvoidPenaltiesEnabled);
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
				bool flag = GUILayout.Button("RESET ROTATION", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag)
				{
					this.ResetCharacterRotation();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				bool showVotekickInfo = SkidMenuPlugin.ShowVotekickInfo;
				if (showVotekickInfo)
				{
					GUILayout.Space(10f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.red;
					GUILayout.Label("\ud83d\udc41? Votekick Monitor Active", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("You will see who votekicks anyone in chat", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				bool antiOverloadEnabled = SkidMenuPlugin.AntiOverloadEnabled;
				if (antiOverloadEnabled)
				{
					GUILayout.Space(10f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("\ud83d\udee1? Protection Active", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("You are protected from RPC overload attacks", new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				bool autoCopyCodeEnabled = SkidMenuPlugin.AutoCopyCodeEnabled;
				if (autoCopyCodeEnabled)
				{
					GUILayout.Space(10f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("\ud83d\udcbe Auto-Copy Active", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("Lobby code will be copied when joining", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				bool killOtherImpostersEnabled = SkidMenuPlugin.KillOtherImpostersEnabled;
				if (killOtherImpostersEnabled)
				{
					GUILayout.Space(10f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.red;
					GUILayout.Label("?? Friendly Fire Enabled", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("You can kill your imposter teammates", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.EndScrollView();
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x00011160 File Offset: 0x0000F360
			private void DrawMapTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 0.7f, 1f));
				GUILayout.Label("Map Controls", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, null);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.8f, 0.3f));
				GUILayout.Label("Lobby Controls", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
				bool flag = GUILayout.Button("SPAWN LOBBY", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag)
				{
					this.SpawnLobby();
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
				bool flag2 = GUILayout.Button("DESPAWN LOBBY", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag2)
				{
					this.DespawnLobby();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndVertical();
				GUILayout.Space(15f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.5f, 0.8f));
				GUILayout.Label("MeetingHud Controls", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
				bool flag3 = GUILayout.Button("SPAWN MEETINGHUD", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag3)
				{
					this.SpawnMeetingHud();
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
				bool flag4 = GUILayout.Button("DESPAWN MEETINGHUD", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag4)
				{
					this.DespawnMeetingHud();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndVertical();
				GUILayout.Space(15f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.5f, 0.8f, 1f));
				GUILayout.Label("Ship Controls", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUILayout.Label("Select Map:", new GUILayoutOption[]
				{
					GUILayout.Width(80f)
				});
				GUILayout.Space(5f);
				for (int i = 0; i < this.mapNames.Length; i++)
				{
					GUI.backgroundColor = ((SkidMenuPlugin.selectedMapId == i) ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
					bool flag5 = GUILayout.Button(this.mapNames[i], new GUILayoutOption[]
					{
						GUILayout.Height(30f)
					});
					if (flag5)
					{
						SkidMenuPlugin.selectedMapId = i;
					}
					GUILayout.Space(3f);
				}
				GUILayout.Space(10f);
				GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
				bool flag6 = GUILayout.Button("SPAWN MAP", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag6)
				{
					this.SpawnMap(SkidMenuPlugin.selectedMapId);
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
				bool flag7 = GUILayout.Button("DESPAWN MAP", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag7)
				{
					this.DespawnMap();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 0.7f, 1f));
				GUILayout.Label("Map Info", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Spawn/Despawn lobby", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Spawn/Despawn meeting screen", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Choose map and spawn it", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Despawn current map", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndScrollView();
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x000117B4 File Offset: 0x0000F9B4
			private void DrawRolesTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.8f, 0.4f, 1f));
				GUILayout.Label("Roles & Abilities", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.8f, 0.2f));
				GUILayout.Label("\ud83c\udfad Set Fake Role (Local)", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Enable Fake Role", ref SkidMenuPlugin.SetFakeRoleEnabled);
				bool setFakeRoleEnabled = SkidMenuPlugin.SetFakeRoleEnabled;
				if (setFakeRoleEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
					string text = SkidMenuPlugin.SelectedFakeRole.ToString();
					bool flag = GUILayout.Button(text, new GUILayoutOption[]
					{
						GUILayout.Height(30f)
					});
					if (flag)
					{
						SkidMenuPlugin.fakeRoleDropdownOpen = !SkidMenuPlugin.fakeRoleDropdownOpen;
					}
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					bool fakeRoleDropdownOpen = SkidMenuPlugin.fakeRoleDropdownOpen;
					if (fakeRoleDropdownOpen)
					{
						GUILayout.Space(5f);
						GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
						GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
						RoleTypes[] array = new RoleTypes[11];
						RuntimeHelpers.InitializeArray(array, typeof(PrivateImplementationDetails).GetField("A3EDC3E4ACB5F9D1BD6090D46604325F77AEF5D96030C640B01766EB0C822185", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).FieldHandle);
						RoleTypes[] array2 = array;
						foreach (RoleTypes roleTypes in array2)
						{
							GUI.backgroundColor = ((SkidMenuPlugin.SelectedFakeRole == roleTypes) ? new Color(0.3f, 0.6f, 0.9f) : new Color(0.25f, 0.25f, 0.25f));
							bool flag2 = GUILayout.Button(roleTypes.ToString(), new GUILayoutOption[]
							{
								GUILayout.Height(25f)
							});
							if (flag2)
							{
								SkidMenuPlugin.SelectedFakeRole = roleTypes;
								SkidMenuPlugin.fakeRoleDropdownOpen = false;
							}
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
						GUILayout.EndVertical();
					}
					GUILayout.Space(5f);
					GUI.backgroundColor = new Color(0.2f, 0.8f, 0.3f);
					bool flag3 = GUILayout.Button("Set Role", new GUILayoutOption[]
					{
						GUILayout.Height(30f)
					});
					if (flag3)
					{
						this.ApplyFakeRole();
					}
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f));
				GUILayout.Label("\ud83d\udd2a Impostor", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Unlimited Kill Range", ref SkidMenuPlugin.UnlimitedKillRange);
				this.DrawToggleSwitch("Do Tasks as Impostor", ref SkidMenuPlugin.ImpostorTasksEnabled);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.6f, 0.2f));
				GUILayout.Label("\ud83d\udd27 Engineer", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Endless Vent Time", ref SkidMenuPlugin.EndlessVentTime);
				this.DrawToggleSwitch("No Vent Cooldown", ref SkidMenuPlugin.NoVentCooldown);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 0.8f, 1f));
				GUILayout.Label("\ud83d\udd2c Scientist", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("No Vitals Cooldown", ref SkidMenuPlugin.NoVitalsCooldown);
				this.DrawToggleSwitch("Endless Battery", ref SkidMenuPlugin.EndlessBattery);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 1f, 0.5f));
				GUILayout.Label("\ud83d\udd0d Tracker", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("No Tracking Cooldown", ref SkidMenuPlugin.NoTrackingCooldown);
				this.DrawToggleSwitch("No Tracking Delay", ref SkidMenuPlugin.NoTrackingDelay);
				this.DrawToggleSwitch("Endless Tracking", ref SkidMenuPlugin.EndlessTracking);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.5f, 0.3f, 0.9f));
				GUILayout.Label("\ud83c\udfad Shapeshifter", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Endless Shapeshift Duration", ref SkidMenuPlugin.EndlessShapeshiftDuration);
				this.DrawToggleSwitch("No Shapeshift Animation", ref SkidMenuPlugin.NoShapeshiftAnimation);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 1f, 0.5f));
				GUILayout.Label("\ud83d\udd75? Detective", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Unlimited Interrogate Range", ref SkidMenuPlugin.UnlimitedInterrogateRange);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndScrollView();
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x00011F94 File Offset: 0x00010194
			private void ApplyFakeRole()
			{
				try
				{
					bool flag;
					if (SkidMenuPlugin.OriginalRole == null)
					{
						PlayerControl localPlayer = PlayerControl.LocalPlayer;
						flag = (((localPlayer != null) ? localPlayer.Data : null) != null);
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (flag2)
					{
						SkidMenuPlugin.OriginalRole = new RoleTypes?(PlayerControl.LocalPlayer.Data.RoleType);
					}
					PlayerControl localPlayer2 = PlayerControl.LocalPlayer;
					bool flag3;
					if (localPlayer2 == null)
					{
						flag3 = false;
					}
					else
					{
						NetworkedPlayerInfo data = localPlayer2.Data;
						flag3 = ((data != null) ? new bool?(data.IsDead) : null).GetValueOrDefault();
					}
					bool flag4 = flag3;
					if (flag4)
					{
						bool flag5 = SkidMenuPlugin.SelectedFakeRole == RoleTypes.Impostor || SkidMenuPlugin.SelectedFakeRole == RoleTypes.Shapeshifter || SkidMenuPlugin.SelectedFakeRole == RoleTypes.Phantom || SkidMenuPlugin.SelectedFakeRole == RoleTypes.Viper;
						if (flag5)
						{
							DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.ImpostorGhost);
						}
						else
						{
							DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, RoleTypes.CrewmateGhost);
						}
					}
					else
					{
						DestroyableSingleton<RoleManager>.Instance.SetRole(PlayerControl.LocalPlayer, SkidMenuPlugin.SelectedFakeRole);
					}
					SkidMenuPlugin.Logger.LogInfo("Fake role set to: " + SkidMenuPlugin.SelectedFakeRole.ToString());
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to set fake role: " + ex.Message);
				}
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x000120F0 File Offset: 0x000102F0
			private void DrawHostTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.5f, 0f));
				GUILayout.Label("Host Controls", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = ((this.activeHostSection == "Utils") ? new Color(1f, 0.5f, 0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag = GUILayout.Button("?? UTILS", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag)
				{
					this.activeHostSection = "Utils";
					this.focusedSettingKey = "";
					this.settingInputBuffer = "";
				}
				GUILayout.Space(10f);
				GUI.backgroundColor = ((this.activeHostSection == "Settings") ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag2 = GUILayout.Button("\ud83c\udfae SETTINGS", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag2)
				{
					this.activeHostSection = "Settings";
					this.settingsLoaded = false;
					this.focusedSettingKey = "";
					this.settingInputBuffer = "";
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(15f);
				bool flag3 = this.activeHostSection == "Utils";
				if (flag3)
				{
					this.DrawHostUtilsSection();
				}
				else
				{
					this.DrawHostSettingsSection();
				}
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x000122E4 File Offset: 0x000104E4
			private void DrawHostUtilsSection()
			{
				this.hostScrollPosition = GUILayout.BeginScrollView(this.hostScrollPosition, null);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f));
				GUILayout.Label("Kick / Ban Players", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag = GUILayout.Button("KICK", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag)
				{
					this.KickSelectedPlayer();
				}
				GUILayout.Space(5f);
				bool flag2 = GUILayout.Button("BAN", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag2)
				{
					this.BanSelectedPlayer();
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndHorizontal();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag3 = GUILayout.Button("TP TO VENT", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag3)
				{
					this.TeleportPlayerToVent();
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				bool flag4 = GUILayout.Button("TP ALL TO VENT", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag4)
				{
					this.TeleportAllToVent();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag5 = GUILayout.Button("FORCE MEETING AS", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag5)
				{
					this.ForceMeetingAsPlayer();
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag6 = GUILayout.Button("KILL", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag6)
				{
					this.KillSelectedPlayer();
				}
				GUI.enabled = true;
				GUILayout.Space(5f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				bool flag7 = GUILayout.Button("KILL ALL", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag7)
				{
					this.KillAllPlayers();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag8 = GUILayout.Button("FLOOD TASKS", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag8)
				{
					PlayerControl playerControl = null;
					foreach (PlayerControl playerControl2 in this._cachedPlayers)
					{
						bool flag9 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
						if (flag9)
						{
							playerControl = playerControl2;
							break;
						}
					}
					bool flag10 = playerControl != null;
					if (flag10)
					{
						byte[] array = new byte[255];
						for (byte b = 0; b < 255; b += 1)
						{
							array[(int)b] = b;
						}
						playerControl.Data.RpcSetTasks(new Il2CppStructArray<byte>(array));
					}
				}
				GUILayout.Space(5f);
				bool flag11 = GUILayout.Button("CLEAR TASKS", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag11)
				{
					PlayerControl playerControl3 = null;
					foreach (PlayerControl playerControl4 in this._cachedPlayers)
					{
						bool flag12 = playerControl4 != null && playerControl4.Data != null && playerControl4.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
						if (flag12)
						{
							playerControl3 = playerControl4;
							break;
						}
					}
					bool flag13 = playerControl3 != null;
					if (flag13)
					{
						playerControl3.Data.RpcSetTasks(new Il2CppStructArray<byte>(System.Array.Empty<byte>()));
					}
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndHorizontal();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag14 = GUILayout.Button("FORCE SHAPESHIFT", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag14)
				{
					this.ForceShapeshiftPlayer();
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = new Color(0.5f, 0.3f, 0.7f, 1f);
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag15 = GUILayout.Button("TURN TO GHOST", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag15)
				{
					this.TurnPlayerToGhost();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.2f, 0.8f, 0.3f, 1f);
				GUI.enabled = (SkidMenuPlugin.selectedHostKickTargetId != -1);
				bool flag16 = GUILayout.Button("REVIVE", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag16)
				{
					this.RevivePlayer();
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUI.contentColor = new Color(0.2f, 0.7f, 0.9f);
				GUILayout.Label("Select Vent Location:", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 10
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(3f);
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
				string str = SkidMenuPlugin.ventNames[SkidMenuPlugin.selectedVentId];
				bool flag17 = GUILayout.Button("? " + str, new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag17)
				{
					this.showVentDropdown = !this.showVentDropdown;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				bool flag18 = this.showVentDropdown;
				if (flag18)
				{
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					this.ventDropdownScrollPosition = GUILayout.BeginScrollView(this.ventDropdownScrollPosition, new GUILayoutOption[]
					{
						GUILayout.Height(150f)
					});
					for (int k = 0; k < SkidMenuPlugin.ventNames.Length; k++)
					{
						GUI.backgroundColor = ((SkidMenuPlugin.selectedVentId == k) ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
						bool flag19 = GUILayout.Button(SkidMenuPlugin.ventNames[k], new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag19)
						{
							SkidMenuPlugin.selectedVentId = k;
							this.showVentDropdown = false;
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					}
					GUILayout.EndScrollView();
					GUILayout.EndVertical();
				}
				GUILayout.Space(5f);
				this.hostKickPlayerScrollPosition = GUILayout.BeginScrollView(this.hostKickPlayerScrollPosition, new GUILayoutOption[]
				{
					GUILayout.Height(120f)
				});
				foreach (PlayerControl playerControl5 in this._cachedPlayers)
				{
					bool flag20 = playerControl5 != null;
					if (flag20)
					{
						NetworkedPlayerInfo data = playerControl5.Data;
						string text = ((data != null) ? data.PlayerName : null) ?? "Unknown";
						NetworkedPlayerInfo data2 = playerControl5.Data;
						int num = (data2 != null) ? data2.ClientId : -1;
						GUI.backgroundColor = ((SkidMenuPlugin.selectedHostKickTargetId == num) ? new Color(1f, 0.5f, 0f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
						bool flag21 = GUILayout.Button(text, new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag21)
						{
							SkidMenuPlugin.selectedHostKickTargetId = num;
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Randomize Outfit", ref SkidMenuPlugin.RandomizeOutfit);
				this.DrawToggleSwitch("Ban Blacklists", ref SkidMenuPlugin.BanBlacklistedEnabled);
				this.DrawToggleSwitch("Show Lobby Timer", ref SkidMenuPlugin.ShowLobbyTimerEnabled);
				this.DrawToggleSwitch("Disable Votekicks", ref SkidMenuPlugin.DisableVotekicks);
				this.DrawToggleSwitch("Disable Meetings", ref SkidMenuPlugin.DisableMeetings);
				this.DrawToggleSwitch("Disable Sabotages", ref SkidMenuPlugin.DisableSabotagesEnabled);
				this.DrawToggleSwitch("God Mode", ref SkidMenuPlugin.GodModeEnabled);
				this.DrawToggleSwitch("Disable Game End", ref SkidMenuPlugin.DisableGameEndEnabled);
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
				bool flag22 = GUILayout.Button("?? FORCE ROLES", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag22)
				{
					SkidMenuPlugin.ShowForceRolesMenu = !SkidMenuPlugin.ShowForceRolesMenu;
					SkidMenuPlugin.showRoleDropdown = false;
					SkidMenuPlugin.dropdownPlayerIndex = -1;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.2f, 0.7f, 0.9f, 1f);
				bool flag23 = GUILayout.Button("\ud83c\udfa8 FORCE COLOR", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag23)
				{
					SkidMenuPlugin.ShowForceColorMenu = !SkidMenuPlugin.ShowForceColorMenu;
					SkidMenuPlugin.showColorDropdown = false;
					SkidMenuPlugin.dropdownPlayerIndexColor = -1;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				bool flag24 = SkidMenuPlugin.forcedColors.Count > 0;
				if (flag24)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("? " + SkidMenuPlugin.forcedColors.Count.ToString() + " Colors Assigned", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				bool flag25 = SkidMenuPlugin.forcedRoles.Count > 0;
				if (flag25)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("? " + SkidMenuPlugin.forcedRoles.Count.ToString() + " Roles Assigned", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
				bool flag26 = GUILayout.Button("FORCE START GAME", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag26)
				{
					bool flag27 = AmongUsClient.Instance != null;
					if (flag27)
					{
						AmongUsClient.Instance.StartGame();
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(3f);
				GUI.backgroundColor = new Color(0.9f, 0.2f, 0.2f, 1f);
				bool flag28 = GUILayout.Button("FORCE END GAME", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag28)
				{
					this.ForceEndGame();
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				bool flag29 = AmongUsClient.Instance != null && !AmongUsClient.Instance.AmHost;
				if (flag29)
				{
					GUI.contentColor = Color.yellow;
					GUILayout.Label("(Host features active when hosting)", new GUIStyle(GUI.skin.label)
					{
						alignment = TextAnchor.MiddleCenter,
						fontSize = 10
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					bool flag30 = AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;
					if (flag30)
					{
						bool flag31 = SkidMenuPlugin.DisableVotekicks || SkidMenuPlugin.DisableMeetings;
						if (flag31)
						{
							GUILayout.Space(5f);
							GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
							GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
							GUI.contentColor = new Color(1f, 0.5f, 0f);
							GUILayout.Label("Active Protections", new GUIStyle(GUI.skin.label)
							{
								fontStyle = FontStyle.Bold
							}, null);
							GUI.contentColor = Color.gray;
							bool disableVotekicks = SkidMenuPlugin.DisableVotekicks;
							if (disableVotekicks)
							{
								GUILayout.Label("\ud83d\udee1? Votekick Protection ON", new GUIStyle(GUI.skin.label)
								{
									fontSize = 10
								}, null);
							}
							bool disableMeetings = SkidMenuPlugin.DisableMeetings;
							if (disableMeetings)
							{
								GUILayout.Label("\ud83d\udeab Meeting Lock ON", new GUIStyle(GUI.skin.label)
								{
									fontSize = 10
								}, null);
							}
							GUI.contentColor = SkidMenuPlugin.GetRGBText();
							GUILayout.EndVertical();
							GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
						}
					}
				}
				GUILayout.EndScrollView();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUI.contentColor = Color.white;
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x000131D4 File Offset: 0x000113D4
			private void DrawHostSettingsSection()
			{
				bool flag = AmongUsClient.Instance != null && !AmongUsClient.Instance.AmHost;
				if (flag)
				{
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.yellow;
					GUILayout.Label("?? Must be Host to change settings", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				else
				{
					bool flag2 = !string.IsNullOrEmpty(this.focusedSettingKey) && this.settingsBoxFocused && Event.current.type == EventType.MouseDown;
					if (flag2)
					{
						this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
						this.focusedSettingKey = "";
						this.settingInputBuffer = "";
						this.settingsBoxFocused = false;
					}
					bool flag3 = !this.settingsLoaded;
					if (flag3)
					{
						this.LoadSettingsFromGame();
						this.settingsLoaded = true;
					}
					bool flag4 = string.IsNullOrEmpty(this.focusedSettingKey);
					if (flag4)
					{
						this._settingsSyncTimer += Time.deltaTime;
						bool flag5 = this._settingsSyncTimer >= 1f;
						if (flag5)
						{
							this._settingsSyncTimer = 0f;
							GameOptionsManager instance = GameOptionsManager.Instance;
							IGameOptions gameOptions = (instance != null) ? instance.CurrentGameOptions : null;
							bool flag6 = gameOptions != null;
							if (flag6)
							{
								this.s_emergencyMeetings = (float)gameOptions.GetInt(Int32OptionNames.NumEmergencyMeetings);
								this.s_emergencyCooldown = (float)gameOptions.GetInt(Int32OptionNames.EmergencyCooldown);
								this.s_discussionTime = (float)gameOptions.GetInt(Int32OptionNames.DiscussionTime);
								this.s_votingTime = (float)gameOptions.GetInt(Int32OptionNames.VotingTime);
								this.s_killDistance = (float)gameOptions.GetInt(Int32OptionNames.KillDistance);
								this.s_taskBarMode = (float)gameOptions.GetInt(Int32OptionNames.TaskBarMode);
								this.s_commonTasks = (float)gameOptions.GetInt(Int32OptionNames.NumCommonTasks);
								this.s_shortTasks = (float)gameOptions.GetInt(Int32OptionNames.NumShortTasks);
								this.s_longTasks = (float)gameOptions.GetInt(Int32OptionNames.NumLongTasks);
								this.s_playerSpeed = gameOptions.GetFloat(FloatOptionNames.PlayerSpeedMod);
								this.s_crewVision = gameOptions.GetFloat(FloatOptionNames.CrewLightMod);
								this.s_impVision = gameOptions.GetFloat(FloatOptionNames.ImpostorLightMod);
								this.s_killCooldown = gameOptions.GetFloat(FloatOptionNames.KillCooldown);
								this.s_vitalsCooldown = gameOptions.GetFloat(FloatOptionNames.ScientistCooldown);
								this.s_batteryDuration = gameOptions.GetFloat(FloatOptionNames.ScientistBatteryCharge);
								this.s_ventCooldown = gameOptions.GetFloat(FloatOptionNames.EngineerCooldown);
								this.s_ventDuration = gameOptions.GetFloat(FloatOptionNames.EngineerInVentMaxTime);
								this.s_protectCooldown = gameOptions.GetFloat(FloatOptionNames.GuardianAngelCooldown);
								this.s_protectDuration = gameOptions.GetFloat(FloatOptionNames.ProtectionDurationSeconds);
								this.s_shapeshiftDuration = gameOptions.GetFloat(FloatOptionNames.ShapeshifterDuration);
								this.s_shapeshiftCooldown = gameOptions.GetFloat(FloatOptionNames.ShapeshifterCooldown);
								this.s_alertDuration = gameOptions.GetFloat(FloatOptionNames.NoisemakerAlertDuration);
								this.s_trackerDuration = gameOptions.GetFloat(FloatOptionNames.TrackerDuration);
								this.s_trackerCooldown = gameOptions.GetFloat(FloatOptionNames.TrackerCooldown);
								this.s_trackerDelay = gameOptions.GetFloat(FloatOptionNames.TrackerDelay);
								this.s_phantomDuration = gameOptions.GetFloat(FloatOptionNames.PhantomDuration);
								this.s_phantomCooldown = gameOptions.GetFloat(FloatOptionNames.PhantomCooldown);
							}
						}
					}
					this.hostSettingsScrollPosition = GUILayout.BeginScrollView(this.hostSettingsScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 0.8f, 1f));
					GUILayout.Label("\ud83d\uddfa? Map", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.Space(3f);
					string[] array = new string[]
					{
						"The Skeld",
						"MIRA HQ",
						"Polus",
						"Reverse Skeld",
						"Airship",
						"The Fungle"
					};
					GUILayout.BeginHorizontal(null);
					for (int i = 0; i < array.Length; i++)
					{
						GUI.backgroundColor = ((this.selectedSettingsMapId == i) ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
						bool flag7 = GUILayout.Button(array[i], new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag7)
						{
							this.selectedSettingsMapId = i;
							try
							{
								GameOptionsManager instance2 = GameOptionsManager.Instance;
								IGameOptions gameOptions2 = (instance2 != null) ? instance2.CurrentGameOptions : null;
								bool flag8 = gameOptions2 != null;
								if (flag8)
								{
									byte value = (i == 3) ? 0 : ((byte)i);
									gameOptions2.SetByte(ByteOptionNames.MapId, value);
									this.SyncGameSettings();
								}
							}
							catch (System.Exception ex)
							{
								SkidMenuPlugin.Logger.LogError("Map error: " + ex.Message);
							}
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						bool flag9 = i == 2;
						if (flag9)
						{
							GUILayout.EndHorizontal();
							GUILayout.Space(3f);
							GUILayout.BeginHorizontal(null);
						}
					}
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("? General", new Color(1f, 0.8f, 0.3f));
					this.DrawSettingsBool("Confirm Ejects", ref this.s_confirmEjects, BoolOptionNames.ConfirmImpostor);
					this.DrawSettingsBool("Anonymous Votes", ref this.s_anonVotes, BoolOptionNames.AnonymousVotes);
					this.DrawSettingsBool("Visual Tasks", ref this.s_visualTasks, BoolOptionNames.VisualTasks);
					GUILayout.Space(4f);
					this.DrawSettingsInt("Emergency Meetings", ref this.s_emergencyMeetings, 0f, 9f, Int32OptionNames.NumEmergencyMeetings);
					this.DrawSettingsInt("Emergency Cooldown", ref this.s_emergencyCooldown, 0f, 60f, Int32OptionNames.EmergencyCooldown);
					this.DrawSettingsInt("Discussion Time", ref this.s_discussionTime, 0f, 120f, Int32OptionNames.DiscussionTime);
					this.DrawSettingsInt("Voting Time", ref this.s_votingTime, 0f, 300f, Int32OptionNames.VotingTime);
					this.DrawSettingsInt("Kill Distance", ref this.s_killDistance, 0f, 2f, Int32OptionNames.KillDistance);
					this.DrawSettingsInt("Task Bar Mode", ref this.s_taskBarMode, 0f, 2f, Int32OptionNames.TaskBarMode);
					this.DrawSettingsInt("# Common Tasks", ref this.s_commonTasks, 0f, 2f, Int32OptionNames.NumCommonTasks);
					this.DrawSettingsInt("# Short Tasks", ref this.s_shortTasks, 0f, 5f, Int32OptionNames.NumShortTasks);
					this.DrawSettingsInt("# Long Tasks", ref this.s_longTasks, 0f, 3f, Int32OptionNames.NumLongTasks);
					GUILayout.Space(4f);
					this.DrawSettingsFloat("Player Speed", ref this.s_playerSpeed, 0.5f, 3f, FloatOptionNames.PlayerSpeedMod);
					this.DrawSettingsFloat("Crewmate Vision", ref this.s_crewVision, 0.25f, 5f, FloatOptionNames.CrewLightMod);
					this.DrawSettingsFloat("Impostor Vision", ref this.s_impVision, 0.25f, 5f, FloatOptionNames.ImpostorLightMod);
					this.DrawSettingsFloat("Kill Cooldown", ref this.s_killCooldown, 10f, 60f, FloatOptionNames.KillCooldown);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83d\udd2c Scientist", new Color(0.3f, 0.8f, 1f));
					this.DrawSettingsFloat("Vitals Display Cooldown", ref this.s_vitalsCooldown, 0f, 60f, FloatOptionNames.ScientistCooldown);
					this.DrawSettingsFloat("Battery Duration", ref this.s_batteryDuration, 1f, 30f, FloatOptionNames.ScientistBatteryCharge);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83d\udd27 Engineer", new Color(1f, 0.6f, 0.2f));
					this.DrawSettingsFloat("Vent Use Cooldown", ref this.s_ventCooldown, 0f, 60f, FloatOptionNames.EngineerCooldown);
					this.DrawSettingsFloat("Max Time in Vents", ref this.s_ventDuration, 0f, 60f, FloatOptionNames.EngineerInVentMaxTime);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83d\udc7c Guardian Angel", new Color(1f, 1f, 0.5f));
					this.DrawSettingsFloat("Protect Cooldown", ref this.s_protectCooldown, 0f, 60f, FloatOptionNames.GuardianAngelCooldown);
					this.DrawSettingsFloat("Protection Duration", ref this.s_protectDuration, 0f, 30f, FloatOptionNames.ProtectionDurationSeconds);
					this.DrawSettingsBool("Protect Visible to Impostors", ref this.s_protectVisible, BoolOptionNames.ImpostorsCanSeeProtect);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83c\udfad Shapeshifter", new Color(0.5f, 0.3f, 0.9f));
					this.DrawSettingsFloat("Shapeshift Duration", ref this.s_shapeshiftDuration, 1f, 30f, FloatOptionNames.ShapeshifterDuration);
					this.DrawSettingsFloat("Shapeshift Cooldown", ref this.s_shapeshiftCooldown, 0f, 60f, FloatOptionNames.ShapeshifterCooldown);
					this.DrawSettingsBool("Leave Shapeshifting Evidence", ref this.s_shapeshiftEvidence, BoolOptionNames.ShapeshifterLeaveSkin);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83d\udd14 Noisemaker", new Color(1f, 0.4f, 0.6f));
					this.DrawSettingsFloat("Alert Duration", ref this.s_alertDuration, 0.5f, 30f, FloatOptionNames.NoisemakerAlertDuration);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83d\udd0d Tracker", new Color(0.3f, 1f, 0.5f));
					this.DrawSettingsFloat("Tracker Duration", ref this.s_trackerDuration, 1f, 30f, FloatOptionNames.TrackerDuration);
					this.DrawSettingsFloat("Tracker Cooldown", ref this.s_trackerCooldown, 0f, 60f, FloatOptionNames.TrackerCooldown);
					this.DrawSettingsFloat("Tracker Delay", ref this.s_trackerDelay, 0f, 10f, FloatOptionNames.TrackerDelay);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					this.DrawSettingsGroupHeader("\ud83d\udc7b Phantom", new Color(0.6f, 0.6f, 0.6f));
					this.DrawSettingsFloat("Phantom Duration", ref this.s_phantomDuration, 0f, 30f, FloatOptionNames.PhantomDuration);
					this.DrawSettingsFloat("Phantom Cooldown", ref this.s_phantomCooldown, 0f, 60f, FloatOptionNames.PhantomCooldown);
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					GUILayout.Space(8f);
					GUILayout.EndScrollView();
				}
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00013D1C File Offset: 0x00011F1C
			private void DrawSettingsGroupHeader(string title, Color color)
			{
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : color);
				GUILayout.Label(title, new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(3f);
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00013D94 File Offset: 0x00011F94
			private void DrawSettingsBool(string label, ref bool value, BoolOptionNames option)
			{
				GUILayout.BeginHorizontal(null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label(label, new GUILayoutOption[]
				{
					GUILayout.Width(170f)
				});
				GUILayout.FlexibleSpace();
				float num = 45f;
				float num2 = 22f;
				Rect rect = GUILayoutUtility.GetRect(num, num2);
				Color color = value ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, color);
				GUI.backgroundColor = color;
				GUI.Box(rect, "", guistyle);
				bool flag = GUI.Button(rect, "", GUIStyle.none);
				if (flag)
				{
					value = !value;
					try
					{
						GameOptionsManager instance = GameOptionsManager.Instance;
						if (instance != null)
						{
							IGameOptions currentGameOptions = instance.CurrentGameOptions;
							if (currentGameOptions != null)
							{
								currentGameOptions.SetBool(option, value);
							}
						}
						this.SyncGameSettings();
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("SetBool error: " + ex.Message);
					}
				}
				float num3 = num2 - 4f;
				float x = value ? (rect.x + num - num3 - 2f) : (rect.x + 2f);
				GUIStyle guistyle2 = new GUIStyle(GUI.skin.box);
				guistyle2.normal.background = this.MakeTex(2, 2, Color.white);
				GUI.backgroundColor = Color.white;
				GUI.Box(new Rect(x, rect.y + 2f, num3, num3), "", guistyle2);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00013F80 File Offset: 0x00012180
			private void DrawSettingsInt(string label, ref float value, float min, float max, Int32OptionNames option)
			{
				bool flag = this.focusedSettingKey == label;
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[]
				{
					GUILayout.Height(24f),
					GUILayout.Width(90f)
				});
				GUI.Box(rect, "", GUI.skin.box);
				bool flag2 = GUI.Button(rect, "", GUIStyle.none);
				if (flag2)
				{
					bool flag3 = !string.IsNullOrEmpty(this.focusedSettingKey) && this.focusedSettingKey != label;
					if (flag3)
					{
						this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
					}
					this.focusedSettingKey = label;
					this.settingsBoxFocused = true;
					this.settingInputBuffer = ((int)value).ToString();
					this.settingCursorVisible = true;
					this.settingCursorBlink = 0f;
				}
				Rect position = new Rect(rect.x + 4f, rect.y + 4f, rect.width - 8f, rect.height - 8f);
				GUIStyle style = flag ? this._styleSettingsFocused : this._styleSettingsNormal;
				string text = flag ? (this.settingCursorVisible ? (this.settingInputBuffer + "|") : this.settingInputBuffer) : ((int)value).ToString();
				GUI.Label(position, text, style);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(4f);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label(label, new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.FlexibleSpace();
				GUI.backgroundColor = new Color(0.6f, 0.2f, 0.2f, 1f);
				bool flag4 = GUILayout.Button("-", new GUILayoutOption[]
				{
					GUILayout.Width(22f),
					GUILayout.Height(22f)
				});
				if (flag4)
				{
					value -= 1f;
					this.ApplyIntOption(option, (int)value);
				}
				GUI.backgroundColor = new Color(0.2f, 0.6f, 0.2f, 1f);
				bool flag5 = GUILayout.Button("+", new GUILayoutOption[]
				{
					GUILayout.Width(22f),
					GUILayout.Height(22f)
				});
				if (flag5)
				{
					value += 1f;
					this.ApplyIntOption(option, (int)value);
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
			}

			// Token: 0x060000AC RID: 172 RVA: 0x00014248 File Offset: 0x00012448
			private void DrawSettingsFloat(string label, ref float value, float min, float max, FloatOptionNames option)
			{
				bool flag = this.focusedSettingKey == label;
				float num = (max - min > 10f) ? 0.5f : 0.25f;
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[]
				{
					GUILayout.Height(24f),
					GUILayout.Width(90f)
				});
				GUI.Box(rect, "", GUI.skin.box);
				bool flag2 = GUI.Button(rect, "", GUIStyle.none);
				if (flag2)
				{
					bool flag3 = !string.IsNullOrEmpty(this.focusedSettingKey) && this.focusedSettingKey != label;
					if (flag3)
					{
						this.ApplySettingInput(this.focusedSettingKey, this.settingInputBuffer);
					}
					this.focusedSettingKey = label;
					this.settingsBoxFocused = true;
					this.settingInputBuffer = value.ToString("F2");
					this.settingCursorVisible = true;
					this.settingCursorBlink = 0f;
				}
				Rect position = new Rect(rect.x + 4f, rect.y + 4f, rect.width - 8f, rect.height - 8f);
				GUIStyle style = flag ? this._styleSettingsFocused : this._styleSettingsNormal;
				string text = flag ? (this.settingCursorVisible ? (this.settingInputBuffer + "|") : this.settingInputBuffer) : value.ToString("F2");
				GUI.Label(position, text, style);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(4f);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label(label, new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.FlexibleSpace();
				GUI.backgroundColor = new Color(0.6f, 0.2f, 0.2f, 1f);
				bool flag4 = GUILayout.Button("-", new GUILayoutOption[]
				{
					GUILayout.Width(22f),
					GUILayout.Height(22f)
				});
				if (flag4)
				{
					value = (float)System.Math.Round((double)(value - num), 2);
					this.ApplyFloatOption(option, value);
				}
				GUI.backgroundColor = new Color(0.2f, 0.6f, 0.2f, 1f);
				bool flag5 = GUILayout.Button("+", new GUILayoutOption[]
				{
					GUILayout.Width(22f),
					GUILayout.Height(22f)
				});
				if (flag5)
				{
					value = (float)System.Math.Round((double)(value + num), 2);
					this.ApplyFloatOption(option, value);
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00014530 File Offset: 0x00012730
			private void ApplySettingInput(string key, string buffer)
			{
				bool flag = string.IsNullOrWhiteSpace(buffer);
				if (!flag)
				{
					float num;
					bool flag2 = !float.TryParse(buffer, out num);
					if (!flag2)
					{
						try
						{
							uint num2 = PrivateImplementationDetails.ComputeStringHash(key);
							if (num2 <= 2459083587U)
							{
								if (num2 <= 612090288U)
								{
									if (num2 <= 467257133U)
									{
										if (num2 != 256644636U)
										{
											if (num2 != 381151393U)
											{
												if (num2 == 467257133U)
												{
													if (key == "# Common Tasks")
													{
														this.s_commonTasks = num;
														this.ApplyIntOption(Int32OptionNames.NumCommonTasks, (int)num);
													}
												}
											}
											else if (key == "Tracker Duration")
											{
												this.s_trackerDuration = num;
												this.ApplyFloatOption(FloatOptionNames.TrackerDuration, num);
											}
										}
										else if (key == "Task Bar Mode")
										{
											this.s_taskBarMode = num;
											this.ApplyIntOption(Int32OptionNames.TaskBarMode, (int)num);
										}
									}
									else if (num2 != 528437283U)
									{
										if (num2 != 568237726U)
										{
											if (num2 == 612090288U)
											{
												if (key == "Emergency Meetings")
												{
													this.s_emergencyMeetings = num;
													this.ApplyIntOption(Int32OptionNames.NumEmergencyMeetings, (int)num);
												}
											}
										}
										else if (key == "Kill Cooldown")
										{
											this.s_killCooldown = num;
											this.ApplyFloatOption(FloatOptionNames.KillCooldown, num);
										}
									}
									else if (key == "Shapeshift Cooldown")
									{
										this.s_shapeshiftCooldown = num;
										this.ApplyFloatOption(FloatOptionNames.ShapeshifterCooldown, num);
									}
								}
								else if (num2 <= 1690602399U)
								{
									if (num2 != 998922042U)
									{
										if (num2 != 1551499986U)
										{
											if (num2 == 1690602399U)
											{
												if (key == "Alert Duration")
												{
													this.s_alertDuration = num;
													this.ApplyFloatOption(FloatOptionNames.NoisemakerAlertDuration, num);
												}
											}
										}
										else if (key == "# Long Tasks")
										{
											this.s_longTasks = num;
											this.ApplyIntOption(Int32OptionNames.NumLongTasks, (int)num);
										}
									}
									else if (key == "Discussion Time")
									{
										this.s_discussionTime = num;
										this.ApplyIntOption(Int32OptionNames.DiscussionTime, (int)num);
									}
								}
								else if (num2 <= 2011133385U)
								{
									if (num2 != 1693244362U)
									{
										if (num2 == 2011133385U)
										{
											if (key == "Vitals Display Cooldown")
											{
												this.s_vitalsCooldown = num;
												this.ApplyFloatOption(FloatOptionNames.ScientistCooldown, num);
											}
										}
									}
									else if (key == "Phantom Duration")
									{
										this.s_phantomDuration = num;
										this.ApplyFloatOption(FloatOptionNames.PhantomDuration, num);
									}
								}
								else if (num2 != 2392924606U)
								{
									if (num2 == 2459083587U)
									{
										if (key == "Crewmate Vision")
										{
											this.s_crewVision = num;
											this.ApplyFloatOption(FloatOptionNames.CrewLightMod, num);
										}
									}
								}
								else if (key == "Shapeshift Duration")
								{
									this.s_shapeshiftDuration = num;
									this.ApplyFloatOption(FloatOptionNames.ShapeshifterDuration, num);
								}
							}
							else if (num2 <= 2919009890U)
							{
								if (num2 <= 2689404054U)
								{
									if (num2 != 2473026420U)
									{
										if (num2 != 2591742836U)
										{
											if (num2 == 2689404054U)
											{
												if (key == "Battery Duration")
												{
													this.s_batteryDuration = num;
													this.ApplyFloatOption(FloatOptionNames.ScientistBatteryCharge, num);
												}
											}
										}
										else if (key == "Kill Distance")
										{
											this.s_killDistance = num;
											this.ApplyIntOption(Int32OptionNames.KillDistance, (int)num);
										}
									}
									else if (key == "Impostor Vision")
									{
										this.s_impVision = num;
										this.ApplyFloatOption(FloatOptionNames.ImpostorLightMod, num);
									}
								}
								else if (num2 <= 2910279947U)
								{
									if (num2 != 2776912636U)
									{
										if (num2 == 2910279947U)
										{
											if (key == "Voting Time")
											{
												this.s_votingTime = num;
												this.ApplyIntOption(Int32OptionNames.VotingTime, (int)num);
											}
										}
									}
									else if (key == "Tracker Cooldown")
									{
										this.s_trackerCooldown = num;
										this.ApplyFloatOption(FloatOptionNames.TrackerCooldown, num);
									}
								}
								else if (num2 != 2915651535U)
								{
									if (num2 == 2919009890U)
									{
										if (key == "Protection Duration")
										{
											this.s_protectDuration = num;
											this.ApplyFloatOption(FloatOptionNames.ProtectionDurationSeconds, num);
										}
									}
								}
								else if (key == "Phantom Cooldown")
								{
									this.s_phantomCooldown = num;
									this.ApplyFloatOption(FloatOptionNames.PhantomCooldown, num);
								}
							}
							else if (num2 <= 3184318511U)
							{
								if (num2 != 3051115085U)
								{
									if (num2 != 3062221931U)
									{
										if (num2 == 3184318511U)
										{
											if (key == "Emergency Cooldown")
											{
												this.s_emergencyCooldown = num;
												this.ApplyIntOption(Int32OptionNames.EmergencyCooldown, (int)num);
											}
										}
									}
									else if (key == "Player Speed")
									{
										this.s_playerSpeed = num;
										this.ApplyFloatOption(FloatOptionNames.PlayerSpeedMod, num);
									}
								}
								else if (key == "Protect Cooldown")
								{
									this.s_protectCooldown = num;
									this.ApplyFloatOption(FloatOptionNames.GuardianAngelCooldown, num);
								}
							}
							else if (num2 <= 3548980197U)
							{
								if (num2 != 3473002466U)
								{
									if (num2 == 3548980197U)
									{
										if (key == "Max Time in Vents")
										{
											this.s_ventDuration = num;
											this.ApplyFloatOption(FloatOptionNames.EngineerInVentMaxTime, num);
										}
									}
								}
								else if (key == "Vent Use Cooldown")
								{
									this.s_ventCooldown = num;
									this.ApplyFloatOption(FloatOptionNames.EngineerCooldown, num);
								}
							}
							else if (num2 != 4029529074U)
							{
								if (num2 == 4063416484U)
								{
									if (key == "Tracker Delay")
									{
										this.s_trackerDelay = num;
										this.ApplyFloatOption(FloatOptionNames.TrackerDelay, num);
									}
								}
							}
							else if (key == "# Short Tasks")
							{
								this.s_shortTasks = num;
								this.ApplyIntOption(Int32OptionNames.NumShortTasks, (int)num);
							}
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("ApplySettingInput error for [" + key + "]: " + ex.Message);
						}
					}
				}
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00014C50 File Offset: 0x00012E50
			private void ApplyIntOption(Int32OptionNames option, int value)
			{
				try
				{
					GameOptionsManager instance = GameOptionsManager.Instance;
					if (instance != null)
					{
						IGameOptions currentGameOptions = instance.CurrentGameOptions;
						if (currentGameOptions != null)
						{
							currentGameOptions.SetInt(option, value);
						}
					}
					this.SyncGameSettings();
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ApplyIntOption error: " + ex.Message);
				}
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00014CB8 File Offset: 0x00012EB8
			private void ApplyFloatOption(FloatOptionNames option, float value)
			{
				try
				{
					GameOptionsManager instance = GameOptionsManager.Instance;
					if (instance != null)
					{
						IGameOptions currentGameOptions = instance.CurrentGameOptions;
						if (currentGameOptions != null)
						{
							currentGameOptions.SetFloat(option, value);
						}
					}
					this.SyncGameSettings();
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ApplyFloatOption error: " + ex.Message);
				}
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00014D20 File Offset: 0x00012F20
			private void SyncGameSettings()
			{
				try
				{
					bool flag = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
					if (!flag)
					{
						bool flag2 = GameManager.Instance != null;
						if (flag2)
						{
							LogicOptions logicOptions = GameManager.Instance.LogicOptions;
							if (logicOptions != null)
							{
								logicOptions.SyncOptions();
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("SyncGameSettings error: " + ex.Message);
				}
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00014DAC File Offset: 0x00012FAC
			private void LoadSettingsFromGame()
			{
				try
				{
					GameOptionsManager instance = GameOptionsManager.Instance;
					IGameOptions gameOptions = (instance != null) ? instance.CurrentGameOptions : null;
					bool flag = gameOptions == null;
					if (!flag)
					{
						this.s_confirmEjects = gameOptions.GetBool(BoolOptionNames.ConfirmImpostor);
						this.s_anonVotes = gameOptions.GetBool(BoolOptionNames.AnonymousVotes);
						this.s_visualTasks = gameOptions.GetBool(BoolOptionNames.VisualTasks);
						this.s_protectVisible = gameOptions.GetBool(BoolOptionNames.ImpostorsCanSeeProtect);
						this.s_shapeshiftEvidence = gameOptions.GetBool(BoolOptionNames.ShapeshifterLeaveSkin);
						this.s_emergencyMeetings = (float)gameOptions.GetInt(Int32OptionNames.NumEmergencyMeetings);
						this.s_emergencyCooldown = (float)gameOptions.GetInt(Int32OptionNames.EmergencyCooldown);
						this.s_discussionTime = (float)gameOptions.GetInt(Int32OptionNames.DiscussionTime);
						this.s_votingTime = (float)gameOptions.GetInt(Int32OptionNames.VotingTime);
						this.s_killDistance = (float)gameOptions.GetInt(Int32OptionNames.KillDistance);
						this.s_taskBarMode = (float)gameOptions.GetInt(Int32OptionNames.TaskBarMode);
						this.s_commonTasks = (float)gameOptions.GetInt(Int32OptionNames.NumCommonTasks);
						this.s_shortTasks = (float)gameOptions.GetInt(Int32OptionNames.NumShortTasks);
						this.s_longTasks = (float)gameOptions.GetInt(Int32OptionNames.NumLongTasks);
						this.s_playerSpeed = gameOptions.GetFloat(FloatOptionNames.PlayerSpeedMod);
						this.s_crewVision = gameOptions.GetFloat(FloatOptionNames.CrewLightMod);
						this.s_impVision = gameOptions.GetFloat(FloatOptionNames.ImpostorLightMod);
						this.s_killCooldown = gameOptions.GetFloat(FloatOptionNames.KillCooldown);
						this.s_vitalsCooldown = gameOptions.GetFloat(FloatOptionNames.ScientistCooldown);
						this.s_batteryDuration = gameOptions.GetFloat(FloatOptionNames.ScientistBatteryCharge);
						this.s_ventCooldown = gameOptions.GetFloat(FloatOptionNames.EngineerCooldown);
						this.s_ventDuration = gameOptions.GetFloat(FloatOptionNames.EngineerInVentMaxTime);
						this.s_protectCooldown = gameOptions.GetFloat(FloatOptionNames.GuardianAngelCooldown);
						this.s_protectDuration = gameOptions.GetFloat(FloatOptionNames.ProtectionDurationSeconds);
						this.s_shapeshiftDuration = gameOptions.GetFloat(FloatOptionNames.ShapeshifterDuration);
						this.s_shapeshiftCooldown = gameOptions.GetFloat(FloatOptionNames.ShapeshifterCooldown);
						this.s_alertDuration = gameOptions.GetFloat(FloatOptionNames.NoisemakerAlertDuration);
						this.s_trackerDuration = gameOptions.GetFloat(FloatOptionNames.TrackerDuration);
						this.s_trackerCooldown = gameOptions.GetFloat(FloatOptionNames.TrackerCooldown);
						this.s_trackerDelay = gameOptions.GetFloat(FloatOptionNames.TrackerDelay);
						this.s_phantomDuration = gameOptions.GetFloat(FloatOptionNames.PhantomDuration);
						this.s_phantomCooldown = gameOptions.GetFloat(FloatOptionNames.PhantomCooldown);
						this.selectedSettingsMapId = (int)gameOptions.GetByte(ByteOptionNames.MapId);
						SkidMenuPlugin.Logger.LogInfo("[Settings] Loaded from game successfully");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("LoadSettingsFromGame error: " + ex.Message);
				}
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x00015024 File Offset: 0x00013224
			private void DrawVotekickTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.6f, 0.2f));
				GUILayout.Label("Votekick Players", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Votekick Many Players", ref SkidMenuPlugin.VotekickAllEnabled);
				bool votekickAllEnabled = SkidMenuPlugin.VotekickAllEnabled;
				if (votekickAllEnabled)
				{
					this.VotekickAll();
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.red;
					GUILayout.Label("?? AUTO-VOTEKICK ACTIVE", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.yellow;
					GUILayout.Label("All current and new players will be votekicked!", new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("Votekicked: " + SkidMenuPlugin.votekickedPlayerIds.Count.ToString() + " players", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				else
				{
					SkidMenuPlugin.votekickedPlayerIds.Clear();
				}
				GUILayout.Space(10f);
				GUI.contentColor = Color.yellow;
				GUILayout.Label("Select Player to Votekick", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.votekickScrollPosition = GUILayout.BeginScrollView(this.votekickScrollPosition, new GUILayoutOption[]
				{
					GUILayout.Height(220f)
				});
				foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
				{
					bool flag = playerControl != null && playerControl.PlayerId != PlayerControl.LocalPlayer.PlayerId;
					if (flag)
					{
						string playerName = playerControl.Data.PlayerName;
						GUI.backgroundColor = ((SkidMenuPlugin.selectedVotekickTargetId == playerControl.Data.ClientId) ? Color.yellow : new Color(0.3f, 0.3f, 0.3f, 1f));
						bool flag2 = GUILayout.Button(playerName, new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag2)
						{
							SkidMenuPlugin.selectedVotekickTargetId = playerControl.Data.ClientId;
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					}
				}
				GUILayout.EndScrollView();
				GUILayout.Space(10f);
				GUI.backgroundColor = new Color(1f, 0.5f, 0f, 1f);
				GUI.enabled = (SkidMenuPlugin.selectedVotekickTargetId != -1);
				bool flag3 = GUILayout.Button("VOTEKICK TARGET", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag3)
				{
					this.VotekickTarget();
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.6f, 0.2f));
				GUILayout.Label("Votekick Info", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Votekick All: Auto-kicks everyone", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Works on new players joining", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Select a player for single votekick", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Leave and rejoin 2 more times", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x000154FC File Offset: 0x000136FC
			private void DrawDestructTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.2f, 0.2f));
				GUILayout.Label("Destruct Players", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = ((this.activeDestructSection == "Overload") ? new Color(1f, 0.3f, 0.3f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag = GUILayout.Button("? OVERLOAD", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag)
				{
					this.activeDestructSection = "Overload";
				}
				GUILayout.Space(10f);
				GUI.backgroundColor = ((this.activeDestructSection == "Overflow") ? new Color(0.3f, 0.7f, 1f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				bool flag2 = GUILayout.Button("\ud83c\udf0a OVERFLOW", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag2)
				{
					this.activeDestructSection = "Overflow";
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(15f);
				this.destroyScrollPosition = GUILayout.BeginScrollView(this.destroyScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				bool flag3 = this.activeDestructSection == "Overload";
				if (flag3)
				{
					this.DrawOverloadSection();
				}
				else
				{
					bool flag4 = this.activeDestructSection == "Overflow";
					if (flag4)
					{
						this.DrawOverflowSection();
					}
				}
				GUILayout.EndScrollView();
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00015700 File Offset: 0x00013900
			private void DrawOverloadSection()
			{
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f));
				GUILayout.Label("? OVERLOAD ATTACKS", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				Color backgroundColor = SkidMenuPlugin.OverloadEnabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor;
				bool flag = GUILayout.Button(SkidMenuPlugin.OverloadEnabled ? "STOP OVERLOAD ALL" : "OVERLOAD ALL [METHOD 1]", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag)
				{
					SkidMenuPlugin.OverloadEnabled = !SkidMenuPlugin.OverloadEnabled;
					bool overloadEnabled = SkidMenuPlugin.OverloadEnabled;
					if (overloadEnabled)
					{
						SkidMenuPlugin.OverloadMethod2Enabled = false;
						SkidMenuPlugin.OverloadMethod3Enabled = false;
						SkidMenuPlugin.OverloadMethod4Enabled = false;
						SkidMenuPlugin.TargetedOverloadEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				Color backgroundColor2 = SkidMenuPlugin.OverloadMethod2Enabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor2;
				bool flag2 = GUILayout.Button(SkidMenuPlugin.OverloadMethod2Enabled ? "STOP OVERLOAD ALL" : "OVERLOAD ALL [METHOD 2]", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag2)
				{
					SkidMenuPlugin.OverloadMethod2Enabled = !SkidMenuPlugin.OverloadMethod2Enabled;
					bool overloadMethod2Enabled = SkidMenuPlugin.OverloadMethod2Enabled;
					if (overloadMethod2Enabled)
					{
						SkidMenuPlugin.OverloadEnabled = false;
						SkidMenuPlugin.OverloadMethod3Enabled = false;
						SkidMenuPlugin.OverloadMethod4Enabled = false;
						SkidMenuPlugin.TargetedOverloadEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				Color backgroundColor3 = SkidMenuPlugin.OverloadMethod3Enabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor3;
				bool flag3 = GUILayout.Button(SkidMenuPlugin.OverloadMethod3Enabled ? "STOP OVERLOAD ALL" : "OVERLOAD ALL [METHOD 3]", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag3)
				{
					SkidMenuPlugin.OverloadMethod3Enabled = !SkidMenuPlugin.OverloadMethod3Enabled;
					bool overloadMethod3Enabled = SkidMenuPlugin.OverloadMethod3Enabled;
					if (overloadMethod3Enabled)
					{
						SkidMenuPlugin.OverloadEnabled = false;
						SkidMenuPlugin.OverloadMethod2Enabled = false;
						SkidMenuPlugin.TargetedOverloadEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				Color backgroundColor4 = SkidMenuPlugin.OverloadMethod4Enabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor4;
				bool flag4 = GUILayout.Button(SkidMenuPlugin.OverloadMethod4Enabled ? "STOP OVERLOAD ALL" : "OVERLOAD ALL [METHOD 4]", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag4)
				{
					SkidMenuPlugin.OverloadMethod4Enabled = !SkidMenuPlugin.OverloadMethod4Enabled;
					bool overloadMethod4Enabled = SkidMenuPlugin.OverloadMethod4Enabled;
					if (overloadMethod4Enabled)
					{
						SkidMenuPlugin.OverloadEnabled = false;
						SkidMenuPlugin.OverloadMethod2Enabled = false;
						SkidMenuPlugin.OverloadMethod3Enabled = false;
						SkidMenuPlugin.TargetedOverloadEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				Color backgroundColor5 = SkidMenuPlugin.LagEveryoneEnabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor5;
				bool flag5 = GUILayout.Button(SkidMenuPlugin.LagEveryoneEnabled ? "STOP LAG EVERYONE" : "LAG EVERYONE", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag5)
				{
					SkidMenuPlugin.LagEveryoneEnabled = !SkidMenuPlugin.LagEveryoneEnabled;
					bool lagEveryoneEnabled = SkidMenuPlugin.LagEveryoneEnabled;
					if (lagEveryoneEnabled)
					{
						SkidMenuPlugin.OverloadEnabled = false;
						SkidMenuPlugin.OverloadMethod2Enabled = false;
						SkidMenuPlugin.OverloadMethod3Enabled = false;
						SkidMenuPlugin.OverloadMethod4Enabled = false;
						SkidMenuPlugin.TargetedOverloadEnabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				Color backgroundColor6 = SkidMenuPlugin.BreakCounterEnabled ? new Color(1f, 0.5f, 0.1f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor6;
				bool flag6 = GUILayout.Button(SkidMenuPlugin.BreakCounterEnabled ? "STOP BREAK COUNTER" : "START BREAK COUNTER", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag6)
				{
					SkidMenuPlugin.BreakCounterEnabled = !SkidMenuPlugin.BreakCounterEnabled;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(10f);
				GUI.contentColor = Color.yellow;
				GUILayout.Label("Target Specific Player", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 10
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = ((SkidMenuPlugin.targetedOverloadMethod == 1) ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : SkidMenuPlugin.GetRGBAccent());
				bool flag7 = GUILayout.Button("Method 2", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag7)
				{
					SkidMenuPlugin.targetedOverloadMethod = 1;
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = ((SkidMenuPlugin.targetedOverloadMethod == 2) ? new Color(0.5f, 0.2f, 0.9f, 0.5f) : SkidMenuPlugin.GetRGBAccent());
				bool flag8 = GUILayout.Button("Method 3", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag8)
				{
					SkidMenuPlugin.targetedOverloadMethod = 2;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				this.destroyPlayerScrollPosition = GUILayout.BeginScrollView(this.destroyPlayerScrollPosition, new GUILayoutOption[]
				{
					GUILayout.Height(100f)
				});
				foreach (PlayerControl playerControl in this._cachedPlayers)
				{
					bool flag9 = playerControl != null && playerControl.PlayerId != PlayerControl.LocalPlayer.PlayerId;
					if (flag9)
					{
						NetworkedPlayerInfo data = playerControl.Data;
						string text = ((data != null) ? data.PlayerName : null) ?? "Unknown";
						GUI.backgroundColor = ((SkidMenuPlugin.selectedTargetId == playerControl.OwnerId) ? new Color(1f, 0.8f, 0f, 0.5f) : SkidMenuPlugin.GetRGBAccent());
						bool flag10 = GUILayout.Button(text, new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag10)
						{
							SkidMenuPlugin.selectedTargetId = playerControl.OwnerId;
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					}
				}
				GUILayout.EndScrollView();
				GUILayout.Space(10f);
				Color backgroundColor7 = SkidMenuPlugin.TargetedOverloadEnabled ? new Color(1f, 0.3f, 0.3f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor7;
				GUI.enabled = (SkidMenuPlugin.selectedTargetId != -1);
				bool flag11 = GUILayout.Button(SkidMenuPlugin.TargetedOverloadEnabled ? "STOP TARGET" : "OVERLOAD TARGET", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag11)
				{
					SkidMenuPlugin.TargetedOverloadEnabled = !SkidMenuPlugin.TargetedOverloadEnabled;
					bool targetedOverloadEnabled = SkidMenuPlugin.TargetedOverloadEnabled;
					if (targetedOverloadEnabled)
					{
						SkidMenuPlugin.OverloadEnabled = false;
						SkidMenuPlugin.OverloadMethod2Enabled = false;
						SkidMenuPlugin.OverloadMethod3Enabled = false;
					}
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = Color.red;
				GUILayout.Label("Overload Info", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Method 1: RPC 54 freeze", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Method 2: RPC 18 freeze", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Method 3: Stored data (scales)", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Method 4: Bad tag (high msgs)", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Lag Everyone: Lags all players", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Target: Lags one player", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Break: Destroys timer", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x00015FF4 File Offset: 0x000141F4
			private void DrawOverflowSection()
			{
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 0.7f, 1f));
				GUILayout.Label("\ud83c\udf0a OVERFLOW ATTACKS", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				Color backgroundColor = SkidMenuPlugin.OverflowMethod1Enabled ? new Color(0.3f, 0.7f, 1f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor;
				bool flag = GUILayout.Button(SkidMenuPlugin.OverflowMethod1Enabled ? "STOP OVERFLOW ALL" : "OVERFLOW ALL [METHOD 1]", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag)
				{
					SkidMenuPlugin.OverflowMethod1Enabled = !SkidMenuPlugin.OverflowMethod1Enabled;
					bool overflowMethod1Enabled = SkidMenuPlugin.OverflowMethod1Enabled;
					if (overflowMethod1Enabled)
					{
						SkidMenuPlugin.OverflowMethod2Enabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				Color backgroundColor2 = SkidMenuPlugin.OverflowMethod2Enabled ? new Color(0.3f, 0.7f, 1f, 0.3f) : SkidMenuPlugin.GetRGBAccent();
				GUI.backgroundColor = backgroundColor2;
				bool flag2 = GUILayout.Button(SkidMenuPlugin.OverflowMethod2Enabled ? "STOP OVERFLOW ALL" : "OVERFLOW ALL [METHOD 2]", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag2)
				{
					SkidMenuPlugin.OverflowMethod2Enabled = !SkidMenuPlugin.OverflowMethod2Enabled;
					bool overflowMethod2Enabled = SkidMenuPlugin.OverflowMethod2Enabled;
					if (overflowMethod2Enabled)
					{
						SkidMenuPlugin.OverflowMethod1Enabled = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(10f);
				Color backgroundColor3 = SkidMenuPlugin.StealthMode ? new Color(0.2f, 0.8f, 0.2f, 1f) : new Color(0.5f, 0.5f, 0.5f, 1f);
				GUI.backgroundColor = backgroundColor3;
				bool flag3 = GUILayout.Button(SkidMenuPlugin.StealthMode ? "\ud83d\udd12 UNHIDE (RETURN)" : "\ud83d\udce1 HIDE FROM ALL", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag3)
				{
					SkidMenuPlugin.StealthMode = !SkidMenuPlugin.StealthMode;
					bool stealthMode = SkidMenuPlugin.StealthMode;
					if (stealthMode)
					{
						this.HideFromHost();
					}
					else
					{
						this.UnhideFromHost();
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				bool stealthMode2 = SkidMenuPlugin.StealthMode;
				if (stealthMode2)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("\ud83d\udd12 HIDDEN FROM ALL", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("Teleported to (800, 800)", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUILayout.Label("Original position stored", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUILayout.Label("Click 'UNHIDE' to return", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = new Color(0.3f, 0.7f, 1f);
				GUILayout.Label("Overflow Info", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("OVERFLOW (Block Data):", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					fontStyle = FontStyle.Bold
				}, null);
				GUILayout.Label("• Method 1: Blocks multiple RPCs", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Method 2: Blocks name only", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Space(5f);
				GUI.contentColor = Color.yellow;
				GUILayout.Label("HIDE FROM ALL:", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Teleports you far away from map", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Host cannot see your position", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Click again to return to spawn", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00016564 File Offset: 0x00014764
			private void HideFromHost()
			{
				try
				{
					bool flag = PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.MyPhysics == null;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("Local player or physics is null");
					}
					else
					{
						SkidMenuPlugin.SkidMenu.originalPosition = PlayerControl.LocalPlayer.transform.position;
						Vector2 position = new Vector2(800f, 800f);
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(position);
						SkidMenuPlugin.Logger.LogInfo("\ud83d\udd12 Hide from Host enabled - teleported to (800, 800)");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Hide from host error: " + ex.Message);
				}
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x00016628 File Offset: 0x00014828
			private void UnhideFromHost()
			{
				try
				{
					bool flag = PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.MyPhysics == null;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("Local player or physics is null");
					}
					else
					{
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(SkidMenuPlugin.SkidMenu.originalPosition);
						ManualLogSource logger = SkidMenuPlugin.Logger;
						string str = "\ud83d\udce1 Hide from Host disabled - teleported back to ";
						Vector2 vector = SkidMenuPlugin.SkidMenu.originalPosition;
						logger.LogInfo(str + vector.ToString());
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Unhide from host error: " + ex.Message);
				}
			}

			// Token: 0x060000B8 RID: 184 RVA: 0x000166E0 File Offset: 0x000148E0
			private void DrawNoDatingTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.5f));
				GUILayout.Label("No Dating Tools", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Find Daters Lobby", ref SkidMenuPlugin.FindDatersEnabled);
				bool findDatersEnabled = SkidMenuPlugin.FindDatersEnabled;
				if (findDatersEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("? Advanced Search Active", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("Filtering: 1 impostor, 4-9 players, free chat", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Extended Lobby List", ref SkidMenuPlugin.ExtendedLobbyEnabled);
				bool extendedLobbyEnabled = SkidMenuPlugin.ExtendedLobbyEnabled;
				if (extendedLobbyEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("? Extended List Active", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("Showing 20+ lobbies with scroll", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Destroy Lobby", ref SkidMenuPlugin.DestroyLobbyEnabled);
				bool destroyLobbyEnabled = SkidMenuPlugin.DestroyLobbyEnabled;
				if (destroyLobbyEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.red;
					GUILayout.Label("?? LOBBY DESTRUCTION ACTIVE", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.yellow;
					GUILayout.Label("This lobby will be destroyed!", new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(15f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.5f));
				GUILayout.Label("No Dating Info", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Find Daters: Apply search filters", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Extended List: Show 20+ lobbies", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("• Destroy: Crash current lobby", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10
				}, null);
				GUILayout.Label("?? Use responsibly!", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00016B38 File Offset: 0x00014D38
			private void DrawChatTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.3f, 1f, 0.3f));
				GUILayout.Label("Manual Chat", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.chatScrollPosition = GUILayout.BeginScrollView(this.chatScrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 1f, 0.5f));
				GUILayout.Label("Type your message:", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 11
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[]
				{
					GUILayout.Height(100f),
					GUILayout.ExpandWidth(true)
				});
				GUI.Box(rect, "", GUI.skin.box);
				bool flag = GUI.Button(rect, "", GUIStyle.none);
				if (flag)
				{
					this.chatBoxFocused = true;
				}
				Rect position = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, rect.height - 10f);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					wordWrap = true,
					alignment = TextAnchor.UpperLeft,
					normal = new GUIStyleState
					{
						textColor = Color.white
					}
				};
				string text = this.chatMessage;
				bool flag2 = this.chatBoxFocused && this.showCursor && this.cursorPosition >= 0 && this.cursorPosition <= this.chatMessage.Length;
				if (flag2)
				{
					text = this.chatMessage.Insert(this.cursorPosition, "|");
				}
				bool flag3 = string.IsNullOrEmpty(this.chatMessage) && !this.chatBoxFocused;
				if (flag3)
				{
					GUI.contentColor = Color.gray;
					GUI.Label(position, "Click here to type...", style);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					bool flag4 = string.IsNullOrEmpty(this.chatMessage) && this.chatBoxFocused && !this.showCursor;
					if (flag4)
					{
						GUI.contentColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
						GUI.Label(position, "Start typing...", style);
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
					}
					else
					{
						bool flag5 = string.IsNullOrEmpty(text) && this.chatBoxFocused;
						if (flag5)
						{
							GUI.Label(position, "|", style);
						}
						else
						{
							GUI.Label(position, text, style);
						}
					}
				}
				GUILayout.Space(5f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(null);
				float num = Time.time - this.lastChatSendTime;
				bool flag6 = num >= 3f;
				GUI.backgroundColor = (flag6 ? new Color(0.2f, 0.7f, 0.3f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				GUI.enabled = flag6;
				string text2 = flag6 ? "SEND" : string.Format("SEND ({0:F1}s)", 3f - num);
				bool flag7 = GUILayout.Button(text2, new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag7)
				{
					bool flag8 = !string.IsNullOrWhiteSpace(this.chatMessage);
					if (flag8)
					{
						this.SendChatMessage(this.chatMessage.Trim());
					}
					this.chatBoxFocused = false;
				}
				GUI.enabled = true;
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.5f, 0.3f, 0.1f, 1f);
				bool flag9 = GUILayout.Button("CLEAR", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag9)
				{
					this.chatMessage = "";
					this.cursorPosition = 0;
					this.chatBoxFocused = false;
				}
				GUILayout.EndHorizontal();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(10f);
				GUI.backgroundColor = new Color(0.9f, 0.4f, 0.1f, 1f);
				bool flag10 = GUILayout.Button("WEIRD CHAT", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag10)
				{
					SkidMenuPlugin.SendWeirdQuickChat();
					this.chatBoxFocused = false;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.5f, 0.2f, 0.9f, 1f);
				bool flag11 = GUILayout.Button("COLOURED CHAT", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag11)
				{
					SkidMenuPlugin.SendColouredChat();
					this.chatBoxFocused = false;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(10f);
				bool flag12 = flag6 && this.selectedWhisperTargetId != -1;
				GUI.backgroundColor = (flag12 ? new Color(0.6f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
				GUI.enabled = flag12;
				string text3 = (this.selectedWhisperTargetId != -1) ? "WHISPER" : "WHISPER (Select Player)";
				bool flag13 = !flag6 && this.selectedWhisperTargetId != -1;
				if (flag13)
				{
					text3 = string.Format("WHISPER ({0:F1}s)", 3f - num);
				}
				bool flag14 = GUILayout.Button(text3, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag14)
				{
					bool flag15 = !string.IsNullOrWhiteSpace(this.chatMessage);
					if (flag15)
					{
						this.SendWhisperMessage(this.chatMessage.Trim(), this.selectedWhisperTargetId);
					}
					this.chatBoxFocused = false;
				}
				GUI.enabled = true;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.contentColor = (this.chatBoxFocused ? new Color(0.3f, 1f, 0.3f) : Color.gray);
				string arg = this.chatBoxFocused ? "[FOCUSED]" : "[Click box to type]";
				GUILayout.Label(string.Format("{0} | Chars: {1}/{2}", arg, this.chatMessage.Length, 100), new GUIStyle(GUI.skin.label)
				{
					fontSize = 9,
					alignment = TextAnchor.MiddleRight
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				bool flag16 = this.DrawToggleSwitchWithReturn("Spam Chat", ref SkidMenuPlugin.SkidMenu.SpamChatEnabled);
				if (flag16)
				{
					this.chatBoxFocused = false;
				}
				bool spamChatEnabled = SkidMenuPlugin.SkidMenu.SpamChatEnabled;
				if (spamChatEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.red;
					GUILayout.Label("?? SPAM ACTIVE", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.yellow;
					bool flag17 = !string.IsNullOrWhiteSpace(this.chatMessage);
					if (flag17)
					{
						GUILayout.Label("Spamming: \"" + this.chatMessage + "\"", new GUIStyle(GUI.skin.label)
						{
							fontSize = 10,
							alignment = TextAnchor.MiddleCenter,
							wordWrap = true
						}, null);
					}
					else
					{
						GUILayout.Label("Enter a message to spam!", new GUIStyle(GUI.skin.label)
						{
							fontSize = 10,
							alignment = TextAnchor.MiddleCenter
						}, null);
					}
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.6f, 0.2f, 0.9f));
				GUILayout.Label("Select Player to Whisper", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 11
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.whisperPlayerScrollPosition = GUILayout.BeginScrollView(this.whisperPlayerScrollPosition, new GUILayoutOption[]
				{
					GUILayout.Height(120f)
				});
				bool flag18 = PlayerControl.AllPlayerControls != null && PlayerControl.AllPlayerControls.Count > 0;
				if (flag18)
				{
					PlayerControl[] cachedPlayers = this._cachedPlayers;
					int i = 0;
					while (i < cachedPlayers.Length)
					{
						PlayerControl playerControl = cachedPlayers[i];
						bool flag19 = playerControl != null && playerControl.Data != null && !playerControl.Data.Disconnected;
						if (flag19)
						{
							bool flag20 = playerControl != null && playerControl.Data != null && !playerControl.Data.Disconnected;
							if (flag20)
							{
								bool flag21 = playerControl.PlayerId == PlayerControl.LocalPlayer.PlayerId;
								if (!flag21)
								{
									string text4 = playerControl.Data.PlayerName ?? "Unknown";
									GUI.backgroundColor = ((this.selectedWhisperTargetId == (int)playerControl.PlayerId) ? new Color(0.6f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
									bool flag22 = GUILayout.Button(text4, new GUILayoutOption[]
									{
										GUILayout.Height(25f)
									});
									if (flag22)
									{
										this.selectedWhisperTargetId = (int)playerControl.PlayerId;
										this.chatBoxFocused = false;
									}
									GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
								}
							}
						}
						IL_AE6:
						i++;
						continue;
						goto IL_AE6;
					}
				}
				else
				{
					GUI.contentColor = Color.gray;
					GUILayout.Label("No players in lobby", new GUIStyle(GUI.skin.label)
					{
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				GUILayout.EndScrollView();
				bool flag23 = this.selectedWhisperTargetId != -1;
				if (flag23)
				{
					PlayerControl playerControl2 = this._cachedPlayers.FirstOrDefault((PlayerControl p) => p != null && (int)p.PlayerId == this.selectedWhisperTargetId);
					bool flag24 = playerControl2 != null;
					if (flag24)
					{
						GUILayout.Space(5f);
						GUI.contentColor = new Color(0.6f, 0.2f, 0.9f);
						GUILayout.Label("Selected: " + playerControl2.Data.PlayerName, new GUIStyle(GUI.skin.label)
						{
							fontSize = 10,
							alignment = TextAnchor.MiddleCenter,
							fontStyle = FontStyle.Bold
						}, null);
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
					}
					else
					{
						this.selectedWhisperTargetId = -1;
					}
				}
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = Color.yellow;
				GUILayout.Label("How to Use", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 10
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Click chat box to start typing", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Enter = new line | Shift+Enter = send", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Hold Backspace to delete continuously", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Arrow keys to move cursor", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Ctrl+C/V for copy/paste", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Press ESC to stop typing", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Whisper = private message", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Spam = auto-send every 3 seconds", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndScrollView();
				bool flag25 = this.chatBoxFocused && Input.GetKeyDown(KeyCode.Escape);
				if (flag25)
				{
					this.chatBoxFocused = false;
				}
			}

			// Token: 0x060000BA RID: 186 RVA: 0x0001792C File Offset: 0x00015B2C
			private void DrawAnticheatTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.2f, 0.2f));
				GUILayout.Label("Anticheat System", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, System.Array.Empty<GUILayoutOption>());
				this.DrawToggleSwitch("Enable Anticheat", ref SkidMenuPlugin.AnticheatEnabled);
				bool anticheatEnabled = SkidMenuPlugin.AnticheatEnabled;
				if (anticheatEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.green;
					GUILayout.Label("? Anticheat Active", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 10,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(10f);
				this.DrawToggleSwitch("Auto-Ban Cheaters", ref SkidMenuPlugin.AutoBanEnabled);
				bool autoBanEnabled = SkidMenuPlugin.AutoBanEnabled;
				if (autoBanEnabled)
				{
					GUILayout.Space(5f);
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.red;
					GUILayout.Label("?? AUTO-BAN ENABLED", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = Color.gray;
					GUILayout.Label("Detected cheaters will be banned", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				GUILayout.Space(15f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f));
				GUILayout.Label("Friend Code Blacklist", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(null);
				GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box, new GUILayoutOption[]
				{
					GUILayout.Height(24f),
					GUILayout.Width(160f)
				});
				GUI.Box(rect, "", GUI.skin.box);
				bool flag = GUI.Button(rect, "", GUIStyle.none);
				if (flag)
				{
					this.blacklistInputFocused = true;
				}
				Rect position = new Rect(rect.x + 4f, rect.y + 4f, rect.width - 8f, rect.height - 8f);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 11,
					alignment = TextAnchor.MiddleLeft,
					normal = new GUIStyleState
					{
						textColor = Color.white
					}
				};
				string text = this.blacklistInput;
				bool flag2 = this.blacklistInputFocused && this.blacklistCursorVisible;
				if (flag2)
				{
					int startIndex = Mathf.Clamp(this.blacklistCursorPos, 0, this.blacklistInput.Length);
					text = this.blacklistInput.Insert(startIndex, "|");
				}
				bool flag3 = string.IsNullOrEmpty(this.blacklistInput) && !this.blacklistInputFocused;
				if (flag3)
				{
					GUI.contentColor = Color.gray;
					GUI.Label(position, "e.g. mostwanted#8746", style);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					GUI.Label(position, text, style);
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
				bool flag4 = GUILayout.Button("Add", new GUILayoutOption[]
				{
					GUILayout.Height(24f),
					GUILayout.Width(40f)
				});
				if (flag4)
				{
					bool flag5 = !string.IsNullOrWhiteSpace(this.blacklistInput);
					if (flag5)
					{
						SkidMenuPlugin.SaveToBlacklist(this.blacklistInput.Trim());
						this.blacklistAddedMessage = "Added: " + this.blacklistInput.Trim();
						this.blacklistMessageTimer = 3f;
						this.blacklistInput = "";
						this.blacklistCursorPos = 0;
						this.blacklistInputFocused = false;
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndHorizontal();
				bool flag6 = this.blacklistMessageTimer > 0f;
				if (flag6)
				{
					GUILayout.Space(3f);
					GUI.contentColor = Color.green;
					GUILayout.Label(this.blacklistAddedMessage, new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				GUILayout.Space(8f);
				GUI.contentColor = Color.yellow;
				GUILayout.Label("Blacklisted (" + SkidMenuPlugin.BlacklistedCodes.Count.ToString() + "):", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				this.blacklistScrollPosition = GUILayout.BeginScrollView(this.blacklistScrollPosition, new GUILayoutOption[]
				{
					GUILayout.Height(80f)
				});
				bool flag7 = SkidMenuPlugin.BlacklistedCodes.Count == 0;
				if (flag7)
				{
					GUI.contentColor = Color.gray;
					GUILayout.Label("No entries yet", new GUIStyle(GUI.skin.label)
					{
						fontSize = 9,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					foreach (string text2 in new HashSet<string>(SkidMenuPlugin.BlacklistedCodes))
					{
						GUILayout.BeginHorizontal(null);
						GUI.contentColor = Color.red;
						GUILayout.Label("? " + text2, new GUIStyle(GUI.skin.label)
						{
							fontSize = 10
						}, null);
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
						GUILayout.FlexibleSpace();
						GUI.backgroundColor = new Color(0.6f, 0.1f, 0.1f, 1f);
						bool flag8 = GUILayout.Button("X", new GUILayoutOption[]
						{
							GUILayout.Width(20f),
							GUILayout.Height(18f)
						});
						if (flag8)
						{
							SkidMenuPlugin.RemoveFromBlacklist(text2);
							break;
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(15f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.5f, 0.2f));
				GUILayout.Label("Detection Options", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				this.DrawToggleSwitch("Invalid Complete Task", ref SkidMenuPlugin.CheckInvalidCompleteTask);
				this.DrawToggleSwitch("Invalid Play Animation", ref SkidMenuPlugin.CheckInvalidPlayAnimation);
				this.DrawToggleSwitch("Invalid Scanner", ref SkidMenuPlugin.CheckInvalidScanner);
				this.DrawToggleSwitch("Invalid Vent", ref SkidMenuPlugin.CheckInvalidVent);
				this.DrawToggleSwitch("Invalid SnapTo", ref SkidMenuPlugin.CheckInvalidSnapTo);
				this.DrawToggleSwitch("Invalid Start Counter", ref SkidMenuPlugin.CheckInvalidStartCounter);
				this.DrawToggleSwitch("Spoofed Platforms", ref SkidMenuPlugin.CheckSpoofedPlatforms);
				this.DrawToggleSwitch("Spoofed Levels", ref SkidMenuPlugin.CheckSpoofedLevels);
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = ((SkidMenuPlugin.totalDetections > 0) ? Color.red : Color.green);
				GUILayout.Label("?? Total Detections: " + SkidMenuPlugin.totalDetections.ToString(), new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 12,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
				GUI.backgroundColor = new Color(0.5f, 0.3f, 0.1f, 1f);
				bool flag9 = GUILayout.Button("CLEAR DETECTION LOG", new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag9)
				{
					SkidMenuPlugin.detectionLog.Clear();
					SkidMenuPlugin.totalDetections = 0;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", System.Array.Empty<GUILayoutOption>());
				GUI.contentColor = Color.yellow;
				GUILayout.Label("Anticheat Info", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 10
				}, null);
				GUI.contentColor = Color.gray;
				GUILayout.Label("• Must be HOST to ban", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Detections shown in chat", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Auto-ban requires host", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUILayout.Label("• Blacklist persists across sessions", new GUIStyle(GUI.skin.label)
				{
					fontSize = 9
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndScrollView();
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00018428 File Offset: 0x00016628
			private bool DrawToggleSwitchWithReturn(string label, ref bool value)
			{
				GUILayout.BeginHorizontal(null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label(label, new GUILayoutOption[]
				{
					GUILayout.Width(140f)
				});
				GUILayout.FlexibleSpace();
				float num = 45f;
				float num2 = 22f;
				Rect rect = GUILayoutUtility.GetRect(num, num2);
				Color color = value ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, color);
				GUI.backgroundColor = color;
				GUI.Box(rect, "", guistyle);
				bool flag = GUI.Button(rect, "", GUIStyle.none);
				bool flag2 = flag;
				if (flag2)
				{
					value = !value;
				}
				float num3 = num2 - 4f;
				float x = value ? (rect.x + num - num3 - 2f) : (rect.x + 2f);
				GUIStyle guistyle2 = new GUIStyle(GUI.skin.box);
				guistyle2.normal.background = this.MakeTex(2, 2, Color.white);
				GUI.backgroundColor = Color.white;
				GUI.Box(new Rect(x, rect.y + 2f, num3, num3), "", guistyle2);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				return flag;
			}

			// Token: 0x060000BC RID: 188 RVA: 0x000185C1 File Offset: 0x000167C1
			[HideFromIl2Cpp]
			private IEnumerator DelayedBlacklistNotify(string playerName, string friendCode, string notifyKey)
			{
				SkidMenuPlugin.SkidMenu.compiled_m_DelayedBlacklistNotify_d_159 compiled_v_DelayedBlacklistNotify_d__ = new SkidMenuPlugin.SkidMenu.compiled_m_DelayedBlacklistNotify_d_159(0);
				compiled_v_DelayedBlacklistNotify_d__.compiled_this = this;
				compiled_v_DelayedBlacklistNotify_d__.playerName = playerName;
				compiled_v_DelayedBlacklistNotify_d__.friendCode = friendCode;
				compiled_v_DelayedBlacklistNotify_d__.notifyKey = notifyKey;
				return compiled_v_DelayedBlacklistNotify_d__;
			}

			// Token: 0x060000BD RID: 189 RVA: 0x000185E8 File Offset: 0x000167E8
			private void SendChatMessage(string message)
			{
				try
				{
					bool flag = PlayerControl.LocalPlayer != null;
					if (flag)
					{
						PlayerControl.LocalPlayer.RpcSendChat(message);
						this.lastChatSendTime = Time.time;
						SkidMenuPlugin.Logger.LogInfo("Chat sent (server-sided): " + message);
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Send chat error: " + ex.Message);
				}
			}

			// Token: 0x060000BE RID: 190 RVA: 0x00018668 File Offset: 0x00016868
			private void SendWhisperMessage(string message, int targetPlayerId)
			{
				try
				{
					bool flag = PlayerControl.LocalPlayer == null;
					if (!flag)
					{
						PlayerControl playerControl = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault((PlayerControl p) => p != null && (int)p.PlayerId == targetPlayerId);
						bool flag2 = playerControl == null || playerControl.Data == null;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Whisper target player not found");
						}
						else
						{
							string playerName = playerControl.Data.PlayerName;
							MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 13, SendOption.Reliable, playerControl.OwnerId);
							messageWriter.Write(message);
							AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
							if (flag3)
							{
								string chatText = "<color=#9966FF>[You are whispering to " + playerName + "]</color>\n" + message;
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, chatText, true);
							}
							this.lastChatSendTime = Time.time;
							SkidMenuPlugin.Logger.LogInfo("Whisper sent to " + playerName + ": " + message);
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Send whisper error: " + ex.Message);
				}
			}

			// Token: 0x060000BF RID: 191 RVA: 0x000187E4 File Offset: 0x000169E4
			private void ExecuteSpamChat()
			{
				bool flag = this.chatSpamDelay > 0;
				if (flag)
				{
					this.chatSpamDelay--;
				}
				else
				{
					try
					{
						bool flag2 = !string.IsNullOrWhiteSpace(this.chatMessage) && PlayerControl.LocalPlayer != null;
						if (flag2)
						{
							PlayerControl.LocalPlayer.RpcSendChat(this.chatMessage);
						}
						int num = (int)(1f / Time.deltaTime);
						this.chatSpamDelay = (int)(3f * (float)num);
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Spam Chat error: " + ex.Message);
					}
				}
			}

			// Token: 0x060000C0 RID: 192 RVA: 0x00018894 File Offset: 0x00016A94
			private void DrawSabotagesTab()
			{
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(1f, 0.3f, 0.3f));
				GUILayout.Label("Sabotage Controls", new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					fontStyle = FontStyle.Bold
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(10f);
				this.sabotageScrollPosition = GUILayout.BeginScrollView(this.sabotageScrollPosition, null);
				ShipStatus ship = ShipStatus.Instance;
				GUI.backgroundColor = (SkidMenuPlugin.SpamRepairSabotages ? new Color(0.2f, 0.8f, 0.2f, 1f) : new Color(0.2f, 0.5f, 0.8f, 1f));
				bool flag = GUILayout.Button(SkidMenuPlugin.SpamRepairSabotages ? "SPAM REPAIR - STOP" : "SPAM REPAIR SABOTAGES", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag)
				{
					SkidMenuPlugin.SpamRepairSabotages = !SkidMenuPlugin.SpamRepairSabotages;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				this.DrawSabotageButton(SkidMenuPlugin.reactorActive ? "REACTOR - STOP" : "REACTOR / LAB", SkidMenuPlugin.reactorActive, delegate
				{
					bool flag4 = ship != null;
					if (flag4)
					{
						SkidMenuPlugin.reactorActive = !SkidMenuPlugin.reactorActive;
						this.HandleReactor(ship, (byte)ship.Type, SkidMenuPlugin.reactorActive ? 128 : 16);
					}
				});
				this.DrawSabotageButton(SkidMenuPlugin.oxygenActive ? "OXYGEN - STOP" : "OXYGEN", SkidMenuPlugin.oxygenActive, delegate
				{
					bool flag4 = ship != null;
					if (flag4)
					{
						SkidMenuPlugin.oxygenActive = !SkidMenuPlugin.oxygenActive;
						this.HandleOxygen(ship, (byte)ship.Type, SkidMenuPlugin.oxygenActive ? 128 : 16);
					}
				});
				this.DrawSabotageButton(SkidMenuPlugin.commsActive ? "COMMS - STOP" : "COMMS", SkidMenuPlugin.commsActive, delegate
				{
					bool flag4 = ship != null;
					if (flag4)
					{
						SkidMenuPlugin.commsActive = !SkidMenuPlugin.commsActive;
						ship.RpcUpdateSystem(SystemTypes.Comms, SkidMenuPlugin.commsActive ? 128 : 16);
					}
				});
				this.DrawSabotageButton(SkidMenuPlugin.lightsActive ? "LIGHTS - STOP" : "LIGHTS", SkidMenuPlugin.lightsActive, delegate
				{
					bool flag4 = ship != null;
					if (flag4)
					{
						SkidMenuPlugin.lightsActive = !SkidMenuPlugin.lightsActive;
						ship.RpcUpdateSystem(SystemTypes.Electrical, SkidMenuPlugin.lightsActive ? 0 : 16);
					}
				});
				this.DrawSabotageButton(SkidMenuPlugin.unfixableLightsActive ? "UNFIXABLE - STOP" : "UNFIXABLE LIGHTS", SkidMenuPlugin.unfixableLightsActive, delegate
				{
					bool flag4 = ship != null;
					if (flag4)
					{
						SkidMenuPlugin.unfixableLightsActive = !SkidMenuPlugin.unfixableLightsActive;
						ship.RpcUpdateSystem(SystemTypes.Electrical, SkidMenuPlugin.unfixableLightsActive ? 69 : 16);
					}
				});
				GUILayout.Space(10f);
				GUI.backgroundColor = new Color(0.6f, 0.3f, 0.1f, 1f);
				bool flag2 = GUILayout.Button("CLOSE ALL DOORS", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag2)
				{
					bool flag3 = ship != null;
					if (flag3)
					{
						this.HandleDoors(ship);
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndScrollView();
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x00018B20 File Offset: 0x00016D20
			private void ExecuteRepairSabotages()
			{
				bool flag = this.repairSabotagesDelay > 0;
				if (flag)
				{
					this.repairSabotagesDelay--;
				}
				else
				{
					try
					{
						ShipStatus instance = ShipStatus.Instance;
						bool flag2 = instance == null;
						if (!flag2)
						{
							instance.RpcUpdateSystem(SystemTypes.Reactor, 16);
							instance.RpcUpdateSystem(SystemTypes.Laboratory, 16);
							instance.RpcUpdateSystem(SystemTypes.LifeSupp, 16);
							instance.RpcUpdateSystem(SystemTypes.Comms, 16);
							instance.RpcUpdateSystem(SystemTypes.Electrical, 16);
							instance.RpcUpdateSystem(SystemTypes.HeliSabotage, 16);
							int num = (int)(1f / Time.deltaTime);
							this.repairSabotagesDelay = (int)(0.1f * (float)num);
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Spam Repair Sabotages: " + ex.Message);
					}
				}
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x00018BF8 File Offset: 0x00016DF8
			private void ExecuteKillAll()
			{
				bool flag = this.killAllDelay > 0;
				if (flag)
				{
					this.killAllDelay--;
				}
				else
				{
					try
					{
						bool flag2 = PlayerControl.LocalPlayer == null || AmongUsClient.Instance == null;
						if (!flag2)
						{
							foreach (PlayerControl playerControl in this._cachedPlayers)
							{
								bool flag3 = playerControl != null && playerControl.Data != null && !playerControl.Data.IsDead;
								if (flag3)
								{
									foreach (PlayerControl character in this._cachedPlayers)
									{
										MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 12, SendOption.None, AmongUsClient.Instance.GetClientIdFromCharacter(character));
										messageWriter.WriteNetObject(playerControl);
										messageWriter.Write(1);
										AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
									}
									SkidMenuPlugin.Logger.LogInfo("Killed: " + playerControl.Data.PlayerName);
									int num = (int)(1f / Time.deltaTime);
									this.killAllDelay = (int)(0.3f * (float)num);
									return;
								}
							}
							SkidMenuPlugin.KillAllEnabled = false;
							bool flag4 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
							if (flag4)
							{
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[Host]</color> Everyone has been killed", true);
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Execute Kill All error: " + ex.Message);
					}
				}
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x00018DDC File Offset: 0x00016FDC
			private void RandomizeEverything(PlayerControl player)
			{
				try
				{
					bool flag = player == null || player.Data == null;
					if (!flag)
					{
						byte bodyColor = (byte)UnityEngine.Random.Range(0, 18);
						player.RpcSetColor(bodyColor);
						HatManager instance = DestroyableSingleton<HatManager>.Instance;
						bool flag2 = instance == null;
						if (!flag2)
						{
							Il2CppReferenceArray<HatData> unlockedHats = instance.GetUnlockedHats();
							Il2CppReferenceArray<SkinData> unlockedSkins = instance.GetUnlockedSkins();
							Il2CppReferenceArray<VisorData> unlockedVisors = instance.GetUnlockedVisors();
							Il2CppReferenceArray<PetData> unlockedPets = instance.GetUnlockedPets();
							bool flag3 = unlockedHats.Count > 0;
							if (flag3)
							{
								player.RpcSetHat(unlockedHats[UnityEngine.Random.Range(0, unlockedHats.Count)].ProdId);
							}
							bool flag4 = unlockedSkins.Count > 0;
							if (flag4)
							{
								player.RpcSetSkin(unlockedSkins[UnityEngine.Random.Range(0, unlockedSkins.Count)].ProdId);
							}
							bool flag5 = unlockedVisors.Count > 0;
							if (flag5)
							{
								player.RpcSetVisor(unlockedVisors[UnityEngine.Random.Range(0, unlockedVisors.Count)].ProdId);
							}
							bool flag6 = unlockedPets.Count > 0;
							if (flag6)
							{
								player.RpcSetPet(unlockedPets[UnityEngine.Random.Range(0, unlockedPets.Count)].ProdId);
							}
						}
					}
				}
				catch (System.Exception)
				{
				}
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00018F34 File Offset: 0x00017134
			private void SpawnLobby()
			{
				try
				{
					bool flag = LobbyBehaviour.Instance == null && DestroyableSingleton<GameStartManager>.Instance != null;
					if (flag)
					{
						LobbyBehaviour netObjParent = UnityEngine.Object.Instantiate<LobbyBehaviour>(DestroyableSingleton<GameStartManager>.Instance.LobbyPrefab);
						AmongUsClient.Instance.Spawn(netObjParent, -2, SpawnFlags.None);
						SkidMenuPlugin.Logger.LogInfo("Lobby spawned successfully");
					}
					else
					{
						SkidMenuPlugin.Logger.LogWarning("Lobby already exists or GameStartManager is null");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to spawn lobby: " + ex.Message);
				}
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x00018FDC File Offset: 0x000171DC
			private void DespawnLobby()
			{
				try
				{
					bool flag = LobbyBehaviour.Instance != null;
					if (flag)
					{
						LobbyBehaviour.Instance.Despawn();
						SkidMenuPlugin.Logger.LogInfo("Lobby despawned successfully");
					}
					else
					{
						SkidMenuPlugin.Logger.LogWarning("No lobby to despawn");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to despawn lobby: " + ex.Message);
				}
			}

			// Token: 0x060000C6 RID: 198 RVA: 0x00019060 File Offset: 0x00017260
			private void SpawnMeetingHud()
			{
				try
				{
					bool flag = MeetingHud.Instance == null && DestroyableSingleton<HudManager>.Instance != null;
					if (flag)
					{
						MeetingHud netObjParent = UnityEngine.Object.Instantiate<MeetingHud>(DestroyableSingleton<HudManager>.Instance.MeetingPrefab);
						AmongUsClient.Instance.Spawn(netObjParent, -2, SpawnFlags.None);
						SkidMenuPlugin.Logger.LogInfo("MeetingHud spawned successfully");
					}
					else
					{
						SkidMenuPlugin.Logger.LogWarning("MeetingHud already exists or HudManager is null");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to spawn MeetingHud: " + ex.Message);
				}
			}

			// Token: 0x060000C7 RID: 199 RVA: 0x00019108 File Offset: 0x00017308
			private void DespawnMeetingHud()
			{
				try
				{
					bool flag = MeetingHud.Instance != null;
					if (flag)
					{
						MeetingHud.Instance.Despawn();
						SkidMenuPlugin.Logger.LogInfo("MeetingHud despawned successfully");
					}
					else
					{
						SkidMenuPlugin.Logger.LogWarning("No MeetingHud to despawn");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to despawn MeetingHud: " + ex.Message);
				}
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x0001918C File Offset: 0x0001738C
			private void SpawnMap(int mapId)
			{
				try
				{
					bool flag = AmongUsClient.Instance == null || AmongUsClient.Instance.ShipPrefabs == null;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogError("AmongUsClient or ShipPrefabs is null");
					}
					else
					{
						bool flag2 = mapId >= AmongUsClient.Instance.ShipPrefabs.Count;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogError("Invalid map ID");
						}
						else
						{
							MonoBehaviourExtensions.StartCoroutine(this, this.CoSpawnMap(mapId));
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to spawn map: " + ex.Message);
				}
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x0001923C File Offset: 0x0001743C
			[HideFromIl2Cpp]
			private IEnumerator CoSpawnMap(int mapId)
			{
				SkidMenuPlugin.SkidMenu.compiled_m_CoSpawnMap_d_172 compiled_v_CoSpawnMap_d__ = new SkidMenuPlugin.SkidMenu.compiled_m_CoSpawnMap_d_172(0);
				compiled_v_CoSpawnMap_d__.compiled_this = this;
				compiled_v_CoSpawnMap_d__.mapId = mapId;
				return compiled_v_CoSpawnMap_d__;
			}

			// Token: 0x060000CA RID: 202 RVA: 0x00019254 File Offset: 0x00017454
			private void DespawnMap()
			{
				try
				{
					bool flag = ShipStatus.Instance != null;
					if (flag)
					{
						ShipStatus.Instance.Despawn();
						SkidMenuPlugin.Logger.LogInfo("Map despawned successfully");
					}
					else
					{
						SkidMenuPlugin.Logger.LogWarning("No map to despawn");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to despawn map: " + ex.Message);
				}
			}

			// Token: 0x060000CB RID: 203 RVA: 0x000192D8 File Offset: 0x000174D8
			private void VotekickTarget()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedVotekickTargetId != -1 && VoteBanSystem.Instance != null;
					if (flag)
					{
						VoteBanSystem.Instance.CmdAddVote(SkidMenuPlugin.selectedVotekickTargetId);
						SkidMenuPlugin.Logger.LogInfo("Votekick added to player with ClientId: " + SkidMenuPlugin.selectedVotekickTargetId.ToString());
						bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Votekick sent! Leave and rejoin 2 more times.");
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to votekick target: " + ex.Message);
				}
			}

			// Token: 0x060000CC RID: 204 RVA: 0x000193A0 File Offset: 0x000175A0
			private void VotekickAll()
			{
				try
				{
					bool flag = VoteBanSystem.Instance == null;
					if (!flag)
					{
						int num = 0;
						foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
						{
							bool flag2 = playerControl != null && !playerControl.AmOwner;
							if (flag2)
							{
								int clientId = playerControl.Data.ClientId;
								bool flag3 = !SkidMenuPlugin.votekickedPlayerIds.Contains(clientId);
								if (flag3)
								{
									for (int i = 0; i < 3; i++)
									{
										VoteBanSystem.Instance.CmdAddVote(clientId);
									}
									SkidMenuPlugin.votekickedPlayerIds.Add(clientId);
									num++;
								}
							}
						}
						SkidMenuPlugin.Logger.LogInfo("Votekick sent to " + num.ToString() + " players");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to votekick all: " + ex.Message);
				}
			}

			// Token: 0x060000CD RID: 205 RVA: 0x000194D8 File Offset: 0x000176D8
			private void CompleteAllTasks()
			{
				try
				{
					bool flag = PlayerControl.LocalPlayer == null;
					if (!flag)
					{
						PlayerControl localPlayer = PlayerControl.LocalPlayer;
						Il2CppSystem.Collections.Generic.List<NetworkedPlayerInfo.TaskInfo> tasks = localPlayer.Data.Tasks;
						bool flag2 = tasks == null || tasks.Count == 0;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("No tasks found");
						}
						else
						{
							for (uint num = 0U; num < (uint)tasks.Count; num += 1U)
							{
								bool flag3 = !tasks[(int)num].Complete;
								if (flag3)
								{
									localPlayer.RpcCompleteTask(num);
								}
							}
							SkidMenuPlugin.Logger.LogInfo("All tasks completed");
							bool flag4 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
							if (flag4)
							{
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=green>[Tasks]</color> All tasks completed!", true);
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("CompleteAllTasks error: " + ex.Message);
				}
			}

			// Token: 0x060000CE RID: 206 RVA: 0x000195FC File Offset: 0x000177FC
			private void ExecuteOverload()
			{
				PlayerControl localPlayer = PlayerControl.LocalPlayer;
				bool flag = localPlayer == null || AmongUsClient.Instance == null;
				if (!flag)
				{
					try
					{
						for (int i = 0; i < 25; i++)
						{
							AmongUsClient.Instance.FinishRpcImmediately(AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 54, SendOption.None, -1));
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Overload Method 1: " + ex.Message);
					}
				}
			}

			// Token: 0x060000CF RID: 207 RVA: 0x00019698 File Offset: 0x00017898
			private void ExecuteOverloadMethod2()
			{
				PlayerControl localPlayer = PlayerControl.LocalPlayer;
				bool flag = localPlayer == null || AmongUsClient.Instance == null;
				if (!flag)
				{
					try
					{
						for (int i = 0; i < 25; i++)
						{
							AmongUsClient.Instance.FinishRpcImmediately(AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 18, SendOption.None, -1));
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Overload Method 2: " + ex.Message);
					}
				}
			}

			// Token: 0x060000D0 RID: 208 RVA: 0x00019734 File Offset: 0x00017934
			private void ExecuteOverloadMethod3()
			{
				bool flag = AmongUsClient.Instance == null || AmongUsClient.Instance.connection == null;
				if (!flag)
				{
					try
					{
						Il2CppArrayBase<PlayerControl> il2CppArrayBase = PlayerControl.AllPlayerControls.ToArray();
						foreach (PlayerControl playerControl in il2CppArrayBase)
						{
							bool flag2 = playerControl == null || playerControl.Data == null || playerControl.Data.Disconnected;
							if (!flag2)
							{
								bool amOwner = playerControl.AmOwner;
								if (!amOwner)
								{
									for (int i = 0; i < 3; i++)
									{
										MessageWriter messageWriter = MessageWriter.Get(SendOption.None);
										messageWriter.StartMessage(6);
										messageWriter.Write(AmongUsClient.Instance.GameId);
										messageWriter.WritePacked(playerControl.OwnerId);
										for (int j = 0; j < 100; j++)
										{
											messageWriter.StartMessage(1);
											messageWriter.Write(int.MaxValue);
											messageWriter.EndMessage();
										}
										messageWriter.EndMessage();
										AmongUsClient.Instance.connection.Send(messageWriter);
										messageWriter.Recycle();
									}
								}
							}
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Overload Method 3: " + ex.Message);
					}
				}
			}

			// Token: 0x060000D1 RID: 209 RVA: 0x000198E8 File Offset: 0x00017AE8
			private void ExecuteOverloadMethod4()
			{
				bool flag = AmongUsClient.Instance == null || AmongUsClient.Instance.connection == null;
				if (!flag)
				{
					bool flag2 = this.overloadMethod4Delay > 0;
					if (flag2)
					{
						this.overloadMethod4Delay--;
					}
					else
					{
						try
						{
							for (int i = 0; i < 5; i++)
							{
								MessageWriter messageWriter = MessageWriter.Get(SendOption.None);
								messageWriter.StartMessage(5);
								messageWriter.Write(AmongUsClient.Instance.GameId);
								messageWriter.StartMessage(1);
								messageWriter.Write(0);
								messageWriter.EndMessage();
								for (int j = 0; j < 600; j++)
								{
									messageWriter.StartMessage(69);
									messageWriter.EndMessage();
								}
								messageWriter.EndMessage();
								AmongUsClient.Instance.connection.Send(messageWriter);
								messageWriter.Recycle();
							}
							int num = (int)(1f / Time.deltaTime);
							this.overloadMethod4Delay = (int)(0.05f * (float)num);
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("Overload Method 4: " + ex.Message);
						}
					}
				}
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x00019A38 File Offset: 0x00017C38
			private void ExecuteLagEveryone()
			{
				bool flag = this.lagEveryoneDelay > 0;
				if (flag)
				{
					this.lagEveryoneDelay--;
				}
				else
				{
					try
					{
						PlayerControl localPlayer = PlayerControl.LocalPlayer;
						bool flag2 = localPlayer == null || AmongUsClient.Instance == null;
						if (!flag2)
						{
							foreach (PlayerControl playerControl in this._cachedPlayers)
							{
								bool flag3 = playerControl == null || playerControl.AmOwner;
								if (!flag3)
								{
									for (int j = 0; j < 169; j++)
									{
										AmongUsClient.Instance.FinishRpcImmediately(AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 18, SendOption.None, playerControl.OwnerId));
									}
								}
							}
							int num = (int)(1f / Time.deltaTime);
							this.lagEveryoneDelay = (int)(0.3f * (float)num);
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("LagEveryone error: " + ex.Message);
					}
				}
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00019B64 File Offset: 0x00017D64
			private void ExecuteTargetedOverload()
			{
				PlayerControl localPlayer = PlayerControl.LocalPlayer;
				bool flag = localPlayer == null || AmongUsClient.Instance == null;
				if (!flag)
				{
					bool flag2 = this.targetedOverloadDelay > 0;
					if (flag2)
					{
						this.targetedOverloadDelay--;
					}
					else
					{
						try
						{
							bool flag3 = SkidMenuPlugin.targetedOverloadMethod == 1;
							if (flag3)
							{
								for (int i = 0; i < 100; i++)
								{
									AmongUsClient.Instance.FinishRpcImmediately(AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 18, SendOption.None, SkidMenuPlugin.selectedTargetId));
								}
							}
							else
							{
								for (int j = 0; j < 3; j++)
								{
									MessageWriter messageWriter = MessageWriter.Get(SendOption.None);
									messageWriter.StartMessage(6);
									messageWriter.Write(AmongUsClient.Instance.GameId);
									messageWriter.WritePacked(SkidMenuPlugin.selectedTargetId);
									for (int k = 0; k < 100; k++)
									{
										messageWriter.StartMessage(1);
										messageWriter.Write(int.MaxValue);
										messageWriter.EndMessage();
									}
									messageWriter.EndMessage();
									AmongUsClient.Instance.connection.Send(messageWriter);
									messageWriter.Recycle();
								}
							}
							this.targetedOverloadDelay = 2;
						}
						catch (System.Exception ex)
						{
							SkidMenuPlugin.Logger.LogError("Targeted Overload: " + ex.Message);
						}
					}
				}
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x00019CFC File Offset: 0x00017EFC
			private void ExecuteBreakCounter()
			{
				bool flag = this.breakCounterDelay > 0;
				if (flag)
				{
					this.breakCounterDelay--;
				}
				else
				{
					try
					{
						bool flag2 = PlayerControl.LocalPlayer != null && DestroyableSingleton<GameStartManager>.Instance != null;
						if (flag2)
						{
							int num = UnityEngine.Random.Range(-1000000, 1000000);
							PlayerControl.LocalPlayer.RpcSetStartCounter(num);
							DestroyableSingleton<GameStartManager>.Instance.SetStartCounter((sbyte)num);
						}
						int num2 = (int)(1f / Time.deltaTime);
						this.breakCounterDelay = (int)(0.03f * (float)num2);
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Break Counter: " + ex.Message);
					}
				}
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x00019DC8 File Offset: 0x00017FC8
			private void ExecuteExileMe()
			{
				bool flag = this.exileMeDelay > 0;
				if (flag)
				{
					this.exileMeDelay--;
				}
				else
				{
					try
					{
						bool flag2 = PlayerControl.LocalPlayer != null && AmongUsClient.Instance != null;
						if (flag2)
						{
							AmongUsClient.Instance.FinishRpcImmediately(AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 4, SendOption.None, AmongUsClient.Instance.GetHost().Id));
						}
						int num = (int)(1f / Time.deltaTime);
						this.exileMeDelay = (int)(0.05f * (float)num);
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("Exile Me: " + ex.Message);
					}
				}
			}

			// Token: 0x060000D6 RID: 214 RVA: 0x00019E9C File Offset: 0x0001809C
			private void DrawSabotageButton(string label, bool isActive, System.Action onClick)
			{
				GUI.backgroundColor = (isActive ? new Color(0.2f, 0.8f, 0.2f, 1f) : new Color(0.8f, 0.2f, 0.2f, 1f));
				bool flag = GUILayout.Button(label, new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag)
				{
					if (onClick != null)
					{
						onClick();
					}
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(5f);
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x00019F28 File Offset: 0x00018128
			private void DrawToggleSwitch(string label, ref bool value)
			{
				GUILayout.BeginHorizontal(null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label(label, new GUILayoutOption[]
				{
					GUILayout.Width(140f)
				});
				GUILayout.FlexibleSpace();
				float num = 45f;
				float num2 = 22f;
				Rect rect = GUILayoutUtility.GetRect(num, num2);
				Color color = value ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.25f, 0.25f, 0.25f, 1f);
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, color);
				GUI.backgroundColor = color;
				GUI.Box(rect, "", guistyle);
				bool flag = GUI.Button(rect, "", GUIStyle.none);
				bool flag2 = flag;
				if (flag2)
				{
					value = !value;
					this.SaveFeatureState(label, value);
				}
				float num3 = num2 - 4f;
				float x = value ? (rect.x + num - num3 - 2f) : (rect.x + 2f);
				GUIStyle guistyle2 = new GUIStyle(GUI.skin.box);
				guistyle2.normal.background = this.MakeTex(2, 2, Color.white);
				GUI.backgroundColor = Color.white;
				GUI.Box(new Rect(x, rect.y + 2f, num3, num3), "", guistyle2);
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
			}

			// Token: 0x060000D8 RID: 216 RVA: 0x0001A0C8 File Offset: 0x000182C8
			private void SaveFeatureState(string label, bool value)
			{
				uint num = PrivateImplementationDetails.ComputeStringHash(label);
				if (num <= 1913133059U)
				{
					if (num <= 978085209U)
					{
						if (num <= 571030591U)
						{
							if (num <= 173091344U)
							{
								if (num != 51093082U)
								{
									if (num != 78693321U)
									{
										if (num == 173091344U)
										{
											if (label == "TP to Cursor")
											{
												SkidMenuPlugin.Config_TeleportToCursorEnabled.Value = value;
											}
										}
									}
									else if (label == "Spoof Menu Identity")
									{
										SkidMenuPlugin.Config_SpoofMenuEnabled.Value = value;
									}
								}
								else if (label == "Show Votekick Info")
								{
									SkidMenuPlugin.Config_ShowVotekickInfo.Value = value;
								}
							}
							else if (num <= 397248711U)
							{
								if (num != 305727902U)
								{
									if (num == 397248711U)
									{
										if (label == "Kill Other Imposters")
										{
											SkidMenuPlugin.Config_KillOtherImpostersEnabled.Value = value;
										}
									}
								}
								else if (label == "Ban Blacklists")
								{
									SkidMenuPlugin.Config_BanBlacklistedEnabled.Value = value;
								}
							}
							else if (num != 522798336U)
							{
								if (num == 571030591U)
								{
									if (label == "Disable Meetings")
									{
										SkidMenuPlugin.Config_DisableMeetings.Value = value;
									}
								}
							}
							else if (label == "God Mode")
							{
								SkidMenuPlugin.Config_GodModeEnabled.Value = value;
							}
						}
						else if (num <= 630847470U)
						{
							if (num <= 619110909U)
							{
								if (num != 585524890U)
								{
									if (num == 619110909U)
									{
										if (label == "Destroy Lobby")
										{
											SkidMenuPlugin.Config_DestroyLobbyEnabled.Value = value;
										}
									}
								}
								else if (label == "Zoom Out")
								{
									SkidMenuPlugin.Config_ZoomOutEnabled.Value = value;
								}
							}
							else if (num != 628479060U)
							{
								if (num == 630847470U)
								{
									if (label == "Exile Me")
									{
										SkidMenuPlugin.Config_ExileMeEnabled.Value = value;
									}
								}
							}
							else if (label == "See Ghosts")
							{
								SkidMenuPlugin.Config_SeeGhostsEnabled.Value = value;
							}
						}
						else if (num <= 823429567U)
						{
							if (num != 641155173U)
							{
								if (num == 823429567U)
								{
									if (label == "Endless Vent Time")
									{
										SkidMenuPlugin.Config_EndlessVentTime.Value = value;
									}
								}
							}
							else if (label == "Overload Info")
							{
								SkidMenuPlugin.Config_OverloadInfoEnabled.Value = value;
							}
						}
						else if (num != 840444804U)
						{
							if (num == 978085209U)
							{
								if (label == "Find Daters Lobby")
								{
									SkidMenuPlugin.Config_FindDatersEnabled.Value = value;
								}
							}
						}
						else if (label == "Kill Notification")
						{
							SkidMenuPlugin.Config_KillNotificationEnabled.Value = value;
						}
					}
					else if (num <= 1424293410U)
					{
						if (num <= 1243286512U)
						{
							if (num != 1084017109U)
							{
								if (num != 1099160580U)
								{
									if (num == 1243286512U)
									{
										if (label == "Extended Lobby List")
										{
											SkidMenuPlugin.Config_ExtendedLobbyEnabled.Value = value;
										}
									}
								}
								else if (label == "Votekick All (Dynamic)")
								{
									SkidMenuPlugin.Config_VotekickAllEnabled.Value = value;
								}
							}
							else if (label == "Reveal Votes")
							{
								SkidMenuPlugin.Config_RevealVotesEnabled.Value = value;
							}
						}
						else if (num <= 1333262036U)
						{
							if (num != 1304088263U)
							{
								if (num == 1333262036U)
								{
									if (label == "See Mod Users")
									{
										SkidMenuPlugin.Config_SeeModUsersEnabled.Value = value;
									}
								}
							}
							else if (label == "Randomize Outfit")
							{
								SkidMenuPlugin.Config_RandomizeOutfit.Value = value;
							}
						}
						else if (num != 1358595668U)
						{
							if (num == 1424293410U)
							{
								if (label == "Anti-Overload")
								{
									SkidMenuPlugin.Config_AntiOverloadEnabled.Value = value;
								}
							}
						}
						else if (label == "Invalid Play Animation")
						{
							SkidMenuPlugin.Config_CheckInvalidPlayAnimation.Value = value;
						}
					}
					else if (num <= 1662814829U)
					{
						if (num <= 1563689794U)
						{
							if (num != 1426049704U)
							{
								if (num == 1563689794U)
								{
									if (label == "No Vent Cooldown")
									{
										SkidMenuPlugin.Config_NoVentCooldown.Value = value;
									}
								}
							}
							else if (label == "Endless Tracking")
							{
								SkidMenuPlugin.Config_EndlessTracking.Value = value;
							}
						}
						else if (num != 1653829359U)
						{
							if (num == 1662814829U)
							{
								if (label == "Invalid SnapTo")
								{
									SkidMenuPlugin.Config_CheckInvalidSnapTo.Value = value;
								}
							}
						}
						else if (label == "Show Player Info")
						{
							SkidMenuPlugin.Config_ShowPlayerInfo.Value = value;
						}
					}
					else if (num <= 1822125143U)
					{
						if (num != 1689032974U)
						{
							if (num == 1822125143U)
							{
								if (label == "RGB Mode")
								{
									SkidMenuPlugin.Config_RGBMode.Value = value;
								}
							}
						}
						else if (label == "Disable Game End")
						{
							SkidMenuPlugin.Config_DisableGameEndEnabled.Value = value;
						}
					}
					else if (num != 1858184668U)
					{
						if (num == 1913133059U)
						{
							if (label == "No Shapeshift Animation")
							{
								SkidMenuPlugin.Config_NoShapeshiftAnimation.Value = value;
							}
						}
					}
					else if (label == "No Tracking Delay")
					{
						SkidMenuPlugin.Config_NoTrackingDelay.Value = value;
					}
				}
				else if (num <= 3174636275U)
				{
					if (num <= 2373709972U)
					{
						if (num <= 2041500273U)
						{
							if (num != 1955176916U)
							{
								if (num != 1977104381U)
								{
									if (num == 2041500273U)
									{
										if (label == "See Kill Cooldown")
										{
											SkidMenuPlugin.Config_ShowKillCooldown.Value = value;
										}
									}
								}
								else if (label == "Show Lobby Timer")
								{
									SkidMenuPlugin.Config_ShowLobbyTimerEnabled.Value = value;
								}
							}
							else if (label == "No Tracking Cooldown")
							{
								SkidMenuPlugin.Config_NoTrackingCooldown.Value = value;
							}
						}
						else if (num <= 2344324409U)
						{
							if (num != 2071922168U)
							{
								if (num == 2344324409U)
								{
									if (label == "Spin")
									{
										SkidMenuPlugin.Config_SpinEnabled.Value = value;
									}
								}
							}
							else if (label == "Dark Mode")
							{
								SkidMenuPlugin.Config_DarkModeEnabled.Value = value;
							}
						}
						else if (num != 2348340167U)
						{
							if (num == 2373709972U)
							{
								if (label == "More Lobby Info")
								{
									SkidMenuPlugin.Config_MoreLobbyInfoEnabled.Value = value;
								}
							}
						}
						else if (label == "No Shadows")
						{
							SkidMenuPlugin.Config_NoShadowsEnabled.Value = value;
						}
					}
					else if (num <= 2874776642U)
					{
						if (num <= 2569498870U)
						{
							if (num != 2505893022U)
							{
								if (num == 2569498870U)
								{
									if (label == "Invalid Scanner")
									{
										SkidMenuPlugin.Config_CheckInvalidScanner.Value = value;
									}
								}
							}
							else if (label == "Spam Chat")
							{
								SkidMenuPlugin.Config_SpamChatEnabled.Value = value;
							}
						}
						else if (num != 2802368137U)
						{
							if (num == 2874776642U)
							{
								if (label == "Spoofed Levels")
								{
									SkidMenuPlugin.Config_CheckSpoofedLevels.Value = value;
								}
							}
						}
						else if (label == "Reveal Roles")
						{
							SkidMenuPlugin.Config_RevealRolesEnabled.Value = value;
						}
					}
					else if (num <= 3077835410U)
					{
						if (num != 3013202415U)
						{
							if (num == 3077835410U)
							{
								if (label == "Endless Battery")
								{
									SkidMenuPlugin.Config_EndlessBattery.Value = value;
								}
							}
						}
						else if (label == "Invalid Vent")
						{
							SkidMenuPlugin.Config_CheckInvalidVent.Value = value;
						}
					}
					else if (num != 3151728088U)
					{
						if (num == 3174636275U)
						{
							if (label == "Do Tasks as Impostor")
							{
								SkidMenuPlugin.Config_ImpostorTasksEnabled.Value = value;
							}
						}
					}
					else if (label == "Invalid Start Counter")
					{
						SkidMenuPlugin.Config_CheckInvalidStartCounter.Value = value;
					}
				}
				else if (num <= 3846652778U)
				{
					if (num <= 3334948974U)
					{
						if (num != 3251019356U)
						{
							if (num != 3309948629U)
							{
								if (num == 3334948974U)
								{
									if (label == "No Vitals Cooldown")
									{
										SkidMenuPlugin.Config_NoVitalsCooldown.Value = value;
									}
								}
							}
							else if (label == "Unlimited Kill Range")
							{
								SkidMenuPlugin.Config_UnlimitedKillRange.Value = value;
							}
						}
						else if (label == "Auto Copy Code")
						{
							SkidMenuPlugin.Config_AutoCopyCodeEnabled.Value = value;
						}
					}
					else if (num <= 3602235653U)
					{
						if (num != 3510940907U)
						{
							if (num == 3602235653U)
							{
								if (label == "Enable Anticheat")
								{
									SkidMenuPlugin.Config_AnticheatEnabled.Value = value;
								}
							}
						}
						else if (label == "Spoofed Platforms")
						{
							SkidMenuPlugin.Config_CheckSpoofedPlatforms.Value = value;
						}
					}
					else if (num != 3838767511U)
					{
						if (num == 3846652778U)
						{
							if (label == "Endless Shapeshift Duration")
							{
								SkidMenuPlugin.Config_EndlessShapeshiftDuration.Value = value;
							}
						}
					}
					else if (label == "Stealth Mode")
					{
						SkidMenuPlugin.Config_StealthMode.Value = value;
					}
				}
				else if (num <= 4160284323U)
				{
					if (num <= 3951370170U)
					{
						if (num != 3929400152U)
						{
							if (num == 3951370170U)
							{
								if (label == "Always Chat")
								{
									SkidMenuPlugin.Config_AlwaysShowChatEnabled.Value = value;
								}
							}
						}
						else if (label == "No Clip")
						{
							SkidMenuPlugin.Config_NoClipEnabled.Value = value;
						}
					}
					else if (num != 4106663488U)
					{
						if (num == 4160284323U)
						{
							if (label == "Unlimited Interrogate Range")
							{
								SkidMenuPlugin.Config_UnlimitedInterrogateRange.Value = value;
							}
						}
					}
					else if (label == "Disable Votekicks")
					{
						SkidMenuPlugin.Config_DisableVotekicks.Value = value;
					}
				}
				else if (num <= 4206477414U)
				{
					if (num != 4196531710U)
					{
						if (num == 4206477414U)
						{
							if (label == "Show Host")
							{
								SkidMenuPlugin.Config_ShowHostEnabled.Value = value;
							}
						}
					}
					else if (label == "Disable Sabotages")
					{
						SkidMenuPlugin.Config_DisableSabotagesEnabled.Value = value;
					}
				}
				else if (num != 4248613343U)
				{
					if (num == 4260578860U)
					{
						if (label == "Invalid Complete Task")
						{
							SkidMenuPlugin.Config_CheckInvalidCompleteTask.Value = value;
						}
					}
				}
				else if (label == "Auto-Ban Cheaters")
				{
					SkidMenuPlugin.Config_AutoBanEnabled.Value = value;
				}
			}

			// Token: 0x060000D9 RID: 217 RVA: 0x0001ADEC File Offset: 0x00018FEC
			private Texture2D MakeTex(int width, int height, Color col)
			{
				Texture2D texture2D;
				bool flag = !SkidMenuPlugin.SkidMenu._texCache.TryGetValue(col, out texture2D) || texture2D == null;
				if (flag)
				{
					texture2D = new Texture2D(width, height);
					Color[] array = new Color[width * height];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = col;
					}
					texture2D.SetPixels(array);
					texture2D.Apply();
					SkidMenuPlugin.SkidMenu._texCache[col] = texture2D;
				}
				return texture2D;
			}

			// Token: 0x060000DA RID: 218 RVA: 0x0001AE70 File Offset: 0x00019070
			private void DrawKillCooldownOverlay()
			{
				bool flag = Camera.main == null;
				if (!flag)
				{
					foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls.ToArray())
					{
						bool flag2 = playerControl == null || playerControl.Data == null || playerControl.Data.Disconnected || playerControl.Data.IsDead || !playerControl.Data.Role.IsImpostor;
						if (!flag2)
						{
							Vector3 position = playerControl.transform.position;
							position.y += 0.7f;
							Vector3 vector = Camera.main.WorldToScreenPoint(position);
							bool flag3 = vector.z > 0f;
							if (flag3)
							{
								float x = vector.x;
								float num = (float)Screen.height - vector.y;
								string text = (playerControl.killTimer <= 0f) ? "READY" : (playerControl.killTimer.ToString("F1") + "s");
								GUIStyle style = new GUIStyle(GUI.skin.label)
								{
									alignment = TextAnchor.MiddleCenter,
									fontSize = 14,
									fontStyle = FontStyle.Bold
								};
								GUI.contentColor = Color.black;
								GUI.Label(new Rect(x - 49f, num - 19f, 100f, 20f), text, style);
								GUI.contentColor = ((playerControl.killTimer <= 0f) ? Color.red : Color.yellow);
								GUI.Label(new Rect(x - 50f, num - 20f, 100f, 20f), text, style);
							}
						}
					}
					GUI.contentColor = Color.white;
				}
			}

			// Token: 0x060000DB RID: 219 RVA: 0x0001B078 File Offset: 0x00019278
			private void ResetCharacterRotation()
			{
				bool flag = PlayerControl.LocalPlayer != null;
				if (flag)
				{
					SkidMenuPlugin.spinAngle = 0f;
					PlayerControl.LocalPlayer.transform.localRotation = Quaternion.identity;
				}
			}

			// Token: 0x060000DC RID: 220 RVA: 0x0001B0B8 File Offset: 0x000192B8
			private void HandleReactor(ShipStatus ship, byte mapId, byte amount)
			{
				bool flag = mapId == 2;
				if (flag)
				{
					ship.RpcUpdateSystem(SystemTypes.Laboratory, amount);
				}
				else
				{
					bool flag2 = mapId == 4;
					if (flag2)
					{
						ship.RpcUpdateSystem(SystemTypes.HeliSabotage, amount);
					}
					else
					{
						ship.RpcUpdateSystem(SystemTypes.Reactor, amount);
					}
				}
			}

			// Token: 0x060000DD RID: 221 RVA: 0x0001B0F8 File Offset: 0x000192F8
			private void HandleOxygen(ShipStatus ship, byte mapId, byte amount)
			{
				bool flag = mapId != 4 && mapId != 2 && mapId != 5;
				if (flag)
				{
					ship.RpcUpdateSystem(SystemTypes.LifeSupp, amount);
				}
			}

			// Token: 0x060000DE RID: 222 RVA: 0x0001B128 File Offset: 0x00019328
			private void HandleDoors(ShipStatus ship)
			{
				for (int i = 0; i < ship.AllDoors.Count; i++)
				{
					try
					{
						OpenableDoor openableDoor = ship.AllDoors[i];
						ship.RpcCloseDoorsOfType(openableDoor.Room);
					}
					catch
					{
					}
				}
			}

			// Token: 0x060000DF RID: 223 RVA: 0x0001B188 File Offset: 0x00019388
			private void SendServerScan(bool start)
			{
				PlayerControl localPlayer = PlayerControl.LocalPlayer;
				bool flag = localPlayer == null;
				if (!flag)
				{
					MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 15, SendOption.Reliable, -1);
					messageWriter.Write(start);
					messageWriter.Write(localPlayer.PlayerId);
					AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
					localPlayer.SetScanner(start, localPlayer.PlayerId);
				}
			}

			// Token: 0x060000E0 RID: 224 RVA: 0x0001B1F0 File Offset: 0x000193F0
			private void ForcePlayAnimation(byte animationType)
			{
				try
				{
					bool flag = PlayerControl.LocalPlayer == null;
					if (!flag)
					{
						PlayerControl.LocalPlayer.RpcPlayAnimation(animationType);
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ForcePlayAnimation error: " + ex.Message);
				}
			}

			// Token: 0x060000E1 RID: 225 RVA: 0x0001B250 File Offset: 0x00019450
			private void ExecuteEmergencyRPC()
			{
				PlayerControl localPlayer = PlayerControl.LocalPlayer;
				bool flag = localPlayer != null;
				if (flag)
				{
					MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 11, SendOption.Reliable, -1);
					messageWriter.Write(byte.MaxValue);
					AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
				}
			}

			// Token: 0x060000E2 RID: 226 RVA: 0x0001B2A0 File Offset: 0x000194A0
			private void KickAllFromVents()
			{
				try
				{
					bool flag = ShipStatus.Instance == null;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No map loaded - cannot kick from vents");
						bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Map not loaded!");
						}
					}
					else
					{
						bool flag3 = !AmongUsClient.Instance.AmHost;
						if (flag3)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to kick from vents");
							bool flag4 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag4)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host!");
							}
						}
						else
						{
							int num = 0;
							foreach (Vent vent in ShipStatus.Instance.AllVents)
							{
								VentilationSystem.Update(VentilationSystem.Operation.BootImpostors, vent.Id);
								num++;
							}
							SkidMenuPlugin.Logger.LogInfo("Kicked all players from " + num.ToString() + " vents");
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Kick all from vents error: " + ex.Message);
				}
			}

			// Token: 0x060000E3 RID: 227 RVA: 0x0001B438 File Offset: 0x00019638
			private void EndMeetingClientSided()
			{
				bool flag = MeetingHud.Instance != null;
				if (flag)
				{
					MeetingHud.Instance.Close();
					bool flag2 = Camera.main != null;
					if (flag2)
					{
						Component component = Camera.main.GetComponent("CameraFollow");
						bool flag3 = component != null;
						if (flag3)
						{
							try
							{
								Il2CppSystem.Reflection.FieldInfo field = component.GetIl2CppType().GetField("IsMeeting");
								bool flag4 = field != null;
								if (flag4)
								{
									field.SetValue(component, false);
								}
							}
							catch
							{
							}
						}
					}
				}
			}

			// Token: 0x060000E4 RID: 228 RVA: 0x0001B4D8 File Offset: 0x000196D8
			private void KickSelectedPlayer()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for kick");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to kick players");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot kick without host, Skill issue :-(");
							}
						}
						else
						{
							ClientData client = AmongUsClient.Instance.GetClient(SkidMenuPlugin.selectedHostKickTargetId);
							bool flag4 = client != null;
							if (flag4)
							{
								string playerName = client.PlayerName;
								AmongUsClient.Instance.KickPlayer(SkidMenuPlugin.selectedHostKickTargetId, false);
								SkidMenuPlugin.Logger.LogInfo("Kicked player: " + playerName);
								bool flag5 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
								if (flag5)
								{
									DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=orange>[Host]</color> Kicked " + playerName, true);
								}
							}
							else
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find client with ID: " + SkidMenuPlugin.selectedHostKickTargetId.ToString());
							}
							SkidMenuPlugin.selectedHostKickTargetId = -1;
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to kick player: " + ex.Message);
				}
			}

			// Token: 0x060000E5 RID: 229 RVA: 0x0001B67C File Offset: 0x0001987C
			private void BanSelectedPlayer()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for ban");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to ban players");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot ban without host, Skill issue :-(");
							}
						}
						else
						{
							ClientData client = AmongUsClient.Instance.GetClient(SkidMenuPlugin.selectedHostKickTargetId);
							bool flag4 = client != null;
							if (flag4)
							{
								string playerName = client.PlayerName;
								AmongUsClient.Instance.KickPlayer(SkidMenuPlugin.selectedHostKickTargetId, true);
								SkidMenuPlugin.Logger.LogInfo("Banned player: " + playerName);
								bool flag5 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
								if (flag5)
								{
									DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=red>[Host]</color> Banned " + playerName, true);
								}
							}
							else
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find client with ID: " + SkidMenuPlugin.selectedHostKickTargetId.ToString());
							}
							SkidMenuPlugin.selectedHostKickTargetId = -1;
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Failed to ban player: " + ex.Message);
				}
			}

			// Token: 0x060000E6 RID: 230 RVA: 0x0001B820 File Offset: 0x00019A20
			private void ForceEndGame()
			{
				try
				{
					bool flag = AmongUsClient.Instance != null && AmongUsClient.Instance.AmHost;
					if (flag)
					{
						bool flag2 = GameManager.Instance != null;
						if (flag2)
						{
							GameManager.Instance.RpcEndGame(GameOverReason.ImpostorsByKill, false);
							SkidMenuPlugin.Logger.LogInfo("Game ended successfully");
						}
						else
						{
							SkidMenuPlugin.Logger.LogWarning("GameManager.Instance is null - cannot end game");
						}
					}
					else
					{
						SkidMenuPlugin.Logger.LogWarning("Must be host to force end game");
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Force End Game error: " + ex.Message);
				}
			}

			// Token: 0x060000E7 RID: 231 RVA: 0x0001B8D8 File Offset: 0x00019AD8
			private void TeleportPlayerToVent()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for teleport");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to teleport players");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot TP without host!");
							}
						}
						else
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in this._cachedPlayers)
							{
								bool flag4 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
								if (flag4)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag5 = playerControl == null;
							if (flag5)
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find player to teleport");
							}
							else
							{
								string text = SkidMenuPlugin.ventNames[SkidMenuPlugin.selectedVentId];
								bool flag6 = playerControl.MyPhysics != null;
								if (flag6)
								{
									playerControl.MyPhysics.RpcBootFromVent(SkidMenuPlugin.selectedVentId);
								}
								string playerName = playerControl.Data.PlayerName;
								SkidMenuPlugin.Logger.LogInfo(string.Concat(new string[]
								{
									"Teleported ",
									playerName,
									" to ",
									text,
									" vent"
								}));
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Teleport to vent error: " + ex.Message);
				}
			}

			// Token: 0x060000E8 RID: 232 RVA: 0x0001BAB8 File Offset: 0x00019CB8
			private void TeleportAllToVent()
			{
				try
				{
					bool flag = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("Must be host to teleport players");
						bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot TP without host!");
						}
					}
					else
					{
						string text = SkidMenuPlugin.ventNames[SkidMenuPlugin.selectedVentId];
						int num = 0;
						foreach (PlayerControl playerControl in this._cachedPlayers)
						{
							bool flag3 = playerControl != null && playerControl.Data != null && !playerControl.Data.Disconnected;
							if (flag3)
							{
								bool flag4 = playerControl.MyPhysics != null;
								if (flag4)
								{
									playerControl.MyPhysics.RpcBootFromVent(SkidMenuPlugin.selectedVentId);
									num++;
								}
							}
						}
						SkidMenuPlugin.Logger.LogInfo(string.Concat(new string[]
						{
							"Teleported ",
							num.ToString(),
							" players (including you) to ",
							text,
							" vent"
						}));
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Teleport all to vent error: " + ex.Message);
				}
			}

			// Token: 0x060000E9 RID: 233 RVA: 0x0001BC48 File Offset: 0x00019E48
			private void ForceMeetingAsPlayer()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for force meeting");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to force meeting");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot force meeting without host!");
							}
						}
						else
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in this._cachedPlayers)
							{
								bool flag4 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
								if (flag4)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag5 = playerControl == null;
							if (flag5)
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find player to force meeting");
							}
							else
							{
								string playerName = playerControl.Data.PlayerName;
								bool flag6 = MeetingRoomManager.Instance != null;
								if (flag6)
								{
									MeetingRoomManager.Instance.AssignSelf(playerControl, null);
									playerControl.RpcStartMeeting(null);
									DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(playerControl);
								}
								SkidMenuPlugin.Logger.LogInfo("Forced meeting as " + playerName);
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Force meeting as player error: " + ex.Message);
				}
			}

			// Token: 0x060000EA RID: 234 RVA: 0x0001BE04 File Offset: 0x0001A004
			private void KillSelectedPlayer()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for kill");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to kill players");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot kill without host!");
							}
						}
						else
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in this._cachedPlayers)
							{
								bool flag4 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
								if (flag4)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag5 = playerControl == null;
							if (flag5)
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find player to kill");
							}
							else
							{
								string playerName = playerControl.Data.PlayerName;
								foreach (PlayerControl character in this._cachedPlayers)
								{
									MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 12, SendOption.None, AmongUsClient.Instance.GetClientIdFromCharacter(character));
									messageWriter.WriteNetObject(playerControl);
									messageWriter.Write(1);
									AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
								}
								SkidMenuPlugin.Logger.LogInfo("Killed player: " + playerName);
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Kill selected player error: " + ex.Message);
				}
			}

			// Token: 0x060000EB RID: 235 RVA: 0x0001BFFC File Offset: 0x0001A1FC
			private void TurnPlayerToGhost()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for turn to ghost");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to turn players to ghost");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot turn to ghost without host!");
							}
						}
						else
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in this._cachedPlayers)
							{
								bool flag4 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
								if (flag4)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag5 = playerControl == null;
							if (flag5)
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find player to turn to ghost");
							}
							else
							{
								string playerName = playerControl.Data.PlayerName;
								bool isDead = playerControl.Data.IsDead;
								if (isDead)
								{
									SkidMenuPlugin.Logger.LogWarning(playerName + " is already dead/ghost");
									bool flag6 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
									if (flag6)
									{
										DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " is already a ghost!");
									}
								}
								else
								{
									bool flag7 = playerControl.Data.Role != null && playerControl.Data.Role.IsImpostor;
									RoleTypes roleType;
									if (flag7)
									{
										roleType = RoleTypes.ImpostorGhost;
									}
									else
									{
										roleType = RoleTypes.CrewmateGhost;
									}
									playerControl.RpcSetRole(roleType, true);
									SkidMenuPlugin.Logger.LogInfo("Turned " + playerName + " into " + roleType.ToString());
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Turn to ghost error: " + ex.Message);
				}
			}

			// Token: 0x060000EC RID: 236 RVA: 0x0001C24C File Offset: 0x0001A44C
			private void RevivePlayer()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("No player selected for revive");
					}
					else
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							SkidMenuPlugin.Logger.LogWarning("Must be host to revive players");
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot revive without host!");
							}
						}
						else
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in this._cachedPlayers)
							{
								bool flag4 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
								if (flag4)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag5 = playerControl == null;
							if (flag5)
							{
								SkidMenuPlugin.Logger.LogWarning("Could not find player to revive");
							}
							else
							{
								string playerName = playerControl.Data.PlayerName;
								bool flag6 = !playerControl.Data.IsDead;
								if (flag6)
								{
									SkidMenuPlugin.Logger.LogWarning(playerName + " is already alive");
									bool flag7 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
									if (flag7)
									{
										DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(playerName + " is already alive!");
									}
								}
								else
								{
									bool flag8 = playerControl.Data.Role != null;
									RoleTypes roleType;
									if (flag8)
									{
										bool flag9 = playerControl.Data.Role.Role == RoleTypes.ImpostorGhost;
										if (flag9)
										{
											roleType = RoleTypes.Impostor;
										}
										else
										{
											roleType = RoleTypes.Crewmate;
										}
									}
									else
									{
										roleType = RoleTypes.Crewmate;
									}
									playerControl.RpcSetRole(roleType, true);
									SkidMenuPlugin.Logger.LogInfo("Revived " + playerName + " as " + roleType.ToString());
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Revive player error: " + ex.Message);
				}
			}

			// Token: 0x060000ED RID: 237 RVA: 0x0001C4AC File Offset: 0x0001A6AC
			private void KillAllPlayers()
			{
				try
				{
					bool flag = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
					if (flag)
					{
						SkidMenuPlugin.Logger.LogWarning("Must be host to kill all");
						bool flag2 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Cannot kill all without host!");
						}
					}
					else
					{
						int num = 0;
						foreach (PlayerControl playerControl in this._cachedPlayers)
						{
							bool flag3 = playerControl != null && playerControl.Data != null;
							if (flag3)
							{
								foreach (PlayerControl character in this._cachedPlayers)
								{
									MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, 12, SendOption.None, AmongUsClient.Instance.GetClientIdFromCharacter(character));
									messageWriter.WriteNetObject(playerControl);
									messageWriter.Write(1);
									AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
								}
								num++;
							}
						}
						SkidMenuPlugin.Logger.LogInfo("Killed all players: " + num.ToString());
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Kill all players error: " + ex.Message);
				}
			}

			// Token: 0x060000EE RID: 238 RVA: 0x0001C644 File Offset: 0x0001A844
			private void ForceShapeshiftPlayer()
			{
				try
				{
					bool flag = SkidMenuPlugin.selectedHostKickTargetId == -1;
					if (!flag)
					{
						bool flag2 = AmongUsClient.Instance == null || !AmongUsClient.Instance.AmHost;
						if (flag2)
						{
							bool flag3 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Notifier != null;
							if (flag3)
							{
								DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage("Must be host to force shapeshift!");
							}
						}
						else
						{
							PlayerControl playerControl = null;
							foreach (PlayerControl playerControl2 in this._cachedPlayers)
							{
								bool flag4 = playerControl2 != null && playerControl2.Data != null && playerControl2.Data.ClientId == SkidMenuPlugin.selectedHostKickTargetId;
								if (flag4)
								{
									playerControl = playerControl2;
									break;
								}
							}
							bool flag5 = playerControl == null;
							if (!flag5)
							{
								PlayerControl playerControl3 = null;
								foreach (PlayerControl playerControl4 in this._cachedPlayers)
								{
									bool flag6 = playerControl4 != null && playerControl4.Data != null && playerControl4.PlayerId != playerControl.PlayerId;
									if (flag6)
									{
										playerControl3 = playerControl4;
										break;
									}
								}
								bool flag7 = playerControl3 == null;
								if (flag7)
								{
									playerControl3 = PlayerControl.LocalPlayer;
								}
								MonoBehaviourExtensions.StartCoroutine(this, this.CoForceShapeshift(playerControl, playerControl3));
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ForceShapeshiftPlayer error: " + ex.Message);
				}
			}

			// Token: 0x060000EF RID: 239 RVA: 0x0001C800 File Offset: 0x0001AA00
			[HideFromIl2Cpp]
			private IEnumerator CoForceShapeshift(PlayerControl target, PlayerControl shapeshiftInto)
			{
				SkidMenuPlugin.SkidMenu.compiled_m_CoForceShapeshift_d_211 compiled_v_CoForceShapeshift_d__ = new SkidMenuPlugin.SkidMenu.compiled_m_CoForceShapeshift_d_211(0);
				compiled_v_CoForceShapeshift_d__.compiled_this = this;
				compiled_v_CoForceShapeshift_d__.target = target;
				compiled_v_CoForceShapeshift_d__.shapeshiftInto = shapeshiftInto;
				return compiled_v_CoForceShapeshift_d__;
			}

			// Token: 0x060000F0 RID: 240 RVA: 0x0001C820 File Offset: 0x0001AA20
			private void MonitorAndVotekickNewPlayers()
			{
				try
				{
					bool flag = VoteBanSystem.Instance == null;
					if (!flag)
					{
						foreach (PlayerControl playerControl in this._cachedPlayers)
						{
							bool flag2 = playerControl != null && !playerControl.AmOwner && playerControl.Data != null;
							if (flag2)
							{
								int clientId = playerControl.Data.ClientId;
								bool flag3 = !SkidMenuPlugin.votekickedPlayerIds.Contains(clientId);
								if (flag3)
								{
									for (int j = 0; j < 3; j++)
									{
										VoteBanSystem.Instance.CmdAddVote(clientId);
									}
									SkidMenuPlugin.votekickedPlayerIds.Add(clientId);
									string str = playerControl.Data.PlayerName ?? "Unknown";
									SkidMenuPlugin.Logger.LogInfo("Auto-votekicked new player: " + str);
									bool flag4 = DestroyableSingleton<HudManager>.Instance != null && DestroyableSingleton<HudManager>.Instance.Chat != null;
									if (flag4)
									{
										DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "<color=orange>[Auto-Votekick]</color> Votekicked " + str, true);
									}
								}
							}
						}
					}
				}
				catch (System.Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("Monitor votekick error: " + ex.Message);
				}
			}

			// Token: 0x060000F1 RID: 241 RVA: 0x0001C9A4 File Offset: 0x0001ABA4
			private void HandleResize()
			{
				Event current = Event.current;
				this.resizeHandleRect = new Rect(this.windowRect.x + this.windowRect.width - 15f, this.windowRect.y + this.windowRect.height - 15f, 15f, 15f);
				bool flag = current.type == EventType.MouseDown && current.button == 0;
				if (flag)
				{
					bool flag2 = this.resizeHandleRect.Contains(current.mousePosition);
					if (flag2)
					{
						this.isResizing = true;
						this.resizeStart = current.mousePosition;
						current.Use();
					}
				}
				bool flag3 = this.isResizing;
				if (flag3)
				{
					bool flag4 = current.type == EventType.MouseDrag;
					if (flag4)
					{
						Vector2 vector = current.mousePosition - this.resizeStart;
						float value = this.windowRect.width + vector.x;
						float value2 = this.windowRect.height + vector.y;
						this.windowRect.width = Mathf.Clamp(value, 280f, 800f);
						this.windowRect.height = Mathf.Clamp(value2, 400f, 1000f);
						this.resizeStart = current.mousePosition;
						current.Use();
					}
					bool flag5 = current.type == EventType.MouseUp && current.button == 0;
					if (flag5)
					{
						this.isResizing = false;
						current.Use();
					}
				}
				bool flag6 = this.resizeHandleRect.Contains(Event.current.mousePosition) || this.isResizing;
				if (flag6)
				{
				}
			}

			// Token: 0x060000F2 RID: 242 RVA: 0x0001CB54 File Offset: 0x0001AD54
			private void DrawResizeHandle()
			{
				GUI.backgroundColor = (this.isResizing ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.8f));
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, this.isResizing ? new Color(1f, 1f, 0f, 0.5f) : new Color(0.3f, 0.3f, 0.3f, 0.5f));
				GUI.Box(this.resizeHandleRect, "", guistyle);
				GUI.contentColor = Color.white;
				GUIStyle guistyle2 = new GUIStyle(GUI.skin.label);
				guistyle2.fontSize = 12;
				guistyle2.fontStyle = FontStyle.Bold;
				guistyle2.alignment = TextAnchor.MiddleCenter;
				GUI.Label(this.resizeHandleRect, "?", guistyle2);
				GUI.contentColor = Color.white;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x04000114 RID: 276
			private Rect windowRect = new Rect(100f, 100f, 400f, 645f);

			// Token: 0x04000115 RID: 277
			private int windowId = 666;

			// Token: 0x04000116 RID: 278
			private bool isResizing = false;

			// Token: 0x04000117 RID: 279
			private Vector2 resizeStart;

			// Token: 0x04000118 RID: 280
			private Rect resizeHandleRect;

			// Token: 0x04000119 RID: 281
			private const float MIN_WIDTH = 280f;

			// Token: 0x0400011A RID: 282
			private const float MIN_HEIGHT = 400f;

			// Token: 0x0400011B RID: 283
			private const float MAX_WIDTH = 800f;

			// Token: 0x0400011C RID: 284
			private const float MAX_HEIGHT = 1000f;

			// Token: 0x0400011D RID: 285
			private const float RESIZE_HANDLE_SIZE = 15f;

			// Token: 0x0400011E RID: 286
			private Vector2 scrollPosition = Vector2.zero;

			// Token: 0x0400011F RID: 287
			private Vector2 aboutScrollPosition = Vector2.zero;

			// Token: 0x04000120 RID: 288
			private Vector2 destroyScrollPosition = Vector2.zero;

			// Token: 0x04000121 RID: 289
			private string searchQuery = "";

			// Token: 0x04000122 RID: 290
			private bool searchBoxFocused = false;

			// Token: 0x04000123 RID: 291
			private int searchCursorPosition = 0;

			// Token: 0x04000124 RID: 292
			private bool searchCursorVisible = true;

			// Token: 0x04000125 RID: 293
			private float searchCursorBlinkTime = 0f;

			// Token: 0x04000126 RID: 294
			private const int MAX_SEARCH_LENGTH = 50;

			// Token: 0x04000127 RID: 295
			private System.Collections.Generic.List<SkidMenuPlugin.SkidMenu.SearchResult> searchResults = new System.Collections.Generic.List<SkidMenuPlugin.SkidMenu.SearchResult>();

			// Token: 0x04000128 RID: 296
			private Vector2 selfScrollPosition = Vector2.zero;

			// Token: 0x04000129 RID: 297
			private Vector2 votekickScrollPosition = Vector2.zero;

			// Token: 0x0400012A RID: 298
			private Vector2 hostScrollPosition = Vector2.zero;

			// Token: 0x0400012B RID: 299
			private Vector2 sabotageScrollPosition = Vector2.zero;

			// Token: 0x0400012C RID: 300
			private Vector2 chatScrollPosition = Vector2.zero;

			// Token: 0x0400012D RID: 301
			private Vector2 ventDropdownScrollPosition = Vector2.zero;

			// Token: 0x0400012E RID: 302
			private bool showSpoofMenuDropdown = false;

			// Token: 0x0400012F RID: 303
			private Vector2 spoofMenuDropdownScrollPosition = Vector2.zero;

			// Token: 0x04000130 RID: 304
			private bool _lastVotekickAllState = false;

			// Token: 0x04000131 RID: 305
			private string activeHostSection = "Utils";

			// Token: 0x04000132 RID: 306
			private Vector2 hostSettingsScrollPosition = Vector2.zero;

			// Token: 0x04000133 RID: 307
			private bool settingsLoaded = false;

			// Token: 0x04000134 RID: 308
			private float _settingsSyncTimer = 0f;

			// Token: 0x04000135 RID: 309
			private GUIStyle _styleSettingsNormal;

			// Token: 0x04000136 RID: 310
			private GUIStyle _styleSettingsFocused;

			// Token: 0x04000137 RID: 311
			private int selectedSettingsMapId = 0;

			// Token: 0x04000138 RID: 312
			private string focusedSettingKey = "";

			// Token: 0x04000139 RID: 313
			private string settingInputBuffer = "";

			// Token: 0x0400013A RID: 314
			private bool settingCursorVisible = true;

			// Token: 0x0400013B RID: 315
			private float settingCursorBlink = 0f;

			// Token: 0x0400013C RID: 316
			private bool s_confirmEjects;

			// Token: 0x0400013D RID: 317
			private bool s_anonVotes;

			// Token: 0x0400013E RID: 318
			private bool s_visualTasks;

			// Token: 0x0400013F RID: 319
			private bool s_protectVisible;

			// Token: 0x04000140 RID: 320
			private bool s_shapeshiftEvidence;

			// Token: 0x04000141 RID: 321
			private float s_emergencyMeetings;

			// Token: 0x04000142 RID: 322
			private float s_emergencyCooldown;

			// Token: 0x04000143 RID: 323
			private float s_discussionTime;

			// Token: 0x04000144 RID: 324
			private float s_votingTime;

			// Token: 0x04000145 RID: 325
			private float s_killDistance;

			// Token: 0x04000146 RID: 326
			private float s_commonTasks;

			// Token: 0x04000147 RID: 327
			private float s_shortTasks;

			// Token: 0x04000148 RID: 328
			private float s_longTasks;

			// Token: 0x04000149 RID: 329
			private float s_taskBarMode;

			// Token: 0x0400014A RID: 330
			private float s_playerSpeed;

			// Token: 0x0400014B RID: 331
			private float s_crewVision;

			// Token: 0x0400014C RID: 332
			private float s_impVision;

			// Token: 0x0400014D RID: 333
			private float s_killCooldown;

			// Token: 0x0400014E RID: 334
			private float s_vitalsCooldown;

			// Token: 0x0400014F RID: 335
			private float s_batteryDuration;

			// Token: 0x04000150 RID: 336
			private float s_ventCooldown;

			// Token: 0x04000151 RID: 337
			private float s_ventDuration;

			// Token: 0x04000152 RID: 338
			private float s_protectCooldown;

			// Token: 0x04000153 RID: 339
			private float s_protectDuration;

			// Token: 0x04000154 RID: 340
			private float s_shapeshiftDuration;

			// Token: 0x04000155 RID: 341
			private float s_shapeshiftCooldown;

			// Token: 0x04000156 RID: 342
			private float s_alertDuration;

			// Token: 0x04000157 RID: 343
			private float s_trackerDuration;

			// Token: 0x04000158 RID: 344
			private float s_trackerCooldown;

			// Token: 0x04000159 RID: 345
			private float s_trackerDelay;

			// Token: 0x0400015A RID: 346
			private float s_phantomDuration;

			// Token: 0x0400015B RID: 347
			private float s_phantomCooldown;

			// Token: 0x0400015C RID: 348
			private string targetTab = "About";

			// Token: 0x0400015D RID: 349
			private float tabTransitionProgress = 1f;

			// Token: 0x0400015E RID: 350
			private const float TAB_TRANSITION_SPEED = 8f;

			// Token: 0x0400015F RID: 351
			private Vector2 contentOffset = Vector2.zero;

			// Token: 0x04000160 RID: 352
			private System.Collections.Generic.Dictionary<string, float> toggleAnimations = new System.Collections.Generic.Dictionary<string, float>();

			// Token: 0x04000161 RID: 353
			private const float TOGGLE_ANIMATION_SPEED = 12f;

			// Token: 0x04000162 RID: 354
			private System.Collections.Generic.Dictionary<string, float> buttonScales = new System.Collections.Generic.Dictionary<string, float>();

			// Token: 0x04000163 RID: 355
			private System.Collections.Generic.Dictionary<string, float> buttonPressTime = new System.Collections.Generic.Dictionary<string, float>();

			// Token: 0x04000164 RID: 356
			private string activeDestructSection = "Overload";

			// Token: 0x04000165 RID: 357
			private static Vector2 originalPosition = Vector2.zero;

			// Token: 0x04000166 RID: 358
			private string chatMessage = "";

			// Token: 0x04000167 RID: 359
			private const int MAX_CHAT_LENGTH = 100;

			// Token: 0x04000168 RID: 360
			public static bool SpamChatEnabled = false;

			// Token: 0x04000169 RID: 361
			private int chatSpamDelay = 0;

			// Token: 0x0400016A RID: 362
			private int cursorPosition = 0;

			// Token: 0x0400016B RID: 363
			private bool showCursor = true;

			// Token: 0x0400016C RID: 364
			private float cursorBlinkTime = 0f;

			// Token: 0x0400016D RID: 365
			private bool chatBoxFocused = false;

			// Token: 0x0400016E RID: 366
			private bool settingsBoxFocused = false;

			// Token: 0x0400016F RID: 367
			private float backspaceHoldTime = 0f;

			// Token: 0x04000170 RID: 368
			private float backspaceRepeatDelay = 0f;

			// Token: 0x04000171 RID: 369
			private float lastChatSendTime = 0f;

			// Token: 0x04000172 RID: 370
			private const float CHAT_COOLDOWN = 3f;

			// Token: 0x04000173 RID: 371
			private Vector2 whisperPlayerScrollPosition = Vector2.zero;

			// Token: 0x04000174 RID: 372
			private int selectedWhisperTargetId = -1;

			// Token: 0x04000175 RID: 373
			private string blacklistInput = "";

			// Token: 0x04000176 RID: 374
			private bool blacklistInputFocused = false;

			// Token: 0x04000177 RID: 375
			private int blacklistCursorPos = 0;

			// Token: 0x04000178 RID: 376
			private bool blacklistCursorVisible = true;

			// Token: 0x04000179 RID: 377
			private float blacklistCursorBlink = 0f;

			// Token: 0x0400017A RID: 378
			private const int MAX_FRIEND_CODE_LENGTH = 20;

			// Token: 0x0400017B RID: 379
			private Vector2 blacklistScrollPosition = Vector2.zero;

			// Token: 0x0400017C RID: 380
			private string blacklistAddedMessage = "";

			// Token: 0x0400017D RID: 381
			private float blacklistMessageTimer = 0f;

			// Token: 0x0400017E RID: 382
			private bool showVentDropdown = false;

			// Token: 0x0400017F RID: 383
			private Vector2 destroyPlayerScrollPosition = Vector2.zero;

			// Token: 0x04000180 RID: 384
			private Vector2 hostKickPlayerScrollPosition = Vector2.zero;

			// Token: 0x04000181 RID: 385
			private int targetedOverloadDelay = 0;

			// Token: 0x04000182 RID: 386
			private int breakCounterDelay = 0;

			// Token: 0x04000183 RID: 387
			private int exileMeDelay = 0;

			// Token: 0x04000184 RID: 388
			private int repairSabotagesDelay = 0;

			// Token: 0x04000185 RID: 389
			private int killAllDelay = 0;

			// Token: 0x04000186 RID: 390
			private int overloadMethod4Delay = 0;

			// Token: 0x04000187 RID: 391
			private int lagEveryoneDelay = 0;

			// Token: 0x04000188 RID: 392
			private string[] mapNames = new string[]
			{
				"The Skeld",
				"MIRA HQ",
				"Polus",
				"Reverse Skeld",
				"Airship"
			};

			// Token: 0x04000189 RID: 393
			private PlayerControl[] _cachedPlayers = new PlayerControl[0];

			// Token: 0x0400018A RID: 394
			private float _playerCacheTimer = 0f;

			// Token: 0x0400018B RID: 395
			private static readonly System.Collections.Generic.Dictionary<Color, Texture2D> _texCache = new System.Collections.Generic.Dictionary<Color, Texture2D>();

			// Token: 0x0200005C RID: 92
			private class SearchResult
			{
				// Token: 0x040001B8 RID: 440
				public string featureName;

				// Token: 0x040001B9 RID: 441
				public string tabName;

				// Token: 0x040001BA RID: 442
				public string description;
			}
		}

		// Token: 0x02000046 RID: 70
		[HarmonyPatch(typeof(EngineerRole), "FixedUpdate")]
		public static class EngineerRole_FixedUpdate
		{
			// Token: 0x060000F5 RID: 245 RVA: 0x0001CC98 File Offset: 0x0001AE98
			[HarmonyPostfix]
			public static void Postfix(EngineerRole __instance)
			{
				bool flag = __instance.Player != PlayerControl.LocalPlayer;
				if (!flag)
				{
					bool endlessVentTime = SkidMenuPlugin.EndlessVentTime;
					if (endlessVentTime)
					{
						__instance.inVentTimeRemaining = float.MaxValue;
					}
					else
					{
						bool flag2 = __instance.inVentTimeRemaining > __instance.GetCooldown();
						if (flag2)
						{
							__instance.inVentTimeRemaining = __instance.GetCooldown();
						}
					}
					bool flag3 = SkidMenuPlugin.NoVentCooldown && __instance.cooldownSecondsRemaining > 0f;
					if (flag3)
					{
						__instance.cooldownSecondsRemaining = 0f;
						HudManager instance = DestroyableSingleton<HudManager>.Instance;
						bool flag4 = ((instance != null) ? instance.AbilityButton : null) != null;
						if (flag4)
						{
							DestroyableSingleton<HudManager>.Instance.AbilityButton.ResetCoolDown();
							DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCooldownFill(0f);
						}
					}
				}
			}
		}

		// Token: 0x02000047 RID: 71
		[HarmonyPatch(typeof(ScientistRole), "Update")]
		public static class ScientistRole_Update
		{
			// Token: 0x060000F6 RID: 246 RVA: 0x0001CD68 File Offset: 0x0001AF68
			[HarmonyPostfix]
			public static void Postfix(ScientistRole __instance)
			{
				bool flag = __instance.Player != PlayerControl.LocalPlayer;
				if (!flag)
				{
					bool noVitalsCooldown = SkidMenuPlugin.NoVitalsCooldown;
					if (noVitalsCooldown)
					{
						__instance.currentCooldown = 0f;
					}
					bool endlessBattery = SkidMenuPlugin.EndlessBattery;
					if (endlessBattery)
					{
						__instance.currentCharge = float.MaxValue;
					}
					else
					{
						bool flag2 = __instance.currentCharge > __instance.RoleCooldownValue;
						if (flag2)
						{
							__instance.currentCharge = __instance.RoleCooldownValue;
						}
					}
				}
			}
		}

		// Token: 0x02000048 RID: 72
		[HarmonyPatch(typeof(TrackerRole), "FixedUpdate")]
		public static class TrackerRole_FixedUpdate
		{
			// Token: 0x060000F7 RID: 247 RVA: 0x0001CDE0 File Offset: 0x0001AFE0
			[HarmonyPostfix]
			public static void Postfix(TrackerRole __instance)
			{
				bool flag = __instance.Player != PlayerControl.LocalPlayer;
				if (!flag)
				{
					bool noTrackingCooldown = SkidMenuPlugin.NoTrackingCooldown;
					if (noTrackingCooldown)
					{
						__instance.cooldownSecondsRemaining = 0f;
						__instance.delaySecondsRemaining = 0f;
						HudManager instance = DestroyableSingleton<HudManager>.Instance;
						bool flag2 = ((instance != null) ? instance.AbilityButton : null) != null;
						if (flag2)
						{
							DestroyableSingleton<HudManager>.Instance.AbilityButton.ResetCoolDown();
							DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCooldownFill(0f);
						}
					}
					bool flag3 = SkidMenuPlugin.NoTrackingDelay && MapBehaviour.Instance != null;
					if (flag3)
					{
						MapBehaviour.Instance.trackedPointDelayTime = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDelay);
					}
					bool endlessTracking = SkidMenuPlugin.EndlessTracking;
					if (endlessTracking)
					{
						__instance.durationSecondsRemaining = float.MaxValue;
					}
					else
					{
						bool flag4 = __instance.durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
						if (flag4)
						{
							__instance.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
						}
					}
				}
			}
		}

		// Token: 0x02000049 RID: 73
		[HarmonyPatch(typeof(ShapeshifterRole), "FixedUpdate")]
		public static class ShapeshifterRole_FixedUpdate
		{
			// Token: 0x060000F8 RID: 248 RVA: 0x0001CF00 File Offset: 0x0001B100
			[HarmonyPostfix]
			public static void Postfix(ShapeshifterRole __instance)
			{
				try
				{
					bool flag = __instance.Player != PlayerControl.LocalPlayer;
					if (!flag)
					{
						bool endlessShapeshiftDuration = SkidMenuPlugin.EndlessShapeshiftDuration;
						if (endlessShapeshiftDuration)
						{
							__instance.durationSecondsRemaining = float.MaxValue;
						}
						else
						{
							bool flag2 = __instance.durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
							if (flag2)
							{
								__instance.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0200004A RID: 74
		[HarmonyPatch(typeof(PlayerControl), "CmdCheckShapeshift")]
		public static class PlayerControl_CmdCheckShapeshift
		{
			// Token: 0x060000F9 RID: 249 RVA: 0x0001CF94 File Offset: 0x0001B194
			[HarmonyPrefix]
			public static void Prefix(ref bool shouldAnimate)
			{
				bool flag = shouldAnimate && SkidMenuPlugin.NoShapeshiftAnimation;
				if (flag)
				{
					shouldAnimate = false;
				}
			}
		}

		// Token: 0x0200004B RID: 75
		[HarmonyPatch(typeof(PlayerControl), "CmdCheckRevertShapeshift")]
		public static class PlayerControl_CmdCheckRevertShapeshift
		{
			// Token: 0x060000FA RID: 250 RVA: 0x0001CFB8 File Offset: 0x0001B1B8
			[HarmonyPrefix]
			public static void Prefix(ref bool shouldAnimate)
			{
				bool flag = shouldAnimate && SkidMenuPlugin.NoShapeshiftAnimation;
				if (flag)
				{
					shouldAnimate = false;
				}
			}
		}

		// Token: 0x0200004C RID: 76
		[HarmonyPatch(typeof(ImpostorRole), "FindClosestTarget")]
		public static class ImpostorRole_FindClosestTarget
		{
			// Token: 0x060000FB RID: 251 RVA: 0x0001CFDC File Offset: 0x0001B1DC
			[HarmonyPrefix]
			public static bool Prefix(ImpostorRole __instance, ref PlayerControl __result)
			{
				bool flag = !SkidMenuPlugin.UnlimitedKillRange;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					try
					{
						System.Collections.Generic.List<PlayerControl> list = new System.Collections.Generic.List<PlayerControl>();
						Il2CppSystem.Collections.Generic.List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
						foreach (NetworkedPlayerInfo networkedPlayerInfo in allPlayers)
						{
							PlayerControl @object = networkedPlayerInfo.Object;
							bool flag2 = @object != null;
							if (flag2)
							{
								list.Add(@object);
							}
						}
						list = (from target in list
						orderby Vector2.Distance(target.GetTruePosition(), PlayerControl.LocalPlayer.GetTruePosition())
						select target).ToList<PlayerControl>();
						System.Collections.Generic.List<PlayerControl> list2 = (from player in list
						where player != null && __instance.IsValidTarget(player.Data) && player.Collider.enabled
						select player).ToList<PlayerControl>();
						bool flag3 = list2.Count > 0;
						if (flag3)
						{
							__result = list2[0];
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("UnlimitedKillRange error: " + ex.Message);
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x0200004D RID: 77
		[HarmonyPatch(typeof(DetectiveRole), "FindClosestTarget")]
		public static class DetectiveRole_FindClosestTarget
		{
			// Token: 0x060000FC RID: 252 RVA: 0x0001D0FC File Offset: 0x0001B2FC
			[HarmonyPrefix]
			public static bool Prefix(DetectiveRole __instance, ref PlayerControl __result)
			{
				bool flag = !SkidMenuPlugin.UnlimitedInterrogateRange;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					try
					{
						System.Collections.Generic.List<PlayerControl> list = new System.Collections.Generic.List<PlayerControl>();
						Il2CppSystem.Collections.Generic.List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
						foreach (NetworkedPlayerInfo networkedPlayerInfo in allPlayers)
						{
							PlayerControl @object = networkedPlayerInfo.Object;
							bool flag2 = @object != null;
							if (flag2)
							{
								list.Add(@object);
							}
						}
						list = (from target in list
						orderby Vector2.Distance(target.GetTruePosition(), PlayerControl.LocalPlayer.GetTruePosition())
						select target).ToList<PlayerControl>();
						System.Collections.Generic.List<PlayerControl> list2 = (from player in list
						where player != null && __instance.IsValidTarget(player.Data) && player.Collider.enabled
						select player).ToList<PlayerControl>();
						bool flag3 = list2.Count > 0;
						if (flag3)
						{
							__result = list2[0];
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("UnlimitedInterrogateRange error: " + ex.Message);
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x0200004E RID: 78
		[HarmonyPatch(typeof(ChatBubble), "SetText")]
		public static class DarkMode_ChatBubblePatch
		{
			// Token: 0x060000FD RID: 253 RVA: 0x0001D21C File Offset: 0x0001B41C
			[HarmonyPrefix]
			public static void Prefix(ChatBubble __instance, ref string chatText)
			{
				bool flag = !SkidMenuPlugin.DarkModeEnabled;
				if (!flag)
				{
					try
					{
						Transform transform = __instance.transform.Find("Background");
						SpriteRenderer spriteRenderer = (transform != null) ? transform.GetComponent<SpriteRenderer>() : null;
						bool flag2 = spriteRenderer != null;
						if (flag2)
						{
							spriteRenderer.color = new Color(0.15f, 0.15f, 0.15f, 1f);
						}
						bool flag3 = !chatText.Contains("¦") && !chatText.Contains("_") && !chatText.Contains("¦") && !chatText.Contains("¦") && !chatText.Contains("¦");
						if (flag3)
						{
							chatText = "<color=#FFFFFF>" + chatText.TrimEnd(new char[1]) + "</color>";
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("DarkMode ChatBubble error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x0200004F RID: 79
		[HarmonyPatch(typeof(ChatController), "Update")]
		public static class DarkMode_ChatControllerPatch
		{
			// Token: 0x060000FE RID: 254 RVA: 0x0001D32C File Offset: 0x0001B52C
			[HarmonyPostfix]
			public static void Postfix(ChatController __instance)
			{
				bool flag = !SkidMenuPlugin.DarkModeEnabled;
				if (!flag)
				{
					try
					{
						FreeChatInputField freeChatField = __instance.freeChatField;
						bool flag2 = ((freeChatField != null) ? freeChatField.background : null) != null;
						if (flag2)
						{
							__instance.freeChatField.background.color = new Color32(40, 40, 40, byte.MaxValue);
						}
						FreeChatInputField freeChatField2 = __instance.freeChatField;
						UnityEngine.Object x;
						if (freeChatField2 == null)
						{
							x = null;
						}
						else
						{
							TextBoxTMP textArea = freeChatField2.textArea;
							x = ((textArea != null) ? textArea.outputText : null);
						}
						bool flag3 = x != null;
						if (flag3)
						{
							__instance.freeChatField.textArea.outputText.color = Color.white;
						}
						QuickChatPreviewField quickChatField = __instance.quickChatField;
						bool flag4 = ((quickChatField != null) ? quickChatField.background : null) != null;
						if (flag4)
						{
							__instance.quickChatField.background.color = new Color32(40, 40, 40, byte.MaxValue);
						}
						QuickChatPreviewField quickChatField2 = __instance.quickChatField;
						bool flag5 = ((quickChatField2 != null) ? quickChatField2.text : null) != null;
						if (flag5)
						{
							__instance.quickChatField.text.color = Color.white;
						}
					}
					catch (System.Exception ex)
					{
						SkidMenuPlugin.Logger.LogError("DarkMode ChatController error: " + ex.Message);
					}
				}
			}
		}
	}
}
