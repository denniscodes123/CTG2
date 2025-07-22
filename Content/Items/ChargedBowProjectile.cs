using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using System;
using CTG2.Content.Items;


public class ChargedBowProjectile : ModProjectile
{
	public override string Texture => "Terraria/Images/Projectile_294";
	private float Rotation, c1, td, framesPulledBack, pullback, charge;
	private int damage, t, t2, special;
	private Color color = new (0, 0, 0);
	private SpriteEffects sprite;
	private bool load = false;
	private Item item;
	private bool start;
	private SlotId sound;


	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.timeLeft = 21;
		Projectile.tileCollide = false;
		Projectile.aiStyle = 75;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.ownerHitCheck = true;
		Projectile.penetrate = -1;
	}

	
	public override bool PreDraw(ref Color lightColor) // graphics
	{
		if (load == false) // loads projectile on first iteration
		{
			Main.instance.LoadProjectile((int)Projectile.ai[1]);
			load = true;
		}

		Player player = Main.player[Projectile.owner];

		if (player.channel)
		{
			if (player.direction == 1)
			{
				Rotation = Vector2.Normalize(Main.MouseWorld - player.MountedCenter).ToRotation();
				sprite = SpriteEffects.None;
			}
			else // flips projectile other directon if facing other direction
			{
				Rotation = Vector2.Normalize(player.MountedCenter - Main.MouseWorld).ToRotation() + MathHelper.Pi;
				sprite = SpriteEffects.FlipVertically;
			}
		}

		if (charge < 40f) color = lightColor;
		else // projectile blinking rate and logic when fully charged
		{
			td++;
			Color highlight = Color.White; // 255, 255, 255
			Color lowlight = new(150, 150, 150);
			if (td <= 200)
			{
				color = Color.Lerp(color, highlight, 0.2f);
			}
			else
			{
				color = Color.Lerp(color, lowlight, 0.2f);
			}
			if (td > 400) td = 0;
		}
		
		// bow
		Texture2D texture = TextureAssets.Item[(int)Projectile.ai[0]].Value;
		Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
		Vector2 origin = rectangle.Size() / 2f;
		Vector2 position = player.MountedCenter + Vector2.One.RotatedBy(Rotation - MathHelper.PiOver4) * 9f; // rotation based on cursor position
		Main.EntitySpriteDraw(texture, position - Main.screenPosition, rectangle, lightColor, Rotation, origin, Projectile.scale, sprite); // draw bow sprite

		// arrow
		Texture2D arrowTexture = TextureAssets.Projectile[(int)Projectile.ai[1]].Value;
		Rectangle arrowRectangle = new(0, 0, arrowTexture.Width, arrowTexture.Height);
		Vector2 origin3 = arrowRectangle.Size() / 2f;
		Vector2 arrowPos = player.MountedCenter + Vector2.One.RotatedBy(Rotation - MathHelper.PiOver4) * 16f - new Vector2(pullback, 0).RotatedBy(Rotation); // rotation based on cursor position: second vector is drawback

		int drawCharge = (int)(charge);

		if (player.channel && drawCharge < 40) // arrow isn't fully pulled back
		{
			framesPulledBack++;

			if (framesPulledBack % 8 == 0 && (!Main.autoPause || (Main.autoPause && !Main.playerInventory))) // pullback animation changes every 8 frames
				pullback = (float) 13.0 * Math.Min(framesPulledBack, 1300) / 1300; // formula to calculate how far to pull back the arrow
		}
		else
			framesPulledBack = 0; // reset pullback counter after releasing
	
		Main.EntitySpriteDraw(arrowTexture, arrowPos - Main.screenPosition, arrowRectangle, color, Rotation + MathHelper.PiOver2,
				origin3, Projectile.scale, SpriteEffects.None); // draw arrow sprite

		return false;
	}


	public override void AI() // behavior
	{
		Player player = Main.player[Projectile.owner];


		Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter); // sets projectile direction
		Projectile.position = player.MountedCenter + Vector2.One.RotatedBy(Rotation - MathHelper.PiOver4) * 10f; // locks projectile's position to player center

		item = player.HeldItem;

		Projectile.knockBack = item.knockBack; // knockback stays the same as base item

		Vector2 position = player.Center + Vector2.One.RotatedBy(Rotation - MathHelper.PiOver4) * 9f; // spawns the arrow from aiming direction, not inside player
		Vector2 speed = new Vector2(item.shootSpeed, 0).RotatedBy(Rotation) * (1.2f + (charge / 40) * 1.5f); // calculates the direction and speed of the arrow based on charge: max 160% velocity (3)

		if (player.channel & charge < 40f) // if still charging
		{
			charge += (float)0.667; // how fast charge grows each frame: max 40

			if (!start) //plays bow draw sound at the beginning of charging
			{
				SoundStyle bowSound = new SoundStyle("CTG2/Content/Items/BowSound");
				sound = SoundEngine.PlaySound(bowSound.WithVolumeScale(Main.soundVolume * 0.6f));

				start = true;
			}

			if (charge >= 40f)
			{
				if (SoundEngine.TryGetActiveSound(sound, out var s)) // stops charging sound from playing after fully charged
					s.Stop();

				if (c1 == 0) // plays max mana sound after fully charged
				{
					c1 = 1;
					SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
				}
				charge = 40f; // caps charge at 40
			}
		}

		else if (!player.channel && t2 == 0) // if release bow
		{
			if (SoundEngine.TryGetActiveSound(sound, out var s)) // stops any sound from playing after fully charged
				s.Stop();

			damage = (int) (item.damage * (0.5 + charge / 80)); // damage calculation for bow depending on charge

			t2 = 1;
			if (charge >= 40f && Projectile.ai[1] == ProjectileID.WoodenArrowFriendly) special = (int)Projectile.ai[0];

			if (Main.myPlayer == Projectile.owner)
				{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed, (int)Projectile.ai[1], damage, item.knockBack, Projectile.owner, 0, special); // generates projectile
				}
			SoundEngine.PlaySound(SoundID.Item5, Projectile.position); // sound played when fired
		}

		if (t2 == 1)
		{
			player.direction = player.oldDirection;
			t++;
			if (t >= item.useTime / 2) Projectile.Kill(); // kills bow projectile after half the weapon's usetime has passed
		}
	}
}
