using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pringles
{
	public interface IEnableGameCenter : IBuildParameter
	{
		bool EnableGameCenter { get; }
	}
	public interface IEnablePushNotification : IBuildParameter
	{
		bool EnablePushNotification { get; }
	}
	public interface IEnableRemoteNotification : IBuildParameter
	{ 
		bool EnableRemoteNotification { get; }
	}
	public class iOSSystemCapability : IEnablePushNotification, IEnableGameCenter, IEnableRemoteNotification
	{
		public bool EnableGameCenter { get; }

		public bool EnablePushNotification { get; }

		public bool EnableRemoteNotification { get; }

		public iOSSystemCapability( 
			bool enablePushNotification, 
			bool enableGameCenter,
			bool enableRemoteNotification
		)
		{
			EnableGameCenter = enableGameCenter;
			EnablePushNotification = enablePushNotification;
			EnableRemoteNotification = enableRemoteNotification;
		}
	}

	public interface IEnableBitCode : IBuildParameter
	{
		bool EnableBitCode { get; }
	}

	public sealed class BitCodeParameter : IEnableBitCode
	{
		public bool EnableBitCode { get; }
		public BitCodeParameter(bool enable) { EnableBitCode = enable; }
	}
}