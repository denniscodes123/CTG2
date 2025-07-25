using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;

namespace CTG2.Content.Items
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class ArcherDash : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<ArcherDashPlayer>().DashAccessoryEquipped = true;
		}
	}

	public class ArcherDashPlayer : ModPlayer {
		public const int DashCooldown = 150; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
		public const int DashDuration = 18; // Duration of the dash afterimage effect in frames

		public float DashVelocity = 14f; // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph

		public bool dashKeybindActive = false; // Uses the hook keybind as the dash keybind

		// The fields related to the dash accessory
		public bool DashAccessoryEquipped;
		private int lastDashDelay = 0;
		public int DashDelay = 0; // frames remaining till we can dash again
		public int DashTimer = 0; // frames remaining in the dash

		public bool recentlyEnded = false;


		public override void ResetEffects() {
			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
			DashAccessoryEquipped = false;

			if (recentlyEnded && DashTimer == 0) {
				Vector2 newVelocity = Player.velocity;
				if (newVelocity != Vector2.Zero) {
					newVelocity.Normalize();
					newVelocity *= 4f;
					Player.gravity = 0.4f;
					Player.velocity = newVelocity;
				}
				recentlyEnded = false;
				DashVelocity = 14f;
			}


			dashKeybindActive = CTG2.ArcherDashKeybind.JustPressed;

			if (DashDelay == 0 && lastDashDelay != 0) SoundEngine.PlaySound(SoundID.Item35);

			lastDashDelay = DashDelay;
		}


		public override void PreUpdateMovement() {

			Vector2 newVelocity = Player.velocity;

			if (dashKeybindActive && DashDelay == 0 && DashAccessoryEquipped) {

				// Get the player's position
        		Vector2 playerPosition = Main.player[Main.myPlayer].Center;

				// Get the mouse cursor position
				Vector2 cursorPosition = Main.MouseWorld;

				// Find the vector from the player to the cursor
				Vector2 directionToCursor = cursorPosition - playerPosition;

				// Normalize the vector
				if (directionToCursor.Length() > 0 && Player.velocity.Length() < DashVelocity) {
					directionToCursor.Normalize();
					newVelocity = directionToCursor * DashVelocity;
					Player.gravity = 0f;
					recentlyEnded = true;
				}
				else return;

				// Start our dash
				DashDelay = DashCooldown;
				DashTimer = DashDuration;
				Player.velocity = newVelocity;
			}

			if (DashDelay > 0) DashDelay--;

			Player.eocDash = DashTimer;

			if (DashTimer > 0) // If dash is active
			{
				if (DashTimer < 10) {
					DashVelocity -= 1f;
					if (Player.velocity != Vector2.Zero) {
						Vector2 decVelocity = Player.velocity;
						decVelocity.Normalize();
						decVelocity *= DashVelocity;
						Player.velocity = decVelocity;
					}
				}

				// Afterimage effect
				Player.armorEffectDrawShadowEOCShield = true;
				DashTimer--;
			}
			else
				Player.armorEffectDrawShadowEOCShield = false;
		}
	}
}
