using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using InnerNet;
using UnityEngine;

namespace SkidMenu
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(ChatController), "SendChat")]
	public static class HostCommandPatch
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00004A44 File Offset: 0x00002C44
		public static bool Prefix(ChatController __instance)
		{
			bool flag = !AmongUsClient.Instance.AmHost;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				string text = __instance.freeChatField.textArea.text.Trim();
				bool flag2 = !text.StartsWith("s!", StringComparison.OrdinalIgnoreCase);
				if (flag2)
				{
					result = true;
				}
				else
				{
					string[] array = text.Substring(2).Trim().Split(new char[]
					{
						' '
					}, StringSplitOptions.RemoveEmptyEntries);
					bool flag3 = array.Length == 0;
					if (flag3)
					{
						result = true;
					}
					else
					{
						string a = array[0].ToLowerInvariant();
						bool flag4 = a == "id";
						if (flag4)
						{
							string text2 = "=== Player IDs ===\n";
							foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls)
							{
								bool flag5 = playerControl != null && playerControl.Data != null;
								if (flag5)
								{
									string text3 = playerControl.Data.PlayerName ?? "Unknown";
									text2 = string.Concat(new string[]
									{
										text2,
										"ID ",
										playerControl.PlayerId.ToString(),
										" - ",
										text3,
										"\n"
									});
								}
							}
							DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, text2, true);
							result = false;
						}
						else
						{
							bool flag6 = a == "commands";
							if (flag6)
							{
								string chatText = "=== Available Commands ===\ns! id - Show all player IDs\ns! kick compiled_v_id_ compiled_v_reason_ - Kick a player\ns! ban compiled_v_id_ compiled_v_reason_ - Ban a player\ns! warn compiled_v_id_ compiled_v_reason_ - Warn a player";
								DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, chatText, true);
								result = false;
							}
							else
							{
								bool flag7 = a == "warn";
								if (flag7)
								{
									bool flag8 = array.Length < 2;
									if (flag8)
									{
										DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Usage: s! warn compiled_v_player_id_ compiled_v_reason_", true);
										result = false;
									}
									else
									{
										byte b;
										bool flag9 = !byte.TryParse(array[1], out b);
										if (flag9)
										{
											DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Invalid ID: " + array[1], true);
											result = false;
										}
										else
										{
											PlayerControl playerControl2 = null;
											foreach (PlayerControl playerControl3 in PlayerControl.AllPlayerControls)
											{
												bool flag10 = playerControl3 != null && playerControl3.PlayerId == b;
												if (flag10)
												{
													playerControl2 = playerControl3;
													break;
												}
											}
											bool flag11 = playerControl2 == null;
											if (flag11)
											{
												DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Format("No player with ID {0}.", b), true);
												result = false;
											}
											else
											{
												string str = (array.Length >= 3) ? string.Join(" ", array, 2, array.Length - 2) : "No reason provided";
												NetworkedPlayerInfo data = playerControl2.Data;
												string str2 = ((data != null) ? data.PlayerName : null) ?? playerControl2.name;
												string text4 = str2 + " was warned\nReason: " + str;
												PlayerControl localPlayer = PlayerControl.LocalPlayer;
												string playerName = localPlayer.Data.PlayerName;
												MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 6, SendOption.Reliable, -1);
												messageWriter.Write(localPlayer.Data.NetId);
												messageWriter.Write("[Host]");
												AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
												MessageWriter messageWriter2 = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 13, SendOption.Reliable, -1);
												messageWriter2.Write(text4);
												AmongUsClient.Instance.FinishRpcImmediately(messageWriter2);
												MessageWriter messageWriter3 = AmongUsClient.Instance.StartRpcImmediately(localPlayer.NetId, 6, SendOption.Reliable, -1);
												messageWriter3.Write(localPlayer.Data.NetId);
												messageWriter3.Write(playerName);
												AmongUsClient.Instance.FinishRpcImmediately(messageWriter3);
												DestroyableSingleton<HudManager>.Instance.Chat.AddChat(localPlayer, text4, true);
												result = false;
											}
										}
									}
								}
								else
								{
									bool flag12 = a != "ban" && a != "kick";
									if (flag12)
									{
										result = true;
									}
									else
									{
										bool flag13 = a == "ban";
										bool flag14 = array.Length < 2;
										if (flag14)
										{
											DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, flag13 ? "Usage: s! ban compiled_v_player_id_ compiled_v_reason_" : "Usage: s! kick compiled_v_player_id_ compiled_v_reason_", true);
											result = false;
										}
										else
										{
											byte b2;
											bool flag15 = !byte.TryParse(array[1], out b2);
											if (flag15)
											{
												DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Invalid ID: " + array[1], true);
												result = false;
											}
											else
											{
												PlayerControl playerControl4 = null;
												foreach (PlayerControl playerControl5 in PlayerControl.AllPlayerControls)
												{
													bool flag16 = playerControl5 != null && playerControl5.PlayerId == b2;
													if (flag16)
													{
														playerControl4 = playerControl5;
														break;
													}
												}
												bool flag17 = playerControl4 == null;
												if (flag17)
												{
													DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, string.Format("No player with ID {0}.", b2), true);
													result = false;
												}
												else
												{
													bool flag18 = playerControl4.PlayerId == PlayerControl.LocalPlayer.PlayerId;
													if (flag18)
													{
														DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "You can't ban/kick yourself.", true);
														result = false;
													}
													else
													{
														string text5 = (array.Length >= 3) ? string.Join(" ", array, 2, array.Length - 2) : "No reason provided";
														NetworkedPlayerInfo data2 = playerControl4.Data;
														string text6 = ((data2 != null) ? data2.PlayerName : null) ?? playerControl4.name;
														ClientData client = AmongUsClient.Instance.GetClient(playerControl4.OwnerId);
														bool flag19 = client == null;
														if (flag19)
														{
															DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "Client not found.", true);
															result = false;
														}
														else
														{
															string text7 = flag13 ? "banned" : "kicked";
															string text8 = string.Concat(new string[]
															{
																text6,
																" was ",
																text7,
																"\nReason: ",
																text5
															});
															PlayerControl localPlayer2 = PlayerControl.LocalPlayer;
															string playerName2 = localPlayer2.Data.PlayerName;
															MessageWriter messageWriter4 = AmongUsClient.Instance.StartRpcImmediately(localPlayer2.NetId, 6, SendOption.Reliable, -1);
															messageWriter4.Write(localPlayer2.Data.NetId);
															messageWriter4.Write("[Host]");
															AmongUsClient.Instance.FinishRpcImmediately(messageWriter4);
															MessageWriter messageWriter5 = AmongUsClient.Instance.StartRpcImmediately(localPlayer2.NetId, 13, SendOption.Reliable, -1);
															messageWriter5.Write(text8);
															AmongUsClient.Instance.FinishRpcImmediately(messageWriter5);
															MessageWriter messageWriter6 = AmongUsClient.Instance.StartRpcImmediately(localPlayer2.NetId, 6, SendOption.Reliable, -1);
															messageWriter6.Write(localPlayer2.Data.NetId);
															messageWriter6.Write(playerName2);
															AmongUsClient.Instance.FinishRpcImmediately(messageWriter6);
															DestroyableSingleton<HudManager>.Instance.Chat.AddChat(localPlayer2, text8, true);
															AmongUsClient.Instance.KickPlayer(client.Id, flag13);
															result = false;
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
			return result;
		}

		// Token: 0x02000052 RID: 82
		public class RolesUI : MonoBehaviour
		{
			// Token: 0x06000103 RID: 259 RVA: 0x0001D4DC File Offset: 0x0001B6DC
			public RolesUI(IntPtr ptr) : base(ptr)
			{
			}

			// Token: 0x06000104 RID: 260 RVA: 0x0001D53C File Offset: 0x0001B73C
			private void Update()
			{
				bool flag = Input.GetKeyDown(KeyCode.Escape) && SkidMenuPlugin.ShowForceRolesMenu;
				if (flag)
				{
					SkidMenuPlugin.ShowForceRolesMenu = false;
					SkidMenuPlugin.showRoleDropdown = false;
				}
			}

			// Token: 0x06000105 RID: 261 RVA: 0x0001D570 File Offset: 0x0001B770
			private void OnGUI()
			{
				bool flag = !SkidMenuPlugin.ShowForceRolesMenu;
				if (!flag)
				{
					this.HandleResize();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					this.windowRect = GUI.Window(this.windowId, this.windowRect, new Action<int>(this.DrawWindowContents), "");
					this.DrawResizeHandle();
					this.DrawCloseButton();
				}
			}

			// Token: 0x06000106 RID: 262 RVA: 0x0001D5DC File Offset: 0x0001B7DC
			private void DrawWindowContents(int id)
			{
				GUILayout.BeginArea(new Rect(10f, 10f, this.windowRect.width - 20f, 30f));
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label("Force Roles Menu", new GUIStyle(GUI.skin.label)
				{
					fontSize = 16,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(10f, 50f, this.windowRect.width - 20f, this.windowRect.height - 90f));
				bool flag = !AmongUsClient.Instance.AmHost;
				if (flag)
				{
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.yellow;
					GUILayout.Label("?? Must be Host to Force Roles", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 12,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				else
				{
					this.DrawRoleAssignments();
				}
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(10f, this.windowRect.height - 55f, this.windowRect.width - 20f, 50f));
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
				GUI.contentColor = Color.cyan;
				GUILayout.Label("\ud83d\udca1 Roles will be assigned when the game starts", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : Color.gray);
				GUILayout.Label("Press ESC to close", new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontSize = 9
				}, null);
				GUI.contentColor = Color.white;
				GUILayout.EndArea();
				bool flag2 = !this.isResizing && !this.resizeHandleRect.Contains(Event.current.mousePosition) && !this.closeButtonRect.Contains(Event.current.mousePosition);
				if (flag2)
				{
					Rect position = new Rect(0f, 0f, this.windowRect.width - 32f - 10f, this.windowRect.height);
					GUI.DragWindow(position);
				}
			}

			// Token: 0x06000107 RID: 263 RVA: 0x0001D8AC File Offset: 0x0001BAAC
			private void DrawCloseButton()
			{
				this.closeButtonRect = new Rect(this.windowRect.x + this.windowRect.width - 32f - 5f, this.windowRect.y + 5f, 32f, 32f);
				Event current = Event.current;
				Color color = this.closeButtonRect.Contains(current.mousePosition) ? new Color(0.8f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, color);
				GUI.backgroundColor = color;
				GUI.Box(this.closeButtonRect, "", guistyle);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 16,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter,
					normal = new GUIStyleState
					{
						textColor = Color.white
					}
				};
				GUI.Label(this.closeButtonRect, "?", style);
				bool flag = GUI.Button(this.closeButtonRect, "", GUIStyle.none);
				if (flag)
				{
					SkidMenuPlugin.ShowForceRolesMenu = false;
					SkidMenuPlugin.showRoleDropdown = false;
					SkidMenuPlugin.dropdownPlayerIndex = -1;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x06000108 RID: 264 RVA: 0x0001DA20 File Offset: 0x0001BC20
			private void DrawRoleAssignments()
			{
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
				List<PlayerControl> list = (from p in PlayerControl.AllPlayerControls.ToArray()
				where p != null && p.Data != null && !p.Data.Disconnected
				select p).ToList<PlayerControl>();
				bool flag = list.Count == 0;
				if (flag)
				{
					GUI.contentColor = Color.gray;
					GUILayout.Label("No players in lobby", new GUIStyle(GUI.skin.label)
					{
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter,
						fontStyle = FontStyle.Italic
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						PlayerControl playerControl = list[i];
						int playerId = (int)playerControl.PlayerId;
						string text = playerControl.Data.PlayerName ?? "Unknown";
						Color contentColor = Palette.PlayerColors[playerControl.Data.DefaultOutfit.ColorId];
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
						GUILayout.BeginHorizontal(null);
						GUI.contentColor = contentColor;
						GUILayout.Label(text, new GUIStyle(GUI.skin.label)
						{
							fontSize = 12,
							fontStyle = FontStyle.Bold
						}, new GUILayoutOption[]
						{
							GUILayout.Width(140f)
						});
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
						GUILayout.FlexibleSpace();
						RoleTypes roleTypes = SkidMenuPlugin.forcedRoles.ContainsKey(playerId) ? SkidMenuPlugin.forcedRoles[playerId] : RoleTypes.Crewmate;
						string roleName = this.GetRoleName(roleTypes);
						bool flag2 = SkidMenuPlugin.showRoleDropdown && SkidMenuPlugin.dropdownPlayerIndex == i;
						GUI.backgroundColor = (flag2 ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.4f, 1f));
						bool flag3 = GUILayout.Button("? " + roleName, new GUILayoutOption[]
						{
							GUILayout.Height(25f),
							GUILayout.Width(150f)
						});
						if (flag3)
						{
							bool flag4 = flag2;
							if (flag4)
							{
								SkidMenuPlugin.showRoleDropdown = false;
								SkidMenuPlugin.dropdownPlayerIndex = -1;
							}
							else
							{
								SkidMenuPlugin.showRoleDropdown = true;
								SkidMenuPlugin.dropdownPlayerIndex = i;
								SkidMenuPlugin.selectedForceRolePlayerId = playerId;
							}
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.Space(5f);
						GUI.backgroundColor = new Color(0.6f, 0.1f, 0.1f, 1f);
						bool flag5 = GUILayout.Button("?", new GUILayoutOption[]
						{
							GUILayout.Width(30f),
							GUILayout.Height(25f)
						});
						if (flag5)
						{
							bool flag6 = SkidMenuPlugin.forcedRoles.ContainsKey(playerId);
							if (flag6)
							{
								SkidMenuPlugin.forcedRoles.Remove(playerId);
								SkidMenuPlugin.Logger.LogInfo("Removed forced role for " + text);
							}
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.EndHorizontal();
						bool flag7 = flag2;
						if (flag7)
						{
							GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
							this.roleDropdownScrollPosition = GUILayout.BeginScrollView(this.roleDropdownScrollPosition, new GUILayoutOption[]
							{
								GUILayout.Height(150f)
							});
							foreach (RoleTypes roleTypes2 in HostCommandPatch.RolesUI.availableRoles)
							{
								GUI.backgroundColor = ((roleTypes == roleTypes2) ? new Color(0.5f, 0.2f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
								string roleName2 = this.GetRoleName(roleTypes2);
								bool flag8 = GUILayout.Button(roleName2, new GUILayoutOption[]
								{
									GUILayout.Height(25f)
								});
								if (flag8)
								{
									SkidMenuPlugin.forcedRoles[playerId] = roleTypes2;
									SkidMenuPlugin.showRoleDropdown = false;
									SkidMenuPlugin.dropdownPlayerIndex = -1;
									SkidMenuPlugin.Logger.LogInfo("Set " + text + " to " + roleName2);
								}
								GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
							}
							GUILayout.EndScrollView();
							GUILayout.EndVertical();
						}
						GUILayout.EndVertical();
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
						GUILayout.Space(3f);
					}
				}
				GUILayout.Space(10f);
				GUI.backgroundColor = new Color(0.8f, 0.3f, 0.2f, 1f);
				bool flag9 = GUILayout.Button("CLEAR ALL ROLES", new GUILayoutOption[]
				{
					GUILayout.Height(35f)
				});
				if (flag9)
				{
					SkidMenuPlugin.forcedRoles.Clear();
					SkidMenuPlugin.Logger.LogInfo("Cleared all forced roles");
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.EndScrollView();
			}

			// Token: 0x06000109 RID: 265 RVA: 0x0001DF24 File Offset: 0x0001C124
			private string GetRoleName(RoleTypes role)
			{
				switch (role)
				{
				case RoleTypes.Crewmate:
					return "Crewmate";
				case RoleTypes.Impostor:
					return "Impostor";
				case RoleTypes.Scientist:
					return "Scientist";
				case RoleTypes.Engineer:
					return "Engineer";
				case RoleTypes.GuardianAngel:
					return "Guardian Angel";
				case RoleTypes.Shapeshifter:
					return "Shapeshifter";
				case RoleTypes.Noisemaker:
					return "Noisemaker";
				case RoleTypes.Phantom:
					return "Phantom";
				case RoleTypes.Tracker:
					return "Tracker";
				case RoleTypes.Detective:
					return "Detective";
				case RoleTypes.Viper:
					return "Viper";
				}
				return role.ToString();
			}

			// Token: 0x0600010A RID: 266 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
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
						this.windowRect.width = Mathf.Clamp(value, 400f, 800f);
						this.windowRect.height = Mathf.Clamp(value2, 400f, 800f);
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
			}

			// Token: 0x0600010B RID: 267 RVA: 0x0001E17C File Offset: 0x0001C37C
			private void DrawResizeHandle()
			{
				GUI.backgroundColor = (this.isResizing ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.8f));
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, this.isResizing ? new Color(1f, 1f, 0f, 0.5f) : new Color(0.3f, 0.3f, 0.3f, 0.5f));
				GUI.Box(this.resizeHandleRect, "", guistyle);
				GUI.contentColor = Color.white;
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				};
				GUI.Label(this.resizeHandleRect, "?", style);
				GUI.contentColor = Color.white;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x0600010C RID: 268 RVA: 0x0001E288 File Offset: 0x0001C488
			private Texture2D MakeTex(int width, int height, Color col)
			{
				Texture2D texture2D;
				bool flag = !HostCommandPatch.RolesUI._texCache.TryGetValue(col, out texture2D) || texture2D == null;
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
					HostCommandPatch.RolesUI._texCache[col] = texture2D;
				}
				return texture2D;
			}

			// Token: 0x0600010D RID: 269 RVA: 0x0001E30A File Offset: 0x0001C50A
			// Note: this type is marked as 'beforefieldinit'.
			static RolesUI()
			{
				RoleTypes[] array = new RoleTypes[11];
				RuntimeHelpers.InitializeArray(array, typeof(PrivateImplementationDetails).GetField("A3EDC3E4ACB5F9D1BD6090D46604325F77AEF5D96030C640B01766EB0C822185", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).FieldHandle);
				HostCommandPatch.RolesUI.availableRoles = array;
				HostCommandPatch.RolesUI._texCache = new Dictionary<Color, Texture2D>();
			}

			// Token: 0x0400018E RID: 398
			private Vector2 scrollPosition = Vector2.zero;

			// Token: 0x0400018F RID: 399
			private Rect windowRect = new Rect(600f, 100f, 450f, 500f);

			// Token: 0x04000190 RID: 400
			private int windowId = 667;

			// Token: 0x04000191 RID: 401
			private bool isResizing = false;

			// Token: 0x04000192 RID: 402
			private Vector2 resizeStart;

			// Token: 0x04000193 RID: 403
			private Rect resizeHandleRect;

			// Token: 0x04000194 RID: 404
			private const float MIN_WIDTH = 400f;

			// Token: 0x04000195 RID: 405
			private const float MIN_HEIGHT = 400f;

			// Token: 0x04000196 RID: 406
			private const float MAX_WIDTH = 800f;

			// Token: 0x04000197 RID: 407
			private const float MAX_HEIGHT = 800f;

			// Token: 0x04000198 RID: 408
			private const float RESIZE_HANDLE_SIZE = 15f;

			// Token: 0x04000199 RID: 409
			private Vector2 roleDropdownScrollPosition = Vector2.zero;

			// Token: 0x0400019A RID: 410
			private const float CLOSE_BUTTON_SIZE = 32f;

			// Token: 0x0400019B RID: 411
			private Rect closeButtonRect;

			// Token: 0x0400019C RID: 412
			private static readonly RoleTypes[] availableRoles;

			// Token: 0x0400019D RID: 413
			private static readonly Dictionary<Color, Texture2D> _texCache;
		}

		// Token: 0x02000053 RID: 83
		public class ForceColorUI : MonoBehaviour
		{
			// Token: 0x0600010E RID: 270 RVA: 0x0001E330 File Offset: 0x0001C530
			public ForceColorUI(IntPtr ptr) : base(ptr)
			{
			}

			// Token: 0x0600010F RID: 271 RVA: 0x0001E3A0 File Offset: 0x0001C5A0
			private void Update()
			{
				bool flag = Input.GetKeyDown(KeyCode.Escape) && SkidMenuPlugin.ShowForceColorMenu;
				if (flag)
				{
					SkidMenuPlugin.ShowForceColorMenu = false;
					SkidMenuPlugin.showColorDropdown = false;
					this.showGlobalColorDropdown = false;
				}
			}

			// Token: 0x06000110 RID: 272 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
			private void OnGUI()
			{
				bool flag = !SkidMenuPlugin.ShowForceColorMenu;
				if (!flag)
				{
					this.HandleResize();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
					this.windowRect = GUI.Window(this.windowId, this.windowRect, new Action<int>(this.DrawWindowContents), "");
					this.DrawResizeHandle();
					this.DrawCloseButton();
				}
			}

			// Token: 0x06000111 RID: 273 RVA: 0x0001E444 File Offset: 0x0001C644
			private void DrawWindowContents(int id)
			{
				GUILayout.BeginArea(new Rect(10f, 10f, this.windowRect.width - 20f, 30f));
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Label("Force Color Menu", new GUIStyle(GUI.skin.label)
				{
					fontSize = 16,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(10f, 50f, this.windowRect.width - 20f, this.windowRect.height - 90f));
				bool flag = !AmongUsClient.Instance.AmHost;
				if (flag)
				{
					GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
					GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
					GUI.contentColor = Color.yellow;
					GUILayout.Label("?? Must be Host to Force Colors", new GUIStyle(GUI.skin.label)
					{
						fontStyle = FontStyle.Bold,
						fontSize = 12,
						alignment = TextAnchor.MiddleCenter
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
					GUILayout.EndVertical();
					GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				}
				else
				{
					this.DrawColorControls();
				}
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(10f, this.windowRect.height - 55f, this.windowRect.width - 20f, 50f));
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
				GUI.contentColor = Color.cyan;
				GUILayout.Label("\ud83c\udfa8 Colors will be applied immediately", new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					alignment = TextAnchor.MiddleCenter
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : Color.gray);
				GUILayout.Label("Press ESC to close", new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontSize = 9
				}, null);
				GUI.contentColor = Color.white;
				GUILayout.EndArea();
				bool flag2 = !this.isResizing && !this.resizeHandleRect.Contains(Event.current.mousePosition) && !this.closeButtonRect.Contains(Event.current.mousePosition);
				if (flag2)
				{
					Rect position = new Rect(0f, 0f, this.windowRect.width - 32f - 10f, this.windowRect.height);
					GUI.DragWindow(position);
				}
			}

			// Token: 0x06000112 RID: 274 RVA: 0x0001E714 File Offset: 0x0001C914
			private void DrawCloseButton()
			{
				this.closeButtonRect = new Rect(this.windowRect.x + this.windowRect.width - 32f - 5f, this.windowRect.y + 5f, 32f, 32f);
				Event current = Event.current;
				Color color = this.closeButtonRect.Contains(current.mousePosition) ? new Color(0.8f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.2f, 0.2f, 0.8f);
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, color);
				GUI.backgroundColor = color;
				GUI.Box(this.closeButtonRect, "", guistyle);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 16,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter,
					normal = new GUIStyleState
					{
						textColor = Color.white
					}
				};
				GUI.Label(this.closeButtonRect, "?", style);
				bool flag = GUI.Button(this.closeButtonRect, "", GUIStyle.none);
				if (flag)
				{
					SkidMenuPlugin.ShowForceColorMenu = false;
					SkidMenuPlugin.showColorDropdown = false;
					this.showGlobalColorDropdown = false;
					SkidMenuPlugin.dropdownPlayerIndexColor = -1;
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x06000113 RID: 275 RVA: 0x0001E890 File Offset: 0x0001CA90
			private void DrawColorControls()
			{
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
				GUI.contentColor = (SkidMenuPlugin.RGBMode ? SkidMenuPlugin.GetRGBText() : new Color(0.2f, 0.7f, 0.9f));
				GUILayout.Label("Select Global Color:", new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 11
				}, null);
				GUI.contentColor = SkidMenuPlugin.GetRGBText();
				GUILayout.Space(5f);
				Color col = Palette.PlayerColors[(int)SkidMenuPlugin.selectedGlobalColor];
				string str = HostCommandPatch.ForceColorUI.colorNames[(int)SkidMenuPlugin.selectedGlobalColor];
				GUI.backgroundColor = new Color(0.3f, 0.3f, 0.4f, 1f);
				GUILayout.BeginHorizontal(null);
				GUILayout.Box("", new GUIStyle(GUI.skin.box)
				{
					normal = 
					{
						background = this.MakeTex(2, 2, col)
					}
				}, new GUILayoutOption[]
				{
					GUILayout.Width(30f),
					GUILayout.Height(30f)
				});
				GUILayout.Space(5f);
				bool flag = GUILayout.Button("? " + str, new GUILayoutOption[]
				{
					GUILayout.Height(30f)
				});
				if (flag)
				{
					this.showGlobalColorDropdown = !this.showGlobalColorDropdown;
				}
				GUILayout.EndHorizontal();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				bool flag2 = this.showGlobalColorDropdown;
				if (flag2)
				{
					GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
					this.colorDropdownScrollPosition = GUILayout.BeginScrollView(this.colorDropdownScrollPosition, new GUILayoutOption[]
					{
						GUILayout.Height(150f)
					});
					for (byte b = 0; b < 18; b += 1)
					{
						GUILayout.BeginHorizontal(null);
						bool flag3 = SkidMenuPlugin.selectedGlobalColor == b;
						Color col2 = Palette.PlayerColors[(int)b];
						GUILayout.Box("", new GUIStyle(GUI.skin.box)
						{
							normal = 
							{
								background = this.MakeTex(2, 2, col2)
							}
						}, new GUILayoutOption[]
						{
							GUILayout.Width(25f),
							GUILayout.Height(25f)
						});
						GUILayout.Space(5f);
						GUI.backgroundColor = (flag3 ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
						bool flag4 = GUILayout.Button(HostCommandPatch.ForceColorUI.colorNames[(int)b], new GUILayoutOption[]
						{
							GUILayout.Height(25f)
						});
						if (flag4)
						{
							SkidMenuPlugin.selectedGlobalColor = b;
							this.showGlobalColorDropdown = false;
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
					GUILayout.EndVertical();
				}
				GUILayout.Space(10f);
				GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
				bool flag5 = GUILayout.Button("\ud83c\udfa8 FORCE COLOR TO ALL", new GUILayoutOption[]
				{
					GUILayout.Height(40f)
				});
				if (flag5)
				{
					this.ForceColorToAll(SkidMenuPlugin.selectedGlobalColor);
				}
				GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
				GUILayout.EndVertical();
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
				GUILayout.Space(10f);
				this.DrawPlayerColorAssignments();
			}

			// Token: 0x06000114 RID: 276 RVA: 0x0001EC20 File Offset: 0x0001CE20
			private void DrawPlayerColorAssignments()
			{
				this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, Array.Empty<GUILayoutOption>());
				List<PlayerControl> list = (from p in PlayerControl.AllPlayerControls.ToArray()
				where p != null && p.Data != null && !p.Data.Disconnected
				select p).ToList<PlayerControl>();
				bool flag = list.Count == 0;
				if (flag)
				{
					GUI.contentColor = Color.gray;
					GUILayout.Label("No players in lobby", new GUIStyle(GUI.skin.label)
					{
						fontSize = 11,
						alignment = TextAnchor.MiddleCenter,
						fontStyle = FontStyle.Italic
					}, null);
					GUI.contentColor = SkidMenuPlugin.GetRGBText();
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						PlayerControl playerControl = list[i];
						int playerId = (int)playerControl.PlayerId;
						string text = playerControl.Data.PlayerName ?? "Unknown";
						Color contentColor = Palette.PlayerColors[playerControl.Data.DefaultOutfit.ColorId];
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
						GUILayout.BeginHorizontal(null);
						GUI.contentColor = contentColor;
						GUILayout.Label(text, new GUIStyle(GUI.skin.label)
						{
							fontSize = 12,
							fontStyle = FontStyle.Bold
						}, new GUILayoutOption[]
						{
							GUILayout.Width(140f)
						});
						GUI.contentColor = SkidMenuPlugin.GetRGBText();
						GUILayout.FlexibleSpace();
						byte b = SkidMenuPlugin.forcedColors.ContainsKey(playerId) ? SkidMenuPlugin.forcedColors[playerId] : ((byte)playerControl.Data.DefaultOutfit.ColorId);
						Color col = Palette.PlayerColors[(int)b];
						string str = HostCommandPatch.ForceColorUI.colorNames[(int)b];
						bool flag2 = SkidMenuPlugin.showColorDropdown && SkidMenuPlugin.dropdownPlayerIndexColor == i;
						GUI.backgroundColor = (flag2 ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.4f, 1f));
						GUILayout.BeginHorizontal(null);
						GUILayout.Box("", new GUIStyle(GUI.skin.box)
						{
							normal = 
							{
								background = this.MakeTex(2, 2, col)
							}
						}, new GUILayoutOption[]
						{
							GUILayout.Width(25f),
							GUILayout.Height(25f)
						});
						GUILayout.Space(3f);
						bool flag3 = GUILayout.Button("? " + str, new GUILayoutOption[]
						{
							GUILayout.Height(25f),
							GUILayout.Width(120f)
						});
						if (flag3)
						{
							bool flag4 = flag2;
							if (flag4)
							{
								SkidMenuPlugin.showColorDropdown = false;
								SkidMenuPlugin.dropdownPlayerIndexColor = -1;
							}
							else
							{
								SkidMenuPlugin.showColorDropdown = true;
								SkidMenuPlugin.dropdownPlayerIndexColor = i;
								SkidMenuPlugin.selectedForceColorId = playerId;
							}
						}
						GUILayout.EndHorizontal();
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.Space(5f);
						GUI.backgroundColor = new Color(0.6f, 0.1f, 0.1f, 1f);
						bool flag5 = GUILayout.Button("?", new GUILayoutOption[]
						{
							GUILayout.Width(30f),
							GUILayout.Height(25f)
						});
						if (flag5)
						{
							bool flag6 = SkidMenuPlugin.forcedColors.ContainsKey(playerId);
							if (flag6)
							{
								SkidMenuPlugin.forcedColors.Remove(playerId);
								SkidMenuPlugin.Logger.LogInfo("Removed forced color for " + text);
							}
						}
						GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
						GUILayout.EndHorizontal();
						bool flag7 = flag2;
						if (flag7)
						{
							GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
							bool flag8 = !this.playerColorScrollPositions.ContainsKey(i);
							if (flag8)
							{
								this.playerColorScrollPositions[i] = Vector2.zero;
							}
							this.playerColorScrollPositions[i] = GUILayout.BeginScrollView(this.playerColorScrollPositions[i], new GUILayoutOption[]
							{
								GUILayout.Height(150f)
							});
							for (byte b2 = 0; b2 < 18; b2 += 1)
							{
								GUILayout.BeginHorizontal(null);
								bool flag9 = b == b2;
								Color col2 = Palette.PlayerColors[(int)b2];
								GUILayout.Box("", new GUIStyle(GUI.skin.box)
								{
									normal = 
									{
										background = this.MakeTex(2, 2, col2)
									}
								}, new GUILayoutOption[]
								{
									GUILayout.Width(25f),
									GUILayout.Height(25f)
								});
								GUILayout.Space(5f);
								GUI.backgroundColor = (flag9 ? new Color(0.2f, 0.7f, 0.9f, 1f) : new Color(0.3f, 0.3f, 0.3f, 1f));
								bool flag10 = GUILayout.Button(HostCommandPatch.ForceColorUI.colorNames[(int)b2], new GUILayoutOption[]
								{
									GUILayout.Height(25f)
								});
								if (flag10)
								{
									SkidMenuPlugin.forcedColors[playerId] = b2;
									playerControl.RpcSetColor(b2);
									SkidMenuPlugin.showColorDropdown = false;
									SkidMenuPlugin.dropdownPlayerIndexColor = -1;
									SkidMenuPlugin.Logger.LogInfo("Set " + text + " to " + HostCommandPatch.ForceColorUI.colorNames[(int)b2]);
								}
								GUI.backgroundColor = SkidMenuPlugin.GetRGBAccent();
								GUILayout.EndHorizontal();
							}
							GUILayout.EndScrollView();
							GUILayout.EndVertical();
						}
						GUILayout.EndVertical();
						GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
						GUILayout.Space(3f);
					}
				}
				GUILayout.EndScrollView();
			}

			// Token: 0x06000115 RID: 277 RVA: 0x0001F1F8 File Offset: 0x0001D3F8
			private void ForceColorToAll(byte colorId)
			{
				try
				{
					IEnumerable<PlayerControl> enumerable = from p in PlayerControl.AllPlayerControls.ToArray()
					where p != null && p.Data != null && !p.Data.Disconnected
					select p;
					int num = 0;
					foreach (PlayerControl playerControl in enumerable)
					{
						playerControl.RpcSetColor(colorId);
						SkidMenuPlugin.forcedColors[(int)playerControl.PlayerId] = colorId;
						num++;
					}
					SkidMenuPlugin.Logger.LogInfo(string.Concat(new string[]
					{
						"Forced ",
						HostCommandPatch.ForceColorUI.colorNames[(int)colorId],
						" to ",
						num.ToString(),
						" players"
					}));
				}
				catch (Exception ex)
				{
					SkidMenuPlugin.Logger.LogError("ForceColorToAll error: " + ex.Message);
				}
			}

			// Token: 0x06000116 RID: 278 RVA: 0x0001F304 File Offset: 0x0001D504
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
						this.windowRect.width = Mathf.Clamp(value, 400f, 800f);
						this.windowRect.height = Mathf.Clamp(value2, 400f, 800f);
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
			}

			// Token: 0x06000117 RID: 279 RVA: 0x0001F48C File Offset: 0x0001D68C
			private void DrawResizeHandle()
			{
				GUI.backgroundColor = (this.isResizing ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.8f));
				GUIStyle guistyle = new GUIStyle(GUI.skin.box);
				guistyle.normal.background = this.MakeTex(2, 2, this.isResizing ? new Color(1f, 1f, 0f, 0.5f) : new Color(0.3f, 0.3f, 0.3f, 0.5f));
				GUI.Box(this.resizeHandleRect, "", guistyle);
				GUI.contentColor = Color.white;
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				};
				GUI.Label(this.resizeHandleRect, "?", style);
				GUI.contentColor = Color.white;
				GUI.backgroundColor = SkidMenuPlugin.GetRGBColor();
			}

			// Token: 0x06000118 RID: 280 RVA: 0x0001F598 File Offset: 0x0001D798
			private Texture2D MakeTex(int width, int height, Color col)
			{
				Texture2D texture2D;
				bool flag = !HostCommandPatch.ForceColorUI._texCache.TryGetValue(col, out texture2D) || texture2D == null;
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
					HostCommandPatch.ForceColorUI._texCache[col] = texture2D;
				}
				return texture2D;
			}

			// Token: 0x0400019E RID: 414
			private Vector2 scrollPosition = Vector2.zero;

			// Token: 0x0400019F RID: 415
			private Rect windowRect = new Rect(600f, 100f, 450f, 550f);

			// Token: 0x040001A0 RID: 416
			private int windowId = 668;

			// Token: 0x040001A1 RID: 417
			private bool isResizing = false;

			// Token: 0x040001A2 RID: 418
			private Vector2 resizeStart;

			// Token: 0x040001A3 RID: 419
			private Rect resizeHandleRect;

			// Token: 0x040001A4 RID: 420
			private const float MIN_WIDTH = 400f;

			// Token: 0x040001A5 RID: 421
			private const float MIN_HEIGHT = 400f;

			// Token: 0x040001A6 RID: 422
			private const float MAX_WIDTH = 800f;

			// Token: 0x040001A7 RID: 423
			private const float MAX_HEIGHT = 800f;

			// Token: 0x040001A8 RID: 424
			private const float RESIZE_HANDLE_SIZE = 15f;

			// Token: 0x040001A9 RID: 425
			private Vector2 colorDropdownScrollPosition = Vector2.zero;

			// Token: 0x040001AA RID: 426
			private bool showGlobalColorDropdown = false;

			// Token: 0x040001AB RID: 427
			private Dictionary<int, Vector2> playerColorScrollPositions = new Dictionary<int, Vector2>();

			// Token: 0x040001AC RID: 428
			private const float CLOSE_BUTTON_SIZE = 32f;

			// Token: 0x040001AD RID: 429
			private Rect closeButtonRect;

			// Token: 0x040001AE RID: 430
			private static readonly string[] colorNames = new string[]
			{
				"Red",
				"Blue",
				"Green",
				"Pink",
				"Orange",
				"Yellow",
				"Black",
				"White",
				"Purple",
				"Brown",
				"Cyan",
				"Lime",
				"Maroon",
				"Rose",
				"Banana",
				"Gray",
				"Tan",
				"Coral"
			};

			// Token: 0x040001AF RID: 431
			private static readonly Dictionary<Color, Texture2D> _texCache = new Dictionary<Color, Texture2D>();
		}
	}
}
