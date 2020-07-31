using Clansty.tianlang.Events;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Native.Core
{
	/// <summary>
	/// 酷Q应用主入口类
	/// </summary>
	public class CQMain
	{
		/// <summary>
		/// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
		/// </summary>
		/// <param name="container">用于注册的 IOC 容器 </param>
		public static void Register (IUnityContainer unityContainer)
		{
			unityContainer.RegisterType<IAppEnable, EnableEvent>("应用已被启用");
			unityContainer.RegisterType<IFriendAddRequest, FriendRequestEvent>("好友添加请求处理");
			unityContainer.RegisterType<IGroupMemberIncrease, GroupAddMemberEvent>("群成员增加事件处理");
			unityContainer.RegisterType<IGroupMessage, GroupMsgEvent>("群消息处理");
			unityContainer.RegisterType<IGroupAddRequest, JoinGroupRequestEvent>("群添加请求处理");
			unityContainer.RegisterType<IPrivateMessage, PrivateMsgEvent>("私聊消息处理");
			unityContainer.RegisterType<ICQStartup, StartupEvent>("酷Q启动事件");
		}
	}
}
