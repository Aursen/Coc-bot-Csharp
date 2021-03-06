﻿namespace CoC.Bot.Functions
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows;

	using Tools;
	using Tools.FastFind;
	using ViewModels;

	/// <summary>
	/// The Main entry point for the Bot Functions.
	/// </summary>
	public class Main
	{
		/// <summary>
		/// Initializes the Bot.
		/// </summary>
		public static void Initialize(MainViewModel vm)
		{
			// Store in properties so we can access in the SubFunctions
			Bot = vm;

			Bot.Output = string.Format(Properties.Resources.OutputWelcomeMessage, Properties.Resources.AppName);
			Bot.Output = Properties.Resources.OutputBotIsStarting;

			// Check if BlueStack is running
			FastFindWrapper.SetHWnd(BlueStackHelper.GetBlueStackWindowHandle(), true);
			if (!BlueStackHelper.IsBlueStackRunning)
			{
				Bot.Output = Properties.Resources.OutputBSNotFound;

				Bot.IsExecuting = false;
				return;
			}

			if (!BlueStackHelper.IsRunningWithRequiredDimensions)
			{
				Bot.Output = Properties.Resources.OutputBSNotRunningWithDimensions;
				Bot.Output = Properties.Resources.OutputBSApplyDimensionsIntoRegistry;

				if (!BlueStackHelper.SetDimensionsIntoRegistry())
				{
					// Woops! Something went wrong, log the error!
					Bot.Output = Properties.Resources.OutputBSApplyDimensionsError;

					Bot.IsExecuting = false;
					return;
				}
				else
					Bot.Output = Properties.Resources.OutputBSAppliedDimensionsIntoRegistry;

				// Restart BlueStack
				// Wait until restart and continue...

				BlueStackHelper.ActivateBlueStack();
			}

			CreateDirectory(GlobalVariables.LogPath);
			CreateDirectory(GlobalVariables.ScreenshotZombieAttacked);
			CreateDirectory(GlobalVariables.ScreenshotZombieSkipped);

			WriteLicense();

			// Run Everything related to the bot in the background
			var thread = new Thread(() =>
			{
				while (Bot.IsExecuting)
				{
					Thread.Sleep(1000);
                    //Bot.Output = "Loop test...";
                    //Bot.Output = "Minimum Gold is: " + Bot.MinimumGold; // Changing values on the fly works as expected
                    //Bot.Output = "Try changing values on the fly.";

					MainScreen.CheckMainScreen();
                    Thread.Sleep(1000);

					MainScreen.ZoomOut();
                    Thread.Sleep(1000);

					Village.TrainTroops();
                    Thread.Sleep(1000);

					Village.RequestTroops();
                    Thread.Sleep(1000);

					//SubFunctions.Village.Collect();

					//Idle();

					//SubFunctions.Attack.AttackMain();
				};
			})
			{
				IsBackground = true
			};
			thread.Start();
		}

		#region Properties

		/// <summary>
		/// Gets the MainViewModel.
		/// </summary>
		/// <value>The MainViewModel.</value>
		internal static MainViewModel Bot { get; private set; }

		#endregion

		#region Private Methods

		/// <summary>
		/// Creates the required directories.
		/// </summary>
		/// <param name="path">The path.</param>
		private static void CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Writes the License.
		/// </summary>
		private static void WriteLicense()
		{
			if (!File.Exists(Path.Combine(GlobalVariables.AppPath, "LICENSE")))
			{
				try
				{
					File.WriteAllText(Path.Combine(GlobalVariables.AppPath, "LICENSE"), Properties.Resources.LICENSE, Encoding.UTF8);
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		#endregion
	}
}
